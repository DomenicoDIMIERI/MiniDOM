using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CImportExportCursor : Databases.DBObjectCursorPO<CImportExport>
        {
            private DBCursorField<bool> m_Esportazione = new DBCursorField<bool>("Esportazione");
            private DBCursorField<DateTime> m_DataEsportazione = new DBCursorField<DateTime>("DataEsportazione");
            private DBCursorField<int> m_IDEsportataDa = new DBCursorField<int>("IDEsportataDa");
            private DBCursorStringField m_NomeEsportataDa = new DBCursorStringField("NomeEsportataDa");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<int> m_IDPresaInCaricoDa = new DBCursorField<int>("IDPresaInCaricoDa");
            private DBCursorStringField m_NomePresaInCaricoDa = new DBCursorStringField("NomePresaInCaricoDa");
            private DBCursorField<int> m_IDPersonaEsportata = new DBCursorField<int>("IDPersonaEsportata");
            private DBCursorStringField m_NomePersonaEsportata = new DBCursorStringField("NomePersonaEsportata");
            private DBCursorField<int> m_IDPersonaImportata = new DBCursorField<int>("IDPersonaImportata");
            private DBCursorStringField m_NomePersonaImportata = new DBCursorStringField("NomePersonaImportata");
            private DBCursorField<int> m_IDFinestraLavorazioneEsportata = new DBCursorField<int>("IDFinestraLavorazioneEsportata");
            private DBCursorField<int> m_IDFinestraLavorazioneImportata = new DBCursorField<int>("IDFinestraLavorazioneImportata");
            private DBCursorField<FlagsEsportazione> m_Flags = new DBCursorField<FlagsEsportazione>("Flags");
            private DBCursorField<StatoEsportazione> m_StatoRemoto = new DBCursorField<StatoEsportazione>("StatoRemoto");
            private DBCursorStringField m_DettaglioStatoRemoto = new DBCursorStringField("DettaglioStatoRemoto");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorStringField m_SharedKey = new DBCursorStringField("SharedKey");
            private DBCursorField<DateTime> m_DataUltimoAggiornamento = new DBCursorField<DateTime>("DataUltimoAggiornamento");
            private DBCursorField<DateTime> m_DataEsportazioneOk = new DBCursorField<DateTime>("DataEsportazioneOk");
            private DBCursorField<StatoConfermaEsportazione> m_StatoConferma = new DBCursorField<StatoConfermaEsportazione>("StatoConferma");
            private DBCursorField<int> m_IDOperatoreConferma = new DBCursorField<int>("IDOperatoreConferma");
            private DBCursorStringField m_NomeOperatoreConferma = new DBCursorStringField("NomeOperatoreConferma");
            private DBCursorStringField m_MessaggioEsportazione = new DBCursorStringField("MessaggioEsportazione");
            private DBCursorStringField m_MessaggioImportazione = new DBCursorStringField("MessaggioImportazione");
            private DBCursorStringField m_MessaggioConferma = new DBCursorStringField("MessaggioConferma");
            private DBCursorStringField m_MotivoRichiesta = new DBCursorStringField("MotivoRichiesta");
            private DBCursorField<decimal> m_ImportoRichiesto = new DBCursorField<decimal>("ImportoRichiesto");
            private DBCursorField<decimal> m_RataMassima = new DBCursorField<decimal>("RataMassima");
            private DBCursorField<decimal> m_DurataMassima = new DBCursorField<decimal>("DurataMassima");
            private DBCursorField<decimal> m_RataProposta = new DBCursorField<decimal>("RataProposta");
            private DBCursorField<int> m_DurataProposta = new DBCursorField<int>("DurataProposta");
            private DBCursorField<decimal> m_NettoRicavoProposto = new DBCursorField<decimal>("NettoRicavoProposto");
            private DBCursorField<decimal> m_NettoAllaManoProposto = new DBCursorField<decimal>("NettoAllaManoProposto");
            private DBCursorField<double> m_TANProposto = new DBCursorField<double>("TANProposto");
            private DBCursorField<double> m_TAEGProposto = new DBCursorField<double>("TAEGProposto");
            private DBCursorField<decimal> m_ValoreProvvigioneProposta = new DBCursorField<decimal>("ValoreProvvigioneProposta");

            public CImportExportCursor()
            {
            }

            public DBCursorStringField MessaggioConferma
            {
                get
                {
                    return m_MessaggioConferma;
                }
            }

            public DBCursorStringField MotivoRichiesta
            {
                get
                {
                    return m_MotivoRichiesta;
                }
            }

            public DBCursorField<decimal> ImportoRichiesto
            {
                get
                {
                    return m_ImportoRichiesto;
                }
            }

            public DBCursorField<decimal> RataMassima
            {
                get
                {
                    return m_RataMassima;
                }
            }

            public DBCursorField<decimal> DurataMassima
            {
                get
                {
                    return m_DurataMassima;
                }
            }

            public DBCursorField<decimal> RataProposta
            {
                get
                {
                    return m_RataProposta;
                }
            }

            public DBCursorField<int> DurataProposta
            {
                get
                {
                    return m_DurataProposta;
                }
            }

            public DBCursorField<decimal> NettoRicavoProposto
            {
                get
                {
                    return m_NettoRicavoProposto;
                }
            }

            public DBCursorField<decimal> NettoAllaManoProposto
            {
                get
                {
                    return m_NettoAllaManoProposto;
                }
            }

            public DBCursorField<double> TANProposto
            {
                get
                {
                    return m_TANProposto;
                }
            }

            public DBCursorField<double> TAEGProposto
            {
                get
                {
                    return m_TAEGProposto;
                }
            }

            public DBCursorField<decimal> ValoreProvvigioneProposta
            {
                get
                {
                    return m_ValoreProvvigioneProposta;
                }
            }

            public DBCursorStringField MessaggioEsportazione
            {
                get
                {
                    return m_MessaggioEsportazione;
                }
            }

            public DBCursorStringField MessaggioImportazione
            {
                get
                {
                    return m_MessaggioImportazione;
                }
            }

            public DBCursorField<int> IDOperatoreConferma
            {
                get
                {
                    return m_IDOperatoreConferma;
                }
            }

            public DBCursorStringField NomeOperatoreConferma
            {
                get
                {
                    return m_NomeOperatoreConferma;
                }
            }

            public DBCursorField<StatoConfermaEsportazione> StatoConferma
            {
                get
                {
                    return m_StatoConferma;
                }
            }

            public DBCursorField<DateTime> DataEsportazioneOk
            {
                get
                {
                    return m_DataEsportazioneOk;
                }
            }

            public DBCursorField<bool> Esportazione
            {
                get
                {
                    return m_Esportazione;
                }
            }

            public DBCursorField<DateTime> DataEsportazione
            {
                get
                {
                    return m_DataEsportazione;
                }
            }

            public DBCursorField<int> IDEsportataDa
            {
                get
                {
                    return m_IDEsportataDa;
                }
            }

            public DBCursorStringField NomeEsportataDa
            {
                get
                {
                    return m_NomeEsportataDa;
                }
            }

            public DBCursorField<DateTime> DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }
            }

            public DBCursorField<int> IDPresaInCaricoDa
            {
                get
                {
                    return m_IDPresaInCaricoDa;
                }
            }

            public DBCursorStringField NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
                }
            }

            public DBCursorField<int> IDPersonaEsportata
            {
                get
                {
                    return m_IDPersonaEsportata;
                }
            }

            public DBCursorStringField NomePersonaEsportata
            {
                get
                {
                    return m_NomePersonaEsportata;
                }
            }

            public DBCursorField<int> IDPersonaImportata
            {
                get
                {
                    return m_IDPersonaImportata;
                }
            }

            public DBCursorStringField NomePersonaImportata
            {
                get
                {
                    return m_NomePersonaImportata;
                }
            }

            public DBCursorField<int> IDFinestraLavorazioneEsportata
            {
                get
                {
                    return m_IDFinestraLavorazioneEsportata;
                }
            }

            public DBCursorField<int> IDFinestraLavorazioneImportata
            {
                get
                {
                    return m_IDFinestraLavorazioneImportata;
                }
            }

            public DBCursorField<FlagsEsportazione> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<StatoEsportazione> StatoRemoto
            {
                get
                {
                    return m_StatoRemoto;
                }
            }

            public DBCursorStringField DettaglioStatoRemoto
            {
                get
                {
                    return m_DettaglioStatoRemoto;
                }
            }

            public DBCursorField<int> SourceID
            {
                get
                {
                    return m_SourceID;
                }
            }

            public DBCursorStringField SharedKey
            {
                get
                {
                    return m_SharedKey;
                }
            }

            public DBCursorField<DateTime> DataUltimoAggiornamento
            {
                get
                {
                    return m_DataUltimoAggiornamento;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CImportExport();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDImportExport";
            }

            protected override Sistema.CModule GetModule()
            {
                return ImportExport.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}