using System;
using System.Collections;
using static minidom.Sistema;
using DMD;
using DMD.XML;

namespace minidom
{

    /// <summary>
    /// Collezione in sola lettura
    /// </summary>
    [Serializable]
    // Inherits System.Collections.CollectionBase
    public class CReadOnlyCollection 
            : ICollection, IComparer, IDMDXMLSerializable, Databases.IDBObjectCollection
    {

        /// <summary>
        /// Oggetto interno che implementa la lista
        /// </summary>
        protected object m_List;

        /// <summary>
        /// Se vero indica che la lista é ordinata
        /// </summary>
        protected bool m_Sorted;

        /// <summary>
        /// Comparatore usato per ordinare la lista
        /// </summary>
        [NonSerialized] private IComparer m_Comparer;

        /// <summary>
        /// Costruttore
        /// </summary>
        public CReadOnlyCollection()
        {
            DMDObject.IncreaseCounter(this);
            m_List = new ArrayList();
            m_Sorted = false;
            m_Comparer = (IComparer)DMD.Arrays.DefaultComparer;
        }

        /// <summary>
        /// Distruttore
        /// </summary>
        ~CReadOnlyCollection()
        {
            DMDObject.DecreaseCounter(this);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }
        /// <summary>
        /// Restituisce il numero di elementi nella lista
        /// </summary>
        public virtual int Count
        {
            get
            {
                return ((ArrayList)m_List).Count;
            }
        }

        /// <summary>
        /// Copia tutti gli elementi della lista nell'array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public virtual void CopyTo(Array array, int index)
        {
            ((ArrayList)m_List).CopyTo(array, index);
        }

        /// <summary>
        /// Restituisce true se la lista è sincronizzata
        /// </summary>
        public virtual bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto su cui viene sincronizzata la lista
        /// </summary>
        public virtual object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Restituisce un oggetto IEnumerator sulla lista
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this._ListGetEnumerator();
        }

        /// <summary>
        /// Restituisce un oggetto IEnumerator sulla lista
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator _ListGetEnumerator()
        {
            return ((ArrayList)m_List).GetEnumerator();
        }

        /// <summary>
        /// Rimuove l'elemento della lista nella posizione index
        /// </summary>
        /// <param name="index"></param>
        protected virtual void InternalRemoveAt(int index)
        {
            ((ArrayList)m_List).RemoveAt(index);
        }

        /// <summary>
        /// Inserisce l'elemento nella lista
        /// </summary>
        /// <param name="item"></param>
        protected virtual void InternalAdd(object item)
        {
            ((ArrayList)m_List).Add(item);
        }

        /// <summary>
        /// Inserisce l'elemento nella posizione specificata
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void InternalInsert(int index, object item)
        {
            ((ArrayList)m_List).Insert(index, item);
        }

        /// <summary>
        /// Inserisce tutti gli elementi nella lista
        /// </summary>
        /// <param name="items"></param>
        protected virtual void InternalAddRange(ICollection items)
        {
            ((ArrayList)m_List).AddRange(items);
        }

        /// <summary>
        /// Evento generato prima che un elemento viene inserito nella lista
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void OnInsert(int index, object value)
        {
        }

        /// <summary>
        /// Evento generato dopo che un elemento é stato inserito nella lista
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void OnInsertComplete(int index, object value)
        {
        }

        /// <summary>
        /// Evento generato prima che un elemento venga modificato
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSet(int index, object oldValue, object newValue)
        {
        }

        /// <summary>
        /// Evento generato dopo che un elemento é stato modificato
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSetComplete(int index, object oldValue, object newValue)
        {
        }

        /// <summary>
        /// Restituisce l'elemento nella posizione specificata
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual object InternalGet(int index)
        {
            return ((ArrayList)m_List)[index];
        }

        /// <summary>
        /// Restituisce o imposta l'oggetto che viene utilizzato per l'ordinamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object Comparer
        {
            get
            {
                return m_Comparer;
            }

            set
            {
                if (ReferenceEquals(m_Comparer, value))
                    return;
                m_Comparer = (IComparer)value;
                if (m_Sorted)
                    Sort();
            }
        }

