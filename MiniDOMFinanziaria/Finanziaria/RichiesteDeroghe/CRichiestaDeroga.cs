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
        public enum StatoRichiestaDeroga : int
        {
            /// <summary>
        /// La richiesta è in fase di preparazione
        /// </summary>
            Nessuno = 0,

            /// <summary>
        /// La richiesta è pronta per essere inviata
        /// </summary>
            DaInviare = 1,

            /// <summary>
        /// La richiesta è stata inviata
        /// </summary>
            Inviata = 2,

            /// <summary>
        /// La richiesta è stata ricevuta dal destinatario
        /// </summary>
            Ricevuta = 3,

            /// <summary>
        /// La richiesta è stata accettata
        /// </summary>
            Accettata = 4,

            /// <summary>
        /// La richiesta è stata rifiutata
        /// </summary>
            Rifiutata = 5
        }

        /// <summary>
    /// Rappresenta una richiesta di deroga fatta al cessionario per ottenere delle condizioni migliorative
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CRichiestaDeroga : Databases.DBObjectPO
        {
            private int m_IDCliente;                  // ID del cliente
            private Anagrafica.CPersonaFisica m_Cliente;             // Cliente
            private string m_NomeCliente;                 // Nome del cliente
            private StatoRichiestaDeroga m_StatoRichiesta; // Stato della richiesta
            private DateTime? m_DataRichiesta;                // Data in cui è stata effettuata la richiesta
            private int m_IDRichiedente;              // ID dell'operatore che ha richiesto la deroga
            private Sistema.CUser m_Richiedente;                  // Operatore che ha richiesto la deroga
            private string m_NomeRichiedente;             // Nome dell'operatore che ha richiesto la deroga
            private string m_MotivoRichiesta;             // Messaggio da inviare per spiegare la richiesta
            private int m_IDAgenziaConcorrente;       // ID dell'agenzia concorrente
            private Anagrafica.CPersona m_AgenziaConcorrente;        // Concorrente
            private string m_NomeAgenziaConcorrente;      // Nome dell'agenzia concorrente
            private string m_NomeProdottoConcorrente;                // Nome del prodotto offerto dalla concorrenza
            private string m_NumeroPreventivoConcorrente;            // Numero del preventivo offerto dalla concorrenza
            private decimal? m_RataConcorrente;                      // Rata dell'offerta concorrente
            private int? m_DurataConcorrente;                     // Durata dell'offerta concorrente
            private decimal? m_NettoRicavoConcorrente;                // Netto ricavo concorrente
            private double? m_TANConcorrente;                         // TAN dell'offerta concorrente
            private double? m_TAEGConcorrente;                         // TAN dell'offerta concorrente
            private int m_IDOffertaIniziale;          // ID dell'offerta che si è richiesto di valutare
            private COffertaCQS m_OffertaIniziale;        // Offerta che si è richiesto di valutare
            private string m_InviatoA;                    // Indirizzo di invio (a cui è stata fatta la richiesta)
            private string m_InviatoACC;                  // Indirizzi in copia carbone
            private string m_MezzoDiInvio;                // Tipo del mezzo di invio (e-mail)
            private string m_SendSubject;                 // Oggetto della mail inviata
            private string m_SendMessange;                // Corpo della mail inviata
            private DateTime? m_SendDate;                     // Data di invio della mail
            private CCollection<Sistema.CAttachment> m_Attachments;   // Documenti allegati alla richiesta (inviati come allegato della e-mail)
            private DateTime? m_RicevutoIl;                    // Data di ricezione del messaggio   
                                                               // Private m_RicevutoDa As CUser
                                                               // Private m_RicevutoDaID As Integer
                                                               // Private m_RicevutoDaNome As String


            private DateTime? m_RispostoIl;                   // Data di ricezione del messaggio di risposta
            private string m_RispostoDa;                  // Indirizzo di provenienza della risposta
            private string m_RispostoAMezzo;              // Mezzo di provenienza della risposta
            private string m_RispostoSubject;             // Oggetto della mail di risposta
            private string m_RispostoMessage;             // Corpo della mail di risposta
            private int m_IDOffertaCorrente;          // Nell'iter di lavorazione l'offerta può essere modificata    
            private COffertaCQS m_OffertaCorrente;
            private int m_Flags;
            private CKeyCollection m_Parameters;
            private int m_IDFinestraLavorazione;
            private FinestraLavorazione m_FinestraLavorazione;

            public CRichiestaDeroga()
            {
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_StatoRichiesta = StatoRichiestaDeroga.Nessuno;
                m_DataRichiesta = default;
                m_IDRichiedente = 0;
                m_Richiedente = null;
                m_NomeRichiedente = "";
                m_MotivoRichiesta = "";
                m_IDAgenziaConcorrente = 0;
                m_AgenziaConcorrente = null;
                m_NomeAgenziaConcorrente = "";
                m_NomeProdottoConcorrente = "";
                m_NumeroPreventivoConcorrente = "";
                m_RataConcorrente = default;
                m_DurataConcorrente = default;
                m_NettoRicavoConcorrente = default;
                m_TANConcorrente = default;
                m_TAEGConcorrente = default;
                m_IDOffertaIniziale = 0;
                m_OffertaIniziale = null;
                m_InviatoA = "";
                m_InviatoACC = "";
                m_MezzoDiInvio = "";
                m_SendSubject = "";
                m_SendMessange = "";
                m_SendDate = default;
                m_Attachments = null; // CCollection(Of CAttachment)   'Documenti allegati alla richiesta (inviati come allegato della e-mail)
                m_RicevutoIl = default;
                m_RispostoIl = default;
                m_RispostoDa = "";
                m_RispostoAMezzo = "";
                m_RispostoSubject = "";
                m_RispostoMessage = "";
                m_IDOffertaCorrente = 0;
                m_OffertaCorrente = null;
                m_Flags = 0;
                m_Parameters = null; // CKeyCollection
                m_IDFinestraLavorazione = 0;
                m_FinestraLavorazione = null;
            }

            /// <summary>
        /// Restituisce o imposta l'ID del cliente per cui è stata chiesta la deroga
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta il cliente per cui è stata chiesta la deroga
        /// </summary>
        /// <returns></returns>
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
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value);
                    m_NomeCliente = "";
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del cliente per cui è stata chiesta la deroga
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta lo stato della richiesta
        /// </summary>
        /// <returns></returns>
            public StatoRichiestaDeroga StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }

                set
                {
                    var oldValue = m_StatoRichiesta;
                    if (oldValue == value)
                        return;
                    m_StatoRichiesta = value;
                    DoChanged("StatoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui è stata creata la richiesta
        /// </summary>
        /// <returns></returns>
            public DateTime? DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }

                set
                {
                    var oldValue = m_DataRichiesta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiesta = value;
                    DoChanged("DataRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha creato la richiestaz
        /// </summary>
        /// <returns></returns>
            public int IDRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_Richiedente, m_IDRichiedente);
                }

                set
                {
                    int oldValue = IDRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDRichiedente = value;
                    m_Richiedente = null;
                    DoChanged("IDRichiedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha creato la richiesta
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser Richiedente
            {
                get
                {
                    if (m_Richiedente is null)
                        m_Richiedente = Sistema.Users.GetItemById(m_IDRichiedente);
                    return m_Richiedente;
                }

                set
                {
                    var oldValue = Richiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiedente = value;
                    m_IDRichiedente = DBUtils.GetID(value);
                    m_NomeRichiedente = "";
                    if (value is object)
                        m_NomeRichiedente = value.Nominativo;
                    DoChanged("Richiedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del richiedente
        /// </summary>
        /// <returns></returns>
            public string NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }

                set
                {
                    string oldValue = m_NomeRichiedente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiedente = value;
                    DoChanged("NomeRichiedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il messaggio composto dal richiedente per giustificare la richiesta
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
                    string oldValue = m_MotivoRichiesta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoRichiesta = value;
                    DoChanged("MotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona/azienda che ha formulato l'offerta concorrente
        /// </summary>
        /// <returns></returns>
            public int IDAgenziaConcorrente
            {
                get
                {
                    return DBUtils.GetID(m_AgenziaConcorrente, m_IDAgenziaConcorrente);
                }

                set
                {
                    int oldValue = IDAgenziaConcorrente;
                    if (oldValue == value)
                        return;
                    m_IDAgenziaConcorrente = value;
                    m_AgenziaConcorrente = null;
                    DoChanged("IDAgenziaConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la persona/azienda che ha formulato l'offerta concorrente
        /// </summary>
        /// <returns></returns>
            public Anagrafica.CPersona AgenziaConcorrente
            {
                get
                {
                    if (m_AgenziaConcorrente is null)
                        m_AgenziaConcorrente = Anagrafica.Persone.GetItemById(m_IDAgenziaConcorrente);
                    return m_AgenziaConcorrente;
                }

                set
                {
                    var oldValue = m_AgenziaConcorrente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AgenziaConcorrente = value;
                    m_IDAgenziaConcorrente = DBUtils.GetID(value);
                    m_NomeAgenziaConcorrente = "";
                    if (value is object)
                        m_NomeAgenziaConcorrente = value.Nominativo;
                    DoChanged("AgenziaConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della persona/agenzia che ha formulato l'offerta concorrente
        /// </summary>
        /// <returns></returns>
            public string NomeAgenziaConcorrente
            {
                get
                {
                    return m_NomeAgenziaConcorrente;
                }

                set
                {
                    string oldValue = m_NomeAgenziaConcorrente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgenziaConcorrente = value;
                    DoChanged("NomeAgenziaConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del prodotto offerto dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public string NomeProdottoConcorrente
            {
                get
                {
                    return m_NomeProdottoConcorrente;
                }

                set
                {
                    string oldValue = m_NomeProdottoConcorrente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProdottoConcorrente = value;
                    DoChanged("NomeProdottoConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero del preventivo formulato dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public string NumeroPreventivoConcorrente
            {
                get
                {
                    return m_NumeroPreventivoConcorrente;
                }

                set
                {
                    string oldValue = m_NumeroPreventivoConcorrente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroPreventivoConcorrente = value;
                    DoChanged("NumeroPreventivoConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la rata del preventivo formulato dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public decimal? RataConcorrente
            {
                get
                {
                    return m_RataConcorrente;
                }

                set
                {
                    var oldValue = m_RataConcorrente;
                    if (oldValue == value == true)
                        return;
                    m_RataConcorrente = value;
                    DoChanged("RataConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata del preventivo formulato dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public int? DurataConcorrente
            {
                get
                {
                    return m_DurataConcorrente;
                }

                set
                {
                    var oldValue = m_DurataConcorrente;
                    if (oldValue == value == true)
                        return;
                    m_DurataConcorrente = value;
                    DoChanged("DurataConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il netto ricavo del preventivo formulato dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public decimal? NettoRicavoConcorrente
            {
                get
                {
                    return m_NettoRicavoConcorrente;
                }

                set
                {
                    var oldValue = m_NettoRicavoConcorrente;
                    if (oldValue == value == true)
                        return;
                    m_NettoRicavoConcorrente = value;
                    DoChanged("NettoRicavoConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TAN del preventivo formulato dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public double? TANConcorrente
            {
                get
                {
                    return m_TANConcorrente;
                }

                set
                {
                    var oldValue = m_TANConcorrente;
                    if (oldValue == value == true)
                        return;
                    m_TANConcorrente = value;
                    DoChanged("TANConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TAEG del preventivo formulato dalla concorrenza
        /// </summary>
        /// <returns></returns>
            public double? TAEGConcorrente
            {
                get
                {
                    return m_TAEGConcorrente;
                }

                set
                {
                    var oldValue = m_TAEGConcorrente;
                    if (oldValue == value == true)
                        return;
                    m_TAEGConcorrente = value;
                    DoChanged("TAEGConcorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'offerta iniziale (inviata in approvazione)
        /// </summary>
        /// <returns></returns>
            public int IDOffertaIniziale
            {
                get
                {
                    return DBUtils.GetID(m_OffertaIniziale, m_IDOffertaIniziale);
                }

                set
                {
                    int oldValue = IDOffertaIniziale;
                    if (oldValue == value)
                        return;
                    m_IDOffertaIniziale = value;
                    m_OffertaIniziale = null;
                    DoChanged("IDOffertaIniziale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'offerta iniziale (inviata in approvazione)
        /// </summary>
        /// <returns></returns>
            public COffertaCQS OffertaIniziale
            {
                get
                {
                    if (m_OffertaIniziale is null)
                        m_OffertaIniziale = Offerte.GetItemById(m_IDOffertaIniziale);
                    return m_OffertaIniziale;
                }

                set
                {
                    var oldValue = m_OffertaIniziale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OffertaIniziale = value;
                    m_IDOffertaIniziale = DBUtils.GetID(value);
                    DoChanged("OffertaIniziale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'indirizzo a cui è stata inviata la richiesta
        /// </summary>
        /// <returns></returns>
            public string InviatoA
            {
                get
                {
                    return m_InviatoA;
                }

                set
                {
                    string oldValue = m_InviatoA;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_InviatoA = value;
                    DoChanged("InviatoA", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una
        /// </summary>
        /// <returns></returns>
            public string InviatoACC
            {
                get
                {
                    return m_InviatoACC;
                }

                set
                {
                    string oldValue = m_InviatoACC;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_InviatoACC = value;
                    DoChanged("InviatoACC", value, oldValue);
                }
            }

            /// <summary>
        /// Restitusice o imposta il mezzo utilizzato per inviare la richiesta (e-mail, ecc)
        /// </summary>
        /// <returns></returns>
            public string MezzoDiInvio
            {
                get
                {
                    return m_MezzoDiInvio;
                }

                set
                {
                    string oldValue = m_MezzoDiInvio;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MezzoDiInvio = value;
                    DoChanged("MezzoDiInvio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto della mail inviata per la richiesta
        /// </summary>
        /// <returns></returns>
            public string SendSubject
            {
                get
                {
                    return m_SendSubject;
                }

                set
                {
                    string oldValue = m_SendSubject;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SendSubject = value;
                    DoChanged("SendSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il corpo del messaggio inviato per la richiesta
        /// </summary>
        /// <returns></returns>
            public string SendMessage
            {
                get
                {
                    return m_SendMessange;
                }

                set
                {
                    string oldValue = m_SendMessange;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SendMessange = value;
                    DoChanged("SendMessange", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di invio
        /// </summary>
        /// <returns></returns>
            public DateTime? SendDate
            {
                get
                {
                    return m_SendDate;
                }

                set
                {
                    var oldValue = m_SendDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_SendDate = value;
                    DoChanged("SendDate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'elenco degli allegati inviati
        /// </summary>
        /// <returns></returns>
            public CCollection<Sistema.CAttachment> Attachments
            {
                get
                {
                    if (m_Attachments is null)
                        m_Attachments = new CCollection<Sistema.CAttachment>();
                    return m_Attachments;
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di ricezione (da parte del cessionario)
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta la data di ricezione del messaggio di risposta
        /// </summary>
        /// <returns></returns>
            public DateTime? RispostoIl
            {
                get
                {
                    return m_RispostoIl;
                }

                set
                {
                    var oldValue = m_RispostoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_RispostoIl = value;
                    DoChanged("RispostoIl", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'indirizzo del mittente della risposta
        /// </summary>
        /// <returns></returns>
            public string RispostoDa
            {
                get
                {
                    return m_RispostoDa;
                }

                set
                {
                    string oldValue = m_RispostoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RispostoDa = value;
                    DoChanged("RispostoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del mezzo della comunicazione di risposta
        /// </summary>
        /// <returns></returns>
            public string RispostoAMezzo
            {
                get
                {
                    return m_RispostoAMezzo;
                }

                set
                {
                    string oldValue = m_RispostoAMezzo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RispostoAMezzo = value;
                    DoChanged("RispostoAMezzo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto della mail inviata come risposta
        /// </summary>
        /// <returns></returns>
            public string RispostoSubject
            {
                get
                {
                    return m_RispostoSubject;
                }

                set
                {
                    string oldValue = m_RispostoSubject;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RispostoSubject = value;
                    DoChanged("RispostoSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il messaggio inviato come risposta
        /// </summary>
        /// <returns></returns>
            public string RispostoMessage
            {
                get
                {
                    return m_RispostoMessage;
                }

                set
                {
                    string oldValue = m_RispostoMessage;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RispostoMessage = value;
                    DoChanged("RispostoMessage", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'offerta accettata dal cessionario
        /// </summary>
        /// <returns></returns>
            public int IDOffertaCorrente
            {
                get
                {
                    return DBUtils.GetID(m_OffertaCorrente, m_IDOffertaCorrente);
                }

                set
                {
                    int oldValue = IDOffertaCorrente;
                    if (oldValue == value)
                        return;
                    m_IDOffertaCorrente = value;
                    m_OffertaCorrente = null;
                    DoChanged("IDOffertaCorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'offerta accettata dal cessionario
        /// </summary>
        /// <returns></returns>
            public COffertaCQS OffertaCorrente
            {
                get
                {
                    if (m_OffertaCorrente is null)
                        m_OffertaCorrente = Offerte.GetItemById(m_IDOffertaCorrente);
                    return m_OffertaCorrente;
                }

                set
                {
                    var oldValue = m_OffertaCorrente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OffertaCorrente = value;
                    m_IDOffertaCorrente = DBUtils.GetID(value);
                    DoChanged("OffertaCorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags aggiuntivi
        /// </summary>
        /// <returns></returns>
            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta dei parametri aggiuntivi
        /// </summary>
        /// <returns></returns>
            public CKeyCollection Parameters
            {
                get
                {
                    if (m_Parameters is null)
                        m_Parameters = new CKeyCollection();
                    return m_Parameters;
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

            /// <summary>
        /// Restituisce o imposta la finestra di lavorazione
        /// </summary>
        /// <returns></returns>
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

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return RichiesteDeroghe.Module;
            }

            public override string GetTableName()
            {
                return "tbl_RichiesteDeroghe";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_StatoRichiesta = reader.Read("StatoRichiesta", this.m_StatoRichiesta);
                m_DataRichiesta = reader.Read("DataRichiesta", this.m_DataRichiesta);
                m_IDRichiedente = reader.Read("IDRichiedente", this.m_IDRichiedente);
                m_NomeRichiedente = reader.Read("NomeRichiedente", this.m_NomeRichiedente);
                m_MotivoRichiesta = reader.Read("MotivoRichiesta", this.m_MotivoRichiesta);
                m_IDAgenziaConcorrente = reader.Read("IDAgenziaConcorrente", this.m_IDAgenziaConcorrente);
                m_NomeAgenziaConcorrente = reader.Read("NomeAgenziaConcorrente", this.m_NomeAgenziaConcorrente);
                m_NomeProdottoConcorrente = reader.Read("NomeProdottoConcorrente", this.m_NomeProdottoConcorrente);
                m_NumeroPreventivoConcorrente = reader.Read("NumeroPreventivoConcorrente", this.m_NumeroPreventivoConcorrente);
                m_RataConcorrente = reader.Read("RataConcorrente", this.m_RataConcorrente);
                m_DurataConcorrente = reader.Read("DurataConcorrente", this.m_DurataConcorrente);
                m_NettoRicavoConcorrente = reader.Read("NettoRicavoConcorrente", this.m_NettoRicavoConcorrente);
                m_TANConcorrente = reader.Read("TANConcorrente", this.m_TANConcorrente);
                m_TAEGConcorrente = reader.Read("TAEGConcorrente", this.m_TAEGConcorrente);
                m_IDOffertaIniziale = reader.Read("IDOffertaIniziale", this.m_IDOffertaIniziale);
                m_InviatoA = reader.Read("InviatoA", this.m_InviatoA);
                m_InviatoACC = reader.Read("InviatoACC", this.m_InviatoACC);
                m_MezzoDiInvio = reader.Read("MezzoDiInvio", this.m_MezzoDiInvio);
                m_SendSubject = reader.Read("SendSubject", this.m_SendSubject);
                m_SendMessange = reader.Read("SendMessange", this.m_SendMessange);
                m_SendDate = reader.Read("SendDate", this.m_SendDate);
                string tmp = reader.Read("Attachments", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Attachments.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                }
                m_RicevutoIl = reader.Read("RicevutoIl", this.m_RicevutoIl);
                m_RispostoIl = reader.Read("RispostoIl", this.m_RispostoIl);
                m_RispostoDa = reader.Read("RispostoDa", this.m_RispostoDa);
                m_RispostoAMezzo = reader.Read("RispostoAMezzo", this.m_RispostoAMezzo);
                m_RispostoSubject = reader.Read("RispostoSubject", this.m_RispostoSubject);
                m_RispostoMessage = reader.Read("RispostoMessage", this.m_RispostoMessage);
                m_IDOffertaCorrente = reader.Read("IDOffertaCorrente", this.m_IDOffertaCorrente);
                m_Flags = reader.Read("Flags", this.m_Flags);
                tmp = reader.Read("Parameters", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                }
                 

                m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", this.m_IDFinestraLavorazione);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("StatoRichiesta", m_StatoRichiesta);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("IDRichiedente", IDRichiedente);
                writer.Write("NomeRichiedente", m_NomeRichiedente);
                writer.Write("MotivoRichiesta", m_MotivoRichiesta);
                writer.Write("IDAgenziaConcorrente", IDAgenziaConcorrente);
                writer.Write("NomeAgenziaConcorrente", m_NomeAgenziaConcorrente);
                writer.Write("NomeProdottoConcorrente", m_NomeProdottoConcorrente);
                writer.Write("NumeroPreventivoConcorrente", m_NumeroPreventivoConcorrente);
                writer.Write("RataConcorrente", m_RataConcorrente);
                writer.Write("DurataConcorrente", m_DurataConcorrente);
                writer.Write("NettoRicavoConcorrente", m_NettoRicavoConcorrente);
                writer.Write("TANConcorrente", m_TANConcorrente);
                writer.Write("TAEGConcorrente", m_TAEGConcorrente);
                writer.Write("IDOffertaIniziale", IDOffertaIniziale);
                writer.Write("InviatoA", m_InviatoA);
                writer.Write("InviatoACC", m_InviatoACC);
                writer.Write("MezzoDiInvio", m_MezzoDiInvio);
                writer.Write("SendSubject", m_SendSubject);
                writer.Write("SendMessange", m_SendMessange);
                writer.Write("SendDate", m_SendDate);
                writer.Write("Attachments", DMD.XML.Utils.Serializer.Serialize(Attachments));
                writer.Write("RicevutoIl", m_RicevutoIl);
                writer.Write("RispostoIl", m_RispostoIl);
                writer.Write("RispostoDa", m_RispostoDa);
                writer.Write("RispostoAMezzo", m_RispostoAMezzo);
                writer.Write("RispostoSubject", m_RispostoSubject);
                writer.Write("RispostoMessage", m_RispostoMessage);
                writer.Write("IDOffertaCorrente", IDOffertaCorrente);
                writer.Write("Flags", m_Flags);
                writer.Write("Parameters", DMD.XML.Utils.Serializer.Serialize(Parameters));
                writer.Write("IDFinestraLavorazione", IDFinestraLavorazione);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("StatoRichiesta", (int?)m_StatoRichiesta);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("IDRichiedente", IDRichiedente);
                writer.WriteAttribute("NomeRichiedente", m_NomeRichiedente);
                writer.WriteAttribute("IDAgenziaConcorrente", IDAgenziaConcorrente);
                writer.WriteAttribute("NomeAgenziaConcorrente", m_NomeAgenziaConcorrente);
                writer.WriteAttribute("NomeProdottoConcorrente", m_NomeProdottoConcorrente);
                writer.WriteAttribute("NumeroPreventivoConcorrente", m_NumeroPreventivoConcorrente);
                writer.WriteAttribute("RataConcorrente", m_RataConcorrente);
                writer.WriteAttribute("DurataConcorrente", m_DurataConcorrente);
                writer.WriteAttribute("NettoRicavoConcorrente", m_NettoRicavoConcorrente);
                writer.WriteAttribute("TANConcorrente", m_TANConcorrente);
                writer.WriteAttribute("TAEGConcorrente", m_TAEGConcorrente);
                writer.WriteAttribute("IDOffertaIniziale", IDOffertaIniziale);
                writer.WriteAttribute("InviatoA", m_InviatoA);
                writer.WriteAttribute("InviatoACC", m_InviatoACC);
                writer.WriteAttribute("MezzoDiInvio", m_MezzoDiInvio);
                writer.WriteAttribute("SendSubject", m_SendSubject);
                writer.WriteAttribute("SendDate", m_SendDate);
                writer.WriteAttribute("RicevutoIl", m_RicevutoIl);
                writer.WriteAttribute("RispostoIl", m_RispostoIl);
                writer.WriteAttribute("RispostoDa", m_RispostoDa);
                writer.WriteAttribute("RispostoAMezzo", m_RispostoAMezzo);
                writer.WriteAttribute("RispostoSubject", m_RispostoSubject);
                writer.WriteAttribute("IDOffertaCorrente", IDOffertaCorrente);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("IDFinestraLavorazione", IDFinestraLavorazione);
                base.XMLSerialize(writer);
                writer.WriteTag("MotivoRichiesta", m_MotivoRichiesta);
                writer.WriteTag("SendMessange", m_SendMessange);
                writer.WriteTag("Attachments", Attachments);
                writer.WriteTag("RispostoMessage", m_RispostoMessage);
                writer.WriteTag("Parameters", Parameters);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
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

                    case "StatoRichiesta":
                        {
                            m_StatoRichiesta = (StatoRichiestaDeroga)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDRichiedente":
                        {
                            m_IDRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRichiedente":
                        {
                            m_NomeRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgenziaConcorrente":
                        {
                            m_IDAgenziaConcorrente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgenziaConcorrente":
                        {
                            m_NomeAgenziaConcorrente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeProdottoConcorrente":
                        {
                            m_NomeProdottoConcorrente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroPreventivoConcorrente":
                        {
                            m_NumeroPreventivoConcorrente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RataConcorrente":
                        {
                            m_RataConcorrente = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DurataConcorrente":
                        {
                            m_DurataConcorrente = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NettoRicavoConcorrente":
                        {
                            m_NettoRicavoConcorrente = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TANConcorrente":
                        {
                            m_TANConcorrente = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEGConcorrente":
                        {
                            m_TAEGConcorrente = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDOffertaIniziale":
                        {
                            m_IDOffertaIniziale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "InviatoA":
                        {
                            m_InviatoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "InviatoACC":
                        {
                            m_InviatoACC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MezzoDiInvio":
                        {
                            m_MezzoDiInvio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SendSubject":
                        {
                            m_SendSubject = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SendDate":
                        {
                            m_SendDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RicevutoIl":
                        {
                            m_RicevutoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RispostoIl":
                        {
                            m_RispostoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RispostoDa":
                        {
                            m_RispostoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RispostoAMezzo":
                        {
                            m_RispostoAMezzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RispostoSubject":
                        {
                            m_RispostoSubject = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOffertaCorrente":
                        {
                            m_IDOffertaCorrente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazione":
                        {
                            m_IDFinestraLavorazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MotivoRichiesta":
                        {
                            m_MotivoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SendMessange":
                        {
                            m_SendMessange = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attachments":
                        {
                            m_Attachments = new CCollection<Sistema.CAttachment>();
                            m_Attachments.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "RispostoMessage":
                        {
                            m_RispostoMessage = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Parameters":
                        {
                            m_Parameters = (CKeyCollection)fieldValue;
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
                return "Richiesta Deroga, Cliente: " + m_NomeCliente + " del : " + Sistema.Formats.FormatUserDateTime(m_DataRichiesta);
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                RichiesteDeroghe.doItemCreated(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                RichiesteDeroghe.doItemDeleted(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                RichiesteDeroghe.doItemModified(new ItemEventArgs(this));
            }

            public void Invia()
            {
                Stato = ObjectStatus.OBJECT_VALID;
                Save();
                OnInviata(new EventArgs());
            }

            protected virtual void OnInviata(EventArgs e)
            {
                RichiesteDeroghe.doOnInviata(new ItemEventArgs(this));
            }

            public void Ricevi()
            {
                Save();
                OnRicevuta(new EventArgs());
            }

            protected virtual void OnRicevuta(EventArgs e)
            {
                RichiesteDeroghe.doOnRicevuta(new ItemEventArgs(this));
            }
        }
    }
}