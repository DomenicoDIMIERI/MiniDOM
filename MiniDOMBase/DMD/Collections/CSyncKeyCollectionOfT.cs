using System;

namespace minidom
{
    [Serializable]
    public class CSyncKeyCollection<T> : CSyncKeyCollection
    {
        public CSyncKeyCollection()
        {
        }

        public CSyncKeyCollection(CKeyCollection col) : base(col)
        {
        }

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
    }
}