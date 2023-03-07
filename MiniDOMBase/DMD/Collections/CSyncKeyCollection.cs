using System;

namespace minidom
{
    [Serializable]
    public class CSyncKeyCollection : CKeyCollection
    {
        [NonSerialized]
        protected readonly object lockObject = new object();

        public CSyncKeyCollection()
        {
        }

        public override bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public override object SyncRoot
        {
            get
            {
                return lockObject;
            }
        }

        public CSyncKeyCollection(CKeyCollection col) : base(col)
        {
        }
    }
}