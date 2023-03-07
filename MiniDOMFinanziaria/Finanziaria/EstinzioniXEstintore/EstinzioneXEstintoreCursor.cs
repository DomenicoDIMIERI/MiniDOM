using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle estinzioni
    /// </summary>
    /// <remarks></remarks>
        public class EstinzioneXEstintoreCursor : Databases.DBObjectCursor<EstinzioneXEstintore>
        {
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<bool> m_Selezionata = new DBCursorField<bool>("Selezionata");
            private DBCursorField<int> m_IDEstinzione = new DBCursorField<int>("IDEstinzione");
            private DBCursorField<int> m_IDEstintore = new DBCursorField<int>("IDEstintore");
            private DBCursorStringField m_TipoEstintore = new DBCursorStringField("TipoEstintore");
            private DBCursorField<DateTime> m_DataDecorrenza = new DBCursorField<DateTime>("DataDecorrenza");
            private DBCursorField<int> m_NumeroQuoteInsolute = new DBCursorField<int>("NQI");
            private DBCursorField<int> m_NumeroQuoteScadute = new DBCursorField<int>("NQS");
            private DBCursorField<int> m_NumeroQuoteResidue = new DBCursorField<int>("NQR");
            private DBCursorStringField m_Parametro = new DBCursorStringField("Parametro");
            private DBCursorField<decimal> m_Correzione = new DBCursorField<decimal>("Correzione");
            private DBCursorField<double> m_PenaleEstinzione = new DBCursorField<double>("PenaleEstinzione");
            private DBCursorField<DateTime> m_DataEstinzione = new DBCursorField<DateTime>("DataEstinzione");
            private DBCursorField<decimal> m_Rata = new DBCursorField<decimal>("Rata");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Durata = new DBCursorField<int>("Durata");
            private DBCursorField<double> m_TAN = new DBCursorField<double>("TAN");
            private DBCursorField<double> m_TAEG = new DBCursorField<double>("TAEG");
            private DBCursorField<double> m_TotaleDaEstinguere = new DBCursorField<double>("TotaleDaEstinguere");
            private DBCursorField<int> m_IDCessionario = new DBCursorField<int>("IDCessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorStringField m_NomeAgenzia = new DBCursorStringField("NomeAgenzia");
            private DBCursorField<TipoEstinzione> m_Tipo = new DBCursorField<TipoEstinzione>("Tipo");
            private DBCursorField<decimal> m_MontanteResiduo = new DBCursorField<decimal>("MontanteResiduo");
            private DBCursorField<DateTime> m_DataCaricamento = new DBCursorField<DateTime>("DataCaricamento");
            private DBCursorField<decimal> m_MontanteResiduoForzato = new DBCursorField<decimal>("MontanteResiduoForzato");

            public EstinzioneXEstintoreCursor()
            {
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<decimal> MontanteResiduoForzato
            {
                get
                {
                    return m_MontanteResiduoForzato;
                }
            }

            public DBCursorField<DateTime> DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }
            }

            public DBCursorField<double> PenaleEstinzione
            {
                get
                {
                    return m_PenaleEstinzione;
                }
            }

            public DBCursorField<decimal> MontanteResiduo
            {
                get
                {
                    return m_MontanteResiduo;
                }
            }

            public DBCursorField<TipoEstinzione> Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            public DBCursorField<int> IDCessionario
            {
                get
                {
                    return m_IDCessionario;
                }
            }

            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            public DBCursorStringField NomeAgenzia
            {
                get
                {
                    return m_NomeAgenzia;
                }
            }

            public DBCursorField<DateTime> DataEstinzione
            {
                get
                {
                    return m_DataEstinzione;
                }
            }

            public DBCursorField<decimal> Rata
            {
                get
                {
                    return m_Rata;
                }
            }

            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            public DBCursorField<int> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            public DBCursorField<double> TAN
            {
                get
                {
                    return m_TAN;
                }
            }

            public DBCursorField<double> TAEG
            {
                get
                {
                    return m_TAEG;
                }
            }

            public DBCursorField<double> TotaleDaEstinguere
            {
                get
                {
                    return m_TotaleDaEstinguere;
                }
            }

            public DBCursorField<bool> Selezionata
            {
                get
                {
                    return m_Selezionata;
                }
            }

            public DBCursorStringField Parametro
            {
                get
                {
                    return m_Parametro;
                }
            }

            public DBCursorField<int> IDEstinzione
            {
                get
                {
                    return m_IDEstinzione;
                }
            }

            public DBCursorField<int> IDEstintore
            {
                get
                {
                    return m_IDEstintore;
                }
            }

            public DBCursorStringField TipoEstintore
            {
                get
                {
                    return m_TipoEstintore;
                }
            }

            public DBCursorField<DateTime> DataDecorrenza
            {
                get
                {
                    return m_DataDecorrenza;
                }
            }

            public DBCursorField<int> NumeroQuoteInsolute
            {
                get
                {
                    return m_NumeroQuoteInsolute;
                }
            }

            public DBCursorField<int> NumeroQuoteScadute
            {
                get
                {
                    return m_NumeroQuoteScadute;
                }
            }

            public DBCursorField<int> NumeroQuoteResidue
            {
                get
                {
                    return m_NumeroQuoteResidue;
                }
            }

            public DBCursorField<decimal> Correzione
            {
                get
                {
                    return m_Correzione;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new EstinzioneXEstintore();
            }

            public override string GetTableName()
            {
                return "tbl_EstinzioniXEstintore";
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}