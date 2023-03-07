using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class ClientiLavoratiStatsItemCursor : Databases.DBObjectCursorPO<ClientiLavoratiStatsItem>
        {
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_IconaCliente = new DBCursorStringField("IconaCliente");
            private DBCursorField<StatoLavorazione> m_StatoLavorazione = new DBCursorField<StatoLavorazione>("StatoLavorazione");
            private DBCursorField<SottostatoLavorazione> m_SottostatoLavorazione = new DBCursorField<SottostatoLavorazione>("SottostatoLavorazione");
            private DBCursorField<DateTime> m_DataInizioLavorazione = new DBCursorField<DateTime>("DataInizioLavorazione");
            private DBCursorField<DateTime> m_DataFineLavorazione = new DBCursorField<DateTime>("DataFineLavorazione");
            private DBCursorField<DateTime> m_DataUltimoAggiornamento = new DBCursorField<DateTime>("DataUltimoAggiornamento");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<int> m_NumeroBustePaga = new DBCursorField<int>("NumeroBustePaga");
            private DBCursorField<int> m_NumeroVisite = new DBCursorField<int>("NumeroVisite");
            // Private m_NumeroVisiteConsulenza As Integer 'Numero di visite con scopo consulenza (prima o successiva) durante il periodo della lavorazione
            private DBCursorField<int> m_NumeroRichiesteFinanziamento = new DBCursorField<int>("NumeroRichiesteFinanziamento");
            private DBCursorField<int> m_NumeroRichiesteConteggiEstintivi = new DBCursorField<int>("NumeroRichiesteConteggiEstintivi");
            private DBCursorField<int> m_NumeroStudiDiFattibilita = new DBCursorField<int>("NumeroStudiDiFattibilita");
            private DBCursorField<int> m_NumeroOfferteProposte = new DBCursorField<int>("NumeroOfferteProposte");
            // Private m_NumeroOfferteAccettate As Integer 'Numero di studi di fattibilità accettati dal cliente durante il periodo della lavorazione
            private DBCursorField<int> m_NumeroOfferteRifiutate = new DBCursorField<int>("NumeroOfferteRifiutate");
            private DBCursorField<int> m_NumeroOfferteNonFattibili = new DBCursorField<int>("NumeroOfferteNonFattibili");
            private DBCursorField<int> m_NumeroOfferteBocciate = new DBCursorField<int>("NumeroOfferteBocciate");
            private DBCursorField<int> m_NumeroPratiche = new DBCursorField<int>("NumeroPratiche");
            private DBCursorField<int> m_NumeroPraticheLiquidate = new DBCursorField<int>("NumeroPraticheLiquidate");
            private DBCursorField<int> m_NumeroPraticheAnnullate = new DBCursorField<int>("NumeroPraticheAnnullate");
            private DBCursorField<int> m_NumeroPraticheRifiutate = new DBCursorField<int>("NumeroPraticheRifiutate");
            private DBCursorField<int> m_NumeroPraticheNonFattibili = new DBCursorField<int>("NumeroPraticheNonFattibili");
            private DBCursorField<int> m_NumeroPraticheBocciate = new DBCursorField<int>("NumeroPraticheBocciate");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            public ClientiLavoratiStatsItemCursor()
            {
            }

            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            public DBCursorStringField IconaCliente
            {
                get
                {
                    return m_IconaCliente;
                }
            }

            public DBCursorField<StatoLavorazione> StatoLavorazione
            {
                get
                {
                    return m_StatoLavorazione;
                }
            }

            public DBCursorField<SottostatoLavorazione> SottostatoLavorazione
            {
                get
                {
                    return m_SottostatoLavorazione;
                }
            }

            public DBCursorField<DateTime> DataInizioLavorazione
            {
                get
                {
                    return m_DataInizioLavorazione;
                }
            }

            public DBCursorField<DateTime> DataFineLavorazione
            {
                get
                {
                    return m_DataFineLavorazione;
                }
            }

            public DBCursorField<DateTime> DataUltimoAggiornamento
            {
                get
                {
                    return m_DataUltimoAggiornamento;
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

            public DBCursorField<int> NumeroBustePaga
            {
                get
                {
                    return m_NumeroBustePaga;
                }
            }

            public DBCursorField<int> NumeroVisite
            {
                get
                {
                    return m_NumeroVisite;
                }
            }

            // Private m_NumeroVisiteConsulenza As Integer 'Numero di visite con scopo consulenza (prima o successiva) durante il periodo della lavorazione
            public DBCursorField<int> NumeroRichiesteFinanziamento
            {
                get
                {
                    return m_NumeroRichiesteFinanziamento;
                }
            }

            public DBCursorField<int> NumeroRichiesteConteggiEstintivi
            {
                get
                {
                    return m_NumeroRichiesteConteggiEstintivi;
                }
            }

            public DBCursorField<int> NumeroStudiDiFattibilita
            {
                get
                {
                    return m_NumeroStudiDiFattibilita;
                }
            }

            public DBCursorField<int> NumeroOfferteProposte
            {
                get
                {
                    return m_NumeroOfferteProposte;
                }
            }

            // Private m_NumeroOfferteAccettate As Integer 'Numero di studi di fattibilità accettati dal cliente durante il periodo della lavorazione
            public DBCursorField<int> NumeroOfferteRifiutate
            {
                get
                {
                    return m_NumeroOfferteRifiutate;
                }
            }

            public DBCursorField<int> NumeroOfferteNonFattibili
            {
                get
                {
                    return m_NumeroOfferteNonFattibili;
                }
            }

            public DBCursorField<int> NumeroOfferteBocciate
            {
                get
                {
                    return m_NumeroOfferteBocciate;
                }
            }

            public DBCursorField<int> NumeroPratiche
            {
                get
                {
                    return m_NumeroPratiche;
                }
            }

            public DBCursorField<int> NumeroPraticheLiquidate
            {
                get
                {
                    return m_NumeroPraticheLiquidate;
                }
            }

            public DBCursorField<int> NumeroPraticheAnnullate
            {
                get
                {
                    return m_NumeroPraticheAnnullate;
                }
            }

            public DBCursorField<int> NumeroPraticheRifiutate
            {
                get
                {
                    return m_NumeroPraticheRifiutate;
                }
            }

            public DBCursorField<int> NumeroPraticheNonFattibili
            {
                get
                {
                    return m_NumeroPraticheNonFattibili;
                }
            }

            public DBCursorField<int> NumeroPraticheBocciate
            {
                get
                {
                    return m_NumeroPraticheBocciate;
                }
            }

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return Finanziaria.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDClientiLavorati";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}