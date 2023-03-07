using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CConsulentiPraticaCursor : Databases.DBObjectCursorPO<CConsulentePratica>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_IDUser = new DBCursorField<int>("IDUser");
            private bool m_OnlyValid;

            public CConsulentiPraticaCursor()
            {
                m_OnlyValid = false;
            }

            public DBCursorField<int> IDUser
            {
                get
                {
                    return m_IDUser;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    m_OnlyValid = value;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return Consulenti.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDConsulenti";
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("OnlyValid", m_OnlyValid);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OnlyValid":
                        {
                            m_OnlyValid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string GetWherePart()
            {
                string wherePart = base.GetWherePart();
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                }

                return wherePart;
            }
        }
    }
}