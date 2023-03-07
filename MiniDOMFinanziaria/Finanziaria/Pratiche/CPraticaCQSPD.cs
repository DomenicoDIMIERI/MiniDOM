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
        public enum PraticaFlags : int
        {
            NOTSET = 0,
            HIDDEN = 1,
            RICHIEDEAPPROVAZIONE = 2,
            APPROVATA = 4,
            DAVEDERE = 8,
            TRASFERITA = 16,
            DIRETTA_COLLABORATORE = 32
        }

        /// <summary>
        /// Pratica CQS/PD
        /// </summary>
        [Serializable]
        public class CPraticaCQSPD 
            : Databases.DBObjectPO, 
              IEstintore, 
              IOggettoApprovabile, 
              IOggettoVerificabile, 
              ICloneable
        {

            /// <summary>
            /// Evento generato quando viene generata una condizione di attenzione per la pratica
            /// </summary>
            public event PraticaWatchEventHandler PraticaWatch;

            /// <summary>
            /// Evento generato quando viene generata una condizione di attenzione per la pratica
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void PraticaWatchEventHandler(object sender, ItemEventArgs e);


            /// <summary>
            /// Evento generato quando viene effettuata una correzione alla pratica
            /// </summary>
            /// <remarks></remarks>
            public event CorrettaEventHandler Corretta;

            /// <summary>
            /// Evento generato quando viene effettuata una correzione alla pratica
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void CorrettaEventHandler(object sender, ItemEventArgs e);

            /// <summary>
            /// Evento generato quando vine formulata un'offerta che richiede l'approvazione
            /// </summary>
            /// <remarks></remarks>
            public event IOggettoApprovabile.RequireApprovationEventHandler RequireApprovation;

           

            /// <summary>
            /// Evento generato quando la pratica subisce un passaggio di stato
            /// </summary>
            /// <remarks></remarks>
            public event StatusChangedEventHandler StatusChanged;

            /// <summary>
            /// Evento generato quando la pratica subisce un passaggio di stato
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void StatusChangedEventHandler(object sender, ItemEventArgs e);


            /// <summary>
            /// Evento generato quando l'offerta corrente viene approvata
            /// </summary>
            /// <remarks></remarks>
            public event ApprovataEventHandler Approvata;

            /// <summary>
            /// Evento generato quando l'offerta corrente viene approvata
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void ApprovataEventHandler(object sender, ItemEventArgs e);

            /// <summary>
            /// Evento generato quando l'offerta viene rifiutata
            /// </summary>
            /// <remarks></remarks>
            public event RifiutataEventHandler Rifiutata;

            /// <summary>
            /// Evento generato quando l'offerta viene rifiutata
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void RifiutataEventHandler(object sender, ItemEventArgs e);

            /// <summary>
            /// Evento generato quando l'offerta viene presa in carico
            /// </summary>
            /// <remarks></remarks>
            public event PresaInCaricoEventHandler PresaInCarico;

            /// <summary>
            /// Evento generato quando l'offerta viene presa in carico
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void PresaInCaricoEventHandler(object sender, ItemEventArgs e);


            // Azienda che ha registrato la pratica
            private int m_IDAzienda;
            [NonSerialized] private Anagrafica.CAzienda m_Azienda;
            private string m_TipoFonteCliente;
            private int m_IDFonteCliente;
            [NonSerialized] private IFonte m_FonteCliente;
            private string m_TipoFonteContatto;
            private int m_IDFonte;
            [NonSerialized] private IFonte m_Fonte;
            private string m_NomeFonte;
            [NonSerialized] private Anagrafica.CCanale m_Canale;
            private int m_IDCanale;
            private string m_NomeCanale;
            [NonSerialized] private Anagrafica.CCanale m_Canale1;
            private int m_IDCanale1;
            private string m_NomeCanale1;
            private int m_ClienteID;
            [NonSerialized] private Anagrafica.CPersonaFisica m_Cliente;
            private string m_NomeCliente;
            private string m_CognomeCliente;
            private Anagrafica.CIndirizzo m_NatoA;
            private DateTime? m_NatoIl;
            private Anagrafica.CIndirizzo m_ResidenteA;
            private string m_CodiceFiscale;
            private string m_PartitaIVA;
            private string m_Sesso;
            private string m_Telefono;
            private string m_Cellulare;
            private string m_Fax;
            private string m_eMail;
            // Private m_WebSite As String

            private Anagrafica.CImpiegato m_Impiego;
            private string m_NumeroEsterno;
            [NonSerialized] private CCQSPDCessionarioClass m_Cessionario;
            private int m_CessionarioID;
            private string m_NomeCessionario;
            [NonSerialized] private CProfilo m_Profilo;
            private int m_ProfiloID;
            private string m_NomeProfilo;
            private int m_ProdottoID;
            [NonSerialized] private CCQSPDProdotto m_Prodotto;
            private string m_NomeProdotto;
            private decimal? m_MontanteLordo;
            private decimal? m_NettoRicavo;
            private int? m_NumeroRate;
            private decimal? m_ValoreProvvMax;
            private decimal? m_ValoreRunning;
            private decimal? m_ValoreUpFront;
            private decimal? m_ValoreRappel;
            private CProvvigionale m_Provvigionale;
            private DateTime? m_DataDecorrenza;
            private PraticaFlags m_Flags;
            private int m_IDConsulente;
            [NonSerialized] private CConsulentePratica m_Consulente;
            [NonSerialized] private CStatoLavorazionePratica m_StatoDiLavorazioneAttuale;
            [NonSerialized] private CStatiLavorazionePraticaCollection m_StatiDiLavorazione;
            [NonSerialized] private CInfoPratica m_Info;

            // Private m_MacroStato As StatoPraticaEnum?

            private int m_IDConsulenza;
            private int m_IDConsulenzaOld;
            [NonSerialized] private CQSPDConsulenza m_Consulenza;
            private int m_IDRichiestaDiFinanziamento;
            [NonSerialized] private CRichiestaFinanziamento m_RichiestaDiFinanziamento;
            private string m_TipoContesto;
            private int m_IDContesto;
            private int m_Durata;
            private int m_IDRichiestaApprovazione;
            [NonSerialized] private CRichiestaApprovazione m_RichiestaApprovazione;
            private CKeyCollection m_Attributi;
            [NonSerialized] private CDocumentoPraticaCaricatoCollection m_Vincoli;
            [NonSerialized] private CEstinzioniXEstintoreCollection m_Estinzioni;
            private int m_IDFinestraLavorazione;
            [NonSerialized] private FinestraLavorazione m_FinestraLavorazione;
            private int m_IDTabellaFinanziaria;
            [NonSerialized] private CTabellaFinanziaria m_TabellaFinanziaria;
            private int m_IDTabellaVita;
            [NonSerialized] private CTabellaAssicurativa m_TabellaVita;
            private int m_IDTabellaImpiego;
            [NonSerialized] private CTabellaAssicurativa m_TabellaImpiego;
            private int m_IDTabellaCredito;
            [NonSerialized] private CTabellaAssicurativa m_TabellaCredito;
            private int m_IDUltimaVerifica;
            [NonSerialized] private VerificaAmministrativa m_UltimaVerifica;
            private DateTime? m_DataValuta;
            private DateTime? m_DataStampaSecci;
            private decimal? m_PremioDaCessionario;               // Premio eventuale che viene corrisposto dal cessionario all'agenzia
            private decimal? m_CapitaleFinanziato;               // Capitale finanziato
            private int m_IDCollaboratore;
            [NonSerialized] private CCollaboratore m_Collaboratore;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPraticaCQSPD()
            {
                m_Canale = null;
                m_IDCanale = 0;
                m_NomeCanale = DMD.Strings.vbNullString;
                m_Canale1 = null;
                m_IDCanale1 = 0;
                m_NomeCanale1 = "";
                m_ClienteID = 0;
                m_Cliente = null;
                m_NomeCliente = DMD.Strings.vbNullString;
                m_CognomeCliente = DMD.Strings.vbNullString;
                m_NatoA = new Anagrafica.CIndirizzo();
                m_NatoIl = default;
                m_ResidenteA = new Anagrafica.CIndirizzo();
                m_CodiceFiscale = DMD.Strings.vbNullString;
                m_PartitaIVA = DMD.Strings.vbNullString;
                m_Sesso = DMD.Strings.vbNullString;
                m_Telefono = DMD.Strings.vbNullString;
                m_Cellulare = DMD.Strings.vbNullString;
                m_Fax = DMD.Strings.vbNullString;
                m_eMail = DMD.Strings.vbNullString;
                m_Impiego = new Anagrafica.CImpiegato();
                m_Cessionario = null;
                m_CessionarioID = 0;
                m_NomeCessionario = DMD.Strings.vbNullString;
                m_Profilo = null;
                m_ProfiloID = 0;
                m_NomeProfilo = DMD.Strings.vbNullString;
                m_Prodotto = null;
                m_ProdottoID = 0;
                m_NomeProdotto = DMD.Strings.vbNullString;
                m_MontanteLordo = default;
                m_NettoRicavo = default;
                m_NumeroRate = default;
                m_ValoreProvvMax = default;
                m_ValoreRunning = default;
                m_ValoreUpFront = default;
                m_ValoreRappel = default;
                m_Provvigionale = new CProvvigionale();
                m_DataDecorrenza = default; // IIf(Calendar.Day(Calendar.Now) < 15, Calendar.DateAdd(DateTimeInterval.Day, 15, Calendar.GetMonthFirstDay(Now)), Calendar.GetNextMonthFirstDay(Calendar.Now))
                m_Flags = PraticaFlags.NOTSET;
                m_IDConsulente = 0;
                m_Consulente = null;
                m_NumeroRate = default;
                m_TipoFonteContatto = DMD.Strings.vbNullString;
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = DMD.Strings.vbNullString;
                m_StatiDiLavorazione = null;
                m_StatoDiLavorazioneAttuale = new CStatoLavorazionePratica();
                m_StatoDiLavorazioneAttuale.SetPratica(this);
                m_StatoDiLavorazioneAttuale.Stato = ObjectStatus.OBJECT_VALID;
                m_StatoDiLavorazioneAttuale.SetChanged(false);
                m_Info = null;
                m_NumeroEsterno = DMD.Strings.vbNullString;
                m_IDConsulenza = 0;
                m_IDConsulenzaOld = 0;
                m_Consulenza = null;
                m_IDRichiestaDiFinanziamento = 0;
                m_RichiestaDiFinanziamento = null;
                m_IDAzienda = 0;
                m_Azienda = null;
                m_IDContesto = 0;
                m_TipoContesto = "";
                m_Durata = 0;
                m_IDRichiestaApprovazione = 0;
                m_RichiestaApprovazione = null;
                m_Attributi = new CKeyCollection();
                m_Vincoli = null;
                m_Estinzioni = null;
                m_IDFinestraLavorazione = 0;
                m_FinestraLavorazione = null;
                m_IDTabellaFinanziaria = 0;
                m_TabellaFinanziaria = null;
                m_IDTabellaVita = 0;
                m_TabellaVita = null;
                m_IDTabellaImpiego = 0;
                m_TabellaImpiego = null;
                m_IDTabellaCredito = 0;
                m_TabellaCredito = null;
                m_IDUltimaVerifica = 0;
                m_UltimaVerifica = null;
                m_DataValuta = default;
                m_DataStampaSecci = default;
                m_PremioDaCessionario = default;
                m_CapitaleFinanziato = default;
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
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
                }
            }

            event IOggettoApprovabile.RifiutataEventHandler IOggettoApprovabile.Rifiutata
            {
                add
                {
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
                }
            }

            event IOggettoApprovabile.PresaInCaricoEventHandler IOggettoApprovabile.PresaInCarico
            {
                add
                {
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
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
                    m_Collaboratore = value;
                    m_IDCollaboratore = DBUtils.GetID(value);
                    DoChanged("IDCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il capitale finanziato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? CapitaleFinanziato
            {
                get
                {
                    return m_CapitaleFinanziato;
                }

                set
                {
                    var oldValue = m_CapitaleFinanziato;
                    if (oldValue == value == true)
                        return;
                    m_CapitaleFinanziato = value;
                    DoChanged("CapitaleFinanziato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il premio eventuale che il cessionario eroga all'agenzia per questa pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? PremioDaCessionario
            {
                get
                {
                    return m_PremioDaCessionario;
                }

                set
                {
                    var oldValue = m_PremioDaCessionario;
                    if (oldValue == value == true)
                        return;
                    m_PremioDaCessionario = value;
                    DoChanged("PremioDaCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il premio eventuale che il cessionario eroga all'agenzia per questa pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? PremioDaCessionario1
            {
                get
                {
                    return Sistema.Formats.ToValuta(Attributi.GetItemByKey("PremioDaCessionario1"));
                }

                set
                {
                    var oldValue = PremioDaCessionario1;
                    if (oldValue == value == true)
                        return;
                    Attributi.SetItemByKey("PremioDaCessionario1", value);
                    DoChanged("PremioDaCessionario1", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta la data valuta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataValuta
            {
                get
                {
                    return m_DataValuta;
                }

                set
                {
                    var oldValue = m_DataValuta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataValuta = value;
                    DoChanged("DataValuta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui è stato stampato il SECCI associato alla pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataStampaSecci
            {
                get
                {
                    return m_DataStampaSecci;
                }

                set
                {
                    var oldValue = m_DataStampaSecci;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataStampaSecci = value;
                    DoChanged("DataStampaSecci", value, oldValue);
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
        /// Restituisce o imposta l'ID della tabella Finanziaria associata all'offerta corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaFinanziaria
            {
                get
                {
                    return DBUtils.GetID(m_TabellaFinanziaria, m_IDTabellaFinanziaria);
                }

                set
                {
                    int oldValue = IDTabellaFinanziaria;
                    if (oldValue == value)
                        return;
                    m_IDTabellaFinanziaria = value;
                    m_TabellaFinanziaria = null;
                    DoChanged("IDTabellaFinanziaria", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la tabella Finanziaria associata all'offerta corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaFinanziaria TabellaFinanziaria
            {
                get
                {
                    if (m_TabellaFinanziaria is null)
                        m_TabellaFinanziaria = TabelleFinanziarie.GetItemById(m_IDTabellaFinanziaria);
                    return m_TabellaFinanziaria;
                }

                set
                {
                    var oldValue = TabellaFinanziaria;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TabellaFinanziaria = value;
                    m_IDTabellaFinanziaria = DBUtils.GetID(value);
                    DoChanged("TabellaFinanziaria", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella assicurativa usata per il rischio vita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaVita
            {
                get
                {
                    return DBUtils.GetID(m_TabellaVita, m_IDTabellaVita);
                }

                set
                {
                    int oldValue = IDTabellaVita;
                    if (oldValue == value)
                        return;
                    m_IDTabellaVita = value;
                    m_TabellaVita = null;
                    DoChanged("IDTabellaVita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la tabella Vita associata all'offerta corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaAssicurativa TabellaVita
            {
                get
                {
                    if (m_TabellaVita is null)
                        m_TabellaVita = TabelleAssicurative.GetItemById(m_IDTabellaVita);
                    return m_TabellaVita;
                }

                set
                {
                    var oldValue = TabellaVita;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TabellaVita = value;
                    m_IDTabellaVita = DBUtils.GetID(value);
                    DoChanged("TabellaVita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella assicurativa usata per il rischio Impiego
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaImpiego
            {
                get
                {
                    return DBUtils.GetID(m_TabellaImpiego, m_IDTabellaImpiego);
                }

                set
                {
                    int oldValue = IDTabellaImpiego;
                    if (oldValue == value)
                        return;
                    m_IDTabellaImpiego = value;
                    m_TabellaImpiego = null;
                    DoChanged("IDTabellaImpiego", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la tabella Impiego associata all'offerta corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaAssicurativa TabellaImpiego
            {
                get
                {
                    if (m_TabellaImpiego is null)
                        m_TabellaImpiego = TabelleAssicurative.GetItemById(m_IDTabellaImpiego);
                    return m_TabellaImpiego;
                }

                set
                {
                    var oldValue = TabellaImpiego;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TabellaImpiego = value;
                    m_IDTabellaImpiego = DBUtils.GetID(value);
                    DoChanged("TabellaImpiego", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella assicurativa usata per il rischio Credito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaCredito
            {
                get
                {
                    return DBUtils.GetID(m_TabellaCredito, m_IDTabellaCredito);
                }

                set
                {
                    int oldValue = IDTabellaCredito;
                    if (oldValue == value)
                        return;
                    m_IDTabellaCredito = value;
                    m_TabellaCredito = null;
                    DoChanged("IDTabellaCredito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la tabella Credito associata all'offerta corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaAssicurativa TabellaCredito
            {
                get
                {
                    if (m_TabellaCredito is null)
                        m_TabellaCredito = TabelleAssicurative.GetItemById(m_IDTabellaCredito);
                    return m_TabellaCredito;
                }

                set
                {
                    var oldValue = TabellaCredito;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TabellaCredito = value;
                    m_IDTabellaCredito = DBUtils.GetID(value);
                    DoChanged("TabellaCredito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della finestra di lavorazione in cui è stata lavorata la pratica
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

            public CKeyCollection Attributi
            {
                get
                {
                    return m_Attributi;
                }
            }

            /// <summary>
        /// Restituisce le informazioni relative al provvigionale riconosciuto al collaboratore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProvvigionale Provvigionale
            {
                get
                {
                    return m_Provvigionale;
                }
            }

            public int Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    int oldValue = m_Durata;
                    if (oldValue == value)
                        return;
                    m_Durata = value;
                    DoChanged("Durata", value, oldValue);
                }
            }

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
        /// Restituisce o imposta l'ID dell'azienda che ha registrato la pratica
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
                    return m_Azienda;
                }

                set
                {
                    var oldValue = m_Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Azienda = value;
                    m_IDAzienda = DBUtils.GetID(value);
                    DoChanged("Azienda", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta l'ID della consulenza da cui è partita la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDConsulenza
            {
                get
                {
                    return DBUtils.GetID(m_Consulenza, m_IDConsulenza);
                }

                set
                {
                    int oldValue = IDConsulenza;
                    if (oldValue == value)
                        return;
                    m_IDConsulenza = value;
                    m_Consulenza = null;
                    DoChanged("IDConsulenza", value, oldValue);
                }
            }

            protected override void DoChanged(string propName, object newVal = null, object oldVal = null)
            {
                base.DoChanged(propName, newVal, oldVal);
            }

            /// <summary>
        /// Restituisce o imposta la consulenza da cui è partita la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CQSPDConsulenza Consulenza
            {
                get
                {
                    if (m_Consulenza is null)
                        m_Consulenza = Consulenze.GetItemById(m_IDConsulenza);
                    return m_Consulenza;
                }

                set
                {
                    var oldValue = m_Consulenza;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Consulenza = value;
                    m_IDConsulenza = DBUtils.GetID(value);
                    DoChanged("Consulenza", value, oldValue);
                }
            }

            protected internal virtual void SetConsulenza(CQSPDConsulenza value)
            {
                m_Consulenza = value;
                m_IDConsulenza = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID del consulente principale della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Restituisce o imposta il consulente principale della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    if (oldValue == value)
                        return;
                    m_Consulente = value;
                    m_IDConsulente = DBUtils.GetID(value);
                    // If (value IsNot Nothing) Then Me.m_NomeConsulente = value.Nome
                    DoChanged("Consulente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il nome del consulente principale della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeConsulente
            {
                get
                {
                    if (Consulente is null)
                        return "";
                    return Consulente.Nome;
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della richiesta di finanziamento da cui è partita la pratica
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
        /// Restituisce o imposta la richiesta di finanziamento da cui è partita la pratica
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

            protected internal virtual void SetRichiestaDiFinanziamento(CRichiestaFinanziamento value)
            {
                m_RichiestaDiFinanziamento = value;
                m_IDRichiestaDiFinanziamento = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il numero pratica in un eventuale sistema esterno su cui è caricata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NumeroEsterno
            {
                get
                {
                    return m_NumeroEsterno;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroEsterno;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroEsterno = value;
                    DoChanged("NumeroPraticaEsterna", value, oldValue);
                }
            }

            public CInfoPratica Info
            {
                get
                {
                    if (m_Info is null)
                        m_Info = InfoPratica.GetItemByPratica(this);
                    if (m_Info is null)
                    {
                        m_Info = new CInfoPratica();
                        m_Info.Pratica = this;
                        m_Info.Save();
                    }

                    return m_Info;
                }
            }

            protected internal void SetInfo(CInfoPratica value)
            {
                m_Info = value;
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
        /// Genera una richiesta di approvazione
        /// </summary>
        /// <param name="motivo"></param>
        /// <param name="dettaglio"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiestaApprovazione RichiediApprovazione(string motivo, string dettaglio, string parametri)
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                var rich = RichiestaApprovazione;
                if (rich is object && rich.StatoRichiesta >= StatoRichiestaApprovazione.ATTESA)
                {
                    throw new InvalidOperationException("La pratica è già in attesa di approvazione");
                }

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
                rich.StatoRichiesta = StatoRichiestaApprovazione.ATTESA;
                rich.Save();

                // Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.RICHIEDEAPPROVAZIONE, True)
                // Me.m_Flags = Sistema.SetFlag(Me.m_Flags, PraticaFlags.APPROVATA, False)
                RichiestaApprovazione = rich;
                Save();
                var e = new ItemEventArgs(this);
                OnRequireApprovation(e);
                Pratiche.DoOnRequireApprovation(e);
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
                Pratiche.DoOnRequireApprovation(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("require_approvation", Sistema.Users.CurrentUser.Nominativo + " richiede l'approvazione dell'offerta fatta per la pratica ID: " + DBUtils.GetID(this), this));
            }

            protected virtual void OnRequireApprovation(ItemEventArgs e)
            {
                RequireApprovation?.Invoke(this, e);
            }


            /// <summary>
        /// Approva l'offerta corrente (se l'offerta non richiede l'approvazione genera un errore)
        /// </summary>
        /// <remarks></remarks>
            public CRichiestaApprovazione Approva(string motivo, string dettaglio)
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                if (RichiestaApprovazione is null || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                {
                    throw new InvalidOperationException("La pratica non richiede l'approvazione o è già stata valutata");
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
                OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA;
                OffertaCorrente.DataConfermaSconto = DMD.DateUtils.Now();
                OffertaCorrente.MotivoConfermaSconto = motivo;
                OffertaCorrente.DettaglioConfermaSconto = dettaglio;
                OffertaCorrente.Supervisore = Sistema.Users.CurrentUser;
                OffertaCorrente.Save();
                Info.ScontoAutorizzatoDa = Sistema.Users.CurrentUser;
                Info.ScontoAutorizzatoIl = OffertaCorrente.DataConfermaSconto;
                Info.ScontoAutorizzatoNote = dettaglio;
                Save();
                var e = new ItemEventArgs(this);
                OnApproved(e);
                Pratiche.DoOnApprovata(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("Approved", Sistema.Users.CurrentUser.Nominativo + " ha approvato l'offerta fatta per la pratica N°" + NumeroPratica, this));
                return RichiestaApprovazione;
            }


            /// <summary>
        /// Nega l'offerta corrente (se l'offerta non richiede l'approvazione genera un errore)
        /// </summary>
        /// <remarks></remarks>
            public CRichiestaApprovazione Nega(string motivo, string dettaglio)
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                if (RichiestaApprovazione is null || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.APPROVATA || RichiestaApprovazione.StatoRichiesta == StatoRichiestaApprovazione.NEGATA)
                {
                    throw new InvalidOperationException("La pratica non richiede l'approvazione o è già stata valutata");
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
                OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA;
                OffertaCorrente.DataConfermaSconto = DMD.DateUtils.Now();
                OffertaCorrente.MotivoConfermaSconto = motivo;
                OffertaCorrente.DettaglioConfermaSconto = dettaglio;
                OffertaCorrente.Supervisore = Sistema.Users.CurrentUser;
                OffertaCorrente.Save();
                Info.ScontoAutorizzatoDa = Sistema.Users.CurrentUser;
                Info.ScontoAutorizzatoIl = OffertaCorrente.DataConfermaSconto;
                Info.ScontoAutorizzatoNote = dettaglio;
                Save();
                var e = new ItemEventArgs(this);
                OnRifiutata(e);
                Pratiche.DoOnRifiutata(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("Rifiutata", Sistema.Users.CurrentUser.Nominativo + " ha rifiutato l'offerta fatta per la pratica N°" + NumeroPratica, this));
                return RichiestaApprovazione;
            }

            /// <summary>
        /// Prende in carico la richiesta di approvazione corrente
        /// </summary>
        /// <remarks></remarks>
            public CRichiestaApprovazione PrendiInCarico()
            {
                // If (Me.OffertaCorrente.StatoOfferta = StatoOfferta.APPROVATA) OrElse (Me.OffertaCorrente.StatoOfferta = StatoOfferta.RIFIUTATA) Then
                // Throw New InvalidOperationException("L'offerta corrente è già stata supervisionata")
                // End If
                if (RichiestaApprovazione is null || RichiestaApprovazione.StatoRichiesta != StatoRichiestaApprovazione.ATTESA)
                {
                    throw new InvalidOperationException("La pratica non è in attesa di valutazione");
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
                Pratiche.DoOnInCarico(e);
                GetModule().DispatchEvent(new Sistema.EventDescription("Presa_in_carico", Sistema.Users.CurrentUser.Nominativo + " ha preso in carico la pratica N°" + NumeroPratica, this));
                return RichiestaApprovazione;
            }

            protected virtual void OnPresaInCarico(ItemEventArgs e)
            {
                PresaInCarico?.Invoke(this, e);
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la pratica è visibile nell'interfaccia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Visible
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, PraticaFlags.HIDDEN) == false;
                }

                set
                {
                    if (value == Visible)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, PraticaFlags.HIDDEN, !value);
                    DoChanged("Visible", value, !value);
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
                        m_Canale1 = Anagrafica.Canali.GetItemById(m_IDCanale);
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

            public override CModulesClass GetModule()
            {
                return Pratiche.Module;
            }

            public Sistema.CAnnotazioni GetAnnotazioni()
            {
                var ret = new Sistema.CAnnotazioni(Cliente, this);
                return ret;
            }

            public Sistema.CAttachmentsCollection GetAttachments()
            {
                var ret = new Sistema.CAttachmentsCollection(Cliente, this);
                return ret;
            }



            /// <summary>
        /// Restituisce il numero della pratica nel sistema interno
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NumeroPratica
            {
                get
                {
                    return Strings.Right("00000000" + DMD.Integers.Hex(ID), 8);
                }
            }

            public DateTime? DataRicontatto
            {
                get
                {
                    var ret = DataRinnovo;
                    if (ret.HasValue)
                    {
                        int giorniAnticipo = Configuration.GiorniAnticipoRifin;
                        ret = DMD.DateUtils.DateAdd("d", -giorniAnticipo, ret);
                    }

                    return ret;
                }
            }

            public DateTime? DataRinnovo
            {
                get
                {
                    DateTime? ret = default;
                    if (DataDecorrenza.HasValue && NumeroRate.HasValue && NumeroRate.Value > 0)
                    {
                        double percDurata = Configuration.PercCompletamentoRifn;
                        ret = DMD.DateUtils.DateAdd("M", Maths.round(NumeroRate.Value * percDurata / 100d), DMD.DateUtils.GetMonthFirstDay(DataDecorrenza));
                    }

                    return ret;
                }
            }

            public DateTime DataCaricamento
            {
                get
                {
                    if (OffertaCorrente is object && OffertaCorrente.DataCaricamento.HasValue)
                    {
                        return OffertaCorrente.DataCaricamento.Value;
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
                    return m_DataDecorrenza;
                }

                set
                {
                    var oldValue = m_DataDecorrenza;
                    if (oldValue == value == true)
                        return;
                    m_DataDecorrenza = value;
                    DoChanged("DataDecorrenza", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la pratica è attenzionata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool DaVedere
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, PraticaFlags.DAVEDERE);
                }

                set
                {
                    if (DaVedere == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, PraticaFlags.DAVEDERE, value);
                    DoChanged("DaVedere", value, !value);
                }
            }


            /// <summary>
        /// Restituisce o imposta l'offerta corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public COffertaCQS OffertaCorrente
            {
                get
                {
                    return StatoDiLavorazioneAttuale.Offerta;
                }

                set
                {
                    StatoDiLavorazioneAttuale.Offerta = value;
                }
            }

            protected internal virtual void SetOffertaCorrente(COffertaCQS value)
            {
                StatoDiLavorazioneAttuale.SetOfferta(value);
            }

            public int IDOffertaCorrente
            {
                get
                {
                    return StatoDiLavorazioneAttuale.IDOfferta;
                }

                set
                {
                    StatoDiLavorazioneAttuale.IDOfferta = value;
                }
            }

            /// <summary>
        /// Restituisce o imposta la collezione dei documenti caricabili o caricati per la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CDocumentoPraticaCaricatoCollection Vincoli
            {
                get
                {
                    if (m_Vincoli is null)
                        m_Vincoli = new CDocumentoPraticaCaricatoCollection(this);
                    return m_Vincoli;
                }
            }

            /// <summary>
        /// Aggiorna i dati dell'anagrafica cliente sulla base dei dati di questa pratica
        /// </summary>
        /// <remarks></remarks>
            public void AggiornaAnagraficaCliente(Anagrafica.CPersonaFisica persona)
            {
                persona.Nome = NomeCliente;
                persona.Cognome = CognomeCliente;
                persona.Sesso = Sesso;
                persona.DataNascita = NatoIl;
                persona.CodiceFiscale = CodiceFiscale;
                persona.PartitaIVA = PartitaIVA;
                persona.NatoA.Citta = NatoA.Citta;
                persona.NatoA.Provincia = NatoA.Provincia;
                persona.ResidenteA.Citta = ResidenteA.Citta;
                persona.ResidenteA.Provincia = ResidenteA.Provincia;
                persona.ResidenteA.CAP = ResidenteA.CAP;
                // .TelefonoCasa(0).Valore = Me.Telefono
                // .Cellulare(0).Valore = Me.Cellulare
                // .Fax(0).Valore = Me.Fax
                // .eMail(0).Valore = Me.eMail
                // .WebSite(0).Valore = Me.WebSite
                persona.ResidenteA.ToponimoViaECivico = ResidenteA.ToponimoViaECivico;
            }

            /// <summary>
        /// Cerca di trovare il cliente corrispondente alla pratica in oggetto
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona TrovaCliente()
            {
                Anagrafica.CPersona ret;
                CCollection<Anagrafica.CPersona> col;
                ret = Cliente;
                if (ret is null)
                {
                    if (!string.IsNullOrEmpty(CodiceFiscale))
                    {
                        col = Anagrafica.Persone.FindPersoneByCF(CodiceFiscale);
                        if (col.Count > 0)
                        {
                            ret = col[0];
                        }
                    }
                }

                return ret;
            }

            // ''' <summary>
            // ''' Cambia lo stato della pratica attuale e restituisce un oggetto CStatoLavorazionePratica che rappresenta il nuovo stato di lavorazione
            // ''' </summary>
            // ''' <param name="nuovoStato"></param>
            // ''' <param name="data"></param>
            // ''' <param name="params"></param>
            // ''' <param name="note"></param>
            // ''' <param name="forza"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Function ChangeStatus(ByVal nuovoStato As CStatoLavorazionePratica, ByVal data As Date, ByVal params As String, ByVal note As String, ByVal forza As Boolean) As CStatoLavorazionePratica
            // Dim ret As New CStatoLavorazionePratica
            // ret.Pratica = Me
            // ret.StatoPratica = nuovoStato
            // ret.Data = data
            // ret.Parameters = params
            // ret.Note = note
            // ret.Forzato = forza
            // ret.Operatore = Users.CurrentUser
            // ret.Stato = ObjectStatus.OBJECT_VALID
            // Call Databases.Save(ret, GetCRMConnection)
            // Me.m_StatoPratica = ret
            // Me.m_IDStatoPratica = DBUtils.GetID(Me.m_StatoPratica, 0)
            // If Not (Me.m_StatiLavorazione Is Nothing) Then
            // Call Me.m_StatiLavorazione.Add(ret)
            // End If
            // Call Databases.Save(Me, GetCRMConnection)
            // Return ret
            // End Function

            // ''' <summary>
            // ''' Restituisce la collezione di tutti gli stati di lavorazione associati alla pratica
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public ReadOnly Property StatiDiLavorazione As CStatiDiLavorazionePratica
            // Get
            // If Me.m_StatiDiLavorazione Is Nothing Then
            // Me.m_StatiDiLavorazione = New CStatiDiLavorazionePratica
            // Me.m_StatiDiLavorazione.Load(Me)
            // End If
            // Return Me.m_StatiDiLavorazione
            // End Get
            // End Property

            public string Telefono
            {
                get
                {
                    return m_Telefono;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Telefono;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Telefono = value;
                    DoChanged("Telefono", value, oldValue);
                }
            }

            public string Cellulare
            {
                get
                {
                    return m_Cellulare;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Cellulare;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Cellulare = value;
                    DoChanged("Cellulare", value, oldValue);
                }
            }

            public string Fax
            {
                get
                {
                    return m_Fax;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Fax;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Fax = value;
                    DoChanged("Fax", value, oldValue);
                }
            }

            public string eMail
            {
                get
                {
                    return m_eMail;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_eMail;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_eMail = value;
                    DoChanged("eMail", value, oldValue);
                }
            }

            // Public Property WebSite As String
            // Get
            // Return Me.m_WebSite
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_WebSite
            // If (oldValue = value) Then Exit Property
            // Me.m_WebSite = value
            // Me.DoChanged("WebSite", value, oldValue)
            // End Set
            // End Property

            // ''' <summary>
            // ''' Restituisce o imposta l'ID dell'azienda erogante associata alla pratica
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property IDAmministrazione As Integer
            // Get
            // Return GetID(Me.m_Amministrazione, Me.m_IDAmministrazione)
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.IDAmministrazione
            // If oldValue = value Then Exit Property
            // Me.m_Amministrazione = Nothing
            // Me.m_IDAmministrazione = value
            // Me.DoChanged("IDAmministrazione", value, oldValue)
            // End Set
            // End Property

            // Public Property Amministrazione As CAzienda
            // Get
            // If Me.m_Amministrazione Is Nothing Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_IDAmministrazione)
            // Return Me.m_Amministrazione
            // End Get
            // Set(value As CAzienda)
            // Dim oldValue As CAzienda = Me.Amministrazione
            // If (oldValue = value) Then Exit Property
            // Me.m_Amministrazione = value
            // Me.m_IDAmministrazione = GetID(value)
            // If (value IsNot Nothing) Then Me.m_NomeAmministrazione = value.Nominativo
            // Me.DoChanged("Amministrazione", value, oldValue)
            // End Set
            // End Property

            // Public Property NomeAmministrazione As String
            // Get
            // Return Me.m_NomeAmministrazione
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_NomeAmministrazione
            // If (oldValue = value) Then Exit Property
            // Me.m_NomeAmministrazione = value
            // Me.DoChanged("NomeAmministrazione", value, oldValue)
            // End Set
            // End Property

            // Public Property IDEntePagante As Integer
            // Get
            // Return GetID(Me.m_EntePagante, Me.m_IDEntePagante)
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.IDEntePagante
            // If oldValue = value Then Exit Property
            // Me.m_IDEntePagante = value
            // Me.m_EntePagante = Nothing
            // Me.DoChanged("IDEntePagante", value, oldValue)
            // End Set
            // End Property

            // Public Property EntePagante As CAzienda
            // Get
            // If Me.m_EntePagante Is Nothing Then Me.m_EntePagante = Anagrafica.Aziende.GetItemById(Me.m_IDEntePagante)
            // Return Me.m_EntePagante
            // End Get
            // Set(value As CAzienda)
            // Dim oldValue As CAzienda = Me.EntePagante
            // If (oldValue = value) Then Exit Property
            // Me.m_EntePagante = value
            // Me.m_IDEntePagante = GetID(value)
            // 'If (value IsNot Nothing) Then Me.m_NomeEntePagante = value.Nominativo
            // Me.DoChanged("EntePagante", value, oldValue)
            // End Set
            // End Property

            // 'Public Property NomeEntePagante As String
            // '    Get
            // '        Return Me.m_NomeEntePagante
            // '    End Get
            // '    Set(value As String)
            // '        value = Trim(value)
            // '        Dim oldValue As String = Me.m_NomeEntePagante
            // '        If (oldValue = value) Then Exit Property
            // '        Me.m_NomeEntePagante = value
            // '        Me.DoChanged("NomeEntePagante", value, oldValue)
            // '    End Set
            // 'End Property


            public int? GiorniDiLavorazione
            {
                get
                {
                    if (StatoLiquidata is object && StatoRichiestaDelibera is object)
                    {
                        if (StatoRichiestaDelibera.Data.HasValue && StatoLiquidata.Data.HasValue)
                        {
                            return (int?)DMD.DateUtils.DateDiff(DateTimeInterval.Day, StatoRichiestaDelibera.Data.Value, StatoLiquidata.Data.Value);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            public int? NumeroRate
            {
                get
                {
                    return m_NumeroRate;
                }

                set
                {
                    var oldValue = m_NumeroRate;
                    if (oldValue == value == true)
                        return;
                    m_NumeroRate = value;
                    DoChanged("NumeroRate", value, oldValue);
                }
            }

            public decimal? Rata
            {
                get
                {
                    if (NumeroRate > 0 == true)
                        return MontanteLordo / NumeroRate;
                    return MontanteLordo;
                }

                set
                {
                    var oldValue = Rata;
                    if (oldValue == value == true)
                        return;
                    if (NumeroRate > 0 == true)
                    {
                        MontanteLordo = value * NumeroRate;
                    }
                    else
                    {
                        MontanteLordo = value;
                    }

                    DoChanged("Rata", value, oldValue);
                }
            }

            public int IDCessionario
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_CessionarioID);
                }

                set
                {
                    int oldValue = IDCessionario;
                    if (oldValue == value)
                        return;
                    m_CessionarioID = value;
                    m_Cessionario = null;
                    DoChanged("IDCessionario", value, oldValue);
                }
            }

            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_CessionarioID);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (oldValue == value)
                        return;
                    m_Cessionario = value;
                    m_CessionarioID = DBUtils.GetID(value);
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
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            public int IDProfilo
            {
                get
                {
                    return DBUtils.GetID(m_Profilo, m_ProfiloID);
                }

                set
                {
                    int oldValue = IDProfilo;
                    if (oldValue == value)
                        return;
                    m_ProfiloID = value;
                    m_Profilo = null;
                    DoChanged("IDProfilo", value, oldValue);
                }
            }

            public CProfilo Profilo
            {
                get
                {
                    if (m_Profilo is null)
                        m_Profilo = Profili.GetItemById(m_ProfiloID);
                    return m_Profilo;
                }

                set
                {
                    var oldValue = Profilo;
                    if (oldValue == value)
                        return;
                    m_Profilo = value;
                    m_ProfiloID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeProfilo = value.ProfiloVisibile;
                    DoChanged("Profilo", value, oldValue);
                }
            }

            public string NomeProfilo
            {
                get
                {
                    return m_NomeProfilo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeProfilo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProfilo = value;
                    DoChanged("NomeProfilo", value, oldValue);
                }
            }

            public string Sesso
            {
                get
                {
                    return m_Sesso;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_Sesso;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sesso = value;
                    DoChanged("Sesso", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale (nascosta) del montante lordo pagata all'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? Rappel
            {
                get
                {
                    if (m_ValoreRappel.HasValue && m_MontanteLordo.HasValue && m_MontanteLordo.Value > 0m)
                    {
                        return (double?)(m_ValoreRappel / m_MontanteLordo * 100);
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if ((value < 0 | value > 100) == true)
                        throw new ArgumentOutOfRangeException("Rappel");
                    var oldValue = Rappel;
                    if (oldValue == value == true)
                        return;
                    m_ValoreRappel = (decimal?)(value * (double?)m_MontanteLordo / 100);
                    DoChanged("Rappel", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione (nascosta) pagata all'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreRappel
            {
                get
                {
                    return m_ValoreRappel;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("ValoreRappel");
                    double? oldValue = (double?)m_ValoreRappel;
                    if (oldValue == (double?)value == true)
                        return;
                    m_ValoreRappel = value;
                    DoChanged("ValoreRappel", value, oldValue);
                }
            }

            // Public Property Costo As Decimal?
            // Get
            // Return Me.m_Costo
            // End Get
            // Set(value As Decimal?)
            // Dim oldValue As Decimal? = Me.m_Costo
            // If (oldValue = value) Then Exit Property
            // Me.m_Costo = value
            // Me.DoChanged("Costo", value, oldValue)
            // End Set
            // End Property

            public string TipoFonteContatto
            {
                get
                {
                    return m_TipoFonteContatto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TipoFonteContatto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonteContatto = value;
                    m_Fonte = null;
                    DoChanged("TipoFonteContatto", value, oldValue);
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
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_TipoFonteContatto, m_TipoFonteContatto, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    Databases.DBObjectBase oldValue = (Databases.DBObjectBase)Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
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

            /// <summary>
        /// Restituisce o imposta il tipo fonte d
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoFonteCliente
            {
                get
                {
                    return m_TipoFonteCliente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TipoFonteCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonteCliente = value;
                    m_FonteCliente = null;
                    DoChanged("TipoFonteCliente", value, oldValue);
                }
            }

            public int IDFonteCliente
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_FonteCliente, m_IDFonteCliente);
                }

                set
                {
                    int oldValue = IDFonteCliente;
                    if (oldValue == value)
                        return;
                    m_IDFonteCliente = value;
                    m_FonteCliente = null;
                    DoChanged("IDFonteCliente", value, oldValue);
                }
            }

            public IFonte FonteCliente
            {
                get
                {
                    if (m_FonteCliente is null)
                        m_FonteCliente = Anagrafica.Fonti.GetItemById(m_TipoFonteContatto, m_TipoFonteCliente, m_IDFonteCliente);
                    return m_FonteCliente;
                }

                set
                {
                    Databases.DBObjectBase oldValue = (Databases.DBObjectBase)FonteCliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_FonteCliente = value;
                    m_IDFonteCliente = DBUtils.GetID((Databases.IDBObjectBase)value);
                    DoChanged("FonteCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome registrato per il cliente
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

            public string CognomeCliente
            {
                get
                {
                    return m_CognomeCliente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_CognomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CognomeCliente = value;
                    DoChanged("CognomeCliente", value, oldValue);
                }
            }

            public string NominativoCliente
            {
                get
                {
                    return Strings.Trim(DMD.Strings.ToNameCase(NomeCliente) + " " + Strings.UCase(CognomeCliente));
                }
            }

            public string NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeProdotto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProdotto = value;
                    DoChanged("NomeProdotto", value, oldValue);
                }
            }

            public CCQSPDProdotto Prodotto
            {
                get
                {
                    if (m_Prodotto is null)
                        m_Prodotto = Prodotti.GetItemById(m_ProdottoID);
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = Prodotto;
                    if (oldValue == value)
                        return;
                    m_Prodotto = value;
                    m_ProdottoID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeProdotto = value.Nome;
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            public int IDProdotto
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_ProdottoID);
                }

                set
                {
                    int oldValue = IDProdotto;
                    if (oldValue == value)
                        return;
                    m_ProdottoID = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            public Anagrafica.CIndirizzo NatoA
            {
                get
                {
                    return m_NatoA;
                }
            }

            public DateTime? NatoIl
            {
                get
                {
                    return m_NatoIl;
                }

                set
                {
                    var oldValue = m_NatoIl;
                    if (oldValue == value == true)
                        return;
                    m_NatoIl = value;
                    DoChanged("NatoIl", value, oldValue);
                }
            }

            public Anagrafica.CIndirizzo ResidenteA
            {
                get
                {
                    return m_ResidenteA;
                }
            }

            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    string oldValue = m_CodiceFiscale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            public string PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }

                set
                {
                    value = Sistema.Formats.ParsePartitaIVA(value);
                    string oldValue = m_PartitaIVA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PartitaIVA = value;
                    DoChanged("PartitaIVA", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'ID della persona fisica che ha stipulato la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_ClienteID);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_ClienteID = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la persona fisica che ha stipulato la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetPersonaById(m_ClienteID);
                    if (m_Cliente is null)
                    {
                        var items = Anagrafica.Persone.FindPersoneByCF(CodiceFiscale);
                        foreach (Anagrafica.CPersona p in items)
                        {
                            if (p is Anagrafica.CPersonaFisica)
                            {
                                m_Cliente = (Anagrafica.CPersonaFisica)p;
                                break;
                            }
                        }
                    }

                    if (m_Cliente is null)
                    {
                        m_Cliente = new Anagrafica.CPersonaFisica();
                        AggiornaAnagraficaCliente(m_Cliente);
                        m_Cliente.Stato = Stato;
                    }

                    return m_Cliente;
                }

                set
                {
                    var oldValue = Cliente;
                    if (oldValue == value)
                        return;
                    m_Cliente = value;
                    m_ClienteID = DBUtils.GetID(value);
                    if (value is object && string.IsNullOrEmpty(m_CognomeCliente))
                    {
                        FromCliente();
                    }

                    DoChanged("Cliente", value, oldValue);
                }
            }

            protected internal virtual void SetCliente(Anagrafica.CPersonaFisica value)
            {
                m_Cliente = value;
                m_ClienteID = DBUtils.GetID(value);
            }

            /// <summary>
        /// Aggionra i dati relativi all'anagrafica ed all'impiego sulla base dei valori attuali del cliente
        /// </summary>
        /// <remarks></remarks>
            public void FromCliente()
            {
                NomeCliente = Cliente.Nome;
                CognomeCliente = Cliente.Cognome;
                Sesso = Cliente.Sesso;
                NatoIl = Cliente.DataNascita;
                CodiceFiscale = Cliente.CodiceFiscale;
                PartitaIVA = Cliente.PartitaIVA;
                NatoA.Citta = Cliente.NatoA.Citta;
                NatoA.Provincia = Cliente.NatoA.Provincia;
                ResidenteA.Citta = Cliente.ResidenteA.Citta;
                ResidenteA.Provincia = Cliente.ResidenteA.Provincia;
                ResidenteA.CAP = Cliente.ResidenteA.CAP;
                ResidenteA.ToponimoViaECivico = Cliente.ResidenteA.ToponimoViaECivico;
                Telefono = ""; // Me.Cliente.TelefonoPrincipale.Valore
                TipoFonteCliente = Cliente.TipoFonte;
                FonteCliente = Cliente.Fonte;
                Cellulare = "";
                Fax = "";
                // Me.m_Mail = Me.Cliente.eMail(0).Valore
                var impiego = Cliente.ImpiegoPrincipale;
                if (impiego is null && Cliente.ImpieghiValidi.Count > 0)
                    impiego = Cliente.ImpieghiValidi[0];
                if (impiego is object)
                {
                    Impiego.InitializeFrom(impiego);
                    // 'Me.Amministrazione = impiego.Azienda
                    // 'Me.EntePagante = impiego.EntePagante
                    // Me.DataAssunzione = impiego.DataAssunzione
                    // 'Me.DataLicenziamento = impiego.DataLicenziamento
                    // Me.Posizione = impiego.Posizione
                    // Me.StipendioLordo = impiego.StipendioLordo
                    // Me.StipendioNetto = impiego.StipendioNetto
                    // Me.TFR = impiego.TFR
                    // Me.PercTFRAzienda = impiego.PercTFRAzienda
                    // Me.TFRNomeFondo = impiego.NomeFPC
                    // Me.TipoRapporto = impiego.TipoRapporto
                    // Me.NumeroMensilita = impiego.MensilitaPercepite
                }
            }



            /// <summary>
        /// Stato Preventivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoPreventivo
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_PREVENTIVO);
                }
            }

            /// <summary>
        /// Stato Preventivo Accettato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoPreventivoAccettato
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO);
                }
            }

            /// <summary>
        /// Accede allo stato Contratto Stampato
        /// </summary>
        /// <returns></returns>
            public CStatoLavorazionePratica StatoContrattoStampato
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_CONTRATTO_STAMPATO);
                }
            }

            /// <summary>
        /// Accede allo stato Contratto Firmato
        /// </summary>
        /// <returns></returns>
            public CStatoLavorazionePratica StatoContrattoFirmato
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_CONTRATTO_FIRMATO);
                }
            }

            /// <summary>
        /// Pratica caricata
        /// </summary>
        /// <returns></returns>
            public CStatoLavorazionePratica StatoPraticaCaricata
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_PRATICA_CARICATA);
                }
            }

            /// <summary>
        /// Restituisce un oggetto che descrive lo stato Richiesta Delibera
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoRichiestaDelibera
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_RICHIESTADELIBERA); // Me.m_StatoRichDelibera
                }
            }

            /// <summary>
        /// Restituisce un oggetto che descrive lo stato Pronta per Liquidazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoProntaLiquidazione
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE); // Me.m_StatoProntaLiquidazione
                }
            }

            /// <summary>
        /// Restituisce un oggetto che descrive lo stato pratica deliberata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoDeliberata
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_DELIBERATA); // Me.m_StatoDeliberata
                }
            }

            /// <summary>
        /// Restituisce un oggetto che descrive lo stato pratica liquidata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoLiquidata
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_LIQUIDATA); // Return Me.m_StatoLiquidata
                }
            }

            /// <summary>
        /// Restituisce un oggetto che descrive lo stato pratica Archiviata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoArchiviata
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_ARCHIVIATA); // Return Me.m_StatoArchiviata
                }
            }

            /// <summary>
        /// Restituisce un oggetto che descrive lo stato pratica Annullata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoAnnullata
            {
                get
                {
                    return StatiDiLavorazione.GetItemByMacroStato(StatoPraticaEnum.STATO_ANNULLATA); // Return Me.m_StatoAnnullata
                }
            }

            /// <summary>
        /// Restituisce un oggetto che rappresenta lo stato attuale della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoPratica StatoAttuale
            {
                get
                {
                    if (StatoDiLavorazioneAttuale is null)
                        return null;
                    return StatoDiLavorazioneAttuale.StatoPratica;
                }
            }

            public int IDStatoAttuale
            {
                get
                {
                    return DBUtils.GetID(StatoAttuale); // Me.StatoDiLavorazioneAttuale.IDStatoPratica
                }
            }

            /// <summary>
        /// Restituisce un oggetto che rappresenta lo stato attuale della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoLavorazionePratica StatoDiLavorazioneAttuale
            {
                get
                {
                    return m_StatoDiLavorazioneAttuale;
                }
                // Set(value As CStatoLavorazionePratica)
                // Dim oldValue As CStatoLavorazionePratica = Me.m_StatoDiLavorazioneAttuale
                // If (oldValue Is value) Then Exit Property
                // Me.m_StatoDiLavorazioneAttuale = value
                // Me.m_IDStatoDiLavorazioneAttuale = GetID(value)
                // Me.StatoAttuale = value.StatoPratica
                // 'If (value.MacroStato.HasValue) Then Me.StatoPratica = value.MacroStato
                // Me.DoChanged("StatoAttuale", value, oldValue)
                // End Set
            }

            protected internal virtual void SetStatoDiLavorazioneAttuale(CStatoLavorazionePratica value)
            {
                m_StatoDiLavorazioneAttuale = value;
                // Me.m_IDStatoDiLavorazioneAttuale = GetID(value)
            }

            public int IDStatoDiLavorazioneAttuale
            {
                get
                {
                    return DBUtils.GetID(m_StatoDiLavorazioneAttuale);
                }
                // Friend Set(value As Integer)
                // Dim oldValue As Integer = Me.IDStatoDiLavorazioneAttuale
                // If (oldValue = value) Then Exit Property
                // Me.m_IDStatoDiLavorazioneAttuale = value
                // Me.m_StatoDiLavorazioneAttuale = Nothing
                // Me.DoChanged("IDStatoDiLavorazioneAttuale", value, oldValue)
                // End Set
            }

            // ''' <summary>
            // ''' Restituisce o imposta un valore che indica lo stato attuale della pratica
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property StatoPratica As StatoPraticaEnum
            // Get
            // Return Me.m_StatoPratica
            // End Get
            // Set(value As StatoPraticaEnum)
            // Dim oldValue As StatoPraticaEnum = Me.m_StatoPratica
            // If (oldValue = value) Then Exit Property
            // Me.m_StatoPratica = value
            // Me.DoChanged("StatoPratica", value, oldValue)
            // End Set
            // End Property

            public CStatoLavorazionePratica get_StatiPratica(StatoPraticaEnum stato)
            {
                switch (stato)
                {
                    case StatoPraticaEnum.STATO_PREVENTIVO:
                        {
                            return StatoPreventivo;
                        }

                    case StatoPraticaEnum.STATO_CONTRATTO_STAMPATO:
                        {
                            return StatoContrattoStampato;
                        }

                    case StatoPraticaEnum.STATO_CONTRATTO_FIRMATO:
                        {
                            return StatoContrattoFirmato;
                        }

                    case StatoPraticaEnum.STATO_PRATICA_CARICATA:
                        {
                            return StatoPraticaCaricata;
                        }

                    case StatoPraticaEnum.STATO_RICHIESTADELIBERA:
                        {
                            return StatoRichiestaDelibera;
                        }

                    case StatoPraticaEnum.STATO_DELIBERATA:
                        {
                            return StatoDeliberata;
                        }

                    case StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE:
                        {
                            return StatoProntaLiquidazione;
                        }

                    case StatoPraticaEnum.STATO_LIQUIDATA:
                        {
                            return StatoLiquidata;
                        }

                    case StatoPraticaEnum.STATO_ARCHIVIATA:
                        {
                            return StatoArchiviata;
                        }

                    case StatoPraticaEnum.STATO_ANNULLATA:
                        {
                            return StatoAnnullata;
                        }

                    default:
                        {
                            throw new NotSupportedException();
                            break;
                        }
                }
            }

            public CStatiLavorazionePraticaCollection StatiDiLavorazione
            {
                get
                {
                    lock (this)
                    {
                        if (m_StatiDiLavorazione is null)
                        {
                            m_StatiDiLavorazione = new CStatiLavorazionePraticaCollection(this);
                            int j = m_StatiDiLavorazione.IndexOf(m_StatiDiLavorazione.GetItemById(DBUtils.GetID(m_StatoDiLavorazioneAttuale)));
                            if (j >= 0)
                            {
                                m_StatiDiLavorazione[j] = m_StatoDiLavorazioneAttuale;
                            }
                            else
                            {
                                m_StatiDiLavorazione.Add(m_StatoDiLavorazioneAttuale);
                                m_StatiDiLavorazione.Sort();
                            }
                        }

                        return m_StatiDiLavorazione;
                    }
                }
            }

            protected internal virtual void SetStatiDiLavorazione(CStatiLavorazionePraticaCollection value)
            {
                m_StatiDiLavorazione = value;
            }

            public decimal? MontanteLordo
            {
                get
                {
                    return m_MontanteLordo;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("Il Montante Lordo deve essere non negativo");
                    var oldValue = m_MontanteLordo;
                    if (oldValue == value == true)
                        return;
                    m_MontanteLordo = value;
                    DoChanged("MontanteLordo", value, oldValue);
                }
            }


            // ''' <summary>
            // ''' Restituisce o imposta la percentuale del montante lordo pagata al collaboratore
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property ProvvigioniBroker As Nullable(Of Double)
            // Get
            // If (Me.m_ValoreProvvBroker.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
            // Return (Me.m_ValoreProvvBroker / Me.m_MontanteLordo) * 100
            // Else
            // Return Nothing
            // End If
            // End Get
            // Set(value As Nullable(Of Double))
            // If (value < 0 Or value > 100) Then Throw New ArgumentOutOfRangeException("ProvvigioniBroker")
            // Dim oldValue As Nullable(Of Double) = Me.ProvvigioniBroker
            // If (oldValue = value) Then Exit Property
            // Me.m_ValoreProvvBroker = value * Me.m_MontanteLordo / 100
            // Me.DoChanged("ProvvigioniBroker", value, oldValue)
            // End Set
            // End Property

            // ''' <summary>
            // ''' Restituisce o imposta il valore delle provvigioni pagate al collaboratore
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property ValoreProvvigioniBroker As Decimal?
            // Get
            // Return Me.m_ValoreProvvBroker
            // End Get
            // Set(value As Decimal?)
            // If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreProvvigioniBroker")
            // Dim oldValue As Decimal? = Me.m_ValoreProvvBroker
            // If (oldValue = value) Then Exit Property
            // Me.m_ValoreProvvBroker = value
            // Me.DoChanged("ValoreProvvigioniBroker", value, oldValue)
            // End Set
            // End Property

            /// <summary>
        /// Restituisce o imposta la percentuale massima della provvigione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? ProvvigioneMassima
            {
                get
                {
                    var baseML = CalcolaBaseML();
                    // If (Me.m_ValoreProvvMax.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
                    if (m_ValoreProvvMax.HasValue && baseML.HasValue && baseML.Value > 0m)
                    {
                        return (double?)(m_ValoreProvvMax.Value * 100m / baseML.Value);
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if ((value < 0 | value > 100) == true)
                        throw new ArgumentOutOfRangeException("ProvvigioneMassima");
                    var oldValue = ProvvigioneMassima;
                    if (oldValue == value == true)
                        return;
                    ValoreProvvigioneMassima = (decimal?)(value * (double?)m_MontanteLordo / 100);
                    DoChanged("ProvvigioneMassima", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione incassata dall'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreProvvigioneMassima
            {
                get
                {
                    return m_ValoreProvvMax;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("ValoreProvvigioneMassima");
                    double? oldValue = (double?)m_ValoreProvvMax;
                    if (oldValue == (double?)value == true)
                        return;
                    m_ValoreProvvMax = value;
                    DoChanged("ValoreProvvigioneMassima", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale del montante lordo incassata dall'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? Spread
            {
                get
                {
                    var baseML = CalcolaBaseML();
                    // If (Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo.Value > 0) Then
                    if (ValoreSpread.HasValue && baseML.HasValue && baseML.Value > 0m)
                    {
                        return (double?)(ValoreSpread.Value * 100m / baseML.Value);
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if ((value < 0 | value > 100) == true)
                        throw new ArgumentOutOfRangeException("Spread");
                    var oldValue = Spread;
                    if (oldValue == value == true)
                        return;
                    ValoreSpread = (decimal?)(value * (double?)m_MontanteLordo / 100);
                    DoChanged("Spread", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione incassata dall'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreSpread
            {
                get
                {
                    if (m_ValoreUpFront.HasValue && m_ValoreRunning.HasValue)
                    {
                        return m_ValoreUpFront.Value + m_ValoreRunning.Value;
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("ValoreSpread");
                    double? oldValue = (double?)ValoreSpread;
                    if (oldValue == (double?)value == true)
                        return;
                    m_ValoreUpFront = value;
                    m_ValoreRunning = 0;
                    DoChanged("ValoreSpread", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale del montante lordo pagato alla filiale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? UpFront
            {
                get
                {
                    var baseML = CalcolaBaseML();
                    // If (Me.m_ValoreUpFront.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo > 0) Then
                    if (m_ValoreUpFront.HasValue && baseML.HasValue && baseML.Value > 0m)
                    {
                        return (double?)(m_ValoreUpFront * 100 / baseML.Value);
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if ((value < 0 | value > 100) == true)
                        throw new ArgumentOutOfRangeException("UpFront");
                    var oldValue = UpFront;
                    if (oldValue == value == true)
                        return;
                    m_ValoreUpFront = (decimal?)(value * (double?)m_MontanteLordo / 100);
                    DoChanged("UpFront", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ammontare della provvigione pagata alla filiale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreUpFront
            {
                get
                {
                    return m_ValoreUpFront;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("ValoreUpFront");
                    double? oldValue = (double?)m_ValoreUpFront;
                    if (oldValue == (double?)value == true)
                        return;
                    m_ValoreUpFront = value;
                    DoChanged("ValoreUpFront", value, oldValue);
                }
            }

            public double? Running
            {
                get
                {
                    var baseML = CalcolaBaseML();
                    // If (Me.m_ValoreRunning.HasValue AndAlso Me.m_MontanteLordo.HasValue AndAlso Me.m_MontanteLordo > 0) Then
                    if (m_ValoreRunning.HasValue && baseML.HasValue && baseML.Value > 0m)
                    {
                        return (double?)(m_ValoreRunning * 100 / baseML.Value);
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if ((value < 0 | value > 100) == true)
                        throw new ArgumentOutOfRangeException("Running");
                    var oldValue = UpFront;
                    if (oldValue == value == true)
                        return;
                    m_ValoreRunning = (decimal?)(value * (double?)m_MontanteLordo / 100);
                    DoChanged("Running", value, oldValue);
                }
            }

            public decimal? ValoreRunning
            {
                get
                {
                    return m_ValoreRunning;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("ValoreRunning");
                    double? oldValue = (double?)m_ValoreRunning;
                    if (oldValue == (double?)value == true)
                        return;
                    m_ValoreRunning = value;
                    DoChanged("ValoreRunning", value, oldValue);
                }
            }

            // Public ReadOnly Property ProvvigioneTotale As Nullable(Of Double)
            // Get
            // If (Me.Provvigionale.Totale.HasValue AndAlso Me.Spread.HasValue) Then
            // Return Me.Provvigionale.Totale + Me.Spread
            // Else
            // Return Nothing
            // End If
            // End Get
            // End Property

            public decimal? NettoRicavo
            {
                get
                {
                    return m_NettoRicavo;
                }

                set
                {
                    var oldValue = m_NettoRicavo;
                    if (oldValue == value == true)
                        return;
                    m_NettoRicavo = value;
                    DoChanged("NettoRicavo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la differenza tra il netto ricavo e la somma degli impegni da estinguere
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? NettoAllaMano
            {
                get
                {
                    var ret = NettoRicavo;
                    if (ret.HasValue == false)
                        return default;
                    var tot = TotaleDaEstinguere;
                    if (tot.HasValue == false)
                        return default;
                    return ret.Value - tot.Value;
                }
            }

            public decimal? TotaleDaEstinguere
            {
                get
                {
                    decimal? ret = (decimal?)0.0d;
                    foreach (EstinzioneXEstintore est in Estinzioni)
                    {
                        if (est.Selezionata && est.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            decimal? val = est.TotaleDaRimborsare;
                            if (val.HasValue == false)
                                return default;
                            ret = ret.Value + val.Value;
                        }
                    }

                    return ret;
                }
            }

            public float? get_Anzianita(DateTime al)
            {
                return DMD.DateUtils.CalcolaEta(Impiego.DataAssunzione, al);
            }

            public float? Anzianita
            {
                get
                {
                    return get_Anzianita(DMD.DateUtils.Now());
                }
            }

            public float? get_Eta(DateTime al)
            {
                return DMD.DateUtils.CalcolaEta(m_NatoIl, al);
            }

            public float? Eta
            {
                get
                {
                    return get_Eta(DMD.DateUtils.Now());
                }
            }

            public float? EtaFineFinanziamento
            {
                get
                {
                    DateTime? d = default;
                    if (m_DataDecorrenza.HasValue & NumeroRate.HasValue)
                    {
                        d = DMD.DateUtils.DateAdd(DateTimeInterval.Month, NumeroRate.Value, m_DataDecorrenza);
                        return get_Eta((DateTime)d);
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            /// <summary>
        /// Restituisce le informazioni sull'impiego al momento della crezione della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CImpiegato Impiego
            {
                get
                {
                    return m_Impiego;
                }
            }

            // Public Property Posizione As String
            // Get
            // Return Me.m_Posizione
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_Posizione
            // If (oldValue = value) Then Exit Property
            // Me.m_Posizione = value
            // Me.DoChanged("Posizione", value, oldValue)
            // End Set
            // End Property

            // Public Property StipendioLordo As Decimal?
            // Get
            // Return Me.m_StipendioLordo
            // End Get
            // Set(value As Decimal?)
            // Dim oldValue As Decimal? = Me.m_StipendioLordo
            // If (oldValue = value) Then Exit Property
            // Me.m_StipendioLordo = value
            // Me.DoChanged("StipendioLordo", value, oldValue)
            // End Set
            // End Property

            public decimal? Quinto
            {
                get
                {
                    if (Impiego.StipendioNetto.HasValue)
                        return Impiego.StipendioNetto.Value / 5m;
                    return default;
                }
            }


            // Public Property StipendioNetto As Decimal?
            // Get
            // Return Me.m_StipendioNetto
            // End Get
            // Set(value As Decimal?)
            // Dim oldValue As Decimal? = Me.m_StipendioNetto
            // If (oldValue = value) Then Exit Property
            // Me.m_StipendioNetto = value
            // Me.DoChanged("StipendioNetto", value, oldValue)
            // End Set
            // End Property

            public override string ToString()
            {
                return NumeroPratica;
            }

            // Public Property TFR As Decimal?
            // Get
            // Return Me.m_TFR
            // End Get
            // Set(value As Decimal?)
            // Dim oldValue As Decimal? = Me.m_TFR
            // If (oldValue = value) Then Exit Property
            // Me.m_TFR = value
            // Me.DoChanged("TFR", value, oldValue)
            // End Set
            // End Property

            // Public Property PercTFRAzienda As Nullable(Of Double)
            // Get
            // Return Me.m_PercTFRAzienda
            // End Get
            // Set(value As Nullable(Of Double))
            // Dim oldValue As Nullable(Of Double) = Me.m_PercTFRAzienda
            // If (oldValue = value) Then Exit Property
            // Me.m_PercTFRAzienda = value
            // Me.DoChanged("PercTFRAzienda", value, oldValue)
            // End Set
            // End Property

            // Public Property TFRAzienda As Decimal?
            // Get
            // If Me.m_TFR.HasValue And Me.m_PercTFRAzienda.HasValue Then
            // Return Me.m_TFR.Value * Me.m_PercTFRAzienda.Value / 100
            // Else
            // Return Nothing
            // End If
            // End Get
            // Set(value As Decimal?)
            // Dim oldValue As Decimal? = Me.TFRAzienda
            // If (oldValue = value) Then Exit Property
            // If value.HasValue Then
            // If Me.m_TFR.HasValue Then
            // Me.m_PercTFRAzienda = value.Value * 100 / Me.m_TFR.Value
            // Else
            // Me.m_TFR = value
            // Me.m_PercTFRAzienda = 100
            // End If
            // Else
            // Me.m_PercTFRAzienda = Nothing
            // End If
            // Me.DoChanged("TFRAzienda", value, oldValue)
            // End Set
            // End Property

            // Public Property PercTFRFPC As Nullable(Of Double)
            // Get
            // Return 100 - Me.PercTFRAzienda
            // End Get
            // Set(value As Nullable(Of Double))
            // Me.PercTFRAzienda = 100 - value
            // End Set
            // End Property


            // Public Property TFRNomeFondo As String
            // Get
            // Return Me.m_TFRNomeFondo
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_TFRNomeFondo
            // If (oldValue = value) Then Exit Property
            // Me.m_TFRNomeFondo = Trim(value)
            // Me.DoChanged("TFRNomeFondo", value, oldValue)
            // End Set
            // End Property

            // ''' <summary>
            // ''' Restituisce o imposta una stringa che descrive il tipo di impiego del cliente
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property TipoRapporto As String
            // Get
            // Return Me.m_TipoRapporto
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_TipoRapporto
            // If (oldValue = value) Then Exit Property
            // Me.m_TipoRapporto = value
            // Me.DoChanged("TipoRapporto", value, oldValue)
            // End Set
            // End Property

            // Public Property NumeroMensilita As Integer?
            // Get
            // Return Me.m_NumeroMensilita
            // End Get
            // Set(value As Integer?)
            // Dim oldValue As Integer? = Me.m_NumeroMensilita
            // If (oldValue = value) Then Exit Property
            // Me.m_NumeroMensilita = value
            // Me.DoChanged("NumeroMensilita", value, oldValue)
            // End Set
            // End Property


            /// <summary>
        /// Restituisce un valore che indica se la pratica è stato trasferita presso un'altra azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Trasferita
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, PraticaFlags.TRASFERITA);
                }

                set
                {
                    if (Trasferita == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, PraticaFlags.TRASFERITA, value);
                    DoChanged("Trasferita", value, !value);
                }
            }

            public override string GetTableName()
            {
                return "tbl_Pratiche";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override bool IsChanged()
            {
                bool ret = base.IsChanged() || StatoDiLavorazioneAttuale.IsChanged() || m_NatoA.IsChanged() || m_ResidenteA.IsChanged() || m_Impiego.IsChanged() || Provvigionale.IsChanged();
                if (m_StatiDiLavorazione is object)
                    ret = ret || DBUtils.IsChanged(m_StatiDiLavorazione);
                if (ret == false && m_Cliente is object)
                    ret = m_Cliente.IsChanged();
                // If (ret = False AndAlso Me.m_Documentazione IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Documentazione)
                if (ret == false && m_Info is object)
                    ret = DBUtils.IsChanged(m_Info);
                return ret;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool isNew = DBUtils.GetID(this) == 0;
                bool ret;
                // If (Me.m_Cliente IsNot Nothing) Then
                // Me.Cliente.Save()
                // Me.m_ClienteID = DBUtils.GetID(Me.Cliente, 0)
                // End If
                ret = base.SaveToDatabase(dbConn, force);
                if (m_Info is object && ret)
                    m_Info.Save(force);
                if (m_StatoDiLavorazioneAttuale is object)
                    m_StatoDiLavorazioneAttuale.Save(force || isNew);
                if (m_StatiDiLavorazione is object && ret)
                    m_StatiDiLavorazione.Save(force);
                if (ret)
                {
                    if (m_StatoDiLavorazioneAttuale is object)
                        m_StatoDiLavorazioneAttuale.SetChanged(false);
                    m_NatoA.SetChanged(false);
                    // Me.m_StatoPraticaOld = Me.m_StatoPratica
                    m_ResidenteA.SetChanged(false);
                    m_Impiego.SetChanged(false);
                    if (m_StatiDiLavorazione is object)
                        DBUtils.SetChanged(m_StatiDiLavorazione, false);
                    m_Provvigionale.SetChanged(false);
                }

                if (m_IDConsulenzaOld != IDConsulenza)
                {
                    StudiDiFattibilita.AggiornaPratiche(m_IDConsulenzaOld);
                    StudiDiFattibilita.AggiornaPratiche(IDConsulenza);
                }
                else
                {
                    StudiDiFattibilita.AggiornaPratiche(IDConsulenza);
                }

                m_IDConsulenzaOld = IDConsulenza;
                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                // Me.m_MacroStato = reader.Read("StatoPratica", Me.m_MacroStato)
                m_DataDecorrenza = reader.Read("DataDecorrenza",  m_DataDecorrenza);
                m_NomeCessionario = reader.Read("NomeCessionario",  m_NomeCessionario);
                m_CessionarioID = reader.Read("Cessionario",  m_CessionarioID);
                m_ProfiloID = reader.Read("Profilo",  m_ProfiloID);
                m_NomeProfilo = reader.Read("NomeProfilo",  m_NomeProfilo);
                m_ProdottoID = reader.Read("Prodotto",  m_ProdottoID);
                m_NomeProdotto = reader.Read("CQS_PD",  m_NomeProdotto);
                m_NumeroRate = reader.Read("NumeroRate",  m_NumeroRate);
                m_MontanteLordo = reader.Read("MontanteLordo",  m_MontanteLordo);
                m_NettoRicavo = reader.Read("NettoRicavo",  m_NettoRicavo);
                m_ValoreProvvMax = reader.Read("ProvvMax",  m_ValoreProvvMax);
                m_ValoreRunning = reader.Read("Running",  m_ValoreRunning);
                m_ValoreUpFront = reader.Read("UpFront",  m_ValoreUpFront);
                m_ValoreRappel = reader.Read("Rappel",  m_ValoreRappel);
                {
                    var withBlock = Provvigionale;
                    withBlock.Tipo = reader.Read("ProvvBrokerSu", TipoCalcoloProvvigionale.SOLOBASE);
                    withBlock.ValoreBase = reader.Read("ProvvBroker", withBlock.ValoreBase);
                    withBlock.ValorePercentuale = reader.Read("ProvvBrokerPerc", withBlock.ValorePercentuale);
                    withBlock.SetChanged(false);
                }

                m_PremioDaCessionario = reader.Read("PremioDaCessionario", m_PremioDaCessionario);
                m_Flags = reader.Read("Flags", m_Flags);
                int idSTL = 0;
                idSTL = reader.Read("IDStatoDiLavorazioneAttuale", idSTL);
                DBUtils.SetID(m_StatoDiLavorazioneAttuale, idSTL);
                {
                    var withBlock1 = m_StatoDiLavorazioneAttuale;
                    withBlock1.MacroStato = reader.Read("StatoPratica", withBlock1.MacroStato);
                    withBlock1.IDPratica = DBUtils.GetID(this);
                    withBlock1.Data = reader.Read("STL_Data", withBlock1.Data);
                    withBlock1.IDOperatore = reader.Read("STL_IDOP", withBlock1.IDOperatore);
                    withBlock1.NomeOperatore = reader.Read("STL_NMOP", withBlock1.NomeOperatore);
                    withBlock1.Note = reader.Read("STL_NOTE", withBlock1.Note);
                    withBlock1.Params = reader.Read("STL_PARS", withBlock1.Params);
                    withBlock1.Forzato = reader.Read("STL_FLAGS", withBlock1.Forzato);
                    withBlock1.IDOfferta = reader.Read("IDOffertaCorrente", withBlock1.IDOfferta);
                    withBlock1.IDFromStato = reader.Read("STL_FROMS", withBlock1.IDFromStato);
                    withBlock1.IDStatoPratica = reader.Read("IDStatoAttuale", withBlock1.IDStatoPratica);
                    withBlock1.DescrizioneStato = reader.Read("STL_DESCST", withBlock1.DescrizioneStato);
                    withBlock1.IDRegolaApplicata = reader.Read("STL_RULE", withBlock1.IDRegolaApplicata);
                    withBlock1.SetChanged(false);
                }

                m_TipoFonteContatto = reader.Read("TipoFonteContatto", m_TipoFonteContatto);
                m_IDFonte = reader.Read("IDFonte", m_IDFonte);
                m_NomeFonte = reader.Read("FonteContatto", m_NomeFonte);
                m_TipoFonteCliente = reader.Read("TipoFonteCliente", m_TipoFonteCliente);
                m_IDFonteCliente = reader.Read("IDFonteCliente", m_IDFonteCliente);
                m_IDCanale = reader.Read("IDCanale", m_IDCanale);
                m_NomeCanale = reader.Read("NomeCanale", m_NomeCanale);
                m_IDCanale1 = reader.Read("IDCanale1", m_IDCanale1);
                m_NomeCanale1 = reader.Read("NomeCanale1", m_NomeCanale1);
                m_ClienteID = reader.Read("Cliente", m_ClienteID);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                m_CognomeCliente = reader.Read("CognomeCliente", m_CognomeCliente);
                m_NatoA.Citta = reader.Read("NatoAComune", m_NatoA.Citta);
                m_NatoA.Provincia = reader.Read("NatoAProvincia", m_NatoA.Provincia);
                m_NatoA.SetChanged(false);
                m_NatoIl = reader.Read("NatoIl", m_NatoIl);
                m_ResidenteA.Citta = reader.Read("ResidenteAComune", m_ResidenteA.Citta);
                m_ResidenteA.Provincia = reader.Read("ResidenteAProvincia", m_ResidenteA.Provincia);
                m_ResidenteA.CAP = reader.Read("ResidenteACAP", m_ResidenteA.CAP);
                m_ResidenteA.ToponimoViaECivico = reader.Read("ResidenteAVia", m_ResidenteA.ToponimoViaECivico);
                m_ResidenteA.SetChanged(false);
                m_CodiceFiscale = reader.Read("CodiceFiscale", m_CodiceFiscale);
                m_Sesso = reader.Read("Sesso", m_Sesso);
                m_Telefono = reader.Read("Telefono", m_Telefono);
                m_Cellulare = reader.Read("Cellulare", m_Cellulare);
                m_Fax = reader.Read("Fax", m_Fax);
                m_eMail = reader.Read("eMail", m_eMail);
                m_Impiego.IDEntePagante = reader.Read("IDEntePagante", m_Impiego.IDEntePagante);
                m_Impiego.IDAzienda = reader.Read("IDAmministrazione", m_Impiego.IDAzienda);
                m_Impiego.NomeAzienda = reader.Read("Ente", m_Impiego.NomeAzienda);
                m_Impiego.DataAssunzione = reader.Read("DataAssunzione", m_Impiego.DataAssunzione);
                m_Impiego.Posizione = reader.Read("Posizione", m_Impiego.Posizione);
                m_Impiego.StipendioLordo = reader.Read("StipendioLordo", m_Impiego.StipendioLordo);
                m_Impiego.StipendioNetto = reader.Read("StipendioNetto", m_Impiego.StipendioNetto);
                m_Impiego.TFR = reader.Read("TFR", m_Impiego.TFR);
                m_Impiego.PercTFRAzienda = reader.Read("PercTFRAzienda", m_Impiego.PercTFRAzienda);
                m_Impiego.NomeFPC = reader.Read("TFRNomeFondo", m_Impiego.NomeFPC);
                m_Impiego.TipoRapporto = reader.Read("TipoImpiego", m_Impiego.TipoRapporto);
                m_Impiego.MensilitaPercepite = reader.Read("NumeroMensilita", m_Impiego.MensilitaPercepite);
                m_Impiego.SetChanged(false);

                // -------------------------------
                m_PartitaIVA = reader.Read("PartitaIVA", m_PartitaIVA);
                m_NumeroEsterno = reader.Read("StatRichD_Params", m_NumeroEsterno);
                m_IDConsulente = reader.Read("IDConsulente", m_IDConsulente);
                m_IDConsulenza = reader.Read("IDConsulenza", m_IDConsulenza);
                // Me.m_IDConsulenzaOld = Me.m_IDConsulenza
                m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaFinanziamento", m_IDRichiestaDiFinanziamento);
                m_IDAzienda = reader.Read("IDAzienda", m_IDAzienda);
                m_IDContesto = reader.Read("IDContesto", m_IDContesto);
                m_TipoContesto = reader.Read("TipoContesto", m_TipoContesto);
                m_Durata = reader.Read("Durata", m_Durata);
                m_IDRichiestaApprovazione = reader.Read("IDRichiestaApprovazione", m_IDRichiestaApprovazione);
                try
                {
                    string attributiString = "";
                    attributiString = reader.Read("Attributi", attributiString);
                    if (!string.IsNullOrEmpty(attributiString))
                    {
                        m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(attributiString);
                    }
                    else
                    {
                        m_Attributi = new CKeyCollection();
                    }
                }
                catch (Exception ex)
                {
                    m_Attributi = new CKeyCollection();
                }

                m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione",  m_IDFinestraLavorazione);
                m_IDTabellaFinanziaria = reader.Read("IDTabellaFinanziaria",  m_IDTabellaFinanziaria);
                m_IDTabellaVita = reader.Read("IDTabellaVita",  m_IDTabellaVita);
                m_IDTabellaImpiego = reader.Read("IDTabellaImpiego",  m_IDTabellaImpiego);
                m_IDTabellaCredito = reader.Read("IDTabellaCredito",  m_IDTabellaCredito);
                m_IDUltimaVerifica = reader.Read("IDUltimaVerifica",  m_IDUltimaVerifica);
                m_DataValuta = reader.Read("DataValuta",  m_DataValuta);
                m_DataStampaSecci = reader.Read("DataStampaSecci",  m_DataStampaSecci);
                m_CapitaleFinanziato = reader.Read("CapitaleFinanziato",  m_CapitaleFinanziato);
                m_IDCollaboratore = reader.Read("IDCollaboratore",  m_IDCollaboratore);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataDecorrenza", m_DataDecorrenza);
                writer.Write("Flags", m_Flags);
                writer.Write("IDStatoDiLavorazioneAttuale", DBUtils.GetID(StatoDiLavorazioneAttuale));
                if (StatoDiLavorazioneAttuale is null)
                {
                    {
                        var withBlock = StatoDiLavorazioneAttuale;
                        writer.Write("StatoPratica", DBNull.Value);
                        writer.Write("STL_Data", DBNull.Value);
                        writer.Write("STL_IDOP", 0);
                        writer.Write("STL_NMOP", "");
                        writer.Write("STL_NOTE", "");
                        writer.Write("STL_PARS", "");
                        writer.Write("STL_FLAGS", DBNull.Value);
                        writer.Write("IDOffertaCorrente", 0);
                        writer.Write("STL_FROMS", 0);
                        writer.Write("IDStatoAttuale", DBUtils.GetID(StatoAttuale));
                        writer.Write("STL_DESCST", "");
                        writer.Write("STL_RULE", 0);
                    }
                }
                else
                {
                    {
                        var withBlock1 = StatoDiLavorazioneAttuale;
                        writer.Write("StatoPratica", withBlock1.MacroStato);
                        writer.Write("STL_Data", withBlock1.Data);
                        writer.Write("STL_IDOP", withBlock1.IDOperatore);
                        writer.Write("STL_NMOP", withBlock1.NomeOperatore);
                        writer.Write("STL_NOTE", withBlock1.Note);
                        writer.Write("STL_PARS", withBlock1.Params);
                        writer.Write("STL_FLAGS", withBlock1.Forzato);
                        writer.Write("IDOffertaCorrente", withBlock1.IDOfferta);
                        writer.Write("STL_FROMS", withBlock1.IDFromStato);
                        writer.Write("IDStatoAttuale", withBlock1.IDStatoPratica);
                        writer.Write("STL_DESCST", withBlock1.DescrizioneStato);
                        writer.Write("STL_RULE", withBlock1.IDRegolaApplicata);
                    }
                }

                if (StatoAttuale is object)
                {
                    writer.Write("StatoPratica", StatoAttuale.MacroStato);
                    int ordine = StatiPratica.GetSequenzaStandard().IndexOf(StatoAttuale);
                    writer.Write("Ordine", ordine);
                }
                else
                {
                    writer.Write("StatoPratica", DBNull.Value);
                    writer.Write("Ordine", -1);
                }

                writer.Write("PremioDaCessionario", m_PremioDaCessionario);
                writer.Write("TipoFonteContatto", m_TipoFonteContatto);
                writer.Write("IDFonte", IDFonte);
                writer.Write("FonteContatto", m_NomeFonte);
                writer.Write("TipoFonteCliente", m_TipoFonteCliente);
                writer.Write("IDFonteCliente", IDFonteCliente);
                writer.Write("IDCanale", IDCanale);
                writer.Write("NomeCanale", m_NomeCanale);
                writer.Write("IDCanale1", IDCanale1);
                writer.Write("NomeCanale1", m_NomeCanale1);

                // writer.Write("NomeCessionario", Me.m_NomeCessionario)
                writer.Write("Cessionario", IDCessionario);
                writer.Write("Profilo", IDProfilo);
                writer.Write("NomeProfilo", m_NomeProfilo);
                writer.Write("Prodotto", IDProdotto);
                writer.Write("CQS_PD", m_NomeProdotto);
                writer.Write("NumeroRate", m_NumeroRate);
                writer.Write("MontanteLordo", m_MontanteLordo);
                writer.Write("NettoRicavo", m_NettoRicavo);
                writer.Write("ProvvMax", m_ValoreProvvMax);
                writer.Write("Running", m_ValoreRunning);
                writer.Write("UpFront", m_ValoreUpFront);
                writer.Write("Rappel", m_ValoreRappel);
                writer.Write("Cliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("CognomeCliente", m_CognomeCliente);
                writer.Write("NatoAComune", m_NatoA.Citta);
                writer.Write("NatoAProvincia", m_NatoA.Provincia);
                writer.Write("NatoIl", m_NatoIl);
                writer.Write("ResidenteAComune", m_ResidenteA.Citta);
                writer.Write("ResidenteAProvincia", m_ResidenteA.Provincia);
                writer.Write("ResidenteACAP", m_ResidenteA.CAP);
                writer.Write("ResidenteAVia", m_ResidenteA.ToponimoViaECivico);
                writer.Write("CodiceFiscale", m_CodiceFiscale);
                writer.Write("Sesso", m_Sesso);
                writer.Write("Telefono", m_Telefono);
                writer.Write("Cellulare", m_Cellulare);
                writer.Write("Fax", m_Fax);
                writer.Write("eMail", m_eMail);
                writer.Write("IDEntePagante", m_Impiego.IDEntePagante);
                writer.Write("IDAmministrazione", m_Impiego.IDAzienda);
                writer.Write("Ente", m_Impiego.NomeAzienda);
                writer.Write("DataAssunzione", m_Impiego.DataAssunzione);
                writer.Write("Posizione", m_Impiego.Posizione);
                writer.Write("StipendioLordo", m_Impiego.StipendioLordo);
                writer.Write("StipendioNetto", m_Impiego.StipendioNetto);
                writer.Write("TFR", m_Impiego.TFR);
                writer.Write("PercTFRAzienda", m_Impiego.PercTFRAzienda);
                writer.Write("TFRNomeFondo", m_Impiego.NomeFPC);
                writer.Write("TipoImpiego", m_Impiego.TipoRapporto);
                writer.Write("NumeroMensilita", m_Impiego.MensilitaPercepite);

                // -------------------------------
                writer.Write("PartitaIVA", m_PartitaIVA);
                writer.Write("StatRichD_Params", m_NumeroEsterno);
                writer.Write("IDConsulente", IDConsulente);
                writer.Write("IDConsulenza", IDConsulenza);
                writer.Write("IDRichiestaFinanziamento", IDRichiestaDiFinanziamento);
                writer.Write("IDAzienda", IDAzienda);
                writer.Write("IDContesto", IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("Durata", m_Durata);
                {
                    var withBlock2 = Provvigionale;
                    writer.Write("ProvvBrokerSu", withBlock2.Tipo);
                    writer.Write("ProvvBroker", withBlock2.ValoreBase);
                    writer.Write("ProvvBrokerPerc", withBlock2.ValorePercentuale);
                }

                writer.Write("IDRichiestaApprovazione", IDRichiestaApprovazione);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.Write("IDTabellaFinanziaria", IDTabellaFinanziaria);
                writer.Write("IDTabellaVita", IDTabellaVita);
                writer.Write("IDTabellaImpiego", IDTabellaImpiego);
                writer.Write("IDTabellaCredito", IDTabellaCredito);
                writer.Write("IDUltimaVerifica", IDUltimaVerifica);
                writer.Write("DataValuta", m_DataValuta);
                writer.Write("DataStampaSecci", m_DataStampaSecci);
                writer.Write("CapitaleFinanziato", m_CapitaleFinanziato);
                writer.Write("IDCollaboratore", IDCollaboratore);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ClienteID", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("CognomeCliente", m_CognomeCliente);
                writer.WriteAttribute("NatoIl", m_NatoIl);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                writer.WriteAttribute("PartitaIVA", m_PartitaIVA);
                writer.WriteAttribute("Sesso", m_Sesso);
                writer.WriteAttribute("Telefono", m_Telefono);
                writer.WriteAttribute("Cellulare", m_Cellulare);
                writer.WriteAttribute("Fax", m_Fax);
                writer.WriteAttribute("eMail", m_eMail);
                writer.WriteAttribute("CessionarioID", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("ProfiloID", IDProfilo);
                writer.WriteAttribute("NomeProfilo", m_NomeProfilo);
                writer.WriteAttribute("ProdottoID", IDProdotto);
                writer.WriteAttribute("NomeProdotto", m_NomeProdotto);
                writer.WriteAttribute("MontanteLordo", m_MontanteLordo);
                writer.WriteAttribute("NettoRicavo", m_NettoRicavo);
                writer.WriteAttribute("NumeroRate", m_NumeroRate);
                writer.WriteAttribute("ProvvMax", m_ValoreProvvMax);
                writer.WriteAttribute("Running", m_ValoreRunning);
                writer.WriteAttribute("UpFront", m_ValoreUpFront);
                writer.WriteAttribute("Rappel", m_ValoreRappel);
                writer.WriteAttribute("PremioDaCessionario", m_PremioDaCessionario);
                writer.WriteAttribute("DataDecorrenza", m_DataDecorrenza);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                // writer.WriteTag("StatoPratica", Me.m_StatoPratica)
                // writer.WriteTag("StatoPraticaOld", Me.m_StatoPraticaOld)

                writer.WriteAttribute("TipoFonteContatto", m_TipoFonteContatto);
                writer.WriteAttribute("FonteContatto", m_NomeFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("IDCanale", IDCanale);
                writer.WriteAttribute("NomeCanale", m_NomeCanale);
                writer.WriteAttribute("IDCanale1", IDCanale1);
                writer.WriteAttribute("NomeCanale1", m_NomeCanale1);
                writer.WriteAttribute("NumeroPraticaEsterna", m_NumeroEsterno);
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("IDConsulenza", IDConsulenza);
                // writer.WriteAttribute("IDConsulenzaOld", Me.m_IDConsulenzaOld)
                writer.WriteAttribute("IDRichiestaFinanziamento", IDRichiestaDiFinanziamento);
                writer.WriteAttribute("IDAzienda", IDAzienda);
                writer.WriteAttribute("IDContesto", IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("IDRichiestaApprovazione", IDRichiestaApprovazione);
                writer.WriteAttribute("TipoFonteCliente", m_TipoFonteCliente);
                writer.WriteAttribute("IDFonteCliente", IDFonteCliente);
                writer.WriteAttribute("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.WriteAttribute("IDTabellaFinanziaria", IDTabellaFinanziaria);
                writer.WriteAttribute("IDTabellaVita", IDTabellaVita);
                writer.WriteAttribute("IDTabellaImpiego", IDTabellaImpiego);
                writer.WriteAttribute("IDTabellaCredito", IDTabellaCredito);
                writer.WriteAttribute("IDUltimaVerifica", IDUltimaVerifica);
                writer.WriteAttribute("DataValuta", m_DataValuta);
                writer.WriteAttribute("DataStampaSecci", m_DataStampaSecci);
                writer.WriteAttribute("CapitaleFinanziato", m_CapitaleFinanziato);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                base.XMLSerialize(writer);
                // writer.WriteTag("Info", Me.Info)
                writer.WriteTag("StatoDiLavorazioneAttuale", StatoDiLavorazioneAttuale);
                writer.WriteTag("NatoA", m_NatoA);
                writer.WriteTag("ResidenteA", m_ResidenteA);
                writer.WriteTag("Provvigionale", Provvigionale);
                writer.WriteTag("Impiego", m_Impiego);
                if (!writer.GetSetting("CPraticaCQSPD.fastXMLserialize", false))
                {
                    writer.WriteTag("StatiDiLavorazione", StatiDiLavorazione);
                    if (Prodotto is object)
                        Attributi.SetItemByKey("ColoreProdotto", Prodotto.Attributi.GetItemByKey("Colore"));
                    if (OffertaCorrente is object && OffertaCorrente.TabellaFinanziariaRel is object && OffertaCorrente.TabellaFinanziariaRel.Tabella is object)
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                        Attributi.SetItemByKey("ColoreTabellaFinanziaria", OffertaCorrente.TabellaFinanziariaRel.Tabella.Attributi.GetItemByKey("Colore"));
                        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    }
                }

                writer.WriteTag("Attributi", Attributi);
                // writer.WriteTag("OffertaCorrente", Me.OffertaCorrente)
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDFonteCliente":
                        {
                            m_IDFonteCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoFonteCliente":
                        {
                            m_TipoFonteCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ClienteID":
                        {
                            m_ClienteID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CognomeCliente":
                        {
                            m_CognomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NatoA":
                        {
                            m_NatoA = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "NatoIl":
                        {
                            m_NatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ResidenteA":
                        {
                            m_ResidenteA = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "CodiceFiscale":
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PartitaIVA":
                        {
                            m_PartitaIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Sesso":
                        {
                            m_Sesso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Telefono":
                        {
                            m_Telefono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Cellulare":
                        {
                            m_Cellulare = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Fax":
                        {
                            m_Fax = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "eMail":
                        {
                            m_eMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Impiego":
                        {
                            m_Impiego = (Anagrafica.CImpiegato)fieldValue;
                            break;
                        }

                    case "CessionarioID":
                        {
                            m_CessionarioID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ProfiloID":
                        {
                            m_ProfiloID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProfilo":
                        {
                            m_NomeProfilo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ProdottoID":
                        {
                            m_ProdottoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProdotto":
                        {
                            m_NomeProdotto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MontanteLordo":
                        {
                            m_MontanteLordo = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoRicavo":
                        {
                            m_NettoRicavo = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NumeroRate":
                        {
                            m_NumeroRate = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ProvvMax":
                        {
                            m_ValoreProvvMax = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Provvigionale":
                        {
                            m_Provvigionale = (CProvvigionale)fieldValue;
                            break;
                        }

                    case "Running":
                        {
                            m_ValoreRunning = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "UpFront":
                        {
                            m_ValoreUpFront = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rappel":
                        {
                            m_ValoreRappel = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PremioDaCessionario":
                        {
                            m_PremioDaCessionario = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataDecorrenza":
                        {
                            m_DataDecorrenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (PraticaFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }


                    // Case "StatoPratica" : Me.m_StatoPratica = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    // Case "StatoPraticaOld" : Me.m_StatoPraticaOld = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)

                    case "TipoFonteContatto":
                        {
                            m_TipoFonteContatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FonteContatto":
                        {
                            m_NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            m_IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "NumeroPraticaEsterna":
                        {
                            m_NumeroEsterno = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // Case "StatoAttuale" : Me.m_StatoAttuale = fieldValue
                    case "StatiDiLavorazione":
                        {
                            m_StatiDiLavorazione = (CStatiLavorazionePraticaCollection)fieldValue;
                            if (m_StatiDiLavorazione is object)
                                m_StatiDiLavorazione.SetPratica(this);
                            break;
                        }

                    case "StatoDiLavorazioneAttuale":
                        {
                            m_StatoDiLavorazioneAttuale = (CStatoLavorazionePratica)fieldValue; // : Me.StatoDiLavorazioneAttuale.SetPratica(Me)
                                                                                                // DBUtils.SetID(Me.m_StatoDiLavorazioneAttuale, GetID(fieldValue))
                            if (m_StatoDiLavorazioneAttuale is object)
                                m_StatoDiLavorazioneAttuale.SetPratica(this);
                            break;
                        }

                    case "IDConsulente":
                        {
                            m_IDConsulente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDConsulenza":
                        {
                            m_IDConsulenza = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Case "IDConsulenzaOld" : Me.m_IDConsulenzaOld = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    case "IDRichiestaFinanziamento":
                        {
                            m_IDRichiestaDiFinanziamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                            m_Durata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRichiestaApprovazione":
                        {
                            m_IDRichiestaApprovazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "IDFinestraLavorazione":
                        {
                            m_IDFinestraLavorazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaFinanziaria":
                        {
                            m_IDTabellaFinanziaria = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaVita":
                        {
                            m_IDTabellaVita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaImpiego":
                        {
                            m_IDTabellaImpiego = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaCredito":
                        {
                            m_IDTabellaCredito = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUltimaVerifica":
                        {
                            m_IDUltimaVerifica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataValuta":
                        {
                            m_DataValuta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataStampaSecci":
                        {
                            m_DataStampaSecci = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Info":
                        {
                            m_Info = (CInfoPratica)fieldValue;
                            break;
                        }

                    case "CapitaleFinanziato":
                        {
                            m_CapitaleFinanziato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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

            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
                m_TipoFonteContatto = "";
                m_NomeFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeProdotto = "";
                m_MontanteLordo = default;
                m_NettoRicavo = default;
                m_Cessionario = null;
                m_CessionarioID = 0;
                m_NomeCessionario = "";
                m_Profilo = null;
                m_ProfiloID = 0;
                m_NomeProfilo = "";
                m_ValoreProvvMax = default;
                m_Provvigionale = new CProvvigionale();
                m_ValoreRunning = default;
                m_ValoreUpFront = default;
                m_ValoreRappel = default;
                m_DataDecorrenza = default;
                m_Canale = null;
                m_IDCanale = 0;
                m_NomeCanale = "";
                m_Canale1 = null;
                m_IDCanale1 = 0;
                m_NomeCanale1 = "";
                m_ProdottoID = 0;
                m_Prodotto = null;
                m_NomeProdotto = "";
                m_NumeroRate = default;
                m_NatoA = new Anagrafica.CIndirizzo();
                m_NatoA.InitializeFrom(((CPraticaCQSPD)value).m_NatoA);
                m_ResidenteA.InitializeFrom(((CPraticaCQSPD)value).m_ResidenteA);
                m_Impiego.InitializeFrom(((CPraticaCQSPD)value).m_Impiego);
                m_Info = null;
                m_NumeroEsterno = DMD.Strings.vbNullString;
                // Me.StatoDiLavorazioneAttuale.Ini = 0
                m_StatoDiLavorazioneAttuale = null;
                m_IDConsulente = 0;
                m_Consulente = null;
                m_IDConsulenza = 0;
                m_Consulenza = null;
                m_IDRichiestaDiFinanziamento = 0;
                m_RichiestaDiFinanziamento = null;
                m_DataValuta = default;
                m_DataStampaSecci = default;
                m_CapitaleFinanziato = default;
                {
                    var withBlock = (CPraticaCQSPD)value;
                    foreach (string k in withBlock.Attributi.Keys)
                        Attributi.SetItemByKey(k, withBlock.Attributi[k]);
                }
            }

            public void Correggi(COffertaCQS offerta)
            {
                if (offerta is null)
                    throw new ArgumentNullException("offerta");
                if (Info.Correzione is null)
                {
                    var corr = new COffertaCQS();
                    corr.InitializeFrom(OffertaCorrente);
                    corr.Stato = ObjectStatus.OBJECT_VALID;
                    corr.Save();
                    Info.Correzione = corr;
                }

                OffertaCorrente = offerta;
                Save();
                OnCorretta(new ItemEventArgs(this));
            }

            protected virtual void OnCorretta(ItemEventArgs e)
            {
                Corretta?.Invoke(this, e);
                Pratiche.DoOnCorretta(new ItemEventArgs(this));
                GetModule().DispatchEvent(new Sistema.EventDescription("corretta", Sistema.Users.CurrentUser.Nominativo + " ha apportato una correzione alla pratica N°" + NumeroPratica, this));
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                Pratiche.DoOnCreate(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                var ra = RichiestaApprovazione;
                if (ra is object)
                {
                    switch (ra.StatoRichiesta)
                    {
                        case StatoRichiestaApprovazione.ATTESA:
                        case StatoRichiestaApprovazione.NONCHIESTA:
                        case StatoRichiestaApprovazione.PRESAINCARICO:
                            {
                                ra.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA;
                                ra.MotivoConferma = "Pratica Eliminata";
                                ra.DettaglioConferma = DMD.Strings.Combine(ra.DettaglioConferma, ra.MotivoConferma + DMD.Strings.vbNewLine + "Operatore: " + Sistema.Users.CurrentUser.Nominativo, DMD.Strings.vbNewLine);
                                ra.Save();
                                break;
                            }
                    }
                }

                RilasciaAltriPrestiti();
                base.OnDelete(e);
                Pratiche.DoOnDelete(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                Pratiche.DoOnModified(new ItemEventArgs(this));
                GetModule().DispatchEvent(new Sistema.EventDescription("Edit", "Modificata la pratica N°" + NumeroPratica, this));
            }

            protected virtual void OnRifiutata(ItemEventArgs e)
            {
                Rifiutata?.Invoke(this, e);
            }

            protected virtual void OnApproved(ItemEventArgs e)
            {
                Approvata?.Invoke(this, e);
            }

            protected virtual void OnChangeStatus(ItemEventArgs e)
            {
                StatusChanged?.Invoke(this, e);
                Pratiche.DoOnChangeStatus(e);
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
            }

            private int GetCurrentStato()
            {
                return Sistema.Formats.ToInteger(GetConnection().ExecuteScalar("SELECT [IDStatoDiLavorazioneAttuale] FROM " + GetTableName() + " WHERE [ID]=" + DBUtils.DBNumber(DBUtils.GetID(this))));
            }

            public CStatoLavorazionePratica ChangeStatus(CStatoPratica toStato, CStatoPratRule regola, DateTime data, string param, string notes, Sistema.CUser operatore)
            {
                if (OffertaCorrente is null)
                    throw new ArgumentNullException("Offerta Correte");
                if (regola is object)
                {
                    var ra = RichiestaApprovazione;
                    if (ra is object && ra.StatoRichiesta != StatoRichiestaApprovazione.APPROVATA)
                    {
                        if (ra.MotivoRichiesta is object && ra.MotivoRichiesta.SoloSegnalazione)
                        {
                        }
                        else if (toStato.MacroStato.HasValue == false || toStato.MacroStato.Value != StatoPraticaEnum.STATO_ANNULLATA)
                            throw new ArgumentException("L'offerta deve essere approvata");
                    }
                }

                // Verifichiamo che la pratica sia ancora nello stato corrente per evitare doppi passaggi di stato
                if (DBUtils.GetID(StatoDiLavorazioneAttuale) != GetCurrentStato())
                {
                    throw new InvalidOperationException("Lo stato della pratica è stato modificato");
                }

                CStatoLavorazionePratica statoLav;
                statoLav = new CStatoLavorazionePratica();
                statoLav.Data = data;
                statoLav.Operatore = operatore;
                statoLav.Pratica = this;
                statoLav.IDOfferta = IDOffertaCorrente;
                statoLav.FromStato = StatoDiLavorazioneAttuale;
                statoLav.StatoPratica = toStato;
                statoLav.RegolaApplicata = regola;
                statoLav.Forzato = regola is null;
                statoLav.Params = param;
                statoLav.Note = notes;
                statoLav.Stato = ObjectStatus.OBJECT_VALID;
                statoLav.MacroStato = toStato.MacroStato;
                statoLav.Save();
                StatiDiLavorazione.Add(statoLav);
                StatiDiLavorazione.Sort();
                m_StatoDiLavorazioneAttuale = statoLav;
                Save(true);
                if (StatoAttuale.MacroStato ==  StatoPraticaEnum.STATO_ANNULLATA  )
                {
                    var ra = RichiestaApprovazione;
                    if (ra is object)
                    {
                        switch (ra.StatoRichiesta)
                        {
                            case StatoRichiestaApprovazione.ATTESA:
                            case StatoRichiestaApprovazione.NONCHIESTA:
                            case StatoRichiestaApprovazione.PRESAINCARICO:
                                {
                                    ra.StatoRichiesta = StatoRichiestaApprovazione.ANNULLATA;
                                    if (regola is object)
                                    {
                                        if (Sistema.TestFlag(regola.Flags, FlagsRegolaStatoPratica.DaCliente))
                                        {
                                            ra.MotivoConferma = "Pratica rifiuntata dal cliente";
                                        }
                                        else if (Sistema.TestFlag(regola.Flags, FlagsRegolaStatoPratica.Bocciata))
                                        {
                                            ra.MotivoConferma = "Pratica bocciata dall'agenzia";
                                        }
                                        else if (Sistema.TestFlag(regola.Flags, FlagsRegolaStatoPratica.NonFattibile))
                                        {
                                            ra.MotivoConferma = "Pratica non fattibile";
                                        }
                                        else
                                        {
                                            ra.MotivoConferma = "Pratica Annullata";
                                        }
                                    }
                                    else
                                    {
                                        ra.MotivoConferma = "Pratica Annullata";
                                    }

                                    ra.DettaglioConferma = DMD.Strings.Combine(ra.DettaglioConferma, ra.MotivoConferma + DMD.Strings.vbNewLine + "Operatore: " + operatore.Nominativo + DMD.Strings.vbNewLine + notes, DMD.Strings.vbNewLine);
                                    ra.Save();
                                    break;
                                }
                        }
                    }
                }


                // Controlla se lo stato corrente prevede di acquisire o rilasciare le estinzioni
                if (StatoAttuale.AcquisisciEstinzioni)
                {
                    AcquisisciAltriPrestiti();
                }
                else if (StatoAttuale.RilasciaEstinzioni)
                {
                    RilasciaAltriPrestiti();
                }

                OnChangeStatus(new ItemEventArgs(this));
                return statoLav;
            }

            /// <summary>
        /// Questo metodo viene richiamato quando la pratica viene passata in uno stato che
        /// richiede l'acquisizione degli altri prestiti
        /// </summary>
        /// <remarks></remarks>
            protected virtual void AcquisisciAltriPrestiti()
            {
                var cursor = new CEstinzioniCursor();
                var nEst = new CEstinzione();
                cursor.IgnoreRights = true;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDPratica.Value = DBUtils.GetID(this);
                if (cursor.Item is object)
                {
                    nEst = cursor.Item;
                }
                else
                {
                    nEst = new CEstinzione();
                }

                cursor.Dispose();
                nEst.Stato = ObjectStatus.OBJECT_VALID;
                nEst.Persona = Cliente;
                nEst.Rata = OffertaCorrente.Rata;
                nEst.Durata = OffertaCorrente.Durata;
                nEst.DataInizio = OffertaCorrente.DataDecorrenza;
                nEst.Scadenza = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd(DateTimeInterval.Month, nEst.Durata.Value, nEst.DataInizio.Value));
                nEst.TAN = OffertaCorrente.TAN;
                nEst.TAEG = OffertaCorrente.TAEG;
                nEst.Pratica = this;
                nEst.Istituto = Cessionario;
                nEst.DettaglioStato = "Pratica interna N°" + NumeroPratica;
                nEst.SourceID = DBUtils.GetID(this);
                nEst.SourceType = minidom.Sistema.vbTypeName(this);
                nEst.PuntoOperativo = PuntoOperativo;
                nEst.Estinta = false;
                nEst.Numero = Sistema.IIF(!string.IsNullOrEmpty(NumeroEsterno), NumeroEsterno, NumeroPratica);
                nEst.NomeAgenzia = Anagrafica.Aziende.AziendaPrincipale.Nominativo;
                if (nEst.PuntoOperativo is null)
                {
                    nEst.NomeFiliale = DMD.Strings.UCase(Anagrafica.Aziende.AziendaPrincipale.Nominativo);
                }
                else
                {
                    nEst.NomeFiliale = DMD.Strings.UCase(Anagrafica.Aziende.AziendaPrincipale.Nominativo + " - " + nEst.PuntoOperativo.Nome);
                }

                if (Prodotto is object)
                {
                    switch (Prodotto.IdTipoContratto ?? "")
                    {
                        case "C":
                            {
                                if (Prodotto.IdTipoRapporto == "H")
                                {
                                    nEst.Tipo = TipoEstinzione.ESTINZIONE_CQP;
                                }
                                else
                                {
                                    nEst.Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO;
                                }

                                break;
                            }

                        case "D":
                            {
                                nEst.Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA;
                                break;
                            }
                    }
                }

                nEst.Save();
                var estinzioni = Finanziaria.Estinzioni.GetEstinzioniXEstintore(this);
                foreach (EstinzioneXEstintore e in estinzioni)
                {
                    if (e.Selezionata && e.Estinzione is object)
                    {
                        e.Estinzione.EstintoDa = nEst;
                        e.Estinzione.Estinta = true;
                        e.Estinzione.DataEstinzione = StatoDiLavorazioneAttuale.Data;
                        e.Estinzione.Save();
                    }
                }
            }

            /// <summary>
        /// Questo metodo viene richiamato quando la pratica viene eliminata oppure quando viene effettuato un passaggio
        /// ad uno stato che è programmato per rilasciare gli altri prestiti (ES. Annullata)
        /// </summary>
        /// <remarks></remarks>
            protected virtual void RilasciaAltriPrestiti()
            {
                CEstinzioniCursor cursor = null;
                try
                {
                    cursor = new CEstinzioniCursor();
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPratica.Value = DBUtils.GetID(this);
                    cursor.SourceID.Value = DBUtils.GetID(this);
                    cursor.SourceType.Value = DMD.RunTime.vbTypeName(this);
                    if (cursor.Item is object)
                    {
                        cursor.Item.DettaglioStato = "Pratica Interna N°" + NumeroPratica + " Annullata";
                        cursor.Item.Estinta = true;
                        cursor.Item.Stato = ObjectStatus.OBJECT_DELETED;
                        cursor.Item.Save();
                    }

                    var estinzioni = Finanziaria.Estinzioni.GetEstinzioniXEstintore(this);
                    foreach (EstinzioneXEstintore e in estinzioni)
                    {
                        if (e.Estinzione is object && e.Selezionata)
                        {
                            e.Estinzione.EstintoDa = null;
                            e.Estinzione.Estinta = false;
                            e.Estinzione.DataEstinzione = default;
                            e.Estinzione.Save();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }



            /// <summary>
        /// Restituisce il valore della provvigione totale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreProvvigioneTotale
            {
                get
                {
                    // If (Me.Provvigionale.ValoreTotale AndAlso Me.ValoreSpread.HasValue) Then Return Me.Provvigionale.ValoreTotale + Me.ValoreSpread.Value
                    var ret = m_ValoreUpFront;
                    if (ret.HasValue && m_ValoreRappel.HasValue)
                        ret = ret.Value + m_ValoreRappel.Value;
                    return ret;
                }
            }



            /// <summary>
        /// Genera un evento che indica che la pratica richiede attenzione
        /// </summary>
        /// <param name="msg"></param>
        /// <remarks></remarks>
            public void Watch(string msg)
            {
                Save();
                var e = new ItemEventArgs(this, msg);
                OnWatch(e);
            }

            protected virtual void OnWatch(ItemEventArgs e)
            {
                PraticaWatch?.Invoke(this, e);
                Pratiche.DoOnWatch(e);
                Pratiche.Module.DispatchEvent(new Sistema.EventDescription("watch", "Condizione di Attenzione", e));
            }

            public decimal? CalcolaBaseML()
            {
                if (OffertaCorrente is null)
                    return default;
                return OffertaCorrente.CalcolaBaseML(Estinzioni);
            }

            public double? CalcolaProvvTAN()
            {
                if (OffertaCorrente is null)
                    return default;
                return OffertaCorrente.CalcolaProvvTAN(Estinzioni);
            }

            public decimal? CalcolaProvvTANE()
            {
                if (OffertaCorrente is null)
                    return default;
                return OffertaCorrente.CalcolaProvvTANE(Estinzioni);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}