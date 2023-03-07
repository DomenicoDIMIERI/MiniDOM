using System;
using System.Collections;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Flag che indicano i possibili esiti di un contatto
        /// </summary>
        public enum EsitoChiamata : int
        {
            /// <summary>
            /// Contatto fallito
            /// </summary>
            NESSUNA_RISPOSTA = 0,

            /// <summary>
            /// Contatto avvenuto
            /// </summary>
            OK = 1,

            /// <summary>
            /// Altri esiti
            /// </summary>
            ALTRO = 255
        }

        /// <summary>
        /// Flag di stato della comunicazione
        /// </summary>
        public enum StatoConversazione : int
        {
            /// <summary>
            /// In attesa di stabilire il contatto (attesa di risposta)
            /// </summary>
            INATTESA = 0,

            /// <summary>
            /// Contatto avvenuto ed in corso 
            /// </summary>
            INCORSO = 1,

            /// <summary>
            /// Contatto concluso
            /// </summary>
            CONCLUSO = 2
        }

        /// <summary>
        /// Contatto
        /// </summary>
        [Serializable()]
        public abstract class CContattoUtente 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<CContattoUtente>
        {
            private int m_IDAzienda;
            [NonSerialized] private CAzienda m_Azienda = null;
            private int m_IDPersona; // [int] ID della persona associata
            private string m_NomePersona;
            [NonSerialized] private Anagrafica.CPersona m_Persona; // [CPersona] Oggetto CPersona associato
            private int m_IDContesto;    // [int]  ID del contesto
            private string m_Contesto; // [Text] Tipo del contesto
            private bool m_Ricevuta; // [Bool] Se vero indica che si tratta di una chiamata ricevuta. Se falso si tratta di una chiamata effettuata
            private string m_Scopo; // [Text] Scopo della chiamata
            private DateTime m_Data; // [Date] Data e ora della chiamata
            private int m_IDOperatore; // [Int]  ID dell'operatore che ha effettuato la chiamata o che ha risposto
            [NonSerialized] private Sistema.CUser m_Operatore; // [CUser] Oggetto CUser che rappresenta l'utenza dell'operatore che ha gestito la chiamata
            private string m_NomeOperatore; // [text] Nome dell'operatore
            private string m_Note; // [text] Campo generico
            private EsitoChiamata m_Esito; // [INT] 0 nessuna risposta/non trovato, 1 Risposto/trovato, 255 altro (vedi dettaglio)
            private string m_DettaglioEsito; // [TEXT] Stringa che aggiunge informazioni sull'esito
            private double m_Durata; // Durata del contatto in secondi
            private string m_MessageID;
            private string m_NumeroOIndirizzo;
            private string m_NomeIndirizzo;
            private CIndirizzo m_Luogo = null;
            private double m_Attesa;
            private StatoConversazione m_StatoConversazione;
            private StatoConversazione m_OldStatoConversazione;
            private int m_IDAccoltoDa;
            private string m_NomeAccoltoDa;
            [NonSerialized] private Sistema.CUser m_AccoltoDa;
            private DateTime? m_DataRicezione;
            private int m_IDPerContoDi;
            [NonSerialized] private Anagrafica.CPersona m_PerContoDi;
            private string m_NomePerContoDi;
            [NonSerialized] private CCollection<TrasferimentoContatto> m_Trasferimenti;
            private decimal? m_Costo;
            [NonSerialized] private CCollection<Sistema.CAttachment> m_Attachments;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoUtente()
            {
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_IDContesto = 0;
                m_Contesto = null;
                m_Ricevuta = true;
                m_Scopo = "";
                m_Data = DMD.DateUtils.ToDay();
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_Note = "";
                m_Esito = 0;
                m_DettaglioEsito = "";
                m_Durata = 0f;
                m_MessageID = DMD.Strings.vbNullString;
                m_NumeroOIndirizzo = DMD.Strings.vbNullString;
                m_NomeIndirizzo = DMD.Strings.vbNullString;
                m_IDAzienda = 0;
                m_Azienda = null;
                m_Attesa = 0f;
                m_StatoConversazione = StatoConversazione.INATTESA;
                m_OldStatoConversazione = StatoConversazione.INATTESA;
                m_IDAccoltoDa = 0;
                m_NomeAccoltoDa = "";
                m_AccoltoDa = null;
                m_DataRicezione = default;
                m_IDPerContoDi = 0;
                m_PerContoDi = null;
                m_NomePerContoDi = "";
                m_Trasferimenti = null;
                m_Costo = default;
                m_Attachments = null;
            }

            /// <summary>
            /// Restituisce l'indirizzo relativo al contatto
            /// </summary>
            public Anagrafica.CIndirizzo Luogo
            {
                get
                {
                    if (this.m_Luogo is null)
                    {
                        this.m_Luogo = this.Parameters.GetItemByKey("Luogo") as CIndirizzo;
                        if (this.m_Luogo is null)
                            this.m_Luogo = new CIndirizzo();
                        this.Parameters.SetItemByKey("Luogo", this.m_Luogo);
                    }
                    return m_Luogo;
                }

                set
                {
                    var oldValue = this.m_Luogo;
                    if (oldValue == value)
                        return;
                    if (value is null)
                    {
                        m_Luogo = new CIndirizzo();
                        this.Parameters.SetItemByKey("Luogo", this.m_Luogo);
                    }
                    else
                    {
                        DMD.RunTime.CopyFrom(this.m_Luogo, value, false);
                    }

                    DoChanged("Luogo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione degli allegati associati al contatto
            /// </summary>
            public CCollection<Sistema.CAttachment> Attachments
            {
                get
                {
                    if (m_Attachments is null)
                    {
                        m_Attachments = new CCollection<Sistema.CAttachment>();
                        var tmp = this.Parameters.GetItemByKey("Attachments") as CCollection;
                        if (tmp is object)
                            this.m_Attachments.AddRange(tmp);
                        this.Parameters.SetItemByKey("Attachments", this.m_Attachments);
                    }
                    return m_Attachments;
                }
            }

            /// <summary>
            /// Restituisce il repository dei contatti
            /// </summary>
            /// <returns></returns>
            public abstract CContattiRepository GetContattoRepository();

            /// <summary>
            /// Restituisce il repository dei contatti
            /// </summary>
            /// <returns></returns>
            public sealed override CModulesClass GetModule()
            {
                return this.GetContattoRepository();
            }

            /// <summary>
            /// Restituisce o imposta il costo totale delle telefonata, sms ecc..
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? Costo
            {
                get
                {
                    return m_Costo;
                }

                set
                {
                    var oldValue = m_Costo;
                    if (oldValue == value == true)
                        return;
                    m_Costo = value;
                    DoChanged("Costo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione dei trasferimenti
            /// </summary>
            public CCollection<TrasferimentoContatto> Trasferimenti
            {
                get
                {
                    lock (this)
                    {
                        if (this.m_Trasferimenti is null)
                        {
                            this.m_Trasferimenti = new CCollection<TrasferimentoContatto>();
                            var tmp = this.Parameters.GetItemByKey("Trasferimenti") as CCollection;
                            if (tmp is object)
                                this.m_Trasferimenti.AddRange(tmp);
                            this.Parameters.SetItemByKey("Trasferimenti", this.m_Trasferimenti);
                        }
                        return this.m_Trasferimenti;
                    }
                }
            }

            /// <summary>
            /// Trasferisce ad un altro utente 
            /// </summary>
            /// <param name="trasferisciA"></param>
            /// <param name="messaggio"></param>
            /// <returns></returns>
            public TrasferimentoContatto Trasferisci(Sistema.CUser trasferisciA, string messaggio)
            {
                if (trasferisciA is null)
                    throw new ArgumentNullException("trasferisciA");
                StatoConversazione = StatoConversazione.INATTESA;
                var t = new TrasferimentoContatto();
                t.Contatto = this;
                t.Operatore = Sistema.Users.CurrentUser;
                t.DataTrasferimento = DMD.DateUtils.Now();
                t.TrasferitoA = trasferisciA;
                t.Messaggio = messaggio;
                Trasferimenti.Add(t);
                SetChanged(true);
                Save();
                return t;
            }

            
            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha ricevuto per la prima volta il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAccoltoDa
            {
                get
                {
                    return DBUtils.GetID(m_AccoltoDa, m_IDAccoltoDa);
                }

                set
                {
                    int oldValue = IDAccoltoDa;
                    if (oldValue == value)
                        return;
                    m_IDAccoltoDa = value;
                    m_AccoltoDa = null;
                    DoChanged("IDAccoltoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha ricevuto per la prima volta il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser AccoltoDa
            {
                get
                {
                    if (m_AccoltoDa is null)
                        m_AccoltoDa = Sistema.Users.GetItemById(m_IDAccoltoDa);
                    return m_AccoltoDa;
                }

                set
                {
                    var oldValue = m_AccoltoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AccoltoDa = value;
                    m_IDAccoltoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAccoltoDa = value.Nominativo;
                    DoChanged("AccoltoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha ricevuto per la prima volta il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAccoltoDa
            {
                get
                {
                    return m_NomeAccoltoDa;
                }

                set
                {
                    string oldValue = m_NomeAccoltoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAccoltoDa = value;
                    DoChanged("NomeAccoltoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituiscec o imposta la data di ricezione del cliente (senza l'attesa)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataRicezione
            {
                get
                {
                    return m_DataRicezione;
                }

                set
                {
                    var oldValue = m_DataRicezione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRicezione = value;
                    DoChanged("DataRicezione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della conversazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoConversazione StatoConversazione
            {
                get
                {
                    return m_StatoConversazione;
                }

                set
                {
                    var oldValue = m_StatoConversazione;
                    if (oldValue == value)
                        return;
                    m_StatoConversazione = value;
                    DoChanged("StatoConversazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azienda che ha registrato la pratica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Azienda, m_IDAzienda);
                }

                set
                {
                    int oldValue = IDAzienda;
                    if (oldValue == value)
                        return;
                    m_IDAzienda = value;
                    m_Azienda = null;
                    DoChanged("IDAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azienda che ha registrato la pratica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda Azienda
            {
                get
                {
                    if (m_Azienda is null)
                        m_Azienda = Anagrafica.Aziende.GetItemById(m_IDAzienda);
                    return (Anagrafica.CAzienda)m_Azienda;
                }

                set
                {
                    Anagrafica.CAzienda oldValue = (Anagrafica.CAzienda)m_Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Azienda = value;
                    m_IDAzienda = DBUtils.GetID(value, 0);
                    DoChanged("Azienda", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'azienda
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetAzienda(Anagrafica.CAzienda value)
            {
                m_Azienda = value;
                m_IDAzienda = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public virtual string NumeroOIndirizzo
            {
                get
                {
                    return m_NumeroOIndirizzo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NumeroOIndirizzo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroOIndirizzo = value;
                    DoChanged("NumeroOIndirizzo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'etichetta dell'indirizzo utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeIndirizzo
            {
                get
                {
                    return m_NomeIndirizzo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeIndirizzo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIndirizzo = value;
                    DoChanged("NomeIndirizzo", value, oldValue);
                }
            }

            

            /// <summary>
            /// Restituisce o imposta l'identificativo del messaggio in un sistema esterno
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string MessageID
            {
                get
                {
                    return m_MessageID;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MessageID;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MessageID = value;
                    DoChanged("MessageID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la durata (in secondi) della comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    double oldValue = m_Durata;
                    if (oldValue == value)
                        return;
                    m_Durata = value;
                    DoChanged("Durata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'attesa (tempo prima della risposta in caso di una telefonata o tempo in sala di attesa per una visita)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Attesa
            {
                get
                {
                    return m_Attesa;
                }

                set
                {
                    double oldValue = m_Attesa;
                    if (oldValue == value)
                        return;
                    m_Attesa = value;
                    DoChanged("Attesa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona con cui è avvenuta la comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// 'Restituisce o imposta la persona con cui è avvenuta la comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(Anagrafica.CPersona value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona con cui è avvenuta la comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona per cui è stata fatta la chiamata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPerContoDi
            {
                get
                {
                    return DBUtils.GetID(m_PerContoDi, m_IDPerContoDi);
                }

                set
                {
                    int oldValue = IDPerContoDi;
                    if (oldValue == value)
                        return;
                    m_IDPerContoDi = value;
                    m_PerContoDi = null;
                    DoChanged("IDPerContoDi", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona per cui è stata fatta la chiamata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona PerContoDi
            {
                get
                {
                    if (m_PerContoDi is null)
                        m_PerContoDi = Anagrafica.Persone.GetItemById(m_IDPerContoDi);
                    return m_PerContoDi;
                }

                set
                {
                    var oldValue = PerContoDi;
                    if (oldValue == value)
                        return;
                    m_PerContoDi = value;
                    m_IDPerContoDi = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePerContoDi = value.Nominativo;
                    DoChanged("PerContoDi", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la persona di riferimento
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPerContoDi(Anagrafica.CPersona value)
            {
                m_PerContoDi = value;
                m_IDPerContoDi = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona per cui è stata fatta la chiamata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePerContoDi
            {
                get
                {
                    return m_NomePerContoDi;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomePerContoDi;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePerContoDi = value;
                    DoChanged("NomePerContoDi", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o i mposta l'ID del contesto in cui è avvenuta la comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContesto
            {
                get
                {
                    return m_IDContesto;
                }

                set
                {
                    int oldValue = m_IDContesto;
                    if (oldValue == value)
                        return;
                    m_IDContesto = value;
                    DoChanged("IDConstesto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del contesto in cui è avvenuta la comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Contesto
            {
                get
                {
                    return m_Contesto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Contesto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Contesto = value;
                    DoChanged("Contesto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce vero se la comunicazione è in ingresso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Ricevuta
            {
                get
                {
                    return m_Ricevuta;
                }

                set
                {
                    if (m_Ricevuta == value)
                        return;
                    m_Ricevuta = value;
                    DoChanged("Ricevuta", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che indica lo scopo della comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Scopo
            {
                get
                {
                    return m_Scopo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Scopo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Scopo = value;
                    DoChanged("Scopo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data della comunicazione
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
            /// Restituisce o imposta l'ID dell'operatore che ha seguito la conversazione
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
            /// Restituisce o imposta il nome dell'operatore che ha seguito la conversazione
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
            /// Restituisce o imposta l'operatore che ha seguito la conversazione
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
                    var oldValue = Operatore;
                    if (oldValue == value)
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il testo della conversazione
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
            /// Restituisce o imposta l'esito della conversazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public EsitoChiamata Esito
            {
                get
                {
                    return m_Esito;
                }

                set
                {
                    var oldValue = m_Esito;
                    if (oldValue == value)
                        return;
                    m_Esito = value;
                    DoChanged("Esito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che specifica l'esito della conversazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioEsito
            {
                get
                {
                    return m_DettaglioEsito;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_DettaglioEsito;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsito = value;
                    DoChanged("DettaglioEsito", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione dell'attività
            /// </summary>
            public virtual string DescrizioneAttivita
            {
                get
                {
                    return "Contatto utente";
                }
            }

            /// <summary>
            /// Restituisce un oggetto CCollection di CAzioneProposta contenente le azioni proposte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public virtual CCollection<CAzioneProposta> AzioniProposte
            {
                get
                {
                    var ret = new CCollection();
                    return (CCollection<CAzioneProposta>)ret;
                }
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CContattoUtente)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CContattoUtente obj)
            {
                var ret = DMD. DateUtils.Compare(this.Data, obj.Data);
                if (ret == 0) ret = DMD.Strings.Compare(NomePersona, obj.NomePersona, true);
                return ret;
            }

            /// <summary>
            /// Serializza l'oggetto
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("IDContesto", IDContesto);
                writer.WriteAttribute("Contesto", m_Contesto);
                writer.WriteAttribute("Ricevuta", m_Ricevuta);
                writer.WriteAttribute("Scopo", m_Scopo);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("Esito", (int?)m_Esito);
                writer.WriteAttribute("DettaglioEsito", m_DettaglioEsito);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("MessageID", m_MessageID);
                writer.WriteAttribute("NumeroOIndirizzo", m_NumeroOIndirizzo);
                writer.WriteAttribute("NomeIndirizzo", m_NomeIndirizzo);
                writer.WriteAttribute("IDAzienda", IDAzienda);
                writer.WriteAttribute("Attesa", m_Attesa);
                writer.WriteAttribute("StatoConversazione", m_StatoConversazione);
                writer.WriteAttribute("OldStatoConversazione", m_OldStatoConversazione);
                writer.WriteAttribute("IDAccoltoDa", IDAccoltoDa);
                writer.WriteAttribute("NomeAccoltoDa", m_NomeAccoltoDa);
                writer.WriteAttribute("DataRicezione", m_DataRicezione);
                writer.WriteAttribute("IDPerContoDi", IDPerContoDi);
                writer.WriteAttribute("NomePerContoDi", m_NomePerContoDi);
                writer.WriteAttribute("Costo", m_Costo);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
            }

            /// <summary>
            /// Deserializza l'oggetto
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Contesto":
                        {
                            m_Contesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Ricevuta":
                        {
                            m_Ricevuta = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Scopo":
                        {
                            m_Scopo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            m_Esito = (EsitoChiamata)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            m_DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = (double)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MessageID":
                        {
                            m_MessageID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroOIndirizzo":
                        {
                            m_NumeroOIndirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeIndirizzo":
                        {
                            m_NomeIndirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attesa":
                        {
                            m_Attesa = (double)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "StatoConversazione":
                        {
                            m_StatoConversazione = (StatoConversazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OldStatoConversazione":
                        {
                            m_OldStatoConversazione = (StatoConversazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

               
                    case "Costo":
                        {
                            m_Costo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                     
                    case "IDAccoltoDa":
                        {
                            m_IDAccoltoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAccoltoDa":
                        {
                            m_NomeAccoltoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRicezione":
                        {
                            m_DataRicezione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPerContoDi":
                        {
                            m_IDPerContoDi = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePerContoDi":
                        {
                            m_NomePerContoDi = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Telefonate";
            }

            /// <summary>
            /// Restituisce il nome del tipo oggetto
            /// </summary>
            /// <returns></returns>
            public abstract string GetNomeTipoOggetto();

            /// <summary>
            /// Genera l'evento Concluso
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnConcluso(ItemEventArgs<CContattoUtente> e)
            {
                var repo = this.GetContattoRepository();
                minidom.CustomerCalls.CRM.OnContattoConcluso(e);
                var message = DMD.Strings.ConcatArray(
                                GetNomeTipoOggetto(), " ", (Ricevuta) ? "ricevuta" : "effettuata", DMD.Strings.vbCrLf,
                                "Cliente: ", NomePersona, DMD.Strings.vbCrLf,
                                "Scopo: ", Scopo
                                );
                var e1 = new Sistema.EventDescription("ContattoConcluso", message, this);
                repo.DispatchEvent(e1);
            }

            /// <summary>
            /// Genera l'evento Iniziato
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnIniziato(ItemEventArgs<CContattoUtente> e)
            {
                var repo = this.GetContattoRepository();
                minidom.CustomerCalls.CRM.OnContattoIniziato(e);
                var message = DMD.Strings.ConcatArray(
                                GetNomeTipoOggetto(), " ", (Ricevuta) ? "ricevuta" : "effettuata", DMD.Strings.vbCrLf,
                                "Cliente: ", NomePersona, DMD.Strings.vbCrLf,
                                "Scopo: ", Scopo
                                );
                var e1 = new Sistema.EventDescription("ContattoIniziato", message, this);
                repo.DispatchEvent(e1);
            }





            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                if (base.IsChanged() || this.m_Luogo.IsChanged())
                    return true;

                if (m_Trasferimenti is object)
                {
                    foreach (TrasferimentoContatto t in m_Trasferimenti)
                    {
                        if (t.IsChanged())
                            return true;
                    }
                }
                if (m_Attachments is object)
                {
                    foreach (var a in m_Attachments)
                    {
                        if (a.IsChanged())
                            return true;
                    }
                }

                return false;
            }


            /// <summary>
            /// Prepara il salvataggio
            /// </summary>
            /// <param name="e"></param>
            protected override void OnBeforeSave(DMDEventArgs e)
            {
                this.Parameters.SetItemByKey("Luogo", this.Luogo);
                this.Parameters.SetItemByKey("Attachments", this.Attachments);
                this.Parameters.SetItemByKey("Trasferimenti", this.Trasferimenti);
                base.OnBeforeSave(e);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPersona = reader.Read("IDPersona",  m_IDPersona);
                m_NomePersona = reader.Read("NomePersona",  m_NomePersona);
                m_IDContesto = reader.Read("IDContesto",  m_IDContesto);
                m_Contesto = reader.Read("TipoContesto",  m_Contesto);
                m_Ricevuta = reader.Read("Ricevuta",  m_Ricevuta);
                m_Scopo = reader.Read("Scopo",  m_Scopo);
                m_Data = reader.Read("Data",  m_Data);
                m_IDOperatore = reader.Read("IDOperatore",  m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore",  m_NomeOperatore);
                m_Esito = reader.Read("Esito",  m_Esito);
                m_DettaglioEsito = reader.Read("DettaglioEsito",  m_DettaglioEsito);
                m_Durata = reader.Read("Durata",  m_Durata);
                m_MessageID = reader.Read("MessageID",  m_MessageID);
                m_NumeroOIndirizzo = reader.Read("Numero",  m_NumeroOIndirizzo);
                m_NomeIndirizzo = reader.Read("Indirizzo_Nome",  m_NomeIndirizzo);
                m_IDAzienda = reader.Read("IDAzienda",  m_IDAzienda);
                m_Attesa = reader.Read("Attesa",  m_Attesa);
                m_StatoConversazione = reader.Read("StatoConversazione",  m_StatoConversazione);
                m_OldStatoConversazione = m_StatoConversazione;
                m_IDAccoltoDa = reader.Read("IDAccoltoDa",  m_IDAccoltoDa);
                m_NomeAccoltoDa = reader.Read("NomeAccoltoDa",  m_NomeAccoltoDa);
                m_DataRicezione = reader.Read("DataRicezione",  m_DataRicezione);
                m_IDPerContoDi = reader.Read("IDPerContoDi",  m_IDPerContoDi);
                m_NomePerContoDi = reader.Read("NomePerContoDi",  m_NomePerContoDi);
                m_Costo = reader.Read("Costo",  m_Costo);
                if (DMD.Strings.Trim(Sistema.ApplicationContext.Settings.GetValueString("CRM.SaveNotesToDB")) == "1")
                {
                    m_Note = reader.Read("Note",  m_Note);
                }

                bool ret = base.LoadFromRecordset(reader);
               
                if (DMD.Strings.Trim(Sistema.ApplicationContext.Settings.GetValueString("CRM.SaveNotesToDB")) != "1")
                {
                    string fName = Sistema.ApplicationContext.SystemDataFolder 
                                   + @"\telefonate\note" + Sistema.RPC.FormatID(DBUtils.GetID(this, 0)) + ".dat";
                    if (System.IO.File.Exists(fName)) 
                        m_Note = System.IO.File.ReadAllText(fName);
                    
                }

                return ret;
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ClassName", DMD.RunTime.vbTypeName(this));
                writer.Write("Ricevuta", m_Ricevuta);
                writer.Write("Scopo", m_Scopo);
                writer.Write("Data", m_Data);
                //writer.Write("DataStr", DBUtils.ToDBDateStr(m_Data));
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("IDContesto", IDContesto);
                writer.Write("TipoContesto", m_Contesto);
                writer.Write("Esito", m_Esito);
                writer.Write("DettaglioEsito", m_DettaglioEsito);
                writer.Write("Durata", m_Durata);
                writer.Write("MessageID", m_MessageID);
                writer.Write("Numero", m_NumeroOIndirizzo);
                writer.Write("Indirizzo_Nome", m_NomeIndirizzo);
                writer.Write("IDAzienda", IDAzienda);
                writer.Write("Attesa", m_Attesa);
                writer.Write("StatoConversazione", m_StatoConversazione);
                writer.Write("IDAccoltoDa", IDAccoltoDa);
                writer.Write("NomeAccoltoDa", m_NomeAccoltoDa);
                writer.Write("DataRicezione", m_DataRicezione);
                writer.Write("IDPerContoDi", IDPerContoDi);
                writer.Write("NomePerContoDi", m_NomePerContoDi);
                writer.Write("Costo", m_Costo);
                if (DMD.Strings.Trim(Sistema.ApplicationContext.Settings.GetValueString("CRM.SaveNotesToDB")) == "1")
                {
                    writer.Write("Note", m_Note);
                    //writer.Write("Options", DMD.XML.Utils.Serializer.Serialize(Options));
                }
                else
                {
                    writer.Write("Note", DMD.Strings.vbNullString);
                    //writer.Write("Options", "");
                }

                return base.SaveToRecordset(writer);
            }

           

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("ClassName", typeof(string), 255);
                c = table.Fields.Ensure("Ricevuta", typeof(bool), 1);
                c = table.Fields.Ensure("Scopo", typeof(string), 255);
                c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("IDContesto", typeof(int), 1);
                c = table.Fields.Ensure("TipoContesto", typeof(string), 255);
                c = table.Fields.Ensure("Esito", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioEsito", typeof(string), 255);
                c = table.Fields.Ensure("Durata", typeof(double), 1);
                c = table.Fields.Ensure("MessageID", typeof(string), 255);
                c = table.Fields.Ensure("Numero", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Nome", typeof(string), 255);
                c = table.Fields.Ensure("IDAzienda", typeof(int), 1);
                c = table.Fields.Ensure("Attesa", typeof(double), 1);
                c = table.Fields.Ensure("StatoConversazione", typeof(int), 1);
                c = table.Fields.Ensure("IDAccoltoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeAccoltoDa", typeof(string), 255);
                c = table.Fields.Ensure("DataRicezione", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDPerContoDi", typeof(int), 1);
                c = table.Fields.Ensure("NomePerContoDi", typeof(string), 255);
                c = table.Fields.Ensure("Costo", typeof(Decimal), 1);
                c = table.Fields.Ensure("Note", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxClass", new string[] { "ClassName", "MessageID"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumero", new string[] { "Numero", "Indirizzo_Nome" }, DBFieldConstraintFlags.None);

                c = table.Constraints.Ensure("idxDate", new string[] { "Data", "IDAzienda", "Ricevuta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxScopo", new string[] { "StatoConversazione", "Scopo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContesto", new string[] { "IDContesto", "TipoContesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEsito", new string[] { "Esito", "DettaglioEsito" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStats", new string[] { "Durata", "Attesa", "Costo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPerContoDi", new string[] { "IDPerContoDi", "NomePerContoDi" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAccolto", new string[] { "IDAccoltoDa", "DataRicezione", "NomeAccoltoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "Note" }, DBFieldConstraintFlags.None);
                 
                 
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DescrizioneAttivita;
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CContattoUtente) && this.Equals((CContattoUtente)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CContattoUtente obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.m_IDAzienda, obj.m_IDAzienda)
                     && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                     && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.Integers.EQ(this.m_IDContesto, obj.m_IDContesto)
                    && DMD.Strings.EQ(this.m_Contesto, obj.m_Contesto)
                    && DMD.Booleans.EQ(this.m_Ricevuta, obj.m_Ricevuta)
                    && DMD.Strings.EQ(this.m_Scopo, obj.m_Scopo)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Integers.EQ((int)this.m_Esito, (int)obj.m_Esito)
                    && DMD.Strings.EQ(this.m_DettaglioEsito, obj.m_DettaglioEsito)
                    && DMD.Doubles.EQ(this.m_Durata, obj.m_Durata)
                    && DMD.Strings.EQ(this.m_MessageID, obj.m_MessageID)
                    && DMD.Strings.EQ(this.m_NumeroOIndirizzo, obj.m_NumeroOIndirizzo)
                    && DMD.Strings.EQ(this.m_NomeIndirizzo, obj.m_NomeIndirizzo)
                    && this.Luogo.Equals(obj.Luogo)
                    && DMD.Doubles.EQ(this.m_Attesa, obj.m_Attesa)
                    && DMD.Integers.EQ((int)this.m_StatoConversazione, (int)obj.m_StatoConversazione)
                    && DMD.Integers.EQ((int)this.m_OldStatoConversazione, (int)obj.m_OldStatoConversazione)
                    && DMD.Integers.EQ(this.m_IDAccoltoDa, obj.m_IDAccoltoDa)
                    && DMD.Strings.EQ(this.m_NomeAccoltoDa, obj.m_NomeAccoltoDa)
                    && DMD.DateUtils.EQ(this.m_DataRicezione, obj.m_DataRicezione)
                    && DMD.Integers.EQ(this.m_IDPerContoDi, obj.m_IDPerContoDi)
                    && DMD.Strings.EQ(this.m_NomePerContoDi, obj.m_NomePerContoDi)
                    && DMD.Decimals.EQ(this.m_Costo, obj.m_Costo)
                    ;
            }

            /// <summary>
            /// Gestisce i contatti in attesa nel relativo repository
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                this.Parameters.SetChanged(false);
                bool isIniziato = m_OldStatoConversazione == StatoConversazione.INATTESA
                                  && m_StatoConversazione == StatoConversazione.INCORSO;
                bool isConcluso =    m_OldStatoConversazione != StatoConversazione.CONCLUSO 
                                  && m_StatoConversazione    == StatoConversazione.CONCLUSO;

                m_OldStatoConversazione = m_StatoConversazione;
                base.OnAfterSave(e);
                var repo = this.GetContattoRepository();
                if (
                        this.Stato == ObjectStatus.OBJECT_VALID 
                    && (
                           StatoConversazione == StatoConversazione.INATTESA
                        || StatoConversazione == StatoConversazione.INCORSO
                        )
                    )
                {
                    repo.SetInAttesa(this);
                }
                else
                {
                    repo.SetFineAttesa(this);
                }


                foreach (TrasferimentoContatto t in m_Trasferimenti)
                    t.SetChanged(false);

                if (DMD.Strings.Trim(Sistema.ApplicationContext.Settings.GetValueString("CRM.SaveNotesToDB")) != "1")
                {
                    string fName = Sistema.ApplicationContext.SystemDataFolder
                                   + @"\telefonate\note" + Sistema.RPC.FormatID(DBUtils.GetID(this), 0) + ".dat";
                    //string fName1 = Sistema.ApplicationContext.SystemDataFolder
                    //               + @"\telefonate\opt" + Sistema.RPC.FormatID(DBUtils.GetID(this, 0)) + ".dat";
                    Sistema.FileSystem.CreateRecursiveFolder(System.IO.Path.GetDirectoryName(fName));
                    System.IO.File.WriteAllText(fName, m_Note);
                    //System.IO.File.WriteAllText(fName1, DMD.XML.Utils.Serializer.Serialize(Options));
                }


                if (this.Stato == ObjectStatus.OBJECT_VALID)
                {
                    if (isIniziato) this.OnIniziato(new ItemEventArgs<CContattoUtente>(this));
                    else if (isConcluso) this.OnConcluso(new ItemEventArgs<CContattoUtente>(this));
                }
            }
        }
    }
}