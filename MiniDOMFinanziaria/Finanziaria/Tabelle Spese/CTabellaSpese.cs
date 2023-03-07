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
        public enum TabellaSpeseFlags : int
        {
            None = 0,
            Attiva = 1
        }




        /// <summary>
    /// SPESE DEFINITE PER UNO O PIU' PRODOTTI
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CTabellaSpese 
            : Databases.DBObject, ICloneable
        {
            private TabellaSpeseFlags m_Flags;
            private int m_CessionarioID;
            [NonSerialized] private CCQSPDCessionarioClass m_Cessionario;
            private string m_NomeCessionario;
            private string m_Nome;
            private string m_Descrizione;
            private decimal[] m_SpeseFisse;
            private double[] m_SpeseVariabili; //percentuale 
            // Private m_Interessi() As Double
            private decimal[] m_Rivalsa;
            private decimal[] m_SpeseProduttore;
            private decimal[] m_SpeseAmministrazione;
            private decimal[] m_SpesePerRata;
            private double[] m_ImpostaSostitutiva;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            // Private m_Running As Double
            private double[] m_Rappel;
            private bool m_Visible;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTabellaSpese()
            {
                m_CessionarioID = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_Nome = "";
                m_Descrizione = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_SpeseFisse = new decimal[11];
                m_SpeseVariabili = new double[11];
                // ReDim m_Interessi(10)
                m_Rivalsa = new decimal[11];
                m_SpeseProduttore = new decimal[11];
                m_SpeseAmministrazione = new decimal[11];
                m_SpesePerRata = new decimal[11];
                m_ImpostaSostitutiva = new double[11];
                m_Rappel = new double[11];
                for (int i = 0; i <= 10; i++)
                {
                    m_SpeseFisse[i] = 0m;
                    m_SpeseVariabili[i] = 0d;
                    // m_Interessi(i) = 0
                    m_Rivalsa[i] = 0m;
                    m_SpeseProduttore[i] = 0m;
                    m_SpeseAmministrazione[i] = 0m;
                    m_SpesePerRata[i] = 0m;
                    m_ImpostaSostitutiva[i] = 0d;
                    m_Rappel[i] = 0d;
                }
                // Me.m_Running = 0
                m_Visible = true;
                m_Flags = TabellaSpeseFlags.Attiva;
            }

            public override CModulesClass GetModule()
            {
                return TabelleSpese.Module;
            }

            public bool Attiva
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, TabellaSpeseFlags.Attiva);
                }

                set
                {
                    if (Attiva == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, TabellaSpeseFlags.Attiva, value);
                    DoChanged("Attiva", value, !value);
                }
            }



            /// <summary>
        /// Restituisce o imposta i flags impostati per la tabella spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TabellaSpeseFlags Flags
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
        /// Restituisce o imposta un valore booleano che indica se la tabella spese è visibile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            // Public Property Running As Double
            // Get
            // Return Me.m_Running
            // End Get
            // Set(value As Double)
            // If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Running")
            // Dim oldValue As Double = Me.m_Running
            // If (oldValue = value) Then Exit Property
            // Me.m_Running = value
            // Me.DoChanged("Running", value, oldValue)
            // End Set
            // End Property

            /// <summary>
        /// Rappresenta la provvigione percentuale sul montante lordo che il cessionario paga all'agenzia (non visibile)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double get_Rappel(int index)
            {
                return m_Rappel[index];
            }

            public void set_Rappel(int index, double value)
            {
                if (value < 0d | value > 100d)
                    throw new ArgumentOutOfRangeException("Rappel");
                double oldValue = m_Rappel[index];
                if (oldValue == value)
                    return;
                m_Rappel[index] = value;
                DoChanged("Running", value, oldValue);
            }

            /// <summary>
        /// Nome della tabella spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            /// <summary>
        /// Descrizione della tabella spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            /// <summary>
        /// ID del cessionario
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int CessionarioID
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_CessionarioID);
                }

                set
                {
                    int oldValue = CessionarioID;
                    if (oldValue == value)
                        return;
                    m_CessionarioID = value;
                    m_Cessionario = null;
                    DoChanged("CessionarioID", value, oldValue);
                }
            }

            /// <summary>
        /// Cessionario
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            /// <summary>
        /// Nome del cessionario
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            /// <summary>
        /// Data di inizio di validità
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
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
        /// Data di fine validità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            public bool IsValid(DateTime atDate)
            {
                return Attiva && DMD.DateUtils.CheckBetween(atDate, m_DataInizio, m_DataFine);
            }

            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
        /// Restituisce o imposta le spese pagate dall'agenzia in funzione della durata del finanziamento
        /// </summary>
        /// <param name="durata"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal get_SpeseProduttore(int durata)
            {
                return m_SpeseProduttore[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpeseProduttore(int durata, decimal value)
            {
                if (value < 0m)
                    throw new ArgumentOutOfRangeException("SpeseProduttore");
                decimal oldValue = m_SpeseProduttore[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_SpeseProduttore[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("SpeseProduttore", value, oldValue);
            }

            /// <summary>
        /// Restituisce o imposta le spese pagate all'amministrazione in base alla durata del finanziamento
        /// </summary>
        /// <param name="durata"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal get_SpeseAmministrazione(int durata)
            {
                return m_SpeseAmministrazione[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpeseAmministrazione(int durata, decimal value)
            {
                if (value < 0m)
                    throw new ArgumentOutOfRangeException("SpeseAmministrazione");
                decimal oldValue = m_SpeseAmministrazione[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_SpeseAmministrazione[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("SpeseAmministrazione", value, oldValue);
            }

            /// <summary>
        /// Restituisce o imposta il valore dell'imposta sostituiva in base alla durata del finanziamento
        /// </summary>
        /// <param name="durata"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double get_ImpostaSostitutiva(int durata)
            {
                return m_ImpostaSostitutiva[(int)Maths.Floor(durata / 12d)];
            }

            public void set_ImpostaSostitutiva(int durata, double value)
            {
                if (value < 0d)
                    throw new ArgumentOutOfRangeException("ImpostaSostitutiva");
                double oldValue = m_ImpostaSostitutiva[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_ImpostaSostitutiva[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("ImpostaSostitutiva", value, oldValue);
            }

            // Public Property Coefficiente(ByVal durata As Integer) As double
            // Get
            // Return Me.m_Durata(Fix(durata / 12))
            // End Get
            // Set(value As Double)
            // Me.m_Durata(Fix(durata / 12)) = value
            // End Set
            // End Property

            public decimal get_Rivalsa(int durata)
            {
                return m_Rivalsa[(int)Maths.Floor(durata / 12d)];
            }

            public void set_Rivalsa(int durata, decimal value)
            {
                if (value < 0m)
                    throw new ArgumentOutOfRangeException("Rivalsa");
                decimal oldValue = m_Rivalsa[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_Rivalsa[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("", value, oldValue);
            }

            public decimal get_SpesePerRata(int durata)
            {
                return m_SpesePerRata[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpesePerRata(int durata, decimal value)
            {
                if (value < 0m)
                    throw new ArgumentOutOfRangeException("SpesePerRata");
                decimal oldValue = m_SpesePerRata[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_SpesePerRata[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("SpesePerRata", value, oldValue);
            }

            public decimal get_SpeseFisse(int durata)
            {
                return m_SpeseFisse[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpeseFisse(int durata, decimal value)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("SpeseFisse");

                var oldValue = m_SpeseFisse[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;

                m_SpeseFisse[(int)Maths.Floor(durata / 12d)] = (decimal)value;

                DoChanged("SpeseFisse", value, oldValue);
            }

            public double get_SpeseVariabili(int durata)
            {
                return m_SpeseVariabili[(int)Maths.Floor(durata / 12d)];
            }

            public void set_SpeseVariabili(int durata, double value)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("SpeseVariabili");

                double oldValue = m_SpeseVariabili[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;

                m_SpeseVariabili[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("SpeseVariabili", value, oldValue);
            }


            /// <summary>
        /// Calcola le spese per l'oggerta selezionata
        /// </summary>
        /// <param name="offerta"></param>
        /// <remarks></remarks>
            public void Calcola(COffertaCQS offerta)
            {
                if (offerta is null)
                    throw new ArgumentNullException("offerta");
                // offerta.Rivalsa = m_Rivalsa(offerta.Durata)
                // offerta.AltreSpese = 0
                // offerta.AltreSpese = offerta.AltreSpese + m_SpeseFisse(offerta.Durata)
                // offerta.AltreSpese = offerta.AltreSpese + m_SpeseCoefficiente(offerta.Durata) * offerta.MontanteLordo
                // offerta.m_Interessi(10)
                // offerta.AltreSpese = offerta.AltreSpese + m_SpeseProduttore(offerta.Durata)
                // offerta.AltreSpese = offerta.AltreSpese + m_SpeseAmministrazione(offerta.Durata)
                // offerta.AltreSpese = offerta.AltreSpese + m_SpesePerRata(offerta.Durata) * offerta.Durata
                // offerta.AltreSpese = offerta.AltreSpese + m_ImpostaSostitutiva(offerta.Durata) * offerta.CapitaleFinanziato
                offerta.Rappel = (double)offerta.MontanteLordo * get_Rappel(offerta.Durata) / 100d;
                offerta.Rivalsa = get_Rivalsa(offerta.Durata);
                // Calcolo degli oneri erariali
                offerta.OneriErariali = get_SpeseProduttore(offerta.Durata) + get_SpeseAmministrazione(offerta.Durata);
                // If Prodotto.Scomposizione Is Nothing Then
                offerta.Imposte = 0m;
                // Else
                // m_Imposte = Prodotto.Scomposizione.ImposteFisse
                // End If

                decimal altreSpese = offerta.AltreSpese;
                altreSpese += this.get_SpeseFisse(offerta.Durata);
                altreSpese += offerta.MontanteLordo * (decimal)get_SpeseVariabili(offerta.Durata) / 100;
                offerta.AltreSpese = altreSpese;
                
                offerta.SpeseConvenzioni = offerta.Durata * this.get_SpesePerRata(offerta.Durata);

                // Imposta
                offerta.ImpostaSostitutiva = (decimal)((double)(offerta.MontanteLordo - offerta.Interessi) * get_ImpostaSostitutiva(offerta.Durata) / 100d);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_TabellaSpese";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                this.m_CessionarioID = reader.Read("Cessionario", this.m_CessionarioID);
                this.m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                for (int i = 2; i <= 10; i++)
                {
                    m_Rivalsa[i] = reader.Read("durata" + i * 12, this.m_Rivalsa[i]);
                    m_SpeseFisse[i] = reader.Read("SpeseFisse" + i * 12, this.m_SpeseFisse[i]);
                    m_SpeseVariabili[i] = reader.Read("SpeseCoefficiente" + i * 12, this.m_SpeseVariabili[i]);
                    // m_Interessi(i) = dbRis ( "Interessi" & (i * 12))
                    m_SpeseProduttore[i] = reader.Read("SpeseProduttore" + i * 12, this.m_SpeseProduttore[i]);
                    m_SpeseAmministrazione[i] = reader.Read("SpeseAmministrazione" + i * 12, this.m_SpeseAmministrazione[i]);
                    m_SpesePerRata[i] = reader.Read("SpesePerRata" + i * 12, this.m_SpesePerRata[i]);
                    m_ImpostaSostitutiva[i] = reader.Read("ImpostaSostitutiva" + i * 12, this.m_ImpostaSostitutiva[i]);
                    // Me.m_Running = reader.Read("Running", Me.m_Running)
                    m_Rappel[i] = reader.Read("Rappel" + i * 12, this.m_Rappel[i]);
                }

                m_Visible = reader.Read("Visible", this.m_Visible);
                m_Flags = reader.Read("Flags", this.m_Flags);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Cessionario", CessionarioID);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                for (int i = 2; i <= 10; i++)
                {
                    writer.Write("Durata" + i * 12, m_Rivalsa[i]);
                    writer.Write("SpeseFisse" + i * 12, m_SpeseFisse[i]);
                    writer.Write("SpeseCoefficiente" + i * 12, m_SpeseVariabili[i]);
                    // dbRis("Interessi" & (i * 12)) = m_Interessi(i)
                    writer.Write("SpeseProduttore" + i * 12, m_SpeseProduttore[i]);
                    writer.Write("SpeseAmministrazione" + i * 12, m_SpeseAmministrazione[i]);
                    writer.Write("SpesePerRata" + i * 12, m_SpesePerRata[i]);
                    writer.Write("ImpostaSostitutiva" + i * 12, m_ImpostaSostitutiva[i]);
                    writer.Write("Rappel" + i * 12, m_Rappel[i]);
                }
                // writer.Write("Running", Me.m_Running)
                writer.Write("Visible", m_Visible);
                writer.Write("Flags", m_Flags);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("CessionarioID", CessionarioID);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Visible", m_Visible);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("SpeseFisse", m_SpeseFisse);
                writer.WriteTag("SpeseCoefficiente", m_SpeseVariabili);
                writer.WriteTag("Rivalsa", m_Rivalsa);
                writer.WriteTag("SpeseProduttore", m_SpeseProduttore);
                writer.WriteTag("SpeseAmministrazione", m_SpeseAmministrazione);
                writer.WriteTag("SpesePerRata", m_SpesePerRata);
                writer.WriteTag("ImpostaSostitutiva", m_ImpostaSostitutiva);
                writer.WriteTag("Rappel", m_Rappel);
                // writer.WriteTag("Running", Me.m_Running)
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
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

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SpeseFisse":
                        {
                            m_SpeseFisse = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "SpeseCoefficiente":
                        {
                            m_SpeseVariabili = (double[])DMD.Arrays.Convert<double>(fieldValue);
                            break;
                        }

                    case "Rivalsa":
                        {
                            m_Rivalsa = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "SpeseProduttore":
                        {
                            m_SpeseProduttore = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "SpeseAmministrazione":
                        {
                            m_SpeseAmministrazione = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "SpesePerRata":
                        {
                            m_SpesePerRata = (decimal[])DMD.Arrays.Convert<decimal>(fieldValue);
                            break;
                        }

                    case "ImpostaSostitutiva":
                        {
                            m_ImpostaSostitutiva = (double[])DMD.Arrays.Convert<double>(fieldValue);
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
                    // Case "Running" : Me.m_Running = DMD.XML.Utils.Serializer.DeserializeDouble(fieldValue)
                    case "Rappel":
                        {
                            m_Rappel = (double[])DMD.Arrays.Convert<double>(fieldValue);
                            break;
                        }

                    case "Visible":
                        {
                            m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (TabellaSpeseFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                TabelleSpese.UpdateCached(this);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}