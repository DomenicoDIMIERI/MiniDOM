using System;

namespace minidom
{
    [Serializable]
    public class CSyncCollection : CCollection
    {
        [NonSerialized]
        private readonly object lockObject = new object();

        public CSyncCollection()
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
    }
}