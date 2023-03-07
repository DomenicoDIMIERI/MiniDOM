using System;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    [Serializable]
    public class CReadOnlyCollection<T> 
        : CReadOnlyCollection, IEnumerable<T>
    {
        public CReadOnlyCollection()
        {
        }

        public new T GetItemById(int id)
        {
            return (T)base.GetItemById(id);
        }

        public new T this[int index]
        {
            get
            {
                return (T)base[index];
            }
        }

        public new int IndexOf(T item)
        {
            return IndexOf((object)item);
        }

        public new int Contains(T item)
        {
            return DMD.Integers.CInt(Contains((object)item));
        }

        public new T[] ToArray()
        {
            return base.ToArray<T>();
        }

        protected override int Compare(object a, object b)
        {
            return CompareT((T)a, (T)b);
        }

        protected virtual int CompareT(T a, T b)
        {
            return base.Compare(a, b);
        }

        //protected override void XMLSerialize(XMLWriter writer)
        //{
        //    writer.BeginTag("items");
        //    writer.Write(this.ToArray());
        //    writer.EndTag("items");
        //}

        protected override void SetFieldInternal(string fieldName, object fieldValue)
        {
            base.SetFieldInternal(fieldName, fieldValue);
        }

        public virtual new IEnumerator<T> GetEnumerator()
        {
            return new Enumerator<T>(base.GetEnumerator());
        }
    }
}