using System;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Cursore sulla tabella degli obiettivi
    /// </summary>
    /// <remarks></remarks>
        public class CObiettivoCategoriaProdottoCursor : Databases.DBObjectCursorPO<CObiettivoCategoriaProdotto>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<TipoObiettivo> m_TipoObiettivo = new DBCursorField<TipoObiettivo>("TipoObiettivo");
            private DBCursorField<PeriodicitaObiettivo> m_PeriodicitaObiettivo = new DBCursorField<PeriodicitaObiettivo>("PeriodicitaObiettivo");
            private DBCursorField<int> m_IDCategoria = new DBCursorField<int>("IDCategoria");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<double> m_Percentuale = new DBCursorField<double>("Percentuale");
            private DBCursorStringField m_NomeGruppo = new DBCursorStringField("NomeGruppo");

            public CObiettivoCategoriaProdottoCursor()
            {
            }

            public DBCursorStringField NomeGruppo
            {
                get
                {
                    return m_NomeGruppo;
                }
            }

            public DBCursorField<double> Percentuale
            {
                get
                {
                    return m_Percentuale;
                }
            }

            public DBCursorField<int> IDCategoria
            {
                get
                {
                    return m_IDCategoria;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<TipoObiettivo> TipoObiettivo
            {
                get
                {
                    return m_TipoObiettivo;
                }
            }

            public DBCursorField<PeriodicitaObiettivo> PeriodicitaObiettivo
            {
                get
                {
                    return m_PeriodicitaObiettivo;
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

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return ObiettiviXCategoria.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDObiettiviXCat";
            }
        }
    }
}