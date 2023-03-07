using System.Collections.Generic;
using System;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;
using minidom;
using static minidom.Sistema;
using DMD.XML;
using System.Threading;

namespace minidom
{
    public partial class Databases
    {

        [Serializable]
        public class DMDDBCObj 
            : IDisposable
        {
            private const int TIMERMILLI = 5000;
            private const int MAXDBWAITTIME = 30000;

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped ElifDirectiveTrivia */
            private delegate void DWORKER();

            private static System.Timers.Timer m_Timer = new System.Timers.Timer(TIMERMILLI);
            private static DWORKER m_as = null;
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            private static ArrayList m_List = new ArrayList();

            static DMDDBCObj()
            {
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped ElifDirectiveTrivia */
                m_Timer.Elapsed += timerClick;
                m_Timer.Enabled = true;
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }



            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped ElifDirectiveTrivia */
            private static void Worker()
            {
#if (!DEBUG)
                try {
#endif
                    lock (m_List)
                    {
                        int i = 0;
                        while (i < m_List.Count)
                        {
                            DMDDBCObj o = (DMDDBCObj)m_List[i];
                            if (o.ExeTimeMilliseconds >= MAXDBWAITTIME)
                            {
                                m_List.RemoveAt(i);
                                Sistema.ApplicationContext.Log(Sistema.Formats.GetTimeStamp() + " - DB Lock? " + o.ToString());
                            }
                            else
                            {
                                i += 1;
                            }
                        }
                    }
#if (!DEBUG)
                } catch (Exception ex) {
                    Sistema.Events.NotifyUnhandledException(ex);
                }
#endif
                m_as = null;
            }

            private static void timerClick(object sender, EventArgs e)
            {
                if (m_as is object)
                    return;
                m_as = Worker;
                m_as.BeginInvoke(null, null);
            }

            /* TODO ERROR: Skipped EndIfDirectiveTrivia */


            public static ArrayList GetList()
            {
                lock (m_List)
                    return (ArrayList)m_List.Clone();
            }

            [NonSerialized] public CDBConnection Conn;
            private DateTime m_CreationTime;
            private DateTime? m_CompletionTime = default;
            protected bool toRemove = false;

            public DMDDBCObj(CDBConnection conn)
            {
                Conn = conn;
                m_CreationTime = DMD.DateUtils.Now();
                // Me.UserName = Sistema.Users.usern
            }

            public virtual void Create()
            {
            }

            public double ExeTimeMilliseconds
            {
                get
                {
                    if (CompletionTime.HasValue)
                    {
                        return (CompletionTime.Value - CreationTime).TotalMilliseconds;
                    }
                    else
                    {
                        return (DMD.DateUtils.Now() - CreationTime).TotalMilliseconds;
                    }
                }
            }

            public DateTime CreationTime
            {
                get
                {
                    return m_CreationTime;
                }
            }

            public DateTime? CompletionTime
            {
                get
                {
                    return m_CompletionTime;
                }
            }

            protected void doStart()
            {
                if (!toRemove)
                {
                    lock (m_List)
                    {
                        m_List.Add(this);
                        toRemove = true;
                    }
                }

                m_CreationTime = DMD.DateUtils.Now();
                m_CompletionTime = default;
            }

            protected void doCompleted()
            {
                if (toRemove)
                {
                    lock (m_List)
                    {
                        m_List.Remove(this);
                        toRemove = false;
                    }
                }

                m_CompletionTime = DMD.DateUtils.Now();
            }

            // Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            public virtual void Dispose()
            {
                if (toRemove)
                {
                    lock (m_List)
                        m_List.Remove(this);
                }

                Conn = null;
            }
        }



