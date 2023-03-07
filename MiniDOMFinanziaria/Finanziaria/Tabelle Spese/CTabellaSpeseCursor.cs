using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
        /// Cursore sulla tabella delle spese
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTabellaSpeseCursor 
            : Databases.DBObjectCursor<CTabellaSpese>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_CessionarioID = new DBCursorField<int>("Cessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<double> m_Running = new DBCursorField<double>("Running");
            private DBCursorField<bool> m_Visible = new DBCursorField<bool>("Visible");
            private DBCursorField<TabellaSpeseFlags> m_Flags = new DBCursorField<TabellaSpeseFlags>("Flags");
            private bool m_OnlyValid;

            public CTabellaSpeseCursor()
            {
                m_OnlyValid = false;
            }

            public DBCursorField<TabellaSpeseFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
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

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<bool> Visible
            {
                get
                {
                    return m_Visible;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<int> CessionarioID
            {
                get
                {
                    return m_CessionarioID;
                }
            }

            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
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

            public DBCursorField<double> Running
            {
                get
                {
                    return m_Running;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CTabellaSpese();
            }

            public override string GetTableName()
            {
                return "tbl_TabellaSpese";
            }

            public override string GetWherePart()
            {
                string wherePart = base.GetWherePart();
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([Flags] AND 1) = 1)", " AND ");
                }

                return wherePart;
            }

            protected override Sistema.CModule GetModule()
            {
                return TabelleSpese.Module;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OnlyValid", m_OnlyValid);
                base.XMLSerialize(writer);
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
        }
    }
}