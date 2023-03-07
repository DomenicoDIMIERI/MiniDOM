using System;

namespace minidom
{
    [Serializable]
    public class CSyncCollection<T> : CSyncCollection
    {
        public CSyncCollection()
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
    }
}