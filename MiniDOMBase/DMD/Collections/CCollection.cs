using System;
using System.Collections;
using DMD;
using DMD.XML;

namespace minidom
{

    /// <summary>
    /// Handler dell'evento Changed della Collezione
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CollectionChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Collezione di oggetti
    /// </summary>
    [Serializable]
    public class CCollection 
        : CReadOnlyCollection
    {

        /// <summary>
        /// Evento generato quando la collezione viene modificata
        /// </summary>
        public event CollectionChangedEventHandler CollectionChanged;

        
        /// <summary>
        /// Costruttore
        /// </summary>
        public CCollection()
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="items"></param>
        public CCollection(IEnumerable items) : this()
        {
            AddRange(items);
        }

        /// <summary>
        /// Rimuove tutti gli elementi dalla collezione
        /// </summary>
        public void Clear()
        {
            var e = new EventArgs();
            this.OnClear();
            InternalClear();
            this.OnClearComplete();
            OnCollectionChanged(e);
        }

        /// <summary>
        /// OnClear
        /// </summary>
        protected virtual void OnClear()
        {

        }

        /// <summary>
        /// OnClearComplete
        /// </summary>
        protected virtual void OnClearComplete()
        {

        }

        /// <summary>
        /// Genera l'evento CollectionChanged
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(EventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Restituisce o imposta la capacità della collezione
        /// </summary>
        public virtual int Capacity
        {
            get
            {
                return ((ArrayList)m_List).Capacity;
            }

            set
            {
                ((ArrayList)m_List).Capacity = value;
            }
        }

        /// <summary>
        /// Unisce la collezione
        /// </summary>
        /// <param name="col"></param>
        private void InternalMerge(CReadOnlyCollection col)
        {
            object[] tmpa;
            object[] tmpb;
            object[] tmpc;
            if (col.Count > 0)
            {
                tmpb = col.ToArray();
                DMD.Arrays.Sort(tmpb, 0, col.Count, Comparer);
                if (Count == 0)
                {
                    tmpc = tmpb;
                }
                else
                {
                    tmpa = ToArray();
                    DMD.Arrays.Sort(tmpa, 0, Count, Comparer);
                    tmpc = DMD.Arrays.Merge(tmpa, 0, Count, tmpb, 0, col.Count, Comparer);
                }

                InternalClear();
                AddRange(tmpc);
            }
        }

        /// <summary>
        /// Effettua la UNION delle due collezioni
        /// </summary>
        /// <param name="col"></param>
        public virtual void Merge(CReadOnlyCollection col)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                    InternalMerge(col);
            }
            else
            {
                InternalMerge(col);
            }
        }

        /// <summary>
        /// Aggiunge l'elemto alla collezione
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int index = Count;
                    OnInsert(index, item);
                    InternalAdd(item);
                    if (Sorted)
                        Sort();
                    OnInsertComplete(index, item);
                }
            }
            else
            {
                int index = Count;
                OnInsert(index, item);
                InternalAdd(item);
                if (Sorted)
                    Sort();
                OnInsertComplete(index, item);
            }
        }

        /// <summary>
        /// Inserisce l'elemento nella collezione
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, object item)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    OnInsert(index, item);
                    InternalInsert(index, item);
                    if (Sorted)
                        Sort();
                    OnInsertComplete(index, item);
                }
            }
            else
            {
                OnInsert(index, item);
                InternalInsert(index, item);
                if (Sorted)
                    Sort();
                OnInsertComplete(index, item);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'elemento nella posizione specifica
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new object this[int index]
        {
            get
            {
                if (IsSynchronized)
                {
                    lock (SyncRoot)
                    {
                        if (index < 0 || index >= Count)
                            throw new IndexOutOfRangeException();
                        return base.InternalGet(index);
                    }
                }
                else
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return base.InternalGet(index);
                }
            }

            set
            {
                if (IsSynchronized)
                {
                    lock (SyncRoot)
                    {
                        if (index < 0 || index >= Count)
                            throw new IndexOutOfRangeException();
                        var oldValue = InternalGet(index);
                        OnSet(index, oldValue, value);
                        InternalSet(index, value);
                        OnSetComplete(index, oldValue, value);
                    }
                }
                else
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    var oldValue = InternalGet(index);
                    OnSet(index, oldValue, value);
                    InternalSet(index, value);
                    OnSetComplete(index, oldValue, value);
                }
            }
        }

        

        public virtual void RemoveAt(int index)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    var value = InternalGet(index);
                    OnRemove(index, value);
                    InternalRemoveAt(index);
                    OnRemoveComplete(index, value);
                    OnCollectionChanged(new EventArgs());
                }
            }
            else
            {
                var value = InternalGet(index);
                OnRemove(index, value);
                InternalRemoveAt(index);
                OnRemoveComplete(index, value);
                OnCollectionChanged(new EventArgs());
            }
        }

        protected virtual void OnRemove(int index, object value)
        {
        }

        protected virtual void OnRemoveComplete(int index, object value)
        {
        }

        protected override void InternalRemoveAt(int index)
        {
            ((ArrayList)m_List).RemoveAt(index);
        }

        public void Remove(object item)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                    RemoveAt(IndexOf(item));
            }
            else
            {
                RemoveAt(IndexOf(item));
            }
        }

        public void AddRange(IEnumerable items)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    foreach (object item in items)
                    {
                        int index = Count;
                        OnInsert(index, item);
                        InternalAdd(item);
                        OnInsertComplete(index, item);
                    }

                    if (Sorted)
                        Sort();
                }
            }
            else
            {
                foreach (object item in items)
                {
                    int index = Count;
                    OnInsert(index, item);
                    InternalAdd(item);
                    OnInsertComplete(index, item);
                }

                if (Sorted)
                    Sort();
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            OnCollectionChanged(new EventArgs());
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);
            OnCollectionChanged(new EventArgs());
        }

        public virtual bool IsChanged()
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    foreach (object o in this)
                    {
                        if (Databases.DBUtils.IsChanged(o))
                            return true;
                    }

                    return false;
                }
            }
            else
            {
                foreach (object o in this)
                {
                    if (Databases.DBUtils.IsChanged(o))
                        return true;
                }

                return false;
            }
        }

        public virtual void SetChanged(bool value)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    foreach (object o in this)
                        Databases.DBUtils.SetChanged(o, value);
                }
            }
            else
            {
                foreach (object o in this)
                    Databases.DBUtils.SetChanged(o, value);
            }
        }

        public virtual CCollection IntersectWith(CCollection col)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    CCollection ret = (CCollection)Sistema.Types.CreateInstance(GetType());
                    foreach (object a in this)
                    {
                        foreach (object b in col)
                        {
                            if (DMD.Arrays.Compare(a, b, Comparer) == 0)
                            {
                                ret.Add(a);
                            }
                        }
                    }

                    return ret;
                }
            }
            else
            {
                CCollection ret = (CCollection)Sistema.Types.CreateInstance(GetType());
                foreach (object a in this)
                {
                    foreach (object b in col)
                    {
                        if (DMD.Arrays.Compare(a, b, Comparer) == 0)
                        {
                            ret.Add(a);
                        }
                    }
                }

                return ret;
            }
        }
    }
}