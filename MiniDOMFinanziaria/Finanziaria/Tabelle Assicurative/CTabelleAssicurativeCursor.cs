using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella CTabellaAssicurativa
    /// </summary>
    /// <remarks></remarks>
        public class CTabelleAssicurativeCursor : Databases.DBObjectCursor<CTabellaAssicurativa>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_Dividendo = new DBCursorField<int>("Dividendo");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_IDAssicurazione = new DBCursorField<int>("IDAssicurazione");
            private DBCursorStringField m_NomeAssicurazione = new DBCursorStringField("NomeAssicurazione");
            private bool m_OnlyValid;

            public CTabelleAssicurativeCursor()
            {
                m_OnlyValid = false;
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<int> Dividendo
            {
                get
                {
                    return m_Dividendo;
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

            public DBCursorField<int> IDAssicurazione
            {
                get
                {
                    return m_IDAssicurazione;
                }
            }

            public DBCursorStringField NomeAssicurazione
            {
                get
                {
                    return m_NomeAssicurazione;
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

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CTabellaAssicurativa();
            }

            public override string GetTableName()
            {
                return "tbl_TabelleAssicurative";
            }

            protected override Sistema.CModule GetModule()
            {
                return TabelleAssicurative.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}