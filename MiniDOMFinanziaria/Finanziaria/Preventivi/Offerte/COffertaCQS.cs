using System;
using System.Diagnostics;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoOfferta : int
        {
            NON_ASSOCIATO = 0,
            PROPOSTA = 1,
            RIFIUTATA_CLIENTE = 2,
            ACCETTATA_CLIENTE = 3,
            RICHIESTA_APPROVAZIONE = 4,
            APPROVATA = 5,
            RIFIUTATA = 6
        }

        [Flags]
        public enum OffertaFlags : int
        {
            NOTSET = 0,

            /// <summary>
        /// Se vero indica che l'offerta è nascosta
        /// </summary>
        /// <remarks></remarks>
            HIDDEN = 1,

            /// <summary>
        /// Indica che la provvigione memorizzata è uno sconto sulla provvigione massima
        /// </summary>
        /// <remarks></remarks>
            PROVVIGIONE_SCONTO = 2,

            /// <summary>
        /// Se vero indica che si tratta di un cliente procacciato dal collaboratore
        /// </summary>
            DirettaCollaboratore = 4
        }

        // ----------------------------------------------------
        public enum ErrorCodes : int
        {
            ERROR_OK = 0,              // Nessun errore
            ERROR_TEGMAX = -1,         // Superato il TEG massimo
            ERROR_TAEGMAX = -2,        // Superato il TAEG massimo
            ERROR_PROVVMAX = -3,       // Superato il provvigionale massimo
            ERROR_INVALIDARGUMENT = -4, // Valore non valido
            ERROR_TABFINCONSTR = -5,   // Vincoli della tabella Finanziaria non rispettati
            ERROR_TABASSCONSTR = -6,   // Vincoli della tabella assicurativa non rispettati
            ERROR_ZERO = -7,           // Divisione per zero o zeri non trovati
            ERROR_GENERIC = -255      // Errore non riconosciuto
        }

        [Serializable]
        public class COffertaCQS : Databases.DBObjectPO, ICloneable
        {
            private bool m_OffertaLibera;
            [NonSerialized]
            private CPraticaCQSPD m_Pratica;
            private int m_IDPratica;
            private StatoOfferta m_StatoOfferta;
            [NonSerialized]
            private Anagrafica.CPersonaFisica m_Cliente;
            private int m_IDCliente;
            private string m_NomeCliente;
            private int m_PreventivoID; // [INT] ID del preventivo a cui è associata l'offerta
            [NonSerialized]
            private CPreventivo m_Preventivo;                // [CPreventivo] Oggetto preventivo a cui è associata l'offerta
            private int m_IDCessionario;
            private string m_NomeCessionario;
            [NonSerialized]
            private CCQSPDCessionarioClass m_Cessionario;
            private int m_IDProfilo;
            private string m_NomeProfilo;
            [NonSerialized]
            private CProfilo m_Profilo;
            [NonSerialized]
            private CCQSPDProdotto m_Prodotto; // [CProdotto] Oggetto prodotto    
            private int m_ProdottoID; // [INT] ID del prodotto
            private string m_NomeProdotto; // [TEXT] Nome del prodotto
            private bool m_Calculating;
            private bool m_Calcolato; // [BOOL] Se falso indica che occorre ricalcolare l'offerta a causa di qualche modifica
            private int m_Durata;
            private decimal m_Rata;
            private double m_Eta; // [Double] età del cliente calcolata alla decorrenza
            private double m_Anzianita; // [Double] anzianità lavorativa del cliente calcolata alla decorrenza
            private int m_TabellaAssicurativaRelID;   // [INT] ID della relazione tra prodotto e la tripla di tabelle assicurative
            private CProdottoXTabellaAss m_TabellaAssicurativaRel;   // [] Oggetto relazione prodotto - tripla di tabelle assicurative
            private string m_NomeTabellaAssicurativa; // [TEXT] Nome della tripla di tabelle assicurative 
            private int m_TabellaFinanziariaRelID;      // [INT] ID della relazione tra prodotto e tabella Finanziaria
            private CProdottoXTabellaFin m_TabellaFinanziariaRel;      // Oggetto relazione prodotto - tabella Finanziaria
            private string m_NomeTabellaFinanziaria; // [TEXT] Nome della tabella Finanziaria
            private int m_TabellaSpeseID;            // [INT] ID della tabella spese
            private CProdottoXTabellaSpesa m_TabellaSpese;  // [CSpesa] Oggetto tabella spese
            private decimal m_ValoreRiduzioneProvvigionale;         // Valore della riduzione del provvigionale per estinzioni
            private double m_ValoreRappel;                  // Valore nascosto
            private double m_ValoreProvvigioneMassima;      // Massima provvigione applicabile 
            private double m_ValoreSpreadBase;              // Spread base sul listino
            private double m_ValoreSpread;                  // Valore predefinito per il profilo
            private double m_ValoreUpFront;                 // Percentuale sulla provvigione caricata
            private double m_ValoreRunning;
            private CProvvigionale m_Provvigionale;
            private DateTime? m_DataNascita;
            private DateTime? m_DataAssunzione;
            private decimal m_PremioVita;  // [Double] Valore del premio vita  
            private decimal m_PremioImpiego; // [Double] Valore del premio impiego
            private decimal m_PremioCredito; // [Double] Valore del premio credito
            private decimal m_ImpostaSostitutiva; // [Double] Valore dell'imposta sostitutiva
            private decimal m_OneriErariali; // [Double] Valore degli oneri erariali
            private decimal m_NettoRicavo; // [Double] Valore del netto ricavo
            private decimal m_CommissioniBancarie; // [Double] Valore delle commissioni bancarie
            private decimal m_Interessi; // [Double] Valore degli interessi
            private decimal m_Imposte; // [Double] Valore delle imposte
            private decimal m_SpeseConvenzioni; // [Double] Valore delle spese per le convenzioni
            private decimal m_AltreSpese; // [Double] Valore di eventuali altre spese
            private decimal m_Rivalsa; // [Double] Valore della rivalsa
            private double m_TEG;    // [Double] TEG del finanziamento
            private double m_TEG_Max; // [Double] TEG massimo per il prodotto
            private double m_TAEG; // [Double] TAEG del finanziamento
            private double m_TAEG_Max; // [Double] TAEG massimo per il prodotto
            private double m_TAN; // [Double] TAN del finanziamento
            private DateTime? m_DataDecorrenza;
            private CCollection<CFsbPrev_StampaOfferta> m_Stampe;                    // [ArrayOf COffertaStampa] Stampe
            private string m_Sesso;
            private bool m_CaricaAlMassimo;
            private TEGCalcFlag m_TipoCalcoloTEG;
            private TEGCalcFlag m_TipoCalcoloTAEG;
            private int m_IDSupervisore;
            [NonSerialized]
            private Sistema.CUser m_Supervisore;
            private string m_NomeSupervisore;
            private string m_MotivoRichiestaSconto;
            private string m_DettaglioRichiestaSconto;
            private string m_MotivoConfermaSconto;
            private string m_DettaglioConfermaSconto;
            private DateTime? m_DataConfermaSconto;
            private int m_IDSchermata;
            private Sistema.CAttachment m_Schermata;
            private ErrorCodes m_ErrorCode; // [INT] Codice di errore del calcolo dell'offerta
            private System.Text.StringBuilder m_Messages; // [TEXT] Messaggi di errore del calcolo dell'offerta
            private OffertaFlags m_Flags;
            private decimal? m_LimiteRataMax;
            private string m_LimiteRataNote;

            // Private m_Estinzioni As CCollection(Of EstinzioneXEstintore)
            private decimal? m_PremioDaCessionario;               // Premio eventuale che viene corrisposto dal cessionario all'agenzia
            private CKeyCollection m_Attributi;
            private decimal? m_CapitaleFinanziato;
            private decimal? m_ProvvTAN;
            private decimal? m_ProvvCollab;
            private CCQSPDProvvigioneXOffertaCollection m_Provvigioni;
            private int m_IDCollaboratore;
            private CCollaboratore m_Collaboratore;
            private int m_IDClienteXCollaboratore;
            private ClienteXCollaboratore m_ClienteXCollaboratore;
            private DateTime? m_DataCaricamento;

            public COffertaCQS()
            {
                m_Calcolato = false;
                m_OffertaLibera = true;
                m_PreventivoID = 0;
                m_Preventivo = null;
                m_Eta = 0d;
                m_Anzianita = 0d;
                // m_AssicurazioneID               'DEPRECATED [INT] id della tripla di spese assicurative
                // m_Assicurazione                 'DEPRECATED [CSpese] tripla di spese assicurative
                // Pm_NomeAssicurazione             'DEPRECATED [TEXT] Nome della tripla di spese assicurative
                m_Prodotto = null;
                m_ProdottoID = 0;
                m_NomeProdotto = "";
                m_TabellaAssicurativaRelID = 0;
                m_TabellaAssicurativaRel = null;
                m_NomeTabellaAssicurativa = "";
                m_TabellaFinanziariaRelID = 0;
                m_TabellaFinanziariaRel = null;
                m_NomeTabellaFinanziaria = "";
                m_TabellaSpeseID = 0;
                m_TabellaSpese = null;
                m_PremioVita = 0m;
                m_PremioImpiego = 0m;
                m_PremioCredito = 0m;
                m_ImpostaSostitutiva = 0m;
                m_OneriErariali = 0m;
                m_NettoRicavo = 0m;
                m_CommissioniBancarie = 0m;
                m_Interessi = 0m;
                m_Imposte = 0m;
                m_SpeseConvenzioni = 0m;
                m_AltreSpese = 0m;
                m_ValoreUpFront = 0d;
                m_ValoreRunning = 0d;
                m_ValoreSpreadBase = 0d;
                m_ValoreSpread = 0d;
                m_ValoreRiduzioneProvvigionale = 0.0m;
                m_ValoreRappel = 0d;
                m_Rivalsa = 0m;
                m_TEG = 0d;
                m_TabellaAssicurativaRel = null;
                m_TEG_Max = 0d;
                m_TAEG = 0d;
                m_TAEG_Max = 0d;
                m_TAN = 0d;
                m_DataDecorrenza = default;
                m_Stampe = null;
                m_Sesso = "";
                m_ErrorCode = 0;
                m_Messages = new System.Text.StringBuilder();
                m_Rata = 0m;
                m_Durata = 0;
                m_DataNascita = default;
                m_DataAssunzione = default;
                m_Cliente = null;
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_IDCessionario = 0;
                m_NomeCessionario = "";
                m_Cessionario = null;
                m_IDProfilo = 0;
                m_NomeProfilo = "";
                m_Profilo = null;
                m_Pratica = null;
                m_IDPratica = 0;
                m_StatoOfferta = StatoOfferta.NON_ASSOCIATO;
                m_CaricaAlMassimo = false;
                m_IDSupervisore = 0;
                m_Supervisore = null;
                m_NomeSupervisore = "";
                m_MotivoRichiestaSconto = "";
                m_DettaglioRichiestaSconto = "";
                m_MotivoConfermaSconto = "";
                m_DettaglioConfermaSconto = "";
                m_DataConfermaSconto = default;
                m_IDSchermata = 0;
                m_Schermata = null;
                // Me.m_TipoCalcoloTAEG = 
                m_Flags = 0;
                m_LimiteRataMax = default;
                m_LimiteRataNote = "";
                m_Provvigionale = new CProvvigionale();
                // Me.m_Estinzioni = Nothing
                m_PremioDaCessionario = default;
                m_Attributi = null;
                m_CapitaleFinanziato = default;
                m_ProvvTAN = default;
                m_ProvvCollab = default;
                m_Provvigioni = null;
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
                m_IDClienteXCollaboratore = 0;
                m_ClienteXCollaboratore = null;
                m_DataCaricamento = DMD.DateUtils.Now();
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

            protected internal void SetCollaboratore(CCollaboratore value)
            {
                m_Collaboratore = value;
                m_IDCollaboratore = DBUtils.GetID(value);
            }

            public int IDClienteXCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_ClienteXCollaboratore, m_IDClienteXCollaboratore);
                }

                set
                {
                    int oldValue = IDClienteXCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDClienteXCollaboratore = value;
                    m_ClienteXCollaboratore = null;
                    DoChanged("IDClienteXCollaboratore", value, oldValue);
                }
            }

            public ClienteXCollaboratore ClienteXCollaboratore
            {
                get
                {
                    if (m_ClienteXCollaboratore is null)
                        m_ClienteXCollaboratore = Collaboratori.ClientiXCollaboratori.GetItemById(m_IDClienteXCollaboratore);
                    return m_ClienteXCollaboratore;
                }

                set
                {
                    var oldValue = m_ClienteXCollaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDClienteXCollaboratore = DBUtils.GetID(value);
                    m_ClienteXCollaboratore = value;
                    DoChanged("ClienteXCollaboratore", value, oldValue);
                }
            }

            protected internal void SetClienteXCollaboratore(ClienteXCollaboratore value)
            {
                m_IDClienteXCollaboratore = DBUtils.GetID(value);
                m_ClienteXCollaboratore = value;
            }

            public CCQSPDProvvigioneXOffertaCollection Provvigioni
            {
                get
                {
                    if (m_Provvigioni is null)
                        m_Provvigioni = new CCQSPDProvvigioneXOffertaCollection(this);
                    return m_Provvigioni;
                }
            }

            public decimal? ValoreProvvigioneCollaboratore
            {
                get
                {
                    return m_ProvvCollab;
                }

                set
                {
                    var oldValue = m_ProvvCollab;
                    if (oldValue == value == true)
                        return;
                    m_ProvvCollab = value;
                    DoChanged("ValoreProvvigioneCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore della provvigione TAN
        /// </summary>
        /// <returns></returns>
            public decimal? ValoreProvvTAN
            {
                get
                {
                    return m_ProvvTAN;
                }

                set
                {
                    var oldValue = m_ProvvTAN;
                    if (oldValue == value == true)
                        return;
                    m_ProvvTAN = value;
                    DoChanged("ValoreProvvTAN", value, oldValue);
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
        /// Restituisce la collezione degli attributi
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

            public decimal ValoreRiduzioneProvvigionale
            {
                get
                {
                    return m_ValoreRiduzioneProvvigionale;
                }

                set
                {
                    decimal oldValue = m_ValoreRiduzioneProvvigionale;
                    if (oldValue == value)
                        return;
                    m_ValoreRiduzioneProvvigionale = value;
                    DoChanged("ValoreRiduzioneProvvigionale", value, oldValue);
                }
            }

            public CProvvigionale Provvigionale
            {
                get
                {
                    return m_Provvigionale;
                }
            }

            // Public ReadOnly Property Estinzioni As CCollection(Of EstinzioneXEstintore)
            // Get
            // If (Me.m_Estinzioni Is Nothing) Then Me.m_Estinzioni = Finanziaria.Estinzioni.GetEstinzioniXEstintore(Me)
            // Return Me.m_Estinzioni
            // End Get
            // End Property

            // ''' <summary>
            // ''' Restituisce vero se l'offerta estingue uno qualsiasi degli altri prestiti in corso registrati
            // ''' </summary>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Function IsEstinzione() As Boolean
            // For Each e As EstinzioneXEstintore In Me.Estinzioni
            // If e.Selezionata Then Return True
            // Next
            // Return False
            // End Function

            // ''' <summary>
            // ''' Restituisce vero se l'offerta estingue un altro prestito fatto con lo stesso cessionario
            // ''' </summary>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Function IsRinnovo() As Boolean
            // For Each e As EstinzioneXEstintore In Me.Estinzioni
            // If e.Selezionata AndAlso (e.Estinzione IsNot Nothing) AndAlso (e.Estinzione.IDIstituto = Me.IDCessionario) Then Return True
            // Next
            // Return False
            // End Function


            public string LimiteRataNote
            {
                get
                {
                    return m_LimiteRataNote;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_LimiteRataNote;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_LimiteRataNote = value;
                    DoChanged("LimiteRataNote", value, oldValue);
                }
            }

            public decimal? LimiteRataMax
            {
                get
                {
                    return m_LimiteRataMax;
                }

                set
                {
                    decimal oldValue = (decimal)m_LimiteRataMax;
                    if (oldValue == value == true)
                        return;
                    m_LimiteRataMax = value;
                    DoChanged("LimiteRataMax", value, oldValue);
                }
            }

            public bool IsScontata()
            {
                return ProvvigioneMassima > 0d && Maths.Abs(Spread - ProvvigioneMassima) > 0.0001d;
            }

            public bool RichiedeApprovazione()
            {
                return IsScontata(); // And Me.MotivoRichiestaSconto <> "Forzato per altri requisiti"
            }

            public override CModulesClass GetModule()
            {
                return Offerte.Module;
            }

            public OffertaFlags Flags
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

            public bool Visible
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, OffertaFlags.HIDDEN) == false;
                }

                set
                {
                    if (value == Visible)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, OffertaFlags.HIDDEN, !value);
                    DoChanged("Visible", value, !value);
                }
            }

            public TEGCalcFlag TipoCalcoloTAEG
            {
                get
                {
                    return m_TipoCalcoloTAEG;
                }

                set
                {
                    var oldValue = m_TipoCalcoloTAEG;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloTAEG = value;
                    DoChanged("TipoCalcoloTAEG", value, oldValue);
                }
            }

            public TEGCalcFlag TipoCalcoloTEG
            {
                get
                {
                    return m_TipoCalcoloTEG;
                }

                set
                {
                    var oldValue = m_TipoCalcoloTEG;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloTEG = value;
                    DoChanged("TipoCalcoloTEG", value, oldValue);
                }
            }

            public bool OffertaLibera
            {
                get
                {
                    return m_OffertaLibera;
                }

                set
                {
                    if (value == m_OffertaLibera)
                        return;
                    m_OffertaLibera = value;
                    Invalidate();
                    DoChanged("OffertaLibera", value, !value);
                }
            }

            public bool CaricaAlMassimo
            {
                get
                {
                    return m_CaricaAlMassimo;
                }

                set
                {
                    if (m_CaricaAlMassimo == value)
                        return;
                    m_CaricaAlMassimo = value;
                    Invalidate();
                    DoChanged("CaricaAlMassimo", value, !value);
                }
            }

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
                    Invalidate();
                    DoChanged("IDCessionario", value, oldValue);
                }
            }

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
                    if (oldValue == value)
                        return;
                    m_Cessionario = value;
                    m_IDCessionario = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    Invalidate();
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
                    return DBUtils.GetID(m_Profilo, m_IDProfilo);
                }

                set
                {
                    int oldValue = IDProfilo;
                    if (oldValue == value)
                        return;
                    m_IDProfilo = value;
                    m_Profilo = null;
                    Invalidate();
                    DoChanged("IDProfilo", value, oldValue);
                }
            }

            public CProfilo Profilo
            {
                get
                {
                    if (m_Profilo is null)
                        m_Profilo = Profili.GetItemById(m_IDProfilo);
                    return m_Profilo;
                }

                set
                {
                    var oldValue = Profilo;
                    if (oldValue == value)
                        return;
                    m_Profilo = value;
                    m_IDProfilo = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeProfilo = value.ProfiloVisibile;
                    Invalidate();
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
                    var oldValue = Cliente;
                    if (oldValue == value)
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
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
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della relazione tra il prodotto e la tabella Finanziaria utilizzata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int TabellaFinanziariaRelID
            {
                get
                {
                    return DBUtils.GetID(m_TabellaFinanziariaRel, m_TabellaFinanziariaRelID);
                }

                set
                {
                    int oldValue = TabellaFinanziariaRelID;
                    if (oldValue == value)
                        return;
                    m_TabellaFinanziariaRel = null;
                    m_TabellaFinanziariaRelID = value;
                    Invalidate();
                    DoChanged("TabellaFinanziariaRelID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la relazione tra il prodotto e la tabella Finanziaria utilizzata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoXTabellaFin TabellaFinanziariaRel
            {
                get
                {
                    if (m_TabellaFinanziariaRel is null && Prodotto is object)
                        m_TabellaFinanziariaRel = Prodotto.TabelleFinanziarieRelations.GetItemById(m_TabellaFinanziariaRelID);
                    if (m_TabellaFinanziariaRel is null)
                        m_TabellaFinanziariaRel = TabelleFinanziarie.GetTabellaXProdottoByID(m_TabellaFinanziariaRelID);
                    return m_TabellaFinanziariaRel;
                }

                set
                {
                    var oldValue = TabellaFinanziariaRel;
                    if (oldValue == value)
                        return;
                    m_TabellaFinanziariaRel = value;
                    m_TabellaFinanziariaRelID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeTabellaFinanziaria = value.Tabella.Nome;
                    Invalidate();
                    DoChanged("TabellaFinanziariaRel", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della relazione tra il prodotto ed la tabella Finanziaria utilizzata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeTabellaFinanziaria
            {
                get
                {
                    return m_NomeTabellaFinanziaria;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeTabellaFinanziaria;
                    m_NomeTabellaFinanziaria = value;
                    DoChanged("NomeTabellaFinanziaria", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta l'ID della relazione tra il prodotto e la tripla di tabelle assicurative utilizzate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int TabellaAssicurativaRelID
            {
                get
                {
                    return DBUtils.GetID(m_TabellaAssicurativaRel, m_TabellaAssicurativaRelID);
                }

                set
                {
                    int oldValue = TabellaAssicurativaRelID;
                    if (oldValue == value)
                        return;
                    m_TabellaAssicurativaRel = null;
                    m_TabellaAssicurativaRelID = value;
                    Invalidate();
                    DoChanged("TabellaAssicurativaRelID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la relazione tra il prodotto e la tripla di tabelle assicurative utilizzate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoXTabellaAss TabellaAssicurativaRel
            {
                get
                {
                    if (m_TabellaAssicurativaRel is null && Prodotto is object)
                        m_TabellaAssicurativaRel = Prodotto.TabelleAssicurativeRelations.GetItemById(m_TabellaAssicurativaRelID);
                    if (m_TabellaAssicurativaRel is null)
                        m_TabellaAssicurativaRel = TabelleAssicurative.GetTabellaXProdottoByID(m_TabellaAssicurativaRelID);
                    return m_TabellaAssicurativaRel;
                }

                set
                {
                    var oldValue = TabellaAssicurativaRel;
                    if (oldValue == value)
                        return;
                    m_TabellaAssicurativaRel = value;
                    m_TabellaAssicurativaRelID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeTabellaAssicurativa = value.Descrizione;
                    Invalidate();
                    DoChanged("TabellaAssicurativaRel", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della relazione tra il prodotto e la tripla di tabelle assicurative utilizzate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeTabellaAssicurativa
            {
                get
                {
                    return m_NomeTabellaAssicurativa;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeTabellaAssicurativa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeTabellaAssicurativa = value;
                    DoChanged("NomeTabellaAssicurativa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int TabellaSpeseID
            {
                get
                {
                    return DBUtils.GetID(m_TabellaSpese, m_TabellaSpeseID);
                }

                set
                {
                    int oldValue = TabellaSpeseID;
                    if (oldValue == value)
                        return;
                    m_TabellaSpese = null;
                    m_TabellaSpeseID = value;
                    Invalidate();
                    DoChanged("TabellaSpeseID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la tabella spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoXTabellaSpesa TabellaSpese
            {
                get
                {
                    if (m_TabellaSpese is null && Prodotto is object)
                        m_TabellaSpese = Prodotto.TabelleSpese.GetItemById(m_TabellaSpeseID);
                    return m_TabellaSpese;
                }

                set
                {
                    var oldValue = TabellaSpese;
                    if (oldValue == value)
                        return;
                    m_TabellaSpese = value;
                    m_TabellaSpeseID = DBUtils.GetID(value);
                    Invalidate();
                    DoChanged("TabellaSpese", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di caricamento dell'offerta
        /// </summary>
        /// <returns></returns>
            public DateTime? DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }

                set
                {
                    var oldValue = m_DataCaricamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataCaricamento = value;
                    DoChanged("DataCaricamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la data di nascita del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataNascita
            {
                get
                {
                    return m_DataNascita;
                }

                set
                {
                    var oldValue = m_DataNascita;
                    if (oldValue == value == true)
                        return;
                    m_DataNascita = value;
                    Invalidate();
                    DoChanged("DataNascita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la data di assunzione del cliente
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
                    if (oldValue == value == true)
                        return;
                    m_DataAssunzione = value;
                    Invalidate();
                    DoChanged("DataAssunzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la data di decorrenza dell'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    Invalidate();
                    DoChanged("DataDecorrenza", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il sesso del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    Invalidate();
                    DoChanged("Sesso", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la rivalsa
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal Rivalsa
            {
                get
                {
                    // Me.Validate()
                    return m_Rivalsa;
                }

                set
                {
                    decimal oldValue = m_Rivalsa;
                    if (oldValue == value)
                        return;
                    m_Rivalsa = value;
                    Invalidate();
                    DoChanged("Rivalsa", value, oldValue);
                }
            }

            // Public Sub Validate()
            // If Me.m_Calcolato Then Exit Sub
            // Me.Calcola()
            // End Sub

            /// <summary>
        /// Restituisce il valore dei punti sotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Rappel
            {
                get
                {
                    if (MontanteLordo > 0m)
                    {
                        return m_ValoreRappel / (double)MontanteLordo * 100d;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                set
                {
                    if (value < 0d | value > 100d)
                        throw new ArgumentOutOfRangeException("Rappel");
                    double oldValue = Rappel;
                    if (oldValue == value)
                        return;
                    m_ValoreRappel = value * (double)MontanteLordo / 100d;
                    DoChanged("Rappel", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il valore dei punti sotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ValoreRappel
            {
                get
                {
                    // Me.Validate()
                    return (decimal)m_ValoreRappel;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("ValoreRappel");
                    decimal oldValue = (decimal)m_ValoreRappel;
                    if (oldValue == value)
                        return;
                    m_ValoreRappel = (double)value;
                    Invalidate();
                    DoChanged("ValoreRappel", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la provvigione percentuale sul montante lordo aggiunta automaticamente allo spread
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double SpreadBase
            {
                get
                {
                    if (MontanteLordo > 0m)
                    {
                        return m_ValoreSpreadBase / (double)MontanteLordo * 100d;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                set
                {
                    if (value < 0d | value > 100d)
                        throw new ArgumentOutOfRangeException("SpreadBase");
                    double oldValue = SpreadBase;
                    if (oldValue == value)
                        return;
                    m_ValoreSpreadBase = value * (double)MontanteLordo / 100d;
                    Invalidate();
                    DoChanged("SpreadBase", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la parte (valore) del montante lordo che va al mediatore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ValoreSpreadBase
            {
                get
                {
                    return (decimal)m_ValoreSpreadBase;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("ValoreSpreadBase");
                    decimal oldValue = (decimal)m_ValoreSpreadBase;
                    if (oldValue == value)
                        return;
                    m_ValoreSpreadBase = (double)value;
                    Invalidate();
                    DoChanged("ValoreSpreadBase", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la provvigione percentaule sul montante lordo destinata all'agenzia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Spread
            {
                get
                {
                    if (MontanteLordo > 0m)
                    {
                        return m_ValoreSpread / (double)MontanteLordo * 100d;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                set
                {
                    if (value < 0d | value > 100d)
                        throw new ArgumentOutOfRangeException("Spread");
                    double oldValue = Spread;
                    if (oldValue == value)
                        return;
                    m_ValoreSpread = value * (double)MontanteLordo / 100d;
                    Invalidate();
                    DoChanged("Spread", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il valore della commissione agenzia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ValoreSpread
            {
                get
                {
                    return (decimal)m_ValoreSpread;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("ValoreSpread");
                    double oldValue = (double)ValoreSpread;
                    if (oldValue == (double)value)
                        return;
                    m_ValoreSpread = (double)value;
                    Invalidate();
                    DoChanged("ValoreSpread", value, oldValue);
                }
            }



            /// <summary>
        /// Somma dei punti base e del rappel
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal GuadagnoAziendale
            {
                get
                {
                    return ValoreRappel + ValoreSpreadBase;
                }
            }

            /// <summary>
        /// Somma dei punti base e del rappel e delle provvigioni
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal GuadagnoTotale
            {
                get
                {
                    return GuadagnoAziendale;
                }
            }

            /// <summary>
        /// Nominativo del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Nominativo
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            // ''' <summary>
            // ''' Percentuale sul montante lordo che va al mediatore
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property Provvigioni As Double
            // Get
            // If (Me.MontanteLordo > 0) Then
            // Return (Me.m_ValoreProvvigioni / Me.MontanteLordo) * 100
            // Else
            // Return 0
            // End If
            // End Get
            // Set(value As Double)
            // If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Provvigioni")
            // Dim oldValue As Double = Me.Provvigioni
            // If oldValue = value Then Exit Property
            // Me.m_ValoreProvvigioni = value * Me.MontanteLordo / 100
            // Me.Invalidate()
            // Me.DoChanged("Provvigioni", value, oldValue)
            // End Set
            // End Property

            // ''' <summary>
            // ''' Parte del montante lordo (valore) che va al mediatore
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property ValoreProvvigioni As Decimal
            // Get
            // Return Me.m_ValoreProvvigioni
            // End Get
            // Set(value As Decimal)
            // If (value < 0) Then Throw New ArgumentOutOfRangeException("ValoreProvvigioni")
            // Dim oldValue As Decimal = Me.m_ValoreProvvigioni
            // If (oldValue = value) Then Exit Property
            // Me.m_ValoreProvvigioni = value
            // Me.Invalidate()
            // Me.DoChanged("ValoreProvvigioni", value, oldValue)
            // End Set
            // End Property

            public double Running
            {
                get
                {
                    if (MontanteLordo > 0m)
                    {
                        return m_ValoreRunning / (double)MontanteLordo * 100d;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                set
                {
                    if (value < 0d | value > 100d)
                        throw new ArgumentOutOfRangeException("Running");
                    double oldValue = Running;
                    if (oldValue == value)
                        return;
                    m_ValoreRunning = value * (double)MontanteLordo / 100d;
                    DoChanged("Running", value, oldValue);
                }
            }

            public decimal ValoreRunning
            {
                get
                {
                    return (decimal)m_ValoreRunning;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("ValoreRunning");
                    decimal oldValue = (decimal)m_ValoreRunning;
                    if (oldValue == value)
                        return;
                    m_ValoreRunning = (double)value;
                    DoChanged("ValoreRunning", value, oldValue);
                }
            }

            public double UpFront
            {
                get
                {
                    if (MontanteLordo > 0m)
                    {
                        return m_ValoreUpFront / (double)MontanteLordo * 100d;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                set
                {
                    if (value < 0d | value > 100d)
                        throw new ArgumentOutOfRangeException("UpFront");
                    double oldValue = UpFront;
                    if (oldValue == value)
                        return;
                    m_ValoreUpFront = value * (double)MontanteLordo / 100d;
                    DoChanged("UpFront", value, oldValue);
                }
            }

            public decimal ValoreUpFront
            {
                get
                {
                    return (decimal)m_ValoreUpFront;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("ValoreUpFront");
                    decimal oldValue = (decimal)m_ValoreUpFront;
                    if (oldValue == value)
                        return;
                    m_ValoreUpFront = (double)value;
                    DoChanged("ValoreUpFront", value, oldValue);
                }
            }

            /// <summary>
        /// Percentuale sul montante lordo che va al mediatore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double ProvvigioneMassima
            {
                get
                {
                    if (MontanteLordo > 0m)
                    {
                        return m_ValoreProvvigioneMassima / (double)MontanteLordo * 100d;
                    }
                    else
                    {
                        return 0d;
                    }
                }

                set
                {
                    if (value < 0d | value > 100d)
                        throw new ArgumentOutOfRangeException("ProvvigioneMassima");
                    double oldValue = ProvvigioneMassima;
                    if (oldValue == value)
                        return;
                    m_ValoreProvvigioneMassima = value * (double)MontanteLordo / 100d;
                    DoChanged("ProvvigioneMassima", value, oldValue);
                }
            }

            public decimal ValoreProvvigioneMassima
            {
                get
                {
                    return (decimal)m_ValoreProvvigioneMassima;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("ValoreProvvigioneMassima");
                    double oldValue = m_ValoreProvvigioneMassima;
                    if (oldValue == (double)value)
                        return;
                    m_ValoreProvvigioneMassima = (double)value;
                    DoChanged("ValoreProvvigioneMassima", value, oldValue);
                }
            }


            // ''' <summary>
            // ''' Restituisce la somma tra spread e provvigioni al produttore
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public ReadOnly Property ProvvigioneTotale As Double
            // Get
            // Return Me.Provvigioni + Me.SpreadBase
            // End Get
            // End Property

            /// <summary>
        /// Restituisce la somma tra spread e provvigioni al produttore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ValoreProvvigioneTotale
            {
                get
                {
                    return (decimal)(Provvigionale.ValoreTotale() + ValoreSpread);
                }
            }



            /// <summary>
        /// Restiuisce il montante lordo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal MontanteLordo
            {
                get
                {
                    return m_Rata * m_Durata;
                }

                set
                {
                    decimal oldValue = MontanteLordo;
                    if (oldValue == value)
                        return;
                    if (m_Durata > 0)
                    {
                        m_Rata = value / m_Durata;
                    }
                    else if (m_Rata > 0m)
                    {
                        m_Durata = (int)Maths.Floor(value / m_Rata);
                    }
                    else
                    {
                        throw new DivideByZeroException();
                    }

                    Invalidate();
                    DoChanged("MontanteLordo", value, oldValue);
                }
            }

            // Public ReadOnly Property CapitaleFinanziato As Decimal
            // Get
            // Return Me.MontanteLordo - Me.Interessi
            // End Get
            // End Property

            /// <summary>
        /// Restituisce la durata in mesi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    Invalidate();
                    DoChanged("Durata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'ID del prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ProdottoID
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_ProdottoID);
                }

                set
                {
                    int oldValue = ProdottoID;
                    if (oldValue == value)
                        return;
                    m_ProdottoID = value;
                    m_Prodotto = null;
                    Invalidate();
                    DoChanged("ProdottoID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'oggetto prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    {
                        m_TipoCalcoloTAEG = (TEGCalcFlag)value.TipoCalcoloTAEG;
                        m_TipoCalcoloTEG = (TEGCalcFlag)value.TipoCalcoloTeg;
                        m_NomeProdotto = value.Nome;
                    }

                    Invalidate();
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del prodotto utilizzato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            /// <summary>
        /// Restituisce il valore dell'imposta sostitutiva
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ImpostaSostitutiva
            {
                get
                {
                    // Me.Validate()
                    return m_ImpostaSostitutiva;
                }

                set
                {
                    decimal oldValue = m_ImpostaSostitutiva;
                    if (oldValue == value)
                        return;
                    m_ImpostaSostitutiva = value;
                    Invalidate();
                    DoChanged("ImpostaSostitutiva", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il valore di eventuali altre spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal AltreSpese
            {
                get
                {
                    // Me.Validate()
                    return m_AltreSpese;
                }

                set
                {
                    decimal oldValue = m_AltreSpese;
                    if (oldValue == value)
                        return;
                    m_AltreSpese = value;
                    Invalidate();
                    DoChanged("AltreSpese", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il valore delle imposte
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal Imposte
            {
                get
                {
                    // Me.Validate()
                    return m_Imposte;
                }

                set
                {
                    decimal oldValue = m_Imposte;
                    if (oldValue == value)
                        return;
                    m_Imposte = value;
                    Invalidate();
                    DoChanged("Imposte", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il valore delle spese per eventuali convenzioni
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SpeseConvenzioni
            {
                get
                {
                    // Me.Validate()
                    return m_SpeseConvenzioni;
                }

                set
                {
                    decimal oldValue = m_SpeseConvenzioni;
                    if (oldValue == value)
                        return;
                    m_SpeseConvenzioni = value;
                    Invalidate();
                    DoChanged("SpeseConvenzioni", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'età calcolata secondo le approssimazioni relative all'assicurazione sulla vita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Eta
            {
                get
                {
                    return m_Eta;
                }

                set
                {
                    decimal oldValue = (decimal)m_Eta;
                    if ((double)oldValue == value)
                        return;
                    m_Eta = value;
                    Invalidate();
                    DoChanged("Eta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'età reale all'inizio del finanziamento ovvero la differenza tra la data di decorrenza e la data di nascita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double EtaIF
            {
                get
                {
                    if (DataNascita.HasValue && DataDecorrenza.HasValue)
                    {
                        return DateUtils.DateDiff(DateTimeInterval.Year, DataNascita.Value, DataDecorrenza.Value);
                    }
                    else
                    {
                        return 0d;
                    }
                }
            }

            /// <summary>
        /// Restituisce l'età reale alla fine del finanziamento ovvero data di decorrenza + durata/12 - data di nascita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double EtaFF
            {
                get
                {
                    return EtaIF + Durata / 12d;
                }
            }

            /// <summary>
        /// Restituisce l'anzianità di servizio calcolata secondo gli arrotondamenti stabiliti dall'assicurazione  rischio impiego o rischio credito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Anzianita
            {
                get
                {
                    return m_Anzianita;
                }

                set
                {
                    decimal oldValue = (decimal)m_Anzianita;
                    if ((double)oldValue == value)
                        return;
                    if ((double)oldValue == value)
                        return;
                    m_Anzianita = value;
                    Invalidate();
                    DoChanged("Anzianita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'anzianità reale all'inizio del finanziamento ovvero la differenza tra la data di decorrenza e la data di assunzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double AnzianitaIF
            {
                get
                {
                    if (DataAssunzione.HasValue && DataDecorrenza.HasValue)
                    {
                        return DateUtils.DateDiff(DateTimeInterval.Year, DataAssunzione.Value, DataDecorrenza.Value);
                    }
                    else
                    {
                        return 0d;
                    }
                }
            }

            /// <summary>
        /// Restituisce l'anzianità reale alla fine del finanziamento ovvero data di decorrenza + durata/12 - data di assunzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double AnzianitaFF
            {
                get
                {
                    return AnzianitaIF + Durata / 12d;
                }
            }

            /// <summary>
        /// Valore degli interessi sul montante lordo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal Interessi
            {
                get
                {
                    Validate();
                    return m_Interessi;
                }

                set
                {
                    decimal oldValue = m_Interessi;
                    if (oldValue == value)
                        return;
                    m_Interessi = value;
                    Invalidate();
                    DoChanged("Interessi", value, oldValue);
                }
            }

            /// <summary>
        /// Valore delle commissioni bancarie
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal CommissioniBancarie
            {
                get
                {
                    Validate();
                    return m_CommissioniBancarie;
                }

                set
                {
                    decimal oldValue = m_CommissioniBancarie;
                    if (oldValue == value)
                        return;
                    m_CommissioniBancarie = value;
                    Invalidate();
                    DoChanged("CommissioniBancarie", value, oldValue);
                }
            }

            /// <summary>
        /// Valore degli oneri erariali
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal OneriErariali
            {
                get
                {
                    Validate();
                    return m_OneriErariali;
                }

                set
                {
                    decimal oldValue = m_OneriErariali;
                    if (oldValue == value)
                        return;
                    m_OneriErariali = value;
                    Invalidate();
                    DoChanged("OneriErariali", value, oldValue);
                }
            }

            /// <summary>
        /// Valore delle spese assicurative (vita + impiego + credito)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SpeseAssicurative
            {
                get
                {
                    return PremioVita + PremioImpiego + PremioCredito;
                }
            }

            /// <summary>
        /// Valore del premio vita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal PremioVita
            {
                get
                {
                    Validate();
                    return m_PremioVita;
                }

                set
                {
                    decimal oldValue = m_PremioVita;
                    if (oldValue == value)
                        return;
                    m_PremioVita = value;
                    Invalidate();
                    DoChanged("PremioVita", value, oldValue);
                }
            }

            /// <summary>
        /// Valore del premio impiego
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal PremioImpiego
            {
                get
                {
                    Validate();
                    return m_PremioImpiego;
                }

                set
                {
                    decimal oldValue = m_PremioImpiego;
                    if (oldValue == value)
                        return;
                    m_PremioImpiego = value;
                    Invalidate();
                    DoChanged("PremioImpiego", value, oldValue);
                }
            }

            /// <summary>
        /// Valore del premio credito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal PremioCredito
            {
                get
                {
                    Validate();
                    return m_PremioCredito;
                }

                set
                {
                    decimal oldValue = m_PremioCredito;
                    if (oldValue == value)
                        return;
                    m_PremioCredito = value;
                    Invalidate();
                    DoChanged("PremioCredito", value, oldValue);
                }
            }

            /// <summary>
        /// Valore del netto ricavo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal NettoRicavo
            {
                get
                {
                    Validate();
                    return m_NettoRicavo;
                }

                set
                {
                    decimal oldValue = m_NettoRicavo;
                    if (oldValue == value)
                        return;
                    m_NettoRicavo = value;
                    Invalidate();
                    DoChanged("NettoRicavo", value, oldValue);
                }
            }

            /// <summary>
        /// Valore del TEG
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double TEG
            {
                get
                {
                    Validate();
                    return m_TEG;
                }

                set
                {
                    double oldValue = m_TEG;
                    if (oldValue == value)
                        return;
                    m_TEG = value;
                    Invalidate();
                    DoChanged("TEG", value, oldValue);
                }
            }

            /// <summary>
        /// TEG Massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double TEG_Max
            {
                get
                {
                    // Me.Validate()
                    return m_TEG_Max;
                }

                set
                {
                    double oldValue = m_TEG_Max;
                    if (oldValue == value)
                        return;
                    m_TEG_Max = value;
                    Invalidate();
                    DoChanged("TEG_Max", value, oldValue);
                }
            }

            /// <summary>
        /// Valore del TAEG
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double TAEG
            {
                get
                {
                    Validate();
                    return m_TAEG;
                }

                set
                {
                    double oldValue = m_TAEG;
                    if (oldValue == value)
                        return;
                    m_TAEG = value;
                    Invalidate();
                    DoChanged("TAEG", value, oldValue);
                }
            }

            /// <summary>
        /// TAEG Massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double TAEG_Max
            {
                get
                {
                    // Me.Validate()
                    return m_TAEG_Max;
                }

                set
                {
                    double oldValue = m_TAEG_Max;
                    if (oldValue == value)
                        return;
                    m_TAEG_Max = value;
                    Invalidate();
                    DoChanged("TAEG_Max", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il TAN
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double TAN
            {
                get
                {
                    Validate();
                    return m_TAN;
                }

                set
                {
                    double oldValue = m_TAN;
                    if (oldValue == value)
                        return;
                    m_TAN = value;
                    Invalidate();
                    DoChanged("TAN", value, oldValue);
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
                    DoChanged("IDPratica", value, oldValue);
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
                    if (oldValue == value)
                        return;
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                    DoChanged("Pratica", value, oldValue);
                }
            }

            public StatoOfferta StatoOfferta
            {
                get
                {
                    return m_StatoOfferta;
                }

                set
                {
                    var oldValue = m_StatoOfferta;
                    if (oldValue == value)
                        return;
                    m_StatoOfferta = value;
                    DoChanged("StatoOfferta", value, oldValue);
                }
            }

            /// <summary>
        /// ID del preventivo a cui appartiene l'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int PreventivoID
            {
                get
                {
                    return DBUtils.GetID(m_Preventivo, m_PreventivoID);
                }

                set
                {
                    int oldValue = PreventivoID;
                    if (oldValue == value)
                        return;
                    m_Preventivo = null;
                    m_PreventivoID = value;
                    // Me.Invalidate()
                    DoChanged("PreventivoID", value, oldValue);
                }
            }

            /// <summary>
        /// Oggetto preventivo a cui appartiene l'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPreventivo Preventivo
            {
                get
                {
                    if (m_Preventivo is null)
                        m_Preventivo = Preventivi.GetItemById(m_PreventivoID);
                    return m_Preventivo;
                }

                set
                {
                    var oldValue = m_Preventivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Preventivo = value;
                    m_PreventivoID = DBUtils.GetID(value);
                    DoChanged("Preventivo", value, oldValue);
                }
            }

            protected internal virtual void SetPreventivo(CPreventivo value)
            {
                m_Preventivo = value;
                m_PreventivoID = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce il TEG Massimo impostato per il prodotto
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public double GetMaxTEG()
            {
                return Prodotto.GetMaxTEG(this);
            }

            /// <summary>
        /// Restituisce il TAEG Massimo impostato per il prodotto
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public double GetMaxTAEG()
            {
                return Prodotto.GetMaxTAEG(this);
            }

            /// <summary>
        /// Somma di tutte le spese (interessi + commissioni + premi assicurativi + oneri + imposte + altre)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SommaDelleSpese
            {
                get
                {
                    return (decimal)(SpeseConvenzioni + AltreSpese + Interessi + CommissioniBancarie + Provvigionale.ValoreTotale() + SpeseAssicurative + OneriErariali + Imposte + ImpostaSostitutiva);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha supervisionato l'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDSupervisore
            {
                get
                {
                    return DBUtils.GetID(m_Supervisore, m_IDSupervisore);
                }

                set
                {
                    int oldValue = IDSupervisore;
                    if (oldValue == value)
                        return;
                    m_IDSupervisore = value;
                    m_Supervisore = null;
                    DoChanged("IDSupervisore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha supervisionato l'offerta
        /// </summary>
        /// <remarks></remarks>
            public Sistema.CUser Supervisore
            {
                get
                {
                    if (m_Supervisore is null)
                        m_Supervisore = Sistema.Users.GetItemById(m_IDSupervisore);
                    return m_Supervisore;
                }

                set
                {
                    m_Supervisore = value;
                    m_IDSupervisore = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeSupervisore = value.Nominativo;
                    DoChanged("Supervisore", value);
                }
            }

            public string NomeSupervisore
            {
                get
                {
                    return m_NomeSupervisore;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeSupervisore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSupervisore = value;
                    DoChanged("NomeSupervisore", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta una stringa che descrive "in sintesi" il motivo della richiesta dello sconto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MotivoRichiestaSconto
            {
                get
                {
                    return m_MotivoRichiestaSconto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_MotivoRichiestaSconto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoRichiestaSconto = value;
                    DoChanged("MotivoRichiestaSconto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che specifica per esteso il motivo della richiesta dello sconto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DettaglioRichiestaSconto
            {
                get
                {
                    return m_DettaglioRichiestaSconto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_DettaglioRichiestaSconto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioRichiestaSconto = value;
                    DoChanged("DettaglioRichiestaSconto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce una stringa che specifica "in sintesi" il motivo per cui si è approvato lo sconto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MotivoConfermaSconto
            {
                get
                {
                    return m_MotivoConfermaSconto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_MotivoConfermaSconto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoConfermaSconto = value;
                    DoChanged("MotivoConfermaSconto", value, oldValue);
                }
            }

            public string DettaglioConfermaSconto
            {
                get
                {
                    return m_DettaglioConfermaSconto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_DettaglioConfermaSconto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioConfermaSconto = value;
                    DoChanged("DettaglioConfermaSconto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui il supervisore ha confermato o negato lo sconto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataConfermaSconto
            {
                get
                {
                    return m_DataConfermaSconto;
                }

                set
                {
                    var oldValue = m_DataConfermaSconto;
                    if (oldValue == value == true)
                        return;
                    m_DataConfermaSconto = value;
                    DoChanged("DataConfermaSconto", value, oldValue);
                }
            }

            public int IDSchermata
            {
                get
                {
                    return DBUtils.GetID(m_Schermata, m_IDSchermata);
                }

                set
                {
                    int oldValue = IDSchermata;
                    if (oldValue == value)
                        return;
                    m_IDSchermata = value;
                    DoChanged("IDSchermata", value, oldValue);
                }
            }

            public Sistema.CAttachment Schermata
            {
                get
                {
                    if (m_Schermata is null)
                        m_Schermata = Sistema.Attachments.GetItemById(m_IDSchermata);
                    return m_Schermata;
                }

                set
                {
                    m_Schermata = value;
                    m_IDSchermata = DBUtils.GetID(value);
                    DoChanged("Schermata", value);
                }
            }



            /// <summary>
        /// Restituisce il codice di errore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public ErrorCodes ErrorCode
            {
                get
                {
                    Validate();
                    return m_ErrorCode;
                }
            }

            /// <summary>
        /// Restituisce i messaggi di errore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Messages
            {
                get
                {
                    Validate();
                    return m_Messages.ToString();
                }
            }

            /// <summary>
        /// Inserisce un nuovo messaggio
        /// </summary>
        /// <param name="message"></param>
        /// <remarks></remarks>
            private void AppendMessage(string message)
            {
                if (this.m_Messages.Length > 0)
                    this.m_Messages.Append(DMD.Strings.vbCrLf);
                this.m_Messages.Append(message);
            }

            /// <summary>
        /// Rimuove tutti i messaggi
        /// </summary>
        /// <remarks></remarks>
            private void ClearMessages()
            {
                this.m_Messages.Clear();
            }

            /// <summary>
        /// Calcola il TEG ed il TAEG in funzione delle spese indicate
        /// </summary>
        /// <param name="offerta"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public double CalculateTEG(COffertaCQS offerta, TEGCalcFlag flag)
            {
                var fEval = new CTAEGFunEvaluator();
                fEval.Durata = Durata;
                fEval.Quota = Rata;
                fEval.NettoRicavo = MontanteLordo;
                if (Sistema.TestFlag(flag, TEGCalcFlag.ALTREIMPOSTE))
                    fEval.NettoRicavo -= Imposte;
                if (Sistema.TestFlag(flag, TEGCalcFlag.ALTRESPESE))
                    fEval.NettoRicavo -= AltreSpese;
                if (Sistema.TestFlag(flag, TEGCalcFlag.IMPOSTASOSTITUTIVA))
                    fEval.NettoRicavo -= ImpostaSostitutiva;
                if (Sistema.TestFlag(flag, TEGCalcFlag.INTERESSI))
                    fEval.NettoRicavo -= Interessi;
                if (Sistema.TestFlag(flag, TEGCalcFlag.COMMISSIONI))
                    fEval.NettoRicavo -= CommissioniBancarie;
                if (Sistema.TestFlag(flag, TEGCalcFlag.ONERIERARIALI))
                    fEval.NettoRicavo -= OneriErariali;
                if (Sistema.TestFlag(flag, TEGCalcFlag.PREMIOCREDITO))
                    fEval.NettoRicavo -= PremioCredito;
                if (Sistema.TestFlag(flag, TEGCalcFlag.PREMIOIMPIEGO))
                    fEval.NettoRicavo -= PremioImpiego;
                if (Sistema.TestFlag(flag, TEGCalcFlag.PREMIOVITA))
                    fEval.NettoRicavo -= PremioVita;
                if (Sistema.TestFlag(flag, TEGCalcFlag.PROVVIGIONE))
                    fEval.NettoRicavo = (decimal)(fEval.NettoRicavo - Provvigionale.ValoreTotale());
                if (Sistema.TestFlag(flag, TEGCalcFlag.RIVALSA))
                    fEval.NettoRicavo -= Rivalsa;
                if (Sistema.TestFlag(flag, TEGCalcFlag.SPESECONVENZIONI))
                    fEval.NettoRicavo -= SpeseConvenzioni;
                if (Sistema.TestFlag(flag, TEGCalcFlag.SPREAD))
                    fEval.NettoRicavo -= ValoreSpreadBase;
                return Maths.FindZero(fEval, Maths.TOLMIN, 1d) * 100d;
            }

            public bool CanCalculate()
            {
                bool ret = true;
                ret = ret & Prodotto is object;
                return ret;
            }

            /// <summary>
        /// Calcola il TAN
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public double CalculateTAN()
            {
                var c = new CTANCalculator();
                c.Rata = Rata;
                c.Durata = Durata;
                c.Importo = MontanteLordo - m_Interessi;
                return c.Calc();
            }

            public decimal Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    decimal oldValue = m_Rata;
                    if (oldValue == value)
                        return;
                    m_Rata = value;
                    Invalidate();
                    DoChanged("Rata", value, oldValue);
                }
            }

            /// <summary>
        /// Azzera tutti i calcoli
        /// </summary>
        /// <remarks></remarks>
            public void Reset()
            {
                m_ErrorCode = ErrorCodes.ERROR_GENERIC;
                ClearMessages();
                m_Calcolato = false;
                m_Eta = 0d;
                m_Anzianita = 0d;
                m_ValoreSpreadBase = 0d;
                m_ValoreSpread = 0d;
                m_ValoreRappel = 0d;
                m_ValoreUpFront = 0d;
                m_ValoreRunning = 0d;
                m_PremioVita = 0m;
                m_PremioImpiego = 0m;
                m_PremioCredito = 0m;
                m_ImpostaSostitutiva = 0m;
                m_OneriErariali = 0m;
                m_NettoRicavo = 0m;
                m_CommissioniBancarie = 0m;
                m_Interessi = 0m;
                m_Imposte = 0m;
                m_SpeseConvenzioni = 0m;
                m_AltreSpese = 0m;
                m_Rivalsa = 0m;
                m_TEG = 0d;
                m_TEG_Max = 0d;
                m_TAEG = 0d;
                m_TAEG_Max = 0d;
                m_TAN = 0d;
                m_Provvigionale = new CProvvigionale();
            }

            public void Validate()
            {
                if (m_OffertaLibera || m_Calcolato)
                    return;
                Calcola();
            }

            public void Invalidate()
            {
                if (m_Calculating)
                    return;
                m_Calcolato = false;
            }

            /// <summary>
        /// Funzione utilizzata internamente per il calcolo dell'offerta
        /// </summary>
        /// <remarks></remarks>
            private void CalculateInternal()
            {
                var SpreadBase = default(double);
                Reset();
                if (!m_DataNascita.HasValue && Cliente is object)
                {
                    m_DataNascita = Cliente.DataNascita;
                }

                if (!m_DataAssunzione.HasValue && Cliente is object)
                {
                    if (Cliente.ImpiegoPrincipale is object)
                    {
                        m_DataAssunzione = Cliente.ImpiegoPrincipale.DataAssunzione;
                    }
                    else
                    {
                        for (int i = 0, loopTo = Cliente.Impieghi.Count - 1; i <= loopTo; i++)
                        {
                            var impiego = Cliente.Impieghi[i];
                            if (impiego.DataAssunzione.HasValue)
                            {
                                m_DataAssunzione = impiego.DataAssunzione;
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(m_Sesso) & Cliente is object)
                {
                    m_Sesso = Cliente.Sesso;
                }

                if (!m_DataDecorrenza.HasValue)
                {
                    m_DataDecorrenza = DMD.DateUtils.ToDay();
                }

                if (m_DataNascita.HasValue == false)
                {
                    m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT;
                    m_Calculating = false;
                    AppendMessage("Data di nascita non valida");
                    return;
                }

                if (m_DataAssunzione.HasValue == false)
                {
                    m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT;
                    m_Calculating = false;
                    AppendMessage("Data di assunzione non valida");
                    return;
                }

                if (m_Sesso != "M" & m_Sesso != "F")
                {
                    m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT;
                    m_Calculating = false;
                    AppendMessage("Sesso non valida");
                    return;
                }

                if (Prodotto is null)
                {
                    m_ErrorCode = ErrorCodes.ERROR_INVALIDARGUMENT;
                    m_Calculating = false;
                    AppendMessage("Prodotto nullo");
                    return;
                }

                // Otteniamo lo spread base definito dal listino
                if (Profilo is object)
                {
                    SpreadBase = Profilo.ProdottiXProfiloRelations.GetSpread(Prodotto);
                }

                m_ValoreSpreadBase = SpreadBase * (double)MontanteLordo / 100d;

                // Validiamo la provvigione
                if (this.SpreadBase + Spread + Provvigionale.get_PercentualeSu(MontanteLordo) > Prodotto.ProvvigioneMassima == true)
                {
                    m_ErrorCode = ErrorCodes.ERROR_PROVVMAX;
                    AppendMessage("Superato il provvigionale massimo:" + Sistema.Formats.FormatPercentage(Prodotto.ProvvigioneMassima - SpreadBase, 3) + "%");
                    m_Calculating = false;
                    return;
                }

                // Calcolo della parte Finanziaria
                if (TabellaFinanziariaRel is object)
                {
                    TabellaFinanziariaRel.Calcola(this);
                }

                // Calcolo della parte assicurativa
                if (TabellaAssicurativaRel is object)
                {
                    TabellaAssicurativaRel.Calcola(this);
                }

                // Calcoliamo le spese
                if (TabellaSpese is object && TabellaSpese.TabellaSpese is object)
                {
                    TabellaSpese.TabellaSpese.Calcola(this);
                }


                // Calciliamo il netto ricavo		
                m_NettoRicavo = MontanteLordo - m_Rivalsa - m_SpeseConvenzioni - m_AltreSpese - (m_PremioVita + m_PremioImpiego + m_PremioCredito) - m_OneriErariali - m_Imposte - m_CommissioniBancarie - m_Interessi;
                if (Prodotto.ProvvigioneErogataDa == ProvvigioneErogataDa.Direttamente)
                {
                    if (Provvigionale.ValoreTotale().HasValue)
                    {
                        m_NettoRicavo = (decimal)(m_NettoRicavo - Provvigionale.ValoreTotale());
                    }
                }

                // Calcoliamo il netto ricavo
                m_NettoRicavo = m_NettoRicavo - ImpostaSostitutiva;
                if (TabellaFinanziariaRel is object)
                {
                    if (!TabellaFinanziariaRel.Check(this))
                    {
                        m_ErrorCode = ErrorCodes.ERROR_TABFINCONSTR;
                        m_Messages.Clear();
                        this.m_Messages.Append("Vincoli della tabella Finanziaria non rispettati");
                        m_Calculating = false;
                        return;
                    }
                }

                if (TabellaAssicurativaRel is object)
                {
                    if (!TabellaAssicurativaRel.Check(this))
                    {
                        m_ErrorCode = ErrorCodes.ERROR_TABASSCONSTR;
                        m_Messages.Clear();
                        this.m_Messages.Append( "Vincoli della tabella assicurativa non rispettati");
                        m_Calculating = false;
                        return;
                    }
                }

                // Otteniamo il TEG Max
                m_TEG_Max = GetMaxTEG();

                // Otteniamo il TAEG Max
                m_TAEG_Max = GetMaxTAEG();

                // Calcoliamo il TAN
                m_TAN = CalculateTAN();

                // Calcoliamo il TEG
                m_TEG = CalculateTEG(this, m_TipoCalcoloTEG);
                if (m_TEG < 0d)
                {
                    m_ErrorCode = ErrorCodes.ERROR_ZERO;
                    AppendMessage("Zero della funzione TEG non trovato");
                }

                // Calcoliamo il TAEG
                m_TAEG = CalculateTEG(this, m_TipoCalcoloTAEG);
                if (m_TEG < 0d)
                {
                    m_ErrorCode = ErrorCodes.ERROR_ZERO;
                    AppendMessage("Zero della funzione TAEG non trovato");
                }

                if (m_TEG > m_TEG_Max)
                {
                    m_ErrorCode = ErrorCodes.ERROR_TEGMAX;
                    AppendMessage("TEG troppo alto: " + Sistema.Formats.FormatPercentage(m_TEG_Max, 4) + "%");
                    return;
                }

                if (m_TAEG > m_TAEG_Max)
                {
                    m_ErrorCode = ErrorCodes.ERROR_TAEGMAX;
                    AppendMessage("TAEG troppo alto: " + Sistema.Formats.FormatPercentage(m_TAEG_Max, 4) + "%");
                    return;
                }

                m_ErrorCode = ErrorCodes.ERROR_OK;
            }

            public void Calcola()
            {
                double spreadBase = 0d;
                if (m_Calculating)
                    return;
                m_Calculating = true;
                double p, step;
                bool t;
                step = 0.01d;
                if (Profilo is object && Prodotto is object)
                {
                    spreadBase = Sistema.Formats.ToDouble(Profilo.ProdottiXProfiloRelations.GetSpread(Prodotto));
                }

                if (CaricaAlMassimo)
                {
                    p = Prodotto.ProvvigioneMassima - spreadBase;
                    while (p >= 0d)
                    {
                        Provvigionale.set_PercentualeSu(MontanteLordo, (float?)p);
                        CalculateInternal();
                        switch (ErrorCode)
                        {
                            case ErrorCodes.ERROR_TEGMAX:
                            case ErrorCodes.ERROR_TAEGMAX:
                                {
                                    p = p - 1d;
                                    break;
                                }

                            case ErrorCodes.ERROR_OK:
                                {
                                    t = true;
                                    while (t & p < Prodotto.ProvvigioneMassima - spreadBase)
                                    {
                                        p = p + step;
                                        Provvigionale.set_PercentualeSu(MontanteLordo, (float?)p);
                                        CalculateInternal();
                                        switch (ErrorCode)
                                        {
                                            case ErrorCodes.ERROR_OK:
                                                {
                                                    break;
                                                }

                                            default:
                                                {
                                                    p = p - step;
                                                    t = false;
                                                    break;
                                                }
                                        }
                                    }

                                    t = true;
                                    while (t & p < Prodotto.ProvvigioneMassima - spreadBase)
                                    {
                                        p = p + step;
                                        Provvigionale.set_PercentualeSu(MontanteLordo, (float?)p);
                                        CalculateInternal();
                                        switch (ErrorCode)
                                        {
                                            case ErrorCodes.ERROR_OK:
                                                {
                                                    break;
                                                }

                                            default:
                                                {
                                                    p = p - step;
                                                    t = false;
                                                    break;
                                                }
                                        }
                                    }

                                    Provvigionale.set_PercentualeSu(MontanteLordo, (float?)p);
                                    CalculateInternal();
                                    return;
                                }

                            default:
                                {
                                    Provvigionale.set_PercentualeSu(MontanteLordo, 0);
                                    return;
                                }
                        }
                    }

                    Provvigionale.set_PercentualeSu(MontanteLordo, 0);
                    CalculateInternal();
                }
                else
                {
                    CalculateInternal();
                }

                m_Calcolato = true;
                m_Calculating = false;
            }

            public object EvaluateExpression(string expr)
            {
                return Sistema.Types.CallMethod(this, expr);
            }

            public override string GetTableName()
            {
                return "tbl_Preventivi_Offerte";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_OffertaLibera = reader.Read("OffertaLibera", this.m_OffertaLibera);
                m_IDPratica = reader.Read("IDPratica", this.m_IDPratica);
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_StatoOfferta = reader.Read("StatoOfferta", this.m_StatoOfferta);
                m_PreventivoID = reader.Read("Preventivo", this.m_PreventivoID);
                m_IDCessionario = reader.Read("IDCessionario", this.m_IDCessionario);
                m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                m_IDProfilo = reader.Read("IDProfilo", this.m_IDProfilo);
                m_NomeProfilo = reader.Read("NomeProfilo", this.m_NomeProfilo);
                m_ProdottoID = reader.Read("Prodotto", this.m_ProdottoID);
                m_NomeProdotto = reader.Read("NomeProdotto", this.m_NomeProdotto);
                m_Calcolato = reader.Read("Calcolato", this.m_Calcolato);
                m_Durata = reader.Read("Durata", this.m_Durata);
                m_Rata = reader.Read("Rata", this.m_Rata);
                m_Eta = reader.Read("Eta", this.m_Eta);
                m_Anzianita = reader.Read("Anzianita", this.m_Anzianita);
                m_TabellaAssicurativaRelID = reader.Read("TabellaAssicurativaRel", this.m_TabellaAssicurativaRelID);
                m_NomeTabellaAssicurativa = reader.Read("NomeTabellaAssicurativa", this.m_NomeTabellaAssicurativa);
                m_TabellaFinanziariaRelID = reader.Read("TabellaFinanziariaRel", this.m_TabellaFinanziariaRelID);
                m_NomeTabellaFinanziaria = reader.Read("NomeTabellaFinanziaria", this.m_NomeTabellaFinanziaria);
                m_TabellaSpeseID = reader.Read("TabellaSpese", this.m_TabellaSpeseID);
                m_ValoreProvvigioneMassima = reader.Read("MaxProvv", this.m_ValoreProvvigioneMassima);
                m_ValoreRappel = reader.Read("Rappel", this.m_ValoreRappel);
                m_ValoreSpreadBase = reader.Read("SpreadBase", this.m_ValoreSpreadBase);
                m_ValoreSpread = reader.Read("Spread", this.m_ValoreSpread);
                m_ValoreUpFront = reader.Read("UpFront", this.m_ValoreUpFront);
                m_ValoreRunning = reader.Read("Running", this.m_ValoreRunning);
                m_PremioVita = reader.Read("PremioVita", this.m_PremioVita);
                m_PremioImpiego = reader.Read("PremioImpiego", this.m_PremioImpiego);
                m_PremioCredito = reader.Read("PremioCredito", this.m_PremioCredito);
                m_DataNascita = reader.Read("DataNascita", this.m_DataNascita);
                m_DataAssunzione = reader.Read("DataAssunzione", this.m_DataAssunzione);
                m_ImpostaSostitutiva = reader.Read("ImpostaSostitutiva", this.m_ImpostaSostitutiva);
                m_OneriErariali = reader.Read("OneriErariali", this.m_OneriErariali);
                m_NettoRicavo = reader.Read("NettoRicavo", this.m_NettoRicavo);
                m_CommissioniBancarie = reader.Read("CommissioniBancarie", this.m_CommissioniBancarie);
                m_Interessi = reader.Read("Interessi", this.m_Interessi);
                m_Imposte = reader.Read("Imposte", this.m_Imposte);
                m_SpeseConvenzioni = reader.Read("SpeseConvenzioni", this.m_SpeseConvenzioni);
                m_AltreSpese = reader.Read("AltreSpese", this.m_AltreSpese);
                m_Rivalsa = reader.Read("Rivalsa", this.m_Rivalsa);
                m_TEG = reader.Read("TEG", this.m_TEG);
                m_TEG_Max = reader.Read("TEG_Max", this.m_TEG_Max);
                m_TAEG = reader.Read("TAEG", this.m_TAEG);
                m_TAEG_Max = reader.Read("TAEG_Max", this.m_TAEG_Max);
                m_TAN = reader.Read("TAN", this.m_TAN);
                m_DataDecorrenza = reader.Read("DataDecorrenza", this.m_DataDecorrenza);
                m_Sesso = reader.Read("Sesso", this.m_Sesso);
                m_CaricaAlMassimo = reader.Read("CaricaAlMassimo", this.m_CaricaAlMassimo);
                m_TipoCalcoloTAEG = reader.Read("TipoCalcoloTAEG", this.m_TipoCalcoloTAEG);
                m_TipoCalcoloTEG = reader.Read("TipoCalcoloTEG", this.m_TipoCalcoloTEG);
                m_ErrorCode = reader.Read("ErrorCode", this.m_ErrorCode);
                m_Messages = reader.Read("Messages", this.m_Messages);
                m_IDSupervisore = reader.Read("IDSupervisore", this.m_IDSupervisore);
                m_NomeSupervisore = reader.Read("NomeSupervisore", this.m_NomeSupervisore);
                m_MotivoRichiestaSconto = reader.Read("MotivoRS", this.m_MotivoRichiestaSconto);
                m_DettaglioRichiestaSconto = reader.Read("DettaglioRS", this.m_DettaglioRichiestaSconto);
                m_MotivoConfermaSconto = reader.Read("MotivoCS", this.m_MotivoConfermaSconto);
                m_DettaglioConfermaSconto = reader.Read("DettaglioCS", this.m_DettaglioConfermaSconto);
                m_DataConfermaSconto = reader.Read("DataCS", this.m_DataConfermaSconto);
                m_IDSchermata = reader.Read("IDSchermata", this.m_IDSchermata);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_LimiteRataMax = reader.Read("LimiteRataMax", this.m_LimiteRataMax);
                m_LimiteRataNote = reader.Read("LimiteRataNote", this.m_LimiteRataNote);
                m_ProvvTAN = reader.Read("ProvvTAN", this.m_ProvvTAN);
                m_DataCaricamento = reader.Read("DataCaricamento", this.m_DataCaricamento);
                {
                    var withBlock = this.Provvigionale;
                    withBlock.Tipo = reader.Read("ProvvBrokerSu", withBlock.Tipo);
                    withBlock.ValoreBase = reader.Read("Provvigioni", withBlock.ValoreBase);
                    withBlock.ValorePercentuale = reader.Read("ProvvBrokerPerc", withBlock.ValorePercentuale);
                    withBlock.SetChanged(false);
                }

                m_ValoreRiduzioneProvvigionale = reader.Read("ValoreRiduzioneProvv", this.m_ValoreRiduzioneProvvigionale);
                m_PremioDaCessionario = reader.Read("PremioDaCessionario", this.m_PremioDaCessionario);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    if (!string.IsNullOrEmpty(tmp))
                        this.m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                }

                this.m_CapitaleFinanziato = reader.Read("CapitaleFinanziato", this.m_CapitaleFinanziato);
                this.m_ProvvCollab = reader.Read("ProvvCollab", this.m_ProvvCollab);
                this.m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                this.m_IDClienteXCollaboratore = reader.Read("IDClienteXCollaboratore", this.m_IDClienteXCollaboratore);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("MaxProvv", m_ValoreProvvigioneMassima);
                writer.Write("UpFront", m_ValoreUpFront);
                writer.Write("Running", m_ValoreRunning);
                writer.Write("Rappel", m_ValoreRappel);
                writer.Write("SpreadBase", m_ValoreSpreadBase);
                writer.Write("Spread", m_ValoreSpread);
                writer.Write("Preventivo", DBUtils.GetID(m_Preventivo, m_PreventivoID));
                writer.Write("Eta", m_Eta);
                writer.Write("Anzianita", m_Anzianita);
                writer.Write("Prodotto", DBUtils.GetID(m_Prodotto, m_ProdottoID));
                writer.Write("NomeProdotto", m_NomeProdotto);
                writer.Write("TabellaAssicurativaRel", TabellaAssicurativaRelID);
                writer.Write("NomeTabellaAssicurativa", m_NomeTabellaAssicurativa);
                writer.Write("TabellaFinanziariaRel", DBUtils.GetID(m_TabellaFinanziariaRel, m_TabellaFinanziariaRelID));
                writer.Write("NomeTabellaFinanziaria", m_NomeTabellaFinanziaria);
                writer.Write("TabellaSpese", TabellaSpeseID);
                // dbRis("Assicurazione") = DBUtils.GetID(m_Assicurazione, m_AssicurazioneID)
                // dbRis("NomeAssicurazione") = m_NomeAssicurazione
                writer.Write("Calcolato", m_Calcolato);
                writer.Write("PremioVita", m_PremioVita);
                writer.Write("PremioImpiego", m_PremioImpiego);
                writer.Write("PremioCredito", m_PremioCredito);
                writer.Write("ImpostaSostitutiva", m_ImpostaSostitutiva);
                writer.Write("OneriErariali", m_OneriErariali);
                writer.Write("NettoRicavo", m_NettoRicavo);
                writer.Write("CommissioniBancarie", m_CommissioniBancarie);
                writer.Write("Interessi", m_Interessi);
                writer.Write("Imposte", m_Imposte);
                writer.Write("AltreSpese", m_AltreSpese);
                writer.Write("SpeseConvenzioni", m_SpeseConvenzioni);
                writer.Write("Rivalsa", m_Rivalsa);
                writer.Write("TEG", m_TEG);
                writer.Write("TEG_Max", m_TEG_Max);
                writer.Write("TAEG", m_TAEG);
                writer.Write("TAEG_Max", m_TAEG_Max);
                writer.Write("TAN", m_TAN);
                writer.Write("ErrorCode", m_ErrorCode);
                writer.Write("Messages", m_Messages);
                writer.Write("DataDecorrenza", m_DataDecorrenza);
                writer.Write("Sesso", m_Sesso);
                writer.Write("DataNascita", m_DataNascita);
                writer.Write("DataAssunzione", m_DataAssunzione);
                writer.Write("Rata", m_Rata);
                writer.Write("Durata", m_Durata);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IDCliente", IDCliente);
                writer.Write("IDCessionario", IDCessionario);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("IDProfilo", IDProfilo);
                writer.Write("NomeProfilo", m_NomeProfilo);
                writer.Write("IDPratica", IDPratica);
                writer.Write("StatoOfferta", m_StatoOfferta);
                writer.Write("CaricaAlMassimo", m_CaricaAlMassimo);
                writer.Write("OffertaLibera", m_OffertaLibera);
                writer.Write("TipoCalcoloTAEG", m_TipoCalcoloTAEG);
                writer.Write("TipoCalcoloTEG", m_TipoCalcoloTEG);
                writer.Write("IDSupervisore", IDSupervisore);
                writer.Write("NomeSupervisore", m_NomeSupervisore);
                writer.Write("MotivoRS", m_MotivoRichiestaSconto);
                writer.Write("DettaglioRS", m_DettaglioRichiestaSconto);
                writer.Write("MotivoCS", m_MotivoConfermaSconto);
                writer.Write("DettaglioCS", m_DettaglioConfermaSconto);
                writer.Write("DataCS", m_DataConfermaSconto);
                writer.Write("IDSchermata", IDSchermata);
                writer.Write("Flags", m_Flags);
                writer.Write("LimiteRataMax", m_LimiteRataMax);
                writer.Write("LimiteRataNote", m_LimiteRataNote);
                writer.Write("DataCaricamento", m_DataCaricamento);
                {
                    var withBlock = Provvigionale;
                    writer.Write("ProvvBrokerSu", withBlock.Tipo);
                    writer.Write("Provvigioni", withBlock.ValoreBase);
                    writer.Write("ProvvBrokerPerc", withBlock.ValorePercentuale);
                }

                writer.Write("ValoreRiduzioneProvv", m_ValoreRiduzioneProvvigionale);
                writer.Write("PremioDaCessionario", m_PremioDaCessionario);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("CapitaleFinanziato", m_CapitaleFinanziato);
                writer.Write("ProvvTAN", m_ProvvTAN);
                writer.Write("ProvvCollab", m_ProvvCollab);
                writer.Write("IDCollaboratore", IDCollaboratore);
                writer.Write("IDClienteXCollaboratore", IDClienteXCollaboratore);
                return base.SaveToRecordset(writer);
            }

            public override bool IsChanged()
            {
                return base.IsChanged() || Provvigionale.IsChanged();
            }

            // ------------------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("StatoOfferta", (int?)m_StatoOfferta);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDPreventivo", PreventivoID);
                writer.WriteAttribute("IDCessionario", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("IDProfilo", IDProfilo);
                writer.WriteAttribute("NomeProfilo", m_NomeProfilo);
                writer.WriteAttribute("IDProdotto", ProdottoID);
                writer.WriteAttribute("NomeProdotto", m_NomeProdotto);
                writer.WriteAttribute("Calcolato", m_Calcolato);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("Rata", m_Rata);
                writer.WriteAttribute("Eta", m_Eta);
                writer.WriteAttribute("Anzianita", m_Anzianita);
                writer.WriteAttribute("IDTabellaSpese", TabellaSpeseID);
                writer.WriteAttribute("IDTabellaFinanziariaID", TabellaFinanziariaRelID);
                writer.WriteAttribute("NomeTabellaFinanziaria", m_NomeTabellaFinanziaria);
                writer.WriteAttribute("IDTabellaAssicurativaID", TabellaAssicurativaRelID);
                writer.WriteAttribute("NomeTabellaAssicurativa", m_NomeTabellaAssicurativa);
                writer.WriteAttribute("Rappel", m_ValoreRappel);
                writer.WriteAttribute("ProvvMax", m_ValoreProvvigioneMassima);
                writer.WriteAttribute("SpreadBase", m_ValoreSpreadBase);
                writer.WriteAttribute("Spread", m_ValoreSpread);
                writer.WriteAttribute("UpFront", m_ValoreUpFront);
                writer.WriteAttribute("Running", m_ValoreRunning);
                writer.WriteAttribute("DataNascita", m_DataNascita);
                writer.WriteAttribute("DataAssunzione", m_DataAssunzione);
                writer.WriteAttribute("PremioVita", m_PremioVita);
                writer.WriteAttribute("PremioImpiego", m_PremioImpiego);
                writer.WriteAttribute("PremioCredito", m_PremioCredito);
                writer.WriteAttribute("ImpostaSostitutiva", m_ImpostaSostitutiva);
                writer.WriteAttribute("OneriErariali", m_OneriErariali);
                writer.WriteAttribute("NettoRicavo", m_NettoRicavo);
                writer.WriteAttribute("CommissioniBancarie", m_CommissioniBancarie);
                writer.WriteAttribute("Interessi", m_Interessi);
                writer.WriteAttribute("Imposte", m_Imposte);
                writer.WriteAttribute("SpeseConvenzioni", m_SpeseConvenzioni);
                writer.WriteAttribute("AltreSpese", m_AltreSpese);
                writer.WriteAttribute("Rivalsa", m_Rivalsa);
                writer.WriteAttribute("TEG", m_TEG);
                writer.WriteAttribute("TEG_Max", m_TEG_Max);
                writer.WriteAttribute("TAEG", m_TAEG);
                writer.WriteAttribute("TAEG_Max", m_TAEG_Max);
                writer.WriteAttribute("TAN", m_TAN);
                writer.WriteAttribute("DataDecorrenza", m_DataDecorrenza);
                writer.WriteAttribute("Sesso", m_Sesso);
                writer.WriteAttribute("ErrorCode", (int?)m_ErrorCode);
                writer.WriteAttribute("CaricaAlMassimo", m_CaricaAlMassimo);
                writer.WriteAttribute("OffertaLibera", m_OffertaLibera);
                writer.WriteAttribute("TipoCalcoloTAEG", (int?)m_TipoCalcoloTAEG);
                writer.WriteAttribute("TipoCalcoloTEG", (int?)m_TipoCalcoloTEG);
                writer.WriteAttribute("IDSupervisore", IDSupervisore);
                writer.WriteAttribute("NomeSupervisore", m_NomeSupervisore);
                writer.WriteAttribute("MotivoRS", m_MotivoRichiestaSconto);
                writer.WriteAttribute("DataCS", m_DataConfermaSconto);
                writer.WriteAttribute("IDSchermata", IDSchermata);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("MotivoCS", m_MotivoConfermaSconto);
                writer.WriteAttribute("LimiteRataMax", m_LimiteRataMax);
                writer.WriteAttribute("LimiteRataNote", m_LimiteRataNote);
                writer.WriteAttribute("ValoreRiduzioneProvvigionale", m_ValoreRiduzioneProvvigionale);
                writer.WriteAttribute("PremioDaCessionario", m_PremioDaCessionario);
                writer.WriteAttribute("CapitaleFinanziato", m_CapitaleFinanziato);
                writer.WriteAttribute("ValoreProvvTAN", m_ProvvTAN);
                writer.WriteAttribute("ProvvCollab", m_ProvvCollab);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteAttribute("IDClienteXCollaboratore", IDClienteXCollaboratore);
                writer.WriteAttribute("DataCaricamento", m_DataCaricamento);
                base.XMLSerialize(writer);
                writer.WriteTag("Provvigioni", m_Provvigioni);
                writer.WriteTag("Provvigionale", Provvigionale);
                writer.WriteTag("Messages", m_Messages);
                writer.WriteTag("DettaglioRS", m_DettaglioRichiestaSconto);
                writer.WriteTag("DettaglioCS", m_DettaglioConfermaSconto);
                writer.WriteTag("Attributi", Attributi);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoOfferta":
                        {
                            m_StatoOfferta = (StatoOfferta)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "IDPreventivo":
                        {
                            m_PreventivoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "IDProfilo":
                        {
                            m_IDProfilo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProfilo":
                        {
                            m_NomeProfilo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDProdotto":
                        {
                            m_ProdottoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProdotto":
                        {
                            m_NomeProdotto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Calcolato":
                        {
                            m_Calcolato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Rata":
                        {
                            m_Rata = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Eta":
                        {
                            m_Eta = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Anzianita":
                        {
                            m_Anzianita = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDTabellaSpese":
                        {
                            m_TabellaSpeseID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaFinanziariaID":
                        {
                            m_TabellaFinanziariaRelID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeTabellaFinanziaria":
                        {
                            m_NomeTabellaFinanziaria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDTabellaAssicurativaID":
                        {
                            m_TabellaAssicurativaRelID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeTabellaAssicurativa":
                        {
                            m_NomeTabellaAssicurativa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Rappel":
                        {
                            m_ValoreRappel = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvMax":
                        {
                            m_ValoreProvvigioneMassima = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpreadBase":
                        {
                            m_ValoreSpreadBase = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Spread":
                        {
                            m_ValoreSpread = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Provvigionale":
                        {
                            m_Provvigionale = (CProvvigionale)fieldValue;
                            break;
                        }

                    case "UpFront":
                        {
                            m_ValoreUpFront = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Running":
                        {
                            m_ValoreRunning = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataNascita":
                        {
                            m_DataNascita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataAssunzione":
                        {
                            m_DataAssunzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "PremioVita":
                        {
                            m_PremioVita = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PremioImpiego":
                        {
                            m_PremioImpiego = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PremioCredito":
                        {
                            m_PremioCredito = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ImpostaSostitutiva":
                        {
                            m_ImpostaSostitutiva = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "OneriErariali":
                        {
                            m_OneriErariali = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoRicavo":
                        {
                            m_NettoRicavo = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CommissioniBancarie":
                        {
                            m_CommissioniBancarie = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Interessi":
                        {
                            m_Interessi = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Imposte":
                        {
                            m_Imposte = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpeseConvenzioni":
                        {
                            m_SpeseConvenzioni = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "AltreSpese":
                        {
                            m_AltreSpese = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rivalsa":
                        {
                            m_Rivalsa = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TEG":
                        {
                            m_TEG = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TEG_Max":
                        {
                            m_TEG_Max = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            m_TAEG = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG_Max":
                        {
                            m_TAEG_Max = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAN":
                        {
                            m_TAN = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataDecorrenza":
                        {
                            m_DataDecorrenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Sesso":
                        {
                            m_Sesso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ErrorCode":
                        {
                            m_ErrorCode = (ErrorCodes)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Messages":
                        {
                            this.m_Messages.Clear();
                            this.m_Messages.Append(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "CaricaAlMassimo":
                        {
                            m_CaricaAlMassimo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "OffertaLibera":
                        {
                            m_OffertaLibera = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "TipoCalcoloTAEG":
                        {
                            m_TipoCalcoloTAEG = (TEGCalcFlag)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCalcoloTEG":
                        {
                            m_TipoCalcoloTEG = (TEGCalcFlag)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDSupervisore":
                        {
                            m_IDSupervisore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeSupervisore":
                        {
                            m_NomeSupervisore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoRS":
                        {
                            m_MotivoRichiestaSconto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioRS":
                        {
                            m_DettaglioRichiestaSconto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoCS":
                        {
                            m_MotivoConfermaSconto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioCS":
                        {
                            m_DettaglioConfermaSconto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataCS":
                        {
                            m_DataConfermaSconto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDSchermata":
                        {
                            m_IDSchermata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (OffertaFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "LimiteRataMax":
                        {
                            m_LimiteRataMax = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LimiteRataNote":
                        {
                            m_LimiteRataNote = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreRiduzioneProvvigionale":
                        {
                            m_ValoreRiduzioneProvvigionale = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PremioDaCessionario":
                        {
                            m_PremioDaCessionario = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "CapitaleFinanziato":
                        {
                            m_CapitaleFinanziato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreProvvTAN":
                        {
                            m_ProvvTAN = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvCollab":
                        {
                            m_ProvvCollab = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Provvigioni":
                        {
                            m_Provvigioni = (CCQSPDProvvigioneXOffertaCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            if (m_Provvigioni is object)
                                m_Provvigioni.SetOfferta(this);
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDClienteXCollaboratore":
                        {
                            m_IDClienteXCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataCaricamento":
                        {
                            m_DataCaricamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
                return NumeroOfferta + NomeProdotto + " - " + NomeCessionario + " (" + Sistema.Formats.FormatValuta(Rata) + " x " + Sistema.Formats.FormatInteger(Durata) + " = " + Sistema.Formats.FormatValuta(MontanteLordo) + ")";
            }

            public string NumeroOfferta
            {
                get
                {
                    return Strings.Right("00000000" + DMD.Integers.Hex(ID), 8);
                }
            }

            protected internal virtual void SetPratica(CPraticaCQSPD value)
            {
                m_Pratica = value;
                m_IDPratica = DBUtils.GetID(value);
            }

            public decimal? CalcolaBaseML(CCollection<EstinzioneXEstintore> estinzioni)
            {
                Debug.Print("SimpleOffertaControl1.prototype.getBaseML");
                decimal? ml = MontanteLordo;
                Debug.Print("ml: " + Sistema.Formats.FormatValuta(ml));
                if (ml.HasValue == false)
                    return default;
                var tariffa = TabellaFinanziariaRel;
                var tipoCalcolo = TipoCalcoloProvvigioni.MONTANTE_LORDO;
                if (tariffa is object && tariffa.Tabella is object && tariffa.Tabella.Stato == ObjectStatus.OBJECT_VALID)
                {
                    tipoCalcolo = tariffa.Tabella.TipoCalcoloProvvigioni;
                }

                Debug.Print("tipoCalcolo: " + ((int)tipoCalcolo).ToString());
                switch (tipoCalcolo)
                {
                    case TipoCalcoloProvvigioni.MONTANTE_LORDO:
                        {
                            break;
                        }

                    case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI:
                        {
                            var sommaEstinzioni = getSommaDebitiResidui(estinzioni);
                            Debug.Print("sommaEstinzioni: " + Sistema.Formats.FormatValuta(sommaEstinzioni));
                            if (sommaEstinzioni.HasValue)
                            {
                                ml = ml.Value - sommaEstinzioni.Value;
                            }
                            else
                            {
                                return default;
                            }

                            break;
                        }

                    case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI1:
                        {
                            var sommaEstinzioni = getSommaDebitiResidui2(estinzioni);
                            Debug.Print("sommaEstinzioni: " + Sistema.Formats.FormatValuta(sommaEstinzioni));
                            if (sommaEstinzioni.HasValue)
                            {
                                ml = ml.Value - sommaEstinzioni.Value;
                            }
                            else
                            {
                                return default;
                            }

                            break;
                        }

                    case TipoCalcoloProvvigioni.FUNZIONE:
                        {
                            break;
                        }
                }

                Debug.Print("baseML: " + Sistema.Formats.FormatValuta(ml));
                return Maths.Max(0m, ml.Value);
            }

            private bool isValid(EstinzioneXEstintore est, string tipoContratto)
            {
                bool ret = est is object && est.Stato == ObjectStatus.OBJECT_VALID && est.NumeroQuoteResidue > 0;
                if (!ret)
                    return ret;
                switch (est.Tipo)
                {
                    case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO:
                    case TipoEstinzione.ESTINZIONE_CQP: // TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                        {
                            return string.IsNullOrEmpty(tipoContratto) || tipoContratto == "C";
                        }

                    case TipoEstinzione.ESTINZIONE_PRESTITODELEGA:
                        {
                            return string.IsNullOrEmpty(tipoContratto) || tipoContratto == "D";
                        }

                    default:
                        {
                            return false;
                        }
                }
            }

            public decimal getSommaProvvigioniA(CQSPDTipoSoggetto pagateA)
            {
                decimal sum = 0.0m;
                foreach (CCQSPDProvvigioneXOfferta item in Provvigioni)
                {
                    if (item.PagataDa != item.PagataA)
                    {
                        if (item.PagataA == pagateA && item.Valore.HasValue)
                            sum += (decimal)item.Valore.Value;

                        if (item.PagataDa == pagateA && item.Valore.HasValue)
                            sum += (decimal)item.Valore;
                    }
                }

                return sum;
            }

            private bool getSommaDebitiResidui_isValid(EstinzioneXEstintore est)
            {
                bool ret = est is object && est.Stato == ObjectStatus.OBJECT_VALID && est.NumeroQuoteResidue > 0;
                if (!ret)
                    return ret;
                switch (est.Tipo)
                {
                    case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO:
                    case TipoEstinzione.ESTINZIONE_PRESTITODELEGA:
                    case TipoEstinzione.ESTINZIONE_CQP:
                        {
                            // case TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                            return true;
                        }

                    default:
                        {
                            return false;
                        }
                }
            }

            // Public Function getSommaDebitiResidui(ByVal items As CCollection(Of EstinzioneXEstintore)) As Decimal?
            // Dim nomeCess As String = "" : If (Me.Cessionario IsNot Nothing) Then nomeCess = Strings.LCase(Me.Cessionario.Nome)
            // Dim tipoContratto As String = "" : If (Me.Prodotto IsNot Nothing) Then tipoContratto = Me.Prodotto.IdTipoContratto
            // If (tipoContratto = "") Then tipoContratto = "C"
            // Dim tp As TipoEstinzione = IIf(tipoContratto = "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA)
            // Dim dec As Date? = Me.DataCaricamento
            // If (Not dec.HasValue) Then dec = Me.DataDecorrenza
            // Dim sum As Decimal = 0
            // If (dec.HasValue = False) Then Return sum

            // For i As Integer = 0 To items.Count() - 1
            // Dim exi As EstinzioneXEstintore = items(i)
            // If (exi.Selezionata) Then
            // Dim est As CEstinzione = exi.Estinzione
            // Dim estTP As TipoEstinzione = est.Tipo
            // If (estTP = TipoEstinzione.ESTINZIONE_CQP) Then estTP = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO 'OrElse estTP = TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
            // Dim IsInterno As Boolean = nomeCess <> "" AndAlso Strings.Compare(nomeCess, nmCess, true) = 0

            // If (estTP = tp AndAlso isValid(est)) Then
            // Dim nRate As Integer = Maths.Max(0, Calendar.DateDiff(DateTimeInterval.Month, Calendar.GetLastMonthDay(dec.Value), exi.DataFine.Value))
            // If ((tp <> TipoEstinzione.ESTINZIONE_NO) AndAlso (estTP = tp) AndAlso IsInterno) Then
            // Dim DeltaML_AggiuntiRate As Integer = Formats.ToInteger(Finanziaria.Configuration.Overflow.GetValueInt("DeltaML_AggiuntiRate"))
            // nRate = nRate + DeltaML_AggiuntiRate
            // End If
            // Dim calculator As New CEstinzioneCalculator()
            // calculator.Rata = Formats.ToValuta(exi.Rata)
            // calculator.Durata = Formats.ToInteger(exi.Durata)
            // calculator.TAN = Formats.ToDouble(exi.TAN)
            // calculator.NumeroRateResidue = nRate
            // Dim Res As Decimal? = calculator.DebitoResiduo

            // If (Res.HasValue) Then sum += Maths.Max(0, Res.Value)
            // End If
            // End If
            // Next
            // Return sum
            // End Function

            public decimal? getSommaDebitiResidui(CCollection<EstinzioneXEstintore> items)
            {
                string nomeCess = DMD.Strings.LCase(NomeCessionario);
                string tipoContratto = "";
                if (Prodotto is object)
                    tipoContratto = Prodotto.IdTipoContratto;
                if (string.IsNullOrEmpty(tipoContratto))
                    tipoContratto = "C";
                TipoEstinzione tp;
                if (tipoContratto == "C")
                {
                    tp = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO;
                }
                else
                {
                    tp = TipoEstinzione.ESTINZIONE_PRESTITODELEGA;
                }

                // Me.m_Contratto = Strings.Trim(this.m_Contratto);


                decimal sum = 0m;
                // If (dec.HasValue = False) Then Return sum
                for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                {
                    var exi = items[i];
                    Debug.Print("exi: " + DBUtils.GetID(exi));
                    Debug.Print("exi.Selezionata: " + exi.Selezionata);
                    Debug.Print("exi.Parametro: " + exi.Parametro + " / " + tipoContratto);
                    if (exi.Selezionata && (string.IsNullOrEmpty(tipoContratto) || string.IsNullOrEmpty(exi.Parametro) || (exi.Parametro ?? "") == (tipoContratto ?? "")))
                    {
                        // Dim est As CEstinzione = exi.Estinzione
                        Debug.Print("est: " + DBUtils.GetID(exi));
                        if (isValid(exi, tipoContratto))
                        {
                            Debug.Print("Valid");
                            string nmCess = DMD.Strings.LCase(exi.NomeCessionario);
                            bool IsInterno = !string.IsNullOrEmpty(nomeCess) && DMD.Strings.Compare(nomeCess, nmCess, true) == 0;
                            DateTime? dec;
                            if (IsInterno)
                            {
                                dec = DataCaricamento;
                            }
                            else
                            {
                                dec = DataDecorrenza;
                            }

                            int nRate = (int)Maths.Max(0L, DMD.DateUtils.DateDiff(DateTimeInterval.Month, DMD.DateUtils.GetLastMonthDay(dec.Value), exi.DataFine.Value));
                            var estTP = exi.Tipo;
                            if (estTP == TipoEstinzione.ESTINZIONE_CQP)
                                estTP = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO; // OrElse estTP = TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                            if (tp != TipoEstinzione.ESTINZIONE_NO && estTP == tp && IsInterno)
                            {
                                int DeltaML_AggiuntiRate = Sistema.Formats.ToInteger(Configuration.GetValueInt("DeltaML_AggiuntiRate", 0));
                                nRate = nRate + DeltaML_AggiuntiRate;
                            }

                            var calculator = new CEstinzioneCalculator();
                            calculator.Rata = Sistema.Formats.ToValuta(exi.Rata);
                            calculator.Durata = Sistema.Formats.ToInteger(exi.Durata);
                            calculator.TAN = Sistema.Formats.ToDouble(exi.TAN);
                            calculator.NumeroRateResidue = nRate;
                            calculator.PenaleEstinzione = Sistema.Formats.ToDouble(exi.PenaleEstinzione);
                            var Res = calculator.DebitoResiduo;
                            if (Res.HasValue)
                            {
                                sum += Maths.Max(0m, Res.Value);
                            }
                        }
                    }
                }

                return sum;
            }

            public decimal? getSommaDebitiResidui2(CCollection<EstinzioneXEstintore> items)
            {
                string nomeCess = DMD.Strings.LCase(NomeCessionario);
                string tipoContratto = "";
                if (Prodotto is object)
                    tipoContratto = Prodotto.IdTipoContratto;
                if (string.IsNullOrEmpty(tipoContratto))
                    tipoContratto = "C";
                TipoEstinzione tp;
                if (tipoContratto == "C")
                {
                    tp = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO;
                }
                else
                {
                    tp = TipoEstinzione.ESTINZIONE_PRESTITODELEGA;
                }

                // Me.m_Contratto = Strings.Trim(this.m_Contratto);


                decimal sum = 0m;
                // If (dec.HasValue = False) Then Return sum
                for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                {
                    var exi = items[i];
                    Debug.Print("exi: " + DBUtils.GetID(exi));
                    Debug.Print("exi.Selezionata: " + exi.Selezionata);
                    Debug.Print("exi.Parametro: " + exi.Parametro + " / " + tipoContratto);
                    if (exi.Selezionata && (string.IsNullOrEmpty(tipoContratto) || string.IsNullOrEmpty(exi.Parametro) || (exi.Parametro ?? "") == (tipoContratto ?? "")))
                    {
                        Debug.Print("est: " + DBUtils.GetID(exi));
                        if (isValid(exi, ""))
                        {
                            Debug.Print("Valid");
                            string nmCess = DMD.Strings.LCase(exi.NomeCessionario);
                            bool IsInterno = !string.IsNullOrEmpty(nomeCess) && DMD.Strings.Compare(nomeCess, nmCess, true) == 0;
                            DateTime? dec;
                            if (IsInterno)
                            {
                                dec = DataCaricamento;
                            }
                            else
                            {
                                dec = DataDecorrenza;
                            }

                            int nRate = (int)Maths.Max(0L, DMD.DateUtils.DateDiff(DateTimeInterval.Month, DMD.DateUtils.GetLastMonthDay(dec.Value), exi.DataFine.Value));
                            var estTP = exi.Tipo;
                            if (estTP == TipoEstinzione.ESTINZIONE_CQP)
                                estTP = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO; // OrElse estTP = TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                            if (tp != TipoEstinzione.ESTINZIONE_NO && estTP == tp && IsInterno)
                            {
                                int DeltaML_AggiuntiRate = Sistema.Formats.ToInteger(Configuration.GetValueInt("DeltaML_AggiuntiRate", 0));
                                nRate = nRate + DeltaML_AggiuntiRate;
                            }

                            var calculator = new CEstinzioneCalculator();
                            calculator.Rata = Sistema.Formats.ToValuta(exi.Rata);
                            calculator.Durata = Sistema.Formats.ToInteger(exi.Durata);
                            calculator.TAN = Sistema.Formats.ToDouble(exi.TAN);
                            calculator.PenaleEstinzione = Sistema.Formats.ToDouble(exi.PenaleEstinzione);
                            calculator.NumeroRateResidue = nRate;
                            var Res = calculator.DebitoResiduo;
                            if (Res.HasValue)
                            {
                                sum += Maths.Max(0m, Res.Value);
                            }
                        }
                    }
                }

                return sum;
            }

            public double? CalcolaProvvTAN(CCollection<EstinzioneXEstintore> estinzioni)
            {
                var p = Prodotto;
                if (p is null)
                    return default;
                return p.CalcolaProvvigioneTAN(this, estinzioni);
            }

            public decimal? CalcolaProvvTANE(CCollection<EstinzioneXEstintore> estinzioni)
            {
                var tariffa = TabellaFinanziariaRel;
                var tCalcoloPTAN = TipoCalcoloProvvigioni.MONTANTE_LORDO;
                if (tariffa is object && tariffa.Tabella is object && tariffa.Tabella.Stato == ObjectStatus.OBJECT_VALID)
                {
                    tCalcoloPTAN = tariffa.Tabella.TipoCalcoloProvvTAN;
                }

                var pTAN = CalcolaProvvTAN(estinzioni);
                decimal? pTANE = default;
                var ml = CalcolaBaseML(estinzioni);
                decimal? mlPieno = MontanteLordo;
                switch (tCalcoloPTAN)
                {
                    case TipoCalcoloProvvigioni.MONTANTE_LORDO:
                        {
                            if (mlPieno.HasValue && pTAN.HasValue)
                            {
                                pTANE = (decimal?)(pTAN.Value * (double)mlPieno.Value / 100d);
                            }

                            break;
                        }

                    case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI:
                        {
                            if (ml.HasValue && pTAN.HasValue)
                            {
                                pTANE = (decimal?)(pTAN.Value * (double)ml.Value / 100d);
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                        // TipoCalcoloProvvigioni.FUNZIONE = 2048
                }

                return pTANE;
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

            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
                COffertaCQS o = (COffertaCQS)value;
                m_Provvigioni = new CCQSPDProvvigioneXOffertaCollection(this);
                foreach (CCQSPDProvvigioneXOfferta p in o.Provvigioni)
                {
                    var p1 = p.Duplicate();
                    Provvigioni.Add(p1);
                    if (DBUtils.GetID(this) != 0)
                        p1.Save();
                }
            }

            public void SincronizzaProvvigioni(CCollection<EstinzioneXEstintore> estinzioni)
            {
                var p = Prodotto;
                CGruppoProdotti gp = null;
                CCQSPDProvvigioneXOfferta provv;
                if (p is object)
                    gp = p.GruppoProdotti;
                if (gp is object)
                {
                    foreach (CCQSPDTipoProvvigione tp in gp.Provvigioni)
                    {
                        provv = Provvigioni.GetItemByTipoProvvigione(tp);
                        if (provv is null && tp.RispettaVincoli(this))
                        {
                            provv = new CCQSPDProvvigioneXOfferta();
                            provv.TipoProvvigione = tp;
                            provv.Nome = tp.Nome;
                            provv.TipoCalcolo = tp.TipoCalcolo;
                            provv.PagataDa = tp.PagataDa;
                            provv.PagataA = tp.PagataA;
                            provv.Fisso = tp.Fisso;
                            provv.Percentuale = tp.Percentuale;
                            provv.Formula = tp.Formula;
                            if (Sistema.TestFlag(tp.Flags, CQSPDTipoProvvigioneFlags.Nascosta))
                            {
                                provv.Flags = Sistema.SetFlag(provv.Flags, ProvvigioneXOffertaFlags.Privileged, true);
                            }

                            foreach (var v in tp.Vincoli)
                                provv.Vincoli.Add(v);
                            var keys = tp.Parameters.Keys;
                            foreach (string key in keys)
                                provv.Parameters.SetItemByKey(key, tp.Parameters.GetItemByKey(key));
                            Provvigioni.Add(provv);
                            provv.Stato = ObjectStatus.OBJECT_VALID;
                            provv.Save();
                        }
                    }
                }

                // Provvigioni collaboratore
                var col = Collaboratore;
                if (col is object)
                {
                    foreach (CTrattativaCollaboratore t in col.Trattative)
                    {
                        if (t.Stato == ObjectStatus.OBJECT_VALID && t.StatoTrattativa == StatoTrattativa.STATO_ACCETTATO && (t.IDProdotto == 0 || t.IDProdotto == ProdottoID))
                        {
                            provv = Provvigioni.GetItemByTrattativaCollaboratore(t);
                            if (provv is null) // AndAlso t.RispettaVincoli(Me)) Then
                            {
                                provv = new CCQSPDProvvigioneXOfferta();
                                provv.TrattativaCollaboratore = t;
                                provv.Collaboratore = col;
                                provv.Nome = t.Nome;
                                provv.TipoCalcolo = t.TipoCalcolo;
                                provv.PagataDa = CQSPDTipoSoggetto.Agenzia;
                                provv.PagataA = CQSPDTipoSoggetto.Collaboratore;
                                provv.Fisso = (decimal?)t.ValoreBase;
                                provv.Percentuale = t.SpreadApprovato;
                                provv.Formula = t.Formula;
                                if (Sistema.TestFlag(t.Flags, TrattativaCollaboratoreFlags.Nascosta))
                                {
                                    provv.Flags = Sistema.SetFlag(provv.Flags, ProvvigioneXOffertaFlags.Privileged, true);
                                }
                                // For Each v In tp.Vincoli()
                                // provv.Vincoli().Add(v)
                                // Next
                                var keys = t.Attributi.Keys;
                                foreach (string key in keys)
                                    provv.Parameters.SetItemByKey(key, t.Attributi.GetItemByKey(key));
                                Provvigioni.Add(provv);
                                provv.Stato = ObjectStatus.OBJECT_VALID;
                                provv.Save();
                            }
                        }
                    }
                }
            }

            public decimal? getBaseML(CCollection<EstinzioneXEstintore> estinzioni)
            {
                decimal? ml = MontanteLordo;
                var tariffa = TabellaFinanziariaRel;
                var tipoCalcolo = TipoCalcoloProvvigioni.MONTANTE_LORDO;
                if (tariffa is object && tariffa.Tabella is object && tariffa.Tabella.Stato == ObjectStatus.OBJECT_VALID)
                {
                    tipoCalcolo = tariffa.Tabella.TipoCalcoloProvvigioni;
                }

                switch (tipoCalcolo)
                {
                    case TipoCalcoloProvvigioni.MONTANTE_LORDO:
                        {
                            break;
                        }

                    case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI:
                        {
                            var sommaEstinzioni = getSommaDebitiResidui(estinzioni);
                            ml = ml - sommaEstinzioni;
                            break;
                        }

                    case TipoCalcoloProvvigioni.FUNZIONE:
                        {
                            break;
                        }
                }

                return Maths.Max(0, ml);
            }

            public object Clone()
            {
                COffertaCQS ret = (COffertaCQS)MemberwiseClone();
                ret.m_Provvigioni = null;
                return ret;
            }
        }
    }
}