        /// <summary>
        /// Cerca e restituisce all'interno della collezione l'oggetto con l'ID specificato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public object GetItemById(int id)
        {
            int i = 0;
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    while (i < Count)
                    {
                        var o = this[i];
                        if (Databases.GetID((Databases.IDBObjectBase)o) == id)
                            return o;
                        i += 1;
                    }

                    return null;
                }
            }
            else
            {
                while (i < Count)
                {
                    var o = this[i];
                    if (Databases.GetID((Databases.IDBObjectBase)o) == id)
                        return o;
                    i += 1;
                }

                return null;
            }
        }

        /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la collezione è ordinata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Sorted
        {
            get
            {
                return m_Sorted;
            }

            set
            {
                if (m_Sorted == value)
                    return;
                m_Sorted = value;
                if (m_Sorted)
                    Sort();
            }
        }

        /// <summary>
        /// Pulisce la lista
        /// </summary>
        protected virtual void InternalClear()
        {
            ((ArrayList)m_List).Clear();
        }

        /// <summary>
        /// Ordina la lista
        /// </summary>
        public virtual void Sort()
        {
            object[] items;
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    if (Count > 0)
                    {
                        var e = new EventArgs();
                        OnBeforeSort(e);
                        items = ToArray();
                        DMD.Arrays.Sort(items, 0, Count, this);
                        InternalClear();
                        InternalAddRange(items);
                        OnAfterSort(e);
                    }
                }
            }
            else if (Count > 0)
            {
                var e = new EventArgs();
                OnBeforeSort(e);
                items = ToArray();
                DMD.Arrays.Sort(items, 0, Count, this);
                InternalClear();
                InternalAddRange(items);
                OnAfterSort(e);
            }
        }

        /// <summary>
        /// Evento generato prima che la lista venga ordinata
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeforeSort(EventArgs e)
        {
        }

        /// <summary>
        /// Evento generato dopo che la lista è stata ordinata
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAfterSort(EventArgs e)
        {
        }

        /// <summary>
        /// Restituisce un array contenente tutti gli elementi della lista
        /// </summary>
        /// <returns></returns>
        public virtual object[] ToArray()
        {
            object[] ret = null;
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    if (Count > 0)
                    {
                        ret = new object[Count];
                        CopyTo(ret, 0);
                    }

                    return ret;
                }
            }
            else
            {
                if (Count > 0)
                {
                    ret = new object[Count];
                    CopyTo(ret, 0);
                }

                return ret;
            }
        }

        /// <summary>
        /// Restituisce un array contenente tutti gli elementi della lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T[] ToArray<T>()
        {
            var ret = DMD.Arrays.Empty<T>();
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    if (Count > 0)
                    {
                        ret = new T[Count];
                        this.CopyTo(ret, 0);
                    }

                    return ret;
                }
            }
            else
            {
                if (Count > 0)
                {
                    ret = new T[Count];
                    this.CopyTo(ret, 0);
                }

                return ret;
            }
        }

        /// <summary>
        /// Restituisce l'elemento nella posizione specificata
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return InternalGet(index);
            }
        }

        /// <summary>
        /// Restituisce la posizione dell'elemento nella lista
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(object item)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    if (m_Sorted)
                    {
                        return BinarySearch(item, 0, Count - 1);
                    }
                    else
                    {
                        return LinearSearch(item, 0, Count - 1);
                    }
                }
            }
            else if (m_Sorted)
            {
                return BinarySearch(item, 0, Count - 1);
            }
            else
            {
                return LinearSearch(item, 0, Count - 1);
            }
        }

        /// <summary>
        /// Ricerca binaria nella lista
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int BinarySearch(object obj, int left, int right)
        {
            int ret = -1;
            if (left == right)
            {
                ret = Sistema.IIF(Compare(obj, this[left]) == 0, left, -1);
            }
            else if (left < right)
            {
                int m, c;
                m = (int)((left + right) / 2d);
                c = Compare(obj, this[m]);
                if (c < 0)
                {
                    ret = BinarySearch(obj, left, m - 1);
                }
                else if (c > 0)
                {
                    ret = BinarySearch(obj, m + 1, right);
                }
                else
                {
                    ret = m;
                }
            }

            return ret;
        }

        private int LinearSearch(object obj, int left, int right)
        {
            // For i As Integer = left To right
            // If Me.Compare(obj, Me.Item(i)) = 0 Then Return i
            // Next
            // Return -1
            return ((ArrayList)m_List).IndexOf(obj, left, right - left + 1);
        }

        /// <summary>
        /// Restituisce true se la lista contiene l'oggetto
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(object item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Compara due elementi
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual int Compare(object a, object b)
        {
            return m_Comparer.Compare(a, b);
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var ret = new System.Text.StringBuilder();
            ret.Append("{ ");
            int i = 0;
            foreach (var obj in this)
            {
                if (i > 0) ret.Append(", ");
                ret.Append(Strings.CStr(obj));
                i += 1;
            }
            ret.Append(" }");
            return ret.ToString();
        }

        // Private Sub AddRange(ByVal items() As Object)
        // Dim i As Integer
        // For i = 0 To UBound(items)
        // MyBase.List.Add(items(i))
        // Next
        // End Sub


        /// <summary>
        /// Deserializzazione XML
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "items":
                    {
                        InternalClear();
                        // If (TypeOf (fieldValue) Is String AndAlso CStr(fieldValue) = "") Then
                        // 'Dim arr() As Object = DMD.Arrays.Convert(Of Object)(fieldValue)
                        // 'If (arr IsNot Nothing) Then Me.InternalAddRange(arr)
                        // Dim arr() As Object = Nothing
                        // Else
                        object[] arr = (object[])DMD.Arrays.Convert<object>(fieldValue);
                        // End If

                        if (arr is object)
                            InternalAddRange(arr); // Throw New MissingFieldException(fieldName)
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Serializzazione XML
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.BeginTag("items");
            writer.Write(this.ToArray());
            writer.EndTag("items");
        }

        /// <summary>
        /// Elimina tutti gli elementi della lista dal database
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        protected virtual bool DropFromDatabase(Databases.CDBConnection dbConn, bool force)
        {
            int i = 0;
            while (i < Count)
            {
                Databases.DBObjectBase item = (Databases.DBObjectBase)this[i];
                item.Delete(force);
                i += 1;
            }

            return true;
        }

        /// <summary>
        /// Salva tutti gli elementi della lista nel database
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        protected virtual bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
        {
            int i = 0;
            while (i < Count)
            {
                Databases.DBObjectBase item = (Databases.DBObjectBase)this[i];
                item.Save(force);
                i += 1;
            }

            return true;
        }

        /// <summary>
        /// Salva tutti gli elementi della lista 
        /// </summary>
        /// <param name="force"></param>
        public virtual void Save(bool force = false)
        {
            int i = 0;
            while (i < Count)
            {
                Databases.DBObjectBase item = (Databases.DBObjectBase)this[i];
                item.Save(force);
                i += 1;
            }
        }

        /// <summary>
        /// Elimina tutti gli elementi della lista
        /// </summary>
        /// <param name="force"></param>
        public virtual void Delete(bool force = false)
        {
            int i = 0;
            while (i < Count)
            {
                Databases.DBObjectBase item = (Databases.DBObjectBase)this[i];
                item.Delete(force);
                i += 1;
            }
        }

        int IComparer.Compare(object x, object y)
        {
            return this.Compare(x, y);
        }

        bool Databases.IDBMinObject.SaveToDatabase(Databases.CDBConnection dbConn, bool force)
        {
            return this.SaveToDatabase(dbConn, force);
        }

        bool Databases.IDBMinObject.DropFromDatabase(Databases.CDBConnection dbConn, bool force)
        {
            return this.DropFromDatabase(dbConn, force);
        }

        /// <summary>
        /// Impostal 'oggetto
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void InternalSet(int index, object value)
        {
            ((ArrayList)m_List)[index] = value;
        }
    }
}