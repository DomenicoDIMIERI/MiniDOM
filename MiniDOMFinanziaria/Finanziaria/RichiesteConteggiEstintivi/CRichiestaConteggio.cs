using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoRichiestaConteggio : int
        {
            /// <summary>
        /// Conteggio non chiesto
        /// </summary>
            NonChiesto = 0,

            /// <summary>
        /// Conteggio da richiedere
        /// </summary>
            DaRichiedere = 10,


            /// <summary>
        /// Conteggio richiesto
        /// </summary>
            Richiesto = 20,

            /// <summary>
        /// Conteggio ricevuto
        /// </summary>
            Ricevuto = 30,

            /// <summary>
        /// Conteggio respinto
        /// </summary>
            Respinto = 40,

            /// <summary>
        /// Richiesta conteggio annullata
        /// </summary>
            Annullato = 50
        }

        /// <summary>
    /// Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CRichiestaConteggio : Databases.DBObjectPO
        {
            public event SegnalataEventHandler Segnalata;

            public delegate void SegnalataEventHandler(object sender, ItemEventArgs e);

            public event PresaInCaricoEventHandler PresaInCarico;

            public delegate void PresaInCaricoEventHandler(object sender, ItemEventArgs e);

            private StatoRichiestaConteggio m_StatoRichiestaConteggio;    // Stato della richiesta
            [NonSerialized]
            private Anagrafica.CPersona m_Cliente;           // Cliente per cui è stato richiesto il conteggio
            private int m_IDCliente;                          // ID del cliente per cui è stato chiesto il conteggio
            private string m_NomeCliente;                         // Nome del cliente per cui é stato chiesto il cliente
            private int m_IDEstinzione;   // ID del prestito per cui è stato richiesto il conteggio
            private CEstinzione m_Estinzione; // Prestito per cui è stato richiesto il conteggio
            private int m_IDRichiestaDiFinanziamento;
            [NonSerialized]
            private CRichiestaFinanziamento m_RichiestaDiFinanziamento;
            private int m_NumeroRichiesta; // Numero sequenziale della richiesta
            private int m_IDIstituto;                 // ID del cessionario
            [NonSerialized]
            private Anagrafica.CAzienda m_Istituto;  // Cessionario
            private string m_NomeIstituto;                // Nome del cessionario
            private DateTime? m_DataRichiesta;    // Data della richiesta 
            private int m_IDAgenziaRichiedente;
            [NonSerialized]
            private Anagrafica.CAzienda m_AgenziaRichiedente;
            private string m_NomeAgenziaRichiedente;
            private int m_IDAgente;
            [NonSerialized]
            private Anagrafica.CPersonaFisica m_Agente;
            private string m_NomeAgente;
            private DateTime? m_DataEvasione;
            private int m_IDPratica;
            [NonSerialized]
            private CPraticaCQSPD m_Pratica;
            private string m_NumeroPratica;
            private string m_Descrizione;
            private int m_IDAllegato;                     // ID del modulo di richiesta
            [NonSerialized]
            private Sistema.CAttachment m_Allegato;   // Modulo di richiesta
            private int m_IDDOCConteggio;                     // ID del documento di risposta
            [NonSerialized]
            private Sistema.CAttachment m_DOCConteggio;                   // Documento di risposta
            private int m_PresaInCaricoDaID;
            private string m_PresaInCaricoDaNome;
            [NonSerialized]
            private Sistema.CUser m_PresaInCaricoDa;
            private DateTime? m_DataPresaInCarico;
            private decimal? m_ImportoCE;
            private DateTime? m_DataSegnalazione;
            [NonSerialized]
            private Anagrafica.CPersona m_InviatoDa;
            private int m_InviatoDaID;
            private string m_InviatoDaNome;
            private DateTime? m_InviatoIl;
            [NonSerialized]
            private Sistema.CUser m_RicevutoDa;
            private int m_RicevutoDaID;
            private string m_RicevutoDaNome;
            private DateTime? m_RicevutoIl;
            private string m_MezzoDiInvio;
            private int m_IDFinestraLavorazione;
            [NonSerialized]
            private FinestraLavorazione m_FinestraLavorazione;
            private string m_Esito;
            private int m_IDCessionario;
            [NonSerialized]
            private CCQSPDCessionarioClass m_Cessionario;
            private string m_NomeCessionario;
            private int? m_DurataMesi;
            private decimal? m_ImportoRata;
            private double? m_TAN;
            private double? m_TAEG;
            private DateTime? m_DataDecorrenzaPratica;
            private DateTime? m_UltimaScadenza;
            private string m_ATCPIVA;
            private string m_ATCDescrizione;
            private DateTime? m_DataRichiestaConteggio;
            private int m_ConteggioRichiestoDaID;
            [NonSerialized]
            private Sistema.CUser m_ConteggioRichiestoDa;
            private string m_NoteRichiestaConteggio;
            private DateTime? m_DataEsito;
            private int m_EsitoUserID;
            [NonSerialized]
            private Sistema.CUser m_EsitoUser;
            private string m_NoteEsito;
            private int m_IDDocumentoEsito;
            private Sistema.CAttachment m_DocumentoEsito;

            public CRichiestaConteggio()
            {
                m_IDRichiestaDiFinanziamento = 0;
                m_RichiestaDiFinanziamento = null;
                m_DataRichiesta = default;
                m_IDIstituto = 0;
                m_Istituto = null;
                m_NomeIstituto = "";
                m_IDAgenziaRichiedente = 0;
                m_AgenziaRichiedente = null;
                m_NomeAgenziaRichiedente = "";
                m_IDAgente = 0;
                m_Agente = null;
                m_NomeAgente = "";
                m_DataEvasione = default;
                m_IDPratica = 0;
                m_Pratica = null;
                m_NumeroPratica = "";
                m_Descrizione = "";
                m_IDAllegato = 0;
                m_Allegato = null;
                m_IDDOCConteggio = 0;
                m_DOCConteggio = null;
                m_PresaInCaricoDa = null;
                m_PresaInCaricoDaID = 0;
                m_PresaInCaricoDaNome = "";
                m_DataPresaInCarico = default;
                m_Cliente = null;
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_ImportoCE = default;
                m_DataSegnalazione = default;
                m_InviatoDa = null;
                m_InviatoDaID = 0;
                m_InviatoDaNome = "";
                m_InviatoIl = default;
                m_RicevutoDa = null;
                m_RicevutoDaID = 0;
                m_RicevutoDaNome = "";
                m_RicevutoIl = default;
                m_MezzoDiInvio = "";
                m_IDFinestraLavorazione = 0;
                m_FinestraLavorazione = null;
                m_Esito = "";
                m_IDCessionario = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_DurataMesi = default;
                m_ImportoRata = default;
                m_TAN = default;
                m_TAEG = default;
                m_DataDecorrenzaPratica = default;
                m_UltimaScadenza = default;
                m_ATCPIVA = "";
                m_ATCDescrizione = "";
                m_IDEstinzione = 0;
                m_Estinzione = null;
                m_StatoRichiestaConteggio = StatoRichiestaConteggio.NonChiesto;
                m_DataRichiestaConteggio = default;
                m_ConteggioRichiestoDaID = 0;
                m_ConteggioRichiestoDa = null;
                m_NoteRichiestaConteggio = "";
                m_DataEsito = default;
                m_EsitoUserID = 0;
                m_EsitoUser = null;
                m_NoteEsito = "";
                m_IDDocumentoEsito = 0;
                m_DocumentoEsito = null;
            }

            /// <summary>
        /// Restituisce o imposta lo stato attuale della richiesta conteggio
        /// </summary>
        /// <returns></returns>
            public StatoRichiestaConteggio StatoRichiestaConteggio
            {
                get
                {
                    return m_StatoRichiestaConteggio;
                }

                set
                {
                    var oldValue = m_StatoRichiestaConteggio;
                    if (oldValue == value)
                        return;
                    m_StatoRichiestaConteggio = value;
                    DoChanged("StatoRichiestaConteggio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data della richiesta conteggio
        /// </summary>
        /// <returns></returns>
            public DateTime? DataRichiestaConteggio
            {
                get
                {
                    return m_DataRichiestaConteggio;
                }

                set
                {
                    var oldValue = m_DataRichiestaConteggio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiestaConteggio = value;
                    DoChanged("DataRichiestaConteggio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha richiesto il conteggio
        /// </summary>
        /// <returns></returns>
            public int ConteggioRichiestoDaID
            {
                get
                {
                    return DBUtils.GetID(m_ConteggioRichiestoDa, m_ConteggioRichiestoDaID);
                }

                set
                {
                    int oldValue = ConteggioRichiestoDaID;
                    if (oldValue == value)
                        return;
                    m_ConteggioRichiestoDa = null;
                    m_ConteggioRichiestoDaID = value;
                    DoChanged("ConteggioRichiestoDaID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha richiesto il conteggio
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser ConteggioRichiestoDa
            {
                get
                {
                    if (m_ConteggioRichiestoDa is null)
                        m_ConteggioRichiestoDa = Sistema.Users.GetItemById(m_ConteggioRichiestoDaID);
                    return m_ConteggioRichiestoDa;
                }

                set
                {
                    var oldValue = ConteggioRichiestoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ConteggioRichiestoDa = value;
                    m_ConteggioRichiestoDaID = DBUtils.GetID(value);
                    DoChanged("ConteggioRichiestoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta le note dello stato
        /// </summary>
        /// <returns></returns>
            public string NoteRichiestaConteggio
            {
                get
                {
                    return m_NoteRichiestaConteggio;
                }

                set
                {
                    string oldValue = m_NoteRichiestaConteggio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NoteRichiestaConteggio = value;
                    DoChanged("NoteRichiestaConteggio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'esito
        /// </summary>
        /// <returns></returns>
            public DateTime? DataEsito
            {
                get
                {
                    return m_DataEsito;
                }

                set
                {
                    var oldValue = m_DataEsito;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEsito = value;
                    DoChanged("DataEsito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha impostato l'esito
        /// </summary>
        /// <returns></returns>
            public int EsitoUserID
            {
                get
                {
                    return DBUtils.GetID(m_EsitoUser, m_EsitoUserID);
                }

                set
                {
                    int oldValue = EsitoUserID;
                    if (oldValue == 0)
                        return;
                    m_EsitoUser = null;
                    m_EsitoUserID = value;
                    DoChanged("EsitoUserID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha impostato l'esito
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser EsitoUser
            {
                get
                {
                    if (m_EsitoUser is null)
                        m_EsitoUser = Sistema.Users.GetItemById(m_EsitoUserID);
                    return m_EsitoUser;
                }

                set
                {
                    var oldValue = EsitoUser;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_EsitoUser = value;
                    m_EsitoUserID = DBUtils.GetID(value);
                    DoChanged("EsitoUser", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta le note dell'esito
        /// </summary>
        /// <returns></returns>
            public string NoteEsito
            {
                get
                {
                    return m_NoteEsito;
                }

                set
                {
                    string oldValue = m_NoteEsito;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NoteEsito = value;
                    DoChanged("NoteEsito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del documento del conteggio estintivo
        /// </summary>
        /// <returns></returns>
            public int IDDocumentoEsito
            {
                get
                {
                    return DBUtils.GetID(m_DocumentoEsito, m_IDDocumentoEsito);
                }

                set
                {
                    int oldValue = IDDocumentoEsito;
                    if (oldValue == value)
                        return;
                    m_IDDocumentoEsito = value;
                    m_DocumentoEsito = null;
                    DoChanged("IDDocumentoEsito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il documento del conteggio estintivo
        /// </summary>
        /// <returns></returns>
            public Sistema.CAttachment DocumentoEsito
            {
                get
                {
                    if (m_DocumentoEsito is null)
                        m_DocumentoEsito = Sistema.Attachments.GetItemById(m_IDDocumentoEsito);
                    return m_DocumentoEsito;
                }

                set
                {
                    var oldValue = DocumentoEsito;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DocumentoEsito = value;
                    m_IDDocumentoEsito = DBUtils.GetID(value);
                    DoChanged("DocumentoEsito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto da estinguere
        /// </summary>
        /// <returns></returns>
            public int IDEstinzione
            {
                get
                {
                    return DBUtils.GetID(m_Estinzione, m_IDEstinzione);
                }

                set
                {
                    int oldValue = IDEstinzione;
                    if (oldValue == value)
                        return;
                    m_IDEstinzione = value;
                    m_Estinzione = null;
                    DoChanged("IDEstinzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il prestito da estinguere
        /// </summary>
        /// <returns></returns>
            public CEstinzione Estinzione
            {
                get
                {
                    if (m_Estinzione is null)
                        m_Estinzione = Estinzioni.GetItemById(m_IDEstinzione);
                    return m_Estinzione;
                }

                set
                {
                    var oldValue = m_Estinzione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDEstinzione = DBUtils.GetID(value);
                    m_Estinzione = value;
                    DoChanged("Estinzione", value, oldValue);
                }
            }

            /// <summary>
        /// ID del cessionario
        /// </summary>
        /// <returns></returns>
            public int IDCessionario
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_IDCessionario);
                }

                set
                {
                    int oldValue = IDCessionario;
                    if (oldValue == value)
                        return;
                    m_IDCessionario = value;
                    m_Cessionario = null;
                    DoChanged("IDCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il cessionario
        /// </summary>
        /// <returns></returns>
            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_IDCessionario);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cessionario = value;
                    m_IDCessionario = DBUtils.GetID(value);
                    m_NomeCessionario = "";
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            public string NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            public int? DurataMesi
            {
                get
                {
                    return m_DurataMesi;
                }

                set
                {
                    var oldValue = m_DurataMesi;
                    if (oldValue == value == true)
                        return;
                    m_DurataMesi = value;
                    DoChanged("DurataMesi", value, oldValue);
                }
            }

            public decimal? ImportoRata
            {
                get
                {
                    return m_ImportoRata;
                }

                set
                {
                    var oldValue = m_ImportoRata;
                    if (oldValue == value == true)
                        return;
                    m_ImportoRata = value;
                    DoChanged("ImportoRata", value, oldValue);
                }
            }

            public double? TAN
            {
                get
                {
                    return m_TAN;
                }

                set
                {
                    var oldValue = m_TAN;
                    if (oldValue == value == true)
                        return;
                    m_TAN = value;
                    DoChanged("TAN", value, oldValue);
                }
            }

            public double? TAEG
            {
                get
                {
                    return m_TAEG;
                }

                set
                {
                    var oldValue = m_TAEG;
                    if (oldValue == value == true)
                        return;
                    m_TAEG = value;
                    DoChanged("TAEG", value, oldValue);
                }
            }

            public DateTime? DataDecorrenzaPratica
            {
                get
                {
                    return m_DataDecorrenzaPratica;
                }

                set
                {
                    var oldValue = m_DataDecorrenzaPratica;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataDecorrenzaPratica = value;
                    DoChanged("DataDecorrenzaPratica", value, oldValue);
                }
            }

            public DateTime? UltimaScadenza
            {
                get
                {
                    return m_UltimaScadenza;
                }

                set
                {
                    var oldValue = m_UltimaScadenza;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_UltimaScadenza = value;
                    DoChanged("UltimaScadenza", value, oldValue);
                }
            }

            public string ATCPIVA
            {
                get
                {
                    return m_ATCPIVA;
                }

                set
                {
                    string oldValue = m_ATCPIVA;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_ATCPIVA = value;
                    DoChanged("ATCPIVA", value, oldValue);
                }
            }

            public string ATCDescrizione
            {
                get
                {
                    return m_ATCDescrizione;
                }

                set
                {
                    string oldValue = m_ATCDescrizione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ATCDescrizione = value;
                    DoChanged("ATCDescrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che descrive l'esito del conteggio
        /// </summary>
        /// <returns></returns>
            public string Esito
            {
                get
                {
                    return m_Esito;
                }

                set
                {
                    string oldValue = m_Esito;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Esito = value;
                    DoChanged("Esito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della finestra di lavorazione in cui è avvenuta la richiesta
        /// </summary>
        /// <returns></returns>
            public int IDFinestraLavorazione
            {
                get
                {
                    return DBUtils.GetID(m_FinestraLavorazione, m_IDFinestraLavorazione);
                }

                set
                {
                    int oldValue = IDFinestraLavorazione;
                    if (oldValue == value)
                        return;
                    m_IDFinestraLavorazione = value;
                    m_FinestraLavorazione = null;
                    DoChanged("IDFinestraLavorazione", value, oldValue);
                }
            }

            public FinestraLavorazione FinestraLavorazione
            {
                get
                {
                    if (m_FinestraLavorazione is null)
                        m_FinestraLavorazione = FinestreDiLavorazione.GetItemById(m_IDFinestraLavorazione);
                    return m_FinestraLavorazione;
                }

                set
                {
                    var oldValue = m_FinestraLavorazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_FinestraLavorazione = value;
                    m_IDFinestraLavorazione = DBUtils.GetID(value);
                    DoChanged("FinestraLavorazione", value, oldValue);
                }
            }

            protected internal virtual void SetFinestraLavorazione(FinestraLavorazione value)
            {
                m_FinestraLavorazione = value;
                m_IDFinestraLavorazione = DBUtils.GetID(value);
            }


            /// <summary>
        /// Restituisce o imposta la persona che ha inviato il modulo di richiesta all'agenzia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona InviatoDa
            {
                get
                {
                    if (m_InviatoDa is null)
                        m_InviatoDa = Anagrafica.Persone.GetItemById(m_InviatoDaID);
                    return m_InviatoDa;
                }

                set
                {
                    var oldValue = m_InviatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InviatoDa = value;
                    m_InviatoDaID = DBUtils.GetID(value);
                    if (value is object)
                        m_InviatoDaNome = value.Nominativo;
                    DoChanged("InviatoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona che ha inviato il modulo all'agenzia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int InviatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_InviatoDa, m_InviatoDaID);
                }

                set
                {
                    int oldValue = InviatoDaID;
                    if (oldValue == value)
                        return;
                    m_InviatoDa = null;
                    m_InviatoDaID = value;
                    DoChanged("InviatoDaID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della persoan che ha inviato il modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string InviatoDaNome
            {
                get
                {
                    return m_InviatoDaNome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_InviatoDaNome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_InviatoDaNome = value;
                    DoChanged("InviatoDaNome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di invio del modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? InviatoIl
            {
                get
                {
                    return m_InviatoIl;
                }

                set
                {
                    var oldValue = m_InviatoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_InviatoIl = value;
                    DoChanged("InviatoIl", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha registrato il modulo nel sistema
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser RicevutoDa
            {
                get
                {
                    if (m_RicevutoDa is null)
                        m_RicevutoDa = Sistema.Users.GetItemById(m_RicevutoDaID);
                    return m_RicevutoDa;
                }

                set
                {
                    var oldValue = RicevutoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RicevutoDa = value;
                    m_RicevutoDaID = DBUtils.GetID(value);
                    if (value is object)
                        m_RicevutoDaNome = value.Nominativo;
                    DoChanged("RicevutoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha registrato il modulo nel sistema
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int RicevutoDaID
            {
                get
                {
                    return DBUtils.GetID(m_RicevutoDa, m_RicevutoDaID);
                }

                set
                {
                    int oldValue = RicevutoDaID;
                    if (oldValue == value)
                        return;
                    m_RicevutoDa = null;
                    m_RicevutoDaID = value;
                    DoChanged("RicevutoDaID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha registrato il modulo nel sistema
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string RicevutoDaNome
            {
                get
                {
                    return m_RicevutoDaNome;
                }

                set
                {
                    string oldValue = m_RicevutoDaNome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RicevutoDaNome = value;
                    DoChanged("RicevutoDaNome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di registrazione del modulo nel sistema
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? RicevutoIl
            {
                get
                {
                    return m_RicevutoIl;
                }

                set
                {
                    var oldValue = m_RicevutoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_RicevutoIl = value;
                    DoChanged("RicevutoIl", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del mezzo utilizzato per inviare il modulo (da Prestitalia all'agenzia, es. e-mail)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MezzoDiInvio
            {
                get
                {
                    return m_MezzoDiInvio;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MezzoDiInvio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MezzoDiInvio = value;
                    DoChanged("MezzoDiInvio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore da estinguere alla data di rilascio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ImportoCE
            {
                get
                {
                    return m_ImportoCE;
                }

                set
                {
                    var oldValue = m_ImportoCE;
                    if (oldValue == value == true)
                        return;
                    m_ImportoCE = value;
                    DoChanged("ImportoCE", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la persona per cui viene fatta la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            protected internal virtual void SetCliente(Anagrafica.CPersona value)
            {
                m_Cliente = value;
                m_IDCliente = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona per cui viene fatta la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del cliente che ha effettuato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    string oldValue = m_NomeCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica (interna) per cui è stato richiesto il conteggio estintivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPratica
            {
                get
                {
                    return DBUtils.GetID(m_Pratica, m_IDPratica);
                }

                set
                {
                    int oldValue = IDPratica;
                    if (oldValue == value)
                        return;
                    m_IDPratica = value;
                    m_Pratica = null;
                    DoChanged("IDPratica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la pratica per cui è stato chiesto il conteggio estintivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD Pratica
            {
                get
                {
                    if (m_Pratica is null)
                        m_Pratica = Pratiche.GetItemById(m_IDPratica);
                    return m_Pratica;
                }

                set
                {
                    var oldValue = m_Pratica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                    DoChanged("Pratica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero esterno della pratica per cui è stato richiesto il conteggio estintivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NumeroPratica
            {
                get
                {
                    return m_NumeroPratica;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroPratica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroPratica = value;
                    DoChanged("NumeroPratica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che descrive la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'allegato contenente il modulo di richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAllegato
            {
                get
                {
                    return DBUtils.GetID(m_Allegato, m_IDAllegato);
                }

                set
                {
                    int oldValue = IDAllegato;
                    if (oldValue == value)
                        return;
                    m_IDAllegato = value;
                    m_Allegato = null;
                    DoChanged("IDAllegato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'allegato contenente il modulo di richiesta del conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CAttachment Allegato
            {
                get
                {
                    if (m_Allegato is null)
                        m_Allegato = Sistema.Attachments.GetItemById(m_IDAllegato);
                    return m_Allegato;
                }

                set
                {
                    var oldValue = m_Allegato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Allegato = value;
                    m_IDAllegato = DBUtils.GetID(value);
                    DoChanged("Allegato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'documento contenente il conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDDOCConteggio
            {
                get
                {
                    return DBUtils.GetID(m_DOCConteggio, m_IDDOCConteggio);
                }

                set
                {
                    int oldValue = IDDOCConteggio;
                    if (oldValue == value)
                        return;
                    m_IDDOCConteggio = value;
                    m_DOCConteggio = null;
                    DoChanged("IDDOCConteggio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il documento contenente il conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CAttachment DOCConteggio
            {
                get
                {
                    if (m_DOCConteggio is null)
                        m_DOCConteggio = Sistema.Attachments.GetItemById(m_IDDOCConteggio);
                    return m_DOCConteggio;
                }

                set
                {
                    var oldValue = m_DOCConteggio;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DOCConteggio = value;
                    m_IDDOCConteggio = DBUtils.GetID(value);
                    DoChanged("DOCConteggio", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta l'utente che ha preso in carico la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser PresaInCaricoDa
            {
                get
                {
                    if (m_PresaInCaricoDa is null)
                        m_PresaInCaricoDa = Sistema.Users.GetItemById(m_PresaInCaricoDaID);
                    return m_PresaInCaricoDa;
                }

                set
                {
                    var oldValue = PresaInCaricoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PresaInCaricoDa = value;
                    m_PresaInCaricoDaID = DBUtils.GetID(value);
                    if (value is object)
                        m_PresaInCaricoDaNome = value.Nominativo;
                    DoChanged("PresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta L'ID dell'operatore che ha preso in carico la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int PresaInCaricoDaID
            {
                get
                {
                    return DBUtils.GetID(m_PresaInCaricoDa, m_PresaInCaricoDaID);
                }

                set
                {
                    int oldValue = PresaInCaricoDaID;
                    if (oldValue == value)
                        return;
                    m_PresaInCaricoDaID = value;
                    m_PresaInCaricoDa = null;
                    DoChanged("PresaInCaricoDaID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha preso in carico la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PresaInCaricoDaNome
            {
                get
                {
                    return m_PresaInCaricoDaNome;
                }

                set
                {
                    string oldValue = m_PresaInCaricoDaNome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PresaInCaricoDaNome = value;
                    DoChanged("PresaInCaricoDaNome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui la richiesta è stata presa in carico
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
        /// Restituisdce o imposta l'ID della richiesta di finanziamento in cui è stato registrato questo conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDRichiestaDiFinanziamento
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaDiFinanziamento, m_IDRichiestaDiFinanziamento);
                }

                set
                {
                    int oldValue = IDRichiestaDiFinanziamento;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaDiFinanziamento = value;
                    m_RichiestaDiFinanziamento = null;
                    DoChanged("IDRichiestaDiFinanziamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la richiesta di finanziamento in cui è stato registrato questo conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiestaFinanziamento RichiestaDiFinanziamento
            {
                get
                {
                    if (m_RichiestaDiFinanziamento is null)
                        m_RichiestaDiFinanziamento = RichiesteFinanziamento.GetItemById(m_IDRichiestaDiFinanziamento);
                    return m_RichiestaDiFinanziamento;
                }

                set
                {
                    var oldValue = m_RichiestaDiFinanziamento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaDiFinanziamento = value;
                    m_IDRichiestaDiFinanziamento = DBUtils.GetID(value);
                    DoChanged("RichiestaDiFinanziamento", value, oldValue);
                }
            }

            protected internal void SetRichiestaDiFinanziamento(CRichiestaFinanziamento value)
            {
                m_RichiestaDiFinanziamento = value;
                m_IDRichiestaDiFinanziamento = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta la data e l'ora della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }

                set
                {
                    var oldValue = m_DataRichiesta;
                    if (oldValue == value == true)
                        return;
                    m_DataRichiesta = value;
                    DoChanged("DataRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta L'ID dell'istituto presso cui è stata effettuata la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDIstituto
            {
                get
                {
                    return DBUtils.GetID(m_Istituto, m_IDIstituto);
                }

                set
                {
                    int oldValue = IDIstituto;
                    if (oldValue == value)
                        return;
                    m_IDIstituto = value;
                    m_Istituto = null;
                    DoChanged("IDIstituto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'istituto presso cui è stato richiesto questo conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CAzienda Istituto
            {
                get
                {
                    if (m_Istituto is null)
                        m_Istituto = (Anagrafica.CAzienda)Anagrafica.Persone.GetItemById(m_IDIstituto);
                    return m_Istituto;
                }

                set
                {
                    var oldValue = m_Istituto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Istituto = value;
                    m_IDIstituto = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeIstituto = value.Nominativo;
                    DoChanged("Istituto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'istituto presso cui è stato richiesto questo conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeIstituto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIstituto = value;
                    DoChanged("NomeIstituto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'agenzia che ha richiesto il conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAgenziaRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_AgenziaRichiedente, m_IDAgenziaRichiedente);
                }

                set
                {
                    int oldValue = IDAgenziaRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDAgenziaRichiedente = value;
                    m_AgenziaRichiedente = null;
                    DoChanged("IDAgenziaRichiedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restitusce o imposta l'agenzia che ha richiesto questo conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CAzienda AgenziaRichiedente
            {
                get
                {
                    if (m_AgenziaRichiedente is null)
                        m_AgenziaRichiedente = Anagrafica.Aziende.GetItemById(m_IDAgenziaRichiedente);
                    return m_AgenziaRichiedente;
                }

                set
                {
                    var oldValue = m_AgenziaRichiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AgenziaRichiedente = value;
                    m_IDAgenziaRichiedente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAgenziaRichiedente = value.Nominativo;
                    DoChanged("AgenziaRichiedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'agenzia richiedente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeAgenziaRichiedente
            {
                get
                {
                    return m_NomeAgenziaRichiedente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgenziaRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgenziaRichiedente = value;
                    DoChanged("NomeAgenziaRichiedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'agente che ha richiesto il conteggio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAgente
            {
                get
                {
                    return DBUtils.GetID(m_Agente, m_IDAgente);
                }

                set
                {
                    int oldValue = IDAgente;
                    if (oldValue == value)
                        return;
                    m_IDAgente = value;
                    m_Agente = null;
                    DoChanged("IDAgente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersonaFisica Agente
            {
                get
                {
                    if (m_Agente is null)
                        m_Agente = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDAgente);
                    return m_Agente;
                }

                set
                {
                    var oldValue = m_Agente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Agente = value;
                    m_IDAgente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAgente = value.Nominativo;
                    DoChanged("Agente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'agente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeAgente
            {
                get
                {
                    return m_NomeAgente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgente = value;
                    DoChanged("NomeAgente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di evasione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataEvasione
            {
                get
                {
                    return m_DataEvasione;
                }

                set
                {
                    var oldValue = m_DataEvasione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataEvasione = value;
                    DoChanged("DataEvasione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui questo oggetto è stato segnalato agli operatori
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataSegnalazione
            {
                get
                {
                    return m_DataSegnalazione;
                }

                set
                {
                    var oldValue = m_DataSegnalazione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataSegnalazione = value;
                    DoChanged("DataSegnalazione", value, oldValue);
                }
            }

            public void Segnala()
            {
                m_DataSegnalazione = DMD.DateUtils.Now();
                Save(true);
                var e = new ItemEventArgs(this);
                doOnSegnalata(e);
            }

            private void doOnSegnalata(ItemEventArgs e)
            {
                OnSegnalata(e);
                RichiesteConteggi.doOnSegnalata(e);
            }

            protected virtual void OnSegnalata(ItemEventArgs e)
            {
                Segnalata?.Invoke(this, e);
            }

            public void PrendiInCarico()
            {
                PresaInCaricoDa = Sistema.Users.CurrentUser;
                DataPresaInCarico = DMD.DateUtils.Now();
                Save(true);
                var e = new ItemEventArgs(this);
                doOnPresaInCarico(e);
            }

            private void doOnPresaInCarico(ItemEventArgs e)
            {
                OnPresaInCarico(e);
                RichiesteConteggi.doOnPresaInCarico(e);
            }

            protected virtual void OnPresaInCarico(ItemEventArgs e)
            {
                PresaInCarico?.Invoke(this, e);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return RichiesteConteggi.Module;
            }

            public override string GetTableName()
            {
                return "tbl_RichiesteFinanziamentiC";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaF", this.m_IDRichiestaDiFinanziamento);
                m_DataRichiesta = reader.Read("DataRichiesta", this.m_DataRichiesta);
                m_IDIstituto = reader.Read("IDIstituto", this.m_IDIstituto);
                m_NomeIstituto = reader.Read("NomeIstituto", this.m_NomeIstituto);
                m_IDAgenziaRichiedente = reader.Read("IDAgenziaR", this.m_IDAgenziaRichiedente);
                m_NomeAgenziaRichiedente = reader.Read("NomeAgenziaR", this.m_NomeAgenziaRichiedente);
                m_IDAgente = reader.Read("IDAgente", this.m_IDAgente);
                m_NomeAgente = reader.Read("NomeAgente", this.m_NomeAgente);
                m_DataEvasione = reader.Read("DataEvasione", this.m_DataEvasione);
                m_ImportoCE = reader.Read("ImportoCE", this.m_ImportoCE);
                m_IDPratica = reader.Read("IDPratica", this.m_IDPratica);
                m_NumeroPratica = reader.Read("NumeroPratica", this.m_NumeroPratica);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_IDAllegato = reader.Read("IDAllegato", this.m_IDAllegato);
                m_IDDOCConteggio = reader.Read("IDDOCConteggio", this.m_IDDOCConteggio);
                m_PresaInCaricoDaID = reader.Read("PresaInCaricoDaID", this.m_PresaInCaricoDaID);
                m_PresaInCaricoDaNome = reader.Read("PresaInCaricoDaNome", this.m_PresaInCaricoDaNome);
                m_DataPresaInCarico = reader.Read("DataPresaInCarico", this.m_DataPresaInCarico);
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_DataSegnalazione = reader.Read("DataSegnalazione", this.m_DataSegnalazione);
                m_InviatoDaID = reader.Read("InviatoDaID", this.m_InviatoDaID);
                m_InviatoDaNome = reader.Read("InviatoDaNome", this.m_InviatoDaNome);
                m_InviatoIl = reader.Read("InviatoIl", this.m_InviatoIl);
                m_RicevutoDaID = reader.Read("RicevutoDaID", this.m_RicevutoDaID);
                m_RicevutoDaNome = reader.Read("RicevutoDaNome", this.m_RicevutoDaNome);
                m_RicevutoIl = reader.Read("RicevutoIl", this.m_RicevutoIl);
                m_MezzoDiInvio = reader.Read("MezzoDiInvio", this.m_MezzoDiInvio);
                m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", this.m_IDFinestraLavorazione);
                m_Esito = reader.Read("Esito", this.m_Esito);
                m_IDCessionario = reader.Read("IDCessionario", this.m_IDCessionario);
                m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                m_DurataMesi = reader.Read("DurataMesi", this.m_DurataMesi);
                m_ImportoRata = reader.Read("ImportoRata", this.m_ImportoRata);
                m_TAN = reader.Read("TAN", this.m_TAN);
                m_TAEG = reader.Read("TAEG", this.m_TAEG);
                m_DataDecorrenzaPratica = reader.Read("DataDecorrenzaPratica", this.m_DataDecorrenzaPratica);
                m_UltimaScadenza = reader.Read("UltimaScadenza", this.m_UltimaScadenza);
                m_ATCPIVA = reader.Read("ATCPIVA", this.m_ATCPIVA);
                m_ATCDescrizione = reader.Read("ATCDescrizione", this.m_ATCDescrizione);
                m_IDEstinzione = reader.Read("IDEstinzione", this.m_IDEstinzione);
                m_StatoRichiestaConteggio = reader.Read("StatoRichiestaConteggio", this.m_StatoRichiestaConteggio);
                m_DataRichiestaConteggio = reader.Read("DataRichiestaConteggio", this.m_DataRichiestaConteggio);
                m_ConteggioRichiestoDaID = reader.Read("ConteggioRichiestoDaID", this.m_ConteggioRichiestoDaID);
                m_NoteRichiestaConteggio = reader.Read("NoteRichiestaConteggio", this.m_NoteRichiestaConteggio);
                m_DataEsito = reader.Read("DataEsito", this.m_DataEsito);
                m_EsitoUserID = reader.Read("EsitoUserID", this.m_EsitoUserID);
                m_NoteEsito = reader.Read("NoteEsito", this.m_NoteEsito);
                m_IDDocumentoEsito = reader.Read("IDDocumentoEsito", this.m_IDDocumentoEsito);
                m_StatoRichiestaConteggio = reader.Read("StatoRichiestaConteggio", this.m_StatoRichiestaConteggio);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDRichiestaF", IDRichiestaDiFinanziamento);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("IDIstituto", IDIstituto);
                writer.Write("NomeIstituto", m_NomeIstituto);
                writer.Write("IDAgenziaR", IDAgenziaRichiedente);
                writer.Write("NomeAgenziaR", m_NomeAgenziaRichiedente);
                writer.Write("IDAgente", IDAgente);
                writer.Write("NomeAgente", m_NomeAgente);
                writer.Write("DataEvasione", m_DataEvasione);
                writer.Write("ImportoCE", m_ImportoCE);
                writer.Write("IDPratica", IDPratica);
                writer.Write("NumeroPratica", m_NumeroPratica);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDAllegato", IDAllegato);
                writer.Write("IDDOCConteggio", IDDOCConteggio);
                writer.Write("PresaInCaricoDaID", PresaInCaricoDaID);
                writer.Write("PresaInCaricoDaNome", m_PresaInCaricoDaNome);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("DataSegnalazione", m_DataSegnalazione);
                writer.Write("InviatoDaID", InviatoDaID);
                writer.Write("InviatoDaNome", m_InviatoDaNome);
                writer.Write("InviatoIl", m_InviatoIl);
                writer.Write("RicevutoDaID", RicevutoDaID);
                writer.Write("RicevutoDaNome", m_RicevutoDaNome);
                writer.Write("RicevutoIl", m_RicevutoIl);
                writer.Write("MezzoDiInvio", m_MezzoDiInvio);
                writer.Write("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.Write("Esito", m_Esito);
                writer.Write("IDCessionario", IDCessionario);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("DurataMesi", m_DurataMesi);
                writer.Write("ImportoRata", m_ImportoRata);
                writer.Write("TAN", m_TAN);
                writer.Write("TAEG", m_TAEG);
                writer.Write("DataDecorrenzaPratica", m_DataDecorrenzaPratica);
                writer.Write("UltimaScadenza", m_UltimaScadenza);
                writer.Write("ATCPIVA", m_ATCPIVA);
                writer.Write("ATCDescrizione", m_ATCDescrizione);
                writer.Write("IDEstinzione", IDEstinzione);
                writer.Write("StatoRichiestaConteggio", m_StatoRichiestaConteggio);
                // writer.Write("DataRichiestaConteggio", Me.m_DataRichiestaConteggio)
                // writer.Write("ConteggioRichiestoDaID", Me.ConteggioRichiestoDaID)
                // writer.Write("NoteRichiestaConteggio", Me.m_NoteRichiestaConteggio)

                // writer.Write("DataEsito", Me.m_DataEsito)
                // writer.Write("EsitoUserID", Me.EsitoUserID)
                // writer.Write("NoteEsito", Me.m_NoteEsito)

                // writer.Write("IDDocumentoEsito", Me.IDDocumentoEsito)


                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDRichiestaF", IDRichiestaDiFinanziamento);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("IDIstituto", IDIstituto);
                writer.WriteAttribute("NomeIstituto", m_NomeIstituto);
                writer.WriteAttribute("IDAgenziaR", IDAgenziaRichiedente);
                writer.WriteAttribute("NomeAgenziaR", m_NomeAgenziaRichiedente);
                writer.WriteAttribute("IDAgente", IDAgente);
                writer.WriteAttribute("NomeAgente", m_NomeAgente);
                writer.WriteAttribute("DataEvasione", m_DataEvasione);
                writer.WriteAttribute("ImportoCE", m_ImportoCE);
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("NumeroPratica", m_NumeroPratica);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("IDAllegato", IDAllegato);
                writer.WriteAttribute("PresaInCaricoDaID", PresaInCaricoDaID);
                writer.WriteAttribute("PresaInCaricoDaNome", m_PresaInCaricoDaNome);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("DataSegnalazione", m_DataSegnalazione);
                writer.WriteAttribute("InviatoDaID", InviatoDaID);
                writer.WriteAttribute("InviatoDaNome", m_InviatoDaNome);
                writer.WriteAttribute("InviatoIl", m_InviatoIl);
                writer.WriteAttribute("RicevutoDaID", RicevutoDaID);
                writer.WriteAttribute("RicevutoDaNome", m_RicevutoDaNome);
                writer.WriteAttribute("RicevutoIl", m_RicevutoIl);
                writer.WriteAttribute("MezzoDiInvio", m_MezzoDiInvio);
                writer.WriteAttribute("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.WriteAttribute("Esito", m_Esito);
                writer.WriteAttribute("IDCessionario", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("DurataMesi", m_DurataMesi);
                writer.WriteAttribute("ImportoRata", m_ImportoRata);
                writer.WriteAttribute("TAN", m_TAN);
                writer.WriteAttribute("TAEG", m_TAEG);
                writer.WriteAttribute("DataDecorrenzaPratica", m_DataDecorrenzaPratica);
                writer.WriteAttribute("UltimaScadenza", m_UltimaScadenza);
                writer.WriteAttribute("ATCPIVA", m_ATCPIVA);
                writer.WriteAttribute("ATCDescrizione", m_ATCDescrizione);
                writer.WriteAttribute("IDEstinzione", IDEstinzione);
                writer.WriteAttribute("IDDOCConteggio", IDDOCConteggio);
                writer.WriteAttribute("StatoRichiestaConteggio", (int?)m_StatoRichiestaConteggio);
                writer.WriteAttribute("DataRichiestaConteggio", m_DataRichiestaConteggio);
                writer.WriteAttribute("ConteggioRichiestoDaID", ConteggioRichiestoDaID);
                writer.WriteAttribute("DataEsito", m_DataEsito);
                writer.WriteAttribute("EsitoUserID", EsitoUserID);
                writer.WriteAttribute("IDDocumentoEsito", IDDocumentoEsito);
                base.XMLSerialize(writer);
                writer.WriteTag("NoteRichiestaConteggio", m_NoteRichiestaConteggio);
                writer.WriteTag("NoteEsito", m_NoteEsito);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDRichiestaF":
                        {
                            m_IDRichiestaDiFinanziamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDIstituto":
                        {
                            m_IDIstituto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeIstituto":
                        {
                            m_NomeIstituto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgenziaR":
                        {
                            m_IDAgenziaRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgenziaR":
                        {
                            m_NomeAgenziaRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgente":
                        {
                            m_IDAgente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgente":
                        {
                            m_NomeAgente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataEvasione":
                        {
                            m_DataEvasione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroPratica":
                        {
                            m_NumeroPratica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAllegato":
                        {
                            m_IDAllegato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PresaInCaricoDaID":
                        {
                            m_PresaInCaricoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PresaInCaricoDaNome":
                        {
                            m_PresaInCaricoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ImportoCE":
                        {
                            m_ImportoCE = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataSegnalazione":
                        {
                            m_DataSegnalazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "InviatoDaID":
                        {
                            m_InviatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "InviatoDaNome":
                        {
                            m_InviatoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "InviatoIl":
                        {
                            m_InviatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RicevutoDaID":
                        {
                            m_RicevutoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RicevutoDaNome":
                        {
                            m_RicevutoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RicevutoIl":
                        {
                            m_RicevutoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MezzoDiInvio":
                        {
                            m_MezzoDiInvio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazione":
                        {
                            m_IDFinestraLavorazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            m_Esito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCessionario":
                        {
                            m_IDCessionario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DurataMesi":
                        {
                            m_DurataMesi = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ImportoRata":
                        {
                            m_ImportoRata = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAN":
                        {
                            m_TAN = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            m_TAEG = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataDecorrenzaPratica":
                        {
                            m_DataDecorrenzaPratica = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "UltimaScadenza":
                        {
                            m_UltimaScadenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ATCPIVA":
                        {
                            m_ATCPIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ATCDescrizione":
                        {
                            m_ATCDescrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDEstinzione":
                        {
                            m_IDEstinzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDOCConteggio":
                        {
                            m_IDDOCConteggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoRichiestaConteggio":
                        {
                            m_StatoRichiestaConteggio = (StatoRichiestaConteggio)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRichiestaConteggio":
                        {
                            m_DataRichiestaConteggio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ConteggioRichiestoDaID":
                        {
                            m_ConteggioRichiestoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NoteRichiestaConteggio":
                        {
                            m_NoteRichiestaConteggio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataEsito":
                        {
                            m_DataEsito = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "EsitoUserID":
                        {
                            m_EsitoUserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NoteEsito":
                        {
                            m_NoteEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDocumentoEsito":
                        {
                            m_IDDocumentoEsito = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                return "Richiesta di Conteggio Estintivo, Cliente: " + m_NomeCliente + ", Pratica: " + m_NumeroPratica;
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                RichiesteConteggi.doItemCreated(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                RichiesteConteggi.doItemDeleted(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                RichiesteConteggi.doItemModified(new ItemEventArgs(this));
            }
        }
    }
}