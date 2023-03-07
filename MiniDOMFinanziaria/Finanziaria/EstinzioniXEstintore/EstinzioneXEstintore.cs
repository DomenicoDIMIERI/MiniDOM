using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Relazione tra un finanziamento in corso ed un oggetto che lo estingue (pratica, consulenza, ecc)
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class EstinzioneXEstintore : Databases.DBObject, ICloneable
        {
            private bool m_Selezionata;        // Se vero indica che questo elemento é flaggato per l'estinzione nel constesto dell'estintore
            [NonSerialized]
            private IEstintore m_Estintore;       // Riferimento all'oggetto che estingue questo elemento
            private string m_TipoEstintore;       // Tipo dell'oggetto che estingue questo elemento
            private int m_IDEstintore;        // ID dell'oggetto che estingue questo elemento
            private DateTime? m_DataCaricamento;      // Data di caricamento (dell'oggetto che estingue. Questo valore é usato per il calcolo del montante residuo)
            private DateTime? m_DataEstinzione;       // Data di estinzione (di questo oggetto. Questo valore é usato per il calcolo del capitale da rimborsare)
            private int m_IDEstinzione;       // ID del prestito in corso associato a questo elemento
            [NonSerialized]
            private CEstinzione m_Estinzione;     // Prestitito associato a questo elemento
            private string m_Parametro;           // Parametro che specifica quale offerta nel contesto dell'estintore estingue questo elemento
            private decimal m_Correzione;         // Valore (in euro) da aggiungere al totale da rimborsare
            private TipoEstinzione m_Tipo;                    // Tipo del prestito in corso associato a questo elemento
            private int m_IDCessionario;                  // ID del cessionario
            [NonSerialized]
            private CCQSPDCessionarioClass m_Cessionario;     // Riferimento al cessionario
            private string m_NomeCessionario;                 // Nome del cessionario
            private string m_NomeAgenzia;                     // Nome dell'agenzia
            private decimal? m_Rata;                          // Valore della rata
            private DateTime? m_DataDecorrenza;                   // Data di decorrenza del prestito
            private DateTime? m_DataFine;                         // Ultima rata del prestito
            private int? m_Durata;                        // Durata in mesi del prestito
            private double? m_TAN;                            // TAN in %
            private double? m_TAEG;                           // TAEG in %
            private int m_NumeroQuoteInsolute;            // Numero di quote insolute
            private int m_NumeroQuoteResidue;             // Numero di quote residue
            private int m_NumeroQuoteScadute;             // Numero di quote scadute
            private decimal? m_TotaleDaEstinguere;            // Capitale da rimborsare
            private decimal? m_MontanteResiduo;               // Valore del montante residuo
            private decimal? m_MontanteResiduoForzato;        // Se valorizzato forza il vaore del montante residuo
            private double? m_PenaleEstinzione;               // Restituisce o imposta il valore della penale estinzione
            private string m_Descrizione;                     // Informazioni aggiuntive alla riga dell'estinzione

            public EstinzioneXEstintore()
            {
                m_Selezionata = false;
                m_Estintore = null;
                m_TipoEstintore = "";
                m_IDEstintore = 0;
                m_Parametro = "";
                m_IDEstinzione = 0;
                m_Estinzione = null;
                m_Correzione = 0m;
                m_Tipo = TipoEstinzione.ESTINZIONE_NO;
                m_IDCessionario = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_NomeAgenzia = "";
                m_Rata = default;
                m_DataDecorrenza = default;
                m_DataFine = default;
                m_Durata = default;
                m_TAN = default;
                m_TAEG = default;
                m_TotaleDaEstinguere = default;
                m_PenaleEstinzione = default;
                m_DataEstinzione = default;
                m_NumeroQuoteInsolute = 0;
                m_NumeroQuoteResidue = 0;
                m_NumeroQuoteScadute = 0;
                m_DataCaricamento = default;
                m_MontanteResiduo = default;
                m_MontanteResiduoForzato = default;
                m_Descrizione = "";
            }

            /// <summary>
        /// Restituisce o imposta la penale estinzione
        /// </summary>
        /// <returns></returns>
            public double? PenaleEstinzione
            {
                get
                {
                    return m_PenaleEstinzione;
                }

                set
                {
                    var oldValue = m_PenaleEstinzione;
                    if (value == oldValue == true)
                        return;
                    m_PenaleEstinzione = value;
                    DoChanged("PenaleEstinzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta delle informazioni aggiuntive per l'estinzione
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta la data dell'ultima rata del prestito in corso
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
        /// Restituisce o imposta il valore del montante residuo calcolato
        /// </summary>
        /// <returns></returns>
            public decimal? MontanteResiduo
            {
                get
                {
                    if (m_MontanteResiduoForzato.HasValue)
                        return m_MontanteResiduoForzato;
                    // Return Me.m_MontanteResiduo
                    if (m_Rata.HasValue)
                        return m_Rata.Value * NumeroQuoteResidueMR();
                    return 0;
                }
                // Set(value As Decimal?)
                // Dim oldValue As Decimal? = Me.m_MontanteResiduo
                // If (oldValue = value) Then Return
                // Me.m_MontanteResiduo = value
                // Me.DoChanged("MontanteResiduo", value, oldValue)
                // End Set
            }

            /// <summary>
        /// Restituisce o imposta il valore del montante residuo forzato
        /// </summary>
        /// <returns></returns>
            public decimal? MontanteResiduoForzato
            {
                get
                {
                    return m_MontanteResiduoForzato;
                }

                set
                {
                    var oldValue = m_MontanteResiduoForzato;
                    if (oldValue == value == true)
                        return;
                    m_MontanteResiduoForzato = value;
                    DoChanged("MontanteResiduoForzato", value, oldValue);
                }
            }

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
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cessionario = value;
                    m_IDCessionario = DBUtils.GetID(value);
                    m_NomeCessionario = "";
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
                    string oldValue = m_NomeCessionario;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            public string NomeAgenzia
            {
                get
                {
                    return m_NomeAgenzia;
                }

                set
                {
                    string oldValue = m_NomeAgenzia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    value = DMD.Strings.Trim(value);
                    m_NomeAgenzia = value;
                    DoChanged("NomeAgenzia", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta il valore della rata mensile al momento del calcolo
        /// </summary>
        /// <returns></returns>
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
                    DoChanged("Rata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultima rata del prestito in corso
        /// </summary>
        /// <returns></returns>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata del prestito in corso
        /// </summary>
        /// <returns></returns>
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
                    DoChanged("Durata", value, oldValue);
                }
            }

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
                    DoChanged("TAN", value, oldValue);
                }
            }

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
        /// Restituisce o imposta un valore booleano che indica se l'oggetto è selezionato per estinguere
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Selezionata
            {
                get
                {
                    return m_Selezionata;
                }

                set
                {
                    if (value == m_Selezionata)
                        return;
                    m_Selezionata = value;
                    DoChanged("Selezionata", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore correttivo da aggiungere al calcolo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal Correzione
            {
                get
                {
                    return m_Correzione;
                }

                set
                {
                    decimal oldValue = m_Correzione;
                    if (oldValue == value)
                        return;
                    m_Correzione = value;
                    DoChanged("Correzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un parametro aggiuntivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Parametro
            {
                get
                {
                    return m_Parametro;
                }

                set
                {
                    string oldValue = m_Parametro;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Parametro = value;
                    DoChanged("Parametro", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto che estingue l'altro prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public IEstintore Estintore
            {
                get
                {
                    if (m_Estintore is null)
                        m_Estintore = (IEstintore)Sistema.Types.GetItemByTypeAndId(m_TipoEstintore, m_IDEstintore);
                    return m_Estintore;
                }

                set
                {
                    var oldValue = m_Estintore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    SetEstintore(value);
                    DoChanged("Estintore", value, oldValue);
                }
            }

            internal void SetEstintore(object value)
            {
                m_Estintore = (IEstintore)value;
                m_IDEstintore = DBUtils.GetID((Databases.IDBObjectBase)value);
                if (value is null)
                {
                    m_TipoEstintore = "";
                }
                else
                {
                    m_TipoEstintore = DMD.RunTime.vbTypeName(value);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto che estingue l'altro prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDEstintore
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Estintore, m_IDEstintore);
                }

                set
                {
                    int oldValue = IDEstintore;
                    if (oldValue == value)
                        return;
                    m_IDEstintore = value;
                    m_Estintore = null;
                    DoChanged("IDEstintore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo dell'oggetto che estingue l'altro prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoEstintore
            {
                get
                {
                    return m_TipoEstintore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoEstintore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoEstintore = value;
                    DoChanged("TipoEstintore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore dell'estinzione (se si desidera forzare il valore occorre usare questa proprietà)
        /// </summary>
        /// <returns></returns>
            public decimal? TotaleDaEstinguere
            {
                get
                {
                    return m_TotaleDaEstinguere;
                }

                set
                {
                    var oldValue = m_TotaleDaEstinguere;
                    if (oldValue == value == true)
                        return;
                    m_TotaleDaEstinguere = value;
                    DoChanged("TotaleDaEstinguere", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di decorrenza per cui è fissato il calcolo dell'estinzione
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
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataDecorrenza = value;
                    DoChanged("DataDecorrenza", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di quote insolute alla data di decorrenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroQuoteInsolute
            {
                get
                {
                    return m_NumeroQuoteInsolute;
                }

                set
                {
                    int oldValue = m_NumeroQuoteInsolute;
                    if (oldValue == value)
                        return;
                    m_NumeroQuoteInsolute = value;
                    DoChanged("NumeroQuoteInsolute", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di quote scadute alla data di decorrenza
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroQuoteScadute
            {
                get
                {
                    return m_NumeroQuoteScadute;
                }

                set
                {
                    int oldValue = m_NumeroQuoteScadute;
                    if (oldValue == value)
                        return;
                    m_NumeroQuoteScadute = value;
                    DoChanged("NumeroQuoteScadute", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di quote rimanenti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroQuoteResidue
            {
                get
                {
                    return m_NumeroQuoteResidue;
                }

                set
                {
                    int oldValue = m_NumeroQuoteResidue;
                    if (oldValue == value)
                        return;
                    m_NumeroQuoteResidue = value;
                    DoChanged("NumeroQuoteResidue", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'altro prestito da estinguere
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDEstinzione
            {
                get
                {
                    return DBUtils.GetID(m_Estinzione, m_IDEstinzione);
                }

                set
                {
                    int oldValue = IDEstinzione;
                    if (oldValue == value)
                        return;
                    m_IDEstinzione = value;
                    m_Estinzione = null;
                    DoChanged("IDEstinzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'altro prestito da estinguere
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CEstinzione Estinzione
            {
                get
                {
                    if (m_Estinzione is null)
                        m_Estinzione = Estinzioni.GetItemById(m_IDEstinzione);
                    return m_Estinzione;
                }

                set
                {
                    var oldValue = m_Estinzione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Estinzione = value;
                    m_IDEstinzione = DBUtils.GetID(value);
                    DoChanged("Estinzione", value, oldValue);
                }
            }

            protected internal virtual void SetEstinzione(CEstinzione value)
            {
                m_Estinzione = value;
                m_IDEstinzione = DBUtils.GetID(value);
            }

            /// <summary>
        /// Aggiorna i valori copiandoli dagli oggetti Estinzione e Estintore associati
        /// </summary>
            public void AggiornaValori()
            {
                var est = Estinzione;
                if (est is object)
                {
                    if (est.Scadenza.HasValue == false && est.DataInizio.HasValue && est.Durata.HasValue)
                    {
                        est.Scadenza = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd(DateTimeInterval.Month, est.Durata.Value, est.DataInizio.Value));
                    }

                    if (est.DataInizio.HasValue == false && est.Scadenza.HasValue && est.Durata.HasValue)
                    {
                        est.DataInizio = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd(DateTimeInterval.Month, -est.Durata.Value, est.Scadenza.Value));
                    }

                    if (est.Istituto is null && !string.IsNullOrEmpty(est.NomeIstituto))
                    {
                        est.Istituto = Cessionari.GetItemByName(est.NomeIstituto);
                    }

                    est.Save();
                    Tipo = est.Tipo;
                    Cessionario = est.Istituto;
                    NomeCessionario = est.NomeIstituto;
                    NomeAgenzia = est.NomeAgenzia;
                    Rata = est.Rata;
                    DataDecorrenza = est.DataInizio;
                    DataFine = est.Scadenza;
                    Durata = est.Durata;
                    TAN = est.TAN;
                    TAEG = est.TAEG;
                }

                AggiornaCalcolo();
            }

            public void AggiornaCalcolo()
            {
                if (DataEstinzione.HasValue && DataFine.HasValue)
                    m_NumeroQuoteResidue = (int)Maths.Max(0L, DMD.DateUtils.DateDiff(DateTimeInterval.Month, DataEstinzione.Value, DataFine.Value) + 1L);
                if (Sistema.Formats.ToInteger(Durata) > 0)
                    m_NumeroQuoteResidue = Maths.Min(m_NumeroQuoteResidue, Sistema.Formats.ToInteger(Durata));
                // If (Me.m_Rata.HasValue) Then Me.MontanteResiduo = Me.NumeroQuoteResidueMR * Me.m_Rata.Value
            }

            /// <summary>
        /// Restituisce o imposta la data di estinzione (corrispondente in genere alla decorrenza dell'estintore)
        /// </summary>
        /// <returns></returns>
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

            /// <summary>
        /// Se il campo TotaleDaEstinguere é valorizzato questa proprietà restituisce il valore di TotaleDaEstinguere
        /// altrimenti restituisce il calcolo dell'estinzione secondo i dati memorizzati (rata, durata, quote residue, ecc)s
        /// </summary>
        /// <returns></returns>
            public decimal TotaleDaRimborsare
            {
                get
                {
                    if (m_TotaleDaEstinguere.HasValue)
                        return m_TotaleDaEstinguere.Value;

                    // Return Me.Estinzione.TotaleDaRimborsare(Me.m_NumeroQuoteScadute, Me.m_NumeroQuoteInsolute) + Me.m_Correzione
                    var c = new CEstinzioneCalculator();
                    c.Rata = Rata; // (this.getEstinzione().getRata());
                    c.PenaleEstinzione = (double)PenaleEstinzione; // (this.getEstinzione().getPenaleEstinzione());
                    c.TAN = (double)TAN; // (this.getEstinzione().getTAN());
                    c.SpeseAccessorie = Correzione; // (this.m_Correzione);
                    c.NumeroRateInsolute = NumeroQuoteInsolute; // (this.m_NumeroQuoteInsolute);
                    c.Durata = NumeroQuoteResidue; // (this.m_NumeroQuoteResidue);
                    return c.TotaleDaRimborsare;
                }
            }

            public override string GetTableName()
            {
                return "tbl_EstinzioniXEstintore";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDEstintore = reader.Read("IDEstintore", m_IDEstintore);
                m_TipoEstintore = reader.Read("TipoEstintore", m_TipoEstintore);
                m_IDEstinzione = reader.Read("IDEstinzione", m_IDEstinzione);
                m_DataDecorrenza = reader.Read("DataDecorrenza", m_DataDecorrenza);
                m_NumeroQuoteInsolute = reader.Read("NQI", m_NumeroQuoteInsolute);
                m_NumeroQuoteScadute = reader.Read("NQS", m_NumeroQuoteScadute);
                m_NumeroQuoteResidue = reader.Read("NQR", m_NumeroQuoteResidue);
                m_Parametro = reader.Read("Parametro", m_Parametro);
                m_Correzione = reader.Read("Correzione", m_Correzione);
                m_Selezionata = reader.Read("Selezionata", m_Selezionata);
                m_DataEstinzione = reader.Read("DataEstinzione", m_DataEstinzione);
                m_Rata = reader.Read("Rata", m_Rata);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                m_Durata = reader.Read("Durata", m_Durata);
                m_TAN = reader.Read("TAN", m_TAN);
                m_TAEG = reader.Read("TAEG", m_TAEG);
                m_TotaleDaEstinguere = reader.Read("TotaleDaEstinguere", m_TotaleDaEstinguere);
                m_IDCessionario = reader.Read("IDCessionario", m_IDCessionario);
                m_NomeCessionario = reader.Read("NomeCessionario", m_NomeCessionario);
                m_NomeAgenzia = reader.Read("NomeAgenzia", m_NomeAgenzia);
                m_Tipo = reader.Read("Tipo", m_Tipo);
                m_MontanteResiduo = reader.Read("MontanteResiduo", m_MontanteResiduo);
                m_DataCaricamento = reader.Read("DataCaricamento", m_DataCaricamento);
                m_MontanteResiduoForzato = reader.Read("MontanteResiduoForzato", m_MontanteResiduoForzato);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_PenaleEstinzione = reader.Read("PenaleEstinzione", m_PenaleEstinzione);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDEstintore", IDEstintore);
                writer.Write("TipoEstintore", m_TipoEstintore);
                writer.Write("IDEstinzione", IDEstinzione);
                writer.Write("DataDecorrenza", m_DataDecorrenza);
                writer.Write("NQI", m_NumeroQuoteInsolute);
                writer.Write("NQS", m_NumeroQuoteScadute);
                writer.Write("Parametro", m_Parametro);
                writer.Write("Correzione", m_Correzione);
                writer.Write("NQR", m_NumeroQuoteResidue);
                writer.Write("Selezionata", m_Selezionata);
                writer.Write("DataEstinzione", m_DataEstinzione);
                writer.Write("Rata", m_Rata);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Durata", m_Durata);
                writer.Write("TAN", m_TAN);
                writer.Write("TAEG", m_TAEG);
                writer.Write("TotaleDaEstinguere", m_TotaleDaEstinguere);
                writer.Write("IDCessionario", IDCessionario);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("NomeAgenzia", m_NomeAgenzia);
                writer.Write("Tipo", m_Tipo);
                writer.Write("MontanteResiduo", m_MontanteResiduo);
                writer.Write("DataCaricamento", m_DataCaricamento);
                writer.Write("MontanteResiduoForzato", m_MontanteResiduoForzato);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("PenaleEstinzione", m_PenaleEstinzione);
                return base.SaveToRecordset(writer);
            }


            // ------------------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDEstintore", IDEstintore);
                writer.WriteAttribute("TipoEstintore", m_TipoEstintore);
                writer.WriteAttribute("IDEstinzione", IDEstinzione);
                writer.WriteAttribute("DataDecorrenza", m_DataDecorrenza);
                writer.WriteAttribute("NQI", m_NumeroQuoteInsolute);
                writer.WriteAttribute("NQS", m_NumeroQuoteScadute);
                writer.WriteAttribute("NQR", m_NumeroQuoteResidue);
                writer.WriteAttribute("Parametro", m_Parametro);
                writer.WriteAttribute("Correzione", m_Correzione);
                writer.WriteAttribute("Selezionata", m_Selezionata);
                writer.WriteAttribute("DataEstinzione", m_DataEstinzione);
                writer.WriteAttribute("Rata", m_Rata);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("TAN", m_TAN);
                writer.WriteAttribute("TAEG", m_TAEG);
                writer.WriteAttribute("TotaleDaEstinguere", m_TotaleDaEstinguere);
                writer.WriteAttribute("IDCessionario", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("NomeAgenzia", m_NomeAgenzia);
                writer.WriteAttribute("Tipo", (int?)m_Tipo);
                writer.WriteAttribute("MontanteResiduo", m_MontanteResiduo);
                writer.WriteAttribute("DataCaricamento", m_DataCaricamento);
                writer.WriteAttribute("MontanteResiduoForzato", m_MontanteResiduoForzato);
                writer.WriteAttribute("PenaleEstinzione", m_PenaleEstinzione);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDEstintore":
                        {
                            m_IDEstintore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoEstintore":
                        {
                            m_TipoEstintore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDEstinzione":
                        {
                            m_IDEstinzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataDecorrenza":
                        {
                            m_DataDecorrenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "PenaleEstinzione":
                        {
                            m_PenaleEstinzione = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NQI":
                        {
                            m_NumeroQuoteInsolute = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NQS":
                        {
                            m_NumeroQuoteScadute = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NQR":
                        {
                            m_NumeroQuoteResidue = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Parametro":
                        {
                            m_Parametro = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Correzione":
                        {
                            m_Correzione = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Selezionata":
                        {
                            m_Selezionata = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataEstinzione":
                        {
                            m_DataEstinzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Rata":
                        {
                            m_Rata = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TAN":
                        {
                            m_TAN = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            TAEG = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TotaleDaEstinguere":
                        {
                            m_TotaleDaEstinguere = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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

                    case "NomeAgenzia":
                        {
                            m_NomeAgenzia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = (TipoEstinzione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MontanteResiduo":
                        {
                            m_MontanteResiduo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataCaricamento":
                        {
                            m_DataCaricamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MontanteResiduoForzato":
                        {
                            m_MontanteResiduoForzato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override CModulesClass GetModule()
            {
                return null; // Return Consulenze.Module
            }

            public object Clone()
            {
                return MemberwiseClone();
            }

            public bool IsInterno()
            {
                if (Estintore is CPraticaCQSPD)
                {
                    return DMD.Strings.Compare(NomeCessionario, ((CPraticaCQSPD)Estintore).NomeCessionario, true) == 0;
                }
                else if (Estintore is CQSPDConsulenza)
                {
                    CQSPDConsulenza c = (CQSPDConsulenza)Estintore;
                    if (Parametro == "C" && c.OffertaCQS is object)
                    {
                        return DMD.Strings.Compare(NomeCessionario, c.OffertaCQS.NomeCessionario, true) == 0;
                    }
                    else
                    {
                        return DMD.Strings.Compare(NomeCessionario, c.OffertaPD.NomeCessionario, true) == 0;
                    }
                }
                else if (Estintore is COffertaCQS)
                {
                    return DMD.Strings.Compare(NomeCessionario, ((COffertaCQS)Estintore).NomeCessionario, true) == 0;
                }
                else
                {
                    return false;
                }
            }

            public int NumeroQuoteResidueMR()
            {
                int resid = 0;
                if (IsInterno())
                {
                    if (DataCaricamento.HasValue && DataFine.HasValue)
                        resid = (int)Maths.Max(0L, DMD.DateUtils.DateDiff(DateTimeInterval.Month, DataCaricamento.Value, DataFine.Value));
                }
                else if (DataEstinzione.HasValue && DataFine.HasValue)
                    resid = (int)Maths.Max(0L, DMD.DateUtils.DateDiff(DateTimeInterval.Month, DataEstinzione.Value, DataFine.Value) + 1L);
                if (Sistema.Formats.ToInteger(Durata) > 0)
                    resid = Maths.Min(resid, Sistema.Formats.ToInteger(Durata));
                return resid;
            }
        }
    }
}