using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoRichiestaFinanziamento : int
        {
            INSERITA = 0,
            EVASA = 1
        }

        /// <summary>
    /// Valore che indica il tipo di specifica sull'importo della richiesta di finanziamento
    /// </summary>
    /// <remarks></remarks>
        public enum TipoRichiestaFinanziamento : int
        {
            /// <summary>
        /// Il cliente ha chiesto di avere il massimo possibile
        /// </summary>
        /// <remarks></remarks>
            MASSIMO_POSSIBILE = 0,

            /// <summary>
        /// Il cliente ha in mente un valore preciso pari a ImportoRichiesto
        /// </summary>
        /// <remarks></remarks>
            UGUALEA = 1,

            /// <summary>
        /// Al cliente servono almeno ImportoRichiesto
        /// </summary>
        /// <remarks></remarks>
            ALMENO = 2,

            /// <summary>
        /// Il cliente vuole totalizzare una somma compresa tra ImportoRichiesto e ImportoRichiesto1
        /// </summary>
        /// <remarks></remarks>
            TRA = 3,

            /// <summary>
        /// Il cliente non ha specificato alcun valore per la richiesta
        /// </summary>
        /// <remarks></remarks>
            NONSPECIFICATO = 4,

            /// <summary>
        /// Indica un consolidamento prestiti.
        /// 2060315 FSE Valore richiesto da K COPPOLA su richiesta di Carrano.
        /// Io ritengo che sia sbagliato inserire qui questo campo poiché non indica il tipo di richiesta ma il motivo della richiesta
        /// che andrebbe specificato nelle note o al massimo aggiungendo un ulteriore campo.
        /// Per evitare di iniziare un'altra discussione inutile in cui Carrano vuole per forza avere ragione solo perché è il capo procedo con la sua richiesta anche se la ritengo errata.
        /// </summary>
        /// <remarks></remarks>
            CONSOLIDAMENTO = 5
        }

        /// <summary>
    /// Flags che specificano alcune proprietà della richiesta di finanziamento
    /// </summary>
    /// <remarks></remarks>
        [Flags]
        public enum RichiestaFinanziamentoFlags : int
        {
            None = 0,

            /// <summary>
        /// Il cliente ha effettuato la stessa richiesta presso un'altra Finanziaria
        /// </summary>
        /// <remarks></remarks>
            ClienteHaChiestoAdAltri = 1,

            /// <summary>
        /// L'operatore ha verificato l'eventuale presenza di altre richieste
        /// </summary>
        /// <remarks></remarks>
            VerificatoAltreRichieste = 2,

            /// <summary>
        /// Il cliente ha effettuato una richiesta di conteggio estintivo
        /// </summary>
        /// <remarks></remarks>
            HaRichieste = 4,

            /// <summary>
        /// Un amministratore ha soppresso i messaggi generati da questa richiesta
        /// </summary>
        /// <remarks></remarks>
            Soppressa = 8
        }

        [Serializable]
        public class CRichiestaFinanziamento : Databases.DBObjectPO, IComparable, ICloneable
        {
            private DateTime m_Data;
            private string m_TipoFonte;
            private int m_IDFonte;
            private IFonte m_Fonte;
            private string m_NomeFonte;
            private string m_Referrer;
            private decimal? m_ImportoRichiesto;
            private decimal? m_ImportoRichiesto1;
            private decimal? m_RataMassima;
            private int? m_DurataMassima;
            private int m_IDCliente;
            private Anagrafica.CPersonaFisica m_Cliente;
            private string m_NomeCliente;
            private string m_Note;
            private int m_IDAssegnatoA;
            private Sistema.CUser m_AssegnatoA;
            private string m_NomeAssegnatoA;
            private int m_IDPresaInCaricoDa;
            private Sistema.CUser m_PresaInCaricoDa;
            private string m_NomePresaInCarocoDa;
            private string m_IDFonteStr;
            private string m_IDCampagnaStr;
            private string m_IDAnnuncioStr;
            private string m_IDKeyWordStr;
            private int m_IDPratica;
            private CPraticaCQSPD m_Pratica;
            private Anagrafica.CCanale m_Canale;
            private int m_IDCanale;
            private string m_NomeCanale;
            private Anagrafica.CCanale m_Canale1;
            private int m_IDCanale1;
            private string m_NomeCanale1;
            private StatoRichiestaFinanziamento m_StatoRichiesta;
            private TipoRichiestaFinanziamento m_TipoRichiesta;
            private int m_IDContesto;
            private string m_TipoContesto;
            private double m_Durata;
            private int m_IDPrivacy;
            private Sistema.CAttachment m_Privacy;
            private int m_IDModulo;
            private Sistema.CAttachment m_Modulo;
            private RichiestaFinanziamentoFlags m_Flags;
            private CRichiesteConteggiXRichiesta m_Conteggi;
            private CAltriPreventiviXRichiesta m_AltriPreventivi;
            private int m_IDFinestraLavorazione;
            private FinestraLavorazione m_FinestraLavorazione;
            private string m_Scopo;
            private int m_IDCollaboratore;
            private CCollaboratore m_Collaboratore;
            private string m_NomeCollaboratore;

            public CRichiestaFinanziamento()
            {
                m_Data = DMD.DateUtils.Now();
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = DMD.Strings.vbNullString;
                m_Referrer = DMD.Strings.vbNullString;
                m_ImportoRichiesto = default;
                m_RataMassima = default;
                m_DurataMassima = default;
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = DMD.Strings.vbNullString;
                m_Note = DMD.Strings.vbNullString;
                m_IDAssegnatoA = 0;
                m_AssegnatoA = null;
                m_NomeAssegnatoA = DMD.Strings.vbNullString;
                m_IDPresaInCaricoDa = 0;
                m_PresaInCaricoDa = null;
                m_NomePresaInCarocoDa = DMD.Strings.vbNullString;
                m_IDFonteStr = DMD.Strings.vbNullString;
                m_IDCampagnaStr = DMD.Strings.vbNullString;
                m_IDAnnuncioStr = DMD.Strings.vbNullString;
                m_IDKeyWordStr = DMD.Strings.vbNullString;
                m_IDPratica = 0;
                m_Pratica = null;
                m_Canale = null;
                m_IDCanale = 0;
                m_NomeCanale = DMD.Strings.vbNullString;
                m_Canale1 = null;
                m_IDCanale1 = 0;
                m_NomeCanale1 = DMD.Strings.vbNullString;
                m_StatoRichiesta = StatoRichiestaFinanziamento.INSERITA;
                m_TipoRichiesta = TipoRichiestaFinanziamento.MASSIMO_POSSIBILE;
                m_ImportoRichiesto1 = default;
                m_IDContesto = 0;
                m_TipoContesto = DMD.Strings.vbNullString;
                m_Durata = 0d;
                m_IDPrivacy = 0;
                m_Privacy = null;
                m_Flags = RichiestaFinanziamentoFlags.None;
                m_IDModulo = 0;
                m_Modulo = null;
                m_Conteggi = null;
                m_AltriPreventivi = null;
                m_IDFinestraLavorazione = 0;
                m_FinestraLavorazione = null;
                m_Scopo = "";
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
                m_NomeCollaboratore = "";
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
                    m_NomeCollaboratore = "";
                    if (value is object)
                        m_NomeCollaboratore = value.NomePersona;
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            public string NomeCollaboratore
            {
                get
                {
                    return m_NomeCollaboratore;
                }

                set
                {
                    string oldValue = m_NomeCollaboratore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCollaboratore = value;
                    DoChanged("NomeCollaboratore", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta l'ID della finestra di lavorazione in cui è stata effettuata la richiesta
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

            /// <summary>
        /// Restituisce o imposta la finestra di lavorazione in cui è stata effettuata la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Restituisce la collezione dei conteggi estintivi collegati a questa richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiesteConteggiXRichiesta Conteggi
            {
                get
                {
                    if (m_Conteggi is null)
                        m_Conteggi = new CRichiesteConteggiXRichiesta(this);
                    return m_Conteggi;
                }
            }

            /// <summary>
        /// Restituisce la collezione dei preventivi ricevuti dalla concorrenza collegati a questa richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CAltriPreventiviXRichiesta AltriPreventivi
            {
                get
                {
                    if (m_AltriPreventivi is null)
                        m_AltriPreventivi = new CAltriPreventiviXRichiesta(this);
                    return m_AltriPreventivi;
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public RichiestaFinanziamentoFlags Flags
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
        /// Restituisce o imposta lo scopo della richiesta di finanziamento (Es. Consolidamento, Liquidità, Rinnovo)
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Scopo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Scopo = value;
                    DoChanged("Scopo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'allegato caricato come informativa sulla privacy
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPrivacy
            {
                get
                {
                    return DBUtils.GetID(m_Privacy, m_IDPrivacy);
                }

                set
                {
                    int oldValue = IDPrivacy;
                    if (oldValue == value)
                        return;
                    m_IDPrivacy = value;
                    m_Privacy = null;
                    DoChanged("IDPrivacy", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'allegato Privacy
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CAttachment Privacy
            {
                get
                {
                    if (m_Privacy is null)
                        m_Privacy = Sistema.Attachments.GetItemById(m_IDPrivacy);
                    return m_Privacy;
                }

                set
                {
                    var oldValue = m_Privacy;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDPrivacy = DBUtils.GetID(value);
                    m_Privacy = value;
                    DoChanged("Privacy", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'allegato caricato contenente il modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDModulo
            {
                get
                {
                    return DBUtils.GetID(m_Modulo, m_IDModulo);
                }

                set
                {
                    int oldValue = IDModulo;
                    if (oldValue == value)
                        return;
                    m_IDModulo = value;
                    m_Modulo = null;
                    DoChanged("IDModulo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la richiesta può generare messaggi e e-mail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Soppressa
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, RichiestaFinanziamentoFlags.Soppressa);
                }

                set
                {
                    if (Soppressa == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, RichiestaFinanziamentoFlags.Soppressa, value);
                    DoChanged("Soppressa", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'allegato del modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CAttachment Modulo
            {
                get
                {
                    if (m_Modulo is null)
                        m_Modulo = Sistema.Attachments.GetItemById(m_IDModulo);
                    return m_Modulo;
                }

                set
                {
                    var oldValue = m_Modulo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDModulo = DBUtils.GetID(value);
                    m_Modulo = value;
                    DoChanged("Modulo", value, oldValue);
                }
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
                    double oldValue = m_Durata;
                    if (DMD.Arrays.Compare(value, oldValue) == 0)
                        return;

                    m_Durata = value;
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
        /// Restituisce o imposta il tipo della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoRichiestaFinanziamento TipoRichiesta
            {
                get
                {
                    return m_TipoRichiesta;
                }

                set
                {
                    var oldValue = m_TipoRichiesta;
                    if (oldValue == value)
                        return;
                    m_TipoRichiesta = value;
                    DoChanged("TipoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'estremo superiore dell'importo richiesto nel caso di TipoRichieste (TRA)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ImportoRichiesto1
            {
                get
                {
                    return m_ImportoRichiesto1;
                }

                set
                {
                    var oldValue = m_ImportoRichiesto1;
                    if (oldValue == value == true)
                        return;
                    m_ImportoRichiesto1 = value;
                    DoChanged("ImportoRichiesto1", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoRichiestaFinanziamento StatoRichiesta
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
        /// Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CCanale Canale
            {
                get
                {
                    if (m_Canale is null)
                        m_Canale = Anagrafica.Canali.GetItemById(m_IDCanale);
                    return m_Canale;
                }

                set
                {
                    var oldValue = Canale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Canale = value;
                    m_IDCanale = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCanale = value.Nome;
                    DoChanged("Canale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del canale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCanale
            {
                get
                {
                    return DBUtils.GetID(m_Canale, m_IDCanale);
                }

                set
                {
                    int oldValue = IDCanale;
                    if (oldValue == value)
                        return;
                    m_IDCanale = value;
                    m_Canale = null;
                    DoChanged("IDCanale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del canale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCanale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCanale = value;
                    DoChanged("NomeCanale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CCanale Canale1
            {
                get
                {
                    if (m_Canale1 is null)
                        m_Canale1 = Anagrafica.Canali.GetItemById(m_IDCanale1);
                    return m_Canale1;
                }

                set
                {
                    var oldValue = Canale1;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Canale1 = value;
                    m_IDCanale1 = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCanale1 = value.Nome;
                    DoChanged("Canale1", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del canale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCanale1
            {
                get
                {
                    return DBUtils.GetID(m_Canale1, m_IDCanale1);
                }

                set
                {
                    int oldValue = IDCanale1;
                    if (oldValue == value)
                        return;
                    m_IDCanale1 = value;
                    m_Canale1 = null;
                    DoChanged("IDCanale1", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del canale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeCanale1
            {
                get
                {
                    return m_NomeCanale1;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCanale1;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCanale1 = value;
                    DoChanged("NomeCanale1", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data
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

            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    string oldValue = m_TipoFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

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
                    m_IDFonte = value;
                    m_Fonte = null;
                    DoChanged("IDFonte", value, oldValue);
                }
            }

            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null)
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    if (DBUtils.GetID((Databases.IDBObjectBase)value) == IDFonte)
                        return;
                    var oldValue = Fonte;
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID((Databases.IDBObjectBase)value);
                    if (value is object)
                        m_NomeFonte = value.Nome;
                    DoChanged("Fonte", value, oldValue);
                }
            }

            public string NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFonte = value;
                    DoChanged("NomeFonte", value, oldValue);
                }
            }

            public string Referrer
            {
                get
                {
                    return m_Referrer;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Referrer;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Referrer = value;
                    DoChanged("Referrer", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'importo richiesto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Se il valore specificato non è NULL esso deve essere positivo</exception>
        /// <remarks></remarks>
            public decimal? ImportoRichiesto
            {
                get
                {
                    return m_ImportoRichiesto;
                }

                set
                {
                    if (value.HasValue && value.Value <= 0m)
                        throw new ArgumentOutOfRangeException("ImportoRichiesto");
                    var oldValue = m_ImportoRichiesto;
                    if (oldValue.Equals(value))
                        return;
                    m_ImportoRichiesto = value;
                    DoChanged("ImportoRichiesto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la massima rata che il cliente desidera pagare.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Se il valore specificato non è NULL esso deve essere positivo</exception>
        /// <remarks></remarks>
            public decimal? RataMassima
            {
                get
                {
                    return m_RataMassima;
                }

                set
                {
                    if (value.HasValue && value.Value <= 0m)
                        throw new ArgumentOutOfRangeException("RataMassima");
                    var oldValue = m_RataMassima;
                    if (oldValue.Equals(value))
                        return;
                    m_RataMassima = value;
                    DoChanged("RataMassima", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata massima desiderata per il finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Se il valore specificato non è NULL esso deve essere positivo</exception>
        /// <remarks></remarks>
            public int? DurataMassima
            {
                get
                {
                    return m_DurataMassima;
                }

                set
                {
                    if (value.HasValue && value.Value <= 0)
                        throw new ArgumentOutOfRangeException("DurataMassima");
                    var oldValue = m_DurataMassima;
                    if (oldValue == value == true)
                        return;
                    m_DurataMassima = value;
                    DoChanged("DurataMassima", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del cliente che ha effettuato la richiesta
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
        /// Restituisce o imposta l'anagrafica del cliente che ha effettuato la richiesta
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
                    // If (GetID(value) = Me.IDCliente) Then Exit Property
                    var oldValue = Cliente;
                    m_IDCliente = DBUtils.GetID(value);
                    m_Cliente = value;
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                }
            }

            protected internal virtual void SetCliente(Anagrafica.CPersonaFisica value)
            {
                m_Cliente = value;
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
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }



            /// <summary>
        /// Restituisce o imposta delle annotazioni testuali
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
        /// Restituisce o imposta l'ID dell'operatore a cui è assegnata la richiesta.
        /// Se questo valore è 0 la richiesta è associata al punto operativo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAssegnatoA
            {
                get
                {
                    return DBUtils.GetID(m_AssegnatoA, m_IDAssegnatoA);
                }

                set
                {
                    int oldValue = IDAssegnatoA;
                    if (oldValue == value)
                        return;
                    m_IDAssegnatoA = value;
                    m_AssegnatoA = null;
                    DoChanged("IDAssegnatoA", value, oldValue);
                }
            }

            public Sistema.CUser AssegnatoA
            {
                get
                {
                    if (m_AssegnatoA is null)
                        m_AssegnatoA = Sistema.Users.GetItemById(m_IDAssegnatoA);
                    return m_AssegnatoA;
                }

                set
                {
                    if (DBUtils.GetID(value) == IDAssegnatoA)
                        return;
                    var oldValue = AssegnatoA;
                    m_AssegnatoA = value;
                    m_IDAssegnatoA = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAssegnatoA = value.Nominativo;
                    DoChanged("AssegnatoA", value, oldValue);
                }
            }

            public string NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeAssegnatoA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssegnatoA = value;
                    DoChanged("NomeAssegnatoA", value, oldValue);
                }
            }

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

            public Sistema.CUser PresaInCaricoDa
            {
                get
                {
                    if (m_PresaInCaricoDa is null)
                        m_PresaInCaricoDa = Sistema.Users.GetItemById(m_IDPresaInCaricoDa);
                    return m_PresaInCaricoDa;
                }

                set
                {
                    if (DBUtils.GetID(value) == IDPresaInCaricoDa)
                        return;
                    var oldValue = PresaInCaricoDa;
                    m_PresaInCaricoDa = value;
                    m_IDPresaInCaricoDa = DBUtils.GetID(value);
                    if (value is object)
                        m_NomePresaInCarocoDa = value.Nominativo;
                    DoChanged("PresaInCaricoDa", value, oldValue);
                }
            }

            public string NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCarocoDa;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomePresaInCarocoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePresaInCarocoDa = value;
                    DoChanged("NomePresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che identifica la fonte
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IDFonteStr
            {
                get
                {
                    return m_IDFonteStr;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IDFonteStr;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDFonteStr = value;
                    DoChanged("IDFonteStr", value, oldValue);
                }
            }
            /// <summary>
        /// Restituisce o imposta una stringa che indica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IDCampagnaStr
            {
                get
                {
                    return m_IDCampagnaStr;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IDCampagnaStr;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDCampagnaStr = value;
                    DoChanged("IDCampagnaStr", value, oldValue);
                }
            }

            public string IDAnnuncioStr
            {
                get
                {
                    return m_IDAnnuncioStr;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValeu = m_IDAnnuncioStr;
                    if ((oldValeu ?? "") == (value ?? ""))
                        return;
                    m_IDAnnuncioStr = value;
                    DoChanged("IDAnnuncioStr", value, oldValeu);
                }
            }

            public string IDKeyWordStr
            {
                get
                {
                    return m_IDKeyWordStr;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IDKeyWordStr;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDKeyWordStr = value;
                    DoChanged("IDKeyWordStr", value, oldValue);
                }
            }

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
                    DoChanged("IDPRatica", value, oldValue);
                }
            }

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
                    var oldValue = Pratica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDPratica = DBUtils.GetID(value);
                    m_Pratica = value;
                    DoChanged("Pratica", value, oldValue);
                }
            }

            public string StatoEx
            {
                get
                {
                    if (m_IDPratica != 0)
                        return "Generata la pratica N°" + DMD.Strings.Hex(m_IDPratica);
                    if (m_IDPresaInCaricoDa != 0)
                        return "Presa in carico da " + NomePresaInCaricoDa;
                    if (m_IDAssegnatoA != 0)
                        return "Assegnata a " + NomeAssegnatoA;
                    return "Non assegnata";
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return RichiesteFinanziamento.Module;
            }

            public override string GetTableName()
            {
                return "tbl_RichiesteFinanziamenti";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Data = reader.Read("Data", this.m_Data);
                m_IDFonte = reader.Read("IDFonte", this.m_IDFonte);
                m_NomeFonte = reader.Read("NomeFonte", this.m_NomeFonte);
                m_Referrer = reader.Read("Referrer", this.m_Referrer);
                m_ImportoRichiesto = reader.Read("ImportoRichiesto", this.m_ImportoRichiesto);
                m_RataMassima = reader.Read("RataMassimima", this.m_RataMassima);
                m_DurataMassima = reader.Read("DurataMassima", this.m_DurataMassima);
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_Note = reader.Read("Note", this.m_Note);
                m_IDAssegnatoA = reader.Read("IDAssegnatoA", this.m_IDAssegnatoA);
                m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", this.m_NomeAssegnatoA);
                m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", this.m_IDPresaInCaricoDa);
                m_NomePresaInCarocoDa = reader.Read("NomePresaInCaricoDa", this.m_NomePresaInCarocoDa);
                m_IDFonteStr = reader.Read("IDFonteStr", this.m_IDFonteStr);
                m_IDCampagnaStr = reader.Read("IDCampagnaStr", this.m_IDCampagnaStr);
                m_IDAnnuncioStr = reader.Read("IDAnnuncioStr", this.m_IDAnnuncioStr);
                m_IDKeyWordStr = reader.Read("IDKeyWordStr", this.m_IDKeyWordStr);
                m_IDPratica = reader.Read("IDPratica", this.m_IDPratica);
                m_TipoFonte = reader.Read("TipoFonte", this.m_TipoFonte);
                m_IDCanale = reader.Read("IDCanale", this.m_IDCanale);
                m_NomeCanale = reader.Read("NomeCanale", this.m_NomeCanale);
                m_IDCanale1 = reader.Read("IDCanale1", this.m_IDCanale1);
                m_NomeCanale1 = reader.Read("NomeCanale1", this.m_NomeCanale1);
                m_StatoRichiesta = reader.Read("StatoRichiesta", this.m_StatoRichiesta);
                m_TipoRichiesta = reader.Read("TipoRichiesta", this.m_TipoRichiesta);
                m_ImportoRichiesto1 = reader.Read("ImportoRichiesto1", this.m_ImportoRichiesto1);
                m_IDContesto = reader.Read("IDContesto", this.m_IDContesto);
                m_TipoContesto = reader.Read("TipoContesto", this.m_TipoContesto);
                m_Durata = reader.Read("Durata", this.m_Durata);
                m_IDPrivacy = reader.Read("IDPrivacy", this.m_IDPrivacy);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_IDModulo = reader.Read("IDModulo", this.m_IDModulo);
                m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", this.m_IDFinestraLavorazione);
                m_Scopo = reader.Read("Scopo", this.m_Scopo);
                m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                m_NomeCollaboratore = reader.Read("NomeCollaboratore", this.m_NomeCollaboratore);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Data", m_Data);
                writer.Write("IDFonte", IDFonte);
                writer.Write("NomeFonte", m_NomeFonte);
                writer.Write("Referrer", m_Referrer);
                writer.Write("ImportoRichiesto", m_ImportoRichiesto);
                writer.Write("RataMassimima", m_RataMassima);
                writer.Write("DurataMassima", m_DurataMassima);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("Note", m_Note);
                writer.Write("IDAssegnatoA", IDAssegnatoA);
                writer.Write("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.Write("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.Write("NomePresaInCaricoDa", m_NomePresaInCarocoDa);
                writer.Write("IDFonteStr", m_IDFonteStr);
                writer.Write("IDCampagnaStr", m_IDCampagnaStr);
                writer.Write("IDAnnuncioStr", m_IDAnnuncioStr);
                writer.Write("IDKeyWordStr", m_IDKeyWordStr);
                writer.Write("IDPratica", IDPratica);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDCanale", IDCanale);
                writer.Write("NomeCanale", m_NomeCanale);
                writer.Write("IDCanale1", IDCanale1);
                writer.Write("NomeCanale1", m_NomeCanale1);
                writer.Write("StatoRichiesta", m_StatoRichiesta);
                writer.Write("TipoRichiesta", m_TipoRichiesta);
                writer.Write("ImportoRichiesto1", m_ImportoRichiesto1);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("Durata", m_Durata);
                writer.Write("IDPrivacy", IDPrivacy);
                writer.Write("Flags", m_Flags);
                writer.Write("IDModulo", IDModulo);
                writer.Write("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.Write("Scopo", m_Scopo);
                writer.Write("IDCollaboratore", IDCollaboratore);
                writer.Write("NomeCollaboratore", m_NomeCollaboratore);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("NomeFonte", m_NomeFonte);
                writer.WriteAttribute("Referrer", m_Referrer);
                writer.WriteAttribute("ImportoRichiesto", m_ImportoRichiesto);
                writer.WriteAttribute("RataMassimima", m_RataMassima);
                writer.WriteAttribute("DurataMassima", m_DurataMassima);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDAssegnatoA", IDAssegnatoA);
                writer.WriteAttribute("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.WriteAttribute("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.WriteAttribute("NomePresaInCaricoDa", m_NomePresaInCarocoDa);
                writer.WriteAttribute("IDFonteStr", m_IDFonteStr);
                writer.WriteAttribute("IDCampagnaStr", m_IDCampagnaStr);
                writer.WriteAttribute("IDAnnuncioStr", m_IDAnnuncioStr);
                writer.WriteAttribute("IDKeyWordStr", m_IDKeyWordStr);
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDCanale", IDCanale);
                writer.WriteAttribute("NomeCanale", m_NomeCanale);
                writer.WriteAttribute("IDCanale1", IDCanale1);
                writer.WriteAttribute("NomeCanale1", m_NomeCanale1);
                writer.WriteAttribute("StatoRichiesta", (int?)m_StatoRichiesta);
                writer.WriteAttribute("TipoRichiesta", (int?)m_TipoRichiesta);
                writer.WriteAttribute("ImportoRichiesto1", m_ImportoRichiesto1);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("IDPrivacy", IDPrivacy);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("IDModulo", IDModulo);
                writer.WriteAttribute("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.WriteAttribute("Scopo", m_Scopo);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteAttribute("NomeCollaboratore", m_NomeCollaboratore);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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

                    case "Referrer":
                        {
                            m_Referrer = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ImportoRichiesto":
                        {
                            m_ImportoRichiesto = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RataMassimima":
                        {
                            m_RataMassima = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DurataMassima":
                        {
                            m_DurataMassima = (int?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAssegnatoA":
                        {
                            m_IDAssegnatoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAssegnatoA":
                        {
                            m_NomeAssegnatoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPresaInCaricoDa":
                        {
                            m_IDPresaInCaricoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePresaInCaricoDa":
                        {
                            m_NomePresaInCarocoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonteStr":
                        {
                            m_IDFonteStr = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCampagnaStr":
                        {
                            m_IDCampagnaStr = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAnnuncioStr":
                        {
                            m_IDAnnuncioStr = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDKeyWordStr":
                        {
                            m_IDKeyWordStr = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            m_TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCanale":
                        {
                            m_IDCanale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCanale":
                        {
                            m_NomeCanale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCanale1":
                        {
                            m_IDCanale1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCanale1":
                        {
                            m_NomeCanale1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoRichiesta":
                        {
                            m_StatoRichiesta = (StatoRichiestaFinanziamento)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoRichiesta":
                        {
                            m_TipoRichiesta = (TipoRichiestaFinanziamento)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ImportoRichiesto1":
                        {
                            m_ImportoRichiesto1 = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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

                    case "IDPrivacy":
                        {
                            m_IDPrivacy = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (RichiestaFinanziamentoFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDModulo":
                        {
                            m_IDModulo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazione":
                        {
                            m_IDFinestraLavorazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Scopo":
                        {
                            m_Scopo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeCollaboratore":
                        {
                            m_NomeCollaboratore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

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
                var ret = new System.Text.StringBuilder();
                if (Cliente is null)
                {
                    ret.Append("Il cliente (" + IDCliente + ")");
                }
                else
                {
                    ret.Append(Sistema.IIF(Cliente.Sesso == "F", "La Sig.ra ", "Il Sig. "));
                    ret.Append(Cliente.Nominativo);
                }

                ret.Append(" ha richiesto un finanziamento ");
                switch (m_TipoRichiesta)
                {
                    case TipoRichiestaFinanziamento.ALMENO:
                        {
                            ret.Append("di almeno " + Sistema.Formats.FormatValuta(ImportoRichiesto));
                            break;
                        }

                    case TipoRichiestaFinanziamento.MASSIMO_POSSIBILE:
                        {
                            ret.Append(" pari al massimo possibile");
                            break;
                        }

                    case TipoRichiestaFinanziamento.TRA:
                        {
                            ret.Append("tra " + Sistema.Formats.FormatValuta(ImportoRichiesto) + " e " + Sistema.Formats.FormatValuta(m_ImportoRichiesto1));
                            break;
                        }

                    case TipoRichiestaFinanziamento.UGUALEA:
                        {
                            ret.Append("di " + Sistema.Formats.FormatValuta(ImportoRichiesto));
                            break;
                        }
                }

                ret.Append(" in data " + Sistema.Formats.FormatUserDateTime(Data));
                return ret.ToString();
            }

            /// <summary>
        /// Prende in carico la richiesta
        /// </summary>
        /// <remarks></remarks>
            public void PrendiInCarico()
            {
                if (PresaInCaricoDa is object)
                    throw new InvalidOperationException("La richiesta è già stata presa in carico da " + NomePresaInCaricoDa);
                PresaInCaricoDa = Sistema.Users.CurrentUser;
                StatoRichiesta = StatoRichiestaFinanziamento.EVASA;
                Save();
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                RichiesteFinanziamento.doNuovaRichiesta(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                RichiesteFinanziamento.doRichiestaEliminata(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                RichiesteFinanziamento.doRichiestaModificata(new ItemEventArgs(this));
            }

            public int CompareTo(CRichiestaFinanziamento obj)
            {
                int ret = DMD.DateUtils.Compare(Data, obj.Data);
                if (ret == 0)
                    ret = ID.CompareTo(obj.ID);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CRichiestaFinanziamento)obj);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}