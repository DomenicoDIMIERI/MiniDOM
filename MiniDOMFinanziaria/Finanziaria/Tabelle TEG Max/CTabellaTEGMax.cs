using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum TipoCalcoloTEG : int
        {
            TEG_COSTANTE = 0,
            /// <summary>
        /// TEG funzione del montante lordo
        /// </summary>
        /// <remarks></remarks>
            TEG_FDIML = 1,
            /// <summary>
        /// TEG funzione dell'età
        /// </summary>
        /// <remarks></remarks>
            TEG_FDIETA = 2
        }



        /// <summary>
    /// Tabella dei TEG Massimi
    /// </summary>
    /// <remarks></remarks>
        public class CTabellaTEGMax : Databases.DBObject
        {
            private TipoCalcoloTEG m_TipoCalcolo;  // [int] Indica il tipo di calcolo 0 = Costate, 1 Su ML, 2 Su Età
            private string m_Nome; // Nome che identifica univocamente questo oggetto
            private int m_CessionarioID; // ID del cessionario
            private CCQSPDCessionarioClass m_Cessionario; // Oggetto cessionario
            private string m_NomeCessionario; // Nome del cessionario
            private string m_Descrizione; // Descrizione di questo oggetto
            private string m_Espressione; // Espressione scalare da valutare per determinare lo scaglione di definizione del TEG massimo
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private CQSPDRigheTEGMax m_Rows;           // Array di oggetti CRigheTEGMax che definiscono il valore soglia ed i coefficienti per ciascuna durata
            private bool m_Visible;

            public CTabellaTEGMax()
            {
                m_TipoCalcolo = 0;
                m_Nome = "";
                m_CessionarioID = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_Descrizione = "";
                m_Espressione = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_Rows = null;
                m_Visible = true;
            }

            public override CModulesClass GetModule()
            {
                return TabelleTEGMax.Module;
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

            public TipoCalcoloTEG TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }

                set
                {
                    var oldValue = m_TipoCalcolo;
                    if (oldValue == value)
                        return;
                    m_TipoCalcolo = value;
                    DoChanged("TipoCalcolo", value, oldValue);
                }
            }

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
                    m_Cessionario = null;
                    m_CessionarioID = value;
                    DoChanged("CessionarioID", value, oldValue);
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

            public string Espressione
            {
                get
                {
                    return m_Espressione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Espressione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Espressione = value;
                    DoChanged("Espressione", value, oldValue);
                }
            }

            public CQSPDRigheTEGMax Rows
            {
                get
                {
                    if (m_Rows is null)
                    {
                        m_Rows = new CQSPDRigheTEGMax();
                        m_Rows.Initialize(this);
                    }

                    return m_Rows;
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

            public bool IsValid()
            {
                return DMD.DateUtils.CheckBetween(DMD.DateUtils.Now(), m_DataInizio, m_DataFine);
            }

            public double Calculate(COffertaCQS offerta)
            {
                CRigaTEGMax row;
                int i;
                double valore;
                double ret = 0d;
                if (Rows.Count > 0)
                {
                    Rows.Sort();
                    if (string.IsNullOrEmpty(Espressione))
                    {
                        row = Rows[0];
                        ret = row.get_Coefficiente(offerta.Durata);
                    }
                    else
                    {
                        row = Rows[Rows.Count - 1];
                        ret = row.get_Coefficiente(offerta.Durata);
                        valore = DMD.Doubles.CDbl(Sistema.Types.CallMethod(offerta, Espressione));
                        for (i = Rows.Count - 2; i >= 0; i -= 1)
                        {
                            row = Rows[i];
                            if (row.ValoreSoglia >= valore)
                            {
                                ret = row.get_Coefficiente(offerta.Durata);
                            }
                        }
                    }
                }

                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_FIN_TEGMax";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_TipoCalcolo = reader.Read("TipoCalcolo", this.m_TipoCalcolo);
                this.m_CessionarioID = reader.Read("Cessionario", this.m_CessionarioID);
                this.m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                this.m_Espressione = reader.Read("Espressione", this.m_Espressione);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                this.m_Visible = reader.Read("Visible", this.m_Visible);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("TipoCalcolo", m_TipoCalcolo);
                writer.Write("Cessionario", DBUtils.GetID(m_Cessionario, m_CessionarioID));
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Espressione", m_Espressione);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Visible", m_Visible);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("TipoCalcolo", (int?)m_TipoCalcolo);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("CessionarioID", CessionarioID);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Espressione", m_Espressione);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Visible", m_Visible);
                base.XMLSerialize(writer);
                // Private m_Rows As CQSPDRigheTEGMax           'Array di oggetti CRigheTEGMax che definiscono il valore soglia ed i coefficienti per ciascuna durata
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "TipoCalcolo":
                        {
                            m_TipoCalcolo = (TipoCalcoloTEG)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Espressione":
                        {
                            m_Espressione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "Visible":
                        {
                            m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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
                TabelleTEGMax.UpdateCached(this);
            }
        }
    }
}