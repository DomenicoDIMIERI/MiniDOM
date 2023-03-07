using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CEstinzione : Databases.DBObjectPO, ICloneable
        {
            private TipoEstinzione m_Tipo; // [INT]      Un valore intero che indica la tipologia di estinzione
            private int m_IDIstituto; // [INT]      ID dell'istituto con cui il cliente ha stipulato il contratto da estinguere
            [NonSerialized]
            private CCQSPDCessionarioClass m_Istituto; // [Object]   Oggetto istituto con cui il cliente ha stipulato il contratto
            private string m_NomeIstituto; // [TEXT]     Nome dell'istituto
            private string m_NomeAgenzia;
            private string m_NomeFiliale;
            private string m_Numero;
            private DateTime? m_DataInizio; // [Date]     Data di inizio del prestito
            private DateTime? m_Scadenza; // [Date]     Data di scadenza del prestito
            private decimal? m_Rata; // [Double]   
            private int? m_Durata; // [INT]      Durata in numero di rate
            private DateTime? m_DataEstinzione;
            private double? m_TAN; // [Double]    
            private double? m_TAEG; // TAEG
            private int? m_NumeroRateInsolute; // [int] Numero di rate insolute
            private int? m_NumeroRateDaPagare;
            private int? m_NumeroRatePagate;
            private decimal m_AbbuonoInteressi;
            private double? m_PenaleEstinzione;
            private decimal? m_SpeseAccessorie;
            private DateTime? m_DecorrenzaPratica;
            private bool m_Estinta;   // [Boolean]  Se vero indica che estingue questo prestito
            private double? m_Penale; // [Double] Percentuale da corrispondere come penale per estinzione anticipata
            private int m_IDPratica;          // [INT] ID della pratica che estingue questo prestito (eventuale)
            [NonSerialized]
            private CPraticaCQSPD m_Pratica;        // [CPraticaCQSPD] Pratica che estingue questo prestito (eventuale)
            private bool m_Calculated;
            private int m_IDPersona;
            [NonSerialized]
            private Anagrafica.CPersona m_Persona;
            private string m_NomePersona;
            private int m_IDEstintoDa;
            [NonSerialized]
            private CEstinzione m_EstintoDa;
            private string m_DettaglioStato;
            private string m_SourceType;  // Tipo dell'oggetto che ha generato questo record
            private int m_SourceID;   // ID dell'oggetto che ha generato questo record
            private string m_Note;
            private string m_TipoFonte;
            private int m_IDFonte;
            private IFonte m_Fonte;
            private string m_NomeFonte;
            private DateTime? m_DataAcquisizione;
            private bool m_Validato;     // Se vero indica che il prestito in corso è stato validato da un operatore
            private DateTime? m_ValidatoIl;     // Data e ora di validazione
            [NonSerialized]
            private Sistema.CUser m_ValidatoDa;     // Utente che ha validato l'operazione
            private int m_IDValidatoDa; // ID dell'utente che ha validato l'operazione
            private string m_NomeValidatoDa; // Nome dell'utente che ha validato l'operazione
            private string m_NomeSorgenteValidazione; // Nome del mezzo di validazione
            private string m_TipoSorgenteValidazione; // Tipo
            private int m_IDSorgenteValidazione; // ID 
            private int m_IDClienteXCollaboratore;
            [NonSerialized]
            private ClienteXCollaboratore m_ClienteXCollaboratore;

            // Private m_IDRichiestaConteggio As Integer                               'ID della richiesta di conteggio estintivo
            // <NonSerialized> Private m_RichiestaConteggio As CRichiestaConteggio     'Richiesta di conteggio estintivo

            public CEstinzione()
            {
                m_Tipo = TipoEstinzione.ESTINZIONE_NO;
                m_IDIstituto = 0;
                m_Istituto = null;
                m_NomeIstituto = "";
                m_NomeAgenzia = "";
                m_NomeFiliale = "";
                m_DataInizio = default;
                m_Scadenza = default;
                m_Rata = default;
                m_Durata = default;
                m_TAN = default;
                m_TAEG = default;
                m_Estinta = false;
                m_DataEstinzione = default;
                m_IDPratica = 0;
                m_Pratica = null;
                m_NumeroRatePagate = default;
                m_Calculated = false;
                m_AbbuonoInteressi = default;
                m_PenaleEstinzione = default;
                m_SpeseAccessorie = default;
                m_DecorrenzaPratica = default;
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_EstintoDa = null;
                m_IDEstintoDa = 0;
                m_DettaglioStato = "";
                m_SourceType = "";
                m_SourceID = 0;
                m_Numero = "";
                m_Note = "";
                m_TipoFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = "";
                m_DataAcquisizione = default;
                m_Validato = false;
                m_ValidatoIl = default;
                m_ValidatoDa = null;
                m_IDValidatoDa = 0;
                m_NomeValidatoDa = "";
                m_NomeSorgenteValidazione = "";
                m_TipoSorgenteValidazione = "";
                m_IDSorgenteValidazione = 0;
                m_IDClienteXCollaboratore = 0;
                m_ClienteXCollaboratore = null;

                // Me.m_IDRichiestaConteggio = 0
                // Me.m_RichiestaConteggio = Nothing
            }

            // ''' <summary>
            // ''' Restituisce o imposta l'ID della richiesta di conteggio estintivo
            // ''' </summary>
            // ''' <returns></returns>
            // Public Property IDRichiestaConteggio As Integer
            // Get
            // Return GetID(Me.m_RichiestaConteggio, Me.m_IDRichiestaConteggio)
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.IDRichiestaConteggio
            // If (oldValue = value) Then Return
            // Me.m_IDRichiestaConteggio = value
            // Me.m_RichiestaConteggio = Nothing
            // Me.DoChanged("IDRichiestaConteggio", value, oldValue)
            // End Set
            // End Property

            // ''' <summary>
            // ''' Restituisce o imposta la richiesta di conteggio estintivo associata
            // ''' </summary>
            // ''' <returns></returns>
            // Public Property RichiestaConteggio As CRichiestaConteggio
            // Get
            // If (Me.m_RichiestaConteggio Is Nothing) Then Me.m_RichiestaConteggio = Finanziaria.RichiesteConteggi.GetItemById(Me.m_IDRichiestaConteggio)
            // Return Me.m_RichiestaConteggio
            // End Get
            // Set(value As CRichiestaConteggio)
            // Dim oldValue As CRichiestaConteggio = Me.m_RichiestaConteggio
            // If (oldValue Is value) Then Return
            // Me.m_RichiestaConteggio = value
            // Me.m_IDRichiestaConteggio = GetID(value)
            // Me.DoChanged("RichiestaConteggio", value, oldValue)
            // End Set
            // End Property

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
                    m_ClienteXCollaboratore = value;
                    m_IDClienteXCollaboratore = DBUtils.GetID(value);
                    DoChanged("ClienteXCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il prestito è stato validato
        /// </summary>
        /// <returns></returns>
            public bool Validato
            {
                get
                {
                    return m_Validato;
                }

                set
                {
                    if (m_Validato == value)
                        return;
                    m_Validato = value;
                    DoChanged("Validato", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di validazione
        /// </summary>
        /// <returns></returns>
            public DateTime? ValidatoIl
            {
                get
                {
                    return m_ValidatoIl;
                }

                set
                {
                    var oldValue = m_ValidatoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_ValidatoIl = value;
                    DoChanged("ValidatoIl", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha validato l'operazione
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser ValidatoDa
            {
                get
                {
                    if (m_ValidatoDa is null)
                        m_ValidatoDa = Sistema.Users.GetItemById(m_IDValidatoDa);
                    return m_ValidatoDa;
                }

                set
                {
                    var oldValue = ValidatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ValidatoDa = value;
                    m_IDValidatoDa = DBUtils.GetID(value);
                    m_NomeValidatoDa = "";
                    if (value is object)
                        m_NomeValidatoDa = value.Nominativo;
                    DoChanged("ValidatoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha validato l'operazione
        /// </summary>
        /// <returns></returns>
            public int IDValidatoDa
            {
                get
                {
                    return DBUtils.GetID(m_ValidatoDa, m_IDValidatoDa);
                }

                set
                {
                    int oldValue = IDValidatoDa;
                    if (oldValue == value)
                        return;
                    m_ValidatoDa = null;
                    m_IDValidatoDa = value;
                    DoChanged("IDValidatoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha validato l'operazione in corso
        /// </summary>
        /// <returns></returns>
            public string NomeValidatoDa
            {
                get
                {
                    return m_NomeValidatoDa;
                }

                set
                {
                    string oldValue = m_NomeValidatoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeValidatoDa = value;
                    DoChanged("NomeValidatoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della sorgente da cui sono stati presi i dati per validare l'oggetto
        /// </summary>
        /// <returns></returns>
            public string NomeSorgenteValidazione
            {
                get
                {
                    return m_NomeSorgenteValidazione;
                }

                set
                {
                    string oldValue = m_NomeSorgenteValidazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSorgenteValidazione = value;
                    DoChanged("NomeSorgenteValidazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo della sorgente da cui sono stati validati i dati
        /// </summary>
        /// <returns></returns>
            public string TipoSorgenteValidazione
            {
                get
                {
                    return m_TipoSorgenteValidazione;
                }

                set
                {
                    string oldValue = m_TipoSorgenteValidazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoSorgenteValidazione = value;
                    DoChanged("TipoSorgenteValidazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della sorgente di validazione
        /// </summary>
        /// <returns></returns>
            public int IDSorgenteValidazione
            {
                get
                {
                    return m_IDSorgenteValidazione;
                }

                set
                {
                    int oldValue = m_IDSorgenteValidazione;
                    if (oldValue == value)
                        return;
                    m_IDSorgenteValidazione = value;
                    DoChanged("IDSorgenteValidazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo della fonte da cui si sono presi i dati del prestito in corso
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della fonte da cui si sono presi i dati del prestito in corso
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
                    m_IDFonte = value;
                    m_Fonte = null;
                    DoChanged("IDFonte", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la fonte
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
                    m_IDFonte = DBUtils.GetID((Databases.IDBObjectBase)value);
                    m_Fonte = value;
                    m_NomeFonte = "";
                    if (value is object)
                        m_NomeFonte = value.Nome;
                    DoChanged("Fonte", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della fonte
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
        /// Restituisce o imposta la data di acquistizione dei dati dalla fonte specificata
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
        /// Restituisce o imposta delle note aggiuntive sull'estinzione
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
        /// Restituisce o imposta il codice identificativo del prestito presso l'istituto erogante
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Numero
            {
                get
                {
                    return m_Numero;
                }

                set
                {
                    string oldValue = m_Numero;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Numero = value;
                    DoChanged("Numero", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo dell'oggetto che ha generato questo record
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string SourceType
            {
                get
                {
                    return m_SourceType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceType = value;
                    DoChanged("SourceType", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto che ha generato questo record
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return m_SourceID;
                }

                set
                {
                    int oldValue = m_SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che descrive lo stato dell'operazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 255);
                    string oldValue = m_DettaglioStato;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato = value;
                    DoChanged("DettaglioStato", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce l'ID dell'altro prestito che eventualmente estingue questo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDEstintoDa
            {
                get
                {
                    return DBUtils.GetID(m_EstintoDa, m_IDEstintoDa);
                }

                set
                {
                    int oldValue = IDEstintoDa;
                    if (oldValue == value)
                        return;
                    m_IDEstintoDa = value;
                    m_EstintoDa = null;
                    DoChanged("EstintoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'altro prestito che eventualmente estingue questo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CEstinzione EstintoDa
            {
                get
                {
                    if (m_EstintoDa is null)
                        m_EstintoDa = Estinzioni.GetItemById(m_IDEstintoDa);
                    return m_EstintoDa;
                }

                set
                {
                    var oldValue = m_EstintoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_EstintoDa = value;
                    m_IDEstintoDa = DBUtils.GetID(value);
                    DoChanged("EstintoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la data in cui è possibile rinnovare il prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataRinnovo
            {
                get
                {
                    DateTime? ret = default;
                    if (Tipo == TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO || Tipo == TipoEstinzione.ESTINZIONE_PRESTITODELEGA || Tipo == TipoEstinzione.ESTINZIONE_CQP)
                    {
                        // Dim percDurata As Double = minidom.Finanziaria.Configuration.PercCompletamentoRifn
                        if (m_DataInizio.HasValue && m_Durata.HasValue)
                        {
                            var mesiRinnovo = Estinzioni.getMeseRinnovo(m_Durata); // Maths.round(Me.m_Durata.Value * percDurata / 100)
                            if (mesiRinnovo.HasValue == false)
                                mesiRinnovo = (int?)Maths.round(m_Durata.Value * Configuration.PercCompletamentoRifn / 100d);
                            ret = DMD.DateUtils.DateAdd(DateTimeInterval.Month, mesiRinnovo.Value, DMD.DateUtils.GetMonthFirstDay(m_DataInizio));
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
        /// Restituisce la data in cui è possibile rinnovare il prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataRicontatto
            {
                get
                {
                    var ret = DataRinnovo;
                    if (ret.HasValue)
                    {
                        int giorniAnticipo = Configuration.GiorniAnticipoRifin;
                        ret = DMD.DateUtils.DateAdd(DateTimeInterval.Day, -giorniAnticipo, ret);
                    }

                    return ret;
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona a cui appartiene questo oggetto
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
        /// Restituisce o imposta la persona a cui appartiene questo oggetto
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
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta il nome della persona a cui appartiene l'oggetto
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            public void Validate()
            {
                if (m_Calculated == false)
                    Calculate();
            }

            public void Invalidate()
            {
                m_Calculated = false;
            }

            public override CModulesClass GetModule()
            {
                return Estinzioni.Module;
            }

            public DateTime? DecorrenzaPratica
            {
                get
                {
                    return m_DecorrenzaPratica;
                }

                set
                {
                    var oldValue = m_DecorrenzaPratica;
                    if (oldValue == value == true)
                        return;
                    m_DecorrenzaPratica = value;
                    DoChanged("DecorrenzaPratica", value, oldValue);
                }
            }

            public double? PenaleEstinzione
            {
                get
                {
                    return m_PenaleEstinzione;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("La penale di estinzione non può assumere un valore negativo");
                    var oldValue = m_PenaleEstinzione;
                    if (oldValue == value == true)
                        return;
                    m_PenaleEstinzione = value;
                    Invalidate();
                    DoChanged("PenaleEstinzione", value, oldValue);
                }
            }

            public decimal? SpeseAccessorie
            {
                get
                {
                    return m_SpeseAccessorie;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("Le spese accessorie non possono assumere un valore negativo");
                    var oldValue = m_SpeseAccessorie;
                    if (oldValue == value == true)
                        return;
                    m_SpeseAccessorie = value;
                    Invalidate();
                    DoChanged("SpeseAccessorie", value, oldValue);
                }
            }

            public int? NumeroRatePagate
            {
                get
                {
                    return m_NumeroRatePagate;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("Il numero di rate pagate non può essere negativo");
                    var oldValue = m_NumeroRatePagate;
                    if (oldValue == value == true)
                        return;
                    m_NumeroRatePagate = value;
                    Invalidate();
                    DoChanged("NumeroRatePagate", value, oldValue);
                }
            }

            public int? NumeroRateResidue
            {
                get
                {
                    return m_NumeroRateDaPagare;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("Il numero di rate residue non può essere negativo");
                    var oldValue = m_NumeroRateDaPagare;
                    if (oldValue == value == true)
                        return;
                    m_NumeroRateDaPagare = value;
                    Invalidate();
                    DoChanged("NumeroRateResidue", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di rate già maturate ma non  ancora pagate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int? NumeroRateInsolute
            {
                get
                {
                    return m_NumeroRateInsolute;
                }

                set
                {
                    if (value < 0 == true)
                        throw new ArgumentOutOfRangeException("Il numero di rate insolute non può essere negativo");
                    var oldValue = m_NumeroRateInsolute;
                    if (oldValue == value == true)
                        return;
                    m_NumeroRateInsolute = value;
                    DoChanged("NumeroRateInsolute", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica che estinge il prestito
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
                    m_Pratica = null;
                    m_IDPratica = value;
                    DoChanged("IDPratica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la pratica che estingue questo prestito
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
                    var oldValue = Pratica;
                    if (oldValue == value)
                        return;
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                    DoChanged("Pratica", value, oldValue);
                }
            }

            protected internal virtual void SetPratica(CPraticaCQSPD value)
            {
                m_Pratica = value;
                m_IDPratica = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il tipo del prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoEstinzione Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    var oldValue = m_Tipo;
                    if (oldValue == value)
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'istituto che ha erogato il prestito
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
        /// Restituisce o imposta l'istituto che ha erogato il prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDCessionarioClass Istituto
            {
                get
                {
                    if (m_Istituto is null)
                        m_Istituto = Cessionari.GetItemById(m_IDIstituto);
                    return m_Istituto;
                }

                set
                {
                    var oldValue = Istituto;
                    if (oldValue == value)
                        return;
                    m_Istituto = value;
                    m_IDIstituto = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeIstituto = value.Nome;
                    DoChanged("Istituto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'istituto che ha erogato il prestito
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
                    value = Strings.Trim(value);
                    string oldValue = m_NomeIstituto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIstituto = value;
                    DoChanged("NomeIstituto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'agenzia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeAgenzia
            {
                get
                {
                    return m_NomeAgenzia;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgenzia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgenzia = value;
                    DoChanged("NomeAgenzia", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della filiale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeFiliale
            {
                get
                {
                    return m_NomeFiliale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeFiliale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFiliale = value;
                    DoChanged("NomeFiliale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la data di decorrenza dell'altro prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    Invalidate();
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultima rata dell'altro prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? Scadenza
            {
                get
                {
                    return m_Scadenza;
                }

                set
                {
                    var oldValue = m_Scadenza;
                    if (oldValue == value == true)
                        return;
                    if (m_Scadenza == value == true)
                        return;
                    m_Scadenza = value;
                    Invalidate();
                    DoChanged("Scadenza", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore della rata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    var oldValue = m_Rata;
                    if (oldValue == value == true)
                        return;
                    m_Rata = value;
                    Invalidate();
                    DoChanged("Rata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata in mesi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int? Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    var oldValue = m_Durata;
                    if (oldValue == value == true)
                        return;
                    m_Durata = value;
                    Invalidate();
                    DoChanged("Durata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TAN
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    Invalidate();
                    DoChanged("TAN", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TAEG
        /// </summary>
        /// <returns></returns>
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

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il prestito è estinto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Estinta
            {
                get
                {
                    return m_Estinta;
                }

                set
                {
                    if (m_Estinta == value)
                        return;
                    m_Estinta = value;
                    DoChanged("Estinta", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui questo prestito è stato eventualmente estinto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataEstinzione
            {
                get
                {
                    return m_DataEstinzione;
                }

                set
                {
                    var oldValue = m_DataEstinzione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEstinzione = value;
                    DoChanged("DataEstinzione", value, oldValue);
                }
            }

            public decimal? DebitoIniziale
            {
                get
                {
                    if (m_Rata.HasValue)
                        return m_Rata.Value * m_Durata;
                    return default;
                }
                // Set(value As Decimal?)
                // Dim oldValue As Decimal? = Me.DebitoIniziale
                // If (oldValue = value) Then Exit Property
                // Me.m_Rata = value / Me.m_Durata
                // Me.DoChanged("DebitoIniziale", value, oldValue)
                // End Set
            }

            public decimal? DebitoResiduo
            {
                get
                {
                    if (m_NumeroRatePagate.HasValue)
                    {
                        return get_DebitoResiduo((int)m_NumeroRatePagate);
                    }
                    else
                    {
                        return default;
                    }
                }
                // Set(value As Decimal?)
                // Dim oldValue As Decimal? = Me.DebitoResiduo
                // If (oldValue = value) Then Exit Property
                // Me.m_NumeroRatePagate = (Me.DebitoIniziale - value) / Me.m_Rata.Value
                // Me.DoChanged("DebitoResiduo", value, oldValue)
                // End Set
            }

            public decimal? get_DebitoResiduo(int numeroRatePagate)
            {
                var ret = DebitoIniziale;
                if (ret.HasValue && m_Rata.HasValue && m_NumeroRatePagate.HasValue)
                {
                    ret = ret.Value - m_Rata.Value * m_NumeroRatePagate.Value;
                }

                return ret;
                // Set(value As Decimal?)
                // Dim oldValue As Decimal? = Me.DebitoResiduo
                // If (oldValue = value) Then Exit Property
                // Me.m_NumeroRatePagate = (Me.DebitoIniziale - value) / Me.m_Rata.Value
                // Me.DoChanged("DebitoResiduo", value, oldValue)
                // End Set
            }

            public decimal AbbuonoInteressi
            {
                get
                {
                    Validate();
                    return m_AbbuonoInteressi;
                }
            }

            /// <summary>
        /// Restituisce il capitale da rimborsare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? CapitaleDaRimborsare
            {
                get
                {
                    var ret = DebitoResiduo;
                    if (ret.HasValue)
                        ret = ret.Value - m_AbbuonoInteressi;
                    return ret;
                }
            }

            /// <summary>
        /// Restituisce il valore della penale di estinzione calcolata sul capitale da rimborsare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValorePenale
            {
                get
                {
                    var ret = CapitaleDaRimborsare;
                    if (ret.HasValue && m_PenaleEstinzione.HasValue)
                        ret = (decimal?)((double?)ret * m_PenaleEstinzione.Value / 100);
                    return ret;
                }
            }

            public decimal? TotaleDaRimborsare
            {
                get
                {
                    return Maths.Sum(new[] { CapitaleDaRimborsare, ValorePenale, SpeseAccessorie, ValoreQuoteInsolute });
                }
            }

            public decimal? get_TotaleDaRimborsare(int numeroQuoteScadute)
            {
                return Maths.Sum(new[] { CapitaleDaRimborsare, ValorePenale, SpeseAccessorie, ValoreQuoteInsolute });
            }

            public decimal? get_TotaleDaRimborsare(int numeroQuoteScadute, int numeroQuoteInsolute)
            {
                return Maths.Sum(new[] { CapitaleDaRimborsare, ValorePenale, SpeseAccessorie, ValoreQuoteInsolute });
            }

            public decimal? ValoreQuoteInsolute
            {
                get
                {
                    Validate();
                    if (m_Rata.HasValue && m_NumeroRateInsolute.HasValue)
                        return m_NumeroRateInsolute * m_Rata.Value;
                    return default;
                }
            }

            public bool Calculate()
            {
                // Dim Dk As Decimal   'Debito residuo per k=1..n-1
                decimal Ik; // Interesse in ciascun periodo k=1...n
                int k;
                double i;

                // Me.m_DebitoIniziale = Me.m_Rata.Value * Me.m_Durata
                // Me.m_DebitoResiduo = Me.m_DebitoIniziale - Me.m_Quota * Me.m_NumeroRatePagate

                i = (double)(m_TAN / 100 / 12);
                m_AbbuonoInteressi = 0m;
                if (m_NumeroRateDaPagare.HasValue && m_Rata.HasValue)
                {
                    var loopTo = m_NumeroRateDaPagare.Value;
                    for (k = 1; k <= loopTo; k++) // Me.m_NumeroRatePagate + 1 To Me.m_Durata
                    {
                        Ik = (decimal)((double)m_Rata.Value * (1d - 1d / Maths.Pow(1d + i, k))); // (Me.m_Durata - k + 1)))
                        m_AbbuonoInteressi = m_AbbuonoInteressi + Ik;
                    }
                }

                m_Calculated = true;
                return true;
            }


            // ---------------------------------------------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_Tipo", (int?)m_Tipo);
                writer.WriteAttribute("m_IDIstituto", IDIstituto);
                writer.WriteAttribute("m_NomeIstituto", m_NomeIstituto);
                writer.WriteAttribute("NomeFiliale", m_NomeFiliale);
                writer.WriteAttribute("m_DataInizio", m_DataInizio);
                writer.WriteAttribute("m_Scadenza", m_Scadenza);
                writer.WriteAttribute("m_Rata", m_Rata);
                writer.WriteAttribute("m_Durata", m_Durata);
                writer.WriteAttribute("m_TAN", m_TAN);
                writer.WriteAttribute("TAEG", m_TAEG);
                writer.WriteAttribute("Estinta", m_Estinta);
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("NumeroRatePagate", m_NumeroRatePagate);
                writer.WriteAttribute("NumeroRateResidue", m_NumeroRateDaPagare);
                writer.WriteAttribute("NumeroRateInsolute", m_NumeroRateInsolute);
                writer.WriteAttribute("Calculated", m_Calculated);
                writer.WriteAttribute("AbbuonoInteressi", m_AbbuonoInteressi);
                writer.WriteAttribute("PenaleEstinzione", m_PenaleEstinzione);
                writer.WriteAttribute("SpeseAccessorie", m_SpeseAccessorie);
                writer.WriteAttribute("DecorrenzaPratica", m_DecorrenzaPratica);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("IDEstintoDa", IDEstintoDa);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
                writer.WriteAttribute("SourceType", m_SourceType);
                writer.WriteAttribute("SourceID", m_SourceID);
                writer.WriteAttribute("Numero", m_Numero);
                writer.WriteAttribute("DataEstinzione", m_DataEstinzione);
                writer.WriteAttribute("NomeAgenzia", m_NomeAgenzia);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("NomeFonte", m_NomeFonte);
                writer.WriteAttribute("DataAcquisizione", m_DataAcquisizione);
                writer.WriteAttribute("Validato", m_Validato);
                writer.WriteAttribute("ValidatoIl", m_ValidatoIl);
                writer.WriteAttribute("IDValidatoDa", m_IDValidatoDa);
                writer.WriteAttribute("NomeValidatoDa", m_NomeValidatoDa);
                writer.WriteAttribute("NomeSorgenteValidazione", m_NomeSorgenteValidazione);
                writer.WriteAttribute("TipoSorgenteValidazione", m_TipoSorgenteValidazione);
                writer.WriteAttribute("IDSorgenteValidazione", IDSorgenteValidazione);
                writer.WriteAttribute("IDClienteXCollaboratore", IDClienteXCollaboratore);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_Tipo":
                        {
                            m_Tipo = (TipoEstinzione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_IDIstituto":
                        {
                            m_IDIstituto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_NomeIstituto":
                        {
                            m_NomeIstituto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeFiliale":
                        {
                            m_NomeFiliale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "m_Scadenza":
                        {
                            m_Scadenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "m_Rata":
                        {
                            m_Rata = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "m_Durata":
                        {
                            m_Durata = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_TAN":
                        {
                            m_TAN = DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            m_TAEG = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Estinta":
                        {
                            m_Estinta = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataEstinzione":
                        {
                            m_DataEstinzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroRateInsolute":
                        {
                            m_NumeroRateInsolute = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroRatePagate":
                        {
                            m_NumeroRatePagate = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Calculated":
                        {
                            m_Calculated = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "AbbuonoInteressi":
                        {
                            m_AbbuonoInteressi = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PenaleEstinzione":
                        {
                            m_PenaleEstinzione = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpeseAccessorie":
                        {
                            m_SpeseAccessorie = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DecorrenzaPratica":
                        {
                            m_DecorrenzaPratica = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDEstintoDa":
                        {
                            m_IDEstintoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroRateResidue":
                        {
                            m_NumeroRateDaPagare = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceType":
                        {
                            m_SourceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Numero":
                        {
                            m_Numero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeAgenzia":
                        {
                            m_NomeAgenzia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "DataAcquisizione":
                        {
                            m_DataAcquisizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Validato":
                        {
                            m_Validato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ValidatoIl":
                        {
                            m_ValidatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDValidatoDa":
                        {
                            m_IDValidatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeValidatoDa":
                        {
                            m_NomeValidatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeSorgenteValidazione":
                        {
                            m_NomeSorgenteValidazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoSorgenteValidazione":
                        {
                            m_TipoSorgenteValidazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDSorgenteValidazione":
                        {
                            m_IDSorgenteValidazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDClienteXCollaboratore":
                        {
                            m_IDClienteXCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string GetTableName()
            {
                return "tbl_Estinzioni";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Tipo = reader.Read("Tipo",  m_Tipo);
                m_IDIstituto = reader.Read("Istituto",  m_IDIstituto);
                m_NomeIstituto = reader.Read("NomeIstituto",  m_NomeIstituto);
                m_NomeFiliale = reader.Read("NomeFiliale",  m_NomeFiliale);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataEstinzione = reader.Read("DataEstinzione",  m_DataEstinzione);
                m_Scadenza = reader.Read("Scadenza",  m_Scadenza);
                m_Rata = reader.Read("Rata",  m_Rata);
                m_Durata = reader.Read("Durata",  m_Durata);
                m_TAN = reader.Read("TAN",  m_TAN);
                m_TAEG = reader.Read("TAEG",  m_TAEG);
                m_Estinta = reader.Read("Estingue",  m_Estinta);
                m_IDPratica = reader.Read("IDPratica",  m_IDPratica);
                m_NumeroRatePagate = reader.Read("NumeroRatePagate",  m_NumeroRatePagate);
                m_NumeroRateInsolute = reader.Read("NumeroRateResidue",  m_NumeroRateInsolute);
                m_Calculated = reader.Read("Calculated",  m_Calculated);
                m_AbbuonoInteressi = reader.Read("AbbuonoInteressi",  m_AbbuonoInteressi);
                m_PenaleEstinzione = reader.Read("PenaleEstinzione",  m_PenaleEstinzione);
                m_SpeseAccessorie = reader.Read("SpeseAccessorie",  m_SpeseAccessorie);
                m_DecorrenzaPratica = reader.Read("DecorrenzaPratica",  m_DecorrenzaPratica);
                m_IDPersona = reader.Read("IDPersona",  m_IDPersona);
                m_NomePersona = reader.Read("NomePersona",  m_NomePersona);
                m_IDEstintoDa = reader.Read("IDEstintoDa",  m_IDEstintoDa);
                m_NumeroRateDaPagare = reader.Read("NumeroRateDaPagare",  m_NumeroRateDaPagare);
                m_DettaglioStato = reader.Read("DettaglioStato",  m_DettaglioStato);
                m_SourceType = reader.Read("SourceType",  m_SourceType);
                m_SourceID = reader.Read("SourceID",  m_SourceID);
                m_Numero = reader.Read("Numero",  m_Numero);
                m_NomeAgenzia = reader.Read("NomeAgenzia",  m_NomeAgenzia);
                m_Note = reader.Read("Note",  m_Note);
                m_TipoFonte = reader.Read("TipoFonte",  m_TipoFonte);
                m_IDFonte = reader.Read("IDFonte",  m_IDFonte);
                m_NomeFonte = reader.Read("NomeFonte",  m_NomeFonte);
                m_DataAcquisizione = reader.Read("DataAcquisizione",  m_DataAcquisizione);
                m_Validato = reader.Read("Validato",  m_Validato);
                m_ValidatoIl = reader.Read("ValidatoIl",  m_ValidatoIl);
                m_IDValidatoDa = reader.Read("IDValidatoDa",  m_IDValidatoDa);
                m_NomeValidatoDa = reader.Read("NomeValidatoDa",  m_NomeValidatoDa);
                m_NomeSorgenteValidazione = reader.Read("NomeSorgenteValidazione",  m_NomeSorgenteValidazione);
                m_TipoSorgenteValidazione = reader.Read("TipoSorgenteValidazione",  m_TipoSorgenteValidazione);
                m_IDSorgenteValidazione = reader.Read("IDSorgenteValidazione",  m_IDSorgenteValidazione);
                m_IDClienteXCollaboratore = reader.Read("IDClienteXCollaboratore",  m_IDClienteXCollaboratore);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Tipo", m_Tipo);
                writer.Write("Istituto", IDIstituto);
                writer.Write("NomeIstituto", m_NomeIstituto);
                writer.Write("NomeFiliale", m_NomeFiliale);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("Scadenza", m_Scadenza);
                writer.Write("Rata", m_Rata);
                writer.Write("Durata", m_Durata);
                writer.Write("TAN", m_TAN);
                writer.Write("TAEG", m_TAEG);
                writer.Write("Estingue", m_Estinta);
                writer.Write("DataEstinzione", m_DataEstinzione);
                writer.Write("IDPratica", IDPratica);
                writer.Write("NumeroRatePagate", m_NumeroRatePagate);
                writer.Write("NumeroRateResidue", m_NumeroRateInsolute);
                writer.Write("Calculated", m_Calculated);
                writer.Write("AbbuonoInteressi", m_AbbuonoInteressi);
                writer.Write("PenaleEstinzione", m_PenaleEstinzione);
                writer.Write("SpeseAccessorie", m_SpeseAccessorie);
                writer.Write("DecorrenzaPratica", m_DecorrenzaPratica);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("IDEstintoDa", IDEstintoDa);
                writer.Write("NumeroRateDaPagare", m_NumeroRateDaPagare);
                writer.Write("DettaglioStato", m_DettaglioStato);
                writer.Write("SourceType", m_SourceType);
                writer.Write("SourceID", m_SourceID);
                writer.Write("Numero", m_Numero);
                writer.Write("NomeAgenzia", m_NomeAgenzia);
                writer.Write("Note", m_Note);
                writer.Write("DataRinnovo", DataRinnovo);
                writer.Write("DataRicontatto", DataRicontatto);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("NomeFonte", m_NomeFonte);
                writer.Write("DataAcquisizione", m_DataAcquisizione);
                writer.Write("Validato", m_Validato);
                writer.Write("ValidatoIl", m_ValidatoIl);
                writer.Write("IDValidatoDa", m_IDValidatoDa);
                writer.Write("NomeValidatoDa", m_NomeValidatoDa);
                writer.Write("NomeSorgenteValidazione", m_NomeSorgenteValidazione);
                writer.Write("TipoSorgenteValidazione", m_TipoSorgenteValidazione);
                writer.Write("IDSorgenteValidazione", IDSorgenteValidazione);
                writer.Write("IDClienteXCollaboratore", IDClienteXCollaboratore);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return Estinzioni.FormatTipo(Tipo) + ", Rata: " + Sistema.Formats.FormatValuta(Rata) + ", Rate residue: " + NumeroRateResidue;
            }

            public bool InCorso()
            {
                return !m_Estinta && InCorso(DMD.DateUtils.Now());
            }

            /// <summary>
        /// Restituisce vero se il questo prestito risultava in corso alla data indicata (cioè se la data è compresa tra l'inizio e la fine o, nel caso di prestiti estinti, alla data di scadenza
        /// </summary>
        /// <param name="allaData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool InCorso(DateTime allaData)
            {
                if (m_Estinta && m_DataEstinzione.HasValue)
                {
                    return DMD.DateUtils.CheckBetween(allaData, m_DataInizio, m_DataEstinzione);
                }
                else
                {
                    return DMD.DateUtils.CheckBetween(allaData, m_DataInizio, m_Scadenza);
                }
            }

            protected internal void SetPersona(Anagrafica.CPersona value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value);
                m_NomePersona = value.Nominativo;
            }

            protected override void OnCreate(SystemEvent e)
            {
                // Me.ProgrammaRicontatto()
                base.OnCreate(e);
                Estinzioni.doItemCreated(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                // Me.ProgrammaRicontatto()
                base.OnModified(e);
                Estinzioni.doItemModified(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                // Ricontatti.AnnullaRicontattoBySource(Me)
                Estinzioni.doItemDeleted(new ItemEventArgs(this));
            }

            /// <summary>
        /// Programma il ricontatto opportuno
        /// </summary>
        /// <remarks></remarks>
            public Anagrafica.CRicontatto ProgrammaRicontatto()
            {
                float percRinnovo;
                int giorniAnticipo;
                var dataRicontatto = DataRicontatto;
                var r = Anagrafica.Ricontatti.GetRicontattoBySource(this);
                if (r is object && r.StatoRicontatto == Anagrafica.StatoRicontatto.EFFETTUATO)
                    return r;
                if (IDPersona == 0 || Estinta)
                {
                    if (r is object)
                    {
                        r.DettaglioStato = "Annullato perché estinto da " + IDEstintoDa;
                        r.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                        r.Save();
                    }
                }
                else if ((NumeroRateResidue <= 0 || Scadenza.HasValue && Scadenza.Value < DMD.DateUtils.ToDay()) == true)
                {
                    if (r is object)
                    {
                        r.DettaglioStato = "Annullato perché scaduto il " + Sistema.Formats.FormatUserDate(Scadenza);
                        r.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                        r.Save();
                    }
                }
                else if (dataRicontatto.HasValue)
                {
                    percRinnovo = (float)Configuration.PercCompletamentoRifn;
                    giorniAnticipo = Configuration.GiorniAnticipoRifin;
                    // motivo = Formats.FormatInteger(percRinnovo) & "% del prestito " & Finanziaria.Estinzioni.FormatTipo(Me.m_Tipo) & " di " & Me.m_NomePersona
                    if (r is object)
                    {
                        r.DettaglioStato = "Riprogrammato in seguito alla modifica del " + Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now());
                    }
                    else if (Persona is object)
                    {
                        // r = Ricontatti.ProgrammaRicontatto(Me.Persona, dataRicontatto, motivo, TypeName(Me), GetID(Me), nomeLista, Me.Persona.PuntoOperativo, Nothing)
                        r = new Anagrafica.CRicontatto();
                        // item.PuntoOperativo = persona.PuntoOperativo
                        r.DettaglioStato = "Programmato";
                    }

                    r.DataPrevista = (DateTime)dataRicontatto;
                    r.Persona = Persona;
                    r.SourceName = DMD.RunTime.vbTypeName(this);
                    r.SourceParam = DBUtils.GetID(this).ToString();
                    r.Stato = ObjectStatus.OBJECT_VALID;
                    r.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO;
                    // r.NomeLista = ""
                    r.GiornataIntera = false;
                    r.Flags = Sistema.SetFlag(r.Flags, Anagrafica.RicontattoFlags.Reserved, true);
                    if (Persona is object)
                        r.PuntoOperativo = Persona.PuntoOperativo;
                    r.Operatore = Sistema.Users.CurrentUser;
                    r.AssegnatoA = null;
                    // r.Produttore = Anagrafica.Aziende.AziendaPrincipale
                    if (m_Tipo == TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO || m_Tipo == TipoEstinzione.ESTINZIONE_CQP)
                    {
                        r.Note = "RINNOVO CQS";
                    }
                    else if (m_Tipo == TipoEstinzione.ESTINZIONE_PRESTITODELEGA)
                    {
                        r.Note = "RINNOVO PD";
                    }

                    r.Save();
                }
                else if (r is object)
                {
                    r.DettaglioStato = "Annullato perché non si tratta di un prestito rinnovabile";
                    r.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                    r.Save();
                }

                return r;
            }

            public bool IsInCorso(DateTime allaData) // = function () {
            {
                if (m_DataEstinzione.HasValue)
                {
                    return DMD.DateUtils.CheckBetween(allaData, m_DataInizio, m_DataEstinzione);
                }
                else
                {
                    return DMD.DateUtils.CheckBetween(allaData, m_DataInizio, m_Scadenza);
                }
            }

            public bool IsInCorso() // = function () {
            {
                return IsInCorso(DMD.DateUtils.Now());
            }

            public bool IsInCorsoOFutura()
            {
                return IsInCorsoOFutura(DMD.DateUtils.Now());
            }

            public bool IsInCorsoOFutura(DateTime allaData)
            {
                const int TOLLERANZA_ESTINZIONE = 1; // anno
                var di = m_DataInizio;
                var df = m_Scadenza;
                if (df.HasValue == false && di.HasValue)
                    df = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd(DateTimeInterval.Month, 120d, di));
                if (di.HasValue)
                    di = DMD.DateUtils.DateAdd(DateTimeInterval.Year, -TOLLERANZA_ESTINZIONE, di);
                if (df.HasValue)
                    df = DMD.DateUtils.DateAdd(DateTimeInterval.Year, +TOLLERANZA_ESTINZIONE, df);
                if (m_Estinta && m_DataEstinzione.HasValue)
                    df = m_DataEstinzione;
                bool ret = DMD.DateUtils.CheckBetween(allaData, di, df);
                return ret;
            }

            public int? GetNumeroRateResidue()
            {
                if (m_Estinta)
                {
                    if (m_DataEstinzione.HasValue)
                    {
                        return GetNumeroRateResidue((DateTime)m_DataEstinzione);
                    }
                    else
                    {
                        return default;
                    }
                }
                else
                {
                    return GetNumeroRateResidue(DMD.DateUtils.Now());
                }
            }

            public int? GetNumeroRateResidue(DateTime al)
            {
                if (m_Scadenza.HasValue)
                {
                    return (int?)Maths.Max(0L, DMD.DateUtils.DateDiff("M", al, m_Scadenza.Value));
                }
                else
                {
                    return default;
                }
            }

            public override bool Equals(object obj)
            {
                if (!(obj is CEstinzione))
                    return false;
                CEstinzione e = (CEstinzione)obj;
                return (IDIstituto == e.IDIstituto && DMD.DateUtils.Compare(DataInizio, e.DataInizio) == 0 && Durata == e.Durata && IDPersona == e.IDPersona && Tipo == e.Tipo && Rata == e.Rata) == true;




            }

            /// <summary>
        /// Restituisce una copia dell'oggetto
        /// </summary>
        /// <returns></returns>
            object ICloneable.Clone()
            {
                return MemberwiseClone();
            }

            /// <summary>
        /// Restituisce una copia dell'oggetto
        /// </summary>
        /// <returns></returns>
            public CEstinzione Clone()
            {
                return Clone();
            }

            /// <summary>
        /// Calcola l'estinzione alla data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
            public decimal CalcolaEstinzioneAl(DateTime data)
            {
                var calculator = new CEstinzioneCalculator();
                calculator.Rata = Rata; // (this.getEstinzione().getRata());
                calculator.PenaleEstinzione = (double)PenaleEstinzione; // (this.getEstinzione().getPenaleEstinzione());
                calculator.TAN = (double)TAN; // (this.getEstinzione().getTAN());
                calculator.SpeseAccessorie = (decimal)SpeseAccessorie; // (this.m_Correzione);
                calculator.NumeroRateInsolute = (int)NumeroRateInsolute; // (this.m_NumeroQuoteInsolute);
                                                                         // If (Me.Scadenza.HasValue) Then
                calculator.Durata = (int)DMD.DateUtils.DateDiff("M", data, Scadenza.Value); // (this.m_NumeroQuoteResidue);
                                                                                                // ElseIf (Me.Durata.HasValue) Then
                                                                                                // calculator.Durata = DateUtils.DateDiff("M", data, Me.Scadenza) '(this.m_NumeroQuoteResidue);
                                                                                                // End If


                return calculator.TotaleDaRimborsare;
            }
        }
    }
}