        public class DMDDBCommand 
            : DMDDBCObj, IDbCommand
        {
            public string Text;
            public IDbCommand m_cmd;

            public DMDDBCommand(CDBConnection conn, string text) : base(conn)
            {
                Text = text;
            }

            public override void Create()
            {
                m_cmd = Conn.CreateCommandInternal(Text);
            }

            public string CommandText
            {
                get
                {
                    return m_cmd.CommandText;
                }

                set
                {
                    m_cmd.CommandText = value;
                }
            }

            public int CommandTimeout
            {
                get
                {
                    return m_cmd.CommandTimeout;
                }

                set
                {
                    m_cmd.CommandTimeout = value;
                }
            }

            public CommandType CommandType
            {
                get
                {
                    return m_cmd.CommandType;
                }

                set
                {
                    m_cmd.CommandType = value;
                }
            }

            public IDbConnection Connection
            {
                get
                {
                    return m_cmd.Connection;
                }

                set
                {
                    m_cmd.Connection = value;
                }
            }

            public IDataParameterCollection Parameters
            {
                get
                {
                    return m_cmd.Parameters;
                }
            }

            public IDbTransaction Transaction
            {
                get
                {
                    return m_cmd.Transaction;
                }

                set
                {
                    m_cmd.Transaction = value;
                }
            }

            public UpdateRowSource UpdatedRowSource
            {
                get
                {
                    return m_cmd.UpdatedRowSource;
                }

                set
                {
                    m_cmd.UpdatedRowSource = value;
                }
            }

            public void Cancel()
            {
                m_cmd.Cancel();
            }

            public void Prepare()
            {
                m_cmd.Prepare();
            }

            public IDbDataParameter CreateParameter()
            {
                return m_cmd.CreateParameter();
            }

            private void WarnQuery(string sql, double time)
            {
                var buffer = new System.Text.StringBuilder();
                buffer.Append("Query lenta: (");
                buffer.Append(Sistema.Formats.FormatDurata((long?)(time / 1000d)));
                buffer.Append(")");
                buffer.Append(DMD.Strings.vbNewLine);
                buffer.Append(sql);
                buffer.Append(DMD.Strings.vbNewLine);
                Sistema.ApplicationContext.Log(buffer.ToString());
            }

            public int ExecuteNonQuery()
            {
                doStart();
                var t = DMD.DateUtils.Now();
                int ret = m_cmd.ExecuteNonQuery();
                var t1 = DMD.DateUtils.Now();
                double exeTime = (t1 - t).TotalMilliseconds;
                if (exeTime >= 1000d && Sistema.ApplicationContext is object)
                {
                    WarnQuery(m_cmd.CommandText, exeTime);
                }

                doCompleted();
                return ret;
            }

            public IDataReader ExecuteReader()
            {
                doStart();
                var t = DMD.DateUtils.Now();
                var ret = new DMDDBReader(Conn, m_cmd, m_cmd.CommandText);
                ret.Create();
                var t1 = DMD.DateUtils.Now();
                double exeTime = (t1 - t).TotalMilliseconds;
                if (exeTime >= 1000d && Sistema.ApplicationContext is object)
                {
                    WarnQuery(m_cmd.CommandText, exeTime);
                }

                doCompleted();
                return ret;
            }

            public IDataReader ExecuteReader(CommandBehavior behavior)
            {
                doStart();
                var t = DMD.DateUtils.Now();
                var ret = new DMDDBReader(Conn, m_cmd, m_cmd.CommandText);
                ret.Create(behavior);
                var t1 = DMD.DateUtils.Now();
                double exeTime = (t1 - t).TotalMilliseconds;
                if (exeTime >= 1000d && Sistema.ApplicationContext is object)
                {
                    WarnQuery(m_cmd.CommandText, exeTime);
                }

                doCompleted();
                return ret;
            }

            public object ExecuteScalar()
            {
                doStart();
                var t = DMD.DateUtils.Now();
                object ret = null;
                // Try
                ret = m_cmd.ExecuteScalar();
                // Catch ex As OleDbException
                // DBUtils.TryToRecoverSomeTable
                // ret = Me.m_cmd.ExecuteScalar
                // Catch ex As Exception
                // Throw
                // End Try

                var t1 = DMD.DateUtils.Now();
                double exeTime = (t1 - t).TotalMilliseconds;
                if (exeTime >= 1000d && Sistema.ApplicationContext is object)
                {
                    WarnQuery(m_cmd.CommandText, exeTime);
                }

                doCompleted();
                return ret;
            }


            // Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            public override void Dispose() // Implements IDisposable.Dispose
            {
                if (m_cmd is object)
                    m_cmd.Dispose();
                m_cmd = null;
                base.Dispose();
            }

            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                if (m_cmd is object)
                {
                    ret.Append(" - ");
                    ret.Append(m_cmd.CommandText);
                }

                return ret.ToString();
            }
        }

