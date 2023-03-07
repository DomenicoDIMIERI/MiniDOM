using System;
using System.Collections;
using System.Collections.Generic;


namespace minidom
{

    /// <summary>
    /// Collezione accessibile per chiave
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CKeyCollection<T> 
        : CKeyCollection, IEnumerable<T>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CKeyCollection()
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="col"></param>
        public CKeyCollection(CKeyCollection col) : base(col)
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="items"></param>
        public CKeyCollection(string[] keys, T[] items) : this()
        {
            for (int i = 0, loopTo = DMD.Arrays.UBound(keys); i <= loopTo; i++)
                Add(keys[i], items[i]);
        }

        /// <summary>
        /// Restituisce l'elemento in base alla chiave
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new T GetItemByKey(string key)
        {
            return (T)base.GetItemByKey(key);
        }

        /// <summary>
        /// Restituisce l'elemento in base all'ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new T GetItemById(int id)
        {
            return (T)base.GetItemById(id);
        }

        /// <summary>
        /// Aggiunge un nuovo elemento
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public new void Add(string key, T item)
        {
            Add(key, (object)item);
        }

        /// <summary>
        /// Rimuove l'elemento
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(T item)
        {
            Remove((object)item);
        }

        /// <summary>
        /// Restituisce l'indice dell'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new int IndexOf(T item)
        {
            return base.IndexOf(item);
        }

        /// <summary>
        /// Restituisce true se la collezione contiene l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Contains(T item)
        {
            return base.Contains(item);
        }

        /// <summary>
        /// Restituisce o imposta l'elemento nella posizione specificata
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new T this[int index]
        {
            get
            {
                return (T)base[index];
            }

            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta l'elemento in base alla chiave
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new T this[string key]
        {
            get
            {
                return (T)base[key];
            }

            set
            {
                base[key] = value;
            }
        }

        /// <summary>
        /// Restituisce un array contenente tutti gli elementi della collezione
        /// </summary>
        /// <returns></returns>
        public new T[] ToArray()
        {
            return base.ToArray<T>();
        }

        /// <summary>
        /// Restitusice un oggetto IEnumerator sulla collezione
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator _ListGetEnumerator()
        {
            return new Enumerator<T>(base.GetEnumerator());
        }

        /// <summary>
        /// Restitusice un oggetto IEnumerator sulla collezione
        /// </summary>
        /// <returns></returns>
        public virtual new IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)base.GetEnumerator();
        }

        /// <summary>
        /// OnInsert
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected sealed override void OnInsert(int index, object value)
        {
            this.OnInsert(index, (T)value);
        }

        /// <summary>
        /// OnInsert
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void OnInsert(int index, T value)
        {
            base.OnInsert(index, value);
        }

        /// <summary>
        /// OnInsertComplete
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected sealed override void OnInsertComplete(int index, object value)
        {
            this.OnInsertComplete(index, (T)value);
        }

        /// <summary>
        /// OnInsertComplete
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void OnInsertComplete(int index, T value)
        {
            base.OnInsertComplete(index, value);
        }

        /// <summary>
        /// OnSet
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected sealed override void OnSet(int index, object oldValue, object newValue)
        {
            this.OnSet(index, (T)oldValue, (T)newValue);
        }

        /// <summary>
        /// OnSet
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSet(int index, T oldValue, T newValue)
        {
            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// OnSetComplete
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected sealed override void OnSetComplete(int index, object oldValue, object newValue)
        {
            this.OnSetComplete(index, (T)oldValue, (T)newValue);
        }

        /// <summary>
        /// OnSetComplete
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSetComplete(int index, T oldValue, T newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);
        }

        /// <summary>
        /// OnRemove
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected sealed override void OnRemove(int index, object value)
        {
            this.OnRemove(index, (T)value);
        }

        /// <summary>
        /// OnRemove
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void OnRemove(int index, T value)
        {
            base.OnRemove(index, value);
        }

        /// <summary>
        /// OnRemoveComplete
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected sealed override void OnRemoveComplete(int index, object value)
        {
            this.OnRemoveComplete(index, (T)value);
        }

        /// <summary>
        /// OnRemoveComplete
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void OnRemoveComplete(int index, T value)
        {
            base.OnRemoveComplete(index, value);
        }

        

    }
}