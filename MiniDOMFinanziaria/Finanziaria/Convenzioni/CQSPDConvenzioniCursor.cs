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
    /// Cursore sulla tabelle delle convenzioni
    /// </summary>
    /// <remarks></remarks>
        public class CQSPDConvenzioniCursor : Databases.DBObjectCursor
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_IDProdotto = new DBCursorField<int>("IDProdotto");
            private DBCursorStringField m_NomeProdotto = new DBCursorStringField("NomeProdotto");
            private DBCursorField<bool> m_Attiva = new DBCursorField<bool>("Attiva");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<double> m_MinimoCaricabile = new DBCursorField<double>("MinimoCaricabile");
            private DBCursorField<int> m_IDAmministrazione = new DBCursorField<int>("IDAmministrazione");
            private DBCursorStringField m_NomeAmministrazione = new DBCursorStringField("NomeAmministrazione");
            private DBCursorStringField m_TipoRapporto = new DBCursorStringField("TipoRapporto");
            private bool m_OnlyValid;

            public CQSPDConvenzioniCursor()
            {
                m_OnlyValid = false;
            }

            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    if (m_OnlyValid == value)
                        return;
                    m_OnlyValid = value;
                    Reset1();
                }
            }

            public DBCursorField<int> IDAmministrazione
            {
                get
                {
                    return m_IDAmministrazione;
                }
            }

            public DBCursorStringField NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }
            }

            public DBCursorStringField TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_IDProdotto;
                }
            }

            public DBCursorStringField NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }
            }

            public DBCursorField<bool> Attiva
            {
                get
                {
                    return m_Attiva;
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

            protected override Sistema.CModule GetModule()
            {
                return Convenzioni.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDConvenzioni";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
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

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CQSPDConvenzione();
            }

            public override string GetWherePart()
            {
                string wherePart = base.GetWherePart();
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "([Attiva]=True)", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                }

                return wherePart;
            }
        }
    }
}