using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Databases
    {
        [Serializable]
        public class CDBTableConstraint : CDBObject
        {
            [NonSerialized]
            private CDBEntity m_Owner;
            private CDBFieldConstraints m_Columns;

            public CDBTableConstraint()
            {
                m_Owner = null;
                m_Columns = null;
            }

            public CDBEntity Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            protected internal void SetOwner(CDBEntity value)
            {
                m_Owner = value;
                if (value is object)
                    SetConnection(value.Connection);
                if (m_Columns is object)
                    m_Columns.SetOwner(this);
            }

            public CDBFieldConstraints Columns
            {
                get
                {
                    if (m_Columns is null)
                        m_Columns = new CDBFieldConstraints(this);
                    return m_Columns;
                }
            }

            protected override void CreateInternal1()
            {
                Connection.CreateTableConstraint(this);
            }

            protected override void DropInternal1()
            {
                Connection.DropTableConstraint(this);
                m_Owner.Constraints.RemoveByKey(Name);
            }

            protected override void UpdateInternal1()
            {
                Connection.UpdateTableConstraint(this);
            }

            protected override void RenameItnernal(string newName)
            {
                throw new NotImplementedException();
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("Columns", Columns);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Columns":
                        {
                            Columns.Clear();
                            Columns.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}