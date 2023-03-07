using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        [Flags]
        public enum ConsulenzeFlags : int
        {
            NOTSET = 0,
            HIDDEN = 1
        }

        public enum StatiConsulenza : int
        {
            /// <summary>
        /// Si tratta di una simulazione non ancora proposta al cliente (es. prove)
        /// </summary>
        /// <remarks></remarks>
            INSERITA = 0,

            /// <summary>
        /// La simulazione è stata proposta al cliente
        /// </summary>
        /// <remarks></remarks>
            PROPOSTA = 1,

            /// <summary>
        /// Il cliente ha accettato la proposta
        /// </summary>
        /// <remarks></remarks>
            ACCETTATA = 2,

            /// <summary>
        /// La proposta fatta al cliente è stata da egli rifiutata
        /// </summary>
        /// <remarks></remarks>
            RIFIUTATA = 3,

            /// <summary>
        /// La proposta è stata bocciata dall'agenzia (non fattibile)
        /// </summary>
        /// <remarks></remarks>
            BOCCIATA = 4,

            /// <summary>
        /// La proposta non è fattibile
        /// </summary>
        /// <remarks></remarks>
            NONFATTIBILE = 5
        }

        [Serializable]
        public class CQSPDConsulenza 
            : Databases.DBObjectPO, IEstintore, IOggettoApprovabile, IComparable, IOggettoVerificabile, ICloneable
        {

            /// <summary>
        /// Evento generato quando la consulenza viene inserita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event InseritaEventHandler Inserita;

            public delegate void InseritaEventHandler(object sender, ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene proposta al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PropostaEventHandler Proposta;

            public delegate void PropostaEventHandler(object sender, ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene accettata dal cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event AccettataEventHandler Accettata;

            public delegate void AccettataEventHandler(object sender, ItemEventArgs e);

            /// <summary>
            /// Evento generato quando la consulenza viene bocciata dall'operatore
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event BocciataEventHandler Bocciata;

            public delegate void BocciataEventHandler(object sender, ItemEventArgs e);

            /// <summary>
            /// Evento generato quando la consulenza viene rifiutata dal cliente
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event IOggettoApprovabile.RifiutataEventHandler Rifiutata;
             


            /// <summary>
            /// Evento generato quando l'offerta richiede l'approvazione di un supervisore
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event IOggettoApprovabile.RequireApprovationEventHandler RequireApprovation;
             

            /// <summary>
            /// Evento generato quando la consulenza viene presa in carico da un supervisore
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event IOggettoApprovabile.PresaInCaricoEventHandler PresaInCarico;
             

            /// <summary>
            /// Evento generato quando la consulenza viene approvata dal supervisore
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event IOggettoApprovabile.ApprovataEventHandler Approvata;

            
            /// <summary>
            /// Evento generato quando la consulenza viene bocciata dal supervisore o dall'operatore
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event NegataEventHandler Negata;

            public delegate void NegataEventHandler(object sender, ItemEventArgs e);


            private int m_IDCliente;                  // ID del cliente
            private Anagrafica.CPersonaFisica m_Cliente;             // Cliente
            private string m_NomeCliente;           // Nome del cliente
            private int m_IDRichiesta;                // ID della richiesta da cui è partita la consulenza
            private CRichiestaFinanziamento m_Richiesta;  // Richiesta da cui è partita la consulenza
            private int m_IDConsulente;               // ID dell'operatore di consulenza
            private CConsulentePratica m_Consulente;      // Consulente
            private string m_NomeConsulente;              // Nominativo del consulente
            private DateTime? m_DataConsulenza;   // Data in cui si è fatta la consulenza
            private DateTime? m_DataProposta;     // Data della proposta al cliente
            private DateTime? m_DataConferma;     // Data in cui il cliente ha accettato o ha rifiutato l'offerta
            private string m_Descrizione;
            private ConsulenzeFlags m_Flags;
            private StatiConsulenza m_StatoConsulenza;
            private COffertaCQS m_OffertaCQS;
            private int m_IDOffertaCQS;
            // Private m_IDProdottoCQS As Integer
            // Private m_NomeProdottoCQS As String

            private COffertaCQS m_OffertaPD;
            private int m_IDOffertaPD;
            // Private m_IDProdottoPD As Integer
            // Private m_NomeProdottoPD As String

            private decimal m_MontanteLordo;
            private decimal m_NettoRicavo;
            private decimal m_SommaEstinzioni;
            private decimal m_SommaTrattenuteVolontarie;
            private decimal m_SommaPignoramenti;
            private decimal m_StipendioNetto;
            private decimal m_TFR;
            private int m_ValutazioneGARF;
            private string m_TipoImpiego;
            private float m_Eta;
            private float m_Anzianita;
            private int m_IDAzienda;
            private Anagrafica.CAzienda m_Azienda;
            private string m_NomeAzienda;
            private DateTime? m_DataAssunzione;
            private Sistema.CUser m_PropostaDa;
            private int m_IDPropostaDa;
            private Sistema.CUser m_ConfermataDa;
            private int m_IDConfermataDa;
            private int m_IDContesto;
            private string m_TipoContesto;
            private double m_Durata;
            private int m_IDStudioDiFattibilita;
            private CQSPDStudioDiFattibilita m_StudioDiFattibilita;
            private int m_IDInseritoDa;
            private Sistema.CUser m_InseritoDa;
            private int m_IDRichiestaApprovazione;
            private CRichiestaApprovazione m_RichiestaApprovazione;
            private string m_MotivoAnnullamento;
            private string m_DettaglioAnnullamento;
            private CEstinzioniXEstintoreCollection m_Estinzioni;
            private int m_IDAnnullataDa;
            private Sistema.CUser m_AnnullataDa;
            private string m_NomeAnnullataDa;
            private DateTime? m_DataAnnullamento;
            private int m_IDFinestraLavorazione;
            private FinestraLavorazione m_FinestraLavorazione;
            private int m_IDUltimaVerifica;
            private VerificaAmministrativa m_UltimaVerifica;
            private int m_IDCollaboratore;
            private CCollaboratore m_Collaboratore;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CQSPDConsulenza()
            {
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_IDRichiesta = 0;
                m_Richiesta = null;
                m_IDConsulente = 0;
                m_Consulente = null;
                m_NomeConsulente = "";
                m_DataConsulenza = default;
                m_DataProposta = default;
                m_DataConferma = default;
                m_Descrizione = "";
                m_Flags = ConsulenzeFlags.NOTSET;
                m_StatoConsulenza = StatiConsulenza.INSERITA;
                m_OffertaCQS = null;
                m_IDOffertaCQS = 0;
                // Me.m_IDProdottoCQS = 0
                // Me.m_NomeProdottoCQS = ""
                m_OffertaPD = null;
                m_IDOffertaPD = 0;
                // Me.m_IDProdottoPD = 0
                // Me.m_NomeProdottoPD = ""
                m_MontanteLordo = 0m;
                m_NettoRicavo = 0m;
                m_SommaEstinzioni = 0m;
                m_SommaTrattenuteVolontarie = 0m;
                m_SommaPignoramenti = 0m;
                m_StipendioNetto = 0m;
                m_TFR = 0m;
                m_ValutazioneGARF = 1;
                m_TipoImpiego = "";
                m_Eta = 0f;
                m_Anzianita = 0f;
                m_IDAzienda = 0;
                m_Azienda = null;
                m_NomeAzienda = "";
                m_DataAssunzione = default;
                m_PropostaDa = null;
                m_IDPropostaDa = 0;
                m_ConfermataDa = null;
                m_IDConfermataDa = 0;
                m_IDContesto = 0;
                m_TipoContesto = DMD.Strings.vbNullString;
                m_Durata = 0d;
                m_InseritoDa = null;
                m_IDInseritoDa = 0;
                m_IDRichiestaApprovazione = 0;
                m_RichiestaApprovazione = null;
                m_MotivoAnnullamento = "";
                m_DettaglioAnnullamento = "";
                m_Estinzioni = null;
                m_IDAnnullataDa = 0;
                m_AnnullataDa = null;
                m_NomeAnnullataDa = "";
                m_DataAnnullamento = default;
                m_IDFinestraLavorazione = 0;
                m_FinestraLavorazione = null;
                m_IDUltimaVerifica = 0;
                m_UltimaVerifica = null;
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
            }

            event IOggettoApprovabile.RequireApprovationEventHandler IOggettoApprovabile.RequireApprovation
            {
                add
                {
                    this.RequireApprovation += value;
                }

                remove
                {
                    this.RequireApprovation -= value;
                }
            }

            event IOggettoApprovabile.ApprovataEventHandler IOggettoApprovabile.Approvata
            {
                add
                {
                    this.Approvata += value;
                }

                remove
                {
                    this.Approvata -= value;
                }
            }

            event IOggettoApprovabile.RifiutataEventHandler IOggettoApprovabile.Rifiutata
            {
                add
                {
                    this.Rifiutata += value;
                }

                remove
                {
                    this.Rifiutata -= value;
                }
            }

            event IOggettoApprovabile.PresaInCaricoEventHandler IOggettoApprovabile.PresaInCarico
            {
                add
                {
                    this.PresaInCarico += value;
                }

                remove
                {
                    this.PresaInCarico -= value;
                }
            }

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
                    m_IDCollaboratore = DBUtils.GetID(value);
                    m_Collaboratore = value;
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'ultima verifica amministrativa
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDUltimaVerifica
            {
                get
                {
                    return DBUtils.GetID(m_UltimaVerifica, m_IDUltimaVerifica);
                }

                set
                {
                    int oldValue = IDUltimaVerifica;
                    if (oldValue == value)
                        return;
                    m_IDUltimaVerifica = value;
                    m_UltimaVerifica = null;
                    DoChanged("IDUltimaVerifica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ultima verifica amministrativa
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public VerificaAmministrativa UltimaVerifica
            {
                get
                {
                    if (m_UltimaVerifica is null)
                        m_UltimaVerifica = VerificheAmministrative.GetItemById(m_IDUltimaVerifica);
                    return m_UltimaVerifica;
                }

                set
                {
                    var oldValue = m_UltimaVerifica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_UltimaVerifica = value;
                    m_IDUltimaVerifica = DBUtils.GetID(value);
                    DoChanged("UltimaVerifica", value, oldValue);
                }
            }

            protected internal void SetUltimaVerifica(VerificaAmministrativa value)
            {
                m_UltimaVerifica = value;
                m_IDUltimaVerifica = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della finestra di lavorazioen a cui appartiene l'oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Restituisce o imposta l'ID dell'utente che ha annullato l'offerta (bocciata)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAnnullataDa
            {
                get
                {
                    return DBUtils.GetID(m_AnnullataDa, m_IDAnnullataDa);
                }

                set
                {
                    int oldValue = IDAnnullataDa;
                    if (oldValue == value)
                        return;
                    m_AnnullataDa = null;
                    m_IDAnnullataDa = value;
                    DoChanged("IDAnnullataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha annullato l'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser AnnullataDa
            {
                get
                {
                    if (m_AnnullataDa is null)
                        m_AnnullataDa = Sistema.Users.GetItemById(m_IDAnnullataDa);
                    return m_AnnullataDa;
                }

                set
                {
                    var oldValue = AnnullataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AnnullataDa = value;
                    m_IDAnnullataDa = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAnnullataDa = value.Nominativo;
                    DoChanged("AnnullataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha annullato lo studio di fattibilita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeAnnullataDa
            {
                get
                {
                    return m_NomeAnnullataDa;
                }

                set
                {
                    string oldValue = m_NomeAnnullataDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAnnullataDa = value;
                    DoChanged("NomeAnnullataDa", value, oldValue);
                }
            }

            public DateTime? DataAnnullamento
            {
                get
                {
                    return m_DataAnnullamento;
                }

                set
                {
                    var oldValue = m_DataAnnullamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAnnullamento = value;
                    DoChanged("DataAnnullamento", value, oldValue);
                }
            }

            public CEstinzioniXEstintoreCollection Estinzioni
            {
                get
                {
                    lock (this)
                    {
                        if (m_Estinzioni is null)
                            m_Estinzioni = new CEstinzioniXEstintoreCollection(this);
                        return m_Estinzioni;
                    }
                }
            }

            protected internal virtual void SetEstinzioni(CEstinzioniXEstintoreCollection value)
            {
                m_Estinzioni = value;
            }

            /// <summary>
        /// Nel caso di bocciatura o di rifiuto da parte del cliente indica il motivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MotivoAnnullamento
            {
                get
                {
                    return m_MotivoAnnullamento;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MotivoAnnullamento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoAnnullamento = value;
                    DoChanged("MotivoAnnullamento", value, oldValue);
                }
            }

            /// <summary>
        /// Nel caso di bocciatura o di rifiuto da parte del cliente descrive nel dettaglio il motivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DettaglioAnnullamento
            {
                get
                {
                    return m_DettaglioAnnullamento;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioAnnullamento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioAnnullamento = value;
                    DoChanged("DettaglioAnnullamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o i mposta l'ID dell'utente che ha registrato la consulenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDInseritoDa
            {
                get
                {
                    return DBUtils.GetID(m_InseritoDa, m_IDInseritoDa);
                }

                set
                {
                    int oldValue = IDInseritoDa;
                    if (oldValue == value)
                        return;
                    m_InseritoDa = null;
                    m_IDInseritoDa = value;
                    DoChanged("IDInseritoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha registrato la consulenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser InseritoDa
            {
                get
                {
                    if (m_InseritoDa is null)
                        m_InseritoDa = Sistema.Users.GetItemById(m_IDInseritoDa);
                    return m_InseritoDa;
                }

                set
                {
                    var oldValue = m_InseritoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InseritoDa = value;
                    m_IDInseritoDa = DBUtils.GetID(value);
                    DoChanged("InseritoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del gruppo di studi di fattibilità a cui appartiene l'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDStudioDiFattibilita
            {
                get
                {
                    return DBUtils.GetID(m_StudioDiFattibilita, m_IDStudioDiFattibilita);
                }

                set
                {
                    int oldValue = IDStudioDiFattibilita;
                    if (oldValue == value)
                        return;
                    m_IDStudioDiFattibilita = value;
                    m_StudioDiFattibilita = null;
                    DoChanged("IDStudioDiFattibilita", value, oldValue);
                }
            }

            public CQSPDStudioDiFattibilita StudioDiFattibilita
            {
                get
                {
                    if (m_StudioDiFattibilita is null)
                        m_StudioDiFattibilita = StudiDiFattibilita.GetItemById(m_IDStudioDiFattibilita);
                    return m_StudioDiFattibilita;
                }

                set
                {
                    var oldValue = m_StudioDiFattibilita;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StudioDiFattibilita = value;
                    m_IDStudioDiFattibilita = DBUtils.GetID(value);
                    DoChanged("StudioDiFattibilita", value, oldValue);
                }
            }

            internal void SetStudioDiFattibilita(CQSPDStudioDiFattibilita value)
            {
                m_StudioDiFattibilita = value;
                m_IDStudioDiFattibilita = DBUtils.GetID(value);
            }



            /// <summary>
        /// Restituisce o imposta la durata in secondi della consulenza
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
                    var oldValue = this.m_Durata;
                    if (DMD.Doubles.EQ(value, oldValue))
                        return;

                    this.m_Durata = value;
                    DoChanged("Durata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del contesto in cui è stata creata la consulenza
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
                    DoChanged("IDContesto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo del contesto in cui è stato creato l'oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContesto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContesto = value;
                    DoChanged("TipoContesto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di assunzione del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataAssunzione
            {
                get
                {
                    return m_DataAssunzione;
                }

                set
                {
                    var oldValue = m_DataAssunzione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataAssunzione = value;
                    DoChanged("DataAssunzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'azienda per cui lavora il cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID(m_Azienda, m_IDAzienda);
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
        /// Restituisce o imposta l'azienda per cui lavora il cliente
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
                    return m_Azienda;
                }

                set
                {
                    var oldValue = m_Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDAzienda = DBUtils.GetID(value);
                    m_Azienda = value;
                    if (value is object)
                        m_NomeAzienda = value.Nominativo;
                    DoChanged("Azienda", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'azienda per cui lavora il cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAzienda;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAzienda = value;
                    DoChanged("NomeAzienda", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il montante lordo proposto al cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal MontanteLordo
            {
                get
                {
                    return m_MontanteLordo;
                }

                set
                {
                    decimal oldValue = m_MontanteLordo;
                    if (oldValue == value)
                        return;
                    m_MontanteLordo = value;
                    DoChanged("MontanteLordo", value, oldValue);
                }
            }

            public DateTime DataCaricamento
            {
                get
                {
                    if (DataConsulenza.HasValue)
                    {
                        return DataConsulenza.Value;
                    }
                    else
                    {
                        return CreatoIl;
                    }
                }
            }

            public DateTime? DataDecorrenza
            {
                get
                {
                    DateTime? ret;
                    var d1 = default(DateTime?);
                    var d2 = default(DateTime?);
                    if (OffertaCQS is object && OffertaCQS.Stato == ObjectStatus.OBJECT_VALID)
                        d1 = OffertaCQS.DataDecorrenza;
                    if (OffertaPD is object && OffertaPD.Stato == ObjectStatus.OBJECT_VALID)
                        d2 = OffertaPD.DataDecorrenza;
                    ret = DMD.DateUtils.Min(d1, d2);
                    if (ret.HasValue == false)
                        ret = DataConsulenza;
                    if (ret.HasValue == false)
                        ret = CreatoIl;
                    return ret;
                }

                set
                {
                    throw new InvalidOperationException();
                }
            }

            /// <summary>
        /// Restituisce o imposta il netto ricavo proposto al cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal NettoRicavo
            {
                get
                {
                    return m_NettoRicavo;
                }

                set
                {
                    decimal oldValue = m_NettoRicavo;
                    if (oldValue == value)
                        return;
                    m_NettoRicavo = value;
                    DoChanged("NettoRicavo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la somma degli altri prestiti da estinguere
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SommaEstinzioni
            {
                get
                {
                    return m_SommaEstinzioni;
                }

                set
                {
                    decimal oldValue = m_SommaEstinzioni;
                    if (oldValue == value)
                        return;
                    m_SommaEstinzioni = value;
                    DoChanged("SommaEstinzioni", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la somma delle trattenute volontarie
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SommaTrattenuteVolontarie
            {
                get
                {
                    return m_SommaTrattenuteVolontarie;
                }

                set
                {
                    decimal oldValue = m_SommaTrattenuteVolontarie;
                    if (oldValue == value)
                        return;
                    m_SommaTrattenuteVolontarie = value;
                    DoChanged("SommaTrattenuteVolontarie", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la somma dei pignoramenti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SommaPignoramenti
            {
                get
                {
                    return m_SommaPignoramenti;
                }

                set
                {
                    decimal oldValue = m_SommaPignoramenti;
                    if (oldValue == value)
                        return;
                    m_SommaPignoramenti = value;
                    DoChanged("SommaPignoramenti", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stipendio netto del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal StipendioNetto
            {
                get
                {
                    return m_StipendioNetto;
                }

                set
                {
                    decimal oldValue = m_StipendioNetto;
                    if (oldValue == value)
                        return;
                    m_StipendioNetto = value;
                    DoChanged("StipendioNetto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TFR del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal TFR
            {
                get
                {
                    return m_TFR;
                }

                set
                {
                    decimal oldValue = m_TFR;
                    if (oldValue == value)
                        return;
                    m_TFR = value;
                    DoChanged("TFR", value, oldValue);
                }
            }

            public int ValutazioneGARF
            {
                get
                {
                    return m_ValutazioneGARF;
                }

                set
                {
                    int oldValue = m_ValutazioneGARF;
                    if (oldValue == value)
                        return;
                    m_ValutazioneGARF = value;
                    DoChanged("ValutazioneGARF", value, oldValue);
                }
            }

            public string TipoImpiego
            {
                get
                {
                    return m_TipoImpiego;
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 1);
                    string oldValue = m_TipoImpiego;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoImpiego = value;
                    DoChanged("TipoImpiego", value, oldValue);
                }
            }

            public float Eta
            {
                get
                {
                    return m_Eta;
                }

                set
                {
                    float oldValue = m_Eta;
                    if (oldValue == value)
                        return;
                    m_Eta = value;
                    DoChanged("Eta", value, oldValue);
                }
            }

            public float Anzianita
            {
                get
                {
                    return m_Anzianita;
                }

                set
                {
                    float oldValue = m_Anzianita;
                    if (oldValue == value)
                        return;
                    m_Anzianita = value;
                    DoChanged("Anzianita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'offerta fatta per la cessione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public COffertaCQS OffertaCQS
            {
                get
                {
                    if (m_OffertaCQS is null)
                        m_OffertaCQS = Offerte.GetItemById(m_IDOffertaCQS);
                    return m_OffertaCQS;
                }

                set
                {
                    var oldValue = m_OffertaCQS;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    SetOffertaCQS(value);
                    DoChanged("OffertaCQS", value, oldValue);
                }
            }

            protected internal virtual void SetOffertaCQS(COffertaCQS value)
            {
                m_OffertaCQS = value;
                m_IDOffertaCQS = DBUtils.GetID(value);
                // If (value IsNot Nothing) Then
                // Me.m_IDProdottoCQS = value.ProdottoID
                // Me.m_NomeProdottoCQS = value.NomeProdotto
                // Else
                // Me.m_IDProdottoCQS = 0
                // Me.m_NomeProdottoCQS = ""
                // End If
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'offerta CQS
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOffertaCQS
            {
                get
                {
                    return DBUtils.GetID(m_OffertaCQS, m_IDOffertaCQS);
                }

                set
                {
                    int oldValue = IDOffertaCQS;
                    if (oldValue == value)
                        return;
                    m_IDOffertaCQS = value;
                    m_OffertaCQS = null;
                    DoChanged("IDOffertaCQS", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'offerta fatta per la delega
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public COffertaCQS OffertaPD
            {
                get
                {
                    if (m_OffertaPD is null)
                        m_OffertaPD = Offerte.GetItemById(m_IDOffertaPD);
                    return m_OffertaPD;
                }

                set
                {
                    var oldValue = m_OffertaPD;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    SetOffertaPD(value);
                    DoChanged("OffertaPD", value, oldValue);
                }
            }

            protected internal virtual void SetOffertaPD(COffertaCQS value)
            {
                m_OffertaPD = value;
                m_IDOffertaPD = DBUtils.GetID(value);
                // If (value IsNot Nothing) Then
                // Me.m_IDProdottoPD = value.ProdottoID
                // Me.m_NomeProdottoPD = value.NomeProdotto
                // Else
                // Me.m_IDProdottoPD = 0
                // Me.m_NomeProdottoPD = ""
                // End If
            }

            public int IDOffertaPD
            {
                get
                {
                    return DBUtils.GetID(m_OffertaPD, m_IDOffertaPD);
                }

                set
                {
                    int oldValue = IDOffertaPD;
                    if (oldValue == value)
                        return;
                    m_IDOffertaPD = value;
                    m_OffertaPD = null;
                    DoChanged("IDOffertaPD", value, oldValue);
                }
            }

            public StatiConsulenza StatoConsulenza
            {
                get
                {
                    return m_StatoConsulenza;
                }

                internal set
                {
                    var oldValue = m_StatoConsulenza;
                    if (oldValue == value)
                        return;
                    m_StatoConsulenza = value;
                    DoChanged("StatoConsulenza", value, oldValue);
                }
            }

            public bool Visible
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, ConsulenzeFlags.HIDDEN) == false;
                }

                set
                {
                    if (Visible == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, ConsulenzeFlags.HIDDEN, value);
                    DoChanged("Visible", value, !value);
                }
            }

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
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            protected internal virtual void SetCliente(Anagrafica.CPersona value)
            {
                m_Cliente = (Anagrafica.CPersonaFisica)value;
                m_IDCliente = DBUtils.GetID(value);
            }

            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            public int IDRichiesta
            {
                get
                {
                    return DBUtils.GetID(m_Richiesta, m_IDRichiesta);
                }

                set
                {
                    int oldValue = IDRichiesta;
                    if (oldValue == value)
                        return;
                    m_IDRichiesta = value;
                    m_Richiesta = null;
                    DoChanged("IDRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la richiesta di finanziamento da cui è partita la consulenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiestaFinanziamento Richiesta
            {
                get
                {
                    if (m_Richiesta is null)
                        m_Richiesta = RichiesteFinanziamento.GetItemById(m_IDRichiesta);
                    return m_Richiesta;
                }

                set
                {
                    var oldValue = m_Richiesta;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiesta = value;
                    m_IDRichiesta = DBUtils.GetID(value);
                    DoChanged("Richiesta", value, oldValue);
                }
            }

            protected internal virtual void SetRichiesta(CRichiestaFinanziamento value)
            {
                m_Richiesta = value;
                m_IDRichiesta = DBUtils.GetID(value);
            }

            // Me.m_OfferteProposte = Nothing

            // Me.m_IDOffertaCorrente = 0
            // Me.m_OffertaCorrente = Nothing
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

            public CConsulentePratica Consulente
            {
                get
                {
                    if (m_Consulente is null)
                        m_Consulente = Consulenti.GetItemById(m_IDConsulente);
                    return m_Consulente;
                }

                set
                {
                    var oldValue = m_Consulente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Consulente = value;
                    m_IDConsulente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeConsulente = value.Nome;
                    DoChanged("Consulente", value, oldValue);
                }
            }

            public string NomeConsulente
            {
                get
                {
                    return m_NomeConsulente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeConsulente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeConsulente = value;
                    DoChanged("NomeConsulente", value, oldValue);
                }
            }

            public DateTime? DataConsulenza
            {
                get
                {
                    return m_DataConsulenza;
                }

                set
                {
                    var oldValue = m_DataConsulenza;
                    if (oldValue == value == true)
                        return;
                    m_DataConsulenza = value;
                    DoChanged("DataConsulenza", value, oldValue);
                }
            }

            public DateTime? DataConferma
            {
                get
                {
                    return m_DataConferma;
                }

                set
                {
                    var oldValue = m_DataConferma;
                    if (oldValue == value == true)
                        return;
                    m_DataConferma = value;
                    DoChanged("DataConferma", value, oldValue);
                }
            }

            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha effettuato la proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser PropostaDa
            {
                get
                {
                    if (m_PropostaDa is null)
                        m_PropostaDa = Sistema.Users.GetItemById(m_IDPropostaDa);
                    return m_PropostaDa;
                }

                set
                {
                    var oldValue = m_PropostaDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PropostaDa = value;
                    m_IDPropostaDa = DBUtils.GetID(value);
                    DoChanged("PropostaDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha proposto la consulenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPropostaDa
            {
                get
                {
                    return DBUtils.GetID(m_PropostaDa, m_IDPropostaDa);
                }

                set
                {
                    int oldValue = IDPropostaDa;
                    if (oldValue == value)
                        return;
                    m_IDPropostaDa = value;
                    m_PropostaDa = null;
                    DoChanged("IDPropostaDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui è stata proposta la consulenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataProposta
            {
                get
                {
                    return m_DataProposta;
                }

                set
                {
                    var oldValue = m_DataProposta;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataProposta = value;
                    DoChanged("DataProposta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha registrato l'accettazione o il rifiuto della proposta da parte del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser ConfermataDa
            {
                get
                {
                    if (m_ConfermataDa is null)
                        m_ConfermataDa = Sistema.Users.GetItemById(m_IDConfermataDa);
                    return m_ConfermataDa;
                }

                set
                {
                    var oldValue = m_ConfermataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ConfermataDa = value;
                    m_IDConfermataDa = DBUtils.GetID(value);
                    DoChanged("ConfermataDa", value, oldValue);
                }
            }

            public int IDConfermataDa
            {
                get
                {
                    return DBUtils.GetID(m_ConfermataDa, m_IDConfermataDa);
                }

                set
                {
                    int oldValue = IDConfermataDa;
                    if (oldValue == value)
                        return;
                    m_IDConfermataDa = value;
                    m_ConfermataDa = null;
                    DoChanged("IDConfermataDa", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDConsulenze";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_IDRichiesta = reader.Read("IDRichiesta", this.m_IDRichiesta);
                // reader.Read("Me.m_IDOffertaCorrente = 0
                // Me.m_OffertaCorrente = Nothing
                m_IDConsulente = reader.Read("IDConsulente", this.m_IDConsulente);
                m_NomeConsulente = reader.Read("NomeConsulente", this.m_NomeConsulente);
                m_DataConsulenza = reader.Read("DataConsulenza", this.m_DataConsulenza);
                m_DataProposta = reader.Read("DataProposta", this.m_DataConferma);
                m_DataConferma = reader.Read("DataConferma", this.m_DataConferma);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_StatoConsulenza = reader.Read("StatoConsulenza", this.m_StatoConsulenza);
                m_IDOffertaCQS = reader.Read("IDOffertaCQS", this.m_IDOffertaCQS);
                m_IDOffertaPD = reader.Read("IDOffertaPD", this.m_IDOffertaPD);
                m_MontanteLordo = reader.Read("MontanteLordo", this.m_MontanteLordo);
                m_NettoRicavo = reader.Read("NettoRicavo", this.m_NettoRicavo);
                m_SommaEstinzioni = reader.Read("SommaEstinzioni", this.m_SommaEstinzioni);
                m_SommaTrattenuteVolontarie = reader.Read("SommaTrattenuteVolontarie", this.m_SommaTrattenuteVolontarie);
                m_SommaPignoramenti = reader.Read("SommaPignoramenti", this.m_SommaPignoramenti);
                m_StipendioNetto = reader.Read("StipendioNetto", this.m_StipendioNetto);
                m_TFR = reader.Read("TFR", this.m_TFR);
                m_ValutazioneGARF = reader.Read("ValutazioneGARF", this.m_ValutazioneGARF);
                m_TipoImpiego = reader.Read("TipoImpiego", this.m_TipoImpiego);
                m_Eta = reader.Read("Eta", this.m_Eta);
                m_Anzianita = reader.Read("Anzianita", this.m_Anzianita);
                m_IDAzienda = reader.Read("IDAzienda", this.m_IDAzienda);
                m_NomeAzienda = reader.Read("NomeAzienda", this.m_NomeAzienda);
                m_DataAssunzione = reader.Read("DataAssunzione", this.m_DataAssunzione);
                m_IDPropostaDa = reader.Read("IDPropostaDa", this.m_IDPropostaDa);
                m_IDConfermataDa = reader.Read("IDConfermataDa", this.m_IDConfermataDa);
                m_IDContesto = reader.Read("IDContesto", this.m_IDContesto);
                m_TipoContesto = reader.Read("TipoContesto", this.m_TipoContesto);
                m_Durata = reader.Read("Durata", this.m_Durata);
                m_IDInseritoDa = reader.Read("IDInseritoDa", this.m_IDInseritoDa);
                m_IDStudioDiFattibilita = reader.Read("IDGruppo", this.m_IDStudioDiFattibilita);
                m_IDRichiestaApprovazione = reader.Read("IDRichiestaApprovazione", this.m_IDRichiestaApprovazione);
                m_MotivoAnnullamento = reader.Read("MotivoAnnullamento", this.m_MotivoAnnullamento);
                m_DettaglioAnnullamento = reader.Read("DettaglioAnnullamento", this.m_DettaglioAnnullamento);
                m_IDAnnullataDa = reader.Read("IDAnnullataDa", this.m_IDAnnullataDa);
                m_NomeAnnullataDa = reader.Read("NomeAnnullataDa", this.m_NomeAnnullataDa);
                m_DataAnnullamento = reader.Read("DataAnnullamento", this.m_DataAnnullamento);
                m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", this.m_IDFinestraLavorazione);
                m_IDUltimaVerifica = reader.Read("IDUltimaVerifica", this.m_IDUltimaVerifica);
                m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                // Me.m_IDProdottoCQS = reader.Read("IDProdottoCQS", Me.m_IDProdottoCQS)
                // Me.m_NomeProdottoCQS = reader.Read("NomeProdottoCQS", Me.m_NomeProdottoCQS)
                // Me.m_IDProdottoPD = reader.Read("IDProdottoPD", Me.m_IDProdottoPD)
                // Me.m_NomeProdottoPD = reader.Read("NomeProdottoPD", Me.m_NomeProdottoPD)
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IDRichiesta", IDRichiesta);
                // reader.Read("Me.m_IDOffertaCorrente = 0
                // Me.m_OffertaCorrente = Nothing
                writer.Write("IDConsulente", IDConsulente);
                writer.Write("NomeConsulente", m_NomeConsulente);
                writer.Write("DataConsulenza", m_DataConsulenza);
                writer.Write("DataConferma", m_DataConferma);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Flags", m_Flags);
                writer.Write("StatoConsulenza", m_StatoConsulenza);
                writer.Write("IDOffertaCQS", IDOffertaCQS);
                writer.Write("IDOffertaPD", IDOffertaPD);
                writer.Write("MontanteLordo", m_MontanteLordo);
                writer.Write("NettoRicavo", m_NettoRicavo);
                writer.Write("SommaEstinzioni", m_SommaEstinzioni);
                writer.Write("SommaTrattenuteVolontarie", m_SommaTrattenuteVolontarie);
                writer.Write("SommaPignoramenti", m_SommaPignoramenti);
                writer.Write("StipendioNetto", m_StipendioNetto);
                writer.Write("TFR", m_TFR);
                writer.Write("ValutazioneGARF", m_ValutazioneGARF);
                writer.Write("TipoImpiego", m_TipoImpiego);
                writer.Write("Eta", m_Eta);
                writer.Write("Anzianita", m_Anzianita);
                writer.Write("IDAzienda", IDAzienda);
                writer.Write("NomeAzienda", m_NomeAzienda);
                writer.Write("DataAssunzione", m_DataAssunzione);
                writer.Write("DataProposta", m_DataProposta);
                writer.Write("IDPropostaDa", IDPropostaDa);
                writer.Write("IDConfermataDa", IDConfermataDa);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("Durata", m_Durata);
                writer.Write("IDInseritoDa", IDInseritoDa);
                writer.Write("IDGruppo", IDStudioDiFattibilita);
                writer.Write("IDRichiestaApprovazione", IDRichiestaApprovazione);
                writer.Write("MotivoAnnullamento", m_MotivoAnnullamento);
                writer.Write("DettaglioAnnullamento", m_DettaglioAnnullamento);
                writer.Write("IDAnnullataDa", IDAnnullataDa);
                writer.Write("NomeAnnullataDa", m_NomeAnnullataDa);
                writer.Write("DataAnnullamento", m_DataAnnullamento);
                writer.Write("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.Write("IDUltimaVerifica", IDUltimaVerifica);
                writer.Write("IDCollaboratore", IDCollaboratore);
                // writer.Write("IDProdottoCQS", Me.IDProdottoCQS)
                // writer.Write("NomeProdottoCQS", Me.m_NomeProdottoCQS)
                // writer.Write("IDProdottoPD", Me.IDProdottoPD)
                // writer.Write("NomeProdottoPD", Me.m_NomeProdottoPD)
                return base.SaveToRecordset(writer);
            }


            // ------------------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDRichiesta", IDRichiesta);
                // reader.Read("Me.m_IDOffertaCorrente = 0
                // Me.m_OffertaCorrente = Nothing
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("NomeConsulente", m_NomeConsulente);
                writer.WriteAttribute("DataConsulenza", m_DataConsulenza);
                writer.WriteAttribute("DataConferma", m_DataConferma);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("StatoConsulenza", (int?)m_StatoConsulenza);
                writer.WriteAttribute("IDOffertaCQS", IDOffertaCQS);
                writer.WriteAttribute("IDOffertaPD", IDOffertaPD);
                writer.WriteAttribute("MontanteLordo", m_MontanteLordo);
                writer.WriteAttribute("NettoRicavo", m_NettoRicavo);
                writer.WriteAttribute("SommaEstinzioni", m_SommaEstinzioni);
                writer.WriteAttribute("SommaTrattenuteVolontarie", m_SommaTrattenuteVolontarie);
                writer.WriteAttribute("SommaPignoramenti", m_SommaPignoramenti);
                writer.WriteAttribute("StipendioNetto", m_StipendioNetto);
                writer.WriteAttribute("TFR", m_TFR);
                writer.WriteAttribute("ValutazioneGARF", m_ValutazioneGARF);
                writer.WriteAttribute("TipoImpiego", m_TipoImpiego);
                writer.WriteAttribute("Eta", m_Eta);
                writer.WriteAttribute("Anzianita", m_Anzianita);
                writer.WriteAttribute("IDAzienda", IDAzienda);
                writer.WriteAttribute("NomeAzienda", m_NomeAzienda);
                writer.WriteAttribute("DataAssunzione", m_DataAssunzione);
                writer.WriteAttribute("DataProposta", m_DataProposta);
                writer.WriteAttribute("IDPropostaDa", IDPropostaDa);
                writer.WriteAttribute("IDConfermataDa", IDConfermataDa);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("IDInseritoDa", IDInseritoDa);
                writer.WriteAttribute("IDStudioDiFattibilita", IDStudioDiFattibilita);
                writer.WriteAttribute("IDRichiestaApprovazione", IDRichiestaApprovazione);
                writer.WriteAttribute("MotivoAnnullamento", m_MotivoAnnullamento);
                writer.WriteAttribute("DettaglioAnnullamento", m_DettaglioAnnullamento);
                writer.WriteAttribute("IDAnnullataDa", IDAnnullataDa);
                writer.WriteAttribute("NomeAnnullataDa", m_NomeAnnullataDa);
                writer.WriteAttribute("DataAnnullamento", m_DataAnnullamento);
                writer.WriteAttribute("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.WriteAttribute("IDUltimaVerifica", IDUltimaVerifica);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                // writer.WriteAttribute("IDProdottoCQS", Me.IDProdottoCQS)
                // writer.WriteAttribute("NomeProdottoCQS", Me.m_NomeProdottoCQS)
                // writer.WriteAttribute("IDProdottoPD", Me.IDProdottoPD)
                // writer.WriteAttribute("NomeProdottoPD", Me.m_NomeProdottoPD)
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                if (!writer.GetSetting("CQSPDConsulenza.fastXMLserialize", false))
                {
                    writer.WriteTag("OffertaCQS", OffertaCQS);
                    writer.WriteTag("OffertaPD", OffertaPD);
                    writer.WriteTag("RichiestaApprovazione", RichiestaApprovazione);
                }
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

                    case "IDRichiesta":
                        {
                            m_IDRichiesta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // reader.Read("Me.m_IDOffertaCorrente = 0
                    // Me.m_OffertaCorrente = Nothing
                    case "IDConsulente":
                        {
                            m_IDConsulente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConsulente":
                        {
                            m_NomeConsulente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataConsulenza":
                        {
                            m_DataConsulenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataConferma":
                        {
                            m_DataConferma = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (ConsulenzeFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoConsulenza":
                        {
                            m_StatoConsulenza = (StatiConsulenza)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOffertaCQS":
                        {
                            m_IDOffertaCQS = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOffertaPD":
                        {
                            m_IDOffertaPD = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MontanteLordo":
                        {
                            m_MontanteLordo = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoRicavo":
                        {
                            m_NettoRicavo = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SommaEstinzioni":
                        {
                            m_SommaEstinzioni = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SommaTrattenuteVolontarie":
                        {
                            m_SommaTrattenuteVolontarie = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SommaPignoramenti":
                        {
                            m_SommaPignoramenti = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "StipendioNetto":
                        {
                            m_StipendioNetto = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TFR":
                        {
                            m_TFR = (decimal)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValutazioneGARF":
                        {
                            m_ValutazioneGARF = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoImpiego":
                        {
                            m_TipoImpiego = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Eta":
                        {
                            m_Eta = (float)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Anzianita":
                        {
                            m_Anzianita = (float)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAzienda":
                        {
                            m_NomeAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAssunzione":
                        {
                            m_DataAssunzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataProposta":
                        {
                            m_DataProposta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPropostaDa":
                        {
                            m_IDPropostaDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDConfermataDa":
                        {
                            m_IDConfermataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoContesto":
                        {
                            m_TipoContesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDInseritoDa":
                        {
                            m_IDInseritoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDStudioDiFattibilita":
                        {
                            m_IDStudioDiFattibilita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRichiestaApprovazione":
                        {
                            m_IDRichiestaApprovazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OffertaCQS":
                        {
                            m_OffertaCQS = (COffertaCQS)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "OffertaPD":
                        {
                            m_OffertaPD = (COffertaCQS)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "RichiestaApprovazione":
                        {
                            m_RichiestaApprovazione = (CRichiestaApprovazione)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "MotivoAnnullamento":
                        {
                            m_MotivoAnnullamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioAnnullamento":
                        {
                            m_DettaglioAnnullamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAnnullataDa":
                        {
                            m_IDAnnullataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAnnullataDa":
                        {
                            m_NomeAnnullataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAnnullamento":
                        {
                            m_DataAnnullamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazione":
                        {
                            m_IDFinestraLavorazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUltimaVerifica":
                        {
                            m_IDUltimaVerifica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Case "IDProdottoCQS" : Me.m_IDProdottoCQS = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    // Case "NomeProdottoCQS" : Me.m_NomeProdottoCQS = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                    // Case "IDProdottoPD" : Me.m_IDProdottoPD = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    // Case "NomeProdottoPD" : Me.m_NomeProdottoPD = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return "Consulenza a " + m_NomeCliente + " del " + Sistema.Formats.FormatUserDateTime(m_DataConsulenza);
            }

            public override CModulesClass GetModule()
            {
                return Consulenze.Module;
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                OnInserita(new ItemEventArgs<CQSPDConsulenza>(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                Consulenze.doConsulenzaEliminata(new ItemEventArgs<CQSPDConsulenza>(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                Consulenze.doConsulenzaModificata(new ItemEventArgs<CQSPDConsulenza>(this));
            }

            protected virtual void OnInserita(ItemEventArgs<CQSPDConsulenza> e)
            {
                Inserita?.Invoke(this, e);
                Consulenze.Module.DispatchEvent(new Sistema.EventDescription("consulenza_inserita", "L'operatore " + Sistema.Users.CurrentUser.Nominativo + " ha inserito la consulenza N°" + DBUtils.GetID(this) + " per il cliente " + NomeCliente, this));
                Consulenze.doConsulenzaInserita(e);
            }

            /// <summary>
        /// Propone la consulenza al cliente
        /// </summary>
        /// <remarks></remarks>
            public void Proponi()
            {
                // If (Me.m_StatoConsulenza <> StatiConsulenza.INSERITA) Then Throw New InvalidOperationException("La consulenza deve essere in stato Inserita")

                var ra = RichiestaApprovazione;
                CMotivoScontoPratica mr = null;
                if (ra is object)
                    mr = ra.MotivoRichiesta;
                if (ra is object)
                {
                    if (ra.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                    {
                        throw new InvalidOperationException("Lo studio di fattibilità è stato bocciato da " + ra.NomeConfermataDa);
                    }
                    else if (ra.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA)
                    {
                    }
                    // OK
                    else if (mr is object && !mr.SoloSegnalazione && ra.StatoRichiesta != StatoRichiestaApprovazione.APPROVATA)
                    {
                        if (!Configuration.ConsentiProposteSenzaSupervisore)
                            throw new InvalidOperationException("Questa proposta deve essere prima approvata da un supervisore");
                        // Throw New InvalidOperationException("Lo studio di fattibilità è in attesa di essere valutato da un supervisore")
                    }
                }

                m_StatoConsulenza = StatiConsulenza.PROPOSTA;
                m_DataProposta = DMD.DateUtils.Now();
                m_PropostaDa = Sistema.Users.CurrentUser;
                m_IDPropostaDa = DBUtils.GetID(m_PropostaDa);
                SetChanged(true);
                Save();
                var e = new ItemEventArgs<CQSPDConsulenza>(this);
                OnProposta(e);
                Consulenze.doConsulenzaProposta(e);
                Consulenze.Module.DispatchEvent(new Sistema.EventDescription("consulenza_proposta", "L'operatore " + Sistema.Users.CurrentUser.Nominativo + " ha proposto la consulenza N°" + DBUtils.GetID(this) + " al cliente " + NomeCliente, this));
            }

            protected virtual void OnProposta(ItemEventArgs<CQSPDConsulenza> e)
            {
                Proposta?.Invoke(this, e);
            }



            /// <summary>
        /// Il cliente ha accettato la consulenza
        /// </summary>
        /// <remarks></remarks>
            public void Accetta()
            {
                // If (Me.m_StatoConsulenza <> StatiConsulenza.PROPOSTA) Then Throw New InvalidOperationException("La consulenza deve essere in stato Proposta")
                if (RichiestaApprovazione is object)
                {
                    if (RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                    {
                        throw new InvalidOperationException("Lo studio di fattibilità è stato bocciato da " + RichiestaApprovazione.NomeConfermataDa);
                    }
                    else if (RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA)
                    {
                    }
                    // OK
                    else if (!Configuration.ConsentiProposteSenzaSupervisore)
                        throw new InvalidOperationException("Lo studio di fattibilità è in attesa di essere valutato da un supervisore");
                }

                m_StatoConsulenza = StatiConsulenza.ACCETTATA;
                m_DataConferma = DMD.DateUtils.Now();
                m_ConfermataDa = Sistema.Users.CurrentUser;
                m_IDConfermataDa = DBUtils.GetID(m_ConfermataDa);
                SetChanged(true);
                Save();
                var e = new ItemEventArgs<CQSPDConsulenza>(this);
                OnAccettata(e);
                Consulenze.doConsulenzaAccettata(e);
                Consulenze.Module.DispatchEvent(new Sistema.EventDescription("consulenza_accettata", "L'operatore " + Sistema.Users.CurrentUser.Nominativo + " ha registrato la conferma della consulenza N°" + DBUtils.GetID(this) + " per il cliente " + NomeCliente, this));
            }

            protected virtual void OnAccettata(ItemEventArgs<CQSPDConsulenza> e)
            {
                Accettata?.Invoke(this, e);
            }

            /// <summary>
        /// Il cliente ha rifiutato la proposta
        /// </summary>
        /// <remarks></remarks>
            public void Rifiuta(string motivo, string dettaglio)
            {
                // If (Me.m_StatoConsulenza <> StatiConsulenza.PROPOSTA) Then Throw New InvalidOperationException("La consulenza deve essere in stato Proposta")
                if (RichiestaApprovazione is object)
                {
                    if (RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                    {
                        throw new InvalidOperationException("Lo studio di fattibilità è stato bocciato da " + RichiestaApprovazione.NomeConfermataDa);
                    }
                    else if (RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA)
                    {
                    }
                    // OK
                    else
                    {
                        // Throw New InvalidOperationException("Lo studio di fattibilità è in attesa di essere valutato da un supervisore")
                        RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA;
                        RichiestaApprovazione.MotivoConferma = motivo;
                        RichiestaApprovazione.DettaglioConferma = dettaglio;
                        RichiestaApprovazione.Save();
                    }
                }

                m_MotivoAnnullamento = DMD.Strings.Trim(motivo);
                m_DettaglioAnnullamento = dettaglio;
                m_StatoConsulenza = StatiConsulenza.RIFIUTATA;
                m_DataConferma = DMD.DateUtils.Now();
                m_ConfermataDa = Sistema.Users.CurrentUser;
                m_IDConfermataDa = DBUtils.GetID(m_ConfermataDa);
                SetChanged(true);
                Save();
                var e = new ItemEventArgs<CQSPDConsulenza>(this);
                OnRifiutata(e);
                Consulenze.doConsulenzaRifiutata(e);
                Consulenze.Module.DispatchEvent(new Sistema.EventDescription("consulenza_rifiutata", "L'operatore " + Sistema.Users.CurrentUser.Nominativo + " ha registrato il rifiuto della consuenza N°" + DBUtils.GetID(this) + " per il cliente " + NomeCliente, this));
            }

            protected virtual void OnRifiutata(ItemEventArgs<CQSPDConsulenza> e)
            {
                Rifiutata?.Invoke(this, e);
            }

            /// <summary>
        /// L'operazione non è fattibile
        /// </summary>
        /// <remarks></remarks>
            public void Boccia(string motivo, string dettaglio)
            {
                // If (Me.m_StatoConsulenza >= StatiConsulenza.ACCETTATA) Then Throw New InvalidOperationException("Stato")
                var pratiche = Pratiche.GetPraticheByProposta(this);
                if (pratiche.Count > 0)
                    throw new InvalidOperationException("Impossibile bocciare una proposta che ha generato una pratica valida");
                m_StatoConsulenza = StatiConsulenza.BOCCIATA;
                m_MotivoAnnullamento = DMD.Strings.Trim(motivo);
                m_DettaglioAnnullamento = dettaglio;
                m_DataAnnullamento = DMD.DateUtils.Now();
                m_AnnullataDa = Sistema.Users.CurrentUser;
                m_IDAnnullataDa = DBUtils.GetID(m_AnnullataDa);
                m_NomeAnnullataDa = m_AnnullataDa.Nominativo;
                SetChanged(true);
                Save();
                var ra = RichiestaApprovazione;
                if (ra is object && ra.StatoRichiesta <= StatoRichiestaApprovazione.PRESAINCARICO)
                {
                    ra.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA;
                    ra.ConfermataDa = Sistema.Users.CurrentUser;
                    ra.DataConferma = DMD.DateUtils.Now();
                    ra.DettaglioConferma = "La proposta è stata bocciata";
                    ra.Save();
                }

                var e = new ItemEventArgs<CQSPDConsulenza>(this);
                OnBocciata(e);
                Consulenze.doConsulenzaBocciata(e);
                Consulenze.Module.DispatchEvent(new Sistema.EventDescription("consulenza_bocciata", "L'operatore " + Sistema.Users.CurrentUser.Nominativo + " ha bocciato la consuenza N°" + DBUtils.GetID(this) + " per il cliente " + NomeCliente, this));
            }

            protected virtual void OnBocciata(ItemEventArgs<CQSPDConsulenza> e)
            {
                Bocciata?.Invoke(this, e);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della richiesta di approvazione corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDRichiestaApprovazione
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaApprovazione, m_IDRichiestaApprovazione);
                }

                set
                {
                    int oldValue = IDRichiestaApprovazione;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaApprovazione = value;
                    m_RichiestaApprovazione = null;
                    DoChanged("IDRichiestaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la richiesta di approvazione corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiestaApprovazione RichiestaApprovazione
            {
                get
                {
                    if (m_RichiestaApprovazione is null)
                    {
                        m_RichiestaApprovazione = RichiesteApprovazione.GetItemById(m_IDRichiestaApprovazione);
                        if (m_RichiestaApprovazione is object)
                            m_RichiestaApprovazione.SetOggettoApprovabile(this);
                    }

                    return m_RichiestaApprovazione;
                }

                set
                {
                    var oldValue = m_RichiestaApprovazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaApprovazione = value;
                    m_IDRichiestaApprovazione = DBUtils.GetID(value);
                    if (value is object)
                        value.SetOggettoApprovabile(this);
                    DoChanged("RichiestaApprovazione", value, oldValue);
                }
            }

            protected internal virtual void SetRichiestaApprovazione(CRichiestaApprovazione value)
            {
                m_RichiestaApprovazione = value;
                m_IDRichiestaApprovazione = DBUtils.GetID(value);
            }

            /// <summary>
        /// Segnala la consulenza ad un supervisore e la mette in stato di valutazione
        /// </summary>
        /// <param name="motivo"></param>
        /// <param name="dettaglio"></param>
        /// <param name="parametri"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiestaApprovazione RichiediApprovazione(string motivo, string dettaglio, string parametri)
            {
                // If (Me.m_StatoConsulenza <> StatiConsulenza.INSERITA) Then
                // Throw New ArgumentException("Solo gli studi di fattibilità non ancora proposti possono essere sottoposti ai supervisori")
                // End If

                var rich = RichiestaApprovazione;

                // If (rich IsNot Nothing) AndAlso (rich.StatoRichiesta >= StatoRichiestaApprovazione.ATTESA) Then
                // Throw New InvalidOperationException("Lo studio di fattibilità è già in attesa di approvazione")
                // End If

                if (rich is null)
                    rich = new CRichiestaApprovazione();
                rich.Cliente = Cliente;
                rich.OggettoApprovabile = this;
                rich.DataRichiestaApprovazione = DMD.DateUtils.Now();
                rich.UtenteRichiestaApprovazione = Sistema.Users.CurrentUser;
                rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA;
                rich.MotivoRichiesta = MotiviSconto.GetItemByName(motivo);
                rich.NomeMotivoRichiesta = motivo;
                rich.ParametriRichiesta = parametri;
                rich.DescrizioneRichiesta = dettaglio;
                rich.Stato = ObjectStatus.OBJECT_VALID;
                rich.PuntoOperativo = PuntoOperativo;
                rich.Save();

                // Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.RICHIEDEAPPROVAZIONE, True)
                // Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.APPROVATA, False)
                RichiestaApprovazione = rich;
                Save();
                var e = new ItemEventArgs(this);
                OnRequireApprovation(e);
                Consulenze.DoOnRequireApprovation(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("require_approvation", Sistema.Users.CurrentUser.Nominativo + " richiede l'approvazione dell'offerta fatta per la pratica ID: " + DBUtils.GetID(this), this));
                return rich;
            }

            public void Sollecita()
            {
                var rich = RichiestaApprovazione;
                if (rich is null || rich.Stato != ObjectStatus.OBJECT_VALID)
                    throw new ArgumentNullException("RichiestaApprovazione");
                switch (rich.StatoRichiesta)
                {
                    case StatoRichiestaApprovazione.NONCHIESTA:
                    case StatoRichiestaApprovazione.ANNULLATA:
                        {
                            rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA;
                            break;
                        }

                    case StatoRichiestaApprovazione.APPROVATA:
                    case StatoRichiestaApprovazione.NEGATA:
                        {
                            throw new ArgumentException("Non puoi sollecitare delle richieste già negate o approvate");
                            break;
                        }
                }

                rich.DescrizioneRichiesta = DMD.Strings.Combine(rich.DescrizioneRichiesta, Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - sollecitata da " + Sistema.Users.CurrentUser.Nominativo, DMD.Strings.vbNewLine);
                rich.Save();

                // Me.Save()
                var e = new ItemEventArgs(this);
                OnRequireApprovation(e);
                Consulenze.DoOnRequireApprovation(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("require_approvation", Sistema.Users.CurrentUser.Nominativo + " richiede l'approvazione dell'offerta fatta per la pratica ID: " + DBUtils.GetID(this), this));
            }

            protected virtual void OnRequireApprovation(ItemEventArgs e)
            {
                RequireApprovation?.Invoke(this, e);
            }

            public CRichiestaApprovazione PrendiInCarico()
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                if (RichiestaApprovazione is null || RichiestaApprovazione.StatoRichiesta != StatoRichiestaApprovazione.ATTESA)
                {
                    throw new InvalidOperationException("La studio di fattibilità non è in attesa di valutazione");
                }

                RichiestaApprovazione.PresaInCaricoDa = Sistema.Users.CurrentUser;
                RichiestaApprovazione.DataPresaInCarico = DMD.DateUtils.Now();
                RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.PRESAINCARICO;
                RichiestaApprovazione.Save();
                var note = new Sistema.CAnnotazione(RichiestaApprovazione);
                note.Valore = "<b>VALUTAZIONE IN CORSO</b><br/><b>Motivo</b>";
                note.Stato = ObjectStatus.OBJECT_VALID;
                note.Save();
                var e = new ItemEventArgs(this);
                OnPresaInCarico(e);
                Consulenze.DoOnInCarico(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("Presa_in_carico", Sistema.Users.CurrentUser.Nominativo + " ha preso in carico la pratica ID: " + DBUtils.GetID(this), this));
                return RichiestaApprovazione;
            }

            protected virtual void OnPresaInCarico(ItemEventArgs e)
            {
                PresaInCarico?.Invoke(this, e);
            }

            public CRichiestaApprovazione Approva(string motivo, string dettaglio)
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                if (RichiestaApprovazione is null || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                {
                    throw new InvalidOperationException("Lo studio di fattibilità non richiede l'approvazione o è già stata valutata");
                }

                RichiestaApprovazione.MotivoConferma = motivo;
                RichiestaApprovazione.DettaglioConferma = dettaglio;
                RichiestaApprovazione.ConfermataDa = Sistema.Users.CurrentUser;
                RichiestaApprovazione.DataConferma = DMD.DateUtils.Now();
                RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.APPROVATA;
                RichiestaApprovazione.Save();
                var note = new Sistema.CAnnotazione(RichiestaApprovazione);
                note.Valore = "<b>RICHIESTA APPROVATA</b><br/><b>Motivo</b>: " + motivo + "<br/><b>Dettaglio:</b> " + dettaglio;
                note.Stato = ObjectStatus.OBJECT_VALID;
                note.Save();
                var e = new ItemEventArgs(this);
                OnApproved(e);
                Consulenze.DoOnApprovata(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("Approved", Sistema.Users.CurrentUser.Nominativo + " ha approvato lo studio di fattibilità ID:" + DBUtils.GetID(this), this));
                return RichiestaApprovazione;
            }

            protected virtual void OnApproved(ItemEventArgs e)
            {
                Approvata?.Invoke(this, e);
            }

            public CRichiestaApprovazione Nega(string motivo, string dettaglio)
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                if (RichiestaApprovazione is null || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                {
                    throw new InvalidOperationException("Lo studio di fattibilità non richiede l'approvazione o è già stata valutata");
                }

                RichiestaApprovazione.MotivoConferma = motivo;
                RichiestaApprovazione.DettaglioConferma = dettaglio;
                RichiestaApprovazione.ConfermataDa = Sistema.Users.CurrentUser;
                RichiestaApprovazione.DataConferma = DMD.DateUtils.Now();
                RichiestaApprovazione.StatoRichiesta = StatoRichiestaApprovazione.NEGATA;
                RichiestaApprovazione.Save();
                var note = new Sistema.CAnnotazione(RichiestaApprovazione);
                note.Valore = "<b>RICHIESTA NEGATA</b><br/><b>Motivo</b>: " + motivo + "<br/><b>Dettaglio:</b> " + dettaglio;
                note.Stato = ObjectStatus.OBJECT_VALID;
                note.Save();
                var e = new ItemEventArgs(this);
                OnNegata(e);
                Consulenze.DoOnNegata(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("Rifiutata", Sistema.Users.CurrentUser.Nominativo + " ha bocciato lo studio di fattibilità ID: °" + DBUtils.GetID(this), this));
                return RichiestaApprovazione;
            }

            protected virtual void OnNegata(ItemEventArgs e)
            {
                Negata?.Invoke(this, e);
            }

            public string getDescrizioneOfferte()
            {
                string ret = "";
                if (OffertaCQS is object)
                    ret += "Cessione: " + OffertaCQS.ToString() + DMD.Strings.vbNewLine;
                if (OffertaPD is object)
                    ret += "Delega: " + OffertaPD.ToString() + DMD.Strings.vbNewLine;
                return ret;
            }






            // Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            // Return Me.CompareTo
            // End Function

            public int CompareTo(CQSPDConsulenza other) // Implements IComparable(Of CQSPDConsulenza).CompareTo
            {
                int ret = -DMD.DateUtils.Compare(DataConsulenza, other.DataConsulenza);
                if (ret == 0)
                    ret = -(DBUtils.GetID(this) - DBUtils.GetID(other));
                return ret;
            }

            int IComparable.CompareTo(object other)
            {
                return CompareTo((CQSPDConsulenza)other);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }

            public string NomePropostaDa
            {
                get
                {
                    if (PropostaDa is null)
                        return "";
                    return PropostaDa.Nominativo;
                }
            }

            public string NomeConfermataDa
            {
                get
                {
                    if (ConfermataDa is null)
                        return "";
                    return ConfermataDa.Nominativo;
                }
            }

            public string NumeroProposta
            {
                get
                {
                    return DMD.Strings.Right("00000000" + DMD.Strings.Hex(DBUtils.GetID(this)), 8);
                }
            }

            public decimal? NettoAllaMano
            {
                get
                {
                    decimal? nr = NettoRicavo;
                    if (nr.HasValue == false)
                        return default;
                    decimal? se = SommaEstinzioni;
                    if (se.HasValue == false)
                        return default;
                    return nr.Value - se.Value;
                }
            }
        }
    }
}