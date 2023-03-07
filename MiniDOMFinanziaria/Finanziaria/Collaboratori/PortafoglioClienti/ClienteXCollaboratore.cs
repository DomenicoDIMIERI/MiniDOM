using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Flag di stato di un cliente assegnato ad un collaboratore
        /// </summary>
        public enum StatoClienteCollaboratore : int
        {
            /// <summary>
            /// Non attivo
            /// </summary>
            NONATTIVO = 0,

            /// <summary>
            /// Attivo
            /// </summary>
            ATTIVO = 10,

            /// <summary>
            /// In contatto
            /// </summary>
            CONTATTO = 20,

            /// <summary>
            /// Cliente da caricare
            /// </summary>
            RICHIESTACARICAMENTO = 25,

            /// <summary>
            /// Cliente caricato
            /// </summary>
            CARICATO = 30,

            /// <summary>
            /// Cliente concluso correttamente
            /// </summary>
            LIQUIDATO = 40,

            /// <summary>
            /// Cliente annullato
            /// </summary>
            ANNULLATO = 50
        }


        /// <summary>
        /// Flag per una relazione cliente x collaboratore
        /// </summary>
        [Flags]
        public enum ClienteCollaboratoreFlags 
            : int
        {
            /// <summary>
            /// Nessun flags
            /// </summary>
            None = 0,

            /// <summary>
            /// Si tratta di un cliente assegnato dall'agenzia
            /// </summary>
            AssegnatoDaAgenzia = 1
        }

        /// <summary>
        /// Cliente assegnato ad un collaboratore
        /// </summary>
        [Serializable]
        public class ClienteXCollaboratore 
            : Databases.DBObject, IComparable, IComparable<ClienteXCollaboratore>
        {
            private int m_IDPersona;
            [NonSerialized] private Anagrafica.CPersonaFisica m_Persona;
            private string m_Nome;
            private string m_Cognome;
            private string m_CodiceFiscale;
            private DateTime? m_DataNascita;
            private Anagrafica.CIndirizzo m_Indirizzo;
            private int m_IDCollaboratore;
            [NonSerialized] private CCollaboratore m_Collaboratore;
            private DateTime? m_DataAcquisizione;
            private string m_TipoFonte;
            private int m_IDFonte;
            private IFonte m_Fonte;
            private string m_NomeFonte;
            private StatoClienteCollaboratore m_StatoLavorazione;
            private string m_DettaglioStatoLavorazione;
            private string m_NomeAmministrazione;
            private string m_TelefonoCasa;
            private string m_TelefonoUfficio;
            private string m_TelefonoCellulare;
            private string m_Fax;
            private string m_AltroTelefono;
            private string m_eMailPersonale;
            private string m_eMailUfficio;
            private string m_PEC;
            private DateTime? m_DataRinnovoCQS;
            private string m_MotivoRicontatto;
            private DateTime? m_DataRinnovoPD;
            private string m_ImportoRichiesto;
            private string m_MotivoRichiesta;
            private DateTime? m_DataRichiesta;
            private int m_IDConsulente;
            [NonSerialized] private CConsulentePratica m_Consulente;
            private DateTime? m_DataAssegnazione;
            private string m_MotivoAssegnazione;
            private int m_IDAssegnatoDa;
            [NonSerialized] private Sistema.CUser m_AssegnatoDa;
            private DateTime? m_DataRimozione;
            private string m_MotivoRimozione;
            private int m_IDRimossoDa;
            [NonSerialized] private Sistema.CUser m_RimossoDa;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ClienteXCollaboratore()
            {
                m_IDPersona = 0;
                m_Persona = null;
                m_Nome = "";
                m_Cognome = "";
                m_CodiceFiscale = "";
                m_DataNascita = default;
                m_Indirizzo = new Anagrafica.CIndirizzo("Indirizzo");
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
                m_DataAcquisizione = default;
                m_TipoFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = "";
                m_Flags = (int) ClienteCollaboratoreFlags.None;
                m_StatoLavorazione = StatoClienteCollaboratore.NONATTIVO;
                m_DettaglioStatoLavorazione = "";
                m_NomeAmministrazione = "";
                m_TelefonoCasa = "";
                m_TelefonoUfficio = "";
                m_TelefonoCellulare = "";
                m_Fax = "";
                m_AltroTelefono = "";
                m_eMailPersonale = "";
                m_eMailUfficio = "";
                m_PEC = "";
                m_DataRinnovoCQS = default;
                m_MotivoRicontatto = "";
                m_DataRinnovoPD = default;
                m_ImportoRichiesto = "";
                m_MotivoRichiesta = "";
                m_DataRichiesta = default;
                m_IDConsulente = 0;
                m_Consulente = null;
                m_DataAssegnazione = default;
                m_MotivoAssegnazione = "";
                m_IDAssegnatoDa = 0;
                m_AssegnatoDa = null;
                m_DataRimozione = default;
                m_MotivoRimozione = "";
                m_IDRimossoDa = 0;
                m_RimossoDa = null;
            }

            /// <summary>
            /// Data in cui il cliente è stato tolto al collaboratore
            /// </summary>
            public DateTime? DataRimozione
            {
                get
                {
                    return m_DataRimozione;
                }

                set
                {
                    var oldValue = m_DataRimozione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRimozione = value;
                    DoChanged("DataRimozione", value, oldValue);
                }
            }

            /// <summary>
            /// Motivo della rimozione
            /// </summary>
            public string MotivoRimozione
            {
                get
                {
                    return m_MotivoRimozione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MotivoRimozione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoRimozione = value;
                    DoChanged("MotivoRimozione", value, oldValue);
                }
            }

            /// <summary>
            /// Id dell'utente che ha tolto il cliente
            /// </summary>
            public int IDRimossoDa
            {
                get
                {
                    return DBUtils.GetID(m_RimossoDa, m_IDRimossoDa);
                }

                set
                {
                    int oldValue = IDRimossoDa;
                    if (oldValue == value)
                        return;
                    m_IDRimossoDa = value;
                    m_RimossoDa = null;
                    DoChanged("IDRimossoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Utente che ha rimosso il cliente
            /// </summary>
            public Sistema.CUser RimossoDa
            {
                get
                {
                    if (m_RimossoDa is null)
                        m_RimossoDa = minidom.Sistema.Users.GetItemById(m_IDRimossoDa);
                    return m_RimossoDa;
                }

                set
                {
                    var oldValue = RimossoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RimossoDa = value;
                    m_IDRimossoDa = DBUtils.GetID(value, 0);
                    DoChanged("RimossoDa", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID del consulente che segue il cliente per il collaboratore
            /// </summary>
            /// <returns></returns>
            public int IDConsulente
            {
                get
                {
                    return DBUtils.GetID(m_Consulente, m_IDConsulente);
                }

                set
                {
                    int oldValue = IDConsulente;
                    if (oldValue == value)
                        return;
                    m_IDConsulente = value;
                    m_Consulente = null;
                    DoChanged("IDConsulente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il consulente che segue il cliente per il collaboratore
            /// </summary>
            /// <returns></returns>
            public CConsulentePratica Consulente
            {
                get
                {
                    if (m_Consulente is null)
                        m_Consulente = minidom.Finanziaria.Consulenti.GetItemById(m_IDConsulente);
                    return m_Consulente;
                }

                set
                {
                    var oldValue = Consulente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Consulente = value;
                    m_IDConsulente = DBUtils.GetID(value, 0);
                    DoChanged("Consulente", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il consulente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetConsulente(CConsulentePratica value)
            {
                m_Consulente = value;
                m_IDConsulente = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il motivo del ricontatto fissato dal collaboratore
            /// </summary>
            /// <returns></returns>
            public string MotivoRicontatto
            {
                get
                {
                    return m_MotivoRicontatto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MotivoRicontatto;
                    m_MotivoRicontatto = value;
                    DoChanged("MotivoRicontatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di richiesta caricamento fatta dal collaboratore al gruppo dei consulenti o al teammanager
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
            /// Restituisce o imposta l'importo richiesto dal cliente al collaboratore
            /// </summary>
            /// <returns></returns>
            public string ImportoRichiesto
            {
                get
                {
                    return m_ImportoRichiesto;
                }

                set
                {
                    string oldValue = m_ImportoRichiesto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ImportoRichiesto = value;
                    DoChanged("ImportoRichiesto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo della richiesta di finanziamento fatta dal cliente al collaboratore
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MotivoRichiesta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoRichiesta = value;
                    DoChanged("MotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisec o imposta la data di ricontatto fissata dal collaboratore
            /// </summary>
            /// <returns></returns>
            public DateTime? DataRinnovoCQS
            {
                get
                {
                    return m_DataRinnovoCQS;
                }

                set
                {
                    var oldValue = m_DataRinnovoCQS;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRinnovoCQS = value;
                    DoChanged("DataRinnovoCQS", value, oldValue);
                }
            }

            /// <summary>
            /// Data di rinnovo della delega
            /// </summary>
            public DateTime? DataRinnovoPD
            {
                get
                {
                    return m_DataRinnovoPD;
                }

                set
                {
                    var oldValue = m_DataRinnovoPD;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRinnovoPD = value;
                    DoChanged("DataRinnovoPD", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona fisica nel database principale associata al cliente gestito dal collaboratore
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta la persona fisica nel database principale associata al cliente gestito dal collaboratore
            /// </summary>
            /// <returns></returns>
            public CPersonaFisica Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del cliente registrato dal collaboratore
            /// </summary>
            /// <returns></returns>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    string oldValue = m_Nome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il cognome del cliente registrato dal collaboratore
            /// </summary>
            /// <returns></returns>
            public string Cognome
            {
                get
                {
                    return m_Cognome;
                }

                set
                {
                    string oldValue = m_Cognome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Cognome = value;
                    DoChanged("Cognome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice fiscale del cliente registrato dal collaboratore
            /// </summary>
            /// <returns></returns>
            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    string oldValue = m_CodiceFiscale;
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di nascita del cliente registrata dal collaboratore
            /// </summary>
            /// <returns></returns>
            public DateTime? DataNascita
            {
                get
                {
                    return m_DataNascita;
                }

                set
                {
                    var oldValue = m_DataNascita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataNascita = value;
                    DoChanged("DataNascita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'indirizzo del cliente registrato dal collaboratore
            /// </summary>
            /// <returns></returns>
            public CIndirizzo Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del collaboratore che ha in gestione il cliente
            /// </summary>
            /// <returns></returns>
            public int IDCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_Collaboratore, m_IDCollaboratore);
                }

                set
                {
                    int oldValue = IDCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDCollaboratore = value;
                    m_Collaboratore = null;
                    DoChanged("IDCollaboratore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il collaboratore che ha in gestione il cliente
            /// </summary>
            /// <returns></returns>
            public CCollaboratore Collaboratore
            {
                get
                {
                    if (m_Collaboratore is null)
                        m_Collaboratore = Collaboratori.GetItemById(m_IDCollaboratore);
                    return m_Collaboratore;
                }

                set
                {
                    var oldValue = Collaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Collaboratore = value;
                    m_IDCollaboratore = DBUtils.GetID(value);
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            /// <summary>
            /// Inizializza i valori di questo oggetto con i valori dei campi corrispondenti dell'oggetto Persona associato
            /// </summary>
            public void FromPersona()
            {
                var p = Persona;
                if (p is null)
                    throw new ArgumentNullException("persona");
                m_Nome = p.Nome;
                m_Cognome = p.Cognome;
                m_CodiceFiscale = p.CodiceFiscale;
                m_DataNascita = p.DataNascita;
                m_Indirizzo.CopyFrom(p.ResidenteA);
                m_DettaglioStatoLavorazione = "";
                m_NomeAmministrazione = p.ImpiegoPrincipale.NomeAzienda;
                m_TelefonoCasa = p.Telefono;
                m_TelefonoCellulare = p.Cellulare;
                m_Fax = p.Fax;
                m_eMailPersonale = p.eMail;
            }

            /// <summary>
            /// Assegna il cliente al collaboratore
            /// </summary>
            public void Assegna(string motivo)
            {
                FromPersona();
                // If (Me.StatoLavorazione >= StatoClienteCollaboratore.CONTATTO) Then Throw New InvalidOperationException("Stato non valido")
                // Me.StatoLavorazione = StatoClienteCollaboratore.RICHIESTACARICAMENTO
                MotivoAssegnazione = motivo;
                DataAssegnazione = DMD.DateUtils.Now();
                if (DataRinnovoCQS.HasValue == false)
                {
                    DataRinnovoCQS = DataAssegnazione;
                    MotivoRicontatto = MotivoAssegnazione;
                }

                AssegnatoDa = Sistema.Users.CurrentUser;
                DataRimozione = default;
                this.Flags = DMD.RunTime.SetFlag(this.Flags, ClienteCollaboratoreFlags.AssegnatoDaAgenzia, true);
                this.Save(true);
                var e = new Sistema.EventDescription("assegnazione", "Assegnazione cliente", this);
                GetModule().DispatchEvent(e);
            }

            /// <summary>
            /// Assegna il cliente al collaboratore
            /// </summary>
            public void Rimuovi(string motivo)
            {
                MotivoRimozione = motivo;
                DataRimozione = DMD.DateUtils.Now();
                RimossoDa = Sistema.Users.CurrentUser;
                Save(true);
                var e = new Sistema.EventDescription("assegnazione", "Assegnazione cliente", this);
                GetModule().DispatchEvent(e);
            }

            /// <summary>
            /// Il collaboratore accetta il cliente
            /// </summary>
            public void Accetta()
            {
                if (StatoLavorazione < StatoClienteCollaboratore.CONTATTO)
                {
                    StatoLavorazione = StatoClienteCollaboratore.CONTATTO;
                    DataAcquisizione = DMD.DateUtils.Now();
                    Save();
                }

                Save(true);
                var e = new Sistema.EventDescription("accettato", "Accettazione cliente", this);
                GetModule().DispatchEvent(e);
            }

            /// <summary>
            /// Richiede il caricamento dell'anagrafica nel database principale.
            /// </summary>
            public void RichiediCaricamento()
            {
                if (StatoLavorazione >= StatoClienteCollaboratore.RICHIESTACARICAMENTO)
                    throw new InvalidOperationException("Stato non valido");
                StatoLavorazione = StatoClienteCollaboratore.RICHIESTACARICAMENTO;
                Save();
                var e = new Sistema.EventDescription("richiedicaricamento", "Richiesta di caricamento", this);
                GetModule().DispatchEvent(e);
            }

            /// <summary>
            /// Richiede il caricamento dell'anagrafica nel database principale.
            /// </summary>
            public void SollecitaCaricamento()
            {
                if (StatoLavorazione != StatoClienteCollaboratore.RICHIESTACARICAMENTO)
                    throw new InvalidOperationException("Stato non valido");
                Save();
                var e = new Sistema.EventDescription("richiedicaricamento", "Richiesta di caricamento", this);
                this.GetModule().DispatchEvent(e);
            }

            /// <summary>
            /// Prende in carico il caricamento del cliente nel database principale
            /// </summary>
            public void PrendiInCarico()
            {
                var c = Consulenti.GetItemByUser(Sistema.Users.CurrentUser);
                if (c is null)
                    throw new InvalidOperationException("L'utente corrente non è un consulente");
                if (Persona is null)
                    throw new ArgumentNullException("Anagrafica non associata");
                if (StatoLavorazione != StatoClienteCollaboratore.RICHIESTACARICAMENTO)
                    throw new InvalidOperationException("Stato non valido");
                Consulente = c;
                DataAcquisizione = DMD.DateUtils.Now();
                StatoLavorazione = StatoClienteCollaboratore.CARICATO;
                Save();
                var e = new Sistema.EventDescription("presaincarico", "Presa in carico", this);
                GetModule().DispatchEvent(e);
            }

            /// <summary>
            /// Restituisce o imposta la data di presa in carico del cliente
            /// </summary>
            /// <returns></returns>
            public DateTime? DataAcquisizione
            {
                get
                {
                    return m_DataAcquisizione;
                }

                set
                {
                    var oldValue = m_DataAcquisizione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAcquisizione = value;
                    DoChanged("DataAcquisizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della fonte registrata dal collaboratore per il cliente
            /// </summary>
            /// <returns></returns>
            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    string oldValue = m_TipoFonte;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    m_Fonte = null;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della fonte registrata dal collaboratore per il cliente
            /// </summary>
            /// <returns></returns>
            public int IDFonte
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Fonte, m_IDFonte);
                }

                set
                {
                    int oldValue = IDFonte;
                    if (oldValue == value)
                        return;
                    m_Fonte = null;
                    m_IDFonte = value;
                    DoChanged("IDFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la fonte registrata dal collaboratore per il cliente
            /// </summary>
            /// <returns></returns>
            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null)
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_TipoFonte, m_TipoFonte, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    var oldValue = Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID(value, 0);
                    m_NomeFonte = "";
                    if (value is object)
                        m_NomeFonte = value.Nome;
                    DoChanged("Fonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della fonte registrata dal collaboratore per il cliente
            /// </summary>
            /// <returns></returns>
            public string NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }

                set
                {
                    string oldValue = m_NomeFonte;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFonte = value;
                    DoChanged("NomeFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags aggiuntivi
            /// </summary>
            /// <returns></returns>
            public new ClienteCollaboratoreFlags Flags
            {
                get
                {
                    return (ClienteCollaboratoreFlags)base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

         

            /// <summary>
            /// Restituisce o imposta lo stato di lavorazione del cliente
            /// </summary>
            /// <returns></returns>
            public StatoClienteCollaboratore StatoLavorazione
            {
                get
                {
                    return m_StatoLavorazione;
                }

                set
                {
                    var oldValue = m_StatoLavorazione;
                    if (oldValue == value)
                        return;
                    m_StatoLavorazione = value;
                    DoChanged("StatoLavorazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il dettaglio dello stato di lavorazione del cliente
            /// </summary>
            /// <returns></returns>
            public string DettaglioStatoLavorazione
            {
                get
                {
                    return m_DettaglioStatoLavorazione;
                }

                set
                {
                    string oldValue = m_DettaglioStatoLavorazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStatoLavorazione = value;
                    DoChanged("DettaglioStatoLavorazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'amministrazione presso cui lavora il cliente
            /// </summary>
            /// <returns></returns>
            public string NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }

                set
                {
                    string oldValue = m_NomeAmministrazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAmministrazione = value;
                    DoChanged("NomeAmministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Telefono principale
            /// </summary>
            public string TelefonoCasa
            {
                get
                {
                    return m_TelefonoCasa;
                }

                set
                {
                    string oldValue = m_TelefonoCasa;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TelefonoCasa = value;
                    DoChanged("TelefonoCasa", value, oldValue);
                }
            }

            /// <summary>
            /// Telefono ufficio
            /// </summary>
            public string TelefonoUfficio
            {
                get
                {
                    return m_TelefonoUfficio;
                }

                set
                {
                    string oldValue = m_TelefonoUfficio;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TelefonoUfficio = value;
                    DoChanged("TelefonoUfficio", value, oldValue);
                }
            }

            /// <summary>
            /// Cellulare
            /// </summary>
            public string TelefonoCellulare
            {
                get
                {
                    return m_TelefonoCellulare;
                }

                set
                {
                    string oldValue = m_TelefonoCellulare;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TelefonoCellulare = value;
                    DoChanged("TelefonoCellulare", value, oldValue);
                }
            }

            /// <summary>
            /// Fax
            /// </summary>
            public string Fax
            {
                get
                {
                    return m_Fax;
                }

                set
                {
                    string oldValue = m_Fax;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Fax = value;
                    DoChanged("Fax", value, oldValue);
                }
            }

            /// <summary>
            /// Altro telefono
            /// </summary>
            public string AltroTelefono
            {
                get
                {
                    return m_AltroTelefono;
                }

                set
                {
                    string oldValue = m_AltroTelefono;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AltroTelefono = value;
                    DoChanged("AltroTelefono", value, oldValue);
                }
            }

            /// <summary>
            /// email personale
            /// </summary>
            public string eMailPersonale
            {
                get
                {
                    return m_eMailPersonale;
                }

                set
                {
                    string oldValue = m_eMailPersonale;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_eMailPersonale = value;
                    DoChanged("eMailPersonale", value, oldValue);
                }
            }

            /// <summary>
            /// email ufficio
            /// </summary>
            public string eMailUfficio
            {
                get
                {
                    return m_eMailUfficio;
                }

                set
                {
                    string oldValue = m_eMailUfficio;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_eMailUfficio = value;
                    DoChanged("eMailUfficio", value, oldValue);
                }
            }

            /// <summary>
            /// PEC
            /// </summary>
            public string PEC
            {
                get
                {
                    return m_PEC;
                }

                set
                {
                    string oldValue = m_PEC;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PEC = value;
                    DoChanged("PEC", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di assegnazione (per i clienti assegnati direttamente dall'agenzia)
            /// </summary>
            /// <returns></returns>
            public DateTime? DataAssegnazione
            {
                get
                {
                    return m_DataAssegnazione;
                }

                set
                {
                    var oldValue = m_DataAssegnazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAssegnazione = value;
                    DoChanged("DataAssegnazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo dell'assegnazione
            /// </summary>
            /// <returns></returns>
            public string MotivoAssegnazione
            {
                get
                {
                    return m_MotivoAssegnazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MotivoAssegnazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoAssegnazione = value;
                    DoChanged("MotivoAssegnazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha assegnato il cliente al collaboratore
            /// </summary>
            /// <returns></returns>
            public int IDAssegnatoDa
            {
                get
                {
                    return DBUtils.GetID(m_AssegnatoDa, m_IDAssegnatoDa);
                }

                set
                {
                    int oldValue = IDAssegnatoDa;
                    if (oldValue == value)
                        return;
                    m_IDAssegnatoDa = value;
                    m_AssegnatoDa = null;
                    DoChanged("IDAssegnatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha assegnato il cliente al collaboratore
            /// </summary>
            /// <returns></returns>
            public Sistema.CUser AssegnatoDa
            {
                get
                {
                    if (m_AssegnatoDa is null)
                        m_AssegnatoDa = Sistema.Users.GetItemById(m_IDAssegnatoDa);
                    return m_AssegnatoDa;
                }

                set
                {
                    var oldValue = AssegnatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AssegnatoDa = value;
                    m_IDAssegnatoDa = DBUtils.GetID(value, 0);
                    DoChanged("AssegnatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome ed il cognome del cliente
            /// </summary>
            /// <returns></returns>
            public string Nominativo
            {
                get
                {
                    return DMD.Strings.Combine(m_Nome, m_Cognome, " ");
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.Nominativo;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDPersona, this.m_IDCollaboratore, this.m_DataAcquisizione);
            }

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || m_Indirizzo.IsChanged();
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Collaboratori.ClientiXCollaboratori;
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_Indirizzo.SetChanged(false);
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CQSPDCliXCollab";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_Cognome = reader.Read("Cognome", this.m_Cognome);
                m_CodiceFiscale = reader.Read("CodiceFiscale", this.m_CodiceFiscale);
                m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                m_DataNascita = reader.Read("DataNascita", this.m_DataNascita);
                m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", m_Indirizzo.Provincia);
                m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", m_Indirizzo.Citta);
                m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", m_Indirizzo.CAP);
                m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                m_Indirizzo.SetChanged(false);
                m_DataAcquisizione = reader.Read("DataAcquisizione", this.m_DataAcquisizione);
                m_TipoFonte = reader.Read("TipoFonte", this.m_TipoFonte);
                m_IDFonte = reader.Read("IDFonte", this.m_IDFonte);
                m_NomeFonte = reader.Read("NomeFonte", this.m_NomeFonte);
                m_StatoLavorazione = reader.Read("StatoLavorazione", this.m_StatoLavorazione);
                m_DettaglioStatoLavorazione = reader.Read("DettaglioStatoLavorazione", this.m_DettaglioStatoLavorazione);
                m_NomeAmministrazione = reader.Read("NomeAmministrazione", this.m_NomeAmministrazione);
                m_TelefonoCasa = reader.Read("TelefonoCasa", this.m_TelefonoCasa);
                m_TelefonoUfficio = reader.Read("TelefonoUfficio", this.m_TelefonoUfficio);
                m_TelefonoCellulare = reader.Read("TelefonoCellulare", this.m_TelefonoCellulare);
                m_Fax = reader.Read("Fax", this.m_Fax);
                m_AltroTelefono = reader.Read("AltroTelefono", this.m_AltroTelefono);
                m_eMailPersonale = reader.Read("eMailPersonale", this.m_eMailPersonale);
                m_eMailUfficio = reader.Read("eMailUfficio", this.m_eMailUfficio);
                m_PEC = reader.Read("PEC", this.m_PEC);
                m_DataRinnovoCQS = reader.Read("DataRinnovoCQS", this.m_DataRinnovoCQS);
                m_DataRinnovoPD = reader.Read("DataRinnovoPD", this.m_DataRinnovoPD);
                m_ImportoRichiesto = reader.Read("ImportoRichiesto", this.m_ImportoRichiesto);
                m_MotivoRichiesta = reader.Read("MotivoRichiesta", this.m_MotivoRichiesta);
                m_DataRichiesta = reader.Read("DataRichiesta", this.m_DataRichiesta);
                m_MotivoRicontatto = reader.Read("MotivoRicontatto", this.m_MotivoRicontatto);
                m_IDConsulente = reader.Read("IDConsulente", this.m_IDConsulente);
                m_DataAssegnazione = reader.Read("DataAssegnazione", this.m_DataAssegnazione);
                m_MotivoAssegnazione = reader.Read("MotivoAssegnazione", this.m_MotivoAssegnazione);
                m_IDAssegnatoDa = reader.Read("IDAssegnatoDa", this.m_IDAssegnatoDa);
                m_DataRimozione = reader.Read("DataRimozione", this.m_DataRimozione);
                m_MotivoRimozione = reader.Read("MotivoRimozione", this.m_MotivoRimozione);
                m_IDRimossoDa = reader.Read("IDRimossoDa", this.m_IDRimossoDa);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPersona", IDPersona);
                writer.Write("Nome", m_Nome);
                writer.Write("Cognome", m_Cognome);
                writer.Write("CodiceFiscale", m_CodiceFiscale);
                writer.Write("DataNascita", m_DataNascita);
                writer.Write("IDCollaboratore", IDCollaboratore);
                writer.Write("Indirizzo_Provincia", m_Indirizzo.Provincia);
                writer.Write("Indirizzo_Citta", m_Indirizzo.Citta);
                writer.Write("Indirizzo_CAP", m_Indirizzo.CAP);
                writer.Write("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                writer.Write("DataAcquisizione", m_DataAcquisizione);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("NomeFonte", m_NomeFonte);
                writer.Write("StatoLavorazione", m_StatoLavorazione);
                writer.Write("DettaglioStatoLavorazione", m_DettaglioStatoLavorazione);
                writer.Write("NomeAmministrazione", m_NomeAmministrazione);
                writer.Write("TelefonoCasa", m_TelefonoCasa);
                writer.Write("TelefonoUfficio", m_TelefonoUfficio);
                writer.Write("TelefonoCellulare", m_TelefonoCellulare);
                writer.Write("Fax", m_Fax);
                writer.Write("AltroTelefono", m_AltroTelefono);
                writer.Write("eMailPersonale", m_eMailPersonale);
                writer.Write("eMailUfficio", m_eMailUfficio);
                writer.Write("PEC", m_PEC);
                writer.Write("DataRinnovoCQS", m_DataRinnovoCQS);
                writer.Write("DataRinnovoPD", m_DataRinnovoPD);
                writer.Write("ImportoRichiesto", m_ImportoRichiesto);
                writer.Write("MotivoRichiesta", m_MotivoRichiesta);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("MotivoRicontatto", m_MotivoRicontatto);
                writer.Write("IDConsulente", IDConsulente);
                writer.Write("DataAssegnazione", m_DataAssegnazione);
                writer.Write("MotivoAssegnazione", m_MotivoAssegnazione);
                writer.Write("IDAssegnatoDa", IDAssegnatoDa);
                writer.Write("DataRimozione", m_DataRimozione);
                writer.Write("MotivoRimozione", m_MotivoRimozione);
                writer.Write("IDRimossoDa", IDRimossoDa);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Cognome", typeof(string), 255);
                c = table.Fields.Ensure("CodiceFiscale", typeof(string), 255);
                c = table.Fields.Ensure("DataNascita", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDCollaboratore", typeof(int), 1);
                c = table.Fields.Ensure("Indirizzo_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Citta", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_CAP", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Via", typeof(string), 255);
                c = table.Fields.Ensure("DataAcquisizione", typeof(DateTime), 1);
                c = table.Fields.Ensure("TipoFonte", typeof(string), 255);
                c = table.Fields.Ensure("IDFonte", typeof(int), 1);
                c = table.Fields.Ensure("NomeFonte", typeof(string), 255);
                c = table.Fields.Ensure("StatoLavorazione", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioStatoLavorazione", typeof(string), 255);
                c = table.Fields.Ensure("NomeAmministrazione", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoCasa", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoUfficio", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoCellulare", typeof(string), 255);
                c = table.Fields.Ensure("Fax", typeof(string), 255);
                c = table.Fields.Ensure("AltroTelefono", typeof(string), 255);
                c = table.Fields.Ensure("eMailPersonale", typeof(string), 255);
                c = table.Fields.Ensure("eMailUfficio", typeof(string), 255);
                c = table.Fields.Ensure("PEC", typeof(string), 255);
                c = table.Fields.Ensure("DataRinnovoCQS", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataRinnovoPD", typeof(DateTime), 1);
                c = table.Fields.Ensure("ImportoRichiesto", typeof(string), 255);
                c = table.Fields.Ensure("MotivoRichiesta", typeof(string), 255);
                c = table.Fields.Ensure("DataRichiesta", typeof(DateTime), 1);
                c = table.Fields.Ensure("MotivoRicontatto", typeof(string), 255);
                c = table.Fields.Ensure("IDConsulente", typeof(int), 1);
                c = table.Fields.Ensure("DataAssegnazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("MotivoAssegnazione", typeof(string), 255);
                c = table.Fields.Ensure("IDAssegnatoDa", typeof(int), 1);
                c = table.Fields.Ensure("DataRimozione", typeof(DateTime), 1);
                c = table.Fields.Ensure("MotivoRimozione", typeof(string), 255);
                c = table.Fields.Ensure("IDRimossoDa", typeof(int), 1);                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxAssociazione", new string[] { "IDPersona", "IDCollaboratore", "DataAcquisizione" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxAnagrafica", new string[] { "CodiceFiscale", "Nome", "Cognome", "DataNascita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo", new string[] { "Indirizzo_Provincia", "Indirizzo_CAP", "Indirizzo_Citta", "Indirizzo_Via" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFonte", new string[] { "TipoFonte", "IDFonte", "NomeFonte" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxLavorazione", new string[] { "StatoLavorazione", "DettaglioStatoLavorazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxImpiego", new string[] { "NomeAmministrazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContatti", new string[] { "TelefonoCasa", "TelefonoUfficio", "TelefonoCellulare", "Fax", }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEMail", new string[] { "eMailPersonale", "eMailUfficio", "PEC", "AltroTelefono" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRinnovi", new string[] { "DataRinnovoCQS", "DataRinnovoPD", "PEC", "AltroTelefono" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRichiesta", new string[] { "DataRichiesta", "ImportoRichiesto", "MotivoRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxConsulente", new string[] { "IDConsulente", "MotivoRicontatto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAssegnatoDA", new string[] { "IDAssegnatoDa", "DataAssegnazione", "MotivoAssegnazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRimozione", new string[] { "IDRimossoDa", "DataRimozione", "MotivoRimozione" }, DBFieldConstraintFlags.None);
                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Cognome", m_Cognome);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                writer.WriteAttribute("DataNascita", m_DataNascita);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteAttribute("DataAcquisizione", m_DataAcquisizione);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("NomeFonte", m_NomeFonte);
                writer.WriteAttribute("StatoLavorazione", (int?)m_StatoLavorazione);
                writer.WriteAttribute("DettaglioStatoLavorazione", m_DettaglioStatoLavorazione);
                writer.WriteAttribute("NomeAmministrazione", m_NomeAmministrazione);
                writer.WriteAttribute("TelefonoCasa", m_TelefonoCasa);
                writer.WriteAttribute("TelefonoUfficio", m_TelefonoUfficio);
                writer.WriteAttribute("TelefonoCellulare", m_TelefonoCellulare);
                writer.WriteAttribute("Fax", m_Fax);
                writer.WriteAttribute("AltroTelefono", m_AltroTelefono);
                writer.WriteAttribute("eMailPersonale", m_eMailPersonale);
                writer.WriteAttribute("eMailUfficio", m_eMailUfficio);
                writer.WriteAttribute("PEC", m_PEC);
                writer.WriteAttribute("DataRinnovoCQS", m_DataRinnovoCQS);
                writer.WriteAttribute("DataRinnovoPD", m_DataRinnovoPD);
                writer.WriteAttribute("ImportoRichiesto", m_ImportoRichiesto);
                writer.WriteAttribute("MotivoRichiesta", m_MotivoRichiesta);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("MotivoRicontatto", m_MotivoRicontatto);
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("DataAssegnazione", m_DataAssegnazione);
                writer.WriteAttribute("MotivoAssegnazione", m_MotivoAssegnazione);
                writer.WriteAttribute("IDAssegnatoDa", IDAssegnatoDa);
                writer.WriteAttribute("DataRimozione", m_DataRimozione);
                writer.WriteAttribute("MotivoRimozione", m_MotivoRimozione);
                writer.WriteAttribute("IDRimossoDa", IDRimossoDa);
                base.XMLSerialize(writer);
                writer.WriteTag("Indirizzo", m_Indirizzo);
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
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Cognome":
                        {
                            m_Cognome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceFiscale":
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataNascita":
                        {
                            m_DataNascita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataAcquisizione":
                        {
                            m_DataAcquisizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            m_TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            m_IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeFonte":
                        {
                            m_NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoLavorazione":
                        {
                            m_StatoLavorazione = (StatoClienteCollaboratore)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioStatoLavorazione":
                        {
                            m_DettaglioStatoLavorazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = (Anagrafica.CIndirizzo)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    
                    case "NomeAmministrazione":
                        {
                            m_NomeAmministrazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TelefonoCasa":
                        {
                            m_TelefonoCasa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TelefonoUfficio":
                        {
                            m_TelefonoUfficio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TelefonoCellulare":
                        {
                            m_TelefonoCellulare = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Fax":
                        {
                            m_Fax = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AltroTelefono":
                        {
                            m_AltroTelefono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "eMailPersonale":
                        {
                            m_eMailPersonale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "eMailUfficio":
                        {
                            m_eMailUfficio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PEC":
                        {
                            m_PEC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRinnovoCQS":
                        {
                            m_DataRinnovoCQS = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataRinnovoPD":
                        {
                            m_DataRinnovoPD = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MotivoRicontatto":
                        {
                            m_MotivoRicontatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ImportoRichiesto":
                        {
                            m_ImportoRichiesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoRichiesta":
                        {
                            m_MotivoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDConsulente":
                        {
                            m_IDConsulente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataAssegnazione":
                        {
                            m_DataAssegnazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDAssegnatoDa":
                        {
                            m_IDAssegnatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MotivoAssegnazione":
                        {
                            m_MotivoAssegnazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRimozione":
                        {
                            m_DataRimozione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MotivoRimozione":
                        {
                            m_MotivoRimozione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRimossoDa":
                        {
                            m_IDRimossoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(ClienteXCollaboratore obj)
            {
                return DMD.Strings.Compare(Nominativo, obj.Nominativo, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((ClienteXCollaboratore)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is ClienteXCollaboratore) && this.Equals((ClienteXCollaboratore)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ClienteXCollaboratore obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome, true)
                    && DMD.Strings.EQ(this.m_Cognome, obj.m_Cognome, true)
                    && DMD.Strings.EQ(this.m_CodiceFiscale, obj.m_CodiceFiscale, true)
                    && DMD.DateUtils.EQ(this.m_DataNascita, obj.m_DataNascita)
                    && this.m_Indirizzo.Equals(obj.m_Indirizzo)
                    && DMD.Integers.EQ(this.m_IDCollaboratore, obj.m_IDCollaboratore)
                    && DMD.DateUtils.EQ(this.m_DataAcquisizione, obj.m_DataAcquisizione)
                    && DMD.Strings.EQ(this.m_TipoFonte, obj.m_TipoFonte, true)
                    && DMD.Integers.EQ(this.m_IDFonte, obj.m_IDFonte)
                    && DMD.Strings.EQ(this.m_NomeFonte, obj.m_NomeFonte, true)
                    && DMD.Integers.EQ((int)this.m_StatoLavorazione, (int)obj.m_StatoLavorazione)
                    && DMD.Strings.EQ(this.m_DettaglioStatoLavorazione, obj.m_DettaglioStatoLavorazione, true)
                    && DMD.Strings.EQ(this.m_NomeAmministrazione, obj.m_NomeAmministrazione, true)
                    && DMD.Strings.EQ(this.m_TelefonoCasa, obj.m_TelefonoCasa, true)
                    && DMD.Strings.EQ(this.m_TelefonoUfficio, obj.m_TelefonoUfficio, true)
                    && DMD.Strings.EQ(this.m_TelefonoCellulare, obj.m_TelefonoCellulare, true)
                    && DMD.Strings.EQ(this.m_Fax, obj.m_Fax, true)
                    && DMD.Strings.EQ(this.m_AltroTelefono, obj.m_AltroTelefono, true)
                    && DMD.Strings.EQ(this.m_eMailPersonale, obj.m_eMailPersonale, true)
                    && DMD.Strings.EQ(this.m_eMailUfficio, obj.m_eMailUfficio, true)
                    && DMD.Strings.EQ(this.m_PEC, obj.m_PEC, true)
                    && DMD.DateUtils.EQ(this.m_DataRinnovoCQS, obj.m_DataRinnovoCQS)
                    && DMD.Strings.EQ(this.m_MotivoRicontatto, obj.m_MotivoRicontatto, true)
                    && DMD.DateUtils.EQ(this.m_DataRinnovoPD, obj.m_DataRinnovoPD)
                    && DMD.Strings.EQ(this.m_ImportoRichiesto, obj.m_ImportoRichiesto, true)
                    && DMD.Strings.EQ(this.m_MotivoRichiesta, obj.m_MotivoRichiesta, true)
                    && DMD.DateUtils.EQ(this.m_DataRichiesta, obj.m_DataRichiesta)
                    && DMD.Integers.EQ(this.m_IDConsulente, obj.m_IDConsulente)
                    && DMD.DateUtils.EQ(this.m_DataAssegnazione, obj.m_DataAssegnazione)
                    && DMD.Strings.EQ(this.m_MotivoAssegnazione, obj.m_MotivoAssegnazione, true)
                    && DMD.Integers.EQ(this.m_IDAssegnatoDa, obj.m_IDAssegnatoDa)
                    && DMD.DateUtils.EQ(this.m_DataRimozione, obj.m_DataRimozione)
                    && DMD.Strings.EQ(this.m_MotivoRimozione, obj.m_MotivoRimozione, true)
                    && DMD.Integers.EQ(this.m_IDRimossoDa, obj.m_IDRimossoDa)
                    ;
             
            }
        }
    }
}