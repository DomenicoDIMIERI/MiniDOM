using System;

namespace minidom
{
    public partial class Databases
    {
        public class CDBFieldConstraint : CDBObject
        {
            private CDBTableConstraint m_Owner;
            private CDBEntityField m_Column;

            public CDBFieldConstraint()
            {
            }

            public CDBFieldConstraint(CDBEntityField column) : this()
            {
                m_Column = column;
            }

            public CDBEntityField Column
            {
                get
                {
                    return m_Column;
                }
            }

            public CDBTableConstraint Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            protected internal void SetOwner(CDBTableConstraint owner)
            {
                m_Owner = owner;
                if (owner is object)
                    SetConnection(owner.Connection);
            }

            protected override void CreateInternal1()
            {
                Connection.CreateFieldConstraint(this);
            }

            protected override void DropInternal1()
            {
                Connection.DropFieldConstraint(this);
            }

            protected override void UpdateInternal1()
            {
                Connection.UpdateFieldConstraint(this);
            }

            protected override void RenameItnernal(string newName)
            {
                throw new NotImplementedException();
            }

            // Private m_Column As CDBEntityField


        }
    }
}