        public class DMDDBReader
            : DMDDBCObj, IDataReader
        {
            private string m_Text;
            public IDbCommand m_Cmd;
            public IDataReader m_Reader;

            public DMDDBReader(CDBConnection conn, IDbCommand cmd, string text) : base(conn)
            {
                m_Cmd = cmd;
                m_Text = text;
            }

            public override void Create()
            {
                doStart();
                m_Reader = m_Cmd.ExecuteReader();
                doCompleted();
            }

            public void Create(CommandBehavior b)
            {
                doStart();
                m_Reader = m_Cmd.ExecuteReader(b);
                doCompleted();
            }

            public int Depth
            {
                get
                {
                    return m_Reader.Depth;
                }
            }

            public int FieldCount
            {
                get
                {
                    return m_Reader.FieldCount;
                }
            }

            public bool IsClosed
            {
                get
                {
                    return m_Reader.IsClosed;
                }
            }

            public object this[string name]
            {
                get
                {
                    return m_Reader[name];
                }
            }

            public object this[int i]
            {
                get
                {
                    return m_Reader[i];
                }
            }

            public int RecordsAffected
            {
                get
                {
                    return m_Reader.RecordsAffected;
                }
            }

            public void Close()
            {
                if (m_Reader is object)
                {
                    m_Reader.Close();
                    m_Reader.Dispose();
                }

                m_Reader = null;
            }

            public bool GetBoolean(int i)
            {
                return m_Reader.GetBoolean(i);
            }

            public byte GetByte(int i)
            {
                return m_Reader.GetByte(i);
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                return m_Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
            }

            public char GetChar(int i)
            {
                return m_Reader.GetChar(i);
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                return m_Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
            }

            public IDataReader GetData(int i)
            {
                return m_Reader.GetData(i);
            }

            public string GetDataTypeName(int i)
            {
                return m_Reader.GetDataTypeName(i);
            }

            public DateTime GetDateTime(int i)
            {
                return m_Reader.GetDateTime(i);
            }

            public decimal GetDecimal(int i)
            {
                return m_Reader.GetDecimal(i);
            }

            public double GetDouble(int i)
            {
                return m_Reader.GetDouble(i);
            }

            public Type GetFieldType(int i)
            {
                return m_Reader.GetFieldType(i);
            }

            public float GetFloat(int i)
            {
                return m_Reader.GetFloat(i);
            }

            public Guid GetGuid(int i)
            {
                return m_Reader.GetGuid(i);
            }

            public short GetInt16(int i)
            {
                return m_Reader.GetInt16(i);
            }

            public int GetInt32(int i)
            {
                return m_Reader.GetInt32(i);
            }

            public long GetInt64(int i)
            {
                return m_Reader.GetInt64(i);
            }

            public string GetName(int i)
            {
                return m_Reader.GetName(i);
            }

            public int GetOrdinal(string name)
            {
                return m_Reader.GetOrdinal(name);
            }

            public DataTable GetSchemaTable()
            {
                return m_Reader.GetSchemaTable();
            }

            public string GetString(int i)
            {
                return m_Reader.GetString(i);
            }

            public object GetValue(int i)
            {
                return m_Reader.GetValue(i);
            }

            public int GetValues(object[] values)
            {
                return m_Reader.GetValues(values);
            }

            public bool IsDBNull(int i)
            {
                return m_Reader.IsDBNull(i);
            }

            public bool NextResult()
            {
                return m_Reader.NextResult();
            }

            public bool Read()
            {
                return m_Reader.Read();
            }

            public override void Dispose() // Implements IDisposable.Dispose
            {
                if (m_Reader is object)
                    m_Reader.Dispose();
                m_Reader = null;
                base.Dispose();
            }

            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(m_Text))
                {
                    ret.Append(" - ");
                    ret.Append(m_Text);
                }

