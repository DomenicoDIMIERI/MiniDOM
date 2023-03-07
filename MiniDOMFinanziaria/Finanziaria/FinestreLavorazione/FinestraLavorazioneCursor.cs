using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class FinestraLavorazioneCursor : Databases.DBObjectCursorPO<FinestraLavorazione>
        {
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_IconaCliente = new DBCursorStringField("IconaCliente");
            private DBCursorField<StatoFinestraLavorazione> m_StatoFinestra = new DBCursorField<StatoFinestraLavorazione>("StatoFinestra");
            private DBCursorField<FinestraLavorazioneFlags> m_Flags = new DBCursorField<FinestraLavorazioneFlags>("Flags");
            // Private m_DataInizioLavorabilita As New DBCursorField(Of Date)("DataInizioLavorabilita")
            private CCursorFieldDBDate m_DataInizioLavorabilita = new CCursorFieldDBDate("DataInizioLavorabilitaStr");
            private DBCursorField<DateTime> m_DataFineLavorabilita = new DBCursorField<DateTime>("DataFineLavorabilita");
            private DBCursorField<DateTime> m_DataInizioLavorazione = new DBCursorField<DateTime>("DataInizioLavorazione");
            private DBCursorField<int> m_IDRichiestaFinanziamento = new DBCursorField<int>("IDRichiestaF");
            private DBCursorField<int> m_IDStudioDiFattibilita = new DBCursorField<int>("IDStudioF");
            private DBCursorField<int> m_IDCQS = new DBCursorField<int>("IDCQS");
            private DBCursorField<int> m_IDPD = new DBCursorField<int>("IDPD");
            private DBCursorField<int> m_IDCQSI = new DBCursorField<int>("IDCQSI");
            private DBCursorField<int> m_IDPDI = new DBCursorField<int>("IDPDI");
            private DBCursorField<StatoOfferteFL> m_StatoCQS = new DBCursorField<StatoOfferteFL>("StatoCQS");
            private DBCursorField<StatoOfferteFL> m_StatoPD = new DBCursorField<StatoOfferteFL>("StatoPD");
            private DBCursorField<StatoOfferteFL> m_StatoCQSI = new DBCursorField<StatoOfferteFL>("StatoCQSI");
            private DBCursorField<StatoOfferteFL> m_StatoPDI = new DBCursorField<StatoOfferteFL>("StatoPDI");
            private DBCursorField<DateTime> m_DataUltimoAggiornamento = new DBCursorField<DateTime>("DataFineLavorazione");
            private DBCursorField<DateTime> m_DataFineLavorazione = new DBCursorField<DateTime>("DataUltimoAggiornamento");
            private DBCursorField<decimal> m_QuotaCedibile = new DBCursorField<decimal>("QuotaCedibile");
            private DBCursorField<int> m_IDBustaPaga = new DBCursorField<int>("IDBustaPaga");
            private DBCursorField<StatoOfferteFL> m_StatoRichiestaFinanziamento = new DBCursorField<StatoOfferteFL>("StatoRichiestaF");
            private DBCursorField<StatoOfferteFL> m_StatoStudioDiFattibilita = new DBCursorField<StatoOfferteFL>("StatoSF");
            private DBCursorField<int> m_IDContatto = new DBCursorField<int>("IDContatto");
            private DBCursorField<StatoOfferteFL> m_StatoContatto = new DBCursorField<StatoOfferteFL>("StatoContatto");
            private DBCursorField<DateTime> m_DataContatto = new DBCursorField<DateTime>("DataContatto");
            // Private m_DataContatto As New CCursorFieldDBDate("DataContattoStr")
            private DBCursorField<DateTime> m_DataBustaPaga = new DBCursorField<DateTime>("DataBustaPaga");
            private DBCursorField<StatoOfferteFL> m_StatoBustaPaga = new DBCursorField<StatoOfferteFL>("StatoBustaPaga");
            private DBCursorField<int> m_IDRichiestaCertificato = new DBCursorField<int>("IDRichiestaCertificato");
            private DBCursorField<DateTime> m_DataRichiestaCertificato = new DBCursorField<DateTime>("DataRichiestaCertificato");
            private DBCursorField<StatoOfferteFL> m_StatoRichiestaCertificato = new DBCursorField<StatoOfferteFL>("StatoRichiestaCertificato");
            private DBCursorField<DateTime> m_DataRichiestaFinanziamento = new DBCursorField<DateTime>("DataRichiestaFinanziamento");
            private DBCursorField<DateTime> m_DataStudioDiFattibilita = new DBCursorField<DateTime>("DataStudioDiFattibilita");
            private DBCursorField<DateTime> m_DataCQS = new DBCursorField<DateTime>("DataCQS");
            private DBCursorField<DateTime> m_DataPD = new DBCursorField<DateTime>("DataPD");
            private DBCursorField<DateTime> m_DataCQSI = new DBCursorField<DateTime>("DataCQSI");
            private DBCursorField<DateTime> m_DataPDI = new DBCursorField<DateTime>("DataPDI");
            private DBCursorField<int> m_IDPrimaVisita = new DBCursorField<int>("IDPrimaVisita");
            private DBCursorField<StatoOfferteFL> m_StatoPrimaVisita = new DBCursorField<StatoOfferteFL>("StatoPrimaVisita");
            private DBCursorField<DateTime> m_DataPrimaVisita = new DBCursorField<DateTime>("DataPrivaVisita");
            private DBCursorField<DateTime> m_DataImportazione = new DBCursorField<DateTime>("DataImportazione");
            private DBCursorField<DateTime> m_DataEsportazioneOk = new DBCursorField<DateTime>("DataEsportazioneOk");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            private DBCursorField<DateTime> m_DataAttivazione = new DBCursorField<DateTime>("DataAttivazione");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");
            private DBCursorStringField m_DettaglioStato1 = new DBCursorStringField("DettaglioStato1");
            private DBCursorField<DateTime> m_DataRicontatto = new DBCursorField<DateTime>("DataRicontatto");
            private DBCursorField<int> m_IDOperatoreRicontatto = new DBCursorField<int>("IDOpRicontatto");
            private DBCursorStringField m_MotivoRicontatto = new DBCursorStringField("MotivoRicontatto");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            private DBCursorField<int> m_IDConsulente = new DBCursorField<int>("IDConsulente");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorField<int> m_IDConsulenza = new DBCursorField<int>("IDConsulenza");

            public FinestraLavorazioneCursor()
            {
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

            public DBCursorField<int> IDCollaboratore
            {
                get
                {
                    return m_IDCollaboratore;
                }
            }

            public DBCursorField<int> IDConsulenza
            {
                get
                {
                    return m_IDConsulenza;
                }
            }

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            public DBCursorField<DateTime> DataAttivazione
            {
                get
                {
                    return m_DataAttivazione;
                }
            }

            public DBCursorStringField DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }
            }

            public DBCursorStringField DettaglioStato1
            {
                get
                {
                    return m_DettaglioStato1;
                }
            }

            public DBCursorField<int> IDOperatoreRicontatto
            {
                get
                {
                    return m_IDOperatoreRicontatto;
                }
            }

            public DBCursorField<DateTime> DataRicontatto
            {
                get
                {
                    return m_DataRicontatto;
                }
            }

            public DBCursorStringField MotivoRicontatto
            {
                get
                {
                    return m_MotivoRicontatto;
                }
            }




            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            public DBCursorField<int> IDConsulente
            {
                get
                {
                    return m_IDConsulente;
                }
            }

            public DBCursorField<DateTime> DataEsportazioneOk
            {
                get
                {
                    return m_DataEsportazioneOk;
                }
            }

            public DBCursorField<DateTime> DataImportazione
            {
                get
                {
                    return m_DataImportazione;
                }
            }

            public DBCursorField<int> IDPrimaVisita
            {
                get
                {
                    return m_IDPrimaVisita;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoPrimaVisita
            {
                get
                {
                    return m_StatoPrimaVisita;
                }
            }

            public DBCursorField<DateTime> DataPrimaVisita
            {
                get
                {
                    return m_DataPrimaVisita;
                }
            }

            public DBCursorField<DateTime> DataRichiestaFinanziamento
            {
                get
                {
                    return m_DataRichiestaFinanziamento;
                }
            }

            public DBCursorField<DateTime> DataStudioDiFattibilita
            {
                get
                {
                    return m_DataStudioDiFattibilita;
                }
            }

            public DBCursorField<DateTime> DataCQS
            {
                get
                {
                    return m_DataCQS;
                }
            }

            public DBCursorField<DateTime> DataPD
            {
                get
                {
                    return m_DataPD;
                }
            }

            public DBCursorField<DateTime> DataCQSI
            {
                get
                {
                    return m_DataCQSI;
                }
            }

            public DBCursorField<DateTime> DataPDI
            {
                get
                {
                    return m_DataPDI;
                }
            }

            public DBCursorField<int> IDContatto
            {
                get
                {
                    return m_IDContatto;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoContatto
            {
                get
                {
                    return m_StatoContatto;
                }
            }

            public DBCursorField<DateTime> DataContatto
            {
                get
                {
                    return m_DataContatto;
                }
            }

            // Public ReadOnly Property DataContatto As CCursorFieldDBDate
            // Get
            // Return Me.m_DataContatto
            // End Get
            // End Property


            public DBCursorField<StatoOfferteFL> StatoRichiestaFinanziamento
            {
                get
                {
                    return m_StatoRichiestaFinanziamento;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoStudioDiFattibilita
            {
                get
                {
                    return m_StatoStudioDiFattibilita;
                }
            }

            public DBCursorField<int> IDBustaPaga
            {
                get
                {
                    return m_IDBustaPaga;
                }
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

            public DBCursorField<StatoFinestraLavorazione> StatoFinestra
            {
                get
                {
                    return m_StatoFinestra;
                }
            }

            public DBCursorField<FinestraLavorazioneFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            // Public ReadOnly Property DataInizioLavorabilita As DBCursorField(Of Date)
            // Get
            // Return Me.m_DataInizioLavorabilita
            // End Get
            // End Property

            public CCursorFieldDBDate DataInizioLavorabilita
            {
                get
                {
                    return m_DataInizioLavorabilita;
                }
            }

            public DBCursorField<DateTime> DataFineLavorabilita
            {
                get
                {
                    return m_DataFineLavorabilita;
                }
            }

            public DBCursorField<DateTime> DataInizioLavorazione
            {
                get
                {
                    return m_DataInizioLavorazione;
                }
            }

            public DBCursorField<int> IDRichiestaFinanziamento
            {
                get
                {
                    return m_IDRichiestaFinanziamento;
                }
            }

            public DBCursorField<int> IDStudioDiFattibilita
            {
                get
                {
                    return m_IDStudioDiFattibilita;
                }
            }

            public DBCursorField<int> IDCQS
            {
                get
                {
                    return m_IDCQS;
                }
            }

            public DBCursorField<int> IDPD
            {
                get
                {
                    return m_IDPD;
                }
            }

            public DBCursorField<int> IDCQSI
            {
                get
                {
                    return m_IDCQSI;
                }
            }

            public DBCursorField<int> IDPDI
            {
                get
                {
                    return m_IDPDI;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoCQS
            {
                get
                {
                    return m_StatoCQS;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoPD
            {
                get
                {
                    return m_StatoPD;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoCQSI
            {
                get
                {
                    return m_StatoCQSI;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoPDI
            {
                get
                {
                    return m_StatoPDI;
                }
            }

            public DBCursorField<DateTime> DataUltimoAggiornamento
            {
                get
                {
                    return m_DataUltimoAggiornamento;
                }
            }

            public DBCursorField<DateTime> DataFineLavorazione
            {
                get
                {
                    return m_DataFineLavorazione;
                }
            }

            public DBCursorField<decimal> QuotaCedibile
            {
                get
                {
                    return m_QuotaCedibile;
                }
            }

            public DBCursorField<DateTime> DataBustaPaga
            {
                get
                {
                    return m_DataBustaPaga;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoBustaPaga
            {
                get
                {
                    return m_StatoBustaPaga;
                }
            }

            public DBCursorField<int> IDRichiestaCertificato
            {
                get
                {
                    return m_IDRichiestaCertificato;
                }
            }

            public DBCursorField<DateTime> DataRichiestaCertificato
            {
                get
                {
                    return m_DataRichiestaCertificato;
                }
            }

            public DBCursorField<StatoOfferteFL> StatoRichiestaCertificato
            {
                get
                {
                    return m_StatoRichiestaCertificato;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return FinestreDiLavorazione.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDFinestreLavorazione";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new FinestraLavorazione();
            }
        }
    }
}