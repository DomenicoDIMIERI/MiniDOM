using System;

namespace minidom
{
    public partial class Databases
    {
        [Serializable]
        public sealed class CCursorFieldsCollection : CKeyCollection<CCursorField>
        {
            public CCursorFieldsCollection()
            {
            }

            public new void Add(CCursorField f)
            {
                Add(f.FieldName, f);
            }

            public new void Clear()
            {
                foreach (CCursorField f in this)
                    f.Clear();
            }
        }
    }
}