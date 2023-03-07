using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CQSPDValutazioneAziendaCursor : Databases.DBObjectCursor<CQSPDValutazioneAzienda>
        {
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorStringField m_NomeAzienda = new DBCursorStringField("NomeAzienda");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorStringField m_NomeFonte = new DBCursorStringField("NomeFonte");
            private DBCursorField<decimal> m_CapitaleSociale = new DBCursorField<decimal>("CapitaleSociale");
            private DBCursorField<int> m_NumeroDipendenti = new DBCursorField<int>("NumeroDipendenti");
            private DBCursorField<decimal> m_FatturatoAnnuo = new DBCursorField<decimal>("FatturatoAnnuo");
            private DBCursorField<double> m_RapportoTFR_VN = new DBCursorField<double>("RapportoTFR_VN");
            private DBCursorField<int> m_Rating = new DBCursorField<int>("Rating");
            private DBCursorField<DateTime> m_DataRevisione = new DBCursorField<DateTime>("DataRevisione");
            private DBCursorField<DateTime> m_DataScadenzaRevisione = new DBCursorField<DateTime>("DataScadenzaRevisioneione");
            private DBCursorStringField m_StatoAzienda = new DBCursorStringField("StatoAzienda");
            private DBCursorStringField m_DettaglioStatoAzienda = new DBCursorStringField("DettaglioStatoAzienda");
            private DBCursorField<int> m_GiorniAnticipoEstinzione = new DBCursorField<int>("GiorniAnticipoEstinzione");
            private DBCursorField<CQSPDValutazioneAziendaFlags> m_Flags = new DBCursorField<CQSPDValutazioneAziendaFlags>("Flags");

            public CQSPDValutazioneAziendaCursor()
            {
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

            public DBCursorField<int> IDAzienda
            {
                get
                {
                    return m_IDAzienda;
                }
            }

            public DBCursorStringField NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }
            }

            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }
            }

            public DBCursorField<int> IDFonte
            {
                get
                {
                    return m_IDFonte;
                }
            }

            public DBCursorStringField NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }
            }

            public DBCursorField<decimal> CapitaleSociale
            {
                get
                {
                    return m_CapitaleSociale;
                }
            }

            public DBCursorField<int> NumeroDipendenti
            {
                get
                {
                    return m_NumeroDipendenti;
                }
            }

            public DBCursorField<decimal> FatturatoAnnuo
            {
                get
                {
                    return m_FatturatoAnnuo;
                }
            }

            public DBCursorField<double> RapportoTFR_VN
            {
                get
                {
                    return m_RapportoTFR_VN;
                }
            }

            public DBCursorField<int> Rating
            {
                get
                {
                    return m_Rating;
                }
            }

            public DBCursorField<DateTime> DataRevisione
            {
                get
                {
                    return m_DataRevisione;
                }
            }

            public DBCursorField<DateTime> DataScadenzaRevisione
            {
                get
                {
                    return m_DataScadenzaRevisione;
                }
            }

            public DBCursorStringField StatoAzienda
            {
                get
                {
                    return m_StatoAzienda;
                }
            }

            public DBCursorStringField DettaglioStatoAzienda
            {
                get
                {
                    return m_DettaglioStatoAzienda;
                }
            }

            public DBCursorField<int> GiorniAnticipoEstinzione
            {
                get
                {
                    return m_GiorniAnticipoEstinzione;
                }
            }

            public DBCursorField<CQSPDValutazioneAziendaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return ValutazioniAzienda.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDValutazioniAzienda";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}