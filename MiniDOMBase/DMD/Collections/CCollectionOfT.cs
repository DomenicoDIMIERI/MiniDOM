using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;

namespace minidom
{

    /// <summary>
    /// Collezione di oggetto di tipo T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CCollection<T> 
        : CCollection, IEnumerable<T>
    {

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
        /// Restituisce l'elemento in base all'ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new T GetItemById(int id)
        {
            return (T)base.GetItemById(id);
        }

        /// <summary>
        /// Restituisce o imposta l'elemento nella posizione index (base 0)
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
        /// Aggiunge l'elemento
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            Add((object)item);
        }

        /// <summary>
        /// Inserisce l'elemento nella posizione index (base 0)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public new void Insert(int index, T item)
        {
            Insert(index, (object)item);
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
        /// Restituisce l'indice base 0 dell'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new int IndexOf(T item)
        {
            return IndexOf((object)item);
        }

        /// <summary>
        /// Restituisce true se la collezione contiene l'elemento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Contains(T item)
        {
            return this.Contains((object)item);
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
        /// Compara due oggetti della collezione
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected sealed override int Compare(object a, object b)
        {
            return CompareWithType((T)a, (T)b);
        }

        /// <summary>
        /// Compara due oggetti della collezione
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual int CompareWithType(T a, T b)
        {
            return base.Compare(a, b);
        }

        /// <summary>
        /// Restituisce un oggetto IEnumerator sulla lista
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator _ListGetEnumerator()
        {
            return new Enumerator<T>(base.GetEnumerator());
        }

        /// <summary>
        /// Restituisce un oggetto IEnumerator sulla lista
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<T> GetEnumerator()
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

        /// <summary>
        /// Aggiunge un elemento alla lista
        /// </summary>
        /// <param name="item"></param>
        protected sealed override void InternalAdd(object item)
        {
            this.InternalAdd((T)item);
        }

        /// <summary>
        /// Aggiunge un elemento alla lista
        /// </summary>
        /// <param name="item"></param>
        protected virtual void InternalAdd(T item)
        {
            base.InternalAdd(item);
        }

        /// <summary>
        /// Inserisce un elemento
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected sealed override void InternalInsert(int index, object item)
        {
            this.InternalInsert(index, (T)item);
        }

        /// <summary>
        /// Inserisce un elemento
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void InternalInsert(int index, T item)
        {
            base.InternalInsert(index, item);
        }

        /// <summary>
        /// Setta un elemento
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected sealed override void InternalSet(int index, object value)
        {
            this.InternalSet(index, (T)value);
        }

        /// <summary>
        /// Setta un elemento
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected virtual void InternalSet(int index, T value)
        {
            base.InternalSet(index, value);
        }

        /// <summary>
        /// Restituisce l'elemento nella posizione index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected sealed override object InternalGet(int index)
        {
            return this.InternalGetT(index);
        }

        /// <summary>
        /// Restituisce l'elemento nella posizione index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual object InternalGetT(int index)
        {
            return base.InternalGet(index);
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
         

    }
}