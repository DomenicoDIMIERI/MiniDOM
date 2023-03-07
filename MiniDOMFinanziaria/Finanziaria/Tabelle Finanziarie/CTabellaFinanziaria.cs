using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum TabellaFinanziariaFlags : int
        {
            None = 0,

            /// <summary>
        /// Se vero indica che il rappel è calcolato in base alla tariffa e non alla tabella spese
        /// </summary>
        /// <remarks></remarks>
            ForzaRappel = 1,

            /// <summary>
        /// Se vero indica che l'utilizzo di questa tabella Finanziaria è sottoposto all'approvazione di un supervisore
        /// </summary>
        /// <remarks></remarks>
            RichiedeApprovazione = 2,

            /// <summary>
        /// Se vero indica che la riduzione provvigionale (per estinzioni o rinnovi) viene detratta dal rappel e non dalla provvigione di UpFront
        /// </summary>
        /// <remarks></remarks>
            ScontaRiduzioniSuRappel = 4,

            /// <summary>
        /// Visualizza il riquadro provvigione TAN
        /// </summary>
            ShowProvvigioneTAN = 1024
        }

        /// <summary>
    /// Definisce la modalità con cui il sistema calcola le provvigioni
    /// </summary>
        public enum TipoCalcoloProvvigioni : int
        {
            /// <summary>
        /// Le commissioni vengono calcolate come prodotto tra il montante lordo e la provvigione definita nella tabella Finanziaria
        /// </summary>
            MONTANTE_LORDO = 0,

            /// <summary>
        /// Le commissioni vengono calcolate come prodotto tra la percentuale e la differenza tra il montante lordo e il valore da estinguere (solo cessioni su cessioni e deleghe su deleghe)
        /// </summary>
            DELTA_MONTANTE_ESTINZIONI = 1,

            /// <summary>
        /// Le commissioni vengono calcolate come prodotto tra la percentuale e la differenza tra il montante lordo e il valore da estinguere (sia cessioni che deleghe)
        /// </summary>
            DELTA_MONTANTE_ESTINZIONI1 = 2,

            /// <summary>
        /// Le commissioni vengono calcolate sulla base di un formula
        /// </summary>
            FUNZIONE = 2048
        }

        /// <summary>
    /// TABELLE FINANZIARIE ASSOCIABILI AD UN SINGOLO PRODOTTO
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CTabellaFinanziaria 
            : Databases.DBObject, ICloneable
        {
            private string m_Nome;                                    // Nome della tabella
            private int m_CessionarioID;                          // ID del cessionario
            [NonSerialized] private CCQSPDCessionarioClass m_Cessionario;             // Cessionario
            private string m_NomeCessionario;                         // Nome del cessionario
            private string m_Descrizione;                             // Nome della tabella visualizzato agli utenti finali
            private double?[] m_Coeff;                                // Array delle percentuali di coefficiente base (per durata)
            private double?[] m_Commiss;                              // Array delle percentuali di commissione (per durata)
            private bool m_TANVariabile;                           // Se vero indica che la tabella è a TAN variabile
            private double? m_TAN;                                    // Valore del TAN (in caso di tabella con TAN fisso)
            private DateTime? m_DataInizio;                   // Data di inizio validità della tabella
            private DateTime? m_DataFine;                     // Data di fine validità della tabella
            private bool m_Visible;                                // La tabella è visibile nell'elenco delle tabelle quando si fa un preventivo
            private double? m_ProvvMax;                               // Percentuale di UpFront massimo
            private double? m_ProvvMaxConRinnovi;                     // Percentuale di riduzione sul provvigionale in caso di estinzioni (interni)
            private double? m_ProvvMaxConEstinzioni;                  // Percentuale di riduzione sul provvigionale in caso di estinzioni (esterne)
            private double m_ProvvAggVisib;                           // Provvigione aggiuntiva visibile
            private double? m_Sconto;                                 // Percentuale dello sconto (rappel)
            private double m_ProvvTANR;                                // Percentuale provv. TAN in caso di rinnovi
            private double m_ProvvTANE;                                // Percentuale provv. TAN in caso di estinzioni
            private TabellaFinanziariaFlags m_Flags;                  // Flags
            private CKeyCollection m_Attributi;                       // Collezione di attributi aggiuntivi
            private decimal? m_UpFrontMax;                            // Valore in euro che indica il limite massimo dell'upfront pagato dal cessionario all'agenzia
            private TipoCalcoloProvvigioni m_TipoCalcoloProvvigioni;  // Valore che indica come calcolare le provvigioni
            private TipoCalcoloProvvigioni m_TipoCalcoloProvvTAN;     // Valore che indica come calcolare le provvigioni tan (provvigioni ScontoVisibile)
            private string m_FormulaProvvigioni;                      // Formula usata per il calcolo delle provvigioni quando in TipoCalcoloProvvigioni è impostato il valore TipoCalcoloProvvigioni.FUNZIONI
            private double m_ScontoVisibile;                          // Percentuale dello sconto (rappel) che è resa visibile agli utenti
            private string m_Categoria;

            public CTabellaFinanziaria()
            {
                m_Nome = "";
                m_CessionarioID = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_Descrizione = "";
                m_Sconto = default;
                m_Coeff = new double?[11];
                m_Commiss = new double?[11];
                m_TANVariabile = false;
                m_TAN = default;
                m_ProvvMax = default;
                m_ProvvMaxConEstinzioni = default;
                m_ProvvMaxConRinnovi = default;
                m_DataInizio = default;
                m_DataFine = default;
                m_Visible = true;
                m_Sconto = default;
                m_Flags = TabellaFinanziariaFlags.None;
                m_Attributi = new CKeyCollection();
                m_UpFrontMax = default;
                m_TipoCalcoloProvvigioni = TipoCalcoloProvvigioni.MONTANTE_LORDO;
                m_FormulaProvvigioni = DMD.Strings.vbNullString;
                m_ScontoVisibile = 0d;
                m_ProvvAggVisib = 0d;
                m_ProvvTANR = 0.0d;
                m_ProvvTANE = 0.0d;
                m_TipoCalcoloProvvTAN = TipoCalcoloProvvigioni.MONTANTE_LORDO;
                m_Categoria = "";
            }

            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione tan in caso di rinnovi
        /// </summary>
        /// <returns></returns>
            public double ProvvTANR
            {
                get
                {
                    return m_ProvvTANR;
                }

                set
                {
                    double oldValue = m_ProvvTANR;
                    if (oldValue == value)
                        return;
                    m_ProvvTANR = value;
                    DoChanged("ProvvTANR", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione tan in caso di estinzioni
        /// </summary>
        /// <returns></returns>
            public double ProvvTANE
            {
                get
                {
                    return m_ProvvTANE;
                }

                set
                {
                    double oldValue = m_ProvvTANE;
                    if (oldValue == value)
                        return;
                    m_ProvvTANE = value;
                    DoChanged("ProvvTANE", value, oldValue);
                }
            }



            /// <summary>
        /// Valore che determina come vengono calcolate le provvigioni TAN
        /// </summary>
        /// <returns></returns>
            public TipoCalcoloProvvigioni TipoCalcoloProvvTAN
            {
                get
                {
                    return m_TipoCalcoloProvvTAN;
                }

                set
                {
                    var oldValue = m_TipoCalcoloProvvTAN;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloProvvTAN = value;
                    DoChanged("TipoCalcoloProvvTAN", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione aggiuntiva visibile
        /// </summary>
        /// <returns></returns>
            public double ProvvAggVisib
            {
                get
                {
                    return m_ProvvAggVisib;
                }

                set
                {
                    double oldValue = m_ProvvAggVisib;
                    if (oldValue == value)
                        return;
                    m_ProvvAggVisib = value;
                    DoChanged("ProvvAggVisib", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica come calcolare le provvigioni
        /// </summary>
        /// <returns></returns>
            public TipoCalcoloProvvigioni TipoCalcoloProvvigioni
            {
                get
                {
                    return m_TipoCalcoloProvvigioni;
                }

                set
                {
                    var oldValue = m_TipoCalcoloProvvigioni;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloProvvigioni = value;
                    DoChanged("TipoCalcoloProvvigioni", value, oldValue);
                }
            }

            /// <summary>
        /// Nel caso in TipoCalcoloProvvigioni sia impostato il tipo "Formula" questo campo rappresenta la formula da valutare
        /// </summary>
        /// <returns></returns>
            public string FormulaProvvigioni
            {
                get
                {
                    return m_FormulaProvvigioni;
                }

                set
                {
                    string oldValue = m_FormulaProvvigioni;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FormulaProvvigioni = value;
                    DoChanged("FormulaProvvigioni", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il massimo UpFront caricabile con questa tabella Finanziaria
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? UpFrontMax
            {
                get
                {
                    return m_UpFrontMax;
                }

                set
                {
                    var oldValue = m_UpFrontMax;
                    if (oldValue == value == true)
                        return;
                    m_UpFrontMax = value;
                    DoChanged("UpFrontMax", value, oldValue);
                }
            }

            public CKeyCollection Attributi
            {
                get
                {
                    return m_Attributi;
                }
            }

            public override CModulesClass GetModule()
            {
                return TabelleFinanziarie.Module;
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se l'utilizzo della tabella Finanziaria è sottoposto all'approvazione di un supervisore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool RichiedeApprovazione
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, TabellaFinanziariaFlags.RichiedeApprovazione);
                }

                set
                {
                    if (RichiedeApprovazione == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, TabellaFinanziariaFlags.RichiedeApprovazione, value);
                    DoChanged("RichiedeApprovazione", value, !value);
                }
            }


            /// <summary>
        /// Restituisce o imposta lo sconto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? Sconto
            {
                get
                {
                    return m_Sconto;
                }

                set
                {
                    var oldValue = m_Sconto;
                    if (oldValue == value == true)
                        return;
                    m_Sconto = value;
                    DoChanged("Sconto", value, oldValue);
                }
            }

            public TabellaFinanziariaFlags Flags
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
        /// Restituisce o imposta la provvigione massima caricabile per il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? ProvvMax
            {
                get
                {
                    return m_ProvvMax;
                }

                set
                {
                    var oldValue = m_ProvvMax;
                    if (oldValue == value == true)
                        return;
                    m_ProvvMax = value;
                    DoChanged("ProvvMax", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione massima in caso di rinnovo (estinzione di pratica dello stesso cessionario)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? ProvvMaxConRinnovi
            {
                get
                {
                    return m_ProvvMaxConRinnovi;
                }

                set
                {
                    var oldValue = m_ProvvMaxConRinnovi;
                    if (oldValue == value == true)
                        return;
                    m_ProvvMaxConRinnovi = value;
                    DoChanged("ProvvMaxConRinnovi", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione massima in caso di estinzione (estinzione di pratica da cessionario diverso)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? ProvvMaxConEstinzioni
            {
                get
                {
                    return m_ProvvMaxConEstinzioni;
                }

                set
                {
                    var oldValue = m_ProvvMaxConEstinzioni;
                    if (oldValue == value == true)
                        return;
                    m_ProvvMaxConEstinzioni = value;
                    DoChanged("ProvvMaxConEstinzioni", value, oldValue);
                }
            }

            public bool Visible
            {
                get
                {
                    return m_Visible;
                }

                set
                {
                    if (m_Visible == value)
                        return;
                    m_Visible = value;
                    DoChanged("Visible", value, !value);
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
                    var oldValue = m_Cessionario;
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

            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
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
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            public double? get_CoefficienteBase(int durata)
            {
                return m_Coeff[(int)Maths.Floor(durata / 12d)];
            }

            public void set_CoefficienteBase(int durata, double? value)
            {
                var oldValue = m_Coeff[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value == true)
                    return;
                m_Coeff[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("CoefficienteBase", value, oldValue);
            }

            public double? get_CommissioniBancarie(int durata)
            {
                return m_Commiss[(int)Maths.Floor(durata / 12d)];
            }

            public void set_CommissioniBancarie(int durata, double? value)
            {
                var oldValue = m_Commiss[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value == true)
                    return;
                m_Commiss[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("CommissioniBancarie", value, oldValue);
            }

            public double? get_Interessi(int durata)
            {
                if (this.get_CoefficienteBase(durata).HasValue && this.get_CommissioniBancarie(durata).HasValue)
                {
                    return get_CoefficienteBase(durata) - get_CommissioniBancarie(durata);
                }
                else
                {
                    return default;
                }
                // Me.CommissioniBancarie(durata) = Me.CoefficienteBase(durata) - value
            }

            public void set_Interessi(int durata, double? value)
            {
                if (value.HasValue)
                {
                    if (get_CoefficienteBase(durata).HasValue)
                    {
                        set_CommissioniBancarie(durata, get_CoefficienteBase(durata) - value);
                    }
                    else if (get_CommissioniBancarie(durata).HasValue)
                    {
                        set_CoefficienteBase(durata, get_CommissioniBancarie(durata) + value);
                    }
                    else
                    {
                        set_CoefficienteBase(durata, value);
                    }
                }
                else
                {
                }
            }

            public bool TANVariabile
            {
                get
                {
                    return m_TANVariabile;
                }

                set
                {
                    bool oldValue = m_TANVariabile;
                    if (oldValue == value)
                        return;
                    m_TANVariabile = value;
                    DoChanged("TANVariabile", value, oldValue);
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
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce vero se la tabella è valida alla data odierna
        /// </summary>
        /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
        /// Restituisce vero se la tabella è valida alla data specificata
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns></returns>
            public bool IsValid(DateTime atDate)
            {
                return Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(atDate, m_DataInizio, m_DataFine);
            }

            /// <summary>
        /// Restituisce o imposta la percentuale visibile dello sconto (Rappel)
        /// </summary>
        /// <returns></returns>
            public double ScontoVisibile
            {
                get
                {
                    return m_ScontoVisibile;
                }

                set
                {
                    double oldValue = m_ScontoVisibile;
                    if (value == oldValue)
                        return;
                    m_ScontoVisibile = value;
                    DoChanged("ScontoVisibile", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_FIN_TblFin";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_CessionarioID = reader.Read("Cessionario", this.m_CessionarioID);
                m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                for (int i = 1; i <= 10; i++)
                {
                    m_Coeff[i] = reader.Read("c" + i * 12, this.m_Coeff[i]);
                    m_Commiss[i] = reader.Read("i" + i * 12, this.m_Commiss[i]);
                }

                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                m_TAN = reader.Read("TAN", this.m_TAN);
                m_TANVariabile = reader.Read("TANVariabile", this.m_TANVariabile);
                m_Visible = reader.Read("Visible", this.m_Visible);
                m_ProvvMax = reader.Read("ProvvMax", this.m_ProvvMax);
                m_ProvvMaxConRinnovi = reader.Read("ProvvMaxRinn", this.m_ProvvMaxConRinnovi);
                m_ProvvMaxConEstinzioni = reader.Read("ProvvMaxEst", this.m_ProvvMaxConEstinzioni);
                m_Sconto = reader.Read("Sconto", this.m_Sconto);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_UpFrontMax = reader.Read("UpFrontMax", this.m_UpFrontMax);
                
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                }
                
                m_TipoCalcoloProvvigioni = reader.Read("TipoCalcoloProvvigioni", this.m_TipoCalcoloProvvigioni);
                m_FormulaProvvigioni = reader.Read("FormulaProvvigioni", this.m_FormulaProvvigioni);
                m_ScontoVisibile = reader.Read("ScontoVisibile", this.m_ScontoVisibile);
                m_ProvvAggVisib = reader.Read("ProvvAggVisib", this.m_ProvvAggVisib);
                m_TipoCalcoloProvvTAN = reader.Read("TipoCalcoloProvvTAN", this.m_TipoCalcoloProvvTAN);
                m_ProvvTANR = reader.Read("ProvvTANR", this.m_ProvvTANR);
                m_ProvvTANE = reader.Read("ProvvTANE", this.m_ProvvTANE);
                m_Categoria = reader.Read("Categoria", this.m_Categoria);

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Cessionario", IDCessionario);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                for (int i = 1; i <= 10; i++)
                {
                    writer.Write("c" + i * 12, m_Coeff[i]);
                    writer.Write("i" + i * 12, m_Commiss[i]);
                }

                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("TAN", m_TAN);
                writer.Write("TANVariabile", m_TANVariabile);
                writer.Write("Visible", m_Visible);
                writer.Write("ProvvMax", m_ProvvMax);
                writer.Write("ProvvMaxRinn", m_ProvvMaxConRinnovi);
                writer.Write("ProvvMaxEst", m_ProvvMaxConEstinzioni);
                writer.Write("Sconto", m_Sconto);
                writer.Write("Flags", m_Flags);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("UpFrontMax", m_UpFrontMax);
                writer.Write("TipoCalcoloProvvigioni", m_TipoCalcoloProvvigioni);
                writer.Write("FormulaProvvigioni", m_FormulaProvvigioni);
                writer.Write("ScontoVisibile", m_ScontoVisibile);
                writer.Write("ProvvAggVisib", m_ProvvAggVisib);
                writer.Write("TipoCalcoloProvvTAN", m_TipoCalcoloProvvTAN);
                writer.Write("ProvvTANR", m_ProvvTANR);
                writer.Write("ProvvTANE", m_ProvvTANE);
                writer.Write("Categoria", m_Categoria);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDCessionario", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("TAN", m_TAN);
                writer.WriteAttribute("TANVariabile", m_TANVariabile);
                writer.WriteAttribute("Visible", m_Visible);
                writer.WriteAttribute("ProvvMax", m_ProvvMax);
                writer.WriteAttribute("ProvvMaxRinn", m_ProvvMaxConRinnovi);
                writer.WriteAttribute("ProvvMaxEst", m_ProvvMaxConEstinzioni);
                writer.WriteAttribute("Sconto", m_Sconto);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("UpFrontMax", m_UpFrontMax);
                writer.WriteAttribute("TipoCalcoloProvvigioni", (int?)m_TipoCalcoloProvvigioni);
                writer.WriteAttribute("ScontoVisibile", m_ScontoVisibile);
                writer.WriteAttribute("ProvvAggVisib", m_ProvvAggVisib);
                writer.WriteAttribute("TipoCalcoloProvvTAN", (int?)m_TipoCalcoloProvvTAN);
                writer.WriteAttribute("ProvvTANR", m_ProvvTANR);
                writer.WriteAttribute("ProvvTANE", m_ProvvTANE);
                writer.WriteAttribute("Categoria", m_Categoria);
                base.XMLSerialize(writer);
                writer.WriteTag("Coefficienti", m_Coeff);
                writer.WriteTag("Commissioni", m_Commiss);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("FormulaProvvigioni", m_FormulaProvvigioni);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCessionario":
                        {
                            m_CessionarioID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Coefficienti":
                        {
                            m_Coeff = (double?[])DMD.Arrays.Convert<double?>(fieldValue);
                            break;
                        }

                    case "Commissioni":
                        {
                            m_Commiss = (double?[])DMD.Arrays.Convert<double?>(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TAN":
                        {
                            m_TAN = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TANVariabile":
                        {
                            m_TANVariabile = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Visible":
                        {
                            m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ProvvMax":
                        {
                            m_ProvvMax = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvMaxRinn":
                        {
                            m_ProvvMaxConRinnovi = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvMaxEst":
                        {
                            m_ProvvMaxConEstinzioni = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Sconto":
                        {
                            m_Sconto = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (TabellaFinanziariaFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UpFrontMax":
                        {
                            m_UpFrontMax = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "TipoCalcoloProvvigioni":
                        {
                            m_TipoCalcoloProvvigioni = (TipoCalcoloProvvigioni)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "FormulaProvvigioni":
                        {
                            m_FormulaProvvigioni = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ScontoVisibile":
                        {
                            m_ScontoVisibile = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvAggVisib":
                        {
                            m_ProvvAggVisib = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TipoCalcoloProvvTAN":
                        {
                            m_TipoCalcoloProvvTAN = (TipoCalcoloProvvigioni)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ProvvTANR":
                        {
                            m_ProvvTANR = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ProvvTANE":
                        {
                            m_ProvvTANE = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                TabelleFinanziarie.UpdateCached(this);
                return ret;
            }

            public decimal CalcolaProvvigioni(COffertaCQS o, CCollection<EstinzioneXEstintore> estinzioni)
            {
                decimal? ret = default;
                decimal? ml = o.MontanteLordo;
                decimal sumEst = 0m;
                switch (TipoCalcoloProvvigioni)
                {
                    case TipoCalcoloProvvigioni.MONTANTE_LORDO:
                        {
                            break;
                        }

                    case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI:
                        {
                            break;
                        }

                    case TipoCalcoloProvvigioni.FUNZIONE:
                        {
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException("TipoCalcoloProvvigioni");
                            break;
                        }
                }

                return (decimal)ret;
            }

            public decimal CalcolaProvvigioniTAN(COffertaCQS o, CCollection<EstinzioneXEstintore> estinzioni)
            {
                decimal? ret = default;
                decimal? ml = o.MontanteLordo;
                decimal sumEst = 0m;
                switch (TipoCalcoloProvvigioni)
                {
                    case TipoCalcoloProvvigioni.MONTANTE_LORDO:
                        {
                            break;
                        }

                    case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI:
                        {
                            break;
                        }

                    case TipoCalcoloProvvigioni.FUNZIONE:
                        {
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException("TipoCalcoloProvvigioni");
                            break;
                        }
                }

                return (decimal)ret;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}