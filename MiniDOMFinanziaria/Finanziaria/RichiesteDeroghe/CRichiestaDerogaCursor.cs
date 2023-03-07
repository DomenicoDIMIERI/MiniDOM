using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CRichiestaDerogaCursor : Databases.DBObjectCursorPO<CRichiestaDeroga>
        {
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<StatoRichiestaDeroga> m_StatoRichiesta = new DBCursorField<StatoRichiestaDeroga>("StatoRichiesta");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<int> m_IDRichiedente = new DBCursorField<int>("IDRichiedente");
            private DBCursorStringField m_NomeRichiedente = new DBCursorStringField("NomeRichiedente");
            private DBCursorStringField m_MotivoRichiesta = new DBCursorStringField("MotivoRichiesta");
            private DBCursorField<int> m_IDAgenziaConcorrente = new DBCursorField<int>("IDAgenziaConcorrente");
            private DBCursorStringField m_NomeAgenziaConcorrente = new DBCursorStringField("NomeAgenziaConcorrente");
            private DBCursorStringField m_NomeProdottoConcorrente = new DBCursorStringField("NomeProdottoConcorrente");
            private DBCursorStringField m_NumeroPreventivoConcorrente = new DBCursorStringField("NumeroPreventivoConcorrente");
            private DBCursorField<decimal> m_RataConcorrente = new DBCursorField<decimal>("RataConcorrente");
            private DBCursorField<int> m_DurataConcorrente = new DBCursorField<int>("DurataConcorrente");
            private DBCursorField<decimal> m_NettoRicavoConcorrente = new DBCursorField<decimal>("NettoRicavoConcorrente");
            private DBCursorField<double> m_TANConcorrente = new DBCursorField<double>("TANConcorrente");
            private DBCursorField<double> m_TAEGConcorrente = new DBCursorField<double>("TAEGConcorrente");
            private DBCursorField<int> m_IDOffertaIniziale = new DBCursorField<int>("IDOffertaIniziale");
            private DBCursorStringField m_InviatoA = new DBCursorStringField("InviatoA");
            private DBCursorStringField m_InviatoACC = new DBCursorStringField("InviatoACC");
            private DBCursorStringField m_MezzoDiInvio = new DBCursorStringField("MezzoDiInvio");
            private DBCursorStringField m_SendSubject = new DBCursorStringField("SendSubject");
            private DBCursorStringField m_SendMessange = new DBCursorStringField("SendMessange");
            private DBCursorField<DateTime> m_SendDate = new DBCursorField<DateTime>("SendDate");
            private DBCursorField<DateTime> m_RicevutoIl = new DBCursorField<DateTime>("RicevutoIl");
            private DBCursorField<DateTime> m_RispostoIl = new DBCursorField<DateTime>("RispostoIl");
            private DBCursorStringField m_RispostoDa = new DBCursorStringField("RispostoDa");
            private DBCursorStringField m_RispostoAMezzo = new DBCursorStringField("RispostoAMezzo");
            private DBCursorStringField m_RispostoSubject = new DBCursorStringField("RispostoSubject");
            private DBCursorStringField m_RispostoMessage = new DBCursorStringField("RispostoMessage");
            private DBCursorField<int> m_IDOffertaCorrente = new DBCursorField<int>("IDOffertaCorrente");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDFinestraLavorazione = new DBCursorField<int>("IDFinestraLavorazione");

            public CRichiestaDerogaCursor()
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

            public DBCursorField<StatoRichiestaDeroga> StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }
            }

            public DBCursorField<DateTime> DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }
            }

            public DBCursorField<int> IDRichiedente
            {
                get
                {
                    return m_IDRichiedente;
                }
            }

            public DBCursorStringField NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }
            }

            public DBCursorStringField MotivoRichiesta
            {
                get
                {
                    return m_MotivoRichiesta;
                }
            }

            public DBCursorField<int> IDAgenziaConcorrente
            {
                get
                {
                    return m_IDAgenziaConcorrente;
                }
            }

            public DBCursorStringField NomeAgenziaConcorrente
            {
                get
                {
                    return m_NomeAgenziaConcorrente;
                }
            }

            public DBCursorStringField NomeProdottoConcorrente
            {
                get
                {
                    return m_NomeProdottoConcorrente;
                }
            }

            public DBCursorStringField NumeroPreventivoConcorrente
            {
                get
                {
                    return m_NumeroPreventivoConcorrente;
                }
            }

            public DBCursorField<decimal> RataConcorrente
            {
                get
                {
                    return m_RataConcorrente;
                }
            }

            public DBCursorField<int> DurataConcorrente
            {
                get
                {
                    return m_DurataConcorrente;
                }
            }

            public DBCursorField<decimal> NettoRicavoConcorrente
            {
                get
                {
                    return m_NettoRicavoConcorrente;
                }
            }

            public DBCursorField<double> TANConcorrente
            {
                get
                {
                    return m_TANConcorrente;
                }
            }

            public DBCursorField<double> TAEGConcorrente
            {
                get
                {
                    return m_TAEGConcorrente;
                }
            }

            public DBCursorField<int> IDOffertaIniziale
            {
                get
                {
                    return m_IDOffertaIniziale;
                }
            }

            public DBCursorStringField InviatoA
            {
                get
                {
                    return m_InviatoA;
                }
            }

            public DBCursorStringField InviatoACC
            {
                get
                {
                    return m_InviatoACC;
                }
            }

            public DBCursorStringField MezzoDiInvio
            {
                get
                {
                    return m_MezzoDiInvio;
                }
            }

            public DBCursorStringField SendSubject
            {
                get
                {
                    return m_SendSubject;
                }
            }

            public DBCursorStringField SendMessage
            {
                get
                {
                    return m_SendMessange;
                }
            }

            public DBCursorField<DateTime> SendDate
            {
                get
                {
                    return m_SendDate;
                }
            }

            public DBCursorField<DateTime> RicevutoIl
            {
                get
                {
                    return m_RicevutoIl;
                }
            }

            public DBCursorField<DateTime> RispostoIl
            {
                get
                {
                    return m_RispostoIl;
                }
            }

            public DBCursorStringField RispostoDa
            {
                get
                {
                    return m_RispostoDa;
                }
            }

            public DBCursorStringField RispostoAMezzo
            {
                get
                {
                    return m_RispostoAMezzo;
                }
            }

            public DBCursorStringField RispostoSubject
            {
                get
                {
                    return m_RispostoSubject;
                }
            }

            public DBCursorStringField RispostoMessage
            {
                get
                {
                    return m_RispostoMessage;
                }
            }

            public DBCursorField<int> IDOffertaCorrente
            {
                get
                {
                    return m_IDOffertaCorrente;
                }
            }

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<int> IDFinestraLavorazione
            {
                get
                {
                    return m_IDFinestraLavorazione;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return RichiesteDeroghe.Module;
            }

            public override string GetTableName()
            {
                return "tbl_RichiesteDeroghe";
            }
        }
    }
}