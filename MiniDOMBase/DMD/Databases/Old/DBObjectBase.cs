using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using minidom;
using DMD;
using DMD.XML;
using DMD.Databases;
using static minidom.Sistema;


namespace minidom
{


    /* TODO ERROR: Skipped RegionDirectiveTrivia */

   

    [Serializable]
    public class DMDObject 
        : ISupportInitializeFrom
    {
        public class ObjectInfo : IComparable
        {
            public Type mType;
            public string TypeName;
            public int NewCount;
            public int FinCount;
            public int TotaleMemory;
            public DateTime? LastNew;
            public DateTime? LastFin;

            public ObjectInfo(object obj)
            {
                var tn = obj.GetType();
                mType = tn;
                TypeName = tn.FullName;
                NewCount = 0;
                FinCount = 0;
                LastNew = default;
                LastFin = default;
                TotaleMemory = 0;
            }

            public int CompareTo(object obj)
            {
                return string.Compare(TypeName, ((ObjectInfo)obj).TypeName, false);
            }

            public void IncNew(object obj)
            {
                LastNew = DMD.DateUtils.Now();
                NewCount += 1;
                TotaleMemory += CalcSize(obj);
            }

            private int CalcSize(object obj)
            {
                try
                {
                    if (obj is string)
                    {
                        return 2 * DMD.Strings.CStr(obj).Length;
                    }
                    else if (obj is object)
                    {
                        // Dim fields As System.Reflection.FieldInfo() = DMDObject.GetAllFields(obj.GetType)
                        // Dim ret As Integer = 0
                        // For Each f As System.Reflection.FieldInfo In fields
                        // If (
                        // Not Sistema.TestFlag(f.Attributes, Reflection.FieldAttributes.Static) AndAlso
                        // Not Sistema.TestFlag(f.Attributes, Reflection.FieldAttributes.InitOnly) AndAlso
                        // Not Sistema.TestFlag(f.Attributes, Reflection.FieldAttributes.Literal)
                        // ) Then
                        // Dim v As Object = f.GetValue(obj)
                        // ret += Me.CalcSize(v)
                        // End If
                        // Next
                        // Return ret

                        return 4;
                    }
                    else
                    {
                        return System.Runtime.InteropServices.Marshal.SizeOf(obj);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("DBObjectBase.CalcSize Cannot get object site " + ex.Message);
                    return 4;
                }
            }

            public void IncFin(object obj)
            {
                LastFin = DMD.DateUtils.Now();
                FinCount += 1;
                TotaleMemory -= CalcSize(obj);
            }

            public int Count()
            {
                return NewCount - FinCount;
            }
        }

        private static ObjectInfo[] ObjectCounters = DMD.Arrays.Empty<ObjectInfo>();

        private static int Compare(ObjectInfo a, ObjectInfo b)
        {
            return a.CompareTo(b);
        }

        private static int GetInsertPosition(ObjectInfo[] items, ObjectInfo item, int fromIndex, int arrayLen)
        {
            if (arrayLen == 0)
                return fromIndex;
            int p;
            p = Compare(item, items[fromIndex]);
            if (p < 0)
                return fromIndex;
            p = Compare(item, items[fromIndex + arrayLen - 1]);
            if (p >= 0)
                return fromIndex + arrayLen;
            int m = (int)Math.Floor(arrayLen / 2d);
            p = Compare(item, items[fromIndex + m]);
            if (p < 0)
            {
                return GetInsertPosition(items, item, fromIndex, m);
            }
            else if (p > 0)
            {
                return GetInsertPosition(items, item, fromIndex + m + 1, arrayLen - m - 1);
            }
            else
            {
                return m;
            }
            // For i As Integer = fromIndex To fromIndex + arrayLen - 1
            // If Compare(items(i), item, comparer) >= 0 Then Return i
            // Next
            // Return fromIndex + arrayLen
        }

        private static ObjectInfo[] Insert(ObjectInfo[] items, int fromIndex, int arrayLen, ObjectInfo item, int insertIndex)
        {
            int i;
            if (items is null)
            {
                items = new ObjectInfo[1];
            }
            else
            {
                Array.Resize(ref items, 1 + DMD.Arrays.UBound(items) + 1);
            }

            var loopTo = insertIndex + 1;
            for (i = fromIndex + arrayLen; i >= loopTo; i -= 1)
                items[i] = items[i - 1];
            items[insertIndex] = item;
            return items;
        }

        public static bool EnableTracking { get; set; } = false;

        public static void IncreaseCounter(object obj)
        {
            lock (ObjectCounters)
            {
                if (EnableTracking == false)
                    return;
                var o = new ObjectInfo(obj);
                int i = Array.BinarySearch(ObjectCounters, o);
                if (i < 0)
                {
                    i = GetInsertPosition(ObjectCounters, o, 0, ObjectCounters.Length);
                    ObjectCounters = Insert(ObjectCounters, 0, ObjectCounters.Length, o, i);
                }

                {
                    var withBlock = ObjectCounters[i];
                    withBlock.IncNew(obj);
                    // .TotaleMemory += GuessMemorySize(obj)
                }
            }
        }

        public static void DecreaseCounter(object obj)
        {
            lock (ObjectCounters)
            {
                if (EnableTracking == false)
                    return;
                var o = new ObjectInfo(obj);
                int i = Array.BinarySearch(ObjectCounters, o);
                if (i >= 0)
                {
                    {
                        var withBlock = ObjectCounters[i];
                        withBlock.IncFin(obj);
                        // .TotaleMemory -= GuessMemorySize(obj)
                    }
                }
                else
                {
                    i = GetInsertPosition(ObjectCounters, o, 0, ObjectCounters.Length);
                    ObjectCounters = Insert(ObjectCounters, 0, ObjectCounters.Length, o, i);
                    {
                        var withBlock1 = ObjectCounters[i];
                        withBlock1.IncNew(obj);
                        withBlock1.IncFin(obj);
                    }
                }
            }
        }

        // Private Shared Function GuessMemorySize(ByVal obj As Object) As Integer
        // Dim m() As System.Runtime.
        // End Function

        public static ObjectInfo[] GetCounters()
        {
            lock (ObjectCounters)
                return (ObjectInfo[])ObjectCounters.Clone();
        }

        public DMDObject()
        {
            IncreaseCounter(this);
        }

        ~DMDObject()
        {
            DecreaseCounter(this);
        }

        /// <summary>
    /// Copia tutti i valori delle proprietà
    /// </summary>
    /// <param name="value"></param>
    /// <remarks></remarks>
        public virtual void InitializeFrom(object value)
        {
            var f1 = GetAllFields(GetType());
            foreach (System.Reflection.FieldInfo f in f1)
            {
                if (!f.IsInitOnly)
                {
                    f.SetValue(this, f.GetValue(value));
                }
            }
        }

        /// <summary>
    /// Copia tutti i valori delle proprietà
    /// </summary>
    /// <param name="value"></param>
    /// <remarks></remarks>
        public virtual void CopyFrom(object value)
        {
            var f1 = GetAllFields(GetType());
            foreach (System.Reflection.FieldInfo f in f1)
            {
                if (!f.IsInitOnly)
                {
                    f.SetValue(this, f.GetValue(value));
                }
            }
        }

        private class CTypeInfo : IComparable
        {
            public Type t;
            public string DeclaringTypeName;
            public System.Reflection.FieldInfo[] arr;

            public CTypeInfo()
            {
                DeclaringTypeName = DMD.Strings.vbNullString;
                arr = DMD.Arrays.Empty<System.Reflection.FieldInfo>();
                t = null;
            }

            public CTypeInfo(Type t)
            {
                DeclaringTypeName = t.FullName;
                arr = DMD.Arrays.Empty<System.Reflection.FieldInfo>();
                this.t = t;
            }

            public void Init()
            {
                var comparer = new TComparer();
                var t1 = t;
                while (t1 is object)
                {
                    var tmp = t1.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.DeclaredOnly);
                    arr = Merge(arr, 0, arr.Length, tmp, 0, tmp.Length, comparer);
                    t1 = t1.BaseType;
                }
            }

            public int CompareTo(object obj)
            {
                CTypeInfo o = (CTypeInfo)obj;
                return string.Compare(DeclaringTypeName, o.DeclaringTypeName, false);
            }
        }

        private static CTypeInfo[] mTypesBuff = DMD.Arrays.Empty<CTypeInfo>();

        /// <summary>
    /// Restituisce tutti i campi definiti per il tipo e per le sue superclassi
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <remarks></remarks>
        public static System.Reflection.FieldInfo[] GetAllFields(Type t)
        {
            lock (mTypesBuff)
            {
                if (t is null)
                    throw new ArgumentNullException("t");
                var o = new CTypeInfo(t);
                int i = Array.BinarySearch(mTypesBuff, o);
                if (i < 0)
                {
                    i = GetInsertPosition(mTypesBuff, o, 0, mTypesBuff.Length);
                    o.Init();
                    mTypesBuff = Insert(mTypesBuff, 0, mTypesBuff.Length, o, i);
                }

                return mTypesBuff[i].arr;
            }
        }

        private class TComparer : IComparer
        {
            public TComparer()
            {
                IncreaseCounter(this);
            }

            ~TComparer()
            {
                DecreaseCounter(this);
            }

            public int Compare(object x, object y)
            {
                System.Reflection.FieldInfo f1 = (System.Reflection.FieldInfo)x;
                System.Reflection.FieldInfo f2 = (System.Reflection.FieldInfo)y;
                return string.Compare(f1.DeclaringType.FullName + "." + f1.Name, f2.DeclaringType.FullName + "." + f2.Name, false);
            }
        }

        private static int GetInsertPosition(CTypeInfo[] items, CTypeInfo item, int fromIndex, int arrayLen)
        {
            if (arrayLen == 0)
                return fromIndex;
            int p;
            p = item.CompareTo(items[fromIndex]);
            if (p < 0)
                return fromIndex;
            p = item.CompareTo(items[fromIndex + arrayLen - 1]);
            if (p >= 0)
                return fromIndex + arrayLen;
            int m = (int)Math.Floor(arrayLen / 2d);
            p = item.CompareTo(items[fromIndex + m]);
            if (p < 0)
            {
                return GetInsertPosition(items, item, fromIndex, m);
            }
            else if (p > 0)
            {
                return GetInsertPosition(items, item, fromIndex + m + 1, arrayLen - m - 1);
            }
            else
            {
                return m;
            }
        }

        private static CTypeInfo[] Insert(CTypeInfo[] items, int fromIndex, int arrayLen, CTypeInfo item, int insertIndex)
        {
            int i;
            if (items is null)
            {
                items = new CTypeInfo[1];
            }
            else
            {
                Array.Resize(ref items, 1 + DMD.Arrays.UBound(items) + 1);
            }

            var loopTo = insertIndex + 1;
            for (i = fromIndex + arrayLen; i >= loopTo; i -= 1)
                items[i] = items[i - 1];
            items[insertIndex] = item;
            return items;
        }

        private static T[] Merge<T>(T[] arr1, int s1, int l1, T[] arr2, int s2, int l2, IComparer comparer)
        {
            T[] arr = null;
            int i, i1, i2;
            arr = new T[1];
            if (l1 + l2 > 0)
            {
                arr = new T[(l1 + l2)];
                i = 0;
                i1 = 0;
                i2 = 0;
                while (i1 < l1 & i2 < l2)
                {
                    if (comparer.Compare(arr1[s1 + i1], arr2[s2 + i2]) >= 0)
                    {
                        arr[i] = arr1[s1 + i1];
                        i1 = i1 + 1;
                    }
                    else
                    {
                        arr[i] = arr2[s2 + i2];
                        i2 = i2 + 1;
                    }

                    i = i + 1;
                }

                while (i1 < l1)
                {
                    arr[i] = arr1[s1 + i1];
                    i = i + 1;
                    i1 = i1 + 1;
                }

                while (i2 < l2)
                {
                    arr[i] = arr2[s2 + i2];
                    i = i + 1;
                    i2 = i2 + 1;
                }
            }
            else
            {
                arr = DMD.Arrays.Empty<T>();
            }

            return arr;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    public partial class Databases
    {

        /// <summary>
        /// Firma dell'evento PropertyChangedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Oggetto base DB
        /// </summary>
        [Serializable]
        public abstract class DBObjectBase  //
            : DMDObject, IDMDXMLSerializable, IDBObjectBase
        {

            /// <summary>
            /// Evento generato quando viene modificata una proprietà dell'oggetto
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            

            private int m_ID;
            [NonSerialized] private int m_OldID;
            [NonSerialized] private CCollection<PropertyChangedEventArgs> m_Changes = new CCollection<PropertyChangedEventArgs>();
            [NonSerialized] private bool m_Changed;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectBase()
            {
                m_ID = 0;
                m_Changed = true;
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            bool IDBMinObject.SaveToDatabase(CDBConnection dbConn, bool force) { return this.SaveToDatabase(dbConn, force);  }
            bool IDBMinObject.DropFromDatabase(CDBConnection dbConn, bool force) { return this.DropFromDatabase(dbConn, force); }
            
            int IDBObjectBase.ID { get { return this.ID; } }

            void IDBObjectBase.SetID(int newID) { this.SetID(newID); }
            void IDBObjectBase.ResetID() { this.ResetID();  }
            bool IDBObjectBase.SaveToRecordset(DBWriter writer) { return this.SaveToRecordset(writer);  }
            bool IDBObjectBase.LoadFromRecordset(DBReader reader) { return this.LoadFromRecordset(reader); }
            string IDBObjectBase.GetTableName() { return this.GetTableName(); }

            /// <summary>
            /// Restituisce l'ID univoco che identifica l'oggetto nella tabella del database in cui è salvato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ID
            {
                get
                {
                    return m_ID;
                }
            }


            /// <summary>
            /// Quando sottoposto ad override in una classe derivata restituisce il modulo che gestisce l'oggetto
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract Sistema.CModule GetModule();

            /// <summary>
        /// Genera l'evento PropertyChanged
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChanged?.Invoke(this, e);
            }

            /// <summary>
        /// Informa l'oggetto che la proprietà specificata è stata modificata
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="newVal"></param>
        /// <param name="oldVal"></param>
        /// <remarks></remarks>
            protected virtual void DoChanged(string propName, object newVal = null, object oldVal = null)
            {
                var e = new PropertyChangedEventArgs(propName, newVal, oldVal);
                m_Changes.Add(e);
                m_Changed = true;
                OnPropertyChanged(e);
            }

            /// <summary>
        /// Restituisce vero se l'oggetto è stato modificato
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool IsChanged()
            {
                return m_Changed;
            }

            /// <summary>
        /// Imposta il valore IsChanged
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
            public void SetChanged(bool value)
            {
                m_Changed = value;
                if (!value)
                    m_Changes.Clear();
            }

            // Public Sub Save()
            // Me.GetConnection.Save(Me)
            // End Sub

            // Private Shared sem As New ManualResetEvent(True)

            public virtual void Save(bool force = false)
            {
                var e = new SystemEvent();
                OnBeforeSave(e);
                if (force || !(IsChanged() == false && ID != 0))
                {
                    // sem.WaitOne()
                    // Try
                    GetConnection().SaveObject(this, force);
                    // Me.SaveToDatabase(Me.GetConnection, force)
                    // Catch ex As Exception
                    // Throw
                    // Finally
                    // sem.Set()
                    // End Try
                }

                if (GetModule() is object)
                {
                    // Me.GetModule().UpdateCached(Me)
                }

                OnAfterSave(e);
            }

            protected virtual bool SaveToDatabase(CDBConnection dbConn, bool force)
            {

                // Dim lockObj As Object = Me.GetTable
                // If (lockObj Is Nothing) Then lockObj = dbConn



                // SyncLock lockObj
                if (dbConn is null)
                    throw new ArgumentNullException("dbConn");
                if (dbConn.IsOpen() == false)
                    throw new InvalidOperationException("La connessione [" + dbConn.Path + "] non è aperta");
                if (m_ID == 0 || force || IsChanged())
                {
                    DataSet ds = null;
                    DataTable dt = null;
                    IDataAdapter da = null;
                    DataRow dr = null;
                    IDbCommand cmd = null;
                    DBWriter writer = null;
                    CKeyCollection<bool> changedValues = null;
                    bool hasChanges = default, tmp;
                    string[] keys = null;
                    CKeyCollection<object> originalValues = null;

                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    string selectCommand;
                    if (ID == 0)
                    {
                        selectCommand = "SELECT * FROM " + GetTable().InternalName + " WHERE (0<>0)";
                    }
                    else
                    {
                        selectCommand = "SELECT * FROM " + GetTable().InternalName + " WHERE ([" + GetIDFieldName() + "]=" + ID + ")";
                    }

                    da = dbConn.CreateAdapter(selectCommand);
                    ds = new DataSet(GetTableName());
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    if (ID == 0)
                    {
                        dr = dt.Rows.Add();
                    }
                    else
                    {
                        dr = dt.Rows[0];
                        originalValues = new CKeyCollection<object>();
                        for (int i = 0, loopTo = dt.Columns.Count - 1; i <= loopTo; i++)
                            originalValues.Add(dt.Columns[i].ColumnName, dr[dt.Columns[i].ColumnName]);
                    }

                    dt.Dispose();
                    dt = null;
                    ds.Dispose();
                    ds = null;
                    writer = new DBWriter(GetTable(), dr);
                    SaveToRecordset(writer);
                    if (ID == 0)
                    {
                        cmd = dbConn.GetInsertCommand(this, GetIDFieldName(), dr, ref m_ID);
                        cmd.ExecuteNonQuery();
                        SetID(DMD.Integers.CInt(dbConn.ExecuteScalar("SELECT @@IDENTITY")));
                    }
                    else
                    {
                        changedValues = new CKeyCollection<bool>();
                        keys = originalValues.Keys;
                        for (int i = 0, loopTo1 = DMD.Arrays.UBound(keys); i <= loopTo1; i++)
                        {
                            bool localCompare() { 
                                var tmp1 = dr;
                                var tmp2 = originalValues;
                                var arga = tmp1[keys[i]]; 
                                var argb = tmp2[keys[i]]; 
                                return DMD.RunTime.EQ(arga, argb); 
                            }

                            tmp = !localCompare();
                            hasChanges = hasChanges || tmp;
                            changedValues.Add(keys[i], tmp);
                        }

                        if (hasChanges)
                        {
                            cmd = dbConn.GetUpdateCommand(this, GetIDFieldName(), ID, dr, changedValues);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    m_Changes.Clear();
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

                    if (cmd is object)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }

                    if (dt is object)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    if (ds is object)
                    {
                        ds.Dispose();
                        ds = null;
                    }

                    if (writer is object)
                    {
                        writer.Dispose();
                        writer = null;
                    }

                    if (keys is object)
                        keys = null;
                    changedValues = null;
                    originalValues = null;
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                }

                // End SyncLock




                return true;
            }

            private class RowUpdater
            {
                public OleDbDataAdapter da;
                public CDBConnection conn;
                public DataRow dr;
                public int ID;
                public ManualResetEvent sem = new ManualResetEvent(false);

                public RowUpdater(CDBConnection conn, OleDbDataAdapter da, DataRow dr)
                {
                    this.conn = conn;
                    this.da = da;
                    this.dr = dr;
                }

                public void Run()
                {
                    da.RowUpdated += rowUpdated;
                    da.Update(new[] { dr });
                    sem.WaitOne();
                    sem.Dispose();
                    da.RowUpdated -= rowUpdated;
                }

                private void rowUpdated(object sender, OleDbRowUpdatedEventArgs e)
                {
                    if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
                    {
                        // Get the Identity column value
                        ID = DMD.Integers.CInt(conn.ExecuteScalar("SELECT @@IDENTITY"));
                        e.Row["ID"] = ID;
                        e.Row.AcceptChanges();
                        sem.Set();
                    }
                }
            }

            /// <summary>
            /// Deserializza dal DB
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected virtual bool LoadFromRecordset(DBReader reader)
            {
                m_ID = reader.Read(GetIDFieldName(), m_ID);
                m_Changed = false;
                m_Changes.Clear();
                return true;
            }

            /// <summary>
            /// Serializza nel DB
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected virtual bool SaveToRecordset(DBWriter writer)
            {
                return true;
            }

            /// <summary>
            /// Restituisce un riferimento al database 
            /// </summary>
            /// <returns></returns>
            protected internal virtual CDBConnection GetConnection()
            {
                var m = this.GetModule();
                return (m is object )? m.GetConnection() : Databases.APPConn;
            }

            /// <summary>
            /// Restituisce il nome del campo ID
            /// </summary>
            /// <returns></returns>
            protected virtual string GetIDFieldName()
            {
                return "ID";
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public abstract string GetTableName();

            [NonSerialized] private CDBTable m_Table;

            /// <summary>
            /// Restituisce la tabella
            /// </summary>
            /// <returns></returns>
            protected virtual CDBTable GetTable()
            {
                if (m_Table is null)
                    m_Table = GetConnection().Tables[GetTableName()];
                return m_Table;
            }

            /// <summary>
            /// Prepara lo schema del db per la serializzazione dell'oggetto
            /// </summary>
            protected internal virtual void PrepareDBSchema(CDBConnection conn)
            {
                var tableName = this.GetTableName();
                if (string.IsNullOrEmpty(tableName)) return;
                CDBTable table = conn.Tables.Ensure(tableName);
                this.PrepareDBSchemaFields(table);
                table.Update();
                this.PrepareDBSchemaConstraints(table);                
            }

            /// <summary>
            /// Prepara i campi dello schema
            /// </summary>
            /// <param name="table"></param>
            protected virtual void PrepareDBSchemaFields(CDBTable table)
            {
                var c = table.Fields.Ensure("ID", typeof(int), 0);
                
            }

            /// <summary>
            /// Prepara i vincoli dello schema
            /// </summary>
            /// <param name="table"></param>
            protected virtual void PrepareDBSchemaConstraints(CDBTable table)
            {
                var c = table.Constraints.Ensure("idxID", new string[] { "ID" }, true);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray("{ ", Sistema.vbTypeName(this), "[" , this.ID , "] }");
            }

            /// <summary>
            /// Elimina l'oggetto dal DB
            /// </summary>
            /// <param name="force"></param>
            public virtual void Delete(bool force = false)
            {
                if (ID != 0)
                {
                    GetConnection().DeleteObject(this, force);
                    SetID(0);
                }
            }

            /// <summary>
            /// Elimina l'oggetto dal DB
            /// </summary>
            /// <param name="dbConn"></param>
            /// <param name="force"></param>
            /// <returns></returns>
            protected virtual bool DropFromDatabase(CDBConnection dbConn, bool force)
            {
                dbConn.ExecuteCommand("DELETE * FROM [" + GetTableName() + "] WHERE [" + GetIDFieldName() + "]=" + ID);
                OnDelete(new SystemEvent());
                return true;
            }

            /// <summary>
            /// Reimposta l'ID
            /// </summary>
            protected virtual void ResetID()
            {
                m_ID = 0;
            }

            /// <summary>
            /// Imposta l'ID
            /// </summary>
            /// <param name="newID"></param>
            protected virtual void SetID(int newID)
            {
                // #If DEBUG Then
                // If (newID <> 0 AndAlso Me.GetConnection IsNot Nothing AndAlso Me.GetConnection.IsOpen) Then
                // Dim dbRis As System.Data.IDataReader = Me.GetConnection.ExecuteReader("SELECT * FROM [" & Me.GetTableName & "] WHERE [" & Me.GetIDFieldName & "]=" & newID)
                // If (dbRis.Read = False) Then
                // Debug.Print("OPPS")
                // End If
                // dbRis.Dispose()
                // End If
                // #End If

                m_ID = newID;
            }

            /// <summary>
            /// Evento generato quando l'oggetto viene creato
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnCreate(SystemEvent e)
            {
            }

            /// <summary>
            /// Evento generato quando l'oggetto viene eliminato
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnDelete(SystemEvent e)
            {
            }

            /// <summary>
            /// Evento generato quando l'oggetto viene modificato
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnModified(SystemEvent e)
            {
            }

            /// <summary>
            /// Evento generato prima del salvataggio
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnBeforeSave(SystemEvent e)
            {
            }

            /// <summary>
            /// Evento generato dopo il salvataggio
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnAfterSave(SystemEvent e)
            {
                if (m_OldID == 0 & m_ID != 0)
                {
                    OnCreate(e);
                }
                else
                {
                    OnModified(e);
                }
            }

            /// <summary>
            /// Deserializza l'oggetto
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_ID":
                        {
                            SetID((int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue));
                            break;
                        }

                    case "m_Changed":
                        {
                            m_Changed = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Changes":
                        {
                            m_Changes.Clear();
                            if (fieldValue is IEnumerable)
                            {
                                m_Changes.AddRange((IEnumerable)fieldValue);
                            } // Throw New ArgumentOutOfRangeException(fieldName)

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }

            /// <summary>
            /// Serializza l'oggetto
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_ID", m_ID);
                writer.WriteAttribute("m_Changed", m_Changed);
                // writer.WriteTag("Changes", Me.m_Changes)
            }

            /// <summary>
            /// Operatore di uguaglianza
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(object obj)
            {
                if (obj is null) return false;
                return this.Equals((DBObjectBase)obj);
            }

            /// <summary>
            /// Operatore di uguaglianza
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(DBObjectBase obj)
            {
                if (this.GetType() != obj.GetType()) return false;
                return this.ID == obj.ID;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.ID.GetHashCode();
            }

            /// <summary>
            /// Operatore di uguaglianza
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator ==(DBObjectBase a, DBObjectBase b)
            {
                if (a is null)
                {
                    if (b is null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (b is null)
                {
                    return false;
                }
                else
                {
                    return a.Equals(b);
                }
            }

            /// <summary>
            /// Operatore di disuguaglianza
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator !=(DBObjectBase a, DBObjectBase b)
            {
                if (a is null)
                {
                    if (b is null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (b is null)
                {
                    return true;
                }
                else
                {
                    return !a.Equals(b);
                }
            }

            /// <summary>
            /// Copia tutti i valori delle proprietà compresi quelle definite nelle gerarchie sottostanti
            /// </summary>
            /// <param name="value"></param>
            /// <remarks></remarks>
            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
            }

            /// <summary>
            /// Copia tutti i valori delle proprietà tranne quelli definiti a partire da questo oggetto in poi
            /// </summary>
            /// <param name="value"></param>
            /// <remarks></remarks>
            public override void CopyFrom(object value)
            {
                var f1 = Sistema.Types.GetAllFields(value.GetType());
                var f2 = Sistema.Types.GetAllFields(typeof(DBObject));
                var f3 = Sistema.Types.GetAllFields(typeof(DBObjectBase));
                foreach (System.Reflection.FieldInfo f in f1)
                {
                    if (!f.IsInitOnly && DMD.Arrays.IndexOf(f2, f) < 0 && DMD.Arrays.IndexOf(f3, f) < 0)
                    {
                        f.SetValue(this, f.GetValue(value));
                    }
                }
            }

            /// <summary>
            /// Restituisce true se la proprietà dell'oggetto é stata modificata
            /// </summary>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            internal bool IsFieldChanged(string fieldName)
            {
                for (int i = 0, loopTo = m_Changes.Count - 1; i <= loopTo; i++)
                {
                    var p = m_Changes[i];
                    if ((p.PropertyName ?? "") == (fieldName ?? ""))
                        return true;
                }

                return false;
            }

            /// <summary>
            /// Restituisce l'ordine della proprietà nel tipo
            /// </summary>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            internal object GetOriginalFieldValue(string fieldName)
            {
                for (int i = 0, loopTo = m_Changes.Count - 1; i <= loopTo; i++)
                {
                    var p = m_Changes[i];
                    if ((p.PropertyName ?? "") == (fieldName ?? ""))
                        return p.OldValue;
                }

                return null;
            }
        }
    }
}