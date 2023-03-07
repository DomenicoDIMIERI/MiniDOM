using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella degli stati di lavorazione delle pratiche
    /// </summary>
    /// <remarks></remarks>
        public class CStatiLavorazionePraticaCursor : Databases.DBObjectCursor<CStatoLavorazionePratica>
        {
            private DBCursorField<StatoPraticaEnum> m_MacroStato = new DBCursorField<StatoPraticaEnum>("MacroStato");
            private DBCursorField<int> m_IDPratica = new DBCursorField<int>("IDPratica");
            private DBCursorField<int> m_IDFromStato = new DBCursorField<int>("IDFromStato");
            private DBCursorField<int> m_IDToStato = new DBCursorField<int>("IDToStato");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorStringField m_Params = new DBCursorStringField("Parameters");
            private DBCursorField<bool> m_Forzato = new DBCursorField<bool>("Forzato");
            private DBCursorField<int> m_IDOfferta = new DBCursorField<int>("IDOfferta");
            private DBCursorStringField m_DescrizioneStato = new DBCursorStringField("DescrizioneStato");
            private DBCursorField<int> m_IDRegolaApplicata = new DBCursorField<int>("IDRegolaApplicata");
            private DBCursorStringField m_NomeRegolaApplicata = new DBCursorStringField("NomeRegolaApplicata");

            public CStatiLavorazionePraticaCursor()
            {
            }

            public DBCursorStringField NomeRegolaApplicata
            {
                get
                {
                    return m_NomeRegolaApplicata;
                }
            }

            public DBCursorField<StatoPraticaEnum> MacroStato
            {
                get
                {
                    return m_MacroStato;
                }
            }

            public DBCursorField<int> IDPratica
            {
                get
                {
                    return m_IDPratica;
                }
            }

            public DBCursorField<int> IDFromStato
            {
                get
                {
                    return m_IDFromStato;
                }
            }

            public DBCursorField<int> IDToStato
            {
                get
                {
                    return m_IDToStato;
                }
            }

            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            public DBCursorStringField Params
            {
                get
                {
                    return m_Params;
                }
            }

            public DBCursorField<bool> Forzato
            {
                get
                {
                    return m_Forzato;
                }
            }

            public DBCursorField<int> IDOfferta
            {
                get
                {
                    return m_IDOfferta;
                }
            }

            public DBCursorStringField DescrizioneStato
            {
                get
                {
                    return m_DescrizioneStato;
                }
            }

            public DBCursorField<int> IDRegolaApplicata
            {
                get
                {
                    return m_IDRegolaApplicata;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return StatiLavorazionePratica.Module;
            }

            public override string GetTableName()
            {
                return "tbl_PraticheSTL";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}