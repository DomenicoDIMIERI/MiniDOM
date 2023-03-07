using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoEsportazione : int
        {
            /// <summary>
        /// Record non esportato
        /// </summary>
        /// <remarks></remarks>
            NonEsportato = 0,

            /// <summary>
        /// Esportato verso il sistema remoto
        /// </summary>
        /// <remarks></remarks>
            Esportato = 1,

            /// <summary>
        /// Importato dal sistema remoto
        /// </summary>
        /// <remarks></remarks>
            Importato = 2,

            // ''' <summary>
            // ''' L'esportazione è stata
            // ''' </summary>
            // Revocato = 3


            /// <summary>
        /// Si è verificato un errore
        /// </summary>
        /// <remarks></remarks>
            Errore = 255
        }

        [Flags]
        public enum FlagsEsportazione : int
        {
            None = 0
        }

        public enum StatoConfermaEsportazione : int
        {
            /// <summary>
        /// La richiesta è stata inviata per confronto
        /// </summary>
            Inviato = 0,

            /// <summary>
        /// La richiesta è stata confermata (da elaborare sul sistema remoto)
        /// </summary>
            Confermato = 1,

            /// <summary>
        /// La richiesta è stata revocata del sistema che l'ha inviata
        /// </summary>
            Revocato = 2,

            /// <summary>
        /// La richiesta è stata annullata dal sistema su cui è arrivata
        /// </summary>
            Rifiutata = 3
        }

        /// <summary>
    /// Rappresenta una importazione o una esportazione
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CImportExport 
            : Databases.DBObjectPO
        {
            private bool m_Esportazione;       // Vero se si tratta di un'esportazione
            private DateTime m_DataEsportazione;      // Data e ora dell'importazione/esportazione
            private int m_IDEsportataDa;      // Nel caso delle esportazioni rappresenta l'operatore che ha esportato l'anagrafica.
            [NonSerialized] private Sistema.CUser m_EsportataDa;          // Utente che ha esportato l'anagrafica
            private string m_NomeEsportataDa;     // Nome dell'utente che ha esportato l'anagrafica
            private DateTime? m_DataPresaInCarico;    // Data di presa in carico
            private int m_IDPresaInCaricoDa;  // ID dell'utente che ha preso in carico l'anagrafica
            [NonSerialized] private Sistema.CUser m_PresaInCaricoDa;      // Utente che ha preso in carico l'anagrafica
            private string m_NomePresaInCaricoDa; // Nome dell'utente che ha preso in carico l'anagrafica
            private int m_IDPersonaEsportata; // ID della persona esportata
            [NonSerialized] private Anagrafica.CPersona m_PersonaEsportata;  // Persona esportata
            private string m_NomePersonaEsportata; // Nome della persona esportata
            private int m_IDPersonaImportata; // ID della persona importata
            [NonSerialized] private Anagrafica.CPersona m_PersonaImportata;  // Persona importata
            private string m_NomePersonaImportata; // Nome della persona importata
            private int m_IDFinestraLavorazioneEsportata; // ID della finestra di lavorazione 
            [NonSerialized] private FinestraLavorazione m_FinestraLavorazioneEsportata;
            private int m_IDFinestraLavorazioneImportata;
            [NonSerialized] private FinestraLavorazione m_FinestraLavorazioneImportata;
            private CCollection<CEstinzione> m_AltriPrestiti;
            private CCollection<CRichiestaFinanziamento> m_RichiesteFinanziamento;
            private CCollection<Sistema.CAttachment> m_Documenti;
            private CCollection<CQSPDConsulenza> m_Consulenze;
            private CCollection<CPraticaCQSPD> m_Pratiche;
            private CCollection<CImportExportMatch> m_Corrispondenze;
            private FlagsEsportazione m_Flags;
            private CKeyCollection m_Attributi;
            private StatoEsportazione m_StatoRemoto;
            private string m_DettaglioStatoRemoto;
            private int m_SourceID;
            private CImportExportSource m_Source;
            private string m_SharedKey;
            private DateTime? m_DataUltimoAggiornamento;
            private StatoConfermaEsportazione m_StatoConferma;
            private DateTime? m_DataEsportazioneOk;
            private int m_IDOperatoreConferma;
            [NonSerialized] private Sistema.CUser m_OperatoreConferma;
            private string m_NomeOperatoreConferma;
            private string m_MessaggioEsportazione;
            private string m_MessaggioImportazione;
            private string m_MotivoRichiesta;
            private decimal? m_ImportoRichiesto;
            private decimal? m_RataMassima;
            private int? m_DurataMassima;
            private decimal? m_RataProposta;
            private int? m_DurataProposta;
            private decimal? m_NettoRicavoProposto;
            private decimal? m_NettoAllaManoProposto;
            private double? m_TANProposto;
            private double? m_TAEGProposto;
            private decimal? m_ValoreProvvigioneProposta;
            private string m_MessaggioConferma;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CImportExport()
            {
                m_Esportazione = false;
                m_DataEsportazione = default;
                m_IDEsportataDa = 0;
                m_EsportataDa = null;
                m_NomeEsportataDa = "";
                m_DataPresaInCarico = default;
                m_IDPresaInCaricoDa = 0;
                m_PresaInCaricoDa = null;
                m_NomePresaInCaricoDa = "";
                m_IDPersonaEsportata = 0;
                m_PersonaEsportata = null;
                m_NomePersonaEsportata = "";
                m_IDPersonaImportata = 0;
                m_PersonaImportata = null;
                m_NomePersonaImportata = "";
                m_IDFinestraLavorazioneEsportata = 0;
                m_FinestraLavorazioneEsportata = null;
                m_IDFinestraLavorazioneImportata = 0;
                m_FinestraLavorazioneImportata = null;
                m_AltriPrestiti = null;
                m_RichiesteFinanziamento = null;
                m_RichiesteFinanziamento = null;
                m_Documenti = null;
                m_Consulenze = null;
                m_Pratiche = null;
                m_Corrispondenze = null;
                m_Flags = 0;
                m_Attributi = null;
                m_StatoRemoto = StatoEsportazione.NonEsportato;
                m_SourceID = 0;
                m_Source = null;
                m_DettaglioStatoRemoto = "";
                m_SourceID = 0;
                m_Source = null;
                m_SharedKey = "";
                m_DataUltimoAggiornamento = default;
                m_DataEsportazioneOk = default;
                m_StatoConferma = StatoConfermaEsportazione.Inviato;
                m_IDOperatoreConferma = 0;
                m_OperatoreConferma = null;
                m_NomeOperatoreConferma = "";
                m_MessaggioConferma = "";
                m_MessaggioEsportazione = "";
                m_MessaggioImportazione = "";
                m_MotivoRichiesta = "";
                m_ImportoRichiesto = default;
                m_RataMassima = default;
                m_DurataMassima = default;
                m_RataProposta = default;
                m_DurataProposta = default;
                m_NettoRicavoProposto = default;
                m_NettoAllaManoProposto = default;
                m_TANProposto = default;
                m_TAEGProposto = default;
                m_ValoreProvvigioneProposta = default;
            }

            public decimal? ValoreProvvigioneProposta
            {
                get
                {
                    return m_ValoreProvvigioneProposta;
                }

                set
                {
                    var oldValue = m_ValoreProvvigioneProposta;
                    if (oldValue == value == true)
                        return;
                    m_ValoreProvvigioneProposta = value;
                    DoChanged("ValoreProvvigioneProposta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il motivo della richiesta confronto
        /// </summary>
        /// <returns></returns>
            public string MotivoRichiesta
            {
                get
                {
                    return m_MotivoRichiesta;
                }

                set
                {
                    int oldValue = DMD.Integers.ValueOf(m_MotivoRichiesta);
                    value = DMD.Strings.Trim(value);
                    if (oldValue == DMD.Doubles.CDbl(value))
                        return;
                    m_MotivoRichiesta = value;
                    DoChanged("MotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'importo richiesto dal cliente
        /// </summary>
        /// <returns></returns>
            public decimal? ImportoRichiesto
            {
                get
                {
                    return m_ImportoRichiesto;
                }

                set
                {
                    var oldValue = m_ImportoRichiesto;
                    if (oldValue == value == true)
                        return;
                    m_ImportoRichiesto = value;
                    DoChanged("ImportoRichiesto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la rata massima richiesta dal cliente
        /// </summary>
        /// <returns></returns>
            public decimal? RataMassima
            {
                get
                {
                    return m_RataMassima;
                }

                set
                {
                    var oldValue = m_RataMassima;
                    if (oldValue == value == true)
                        return;
                    m_RataMassima = value;
                    DoChanged("RataMassima", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata massima richiesta dal cliente
        /// </summary>
        /// <returns></returns>
            public int? DurataMassima
            {
                get
                {
                    return m_DurataMassima;
                }

                set
                {
                    var oldValue = m_DurataMassima;
                    if (oldValue == value == true)
                        return;
                    m_DurataMassima = value;
                    DoChanged("DurataMassima", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la rata dell'offerta migliore proposta al cliente
        /// </summary>
        /// <returns></returns>
            public decimal? RataProposta
            {
                get
                {
                    return m_RataProposta;
                }

                set
                {
                    var oldValue = m_RataProposta;
                    if (oldValue == value == true)
                        return;
                    m_RataProposta = value;
                    DoChanged("RataProposta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata dell'offerta migliore proposta al cliente
        /// </summary>
        /// <returns></returns>
            public int? DurataProposta
            {
                get
                {
                    return m_DurataProposta;
                }

                set
                {
                    var oldValue = m_DurataProposta;
                    if (oldValue == value == true)
                        return;
                    m_DurataProposta = value;
                    DoChanged("DurataProposta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il netto ricavo
        /// </summary>
        /// <returns></returns>
            public decimal? NettoRicavoProposto
            {
                get
                {
                    return m_NettoRicavoProposto;
                }

                set
                {
                    var oldValue = m_NettoRicavoProposto;
                    if (oldValue == value == true)
                        return;
                    m_NettoRicavoProposto = value;
                    DoChanged("NettoRicavoProposto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il netto alla mano proposto al cliente
        /// </summary>
        /// <returns></returns>
            public decimal? NettoAllaManoProposto
            {
                get
                {
                    return m_NettoAllaManoProposto;
                }

                set
                {
                    var oldValue = m_NettoAllaManoProposto;
                    if (oldValue == value == true)
                        return;
                    m_NettoAllaManoProposto = value;
                    DoChanged("NettoAllaManoProposto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TAN dell'offerta proposta al cliente
        /// </summary>
        /// <returns></returns>
            public double? TANProposto
            {
                get
                {
                    return m_TANProposto;
                }

                set
                {
                    var oldValue = m_TANProposto;
                    if (oldValue == value == true)
                        return;
                    m_TANProposto = value;
                    DoChanged("TANProposto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TAEG dell'offerta proposta al cliente
        /// </summary>
        /// <returns></returns>
            public double? TAEGProposto
            {
                get
                {
                    return m_TAEGProposto;
                }

                set
                {
                    var oldValue = m_TAEGProposto;
                    if (oldValue == value == true)
                        return;
                    m_TAEGProposto = value;
                    DoChanged("TAEGProposto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il messaggio inviato da chi ha effettuato l'esportazione
        /// </summary>
        /// <returns></returns>
            public string MessaggioEsportazione
            {
                get
                {
                    return m_MessaggioEsportazione;
                }

                set
                {
                    string oldValue = m_MessaggioEsportazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    value = DMD.Strings.Trim(value);
                    m_MessaggioEsportazione = value;
                    DoChanged("MessaggioEsportazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il messaggio inviato da chi ha confermato l'esportazione
        /// </summary>
        /// <returns></returns>
            public string MessaggioConferma
            {
                get
                {
                    return m_MessaggioConferma;
                }

                set
                {
                    string oldValue = m_MessaggioConferma;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    value = DMD.Strings.Trim(value);
                    m_MessaggioConferma = value;
                    DoChanged("MessaggioConferma", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il messaggio inviato da chi ha effettuato l'importazione
        /// </summary>
        /// <returns></returns>
            public string MessaggioImportazione
            {
                get
                {
                    return m_MessaggioImportazione;
                }

                set
                {
                    string oldValue = m_MessaggioImportazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    value = DMD.Strings.Trim(value);
                    m_MessaggioImportazione = value;
                    DoChanged("MessaggioImportazione", value, oldValue);
                }
            }

            public int IDOperatoreConferma
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreConferma, m_IDOperatoreConferma);
                }

                set
                {
                    int oldValue = IDOperatoreConferma;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreConferma = value;
                    m_OperatoreConferma = null;
                    DoChanged("IDOperatoreConferma", value, oldValue);
                }
            }

            public Sistema.CUser OperatoreConferma
            {
                get
                {
                    if (m_OperatoreConferma is null)
                        m_OperatoreConferma = Sistema.Users.GetItemById(m_IDOperatoreConferma);
                    return m_OperatoreConferma;
                }

                set
                {
                    var oldValue = OperatoreConferma;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OperatoreConferma = value;
                    m_IDOperatoreConferma = DBUtils.GetID(value);
                    m_NomeOperatoreConferma = "";
                    if (value is object)
                        m_NomeOperatoreConferma = value.Nominativo;
                    DoChanged("OperatoreConferma", value, oldValue);
                }
            }

            public string NomeOperatoreConferma
            {
                get
                {
                    return m_NomeOperatoreConferma;
                }

                set
                {
                    string oldValue = m_NomeOperatoreConferma;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreConferma = value;
                    DoChanged("NomeOperatoreConferma", value, oldValue);
                }
            }

            public StatoConfermaEsportazione StatoConferma
            {
                get
                {
                    return m_StatoConferma;
                }

                set
                {
                    var oldValue = m_StatoConferma;
                    if (oldValue == value)
                        return;
                    m_StatoConferma = value;
                    DoChanged("StatoConferma", value, oldValue);
                }
            }

            public DateTime? DataEsportazioneOk
            {
                get
                {
                    return m_DataEsportazioneOk;
                }

                set
                {
                    var oldValue = m_DataEsportazioneOk;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEsportazioneOk = value;
                    DoChanged("DataEsportazioneOk", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultimo aggiornamento del record
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataUltimoAggiornamento
            {
                get
                {
                    return m_DataUltimoAggiornamento;
                }

                set
                {
                    var oldValue = m_DataUltimoAggiornamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataUltimoAggiornamento = value;
                    DoChanged("DataUltimoAggiornamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se si tratta di una esportazione o di una importazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Esportazione
            {
                get
                {
                    return m_Esportazione;
                }

                set
                {
                    if (m_Esportazione == value)
                        return;
                    m_Esportazione = value;
                    DoChanged("Esportazione", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di esportazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime DataEsportazione
            {
                get
                {
                    return m_DataEsportazione;
                }

                set
                {
                    var oldValue = m_DataEsportazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEsportazione = value;
                    DoChanged("DataEsportazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha esportato la scheda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDEsportataDa
            {
                get
                {
                    return DBUtils.GetID(m_EsportataDa, m_IDEsportataDa);
                }

                set
                {
                    int oldValue = IDEsportataDa;
                    if (oldValue == value)
                        return;
                    m_IDEsportataDa = value;
                    m_EsportataDa = null;
                    DoChanged("IDEsportataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha esportato la scheda.
        /// Se si tratta di un utente remoto (Esportazione = False) la proprietà restituisce NULL e solo i campi IDEsportatoDa e NomeEsportatoDa identificano l'utente remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser EsportataDa
            {
                get
                {
                    if (!m_Esportazione)
                        return null;
                    if (m_EsportataDa is null)
                        m_EsportataDa = Sistema.Users.GetItemById(m_IDEsportataDa);
                    return m_EsportataDa;
                }

                set
                {
                    var oldValue = EsportataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDEsportataDa = DBUtils.GetID(value);
                    m_EsportataDa = value;
                    m_NomeEsportataDa = "";
                    if (value is object)
                        m_NomeEsportataDa = value.Nominativo;
                    DoChanged("EsportataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha esportato la scheda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeEsportataDa
            {
                get
                {
                    return m_NomeEsportataDa;
                }

                set
                {
                    string oldValue = m_NomeEsportataDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeEsportataDa = value;
                    DoChanged("NomeEsportataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui l'utente remoto ha preso in carico l'esportazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }

                set
                {
                    var oldValue = m_DataPresaInCarico;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPresaInCarico = value;
                    DoChanged("DataPresaInCarico", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha preso in carico l'oggetto.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPresaInCaricoDa
            {
                get
                {
                    return DBUtils.GetID(m_PresaInCaricoDa, m_IDPresaInCaricoDa);
                }

                set
                {
                    int oldValue = IDPresaInCaricoDa;
                    if (oldValue == value)
                        return;
                    m_IDPresaInCaricoDa = value;
                    m_PresaInCaricoDa = null;
                    DoChanged("IDPresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha preso in carico la scheda.
        /// Se si tratta di un utente remoto (Esportazione = True) viene restituito NULL e solo l'ID ed il nome identificano l'utente remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser PresaInCaricoDa
            {
                get
                {
                    if (m_Esportazione)
                        return null;
                    if (m_PresaInCaricoDa is null)
                        m_PresaInCaricoDa = Sistema.Users.GetItemById(m_IDPresaInCaricoDa);
                    return m_PresaInCaricoDa;
                }

                set
                {
                    var oldValue = PresaInCaricoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PresaInCaricoDa = value;
                    m_IDPresaInCaricoDa = DBUtils.GetID(value);
                    m_NomePresaInCaricoDa = "";
                    if (value is object)
                        m_NomePresaInCaricoDa = value.Nominativo;
                    DoChanged("NomePresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha preso in carico la scheda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
                }

                set
                {
                    string oldValue = m_NomePresaInCaricoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePresaInCaricoDa = value;
                    DoChanged("NomePresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona esportata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPersonaEsportata
            {
                get
                {
                    return DBUtils.GetID(m_PersonaEsportata, m_IDPersonaEsportata);
                }

                set
                {
                    int oldValue = IDPersonaEsportata;
                    if (oldValue == value)
                        return;
                    m_IDPersonaEsportata = value;
                    m_PersonaEsportata = null;
                    DoChanged("IDPersonaEsportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la persona esportata.
        /// Se si tratta di una persona remoto (Esportazione = False) la proprietà restituisce NULL e solo l'ID ed il nome identificano la persona
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona PersonaEsportata
            {
                get
                {
                    if (!m_Esportazione)
                        return null;
                    if (m_PersonaEsportata is null)
                        m_PersonaEsportata = Anagrafica.Persone.GetItemById(m_IDPersonaEsportata);
                    return m_PersonaEsportata;
                }

                set
                {
                    var oldValue = m_PersonaEsportata;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PersonaEsportata = value;
                    m_IDPersonaEsportata = DBUtils.GetID(value);
                    m_NomePersonaEsportata = "";
                    if (value is object)
                        m_NomePersonaEsportata = value.Nominativo;
                    DoChanged("PersonaEsportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della persona esportata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomePersonaEsportata
            {
                get
                {
                    return m_NomePersonaEsportata;
                }

                set
                {
                    string oldValue = m_NomePersonaEsportata;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersonaEsportata = value;
                    DoChanged("NomePersonaEsportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona importata.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPersonaImportata
            {
                get
                {
                    return DBUtils.GetID(m_PersonaImportata, m_IDPersonaImportata);
                }

                set
                {
                    int oldValue = IDPersonaImportata;
                    if (oldValue == value)
                        return;
                    m_IDPersonaImportata = value;
                    m_PersonaImportata = null;
                    DoChanged("IDPersonaImportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la persona importata.
        /// Se si tratta di un'esportazione il sistema restituisce NULL e solo l'ID ed il nome identificano la persona sul sistema remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona PersonaImportata
            {
                get
                {
                    if (m_Esportazione)
                        return null;
                    if (m_PersonaImportata is null)
                        m_PersonaImportata = Anagrafica.Persone.GetItemById(m_IDPersonaImportata);
                    return m_PersonaImportata;
                }

                set
                {
                    var oldValue = m_PersonaImportata;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PersonaImportata = value;
                    m_IDPersonaImportata = DBUtils.GetID(value);
                    m_NomePersonaImportata = "";
                    if (value is object)
                        m_NomePersonaImportata = value.Nominativo;
                    DoChanged("PersonaImportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della persona importata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomePersonaImportata
            {
                get
                {
                    return m_NomePersonaImportata;
                }

                set
                {
                    string oldValue = m_NomePersonaImportata;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersonaImportata = value;
                    DoChanged("NomePersonaImportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della finestra di lavorazione esportata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDFinestraLavorazioneEsportata
            {
                get
                {
                    return DBUtils.GetID(m_FinestraLavorazioneEsportata, m_IDFinestraLavorazioneEsportata);
                }

                set
                {
                    int oldValue = IDFinestraLavorazioneEsportata;
                    if (oldValue == value)
                        return;
                    m_IDFinestraLavorazioneEsportata = value;
                    m_FinestraLavorazioneEsportata = null;
                    DoChanged("IDFinestraLavorazioneEsportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la finestra di lavorazione esportata.
        /// Se si tratta di una importazione la proprietà restituisce NULL e solo l'ID identifica la finestra sul sistema remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public FinestraLavorazione FinestraLavorazioneEsportata
            {
                get
                {
                    if (!m_Esportazione)
                        return null;
                    if (m_FinestraLavorazioneEsportata is null)
                        m_FinestraLavorazioneEsportata = FinestreDiLavorazione.GetItemById(m_IDFinestraLavorazioneEsportata);
                    return m_FinestraLavorazioneEsportata;
                }

                set
                {
                    var oldValue = m_FinestraLavorazioneEsportata;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDFinestraLavorazioneEsportata = DBUtils.GetID(value);
                    m_FinestraLavorazioneEsportata = value;
                    DoChanged("FinestraLavorazioneEsportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della finestra di lavorazione importata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDFinestraLavorazioneImportata
            {
                get
                {
                    return DBUtils.GetID(m_FinestraLavorazioneImportata, m_IDFinestraLavorazioneImportata);
                }

                set
                {
                    int oldValue = IDFinestraLavorazioneImportata;
                    if (oldValue == value)
                        return;
                    m_IDFinestraLavorazioneImportata = value;
                    m_FinestraLavorazioneImportata = null;
                    DoChanged("IDFinestraLavorazioneImportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la finestra di lavorazione importata.
        /// In caso si tratti di una esportazione la proporietà restituisce NULL e solo l'ID identifica la finestra remota
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public FinestraLavorazione FinestraLavorazioneImportata
            {
                get
                {
                    if (m_Esportazione)
                        return null;
                    if (m_FinestraLavorazioneImportata is null)
                        m_FinestraLavorazioneImportata = FinestreDiLavorazione.GetItemById(m_IDFinestraLavorazioneImportata);
                    return m_FinestraLavorazioneImportata;
                }

                set
                {
                    var oldValue = m_FinestraLavorazioneImportata;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDFinestraLavorazioneImportata = DBUtils.GetID(value);
                    m_FinestraLavorazioneImportata = value;
                    DoChanged("FinestraLavorazioneImportata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la collezione dei prestiti che sono stati esportati verso il server remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CEstinzione> AltriPrestiti
            {
                get
                {
                    if (m_AltriPrestiti is null)
                        m_AltriPrestiti = new CCollection<CEstinzione>();
                    return m_AltriPrestiti;
                }
            }

            /// <summary>
        /// Restituisce la collezione delle richieste di finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CRichiestaFinanziamento> RichiesteFinanziamento
            {
                get
                {
                    if (m_RichiesteFinanziamento is null)
                        m_RichiesteFinanziamento = new CCollection<CRichiestaFinanziamento>();
                    return m_RichiesteFinanziamento;
                }
            }

            /// <summary>
        /// Restituisce la collezione dei documenti che sono stati esportati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Sistema.CAttachment> Documenti
            {
                get
                {
                    if (m_Documenti is null)
                        m_Documenti = new CCollection<Sistema.CAttachment>();
                    return m_Documenti;
                }
            }

            public CCollection<CQSPDConsulenza> Consulenze
            {
                get
                {
                    if (m_Consulenze is null)
                        m_Consulenze = new CCollection<CQSPDConsulenza>();
                    return m_Consulenze;
                }
            }

            public CCollection<CPraticaCQSPD> Pratiche
            {
                get
                {
                    if (m_Pratiche is null)
                        m_Pratiche = new CCollection<CPraticaCQSPD>();
                    return m_Pratiche;
                }
            }

            /// <summary>
        /// Restituisce la collezione delle corrispondenze tra gli oggetti locali e gli oggetti remoti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CImportExportMatch> Corrispondenze
            {
                get
                {
                    if (m_Corrispondenze is null)
                        m_Corrispondenze = new CCollection<CImportExportMatch>();
                    return m_Corrispondenze;
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public FlagsEsportazione Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la collezione degli attributi aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CKeyCollection Attributi
            {
                get
                {
                    if (m_Attributi is null)
                        m_Attributi = new CKeyCollection();
                    return m_Attributi;
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato dell'esportazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoEsportazione StatoRemoto
            {
                get
                {
                    return m_StatoRemoto;
                }

                set
                {
                    var oldValue = m_StatoRemoto;
                    if (oldValue == value)
                        return;
                    m_StatoRemoto = value;
                    DoChanged("StatoRemoto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che descrive lo stato remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DettaglioStatoRemoto
            {
                get
                {
                    return m_DettaglioStatoRemoto;
                }

                set
                {
                    string oldValue = m_DettaglioStatoRemoto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStatoRemoto = value;
                    DoChanged("DettaglioStatoRemoto", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto usato per importare/esportare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID(m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto usato per importare/esportare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CImportExportSource Source
            {
                get
                {
                    if (m_Source is null)
                        m_Source = ImportExportSources.GetItemById(m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_SourceID = DBUtils.GetID(value);
                    DoChanged("Source", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la chiave generata dal sistema di esportazione e condivisa con il sistema di importazione.
        /// Questa chiave è univoca per ogni record di esportazione (nel sistema sorgente) ed è necessaria per i successivi aggiornamenti ricevuti dal sistema destinazione.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string SharedKey
            {
                get
                {
                    return m_SharedKey;
                }

                set
                {
                    string oldValue = m_SharedKey;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SharedKey = value;
                    DoChanged("SharedKey", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return ImportExport.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDImportExport";
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Esportazione", m_Esportazione);
                writer.Write("DataEsportazione", m_DataEsportazione);
                writer.Write("IDEsportataDa", IDEsportataDa);
                writer.Write("NomeEsportataDa", m_NomeEsportataDa);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.Write("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.Write("IDPersonaEsportata", IDPersonaEsportata);
                writer.Write("NomePersonaEsportata", m_NomePersonaEsportata);
                writer.Write("IDPersonaImportata", IDPersonaImportata);
                writer.Write("NomePersonaImportata", m_NomePersonaImportata);
                writer.Write("IDFinestraLavorazioneEsportata", IDFinestraLavorazioneEsportata);
                writer.Write("IDFinestraLavorazioneImportata", IDFinestraLavorazioneImportata);
                writer.Write("AltriPrestiti", DMD.XML.Utils.Serializer.Serialize(AltriPrestiti));
                writer.Write("RichiesteFinanziamento", DMD.XML.Utils.Serializer.Serialize(RichiesteFinanziamento));
                writer.Write("Documenti", DMD.XML.Utils.Serializer.Serialize(Documenti));
                writer.Write("Consulenze", DMD.XML.Utils.Serializer.Serialize(Consulenze));
                writer.Write("Pratiche", DMD.XML.Utils.Serializer.Serialize(Pratiche));
                writer.Write("Corrispondenze", DMD.XML.Utils.Serializer.Serialize(Corrispondenze));
                writer.Write("Flags", m_Flags);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("StatoRemoto", m_StatoRemoto);
                writer.Write("DettaglioStatoRemoto", m_DettaglioStatoRemoto);
                writer.Write("SourceID", SourceID);
                writer.Write("SharedKey", m_SharedKey);
                writer.Write("DataUltimoAggiornamento", m_DataUltimoAggiornamento);
                writer.Write("DataEsportazioneOk", m_DataEsportazioneOk);
                writer.Write("StatoConferma", m_StatoConferma);
                writer.Write("IDOperatoreConferma", IDOperatoreConferma);
                writer.Write("NomeOperatoreConferma", m_NomeOperatoreConferma);
                writer.Write("MessaggioEsportazione", m_MessaggioEsportazione);
                writer.Write("MessaggioImportazione", m_MessaggioImportazione);
                writer.Write("MotivoRichiesta", m_MotivoRichiesta);
                writer.Write("ImportoRichiesto", m_ImportoRichiesto);
                writer.Write("RataMassima", m_RataMassima);
                writer.Write("DurataMassima", m_DurataMassima);
                writer.Write("RataProposta", m_RataProposta);
                writer.Write("DurataProposta", m_DurataProposta);
                writer.Write("NettoRicavoProposto", m_NettoRicavoProposto);
                writer.Write("NettoAllaManoProposto", m_NettoAllaManoProposto);
                writer.Write("TANProposto", m_TANProposto);
                writer.Write("TAEGProposto", m_TAEGProposto);
                writer.Write("ValoreProvvigioneProposta", m_ValoreProvvigioneProposta);
                writer.Write("MessaggioConferma", m_MessaggioConferma);
                return base.SaveToRecordset(writer);
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Esportazione = reader.Read("Esportazione", m_Esportazione);
                m_DataEsportazione = reader.Read("DataEsportazione",  m_DataEsportazione);
                m_IDEsportataDa = reader.Read("IDEsportataDa",  m_IDEsportataDa);
                m_NomeEsportataDa = reader.Read("NomeEsportataDa",  m_NomeEsportataDa);
                m_DataPresaInCarico = reader.Read("DataPresaInCarico",  m_DataPresaInCarico);
                m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa",  m_IDPresaInCaricoDa);
                m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa",  m_NomePresaInCaricoDa);
                m_IDPersonaEsportata = reader.Read("IDPersonaEsportata",  m_IDPersonaEsportata);
                m_NomePersonaEsportata = reader.Read("NomePersonaEsportata",  m_NomePersonaEsportata);
                m_IDPersonaImportata = reader.Read("IDPersonaImportata",  m_IDPersonaImportata);
                m_NomePersonaImportata = reader.Read("NomePersonaImportata",  m_NomePersonaImportata);
                m_IDFinestraLavorazioneEsportata = reader.Read("IDFinestraLavorazioneEsportata",  m_IDFinestraLavorazioneEsportata);
                m_IDFinestraLavorazioneImportata = reader.Read("IDFinestraLavorazioneImportata",  m_IDFinestraLavorazioneImportata);
                m_Flags = reader.Read("Flags",  m_Flags);
                m_StatoRemoto = reader.Read("StatoRemoto",  m_StatoRemoto);
                m_DettaglioStatoRemoto = reader.Read("DettaglioStatoRemoto",  m_DettaglioStatoRemoto);
                m_SourceID = reader.Read("SourceID",  m_SourceID);
                m_SharedKey = reader.Read("SharedKey",  m_SharedKey);
                m_DataUltimoAggiornamento = reader.Read("DataUltimoAggiornamento",  m_DataUltimoAggiornamento);
                m_DataEsportazioneOk = reader.Read("DataEsportazioneOk",  m_DataEsportazioneOk);
                m_StatoConferma = reader.Read("StatoConferma",  m_StatoConferma);
                m_IDOperatoreConferma = reader.Read("IDOperatoreConferma",  m_IDOperatoreConferma);
                m_NomeOperatoreConferma = reader.Read("NomeOperatoreConferma",  m_NomeOperatoreConferma);
                m_MessaggioEsportazione = reader.Read("MessaggioEsportazione",  m_MessaggioEsportazione);
                m_MessaggioImportazione = reader.Read("MessaggioImportazione",  m_MessaggioImportazione);
                m_MotivoRichiesta = reader.Read("MotivoRichiesta",  m_MotivoRichiesta);
                m_ImportoRichiesto = reader.Read("ImportoRichiesto",  m_ImportoRichiesto);
                m_RataMassima = reader.Read("RataMassima",  m_RataMassima);
                m_DurataMassima = reader.Read("DurataMassima",  m_DurataMassima);
                m_RataProposta = reader.Read("RataProposta",  m_RataProposta);
                m_DurataProposta = reader.Read("DurataProposta",  m_DurataProposta);
                m_NettoRicavoProposto = reader.Read("NettoRicavoProposto",  m_NettoRicavoProposto);
                m_NettoAllaManoProposto = reader.Read("NettoAllaManoProposto",  m_NettoAllaManoProposto);
                m_TANProposto = reader.Read("TANProposto",  m_TANProposto);
                m_TAEGProposto = reader.Read("TAEGProposto",  m_TAEGProposto);
                m_ValoreProvvigioneProposta = reader.Read("ValoreProvvigioneProposta",  m_ValoreProvvigioneProposta);
                m_MessaggioConferma = reader.Read("MessaggioConferma",  m_MessaggioConferma);
                string tmp = "";
                tmp = reader.Read("Attributi",  tmp);
                if (!string.IsNullOrEmpty(tmp))
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                m_AltriPrestiti = new CCollection<CEstinzione>();
                string argvalue = "";
                tmp = reader.Read("AltriPrestiti",  argvalue);
                if (!string.IsNullOrEmpty(tmp))
                    m_AltriPrestiti.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                try
                {
                    m_RichiesteFinanziamento = new CCollection<CRichiestaFinanziamento>();
                    string argvalue1 = "";
                    m_RichiesteFinanziamento.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(reader.Read("RichiesteFinanziamento",  argvalue1)));
                }
                catch (Exception ex)
                {
                    m_RichiesteFinanziamento = null;
                }

                try
                {
                    m_Documenti = new CCollection<Sistema.CAttachment>();
                    string argvalue2 = "";
                    m_Documenti.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Documenti",  argvalue2)));
                }
                catch (Exception ex)
                {
                    m_Documenti = null;
                }

                try
                {
                    m_Consulenze = new CCollection<CQSPDConsulenza>();
                    string argvalue3 = "";
                    m_Consulenze.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Consulenze",  argvalue3)));
                }
                catch (Exception ex)
                {
                    m_Consulenze = null;
                }

                try
                {
                    m_Pratiche = new CCollection<CPraticaCQSPD>();
                    string argvalue4 = "";
                    m_Pratiche.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Pratiche",  argvalue4)));
                }
                catch (Exception ex)
                {
                    m_Pratiche = null;
                }

                try
                {
                    m_Corrispondenze = new CCollection<CImportExportMatch>();
                    string argvalue5 = "";
                    m_Corrispondenze.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Corrispondenze",  argvalue5)));
                }
                catch (Exception ex)
                {
                    m_Corrispondenze = null;
                }

                return base.LoadFromRecordset(reader);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Esportazione", m_Esportazione);
                writer.WriteAttribute("DataEsportazione", m_DataEsportazione);
                writer.WriteAttribute("IDEsportataDa", IDEsportataDa);
                writer.WriteAttribute("NomeEsportataDa", m_NomeEsportataDa);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.WriteAttribute("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.WriteAttribute("IDPersonaEsportata", IDPersonaEsportata);
                writer.WriteAttribute("NomePersonaEsportata", m_NomePersonaEsportata);
                writer.WriteAttribute("IDPersonaImportata", IDPersonaImportata);
                writer.WriteAttribute("NomePersonaImportata", m_NomePersonaImportata);
                writer.WriteAttribute("IDFinestraLavorazioneEsportata", IDFinestraLavorazioneEsportata);
                writer.WriteAttribute("IDFinestraLavorazioneImportata", IDFinestraLavorazioneImportata);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("DettaglioStatoRemoto", m_DettaglioStatoRemoto);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("SharedKey", m_SharedKey);
                writer.WriteAttribute("DataUltimoAggiornamento", m_DataUltimoAggiornamento);
                writer.WriteAttribute("StatoRemoto", (int?)m_StatoRemoto);
                writer.WriteAttribute("DataEsportazioneOk", m_DataEsportazioneOk);
                writer.WriteAttribute("StatoConferma", (int?)m_StatoConferma);
                writer.WriteAttribute("IDOperatoreConferma", IDOperatoreConferma);
                writer.WriteAttribute("NomeOperatoreConferma", m_NomeOperatoreConferma);
                writer.WriteAttribute("MotivoRichiesta", m_MotivoRichiesta);
                writer.WriteAttribute("ImportoRichiesto", m_ImportoRichiesto);
                writer.WriteAttribute("RataMassima", m_RataMassima);
                writer.WriteAttribute("DurataMassima", m_DurataMassima);
                writer.WriteAttribute("RataProposta", m_RataProposta);
                writer.WriteAttribute("DurataProposta", m_DurataProposta);
                writer.WriteAttribute("NettoRicavoProposto", m_NettoRicavoProposto);
                writer.WriteAttribute("NettoAllaManoProposto", m_NettoAllaManoProposto);
                writer.WriteAttribute("TANProposto", m_TANProposto);
                writer.WriteAttribute("TAEGProposto", m_TAEGProposto);
                writer.WriteAttribute("ValoreProvvigioneProposta", m_ValoreProvvigioneProposta);
                base.XMLSerialize(writer);
                writer.WriteTag("AltriPrestiti", AltriPrestiti);
                writer.WriteTag("RichiesteFinanziamento", RichiesteFinanziamento);
                writer.WriteTag("Documenti", Documenti);
                writer.WriteTag("Consulenze", Consulenze);
                writer.WriteTag("Pratiche", Pratiche);
                writer.WriteTag("Corrispondenze", Corrispondenze);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("MessaggioEsportazione", m_MessaggioEsportazione);
                writer.WriteTag("MessaggioImportazione", m_MessaggioImportazione);
                writer.WriteTag("MessaggioConferma", m_MessaggioConferma);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Esportazione":
                        {
                            m_Esportazione = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataEsportazione":
                        {
                            m_DataEsportazione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDEsportataDa":
                        {
                            m_IDEsportataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeEsportataDa":
                        {
                            m_NomeEsportataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPresaInCaricoDa":
                        {
                            m_IDPresaInCaricoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePresaInCaricoDa":
                        {
                            m_NomePresaInCaricoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersonaEsportata":
                        {
                            m_IDPersonaEsportata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersonaEsportata":
                        {
                            m_NomePersonaEsportata = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersonaImportata":
                        {
                            m_IDPersonaImportata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersonaImportata":
                        {
                            m_NomePersonaImportata = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazioneEsportata":
                        {
                            m_IDFinestraLavorazioneEsportata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazioneImportata":
                        {
                            m_IDFinestraLavorazioneImportata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (FlagsEsportazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioStatoRemoto":
                        {
                            m_DettaglioStatoRemoto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SharedKey":
                        {
                            m_SharedKey = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataUltimoAggiornamento":
                        {
                            m_DataUltimoAggiornamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoRemoto":
                        {
                            m_StatoRemoto = (StatoEsportazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AltriPrestiti":
                        {
                            m_AltriPrestiti = new CCollection<CEstinzione>();
                            m_AltriPrestiti.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "RichiesteFinanziamento":
                        {
                            m_RichiesteFinanziamento = new CCollection<CRichiestaFinanziamento>();
                            m_RichiesteFinanziamento.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Documenti":
                        {
                            m_Documenti = new CCollection<Sistema.CAttachment>();
                            m_Documenti.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Consulenze":
                        {
                            m_Consulenze = new CCollection<CQSPDConsulenza>();
                            m_Consulenze.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Pratiche":
                        {
                            m_Pratiche = new CCollection<CPraticaCQSPD>();
                            m_Pratiche.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Corrispondenze":
                        {
                            m_Corrispondenze = new CCollection<CImportExportMatch>();
                            m_Corrispondenze.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "DataEsportazioneOk":
                        {
                            m_DataEsportazioneOk = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoConferma":
                        {
                            m_StatoConferma = (StatoConfermaEsportazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOperatoreConferma":
                        {
                            m_IDOperatoreConferma = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreConferma":
                        {
                            m_NomeOperatoreConferma = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MessaggioEsportazione":
                        {
                            m_MessaggioEsportazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MessaggioImportazione":
                        {
                            m_MessaggioImportazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MessaggioConferma":
                        {
                            m_MessaggioConferma = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoRichiesta":
                        {
                            m_MotivoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ImportoRichiesto":
                        {
                            m_ImportoRichiesto = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RataMassima":
                        {
                            m_RataMassima = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DurataMassima":
                        {
                            m_DurataMassima = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RataProposta":
                        {
                            m_RataProposta = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DurataProposta":
                        {
                            m_DurataProposta = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NettoRicavoProposto":
                        {
                            m_NettoRicavoProposto = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoAllaManoProposto":
                        {
                            m_NettoAllaManoProposto = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TANProposto":
                        {
                            m_TANProposto = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEGProposto":
                        {
                            m_TAEGProposto = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreProvvigioneProposta":
                        {
                            m_ValoreProvvigioneProposta = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public void Esporta()
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                if (m_StatoRemoto != StatoEsportazione.NonEsportato)
                    throw new InvalidOperationException("Già esportato");
                m_SharedKey = Sistema.ASPSecurity.GetRandomKey(128);
                EsportataDa = Sistema.Users.CurrentUser;
                DataEsportazione = DMD.DateUtils.Now();
                Source.Esporta(this);
                m_StatoRemoto = StatoEsportazione.Esportato;
                m_DettaglioStatoRemoto = "Richiesta Confronto";
                Save(true);
            }

            public void ConfermaEsportazione(string message)
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                message = DMD.Strings.Trim(message);
                Source.ConfermaEsportazione(this);
                DataEsportazioneOk = DMD.DateUtils.Now();
                StatoConferma = StatoConfermaEsportazione.Confermato;
                MessaggioConferma = message;
                m_DettaglioStatoRemoto = "Esportazione Confermata";
                OperatoreConferma = Sistema.Users.CurrentUser;
                Save(true);
            }

            public void AnnullaEsportazione(string message)
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                message = DMD.Strings.Trim(message);
                if (string.IsNullOrEmpty(message))
                    throw new ArgumentNullException("E' necessario specificare il motivo dell'annullamento");
                Source.AnnullaEsportazione(this);
                DataEsportazioneOk = DMD.DateUtils.Now();
                StatoConferma = StatoConfermaEsportazione.Revocato;
                MessaggioConferma = message;
                m_DettaglioStatoRemoto = "Esportazione Revocata";
                OperatoreConferma = Sistema.Users.CurrentUser;
                Save(true);
            }

            public void RifiutaCliente(string message)
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                Source.RifiutaCliente(this);
                message = DMD.Strings.Trim(message);
                if (string.IsNullOrEmpty(message))
                    throw new ArgumentNullException("E' necessario specificare il motivo del rifiuto");
                DataEsportazioneOk = DMD.DateUtils.Now();
                StatoConferma = StatoConfermaEsportazione.Rifiutata;
                MessaggioConferma = message;
                m_DettaglioStatoRemoto = "Cliente Rifiutato";
                OperatoreConferma = Sistema.Users.CurrentUser;
                Save(true);
            }

            public void Importa()
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                if (m_StatoRemoto == StatoEsportazione.Importato)
                    throw new InvalidOperationException("Già importato");
                try
                {
                    PresaInCaricoDa = Sistema.Users.CurrentUser;
                    DataPresaInCarico = DMD.DateUtils.Now();
                    Source.Importa(this);
                    m_StatoRemoto = StatoEsportazione.Importato;
                    m_DettaglioStatoRemoto = "Importato";
                }
                catch (Exception ex)
                {
                    m_StatoRemoto = StatoEsportazione.Errore;
                    m_DettaglioStatoRemoto = "Errore di Importazione: " + ex.Message;
                }

                Save(true);
            }

            public void PrendiInCarico()
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                if (m_StatoRemoto != StatoEsportazione.Importato)
                    throw new InvalidOperationException("Non importato");
                try
                {
                    PresaInCaricoDa = Sistema.Users.CurrentUser;
                    DataPresaInCarico = DMD.DateUtils.Now();
                    FinestraLavorazioneImportata = FinestreDiLavorazione.GetFinestraCorrente((Anagrafica.CPersonaFisica)PersonaImportata);
                    Source.Importa(this);
                    m_StatoRemoto = StatoEsportazione.Importato;
                    m_DettaglioStatoRemoto = "Importato";
                }
                catch (Exception ex)
                {
                    m_StatoRemoto = StatoEsportazione.Errore;
                    m_DettaglioStatoRemoto = "Errore di Importazione: " + ex.Message;
                }

                Save(true);
            }

            public void Sincronizza(CKeyCollection oggetti)
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                if (m_StatoRemoto != StatoEsportazione.Importato)
                    throw new InvalidOperationException("Non importato");
                try
                {
                    Source.Sincronizza(this, oggetti);
                    m_DataUltimoAggiornamento = DMD.DateUtils.Now();
                }
                catch (Exception ex)
                {
                    m_StatoRemoto = StatoEsportazione.Errore;
                    m_DettaglioStatoRemoto = "Errore di Sincronizzazione: " + ex.Message;
                }

                Save(true);
            }

            public void Sollecita()
            {
                if (Source is null)
                    throw new ArgumentNullException("Sorgente di esportazione non definita");
                // If (Me.m_StatoRemoto <> StatoEsportazione.Importato) Then Throw New InvalidOperationException("Non importato")
                try
                {
                    Source.Sollecita(this);
                    m_DataUltimoAggiornamento = DMD.DateUtils.Now();
                }
                catch (Exception ex)
                {
                }

                Save(true);
            }
        }
    }
}