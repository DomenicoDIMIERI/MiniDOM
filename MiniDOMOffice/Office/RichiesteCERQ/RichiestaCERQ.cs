using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Stato di una richiesta di generica
        /// </summary>
        public enum StatoRichiestaCERQ : int
        {
            /// <summary>
            /// La quota non è ancora stata richiesta
            /// </summary>
            /// <remarks></remarks>
            DA_RICHIEDERE = 0,

            /// <summary>
            /// La quota è stata richiesta
            /// </summary>
            /// <remarks></remarks>
            RICHIESTA = 1,

            /// <summary>
            /// La quota è stata ritirata
            /// </summary>
            /// <remarks></remarks>
            RITIRATA = 3,

            /// <summary>
            /// La richiesta è stata annullata dall'operatore
            /// </summary>
            /// <remarks></remarks>
            ANNULLATA = 4,

            /// <summary>
            /// La richiesta è stata rifiutata dall'amministrazione
            /// </summary>
            /// <remarks></remarks>
            RIFIUTATA = 2
        }

        /// <summary>
        /// Rappresenta una richiesta di conteggio estintivo/rimborso quote
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RichiestaCERQ 
            : minidom.Databases.DBObjectPO, ICloneable
        {

            /// <summary>
            /// Evento generato quando la richiesta viene inviata
            /// </summary>            
            /// <remarks></remarks>
            public event ItemEventHandler<RichiestaCERQ> RichiestaStatusChanged;
              
            private DateTime m_Data;                      // Data ed ora della richiesta
            [NonSerialized] private Sistema.CUser m_Operatore;                // Utente che ha effettuato la richiesta
            private int m_IDOperatore;            // ID dell'operatore che ha effettuato la richiesta
            private string m_NomeOperatore;           // Nome dell'operatore che ha effettuato la richiesta
            [NonSerialized] private Anagrafica.CPersonaFisica m_Cliente;         // Persona fisica per cui si è richiesto il conteggio/quota
            private int m_IDCliente;              // ID della persona fisica per cui si è richiesto il conteggio/quota
            private string m_NomeCliente;             // Nome della persona fisica per cui si è richiesto il conteggio/quota
            private string m_TipoRichiesta;           // Quota cedibile o conteggio estintivo
            [NonSerialized] private Anagrafica.CAzienda m_AziendaRichiedente;    // Azienda che ha fatto la richiesta
            private int m_IDAziendaRichiedente;   // ID dell'azienda che ha effettuato la richiesta
            private string m_NomeAziendaRichiedente;  // Nome dell'azienda che ha effettuato la richiesta
            [NonSerialized] private Anagrafica.CAzienda m_Amministrazione;       // Amministrazione a cui si è fatta la richiesta
            private int m_IDAmministrazione;      // ID dell'amministrazione a cui si è fatta la richiesta
            private string m_NomeAmministrazione;     // Nome dell'amministrazione a cui si è fatta la richiesta
            private string m_RichiestaAMezzo;         // Nome del mezzo usato per la richiesta (telfono, fax, email)
            private string m_RichiestaAIndirizzo;     // Indirizzo usato per inviare la richiesta
            private string m_Note;                    // Dettagli aggiunti alla richieta
            private StatoRichiestaCERQ m_StatoOperazione;     // Stato dell'operazione
            private DateTime? m_DataPrevista; // Data in cui si prevede di ritirare la richiesta
            private DateTime? m_DataEffettiva;   // Data del ritiro
            private Sistema.CUser m_OperatoreEffettivo;          // Utente che ha ritirato la quota
            private int m_IDOperatoreEffettivo;      // ID dell'utente che ha ritirato la quota
            private string m_NomeOperatoreEffettivo;     // Nome dell'operatore che ha ritirato la quota
            [NonSerialized] private Commissione m_Commissione;             // Commissione per cui è stata effettuata la richiesta
            private int m_IDCommissione;              // ID della pratica per cui è stata effettuata la richiesta
                                                      // Private m_DescrizioneCommissione As String      'Stringa descrittiva della pratica
            private string m_ContextType;             // Contesto che ha generato la richiesta (es. una pratica)
            private int m_ContextID;              // ID del contesto che ha generato la richiesta
            [NonSerialized] private Databases.DBObjectBase m_DocumentoProdotto;
            private int m_IDDocumentoProdotto;
            private string m_TipoDocumentoProdotto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RichiestaCERQ()
            {
                m_Data = default;
                m_Operatore = null;
                m_IDOperatore = 0;
                m_Cliente = null;
                m_IDCliente = 0;
                m_NomeCliente = DMD.Strings.vbNullString;
                m_TipoRichiesta = DMD.Strings.vbNullString;
                m_Amministrazione = null;
                m_IDAmministrazione = 0;
                m_NomeAmministrazione = DMD.Strings.vbNullString;
                m_RichiestaAMezzo = DMD.Strings.vbNullString;
                m_RichiestaAIndirizzo = DMD.Strings.vbNullString;
                m_Note = DMD.Strings.vbNullString;
                m_StatoOperazione = StatoRichiestaCERQ.DA_RICHIEDERE;
                m_DataPrevista = default;
                m_DataEffettiva = default;
                m_OperatoreEffettivo = null;
                m_IDOperatoreEffettivo = 0;
                m_NomeOperatoreEffettivo = DMD.Strings.vbNullString;
                m_Commissione = null;
                m_IDCommissione = 0;
                // Me.m_DescrizioneCommissione = vbNullString
                m_ContextType = DMD.Strings.vbNullString;
                m_ContextID = 0;
                m_AziendaRichiedente = null;
                m_IDAziendaRichiedente = 0;
                m_NomeAziendaRichiedente = DMD.Strings.vbNullString;
                m_DocumentoProdotto = null;
                m_IDDocumentoProdotto = 0;
                m_TipoDocumentoProdotto = DMD.Strings.vbNullString;
            }

            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    m_Operatore = Sistema.Users.CurrentUser;
            //    m_IDOperatore = DBUtils.GetID(m_Operatore);
            //    m_NomeOperatore = m_Operatore.Nominativo;
            //    m_Data = DMD.DateUtils.Now();
            //    m_StatoOperazione = StatoRichiestaCERQ.DA_RICHIEDERE;
            //    m_OperatoreEffettivo = null;
            //    m_IDOperatoreEffettivo = default;
            //    m_NomeOperatoreEffettivo = DMD.Strings.vbNullString;
            //    m_DataEffettiva = default;
            //    m_DataPrevista = default;
            //}

            /// <summary>
            /// Restituisce o imposta il documento prodotto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Databases.DBObjectBase DocumentoProdotto
            {
                get
                {
                    if (m_DocumentoProdotto is null)
                        m_DocumentoProdotto = Sistema.Types.GetItemByTypeAndId(m_TipoDocumentoProdotto, m_IDDocumentoProdotto);
                    return m_DocumentoProdotto;
                }

                set
                {
                    var oldValue = m_DocumentoProdotto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DocumentoProdotto = value;
                    if (value is object)
                    {
                        m_TipoDocumentoProdotto = DMD.RunTime.vbTypeName(value);
                        m_IDDocumentoProdotto = DBUtils.GetID(value, 0);
                    }
                    else
                    {
                        m_TipoDocumentoProdotto = DMD.Strings.vbNullString;
                        m_IDDocumentoProdotto = 0;
                    }

                    DoChanged("DocumentoProdotto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del documento prodotto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDocumentoProdotto
            {
                get
                {
                    return DBUtils.GetID(m_DocumentoProdotto, m_IDDocumentoProdotto);
                }

                set
                {
                    int oldValue = IDDocumentoProdotto;
                    if (oldValue == value)
                        return;
                    m_IDDocumentoProdotto = value;
                    m_DocumentoProdotto = null;
                    DoChanged("IDDocumentoProdotto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del tipo del documento prodotto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoDocumentoProdotto
            {
                get
                {
                    return m_TipoDocumentoProdotto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoDocumentoProdotto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoDocumentoProdotto = value;
                    DoChanged("TipoDocumentoProdotto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o importa l'azienda che ha effettuato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda AziendaRichiedente
            {
                get
                {
                    if (m_AziendaRichiedente is null)
                        m_AziendaRichiedente = Anagrafica.Aziende.GetItemById(m_IDAziendaRichiedente);
                    return m_AziendaRichiedente;
                }

                set
                {
                    var oldValue = m_AziendaRichiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AziendaRichiedente = value;
                    m_IDAziendaRichiedente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAziendaRichiedente = value.Nominativo;
                    DoChanged("AziendaRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azienda richiedente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAziendaRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_AziendaRichiedente, m_IDAziendaRichiedente);
                }

                set
                {
                    int oldValue = IDAziendaRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDAziendaRichiedente = value;
                    m_AziendaRichiedente = null;
                    DoChanged("IDAziendaRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda che ha effettuato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAziendaRichiedente
            {
                get
                {
                    return m_NomeAziendaRichiedente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAziendaRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAziendaRichiedente = value;
                    DoChanged("NomeAziendaRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data della richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha effettuato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    object oldValue = m_Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore a cui è stata assegnata la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha effettuato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona per cui si è fatta la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (oldValue == value)
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del cliente per cui si è fatta la richiesta
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
            /// Restituisce o imposta il nominativo del cliente per cui si è fatta la richiesta
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
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della richiesta (Richiesta conteggio estintivo, Rimborso quote, ...)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoRichiesta
            {
                get
                {
                    return m_TipoRichiesta;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TipoRichiesta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoRichiesta = value;
                    DoChanged("TipoRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'amministrazione a cui si è stata inviata la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda Amministrazione
            {
                get
                {
                    if (m_Amministrazione is null)
                        m_Amministrazione = Anagrafica.Aziende.GetItemById(m_IDAmministrazione);
                    return m_Amministrazione;
                }

                set
                {
                    var oldValue = m_Amministrazione;
                    if (oldValue == value)
                        return;
                    m_Amministrazione = value;
                    m_IDAmministrazione = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAmministrazione = value.Nominativo;
                    DoChanged("Amministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'amministrazione a cui è stata fatta la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAmministrazione
            {
                get
                {
                    return DBUtils.GetID(m_Amministrazione, m_IDAmministrazione);
                }

                set
                {
                    int oldValue = IDAmministrazione;
                    if (oldValue == value)
                        return;
                    m_IDAmministrazione = value;
                    m_Amministrazione = null;
                    DoChanged("IDAmministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'amministrazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeAmministrazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAmministrazione = value;
                    DoChanged("NomeAmministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il mezzo usato per la richiesta (e-mail, fax, telefono)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string RichiestaAMezzo
            {
                get
                {
                    return m_RichiestaAMezzo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_RichiestaAMezzo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RichiestaAMezzo = value;
                    DoChanged("RichiestaAMezzo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo usato per la richiesta (numero di telefono o di fax, indirizzo email, indirizzo postale)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string RichiestaAIndirizzo
            {
                get
                {
                    return m_RichiestaAIndirizzo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_RichiestaAIndirizzo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RichiestaAIndirizzo = value;
                    DoChanged("RichiestaAIndirizzo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa per informazioni aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato dell'operazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoRichiestaCERQ StatoOperazione
            {
                get
                {
                    return m_StatoOperazione;
                }

                set
                {
                    var oldValue = m_StatoOperazione;
                    if (oldValue == value)
                        return;
                    m_StatoOperazione = value;
                    DoChanged("StatoOperazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data prevista per il ritiro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }

                set
                {
                    var oldValue = m_DataPrevista;
                    if (oldValue == value == true)
                        return;
                    m_DataPrevista = value;
                    DoChanged("DataPrevista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di ritiro (o di annullamento/rifiuto)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataEffettiva
            {
                get
                {
                    return m_DataEffettiva;
                }

                set
                {
                    var oldValue = m_DataEffettiva;
                    if (oldValue == value == true)
                        return;
                    m_DataEffettiva = value;
                    DoChanged("DataEffettiva", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha ritirato (annullato o memorizzato il rifiuto)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser OperatoreEffettivo
            {
                get
                {
                    if (m_OperatoreEffettivo is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatoreEffettivo);
                    return m_OperatoreEffettivo;
                }

                set
                {
                    var oldValue = m_OperatoreEffettivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OperatoreEffettivo = value;
                    m_IDOperatoreEffettivo = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatoreEffettivo = value.Nominativo;
                    DoChanged("OperatoreEffettivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha ritirato (annulato o memorizzato il rifiuto)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOperatoreEffettivo
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreEffettivo, m_IDOperatoreEffettivo);
                }

                set
                {
                    int oldValue = m_IDOperatoreEffettivo;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreEffettivo = value;
                    m_OperatoreEffettivo = null;
                    DoChanged("IDOperatoreEffettivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha ritirato (annullato o memorizzato il rifiuto)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatoreEffettivo
            {
                get
                {
                    return m_NomeOperatoreEffettivo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatoreEffettivo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreEffettivo = value;
                    DoChanged("NomeOperatoreEffettivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la pratica per cui è stata effettuata la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Commissione Commissione
            {
                get
                {
                    if (m_Commissione is null)
                        m_Commissione = Commissioni.GetItemById(m_IDCommissione);
                    return m_Commissione;
                }

                set
                {
                    var oldValue = m_Commissione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Commissione = value;
                    m_IDCommissione = DBUtils.GetID(value, 0);
                    DoChanged("Commissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della pratica per cui è stata fatta la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCommissione
            {
                get
                {
                    return DBUtils.GetID(m_Commissione, m_IDCommissione);
                }

                set
                {
                    int oldValue = IDCommissione;
                    if (oldValue == value)
                        return;
                    m_IDCommissione = value;
                    m_Commissione = null;
                    DoChanged("IDCommissione", value, oldValue);
                }
            }

            // ''' <summary>
            // ''' Restituisce o imposta una stringa descrittiva per la pratica
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property DescrizioneCommissione As String
            // Get
            // Return Me.m_DescrizioneCommissione
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_DescrizioneCommissione
            // If (oldValue = value) Then Exit Property
            // Me.m_DescrizioneCommissione = value
            // Me.DoChanged("DescrizioneCommissione", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce o imposta il tipo del contesto a cui è associata la richiesta (es. una pratica)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ContextType
            {
                get
                {
                    return m_ContextType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ContextType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContextType = value;
                    DoChanged("ContextType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del contesto che ha generato la richiesta (es. una pratica)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ContextID
            {
                get
                {
                    return m_ContextID;
                }

                set
                {
                    int oldValue = m_ContextID;
                    if (oldValue == value)
                        return;
                    m_ContextID = value;
                    DoChanged("ContextID", value, oldValue);
                }
            }

            /// <summary>
            /// Richiede
            /// </summary>
            public void Richiedi()
            {
                if (StatoOperazione != StatoRichiestaCERQ.DA_RICHIEDERE)
                    throw new InvalidOperationException("La richiesta è già stata inoltrata");
                StatoOperazione = StatoRichiestaCERQ.RICHIESTA;
                Operatore = Sistema.Users.CurrentUser;
                Data = DMD.DateUtils.Now();
                Save();
                var e = new ItemEventArgs(this);
                OnRichiestaInviata(e);
                RichiesteCERQ.doRichiestaInviata(e);
            }

            /// <summary>
            /// Genera l'evento RichiestaInviata
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnRichiestaInviata(ItemEventArgs e)
            {
                RichiestaInviata?.Invoke(this, e);
            }

            /// <summary>
            /// La richiesta é stata evasa correttamente
            /// </summary>
            public void Ritira()
            {
                if (StatoOperazione != StatoRichiestaCERQ.RICHIESTA)
                    throw new InvalidOperationException("La richiesta non è in stato Richiesta");
                StatoOperazione = StatoRichiestaCERQ.RITIRATA;
                OperatoreEffettivo = Sistema.Users.CurrentUser;
                DataEffettiva = DMD.DateUtils.Now();
                Save();
                var e = new ItemEventArgs(this);
                OnRichiestaRitirata(e);
                RichiesteCERQ.doRichiestaRitirata(e);
            }

            /// <summary>
            /// Genera l'evento RichiestaRitirata
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnRichiestaRitirata(ItemEventArgs e)
            {
                RichiestaRitirata?.Invoke(this, e);
            }

            /// <summary>
            /// Richiesta annullata dal richiedente
            /// </summary>
            public void Annulla()
            {
                if (StatoOperazione > StatoRichiestaCERQ.RICHIESTA)
                    throw new InvalidOperationException("La richiesta non è in stato Richiesta");
                StatoOperazione = StatoRichiestaCERQ.ANNULLATA;
                OperatoreEffettivo = Sistema.Users.CurrentUser;
                DataEffettiva = DMD.DateUtils.Now();
                Save();
                var e = new ItemEventArgs(this);
                OnRichiestaAnnullata(e);
                RichiesteCERQ.doRichiestaAnnullata(e);
            }

            /// <summary>
            /// Genera l'evento RichiestaAnnullata
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnRichiestaAnnullata(ItemEventArgs e)
            {
                RichiestaAnnullata?.Invoke(this, e);
            }

            /// <summary>
            /// Richiesta rifiutata dall'amministrazione
            /// </summary>
            public void Rifiuta()
            {
                if (StatoOperazione != StatoRichiestaCERQ.RICHIESTA)
                    throw new InvalidOperationException("La richiesta non è in stato Richiesta");
                StatoOperazione = StatoRichiestaCERQ.RIFIUTATA;
                OperatoreEffettivo = Sistema.Users.CurrentUser;
                DataEffettiva = DMD.DateUtils.Now();
                Save();
                var e = new ItemEventArgs(this);
                OnRichiestaRifiutata(e);
                RichiesteCERQ.doRichiestaRifiutata(e);
            }

            /// <summary>
            /// Genera l'evento RichiestaRifiutata
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnRichiestaRifiutata(ItemEventArgs e)
            {
                RichiestaRifiutata?.Invoke(this, e);
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.RichiesteCERQ;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CQSPDRichCERQ";
            }

            /// <summary>
            /// Salva nel recordset
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Data", m_Data);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("TipoRichiesta", m_TipoRichiesta);
                writer.Write("IDAmministrazione", IDAmministrazione);
                writer.Write("NomeAmministrazione", m_NomeAmministrazione);
                writer.Write("RichiestaAMezzo", m_RichiestaAMezzo);
                writer.Write("RichiestaAIndirizzo", m_RichiestaAIndirizzo);
                writer.Write("Note", m_Note);
                writer.Write("StatoOperazione", m_StatoOperazione);
                writer.Write("DataPrevista", m_DataPrevista);
                writer.Write("DataEffettiva", m_DataEffettiva);
                writer.Write("IDOperatoreEffettivo", IDOperatoreEffettivo);
                writer.Write("NomeOperatoreEffettivo", m_NomeOperatoreEffettivo);
                writer.Write("IDCommissione", IDCommissione);
                writer.Write("ContextType", m_ContextType);
                writer.Write("ContextID", m_ContextID);
                writer.Write("IDAziendaRichiedente", IDAziendaRichiedente);
                writer.Write("NomeAziendaRichiedente", m_NomeAziendaRichiedente);
                writer.Write("TipoOggettoProdotto", m_TipoDocumentoProdotto);
                writer.Write("IDOggettoProdotto", IDDocumentoProdotto);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Carica dal database
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Data = reader.Read("Data", this.m_Data);
                m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_TipoRichiesta = reader.Read("TipoRichiesta", this.m_TipoRichiesta);
                m_IDAmministrazione = reader.Read("IDAmministrazione", this.m_IDAmministrazione);
                m_NomeAmministrazione = reader.Read("NomeAmministrazione", this.m_NomeAmministrazione);
                m_RichiestaAMezzo = reader.Read("RichiestaAMezzo", this.m_RichiestaAMezzo);
                m_RichiestaAIndirizzo = reader.Read("RichiestaAIndirizzo", this.m_RichiestaAIndirizzo);
                m_Note = reader.Read("Note", this.m_Note);
                m_StatoOperazione = reader.Read("StatoOperazione", this.m_StatoOperazione);
                m_DataPrevista = reader.Read("DataPrevista", this.m_DataPrevista);
                m_DataEffettiva = reader.Read("DataEffettiva", this.m_DataEffettiva);
                m_IDOperatoreEffettivo = reader.Read("IDOperatoreEffettivo", this.m_IDOperatoreEffettivo);
                m_NomeOperatoreEffettivo = reader.Read("NomeOperatoreEffettivo", this.m_NomeOperatoreEffettivo);
                m_IDCommissione = reader.Read("IDCommissione", this.m_IDCommissione);
                m_ContextType = reader.Read("ContextType", this.m_ContextType);
                m_ContextID = reader.Read("ContextID", this.m_ContextID);
                m_IDAziendaRichiedente = reader.Read("IDAziendaRichiedente", this.m_IDAziendaRichiedente);
                m_NomeAziendaRichiedente = reader.Read("NomeAziendaRichiedente", this.m_NomeAziendaRichiedente);
                m_TipoDocumentoProdotto = reader.Read("TipoOggettoProdotto", this.m_TipoDocumentoProdotto);
                m_IDDocumentoProdotto = reader.Read("IDOggettoProdotto", this.m_IDDocumentoProdotto);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("TipoRichiesta", typeof(string), 255);
                c = table.Fields.Ensure("IDAmministrazione", typeof(int), 1);
                c = table.Fields.Ensure("NomeAmministrazione", typeof(string), 255);
                c = table.Fields.Ensure("RichiestaAMezzo", typeof(string), 255);
                c = table.Fields.Ensure("RichiestaAIndirizzo", typeof(string), 255);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("StatoOperazione", typeof(int), 1);
                c = table.Fields.Ensure("DataPrevista", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataEffettiva", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDOperatoreEffettivo", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatoreEffettivo", typeof(string), 255);
                c = table.Fields.Ensure("IDCommissione", typeof(int), 1);
                c = table.Fields.Ensure("ContextType", typeof(string), 255);
                c = table.Fields.Ensure("ContextID", typeof(int), 1);
                c = table.Fields.Ensure("IDAziendaRichiedente", typeof(int), 1);
                c = table.Fields.Ensure("NomeAziendaRichiedente", typeof(string), 255);
                c = table.Fields.Ensure("TipoOggettoProdotto", typeof(string), 255);
                c = table.Fields.Ensure("IDOggettoProdotto", typeof(int), 1);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.Ensure("idxDate", new string[] { "Data", "DataPrevista", "DataEffettiva" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore", "TipoRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente", "IDAmministrazione" , "NomeAmministrazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRichiesta", new string[] { "RichiestaAMezzo", "RichiestaAIndirizzo", "StatoOperazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "Note" , "IDAziendaRichiedente", "NomeAziendaRichiedente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatoreE", new string[] { "IDOperatoreEffettivo", "NomeOperatoreEffettivo", "IDCommissione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContext", new string[] { "ContextType", "ContextID", "TipoOggettoProdotto", "IDOggettoProdotto" }, DBFieldConstraintFlags.None);
                 
                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("TipoRichiesta", m_TipoRichiesta);
                writer.WriteAttribute("IDAmministrazione", IDAmministrazione);
                writer.WriteAttribute("NomeAmministrazione", m_NomeAmministrazione);
                writer.WriteAttribute("RichiestaAMezzo", m_RichiestaAMezzo);
                writer.WriteAttribute("RichiestaAIndirizzo", m_RichiestaAIndirizzo);
                writer.WriteAttribute("StatoOperazione", (int?)m_StatoOperazione);
                writer.WriteAttribute("DataPrevista", m_DataPrevista);
                writer.WriteAttribute("DataEffettiva", m_DataEffettiva);
                writer.WriteAttribute("IDOperatoreEffettivo", IDOperatoreEffettivo);
                writer.WriteAttribute("NomeOperatoreEffettivo", m_NomeOperatoreEffettivo);
                writer.WriteAttribute("IDCommissione", IDCommissione);
                writer.WriteAttribute("ContextType", m_ContextType);
                writer.WriteAttribute("ContextID", m_ContextID);
                writer.WriteAttribute("IDAziendaRichiedente", IDAziendaRichiedente);
                writer.WriteAttribute("NomeAziendaRichiedente", m_NomeAziendaRichiedente);
                writer.WriteAttribute("TipoOggettoProdotto", m_TipoDocumentoProdotto);
                writer.WriteAttribute("IDOggettoProdotto", IDDocumentoProdotto);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "TipoRichiesta":
                        {
                            m_TipoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAmministrazione":
                        {
                            m_IDAmministrazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAmministrazione":
                        {
                            m_NomeAmministrazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RichiestaAMezzo":
                        {
                            m_RichiestaAMezzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RichiestaAIndirizzo":
                        {
                            m_RichiestaAIndirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoOperazione":
                        {
                            m_StatoOperazione = (StatoRichiestaCERQ)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataPrevista":
                        {
                            m_DataPrevista = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataEffettiva":
                        {
                            m_DataEffettiva = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatoreEffettivo":
                        {
                            m_IDOperatoreEffettivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreEffettivo":
                        {
                            m_NomeOperatoreEffettivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCommissione":
                        {
                            m_IDCommissione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ContextType":
                        {
                            m_ContextType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContextID":
                        {
                            m_ContextID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDAziendaRichiedente":
                        {
                            m_IDAziendaRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAziendaRichiedente":
                        {
                            m_NomeAziendaRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoOggettoProdotto":
                        {
                            m_TipoDocumentoProdotto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOggettoProdotto":
                        {
                            m_IDDocumentoProdotto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(m_TipoRichiesta , " fatta il " , Sistema.Formats.FormatUserDate(m_Data) , " da " , m_NomeOperatore , " per " , m_NomeCliente , " presso " , m_NomeAmministrazione);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.m_Data.GetHashCode();
            } 

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (m_Commissione is object)
                    m_Commissione.Save(force);
                return ret;
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            protected override Databases.DBObjectBase _Clone()
            {
                var ret = (RichiestaCERQ)base._Clone();
                ret.Commissione = null;
                return ret;
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public RichiestaCERQ Clone()
            {
                return (RichiestaCERQ)this._Clone();
            }

            /// <summary>
            /// Restituisce rue se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is RichiestaCERQ) && this.Equals((RichiestaCERQ)obj);
            }

            /// <summary>
            /// Restituisce rue se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RichiestaCERQ obj)
            {
                //TODO verificare meglio i parametri
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Strings.EQ(this.m_TipoRichiesta, obj.m_TipoRichiesta)
                    && DMD.Integers.EQ(this.m_IDAziendaRichiedente, obj.m_IDAziendaRichiedente)
                    && DMD.Strings.EQ(this.m_NomeAziendaRichiedente, obj.m_NomeAziendaRichiedente)
                    && DMD.Integers.EQ(this.m_IDAmministrazione, obj.m_IDAmministrazione)
                    && DMD.Strings.EQ(this.m_NomeAmministrazione, obj.m_NomeAmministrazione)
                    && DMD.Strings.EQ(this.m_RichiestaAMezzo, obj.m_RichiestaAMezzo)
                    && DMD.Strings.EQ(this.m_RichiestaAIndirizzo, obj.m_RichiestaAIndirizzo)
                    && DMD.Integers.EQ((int)this.m_StatoOperazione, (int)obj.m_StatoOperazione)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.DateUtils.EQ(this.m_DataPrevista, obj.m_DataPrevista)
                    && DMD.DateUtils.EQ(this.m_DataEffettiva, obj.m_DataEffettiva)
                    && DMD.Integers.EQ(this.m_IDOperatoreEffettivo, obj.m_IDOperatoreEffettivo)
                    && DMD.Strings.EQ(this.m_NomeOperatoreEffettivo, obj.m_NomeOperatoreEffettivo)
                    && DMD.Integers.EQ(this.m_IDCommissione, obj.m_IDCommissione)
                    && DMD.Strings.EQ(this.m_ContextType, obj.m_ContextType)
                    && DMD.Integers.EQ(this.m_ContextID, obj.m_ContextID)
                    && DMD.Integers.EQ(this.m_IDDocumentoProdotto, obj.m_IDDocumentoProdotto)
                    && DMD.Strings.EQ(this.m_TipoDocumentoProdotto, obj.m_TipoDocumentoProdotto)
                    ;
            }
        }
    }
}