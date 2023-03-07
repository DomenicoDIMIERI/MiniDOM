using System;

namespace minidom
{
    public partial class Databases
    {
        public class CDBFieldConstraints : CCollection<CDBFieldConstraint>
        {
            private CDBTableConstraint m_Owner;

            public CDBFieldConstraints()
            {
                m_Owner = null;
            }

            public CDBFieldConstraints(CDBTableConstraint tblConstraint)
            {
                if (tblConstraint is null)
                    throw new ArgumentNullException("tblConstraint");
                SetOwner(tblConstraint);
            }

            protected internal void SetOwner(CDBTableConstraint owner)
            {
                m_Owner = owner;
                foreach (CDBFieldConstraint t in this)
                    t.SetOwner(owner);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CDBFieldConstraint)value).SetOwner(m_Owner);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Owner is object)
                    ((CDBFieldConstraint)newValue).SetOwner(m_Owner);
                base.OnSet(index, oldValue, newValue);
            }

            public CDBFieldConstraint Add(CDBEntityField column)
            {
                var item = new CDBFieldConstraint(column);
                Add(item);
                return item;
            }
        }
    }
}