                return ret.ToString();
            }
        }

        public enum DBAuthenticationTypes : int
        {
            NotSet = 0,
            WindowsIntegration = 1,
            UserNameAndPassword = 2
        }

        public enum DBShareMode : int
        {
            OpenShared = 0,
            OpenExclusive = 1
            // OpenRead = 2
            // OpenShareDenyRead = 3
            // OpenShareDenyWrite = 4
        }


        
        /// <summary>
        /// Connessione generica ad un database
        /// </summary>
        /// <remarks></remarks>
        public abstract class CDBConnection : IDisposable, IDMDXMLSerializable
        {
            private const int QUERYMAXEXECTIMELOG = 250;
            private static int queryCNT = 0;

            private static int GetUniqueQueryID()
            {
                Interlocked.Increment(ref queryCNT);
                return queryCNT;
            }

    /// <summary>
            /// Compatta il database
            /// </summary>
            public void Compact()
            {
                this.CompactInternal();
            }

            /// <summary>
            /// Compatta il database
            /// </summary>
            protected internal abstract void CompactInternal();

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            private const int MAXDEBUGITEMS = 500;

            public event ConnectionOpenedEventHandler ConnectionOpened;

            public delegate void ConnectionOpenedEventHandler(object sender, EventArgs e);

            public event ConnectionClosedEventHandler ConnectionClosed;

            public delegate void ConnectionClosedEventHandler(object sender, EventArgs e);

            public event ConnectionErrorEventHandler ConnectionError;

            public delegate void ConnectionErrorEventHandler(object sender, EventArgs e);

            public event QueryBeginEventHandler QueryBegin;

            public delegate void QueryBeginEventHandler(object sender, DBQueryEventArgs e);

            public event QueryCompletedEventHandler QueryCompleted;

            public delegate void QueryCompletedEventHandler(object sender, DBQueryCompletedEventArgs e);

            private string m_Path;
            private DBAuthenticationTypes m_AuthType;
            private System.Net.NetworkCredential m_Credentials;
            private System.Net.NetworkCredential m_EncriptionKey;
            private IDbConnection m_dbConn;
            private CDBTablesCollection m_Tables;
            private CDBQueriesCollection m_Queries;
            private DBShareMode m_ShareMode;


            public readonly object dbLock = new object();

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDBConnection()
            {
                DMDObject.IncreaseCounter(this);
                m_Path = "";
                m_dbConn = null;
                m_AuthType = DBAuthenticationTypes.NotSet;
            }

            /// <summary>
            /// Crea il database
            /// </summary>
            protected internal abstract void CreateInternal();


            /// <summary>
            /// Crea il database
            /// </summary>
            public void Create()
            {
                CreateInternal();
            }

            public DBShareMode ShareMode
            {
                get
                {
                    return m_ShareMode;
                }

                set
                {
                    m_ShareMode = value;
                }
            }

            public DBAuthenticationTypes AuthenticationType
            {
                get
                {
                    return m_AuthType;
                }

                set
                {
                    if (IsOpen())
                        throw new InvalidOperationException("Il DB è già aperto");
                    m_AuthType = value;
                }
            }

            public byte[] EncryptionKey
            {
                get
                {
                    if (m_EncriptionKey is null)
                    {
                        return null;
                    }
                    else
                    {
                        return System.Text.Encoding.ASCII.GetBytes(m_EncriptionKey.Password);
                    }
                }

                set
                {
                    if (value is null)
                    {
                        m_EncriptionKey = null;
                    }
                    else
                    {
                        m_EncriptionKey = new System.Net.NetworkCredential("", System.Text.Encoding.ASCII.GetString(value));
                    }
                }
            }

            protected System.Net.NetworkCredential GetCredentials()
            {
                return m_Credentials;
            }

            protected internal abstract IDataAdapter CreateAdapterInternal(string selectCommand);

            public IDataAdapter CreateAdapter(string selectCommand)
            {
                // Dim ret As New DMDDBAdapter(selectCommand, Me)
                // ret.Create
                // Return ret
                return CreateAdapterInternal(selectCommand);
            }

            public abstract IDbConnection CreateConnection();

            public IDbCommand CreateCommand(string sql)
            {
                var ret = new DMDDBCommand(this, sql);
                ret.Create();
                return ret;
            }

            protected internal abstract IDbCommand CreateCommandInternal(string sql);
            public abstract IDbCommand GetUpdateCommand(object obj, string idFieldName, int idValue, DataRow dr, CKeyCollection<bool> changedValues);
            public abstract IDbCommand GetInsertCommand(object obj, string idFieldName, DataRow dr, [Optional, DefaultParameterValue(0)] ref int maxID);
            public abstract string GetSqlDataType(CDBEntityField field);
            protected internal abstract void CreateTable(CDBTable table);
            protected internal abstract void UpdateTable(CDBTable table);
            protected internal abstract void DropTable(CDBTable table);
            protected internal abstract void CreateView(CDBQuery view);
            protected internal abstract void UpdateView(CDBQuery vuew);
            protected internal abstract void DropView(CDBQuery view);
            protected internal abstract void CreateField(CDBEntityField field);
            protected internal abstract void DropField(CDBEntityField field);
            protected internal abstract void UpdateField(CDBEntityField field);
            protected internal abstract void CreateFieldConstraint(CDBFieldConstraint c);
            protected internal abstract void UpdateFieldConstraint(CDBFieldConstraint c);
            protected internal abstract void DropFieldConstraint(CDBFieldConstraint c);
            protected internal abstract void CreateTableConstraint(CDBTableConstraint c);
            protected internal abstract void UpdateTableConstraint(CDBTableConstraint c);
            protected internal abstract void DropTableConstraint(CDBTableConstraint c);
            /// <summary>
        /// Restituisce o imposta la stringa che identifica la risorsa database
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Path
            {
                get
                {
                    return m_Path;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    if ((m_Path ?? "") == (value ?? ""))
                        return;
                    m_Path = value;
                }
            }

            public void SetCredentials(string userName, string password)
            {
                m_Credentials = new System.Net.NetworkCredential(userName, password);
            }

            /// <summary>
        /// Restituisce la collezione delle tabelle contenute nel database
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CDBTablesCollection Tables
            {
                get
                {
                    if (IsOpen() == false)
                        throw new DBException("Il database non è aperto: " + Path);

                    lock (this.dbLock)
                    {
                        if (m_Tables is null)
                        {
                            m_Tables = new CDBTablesCollection(this);
                            var arr = GetTablesArray();
                            for (int i = 0, loopTo = DMD.Arrays.Len(arr) - 1; i <= loopTo; i++)
                            {
                                var item = arr[i];
                                m_Tables.Add(item);
                                item.SetChanged(false);
                                item.SetCreated(true);
                                item.SetConnection(this);
                            }
                        }

                        return m_Tables;
                    }
                }
            }

            protected abstract CDBTable[] GetTablesArray();

            /// <summary>
        /// Restituisce la collezione delle queries contenute nel database
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CDBQueriesCollection Queries
            {
                get
                {
                    lock (this.dbLock)
                    {
                        if (IsOpen() == false)
                            throw new DBException("Il database non è aperto:  " + Path);
                        if (m_Queries is null)
                            m_Queries = new CDBQueriesCollection(this);
                        return m_Queries;
                    }
                }
            }

            public virtual object ToDB<T>(T value)
            {
                if (value is null)
                    return DBNull.Value;
                if (value is null)
                    return DBNull.Value;
                return value;
            }

            public virtual object ToDB(object value)
            {
                if (value is DBNull)
                    return DBNull.Value;
                if (value is null)
                    return DBNull.Value;
                return value;
            }

            /// <summary>
        /// Cambia la password del database
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
            public void ChangeDabasePassword(string newPassword, string oldPassword)
            {
                ChangeDatabasePasswordInternal(newPassword, oldPassword);
            }

            /// <summary>
        /// Quando sottoposto ad override in una classe derivata cambia la password del database
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
            protected abstract void ChangeDatabasePasswordInternal(string newPassword, string oldPassword);


            // adStateClosed      0           The object is closed
            // adStateOpen        1           The object is open
            // adStateConnecting  2       The object is connecting
            // adStateExecuting   4       The object is executing a command
            // adStateFetching    8       The rows of the object are being retrieved

            /// <summary>
        /// Restituisce vero se la connessione è aperta
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsOpen()
            {
                if (m_dbConn is null)
                    return false;
                return m_dbConn.State != 0;
            }

            /// <summary>
        /// Apre la connessione
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool OpenDB()
            {
                if (IsOpen())
                    throw new DBException("Il database è già aperto: " + Path);
                lock (this.dbLock)
                {
                    if (m_dbConn is null)
                        m_dbConn = CreateConnection();
                    m_dbConn.Open();
                }
                var e = new DBEventArgs(this);
                OnConnectionOpened(e);
                DBUtils.doConnectionOpened(e);
                return true;
            }

            protected virtual void OnConnectionOpened(EventArgs e)
            {
                ConnectionOpened?.Invoke(this, e);
            }

            /// <summary>
        /// Chiude la connessione
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CloseDB()
            {
                if (IsOpen() == false)
                    throw new DBException("Il database non è aperto: " + Path);
                lock (this.dbLock)
                {
                    m_dbConn.Close();
                    m_dbConn = null;
                    m_Tables = null;
                    m_Queries = null;
                    this.locks = new Dictionary<string, object>();
                }
                var e = new DBEventArgs(this);
                OnConnectionClosed(e);
                DBUtils.doConnectionClosed(e);
                return true;
            }

            protected virtual void OnConnectionClosed(EventArgs e)
            {
                ConnectionClosed?.Invoke(this, e);
            }

            /// <summary>
        /// Restituisce l'oggetto IDBConnection sottostante
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public IDbConnection GetConnection()
            {
                return m_dbConn;
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */

            public virtual string str2db(object value)
            {
                if (value is DBNull || value is null)
                {
                    return "NULL";
                }
                else
                {
                    return DMD.Strings.JoinW("'", DMD.Strings.Replace(DMD.Strings.CStr(value), "'", "''"), "'");
                }
            }

            public virtual string int2db(int? value)
            {
                return DBUtils.DBNumber(value);
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /// <summary>
        /// Restituisce un data reader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public IDataReader ExecuteReader(string sql)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");
                
                sql = DMD.Strings.Trim(sql);

                var dbCmd = CreateCommand(sql);

                return dbCmd.ExecuteReader();
            }

            protected virtual void OnQueryBegin(DBQueryEventArgs e)
            {
                if (DBUtils.StopStatistics)
                    return;
                QueryBegin?.Invoke(this, e);
            }

            protected virtual void OnQueryCompleted(DBQueryCompletedEventArgs e)
            {
                if (DBUtils.StopStatistics)
                    return;
                QueryCompleted?.Invoke(this, e);
            }

            /// <summary>
        /// Restituisce il valore di una query scalare
        /// </summary>
        /// <param name="dbSQL"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public object ExecuteScalar(string dbSQL)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");

                dbSQL = DMD.Strings.Trim(dbSQL);
                using (var dbCmd = CreateCommand(dbSQL))
                {
                    return dbCmd.ExecuteScalar();
                }
            }

            /// <summary>
        /// Esegue il comando
        /// </summary>
        /// <param name="sql"></param>
        /// <remarks></remarks>
            public void ExecuteCommand(string sql)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");
                
                sql = DMD.Strings.Trim(sql);

                using (var cmd = CreateCommand(sql))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            private Dictionary<string, object> locks = new Dictionary<string, object>();

            protected internal virtual object GetTableLock(string tableName)
            {
                lock (this.dbLock)
                {
                    object ret = null;
                    if (!this.locks.TryGetValue(tableName, out ret)) {
                        ret = new object();
                        this.locks.Add(tableName, ret);
                    }
                    return ret;
                }
            }

            /// <summary>
            /// Carica l'oggetto dal recordset
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Load(object obj, DBReader dbRis)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");
                if (obj is IDBObjectBase)
                {
                    return ((IDBObjectBase)obj).LoadFromRecordset(dbRis);
                }
                else
                {
                    throw new ArgumentException("L'oggetto non implementa alcuna interfaccia valida");
                }
            }

            public bool Load(object obj, IDataReader dbRis)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");
                if (obj is IDBObjectBase)
                {
                    var reader = new DBReader(dbRis);
                    return ((IDBObjectBase)obj).LoadFromRecordset(reader);
                }
                else
                {
                    throw new ArgumentException("L'oggetto non implementa alcuna interfaccia valida");
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return Sistema.FileSystem.GetFileName(m_Path);
                }
            }

            public virtual string GetFriendlyName(string name)
            {
                return name;
            }

            public string GetSaveTableName(object obj)
            {
                return ((IDBObjectBase)obj).GetTableName();
            }

            public virtual string GetInternalTableName(CDBEntity table)
            {
                return table.Name;
            }


            // Public Function Delete(ByVal item As Object, Optional ByVal force As Boolean = False) As Boolean
            // Return DirectCast(item, IDBMinObject).DropFromDatabase(Me, force)
            // End Function

            public bool Drop(CDBTableConstraint constraint)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");
                lock (this.dbLock)
                {
                    var table = constraint.Owner;
                    ExecuteCommand("DROP INDEX [" + constraint.Name + "] ON [" + table.Name + "]");
                    table.Constraints.Remove(constraint);
                    return true;
                }
            }

            /// <summary>
        /// Elimina una tabella dal database
        /// </summary>
        /// <param name="tableName"></param>
        /// <remarks></remarks>
            public virtual void DropTable(string tableName)
            {
                if (IsOpen() == false)
                    throw new DBException("Il DB [" + Path + "] non è aperto");
                lock (this.dbLock)
                {
                    tableName = DMD.Strings.Replace(DMD.Strings.Trim(tableName), "'", "''");
                    ExecuteCommand("DROP TABLE " + tableName + "");
                    if (m_Tables is object)
                        m_Tables.RemoveByKey(tableName);
                }
            }

            public bool TableExists(string tableName)
            {
                tableName = DMD.Strings.Trim(tableName);
                try
                {
                    using (var r = this.ExecuteReader("SELECT * FROM " + tableName + " WHERE (0<>0)"))
                    {
                        return true;
                    }
                } catch (System.Exception ) {
                    return false;
                }
            }

            public DataTable FillDataTable(string selectCommand)
            {
                var da = CreateAdapter(selectCommand);
                using (var ds = new DataSet())
                { 
                    da.Fill(ds);
                    return ds.Tables[0];
                }                 
            }

            public virtual void Dispose()
            {
                if (IsOpen())
                    CloseDB();
                m_dbConn = null;
                m_Path = DMD.Strings.vbNullString;
                m_Queries = null;
                m_Tables = null;
                this.locks = null;
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Path":
                        {
                            m_Path = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // Case "UserName" : Me.m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                    // Case "Password" : Me.m_Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                    case "Tables":
                        {
                            m_Tables = (CDBTablesCollection)fieldValue;
                            m_Tables.SetOwner(this);
                            break;
                        }

                    case "Queries":
                        {
                            m_Queries = (CDBQueriesCollection)fieldValue;
                            m_Queries.SetOwner(this);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Path", m_Path);
                // writer.WriteAttribute("UserName", Me.m_UserName)
                // writer.WriteAttribute("Password", Me.m_Password)
                // m_dbConn As System.Data.IDbConnection
                writer.WriteTag("Tables", Tables);
                writer.WriteTag("Queries", Queries);
            }

            ~CDBConnection()
            {
                DMDObject.DecreaseCounter(this);
            }

            public virtual void SaveObject(object obj, bool force)
            {
                if (obj is null)
                    throw new ArgumentNullException("obj");
                ((IDBMinObject)obj).SaveToDatabase(this, force);
            }

            public virtual void DeleteObject(object obj, bool force)
            {
                if (obj is null)
                    throw new ArgumentNullException("obj");
                ((IDBMinObject)obj).DropFromDatabase(this, force);
            }

            public virtual object GetItemById(Sistema.CModule m, int id)
            {
                if (id == 0)
                    return null;

                IModuleHandler h = (IModuleHandler)m.CreateHandler(null);
                if (h is null)
                    throw new ArgumentNullException("Module Handler");

                using (var cursor = h.CreateCursor())
                {
                    if (cursor is null)
                        throw new ArgumentNullException("cursor");
                    cursor.ID.Value = id;
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }

            }

            public virtual bool IsRemote()
            {
                return false;
            }

            public abstract string InvokeMethodArray(Sistema.CModule module, string methodName, object[] args);

            public string InvokeMethod(Sistema.CModule module, string methodName, params object[] args)
            {
                return InvokeMethodArray(module, methodName, args);
            }

            public abstract Sistema.AsyncState InvokeMethodArrayAsync(Sistema.CModule module, string methodName, object handler, object[] args);

            public Sistema.AsyncState InvokeMethodAsync(Sistema.CModule module, string methodName, object handler, params object[] args)
            {
                return InvokeMethodArrayAsync(module, methodName, handler, args);
            }
        }
    }
}