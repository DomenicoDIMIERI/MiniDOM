/* TODO ERROR: Skipped DefineDirectiveTrivia */
using System;
using System.Collections;
using System.Linq;
using DMD;
using DMD.XML;
using minidom;
using static minidom.CustomerCalls;
using static minidom.Office;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoFinestraLavorazione : int
        {
            /// <summary>
        /// Indica che la finestra è attiva e che non è ancora stata iniziata la lavorazione prevista per il cliente (a partire dalla data di lavorabilità)
        /// </summary>
        /// <remarks></remarks>
            NonAperta = 0,

            /// <summary>
        /// Indica che la finestra è stata aperta ed il cliente è in lavorazione
        /// </summary>
        /// <remarks></remarks>
            Aperta = 1,

            /// <summary>
        /// Indica che la finestra è attiva e che il cliente è stato contattato
        /// </summary>
        /// <remarks></remarks>
            Contattato = 10,

            /// <summary>
        /// Indica che la finestra è attiva e che il cliente è interessato all'offerta
        /// </summary>
        /// <remarks></remarks>
            Interessato = 15,

            /// <summary>
        /// Indica che al cliente è stata proposta almeno un'offerta
        /// </summary>
        /// <remarks></remarks>
            Consulenza = 20,

            /// <summary>
        /// Indica che è stata caricata almeno una pratica per il cliente
        /// </summary>
        /// <remarks></remarks>
            Pratica = 25,

            /// <summary>
        /// Indica che la finestra è stata chiusa e non + più in lavorazione
        /// </summary>
        /// <remarks></remarks>
            Chiusa = 255
        }

        public enum StatoOfferteFL : int
        {
            /// <summary>
        /// Stato sconosciuto
        /// </summary>
        /// <remarks></remarks>
            Sconosciuto = 0,

            /// <summary>
        /// Pratica in lavorazione
        /// </summary>
        /// <remarks></remarks>
            InLavorazione = 1,

            /// <summary>
        /// Pratica liquidata
        /// </summary>
        /// <remarks></remarks>
            Liquidata = 2,

            /// <summary>
        /// Pratica rifiutata dal cliente
        /// </summary>
        /// <remarks></remarks>
            RifiutataCliente = 3,

            /// <summary>
        /// Pratica bocciata dal cessionario
        /// </summary>
        /// <remarks></remarks>
            BocciataCessionario = 4,

            /// <summary>
        /// Pratica bocciata dall'agenzia
        /// </summary>
        /// <remarks></remarks>
            BocciataAgenzia = 5,

            /// <summary>
        /// Pratica non fattibile
        /// </summary>
        /// <remarks></remarks>
            NonFattibile = 6
        }

        [Flags]
        public enum FinestraLavorazioneFlags : int
        {
            None = 0,

            /// <summary>
        /// Flag che indica che il sistema richiede l'attenzione del cliente
        /// </summary>
        /// <remarks></remarks>
            OnCliente = 1,

            /// <summary>
        /// Flag che indica che il sistema richiede l'attenzione dell'operatore CRM
        /// </summary>
        /// <remarks></remarks>
            OnOperatore = 2,

            /// <summary>
        /// Flag che indica che il sistema richiede l'attenzione del consulente
        /// </summary>
        /// <remarks></remarks>
            OnConsulente = 4,

            /// <summary>
        /// Flag che indica che il sistema richiede l'attenzione del broker
        /// </summary>
        /// <remarks></remarks>
            OnSubAgenzia = 8,

            /// <summary>
        /// Flag che indica che il sistema richiede l'attenzione dell'agenzia
        /// </summary>
        /// <remarks></remarks>
            OnAgenzia = 16,

            /// <summary>
        /// Flag che indica che il sistema richiede l'attenzione del cessionario
        /// </summary>
        /// <remarks></remarks>
            OnCessionario = 32,

            /// <summary>
        /// Indica che nella finestra è possibile proporre una cessione
        /// </summary>
        /// <remarks></remarks>
            Disponibile_CQS = 64,

            /// <summary>
        /// Indica che nella finestra è possibile proporre una delega
        /// </summary>
        /// <remarks></remarks>
            Disponibile_PD = 128,

            /// <summary>
        /// Indica che nella finestra è possibile proporre una integrazione alla cessione
        /// </summary>
        /// <remarks></remarks>
            Disponibile_CQSI = 256,

            /// <summary>
        /// Indica che nella finestra è possibile proporre una integrazione alla delega
        /// </summary>
        /// <remarks></remarks>
            Disponibile_PDI = 512,

            /// <summary>
        /// Vero se è stato richiesto un conteggio estintivo per il cliente prima o durante questa finestra di lavorazione
        /// </summary>
        /// <remarks></remarks>
            RichiestoCE = 1024,

            /// <summary>
        /// Vero se il cliente ha visitato o è stato visitato durante questa finestra di lavorazione
        /// </summary>
        /// <remarks></remarks>
            VisitaDurante = 2048,

            /// <summary>
        /// Flag inserito quando la finestra di lavorazione prevede il rinnovo di una pratica in corso
        /// </summary>
        /// <remarks></remarks>
            Rinnovo = 4096,

            /// <summary>
        /// Flags impostato quando la busta paga è stata ricevuta e l'operatore l'ha segnata come valida
        /// </summary>
        /// <remarks></remarks>
            BustaPagaValida = 8192,
            Ricalcola = 65536
        }

        [Serializable]
        public class FinestraLavorazione 
            : Databases.DBObjectPO, IComparable
        {
            private int m_IDCliente;          // ID del cliente
            private string m_NomeCliente;         // Nome del cliente
            [NonSerialized] private Anagrafica.CPersona m_Cliente;           // Cliente
            private string m_IconaCliente;        // Icona del cliente
            private int m_IDConsulente;
            private CConsulentePratica m_Consulente;

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            private DateTime? m_DataAttivazione;
            private string m_DettaglioStato;      // stato del cliente (testo)
            private string m_DettaglioStato1;     // dettaglio stato del cliente (testo)
            private int m_IDOperatoreRicontatto;  // ID dell'operatore a cui é assegnato il contatto (CRM)
            [NonSerialized] private Sistema.CUser m_OperatoreRicontatto;      // Operatore a cui é assegnato il contatto (CRM)
            private DateTime? m_DataRicontatto;   // data di ricontatto
            private string m_MotivoRicontatto;    // motivo del ricontatto
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            private StatoFinestraLavorazione m_StatoFinestra; // Stato della finestra
            private FinestraLavorazioneFlags m_Flags;               // Flags per questo oggetto
            private DateTime? m_DataUltimoAggiornamento;  // Data dell'ultimo aggiornamento
            private DateTime m_DataInizioLavorabilita;     // Data di inizio di lavorabilità (es. pratiche rinnovabili)
            private DateTime? m_DataFineLavorabilita; // Data di fine lavorabilità (indica la data oltre la quale non ha più senso lavorare questa finestra)
            private DateTime? m_DataInizioLavorazione; // Data di inizio della lavorazione
            private DateTime? m_DataFineLavorazione;  // Data di fine lavorazione

            // Private m_Documenti As CCollection(Of DocumentoCaricato)   'Elenco dei documenti richiesti
            private string m_DocumentiRichiestiStr;
            [NonSerialized] private CCollection<CDocumentoXGruppoProdotti> m_DocumentiRichiesti;   // Elenco dei documenti caricati
            private string m_MessaggiStr;
            private CCollection<FinestraLavorazioneMsg> m_Messaggi;
            private int m_IDContatto;                 // ID dell'ultima telefonata fatta
            [NonSerialized] private CContattoUtente m_Contatto;           // ultima telefonata fatta
            private StatoOfferteFL m_StatoContatto;       // esito dell'ultima telefonata fatta
            private DateTime? m_DataContatto;                 // data dell'ultima telefonata fatta
            private int m_IDPrimaVisita;              // ID della prima visita fatta
            [NonSerialized] private CVisita m_PrimaVisita;                // Prima visita fatta
            private StatoOfferteFL m_StatoPrimaVisita;    // Esito della prima visita
            private DateTime? m_DataPrimaVisita;              // Data della prima visita
            private int m_IDRichiestaFinanziamento;                   // ID della richiesta di finanziamento associata
            [NonSerialized] private CRichiestaFinanziamento m_RichiestaFinanziamento;     // Richiesta di finanziamento associata    
            private StatoOfferteFL m_StatoRichiestaFinanziamento;         // Stato della richiesta di finanziamento associata
            private DateTime? m_DataRichiestaFinanziamento;                   // data della richiesta di finanziamento associata
            private int m_IDBustaPaga;                                // ID dell'ultima busta paga caricata
            [NonSerialized] private Sistema.CAttachment m_BustaPaga;                              // ultima busta paga caricata     
            private DateTime? m_DataBustaPaga;                                // data dell'ultima busta paga caricata
            private StatoOfferteFL m_StatoBustaPaga;                      // stato di verifica dell'ultima busta paga
            private int m_IDRichiestaCertificato;                     // id del
            [NonSerialized] private RichiestaCERQ m_RichiestaCertificato;
            private DateTime? m_DataRichiestaCertificato;
            private StatoOfferteFL m_StatoRichiestaCertificato;
            private decimal? m_QuotaCedibile;                             // Valore della quota cedibile
            private int m_IDStudioDiFattibilita;
            [NonSerialized] private CQSPDConsulenza m_StudioDiFattibilita;
            private StatoOfferteFL m_StatoStudioDiFattibilita;
            private DateTime? m_DataStudioDiFattibilita;
            private int m_IDCQS;
            [NonSerialized] private CPraticaCQSPD m_CQS;
            private StatoOfferteFL m_StatoCQS;
            private DateTime? m_DataCQS;
            private int m_IDPD;
            [NonSerialized] private CPraticaCQSPD m_PD;
            private StatoOfferteFL m_StatoPD;
            private DateTime? m_DataPD;
            private int m_IDCQSI;
            [NonSerialized] private CPraticaCQSPD m_CQSI;
            private StatoOfferteFL m_StatoCQSI;
            private DateTime? m_DataCQSI;
            private int m_IDPDI;
            [NonSerialized] private CPraticaCQSPD m_PDI;
            private StatoOfferteFL m_StatoPDI;
            private DateTime? m_DataPDI;
            [NonSerialized] private CCollection<CEstinzione> m_AltriPrestiti;
            private DateTime? m_DataEsportazione;
            private DateTime? m_DataEsportazioneOk;
            private string m_TokenEsportazione;
            private string m_EsportatoVerso;
            private DateTime? m_DataImportazione;
            private int m_IDCollaboratore;
            [NonSerialized] private CCollaboratore m_Collaboratore;
            private string m_TipoFonte;
            private int m_IDFonte;
            private IFonte m_Fonte;
            private int m_IDConsulenza;
            [NonSerialized] private CQSPDStudioDiFattibilita m_Consulenza;
            [NonSerialized] private PratichePerFinestraLavorazione m_Pratiche;

            /// <summary>
            /// Costruttore
            /// </summary>
            public FinestraLavorazione()
            {
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_Cliente = null;
                m_IconaCliente = "";
                m_StatoFinestra = StatoFinestraLavorazione.NonAperta;
                m_Flags = FinestraLavorazioneFlags.None;
                m_DataInizioLavorabilita = DMD.DateUtils.Now();
                m_DataFineLavorabilita = default;
                m_DataInizioLavorazione = default;
                m_DocumentiRichiesti = null;
                m_Messaggi = null;
                m_IDPrimaVisita = 0;
                m_PrimaVisita = null;
                m_StatoPrimaVisita = StatoOfferteFL.Sconosciuto;
                m_DataPrimaVisita = default;
                m_IDRichiestaFinanziamento = 0;
                m_RichiestaFinanziamento = null;
                m_IDStudioDiFattibilita = 0;
                m_StudioDiFattibilita = null;
                m_IDCQS = 0;
                m_CQS = null;
                m_IDPD = 0;
                m_PD = null;
                m_IDCQSI = 0;
                m_CQSI = null;
                m_IDPDI = 0;
                m_PDI = null;
                m_StatoCQS = StatoOfferteFL.Sconosciuto;
                m_StatoPD = StatoOfferteFL.Sconosciuto;
                m_StatoCQSI = StatoOfferteFL.Sconosciuto;
                m_StatoPDI = StatoOfferteFL.Sconosciuto;
                m_DataUltimoAggiornamento = default;
                m_DataFineLavorazione = default;
                m_QuotaCedibile = default;
                m_IDBustaPaga = 0;
                m_BustaPaga = null;
                m_StatoStudioDiFattibilita = StatoOfferteFL.Sconosciuto;
                m_StatoRichiestaFinanziamento = StatoOfferteFL.Sconosciuto;
                m_IDContatto = 0;
                m_Contatto = null;
                m_StatoContatto = StatoOfferteFL.Sconosciuto;
                m_DataContatto = default;
                m_DataBustaPaga = default;
                m_StatoBustaPaga = StatoOfferteFL.Sconosciuto;
                m_IDRichiestaCertificato = 0;
                m_RichiestaCertificato = null;
                m_DataRichiestaCertificato = default;
                m_StatoRichiestaCertificato = StatoOfferteFL.Sconosciuto;
                m_AltriPrestiti = null;
                m_DataEsportazione = default;
                m_DataEsportazioneOk = default;
                m_TokenEsportazione = "";
                m_EsportatoVerso = "";
                m_DataRichiestaFinanziamento = default;
                m_DataStudioDiFattibilita = default;
                m_DataCQS = default;
                m_DataPD = default;
                m_DataCQSI = default;
                m_DataPDI = default;
                m_DataImportazione = default;

                /* TODO ERROR: Skipped IfDirectiveTrivia */
                m_DataAttivazione = default;
                m_DettaglioStato = "";
                m_DettaglioStato1 = "";
                m_IDOperatoreRicontatto = 0;
                m_OperatoreRicontatto = null;
                m_DataRicontatto = default;
                m_MotivoRicontatto = "";
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                m_IDConsulente = 0;
                m_Consulente = null;
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
                m_TipoFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_IDConsulenza = 0;
                m_Consulenza = null;
            }

            public bool Ricalcola
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, FinestraLavorazioneFlags.Ricalcola);
                }

                set
                {
                    if (Ricalcola == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, FinestraLavorazioneFlags.Ricalcola, value);
                    DoChanged("Ricalcola", value, !value);
                }
            }

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

            public CQSPDStudioDiFattibilita Consulenza
            {
                get
                {
                    if (m_Consulenza is null)
                        m_Consulenza = StudiDiFattibilita.GetItemById(m_IDConsulenza);
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

            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    m_Fonte = null;
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
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_TipoFonte, m_TipoFonte, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    var oldValue = Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDFonte = DBUtils.GetID((Databases.IDBObjectBase)value);
                    m_Fonte = value;
                    DoChanged("Fonte", value, oldValue);
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


            /* TODO ERROR: Skipped IfDirectiveTrivia */
            public DateTime? DataAttivazione
            {
                get
                {
                    return m_DataAttivazione;
                }

                set
                {
                    var oldValue = m_DataAttivazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAttivazione = value;
                    DoChanged("DataAttivazione", value, oldValue);
                }
            }

            public string DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioStato;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato = value;
                    DoChanged("DettaglioStato", value, oldValue);
                }
            }

            public string DettaglioStato1
            {
                get
                {
                    return m_DettaglioStato1;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioStato1;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato1 = value;
                    DoChanged("DettaglioStato1", value, oldValue);
                }
            }

            public int IDOperatoreRicontatto  // ID dell'operatore a cui é assegnato il contatto (CRM)
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreRicontatto, m_IDOperatoreRicontatto);
                }

                set
                {
                    int oldValue = IDOperatoreRicontatto;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreRicontatto = value;
                    m_OperatoreRicontatto = null;
                    DoChanged("IDOperatoreRicontatto", value, oldValue);
                }
            }

            public Sistema.CUser OperatoreRicontatto      // Operatore a cui é assegnato il contatto (CRM)
            {
                get
                {
                    if (m_OperatoreRicontatto is null)
                        m_OperatoreRicontatto = Sistema.Users.GetItemById(m_IDOperatoreRicontatto);
                    return m_OperatoreRicontatto;
                }

                set
                {
                    var oldValue = OperatoreRicontatto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OperatoreRicontatto = value;
                    m_IDOperatoreRicontatto = DBUtils.GetID(value);
                    DoChanged("OperatoreRicontatto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di ricontatto fissata.
        /// </summary>
        /// <returns></returns>
            public DateTime? DataRicontatto
            {
                get
                {
                    return m_DataRicontatto;
                }

                set
                {
                    var oldValue = m_DataRicontatto;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRicontatto = value;
                    DoChanged("DataRicontatto", value, oldValue);
                }
            }

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
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoRicontatto = value;
                    DoChanged("MotivoRicontatto", value, oldValue);
                }
            }



            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
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
                    m_Consulente = null;
                    m_IDConsulente = value;
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
                    var oldValue = Consulente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDConsulente = DBUtils.GetID(value);
                    m_Consulente = value;
                    DoChanged("Consulente", value, oldValue);
                }
            }

            public DateTime? DataProssimaFinestra
            {
                get
                {
                    var d = DMD.DateUtils.Max(DataRinnovoCQS, DataRinnovoPD);
                    var di = DataInizioLavorazione;
                    if (di.HasValue == false)
                        di = DataInizioLavorabilita;
                    // If (d.HasValue = False OrElse Calendar.Compare(d, di) <= 0) Then
                    // Dim minDurata As Integer? = Nothing
                    // For Each p As CEstinzione In Me.AltriPrestiti
                    // If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso p.Durata.HasValue AndAlso p.Durata.Value > 0 AndAlso p.IsInCorsoOFutura(di)) Then
                    // If (minDurata.HasValue) Then
                    // minDurata = Maths.Min(p.Durata.Value, minDurata.Value)
                    // Else
                    // minDurata = p.Durata
                    // End If
                    // End If
                    // Next

                    // If (minDurata.HasValue) Then
                    // minDurata = Finanziaria.Estinzioni.getMeseRinnovo(minDurata.Value)
                    // End If
                    // If (minDurata.HasValue = False) Then
                    // d = Calendar.DateAdd(DateTimeInterval.Year, 2, di)
                    // Else
                    // d = Calendar.DateAdd(DateTimeInterval.Month, minDurata.Value, di)
                    // End If
                    // End If
                    if (d.HasValue == false)
                        d = DMD.DateUtils.DateAdd(DateTimeInterval.Day, 2d, di.Value);
                    return DMD.DateUtils.GetYearFirstDay(d.Value);
                }
            }

            public DateTime? DataRinnovoCQS
            {
                get
                {
                    DateTime? ret = default;
                    var items = AltriPrestiti;
                    var dMin = DataInizioLavorazione;
                    if (dMin.HasValue == false)
                        dMin = DataInizioLavorabilita;
                    dMin = DMD.DateUtils.GetYearFirstDay(dMin.Value);
                    dMin = DMD.DateUtils.DateAdd(DateTimeInterval.Day, 1d, dMin.Value);
                    foreach (CEstinzione p in items)
                    {
                        if (p.Stato == ObjectStatus.OBJECT_VALID && (p.Tipo == TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO || p.Tipo == TipoEstinzione.ESTINZIONE_CQP || p.Tipo == TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO) && p.IsInCorsoOFutura((DateTime)dMin))
                        {
                            var dr = p.DataRinnovo;
                            if (dr.HasValue && DMD.DateUtils.Compare(dMin, dr) <= 0)
                            {
                                ret = DMD.DateUtils.Min(ret, dr);
                            }
                        }
                    }

                    if (ret.HasValue)
                    {
                        return DMD.DateUtils.GetMonthFirstDay(ret);
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            public DateTime? DataRinnovoPD
            {
                get
                {
                    if (Cliente.ImpiegoPrincipale.TipoRapporto == "H")
                        return default;
                    DateTime? ret = default;
                    var items = AltriPrestiti;
                    var dMin = DataInizioLavorazione;
                    if (dMin.HasValue == false)
                        dMin = DataInizioLavorabilita;
                    dMin = DMD.DateUtils.GetYearFirstDay(dMin.Value);
                    dMin = DMD.DateUtils.DateAdd(DateTimeInterval.Day, 1d, dMin.Value);
                    foreach (CEstinzione p in items)
                    {
                        if (p.Stato == ObjectStatus.OBJECT_VALID && p.Tipo == TipoEstinzione.ESTINZIONE_PRESTITODELEGA && p.IsInCorsoOFutura((DateTime)dMin))
                        {
                            var dr = p.DataRinnovo;
                            if (dr.HasValue && DMD.DateUtils.Compare(dMin, dr) <= 0)
                                ret = DMD.DateUtils.Min(ret, dr);
                        }
                    }

                    if (ret.HasValue == false && Sistema.TestFlag(Flags, FinestraLavorazioneFlags.Disponibile_PD))
                    {
                        ret = DataRinnovoCQS;
                        if (ret.HasValue)
                            ret = DMD.DateUtils.DateAdd(DateTimeInterval.Month, 6d, ret);
                    }

                    if (ret.HasValue)
                    {
                        return DMD.DateUtils.GetMonthFirstDay(ret);
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui per la prima volta la finestra è stata importata
        /// </summary>
        /// <returns></returns>
            public DateTime? DataImportazione
            {
                get
                {
                    return m_DataImportazione;
                }

                set
                {
                    var oldValue = m_DataImportazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataImportazione = value;
                    DoChanged("DataImportazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della prima visita (ricevuta o effettuata)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPrimaVisita
            {
                get
                {
                    return DBUtils.GetID(m_PrimaVisita, m_IDPrimaVisita);
                }

                set
                {
                    int oldValue = IDPrimaVisita;
                    if (oldValue == value)
                        return;
                    m_IDPrimaVisita = value;
                    m_PrimaVisita = null;
                    DoChanged("IDPrimaVisita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la prima visita (effettuata o ricavuta)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CVisita PrimaVisita
            {
                get
                {
                    if (m_PrimaVisita is null)
                        m_PrimaVisita = (CVisita) CustomerCalls.CRM.GetItemById(m_IDPrimaVisita);
                    return m_PrimaVisita;
                }

                set
                {
                    var oldValue = m_PrimaVisita;
                    if (object.ReferenceEquals(oldValue, value))
                        return;
                    m_PrimaVisita = value;
                    this.DoChanged("PrimaVisita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato della prima visita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoPrimaVisita
            {
                get
                {
                    return m_StatoPrimaVisita;
                }

                set
                {
                    var oldValue = m_StatoPrimaVisita;
                    if (oldValue == value)
                        return;
                    m_StatoPrimaVisita = value;
                    DoChanged("StatoPrimaVisita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data della prima visita
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataPrimaVisita
            {
                get
                {
                    return m_DataPrimaVisita;
                }

                set
                {
                    var oldValue = m_DataPrimaVisita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPrimaVisita = value;
                    DoChanged("DataPrimaVisita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data della richiesta di finanziamento associata alla finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataRichiestaFinanziamento
            {
                get
                {
                    return m_DataRichiestaFinanziamento;
                }

                set
                {
                    var oldValue = m_DataRichiestaFinanziamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiestaFinanziamento = value;
                    DoChanged("DataRichiestaFinanziamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dello studio di fattibilità associato alla finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataStudioDiFattibilita
            {
                get
                {
                    return m_DataStudioDiFattibilita;
                }

                set
                {
                    var oldValue = m_DataStudioDiFattibilita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataStudioDiFattibilita = value;
                    DoChanged("DataStudioDiFattibilita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dello stato attuale della pratica di CQS associata alla finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataCQS
            {
                get
                {
                    return m_DataCQS;
                }

                set
                {
                    var oldValue = m_DataCQS;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataCQS = value;
                    DoChanged("DataCQS", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dello stato attuale della pratica di PD associata alla finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataPD
            {
                get
                {
                    return m_DataPD;
                }

                set
                {
                    var oldValue = m_DataPD;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPD = value;
                    DoChanged("DataPD", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dello stato attuale della pratica di CQS (integrazione) associata alla finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataCQSI
            {
                get
                {
                    return m_DataCQSI;
                }

                set
                {
                    var oldValue = m_DataCQSI;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataCQSI = value;
                    DoChanged("DataCQSI", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dello stato attuale della pratica di PD (integrazione) associata alla finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataPDI
            {
                get
                {
                    return m_DataPDI;
                }

                set
                {
                    var oldValue = m_DataPDI;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPDI = value;
                    DoChanged("DataPDI", value, oldValue);
                }
            }



            /// <summary>
        /// Restituisce o imposta la data e l'ora in cui la finestra di elaborazione è stata esportata per la prima volta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataEsportazione
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
        /// Restituisce o imposta la data in cui è stata confermata l'esportazione
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta il nome token utilizzato per esportare l'anagrafica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TokenEsportazione
            {
                get
                {
                    return m_TokenEsportazione;
                }

                set
                {
                    string oldValue = m_TokenEsportazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TokenEsportazione = value;
                    DoChanged("TokenEsportazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del sito esterno verso cui è stato esportato il token
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string EsportatoVerso
            {
                get
                {
                    return m_EsportatoVerso;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_EsportatoVerso;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EsportatoVerso = value;
                    DoChanged("EsportatoVerso", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la collezione dei presti attivi nel periodo di validità della finestra
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CEstinzione> AltriPrestiti
            {
                get
                {
                    lock (this)
                    {
                        if (m_AltriPrestiti is null)
                        {
                            m_AltriPrestiti = new CCollection<CEstinzione>();
                            CCollection<CEstinzione> items = Estinzioni.GetEstinzioniByPersona(Cliente);
                            var di = DataInizioLavorazione;
                            if (di.HasValue == false)
                                di = DataInizioLavorabilita;
                            foreach (CEstinzione p in items)
                            {
                                if (p.IsInCorsoOFutura((DateTime)di))
                                {
                                    p.SetPersona(Cliente);
                                    m_AltriPrestiti.Add(p);
                                }
                            }
                        }

                        return m_AltriPrestiti;
                    }
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del clietne
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
        /// Restituisce o imposta il cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return (Anagrafica.CPersonaFisica)m_Cliente;
                }

                set
                {
                    Anagrafica.CPersonaFisica oldValue = (Anagrafica.CPersonaFisica)m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDCliente = DBUtils.GetID(value);
                    m_Cliente = value;
                    if (value is object)
                    {
                        m_NomeCliente = value.Nominativo;
                        m_IconaCliente = value.IconURL;
                        PuntoOperativo = value.PuntoOperativo;
                    }

                    DoChanged("Cliente", value, oldValue);
                }
            }

            protected internal virtual void SetCliente(Anagrafica.CPersona value)
            {
                m_Cliente = value;
                m_IDCliente = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il nome del cliente
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
        /// Restituisce o imposta l'icona del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IconaCliente
            {
                get
                {
                    return m_IconaCliente;
                }

                set
                {
                    string oldValue = m_IconaCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconaCliente = value;
                    DoChanged("IconaCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato della fienstra di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoFinestraLavorazione StatoFinestra
            {
                get
                {
                    return m_StatoFinestra;
                }

                set
                {
                    var oldValue = m_StatoFinestra;
                    if (oldValue == value)
                        return;
                    m_StatoFinestra = value;
                    DoChanged("StatoFinestra", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o i imposta dei flags aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public FinestraLavorazioneFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = (int)m_Flags;
                    if (oldValue == (int)value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data a partire dalla quale sarà possibile proporre una operazione al cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime DataInizioLavorabilita
            {
                get
                {
                    return m_DataInizioLavorabilita;
                }

                set
                {
                    var oldValue = m_DataInizioLavorabilita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioLavorabilita = value;
                    DoChanged("DataInizioLavorabilita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data oltre la quale non sarà più possibile proporre alcuna operazione al cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataFineLavorabilita
            {
                get
                {
                    return m_DataFineLavorabilita;
                }

                set
                {
                    var oldValue = m_DataFineLavorabilita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFineLavorabilita = value;
                    DoChanged("DataFineLavorabilita", value, oldValue);
                }
            }




            /// <summary>
        /// Restituisce o imposta la data di inizio lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizioLavorazione
            {
                get
                {
                    return m_DataInizioLavorazione;
                }

                set
                {
                    var oldValue = m_DataInizioLavorazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioLavorazione = value;
                    DoChanged("DataInizioLavorazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di fine lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataFineLavorazione
            {
                get
                {
                    return m_DataFineLavorazione;
                }

                set
                {
                    var oldValue = m_DataFineLavorazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFineLavorazione = value;
                    DoChanged("DataFineLavorazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultimo aggiornamento di stato
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

            public bool GetFlag(FinestraLavorazioneFlags flag)
            {
                return Sistema.TestFlag(m_Flags, flag);
            }

            public void SetFlag(FinestraLavorazioneFlags flag, bool value)
            {
                Flags = Sistema.SetFlag(Flags, flag, value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'ultimo contatto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDContatto
            {
                get
                {
                    return DBUtils.GetID(m_Contatto, m_IDContatto);
                }

                set
                {
                    int oldValue = IDContatto;
                    if (oldValue == value)
                        return;
                    m_Contatto = null;
                    m_IDContatto = value;
                    DoChanged("IDContatto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ultimo contatto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CContattoUtente Contatto
            {
                get
                {
                    if (m_Contatto is null)
                        m_Contatto = CustomerCalls.CRM.GetItemById(m_IDContatto);
                    return m_Contatto;
                }

                set
                {
                    var oldValue = m_Contatto;
                    if (object.ReferenceEquals(oldValue, value))
                        return;
                    m_Contatto = value;
                    m_IDContatto = DBUtils.GetID(value);
                    this.DoChanged("Contatto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultimo contatto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataContatto
            {
                get
                {
                    return m_DataContatto;
                }

                set
                {
                    var oldValue = m_DataContatto;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataContatto = value;
                    DoChanged("DataContatto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato del contatto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoContatto
            {
                get
                {
                    return m_StatoContatto;
                }

                set
                {
                    var oldValue = m_StatoContatto;
                    if (oldValue == value)
                        return;
                    m_StatoContatto = value;
                    DoChanged("StatoContatto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la collezione dei documenti che vengono richiesti al cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CDocumentoXGruppoProdotti> DocumentiRichiesti
            {
                get
                {
                    lock (this)
                    {
                        if (m_DocumentiRichiesti is null)
                        {
                            if (!string.IsNullOrEmpty(m_DocumentiRichiestiStr))
                            {
                                m_DocumentiRichiesti = GetDocs(m_DocumentiRichiestiStr);
                                m_DocumentiRichiestiStr = DMD.Strings.vbNullString;
                            }
                            else
                            {
                                m_DocumentiRichiesti = new CCollection<CDocumentoXGruppoProdotti>();
                            }
                        }

                        return m_DocumentiRichiesti;
                    }
                }
            }

            /// <summary>
        /// Restituisce la collezione dei messaggi inviati o ricevuti per questa finestra di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<FinestraLavorazioneMsg> Messaggi
            {
                get
                {
                    lock (this)
                    {
                        if (m_Messaggi is null)
                        {
                            if (!string.IsNullOrEmpty(m_MessaggiStr))
                            {
                                m_Messaggi = GetMessages(m_MessaggiStr);
                                m_MessaggiStr = DMD.Strings.vbNullString;
                            }
                            else
                            {
                                m_Messaggi = new CCollection<FinestraLavorazioneMsg>();
                            }
                        }

                        return m_Messaggi;
                    }
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'ultima richiesta di finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDRichiestaFinanziamento
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaFinanziamento, m_IDRichiestaFinanziamento);
                }

                set
                {
                    int oldValue = IDRichiestaFinanziamento;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaFinanziamento = value;
                    m_RichiestaFinanziamento = null;
                    DoChanged("IDRichiestaFinanziamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ultima richiesta di finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRichiestaFinanziamento RichiestaFinanziamento
            {
                get
                {
                    if (m_RichiestaFinanziamento is null)
                        m_RichiestaFinanziamento = RichiesteFinanziamento.GetItemById(m_IDRichiestaFinanziamento);
                    return m_RichiestaFinanziamento;
                }

                set
                {
                    var oldValue = m_RichiestaFinanziamento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaFinanziamento = value;
                    m_IDRichiestaFinanziamento = DBUtils.GetID(value);
                    DoChanged("RichiestaFinanziamento", value, oldValue);
                }
            }

            protected internal virtual void SetRichiestaFinanziamento(CRichiestaFinanziamento value)
            {
                m_RichiestaFinanziamento = value;
                m_IDRichiestaFinanziamento = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'ultimo studio di fattibilità
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

            /// <summary>
        /// Restituisce o imposta l'ultimo studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CQSPDConsulenza StudioDiFattibilita
            {
                get
                {
                    if (m_StudioDiFattibilita is null)
                        m_StudioDiFattibilita = Consulenze.GetItemById(m_IDStudioDiFattibilita);
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

            protected internal virtual void SetStudioDiFattibilita(CQSPDConsulenza value)
            {
                m_StudioDiFattibilita = value;
                m_IDStudioDiFattibilita = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica di CQS proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCQS
            {
                get
                {
                    return DBUtils.GetID(m_CQS, m_IDCQS);
                }

                set
                {
                    int oldValue = IDCQS;
                    if (oldValue == value)
                        return;
                    m_IDCQS = value;
                    m_CQS = null;
                    DoChanged("IDCQS", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la CQS proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD CQS
            {
                get
                {
                    if (m_CQS is null)
                        m_CQS = Pratiche.GetItemById(m_IDCQS);
                    return m_CQS;
                }

                set
                {
                    var oldValue = m_CQS;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CQS = value;
                    m_IDCQS = DBUtils.GetID(value);
                    DoChanged("CQS", value, oldValue);
                }
            }

            protected internal virtual void SetCQS(CPraticaCQSPD value)
            {
                m_CQS = value;
                m_IDCQS = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica di delega proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPD
            {
                get
                {
                    return DBUtils.GetID(m_PD, m_IDPD);
                }

                set
                {
                    int oldValue = IDPD;
                    if (oldValue == value)
                        return;
                    m_IDPD = value;
                    m_PD = null;
                    DoChanged("IDPD", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la pratica di delega corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD PD
            {
                get
                {
                    if (m_PD is null)
                        m_PD = Pratiche.GetItemById(m_IDPD);
                    return m_PD;
                }

                set
                {
                    var oldValue = m_PD;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PD = value;
                    m_IDPD = DBUtils.GetID(value);
                    DoChanged("PD", value, oldValue);
                }
            }

            protected internal virtual void SetPD(CPraticaCQSPD value)
            {
                m_PD = value;
                m_IDPD = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica di integrazione alla cessione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCQSI
            {
                get
                {
                    return DBUtils.GetID(m_CQSI, m_IDCQSI);
                }

                set
                {
                    int oldValue = IDCQSI;
                    if (oldValue == value)
                        return;
                    m_CQSI = null;
                    m_IDCQSI = value;
                    DoChanged("IDCQSI", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la pratica di integrazione alla cessione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD CQSI
            {
                get
                {
                    if (m_CQSI is null)
                        m_CQSI = Pratiche.GetItemById(m_IDCQSI);
                    return m_CQSI;
                }

                set
                {
                    var oldValue = m_CQSI;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CQSI = value;
                    m_IDCQSI = DBUtils.GetID(value);
                    DoChanged("CQSI", value, oldValue);
                }
            }

            protected internal virtual void SetCQSI(CPraticaCQSPD value)
            {
                m_CQSI = value;
                m_IDCQSI = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica di integrazione alla delega
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPDI
            {
                get
                {
                    return DBUtils.GetID(m_PDI, m_IDPDI);
                }

                set
                {
                    int oldValue = IDPDI;
                    if (oldValue == value)
                        return;
                    m_IDPDI = value;
                    m_PDI = null;
                    DoChanged("IDPDI", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la pratica di integrazione alla delega
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD PDI
            {
                get
                {
                    if (m_PDI is null)
                        m_PDI = Pratiche.GetItemById(m_IDPDI);
                    return m_PDI;
                }

                set
                {
                    var oldValue = m_PDI;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PDI = value;
                    m_IDPDI = DBUtils.GetID(value);
                    DoChanged("PDI", value, oldValue);
                }
            }

            protected internal virtual void SetPDI(CPraticaCQSPD value)
            {
                m_PDI = value;
                m_IDPDI = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta lo stato di lavorazione della pratica di cessione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoCQS
            {
                get
                {
                    return m_StatoCQS;
                }

                set
                {
                    var oldValue = m_StatoCQS;
                    if (oldValue == value)
                        return;
                    m_StatoCQS = value;
                    DoChanged("StatoCQS", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato di lavorazione della pratica di delegazione di pagamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoPD
            {
                get
                {
                    return m_StatoPD;
                }

                set
                {
                    var oldValue = m_StatoPD;
                    if (oldValue == value)
                        return;
                    m_StatoPD = value;
                    DoChanged("StatoPD", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato di lavorazione della pratica di integrazione alla cessione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoCQSI
            {
                get
                {
                    return m_StatoCQSI;
                }

                set
                {
                    var oldValue = m_StatoCQSI;
                    if (oldValue == value)
                        return;
                    m_StatoCQSI = value;
                    DoChanged("StatoCQSI", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato di lavorazione della pratica di integrazione alla delega
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoPDI
            {
                get
                {
                    return m_StatoPDI;
                }

                set
                {
                    var oldValue = m_StatoPDI;
                    if (oldValue == value)
                        return;
                    m_StatoPDI = value;
                    DoChanged("StatoPDI", value, oldValue);
                }
            }

            public StatoOfferteFL StatoRichiestaFinanziamento
            {
                get
                {
                    return m_StatoRichiestaFinanziamento;
                }

                set
                {
                    var oldValue = m_StatoRichiestaFinanziamento;
                    if (oldValue == value)
                        return;
                    m_StatoRichiestaFinanziamento = value;
                    DoChanged("StatoRichiestaFinanziamento", value, oldValue);
                }
            }

            public StatoOfferteFL StatoStudioFattibilita
            {
                get
                {
                    return m_StatoStudioDiFattibilita;
                }

                set
                {
                    var oldValue = m_StatoStudioDiFattibilita;
                    if (oldValue == value)
                        return;
                    m_StatoStudioDiFattibilita = value;
                    DoChanged("StatoStudioFattibilita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la quota cedibile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? QuotaCedibile
            {
                get
                {
                    return m_QuotaCedibile;
                }

                set
                {
                    var oldValue = m_QuotaCedibile;
                    if (oldValue == value == true)
                        return;
                    m_QuotaCedibile = value;
                    DoChanged("QuotaCedibile", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'ultima busta paga ricevuta durante la finestra di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDBustaPaga
            {
                get
                {
                    return DBUtils.GetID(m_BustaPaga, m_IDBustaPaga);
                }

                set
                {
                    int oldValue = IDBustaPaga;
                    if (oldValue == value)
                        return;
                    m_IDBustaPaga = value;
                    m_BustaPaga = null;
                    DoChanged("IDBustaPaga", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ultima busta paga ricevuta durante la finestra di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CAttachment BustaPaga
            {
                get
                {
                    if (m_BustaPaga is null)
                        m_BustaPaga = Sistema.Attachments.GetItemById(m_IDBustaPaga);
                    return m_BustaPaga;
                }

                set
                {
                    var oldValue = m_BustaPaga;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_BustaPaga = value;
                    m_IDBustaPaga = DBUtils.GetID(value);
                    DoChanged("IDBustaPaga", value, oldValue);
                }
            }

            public DateTime? DataBustaPaga
            {
                get
                {
                    return m_DataBustaPaga;
                }

                set
                {
                    var oldValue = m_DataBustaPaga;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataBustaPaga = value;
                    DoChanged("DataBustaPaga", value, oldValue);
                }
            }

            public StatoOfferteFL StatoBustaPaga
            {
                get
                {
                    return m_StatoBustaPaga;
                }

                set
                {
                    var oldValue = m_StatoBustaPaga;
                    if (oldValue == value)
                        return;
                    m_StatoBustaPaga = value;
                    DoChanged("StatoBustaPaga", value, oldValue);
                }
            }

            public int IDRichiestaCertificato
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaCertificato, m_IDRichiestaCertificato);
                }

                set
                {
                    int oldValue = IDRichiestaCertificato;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaCertificato = value;
                    m_RichiestaCertificato = null;
                    DoChanged("IDRichiestaCertificato", value, oldValue);
                }
            }

            public RichiestaCERQ RichiestaCertificato
            {
                get
                {
                    if (m_RichiestaCertificato is null)
                        m_RichiestaCertificato = Office.RichiesteCERQ.GetItemById(m_IDRichiestaCertificato);
                    return m_RichiestaCertificato;
                }

                set
                {
                    var oldValue = m_RichiestaCertificato;
                    if (object.ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaCertificato = value;
                    m_IDRichiestaCertificato = DBUtils.GetID(value);
                    this.DoChanged("RichiestaCertificato", value, oldValue);
                }
            }

            public DateTime? DataRichiestaCertificato
            {
                get
                {
                    return m_DataRichiestaCertificato;
                }

                set
                {
                    var oldValue = m_DataRichiestaCertificato;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiestaCertificato = value;
                    DoChanged("DataRichiestaCertificato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato della richiesta del certificato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoOfferteFL StatoRichiestaCertificato
            {
                get
                {
                    return m_StatoRichiestaCertificato;
                }

                set
                {
                    var oldValue = m_StatoRichiestaCertificato;
                    if (oldValue == value)
                        return;
                    m_StatoRichiestaCertificato = value;
                    DoChanged("StatoRichiestaCertificato", value, oldValue);
                }
            }

            private CCollection<CDocumentoXGruppoProdotti> GetDocs(string text)
            {
                var ret = new CCollection<CDocumentoXGruppoProdotti>();
                try
                {
                    ret.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(text));
                }
                catch (Exception ex)
                {
                    // ret = New CCollection(Of CDocumentoXGruppoProdotti)
                }

                return ret;
            }

            private CCollection<FinestraLavorazioneMsg> GetMessages(string text)
            {
                var ret = new CCollection<FinestraLavorazioneMsg>();
                try
                {
                    ret.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(text));
                    ret.Sort();
                }
                catch (Exception ex)
                {
                    // ret = New CCollection(Of FinestraLavorazioneMsg)
                }

                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return FinestreDiLavorazione.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDFinestreLavorazione";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente",  m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente",  m_NomeCliente);
                m_IconaCliente = reader.Read("IconaCliente",  m_IconaCliente);
                m_StatoFinestra = reader.Read("StatoFinestra",  m_StatoFinestra);
                m_Flags = reader.Read("Flags",  m_Flags);
                m_DataInizioLavorabilita = reader.Read("DataInizioLavorabilita",  m_DataInizioLavorabilita);
                m_DataFineLavorabilita = reader.Read("DataFineLavorabilita",  m_DataFineLavorabilita);
                m_DataInizioLavorazione = reader.Read("DataInizioLavorazione",  m_DataInizioLavorazione);
                m_IDRichiestaFinanziamento = reader.Read("IDRichiestaF",  m_IDRichiestaFinanziamento);
                m_IDStudioDiFattibilita = reader.Read("IDStudioF",  m_IDStudioDiFattibilita);
                m_IDCQS = reader.Read("IDCQS",  m_IDCQS);
                m_IDPD = reader.Read("IDPD",  m_IDPD);
                m_IDCQSI = reader.Read("IDCQSI",  m_IDCQSI);
                m_IDPDI = reader.Read("IDPDI",  m_IDPDI);
                m_StatoCQS = reader.Read("StatoCQS",  m_StatoCQS);
                m_StatoPD = reader.Read("StatoPD",  m_StatoPD);
                m_StatoCQSI = reader.Read("StatoCQSI",  m_StatoCQSI);
                m_StatoPDI = reader.Read("StatoPDI",  m_StatoPDI);
                m_DataFineLavorazione = reader.Read("DataFineLavorazione",  m_DataFineLavorazione);
                m_DataUltimoAggiornamento = reader.Read("DataUltimoAggiornamento",  m_DataUltimoAggiornamento);
                m_QuotaCedibile = reader.Read("QuotaCedibile",  m_QuotaCedibile);
                m_IDBustaPaga = reader.Read("IDBustaPaga",  m_IDBustaPaga);
                m_StatoRichiestaFinanziamento = reader.Read("StatoRichiestaF",  m_StatoRichiestaFinanziamento);
                m_StatoStudioDiFattibilita = reader.Read("StatoSF",  m_StatoStudioDiFattibilita);
                m_IDContatto = reader.Read("IDContatto",  m_IDContatto);
                m_StatoContatto = reader.Read("StatoContatto",  m_StatoContatto);
                m_DataContatto = reader.Read("DataContatto",  m_DataContatto);
                m_DataBustaPaga = reader.Read("DataBustaPaga",  m_DataBustaPaga);
                m_StatoBustaPaga = reader.Read("StatoBustaPaga",  m_StatoBustaPaga);
                m_IDRichiestaCertificato = reader.Read("IDRichiestaCertificato",  m_IDRichiestaCertificato);
                m_DataRichiestaCertificato = reader.Read("DataRichiestaCertificato",  m_DataRichiestaCertificato);
                m_StatoRichiestaCertificato = reader.Read("StatoRichiestaCertificato",  m_StatoRichiestaCertificato);
                m_DataEsportazione = reader.Read("DataEsportazione",  m_DataEsportazione);
                m_EsportatoVerso = reader.Read("EsportatoVerso",  m_EsportatoVerso);
                m_TokenEsportazione = reader.Read("TokenEsportazione",  m_TokenEsportazione);
                m_DataRichiestaFinanziamento = reader.Read("DataRichiestaFinanziamento", m_DataRichiestaFinanziamento);
                m_DataStudioDiFattibilita = reader.Read("DataStudioDiFattibilita", m_DataStudioDiFattibilita);
                m_DataCQS = reader.Read("DataCQS", m_DataCQS);
                m_DataPD = reader.Read("DataPD", m_DataPD);
                m_DataCQSI = reader.Read("DataCQSI", m_DataCQSI);
                m_DataPDI = reader.Read("DataPDI", m_DataPDI);
                m_IDPrimaVisita = reader.Read("IDPrimaVisita", m_IDPrimaVisita);
                m_StatoPrimaVisita = reader.Read("StatoPrimaVisita", m_StatoPrimaVisita);
                m_DataPrimaVisita = reader.Read("DataPrivaVisita", m_DataPrimaVisita);
                m_DataImportazione = reader.Read("DataImportazione", m_DataImportazione);
                m_DataEsportazioneOk = reader.Read("DataEsportazioneOk", m_DataEsportazioneOk);
                m_IDCollaboratore = reader.Read("IDCollaboratore", m_IDCollaboratore);
                m_DocumentiRichiesti = null;
                m_Messaggi = null;
                /* TODO ERROR: Skipped IfDirectiveTrivia */            // Me.m_DocumentiRichiesti = Me.GetDocs(reader.Read("DocumentiRichiesti", ""))
                string argvalue = "";
                m_DocumentiRichiestiStr = reader.Read("DocumentiRichiesti", argvalue);
                string argvalue1 = "";
                m_MessaggiStr = reader.Read("Messaggi", argvalue1); // Me.GetMessages(
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                /* TODO ERROR: Skipped IfDirectiveTrivia */
                m_DataAttivazione = reader.Read("DataAttivazione", m_DataAttivazione);
                m_DettaglioStato = reader.Read("DettaglioStato", m_DettaglioStato);
                m_DettaglioStato1 = reader.Read("DettaglioStato1", m_DettaglioStato1);
                m_IDOperatoreRicontatto = reader.Read("IDOpRicontatto", m_IDOperatoreRicontatto);
                m_DataRicontatto = reader.Read("DataRicontatto", m_DataRicontatto);
                m_MotivoRicontatto = reader.Read("MotivoRicontatto", m_MotivoRicontatto);
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                m_IDConsulente = reader.Read("IDConsulente", m_IDConsulente);
                m_TipoFonte = reader.Read("TipoFonte", m_TipoFonte);
                m_IDFonte = reader.Read("IDFonte", m_IDFonte);
                m_IDConsulenza = reader.Read("IDConsulenza", m_IDConsulenza);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IconaCliente", m_IconaCliente);
                writer.Write("StatoFinestra", m_StatoFinestra);
                writer.Write("Flags", m_Flags);
                writer.Write("DataInizioLavorabilita", m_DataInizioLavorabilita);
                writer.Write("DataInizioLavorabilitaStr", DBUtils.ToDBDateStr(m_DataInizioLavorabilita));
                writer.Write("DataFineLavorabilita", m_DataFineLavorabilita);
                writer.Write("DataInizioLavorazione", m_DataInizioLavorazione);
                /* TODO ERROR: Skipped IfDirectiveTrivia */
                if (m_DocumentiRichiesti is object)
                {
                    writer.Write("DocumentiRichiesti", DMD.XML.Utils.Serializer.Serialize(DocumentiRichiesti));
                }
                else
                {
                    writer.Write("DocumentiRichiesti", m_DocumentiRichiestiStr);
                }

                if (m_Messaggi is object)
                {
                    writer.Write("Messaggi", DMD.XML.Utils.Serializer.Serialize(Messaggi));
                }
                else
                {
                    writer.Write("Messaggi", m_MessaggiStr);
                }
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                writer.Write("IDRichiestaF", IDRichiestaFinanziamento);
                writer.Write("IDStudioF", IDStudioDiFattibilita);
                writer.Write("IDCQS", IDCQS);
                writer.Write("IDPD", IDPD);
                writer.Write("IDCQSI", IDCQSI);
                writer.Write("IDPDI", IDPDI);
                writer.Write("StatoCQS", m_StatoCQS);
                writer.Write("StatoPD", m_StatoPD);
                writer.Write("StatoCQSI", m_StatoCQSI);
                writer.Write("StatoPDI", m_StatoPDI);
                writer.Write("DataFineLavorazione", m_DataFineLavorazione);
                writer.Write("DataUltimoAggiornamento", m_DataUltimoAggiornamento);
                writer.Write("QuotaCedibile", m_QuotaCedibile);
                writer.Write("IDBustaPaga", IDBustaPaga);
                writer.Write("StatoRichiestaF", m_StatoRichiestaFinanziamento);
                writer.Write("StatoSF", m_StatoStudioDiFattibilita);
                writer.Write("IDContatto", IDContatto);
                writer.Write("StatoContatto", m_StatoContatto);
                writer.Write("DataContatto", m_DataContatto);
                writer.Write("DataContattoStr", DBUtils.ToDBDateStr(m_DataContatto));
                writer.Write("DataBustaPaga", m_DataBustaPaga);
                writer.Write("StatoBustaPaga", m_StatoBustaPaga);
                writer.Write("IDRichiestaCertificato", IDRichiestaCertificato);
                writer.Write("DataRichiestaCertificato", m_DataRichiestaCertificato);
                writer.Write("StatoRichiestaCertificato", m_StatoRichiestaCertificato);
                writer.Write("DataEsportazione", m_DataEsportazione);
                writer.Write("EsportatoVerso", m_EsportatoVerso);
                writer.Write("TokenEsportazione", m_TokenEsportazione);
                writer.Write("DataRichiestaFinanziamento", m_DataRichiestaFinanziamento);
                writer.Write("DataStudioDiFattibilita", m_DataStudioDiFattibilita);
                writer.Write("DataCQS", m_DataCQS);
                writer.Write("DataPD", m_DataPD);
                writer.Write("DataCQSI", m_DataCQSI);
                writer.Write("DataPDI", m_DataPDI);
                writer.Write("IDPrimaVisita", IDPrimaVisita);
                writer.Write("StatoPrimaVisita", m_StatoPrimaVisita);
                writer.Write("DataPrivaVisita", m_DataPrimaVisita);
                writer.Write("DataImportazione", m_DataImportazione);
                writer.Write("DataEsportazioneOk", m_DataEsportazioneOk);
                writer.Write("IDCollaboratore", IDCollaboratore);
                /* TODO ERROR: Skipped IfDirectiveTrivia */
                writer.Write("DataAttivazione", m_DataAttivazione);
                writer.Write("DettaglioStato", m_DettaglioStato);
                writer.Write("DettaglioStato1", m_DettaglioStato1);
                writer.Write("IDOpRicontatto", IDOperatoreRicontatto);
                writer.Write("DataRicontatto", m_DataRicontatto);
                writer.Write("MotivoRicontatto", m_MotivoRicontatto);
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                writer.Write("IDConsulente", IDConsulente);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("IDConsulenza", IDConsulenza);
                return base.SaveToRecordset(writer);
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

                    case "IconaCliente":
                        {
                            m_IconaCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoFinestra":
                        {
                            m_StatoFinestra = (StatoFinestraLavorazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (FinestraLavorazioneFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataInizioLavorabilita":
                        {
                            m_DataInizioLavorabilita = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFineLavorabilita":
                        {
                            m_DataFineLavorabilita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataInizioLavorazione":
                        {
                            m_DataInizioLavorazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDRichiestaF":
                        {
                            m_IDRichiestaFinanziamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDStudioF":
                        {
                            m_IDStudioDiFattibilita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCQS":
                        {
                            m_IDCQS = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPD":
                        {
                            m_IDPD = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCQSI":
                        {
                            m_IDCQSI = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPDI":
                        {
                            m_IDPDI = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoCQS":
                        {
                            m_StatoCQS = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoPD":
                        {
                            m_StatoPD = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoCQSI":
                        {
                            m_StatoCQSI = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoPDI":
                        {
                            m_StatoPDI = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataFineLavorazione":
                        {
                            m_DataFineLavorazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataUltimoAggiornamento":
                        {
                            m_DataUltimoAggiornamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DocumentiRichiesti":
                        {
                            m_DocumentiRichiesti = new CCollection<CDocumentoXGruppoProdotti>();
                            m_DocumentiRichiesti.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Messaggi":
                        {
                            m_Messaggi = new CCollection<FinestraLavorazioneMsg>();
                            m_Messaggi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "QuotaCedibile":
                        {
                            m_QuotaCedibile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDBustaPaga":
                        {
                            m_IDBustaPaga = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoRichiestaF":
                        {
                            m_StatoRichiestaFinanziamento = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoSF":
                        {
                            m_StatoStudioDiFattibilita = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDContatto":
                        {
                            m_IDContatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoContatto":
                        {
                            m_StatoContatto = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataContatto":
                        {
                            m_DataContatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataBustaPaga":
                        {
                            m_DataBustaPaga = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoBustaPaga":
                        {
                            m_StatoBustaPaga = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRichiestaCertificato":
                        {
                            m_IDRichiestaCertificato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRichiestaCertificato":
                        {
                            m_DataRichiestaCertificato = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoRichiestaCertificato":
                        {
                            m_StatoRichiestaCertificato = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataEsportazione":
                        {
                            m_DataEsportazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "EsportatoVerso":
                        {
                            m_EsportatoVerso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TokenEsportazione":
                        {
                            m_TokenEsportazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRichiestaFinanziamento":
                        {
                            m_DataRichiestaFinanziamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataStudioDiFattibilita":
                        {
                            m_DataStudioDiFattibilita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataCQS":
                        {
                            m_DataCQS = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataPD":
                        {
                            m_DataPD = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataCQSI":
                        {
                            m_DataCQSI = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataPDI":
                        {
                            m_DataPDI = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPrimaVisita":
                        {
                            m_IDPrimaVisita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoPrimaVisita":
                        {
                            m_StatoPrimaVisita = (StatoOfferteFL)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataPrivaVisita":
                        {
                            m_DataPrimaVisita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataImportazione":
                        {
                            m_DataImportazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataEsportazioneOk":
                        {
                            m_DataEsportazioneOk = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    /* TODO ERROR: Skipped IfDirectiveTrivia */
                    case "DataAttivazione":
                        {
                            m_DataAttivazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioStato1":
                        {
                            m_DettaglioStato1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOpRicontatto":
                        {
                            m_IDOperatoreRicontatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRicontatto":
                        {
                            m_DataRicontatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MotivoRicontatto":
                        {
                            m_MotivoRicontatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    case "IDConsulente":
                        {
                            m_IDConsulente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "IDConsulenza":
                        {
                            m_IDConsulenza = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IconaCliente", m_IconaCliente);
                writer.WriteAttribute("StatoFinestra", (int?)m_StatoFinestra);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("DataInizioLavorabilita", m_DataInizioLavorabilita);
                writer.WriteAttribute("DataFineLavorabilita", m_DataFineLavorabilita);
                writer.WriteAttribute("DataInizioLavorazione", m_DataInizioLavorazione);
                writer.WriteAttribute("IDRichiestaF", IDRichiestaFinanziamento);
                writer.WriteAttribute("IDStudioF", IDStudioDiFattibilita);
                writer.WriteAttribute("IDCQS", IDCQS);
                writer.WriteAttribute("IDPD", IDPD);
                writer.WriteAttribute("IDCQSI", IDCQSI);
                writer.WriteAttribute("IDPDI", IDPDI);
                writer.WriteAttribute("StatoCQS", (int?)m_StatoCQS);
                writer.WriteAttribute("StatoPD", (int?)m_StatoPD);
                writer.WriteAttribute("StatoCQSI", (int?)m_StatoCQSI);
                writer.WriteAttribute("StatoPDI", (int?)m_StatoPDI);
                writer.WriteAttribute("DataFineLavorazione", m_DataFineLavorazione);
                writer.WriteAttribute("DataUltimoAggiornamento", m_DataUltimoAggiornamento);
                writer.WriteAttribute("QuotaCedibile", m_QuotaCedibile);
                writer.WriteAttribute("IDBustaPaga", IDBustaPaga);
                writer.WriteAttribute("StatoRichiestaF", (int?)m_StatoRichiestaFinanziamento);
                writer.WriteAttribute("StatoSF", (int?)m_StatoStudioDiFattibilita);
                writer.WriteAttribute("IDContatto", IDContatto);
                writer.WriteAttribute("StatoContatto", (int?)m_StatoContatto);
                writer.WriteAttribute("DataContatto", m_DataContatto);
                writer.WriteAttribute("DataBustaPaga", m_DataBustaPaga);
                writer.WriteAttribute("StatoBustaPaga", (int?)m_StatoBustaPaga);
                writer.WriteAttribute("IDRichiestaCertificato", IDRichiestaCertificato);
                writer.WriteAttribute("DataRichiestaCertificato", m_DataRichiestaCertificato);
                writer.WriteAttribute("StatoRichiestaCertificato", (int?)m_StatoRichiestaCertificato);
                writer.WriteAttribute("DataEsportazione", m_DataEsportazione);
                writer.WriteAttribute("EsportatoVerso", m_EsportatoVerso);
                writer.WriteAttribute("TokenEsportazione", m_TokenEsportazione);
                writer.WriteAttribute("DataRichiestaFinanziamento", m_DataRichiestaFinanziamento);
                writer.WriteAttribute("DataStudioDiFattibilita", m_DataStudioDiFattibilita);
                writer.WriteAttribute("DataCQS", m_DataCQS);
                writer.WriteAttribute("DataPD", m_DataPD);
                writer.WriteAttribute("DataCQSI", m_DataCQSI);
                writer.WriteAttribute("DataPDI", m_DataPDI);
                writer.WriteAttribute("IDPrimaVisita", IDPrimaVisita);
                writer.WriteAttribute("StatoPrimaVisita", (int?)m_StatoPrimaVisita);
                writer.WriteAttribute("DataPrivaVisita", m_DataPrimaVisita);
                writer.WriteAttribute("DataImportazione", m_DataImportazione);
                writer.WriteAttribute("DataEsportazioneOk", m_DataEsportazioneOk);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                /* TODO ERROR: Skipped IfDirectiveTrivia */
                writer.WriteAttribute("DataAttivazione", m_DataAttivazione);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
                writer.WriteAttribute("DettaglioStato1", m_DettaglioStato1);
                writer.WriteAttribute("IDOpRicontatto", IDOperatoreRicontatto);
                writer.WriteAttribute("DataRicontatto", m_DataRicontatto);
                writer.WriteAttribute("MotivoRicontatto", m_MotivoRicontatto);
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("IDConsulenza", IDConsulenza);
                base.XMLSerialize(writer);
                writer.WriteTag("DocumentiRichiesti", DocumentiRichiesti);
                writer.WriteTag("Messaggi", Messaggi);
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                FinestreDiLavorazione.doItemCreated(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                FinestreDiLavorazione.doItemDeleted(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                FinestreDiLavorazione.doItemModified(new ItemEventArgs(this));
            }

            private bool IsBocciato(StatoOfferteFL stato)
            {
                switch (stato)
                {
                    case StatoOfferteFL.BocciataAgenzia:
                    case StatoOfferteFL.BocciataCessionario:
                    case StatoOfferteFL.NonFattibile:
                    case StatoOfferteFL.RifiutataCliente:
                        {
                            return true;
                        }

                    default:
                        {
                            return false;
                        }
                }
            }

            public void NotificaProposta(CQSPDConsulenza p)
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                if (DataInizioLavorazione.HasValue)
                {
                    if (!CheckBetween(p.DataConsulenza, DataInizioLavorazione, DataFineLavorazione))
                        return;
                }
                else if (!CheckBetween(p.DataConsulenza, DataInizioLavorabilita, DataFineLavorabilita))
                    return;
                int pid = DBUtils.GetID(p);
                if (IDStudioDiFattibilita == 0 || IsBocciato(StatoStudioFattibilita))
                {
                    StudioDiFattibilita = p;
                }

                if (StudioDiFattibilita.Stato == ObjectStatus.OBJECT_VALID)
                {
                    switch (StudioDiFattibilita.StatoConsulenza)
                    {
                        case StatiConsulenza.ACCETTATA:
                            {
                                StatoStudioFattibilita = StatoOfferteFL.Liquidata;
                                DataStudioDiFattibilita = StudioDiFattibilita.DataConferma;
                                break;
                            }

                        case StatiConsulenza.BOCCIATA:
                            {
                                StatoStudioFattibilita = StatoOfferteFL.BocciataAgenzia;
                                DataStudioDiFattibilita = StudioDiFattibilita.DataAnnullamento;
                                break;
                            }

                        case StatiConsulenza.NONFATTIBILE:
                            {
                                StatoStudioFattibilita = StatoOfferteFL.NonFattibile;
                                DataStudioDiFattibilita = StudioDiFattibilita.DataAnnullamento;
                                break;
                            }

                        case StatiConsulenza.RIFIUTATA:
                            {
                                StatoStudioFattibilita = StatoOfferteFL.RifiutataCliente;
                                DataStudioDiFattibilita = StudioDiFattibilita.DataConferma;
                                break;
                            }

                        default:
                            {
                                StatoStudioFattibilita = StatoOfferteFL.InLavorazione;
                                DataStudioDiFattibilita = StudioDiFattibilita.DataConsulenza;
                                break;
                            }
                    }
                }
                else
                {
                    StatoStudioFattibilita = StatoOfferteFL.Sconosciuto;
                }

                m_DataUltimoAggiornamento = DMD.DateUtils.Now();
                Save(true);
            }

            public void MergeWith(FinestraLavorazione w)
            {
                if (w is null)
                    throw new ArgumentNullException("w");
                if (ReferenceEquals(w, this))
                    return;
                if (DBUtils.GetID(w) == DBUtils.GetID(this))
                    return;
                if (m_IDCliente == 0)
                    m_IDCliente = w.IDCliente;
                if (string.IsNullOrEmpty(m_NomeCliente))
                    m_NomeCliente = w.m_NomeCliente;
                if (m_Cliente is null)
                    m_Cliente = w.m_Cliente;
                if (string.IsNullOrEmpty(m_IconaCliente))
                    m_IconaCliente = w.m_IconaCliente;
                if (m_QuotaCedibile.HasValue == false)
                    m_QuotaCedibile = w.m_QuotaCedibile;
                m_StatoFinestra = Maths.Max(m_StatoFinestra, w.m_StatoFinestra);
                m_Flags = m_Flags | w.m_Flags;
                m_DataInizioLavorabilita = (DateTime)DMD.DateUtils.Min(m_DataInizioLavorabilita, w.m_DataInizioLavorabilita);
                m_DataFineLavorabilita = DMD.DateUtils.Max(m_DataFineLavorabilita, w.m_DataFineLavorabilita);
                m_DataInizioLavorazione = DMD.DateUtils.Min(m_DataInizioLavorazione, w.m_DataInizioLavorazione);
                m_DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, w.m_DataUltimoAggiornamento);
                m_DataFineLavorazione = DMD.DateUtils.Max(m_DataFineLavorazione, w.m_DataFineLavorazione);
                m_DataImportazione = DMD.DateUtils.Min(m_DataImportazione, w.m_DataImportazione);
                m_DataEsportazioneOk = DMD.DateUtils.Min(m_DataEsportazioneOk, w.m_DataEsportazioneOk);
                DocumentiRichiesti.AddRange(w.DocumentiRichiesti);
                DocumentiRichiesti.Sort();
                Messaggi.AddRange(w.Messaggi);
                Messaggi.Sort();
                if (IDContatto == 0 || DMD.DateUtils.Compare(m_DataContatto, w.m_DataContatto) > 0)
                {
                    m_IDContatto = w.m_IDContatto;
                    m_Contatto = w.m_Contatto;
                    m_DataContatto = w.m_DataContatto;
                    m_StatoContatto = w.m_StatoContatto;
                }

                if (IDPrimaVisita == 0 || DMD.DateUtils.Compare(m_DataPrimaVisita, w.m_DataPrimaVisita) > 0)
                {
                    m_IDPrimaVisita = w.m_IDPrimaVisita;
                    m_PrimaVisita = w.m_PrimaVisita;
                    m_DataPrimaVisita = w.m_DataPrimaVisita;
                    m_StatoPrimaVisita = w.m_StatoPrimaVisita;
                }

                if (IDRichiestaFinanziamento == 0)
                {
                    m_IDRichiestaFinanziamento = w.m_IDRichiestaFinanziamento;
                    m_RichiestaFinanziamento = w.m_RichiestaFinanziamento;
                    m_StatoRichiestaFinanziamento = w.m_StatoRichiestaFinanziamento;
                    m_DataRichiestaFinanziamento = w.m_DataRichiestaFinanziamento;
                    if (RichiestaFinanziamento is object)
                    {
                        RichiestaFinanziamento.FinestraLavorazione = this;
                        RichiestaFinanziamento.Save();
                    }
                }

                if (IDBustaPaga == 0)
                {
                    m_IDBustaPaga = w.m_IDBustaPaga;
                    m_BustaPaga = w.m_BustaPaga;
                    m_DataBustaPaga = w.m_DataBustaPaga;
                    m_StatoBustaPaga = w.m_StatoBustaPaga;
                }

                if (IDRichiestaCertificato == 0)
                {
                    m_IDRichiestaCertificato = w.m_IDRichiestaCertificato;
                    m_RichiestaCertificato = w.m_RichiestaCertificato;
                    m_DataRichiestaCertificato = w.m_DataRichiestaCertificato;
                    m_StatoRichiestaCertificato = w.m_StatoRichiestaCertificato;
                }

                if (IDConsulenza == 0)
                {
                    m_IDConsulenza = w.m_IDConsulenza;
                    m_Consulenza = w.m_Consulenza;
                }

                if (IDCQS == 0)
                {
                    m_IDCQS = w.IDCQS;
                    m_CQS = w.m_CQS;
                    m_DataCQS = w.m_DataCQS;
                    m_StatoCQS = w.m_StatoCQS;
                    if (CQS is object)
                    {
                        CQS.FinestraLavorazione = this;
                        CQS.Save();
                    }
                }

                if (IDPD == 0)
                {
                    m_IDPD = w.IDPD;
                    m_PD = w.m_PD;
                    m_StatoPD = w.m_StatoPD;
                    m_DataPD = w.m_DataPD;
                    if (PD is object)
                    {
                        PD.FinestraLavorazione = this;
                        PD.Save();
                    }
                }

                if (IDCQSI == 0)
                {
                    m_IDCQSI = w.IDCQSI;
                    m_CQSI = w.m_CQSI;
                    m_StatoCQSI = w.m_StatoCQSI;
                    m_DataCQSI = w.m_DataCQSI;
                    if (CQSI is object)
                    {
                        CQSI.FinestraLavorazione = this;
                        CQSI.Save();
                    }
                }

                if (IDPDI == 0)
                {
                    m_IDPDI = w.IDPDI;
                    m_PDI = w.m_PDI;
                    m_StatoPDI = w.m_StatoPDI;
                    m_DataPDI = w.m_DataPDI;
                    if (PDI is object)
                    {
                        PDI.FinestraLavorazione = this;
                        PDI.Save();
                    }
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia */
                m_DataAttivazione = DMD.DateUtils.Min(m_DataAttivazione, w.m_DataAttivazione);
                if (string.IsNullOrEmpty(m_DettaglioStato))
                {
                    m_DettaglioStato = w.m_DettaglioStato;
                    m_DettaglioStato1 = w.m_DettaglioStato1;
                }

                if (IDOperatoreRicontatto == 0)
                {
                    m_IDOperatoreRicontatto = w.m_IDOperatoreRicontatto;
                    m_OperatoreRicontatto = w.m_OperatoreRicontatto;
                }

                if (DMD.DateUtils.Compare(m_DataRicontatto, w.m_DataRicontatto) > 0)
                {
                    m_DataRicontatto = w.m_DataRicontatto;
                    m_MotivoRicontatto = w.m_MotivoRicontatto;
                }
                /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (m_IDConsulente == 0)
                {
                    m_IDConsulente = w.m_IDConsulente;
                    m_Consulente = w.m_Consulente;
                }

                if (m_IDCollaboratore == 0)
                {
                    m_IDCollaboratore = w.m_IDCollaboratore;
                    m_Collaboratore = w.m_Collaboratore;
                }

                if (Fonte is null)
                {
                    m_TipoFonte = w.m_TipoFonte;
                    m_IDFonte = w.m_IDFonte;
                    m_Fonte = w.m_Fonte;
                }

                SetChanged(true);
            }

            public int CompareTo(FinestraLavorazione obj)
            {
                var arga = StatoFinestra;
                var argb = obj.StatoFinestra;
                int ret = DMD.Arrays.Compare(arga, argb);
                StatoFinestra = arga;
                obj.StatoFinestra = argb;
                var d1 = DataInizioLavorazione;
                if (d1.HasValue == false)
                    d1 = DataInizioLavorabilita;
                var d2 = obj.DataInizioLavorazione;
                if (d2.HasValue == false)
                    d2 = obj.DataInizioLavorabilita;
                // If (ret = 0) Then ret = Calendar.Compare(Me.DataInizioLavorabilita, obj.DataInizioLavorabilita)
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(d1, d2);
                if (ret == 0)
                    ret = DMD.Strings.Compare(NomeCliente, obj.NomeCliente, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((FinestraLavorazione)obj);
            }

            public bool MatchDate(DateTime d)
            {
                if (DataInizioLavorazione.HasValue)
                {
                    return CheckBetween(d, DataInizioLavorazione, DataFineLavorazione);
                }
                else
                {
                    return CheckBetween(d, DataInizioLavorabilita, DataFineLavorabilita);
                }
            }

            private bool CheckBetween(DateTime? d, DateTime? di, DateTime? df)
            {
                di = DMD.DateUtils.GetMonthFirstDay(di);
                df = DMD.DateUtils.GetLastSecond(DMD.DateUtils.GetLastMonthDay(df));
                return DMD.DateUtils.CheckBetween(d, di, df);
            }

            private DateTime GetDataCaricamento(CPraticaCQSPD p)
            {
                var ret = p.DataDecorrenza;
                foreach (CStatoLavorazionePratica s in p.StatiDiLavorazione)
                {
                    if (s.Data.HasValue && DMD.DateUtils.Compare(s.Data, ret) < 0)
                        ret = s.Data;
                }

                if (ret.HasValue == false)
                    ret = p.CreatoIl;
                return (DateTime)ret;
            }

            public void NotificaPratica(CPraticaCQSPD p)
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                var data = GetDataCaricamento(p);
                if (!MatchDate(data))
                    return;
                int pid = DBUtils.GetID(p);
                var statoo = StatoOfferteFL.InLavorazione;
                if (ReferenceEquals(p.StatoAttuale, StatiPratica.StatoLiquidato) || ReferenceEquals(p.StatoAttuale, StatiPratica.StatoArchiviato))
                {
                    statoo = StatoOfferteFL.Liquidata;
                }
                else if (ReferenceEquals(p.StatoAttuale, StatiPratica.StatoAnnullato))
                {
                    var rule = p.StatoDiLavorazioneAttuale.RegolaApplicata;
                    if (rule is object)
                    {
                        if (Sistema.TestFlag(rule.Flags, FlagsRegolaStatoPratica.DaCliente))
                        {
                            statoo = StatoOfferteFL.RifiutataCliente;
                        }
                        else if (Sistema.TestFlag(rule.Flags, FlagsRegolaStatoPratica.Bocciata))
                        {
                            statoo = StatoOfferteFL.BocciataAgenzia;
                        }
                        else
                        {
                            statoo = StatoOfferteFL.NonFattibile;
                        }
                    }
                    else
                    {
                        statoo = StatoOfferteFL.NonFattibile;
                    }
                }

                if (pid == IDCQS)
                {
                    StatoCQS = statoo;
                    DataCQS = data;
                }
                else if (pid == IDPD)
                {
                    StatoPD = statoo;
                    DataPD = data;
                }
                else if (pid == IDCQSI)
                {
                    StatoCQSI = statoo;
                    DataCQSI = data;
                }
                else if (pid == IDPDI)
                {
                    StatoPDI = statoo;
                    DataPDI = data;
                }

                m_DataUltimoAggiornamento = DMD.DateUtils.Now();
                // Me.Save(True)
            }

            public void NotificaContatto(CContattoUtente c)
            {
                if (c is null)
                    throw new ArgumentNullException("c");
                if (c.Stato != ObjectStatus.OBJECT_VALID)
                    return;
                if (DataInizioLavorazione.HasValue)
                {
                    if (!this.CheckBetween(c.Data, DataInizioLavorazione, DataFineLavorazione))
                        return;
                }
                else if (!this.CheckBetween(c.Data, DataInizioLavorabilita, DataFineLavorabilita))
                    return;
                if (StatoFinestra == StatoFinestraLavorazione.NonAperta)
                {
                    if (GetFlag(FinestraLavorazioneFlags.Rinnovo)) // AndAlso Me.CheckBetween(visita.Data, w.DataInizioLavorabilita, w.DataFineLavorabilita)) Then
                    {
                        StatoFinestra = StatoFinestraLavorazione.Aperta;
                        DataInizioLavorazione = DMD.DateUtils.Now();
                    }
                    // ElseIf w.StatoFinestra <> StatoFinestraLavorazione.Chiusa Then
                    // If (Me.CheckBetween(visita.Data, w.DataInizioLavorazione, w.DataFineLavorazione)) Then
                    // w.DataUltimoAggiornamento = Calendar.Now
                    // w.Save()
                    // End If
                }

                if (c is CVisita)
                {
                    if (m_DataPrimaVisita.HasValue == false || DMD.DateUtils.Compare(m_DataPrimaVisita, c.Data) > 0)
                    {
                        PrimaVisita = (CVisita)c;
                        DataPrimaVisita = c.Data;
                    }

                    switch (DMD.Strings.LCase(c.Persona.DettaglioEsito) ?? "")
                    {
                        case "bocciata":
                            {
                                StatoPrimaVisita = StatoOfferteFL.BocciataAgenzia;
                                break;
                            }

                        case "non fattibile":
                        case "irrintracciabile":
                            {
                                StatoPrimaVisita = StatoOfferteFL.NonFattibile;
                                break;
                            }

                        case "non interessato":
                        case "rifiutata dal cliente":
                        case "non contattare":
                            {
                                StatoPrimaVisita = StatoOfferteFL.RifiutataCliente;
                                break;
                            }

                        default:
                            {
                                StatoPrimaVisita = StatoOfferteFL.InLavorazione;
                                break;
                            }
                    }
                }
                else
                {
                    if (m_DataContatto.HasValue == false || DMD.DateUtils.Compare(m_DataContatto, c.Data) > 0)
                    {
                        Contatto = c;
                        DataContatto = c.Data;
                    }

                    switch (DMD.Strings.LCase(c.Persona.DettaglioEsito) ?? "")
                    {
                        case "bocciata":
                            {
                                StatoContatto = StatoOfferteFL.BocciataAgenzia;
                                break;
                            }

                        case "non fattibile":
                        case "irrintracciabile":
                            {
                                StatoContatto = StatoOfferteFL.NonFattibile;
                                break;
                            }

                        case "non interessato":
                        case "rifiutata dal cliente":
                        case "non contattare":
                            {
                                StatoContatto = StatoOfferteFL.RifiutataCliente;
                                break;
                            }

                        default:
                            {
                                StatoContatto = StatoOfferteFL.InLavorazione;
                                break;
                            }
                    }
                }

                if (Contatto is null)
                {
                    Contatto = PrimaVisita;
                    DataContatto = DataPrimaVisita;
                    StatoContatto = StatoPrimaVisita;
                }

                m_DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, c.Data);
                Save(true);
            }

            public void NotificaRichiesta(CRichiestaFinanziamento r)
            {
                if (r is null)
                    throw new ArgumentNullException("richiesta");
                if (DataInizioLavorazione.HasValue)
                {
                    if (!CheckBetween(r.Data, DataInizioLavorazione, DataFineLavorazione))
                        return;
                }
                else if (!CheckBetween(r.Data, DataFineLavorabilita, DataFineLavorabilita))
                    return;

                // If (GetID(Me) = r.IDFinestraLavorazione) Then Exit Sub
                var oldW = r.FinestraLavorazione;
                RichiestaFinanziamento = r;
                StatoRichiestaFinanziamento = StatoOfferteFL.Liquidata;
                DataUltimoAggiornamento = DMD.DateUtils.Now();
                DataRichiestaFinanziamento = r.Data;
                Save(true);
                if (oldW is object && DBUtils.GetID(oldW) != DBUtils.GetID(this))
                {
                    oldW.RichiestaFinanziamento = null;
                    oldW.StatoRichiestaFinanziamento = StatoOfferteFL.Sconosciuto;
                    oldW.DataRichiestaFinanziamento = default;
                    oldW.Save();
                }
            }

            private StatoOfferteFL GetMaxStatoPratica1(StatoOfferteFL a, StatoOfferteFL b)
            {
                return Maths.Max(a, b);
            }

            public StatoOfferteFL GetMaxStatoPratica()
            {
                var ret = m_StatoCQS;
                ret = GetMaxStatoPratica1(ret, m_StatoPD);
                ret = GetMaxStatoPratica1(ret, m_StatoCQSI);
                ret = GetMaxStatoPratica1(ret, m_StatoPDI);
                return ret;
            }

            /// <summary>
        /// Restituisce la data di rinnovo della cessione con scadenza più recente
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? GetDataRinnovoCQS()
            {
                DateTime? ret = default;
                foreach (CEstinzione p in AltriPrestiti)
                {
                    if ((p.Tipo == TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO || p.Tipo == TipoEstinzione.ESTINZIONE_CQP) && p.Estinta == false)
                    {
                        var dr = p.DataRinnovo;
                        if (dr.HasValue) // AndAlso Calendar.Compare(Me.DataInizioLavorabilita, dr) > 0) Then
                        {
                            ret = DMD.DateUtils.Min(ret, dr);
                        }
                    }
                }

                return ret;
            }

            /// <summary>
        /// Restituisce la data di rinnovo della delega con scadenza più recente
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? GetDataRinnovoPD()
            {
                DateTime? ret = default;
                foreach (CEstinzione p in AltriPrestiti)
                {
                    if (p.Tipo == TipoEstinzione.ESTINZIONE_PRESTITODELEGA && p.Estinta == false)
                    {
                        var dr = p.DataRinnovo;
                        if (dr.HasValue) // AndAlso Calendar.Compare(Me.DataInizioLavorabilita, dr) > 0) Then
                        {
                            ret = DMD.DateUtils.Min(ret, dr);
                        }
                    }
                }

                return ret;
            }

            protected internal void SetAltriPrestiti(CCollection<CEstinzione> items)
            {
                m_AltriPrestiti = items;
            }

            public CImportExport Esporta(CImportExportSource src)
            {
                Anagrafica.CPersona p = Cliente;
                var ie = new CImportExport();
                ie.Esportazione = true;
                ie.PersonaEsportata = p;
                ie.DataEsportazione = DMD.DateUtils.Now();
                ie.EsportataDa = Sistema.Users.CurrentUser;
                ie.FinestraLavorazioneEsportata = this;
                ie.AltriPrestiti.AddRange(AltriPrestiti);
                ie.Documenti.AddRange(p.Attachments);
                ie.PuntoOperativo = p.PuntoOperativo;
                if (RichiestaFinanziamento is object)
                    ie.RichiesteFinanziamento.Add(RichiestaFinanziamento);
                if (StudioDiFattibilita is object)
                    ie.Consulenze.Add(StudioDiFattibilita);
                if (CQS is object)
                    ie.Pratiche.Add(CQS);
                if (PD is object)
                    ie.Pratiche.Add(PD);
                if (CQSI is object)
                    ie.Pratiche.Add(CQSI);
                if (PDI is object)
                    ie.Pratiche.Add(PDI);
                ie.StatoRemoto = StatoEsportazione.NonEsportato;
                ie.Source = src;
                ie.Stato = ObjectStatus.OBJECT_VALID;
                ie.Esporta();
                if (ie.StatoRemoto != StatoEsportazione.Esportato)
                    throw new Exception(ie.DettaglioStatoRemoto);
                DataEsportazione = ie.DataEsportazione;
                TokenEsportazione = ie.SharedKey;
                Save();
                return ie;
            }
        }
    }
}