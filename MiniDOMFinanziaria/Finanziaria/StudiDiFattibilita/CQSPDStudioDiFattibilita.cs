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
        public class CQSPDStudioDiFattibilita : Databases.DBObjectPO, ICloneable
        {
            private int m_IDCliente;
            private Anagrafica.CPersona m_Cliente;
            private string m_NomeCliente;
            private DateTime m_Data;
            private int m_IDRichiesta;
            private CRichiestaFinanziamento m_Richiesta;
            private DateTime? m_OraInizio;
            private DateTime? m_OraFine;
            private int m_IDConsulente;
            private CConsulentePratica m_Consulente;
            private string m_NomeConsulente;
            private Anagrafica.CImpiegato m_Impiego;
            private decimal m_SommaTrattenuteVolontarie;
            private decimal m_SommaCQS;
            private decimal m_SommaPD;
            private decimal m_SommaPignoramenti;
            private int m_GARF;
            private decimal m_LimiteCumulativo;
            private decimal m_RataMaxC;
            private decimal m_RataMaxD;
            private DateTime m_DecorrenzaPratica;
            private CQSPDSoluzioniXStudioDiFattibilita m_Proposte;
            private int m_IDContesto;
            private string m_TipoContesto;

            public CQSPDStudioDiFattibilita()
            {
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_Data = DMD.DateUtils.Now();
                m_IDRichiesta = 0;
                m_Richiesta = null;
                m_IDConsulente = 0;
                m_Consulente = null;
                m_NomeConsulente = "";
                m_OraInizio = default;
                m_OraFine = default;
                m_Impiego = new Anagrafica.CImpiegato();
                m_SommaTrattenuteVolontarie = 0m;
                m_SommaCQS = 0m;
                m_SommaPD = 0m;
                m_SommaPignoramenti = 0m;
                m_GARF = 0;
                m_LimiteCumulativo = 0m;
                m_RataMaxC = 0m;
                m_RataMaxD = 0m;
                m_DecorrenzaPratica = Pratiche.CalcolaProssimaDecorrenza();
                m_Proposte = null;
                m_IDContesto = 0;
                m_TipoContesto = "";
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

            public CQSPDSoluzioniXStudioDiFattibilita Proposte
            {
                get
                {
                    if (m_Proposte is null)
                        m_Proposte = new CQSPDSoluzioniXStudioDiFattibilita(this);
                    return m_Proposte;
                }
            }

            public DateTime DecorrenzaPratica
            {
                get
                {
                    return m_DecorrenzaPratica;
                }

                set
                {
                    var oldValue = m_DecorrenzaPratica;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DecorrenzaPratica = value;
                    DoChanged("DecorrenzaPratica", value, oldValue);
                }
            }

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

            public decimal SommaCQS
            {
                get
                {
                    return m_SommaCQS;
                }

                set
                {
                    decimal oldValue = m_SommaCQS;
                    if (oldValue == value)
                        return;
                    m_SommaCQS = value;
                    DoChanged("SommaCQS", value, oldValue);
                }
            }

            public decimal SommaPD
            {
                get
                {
                    return m_SommaPD;
                }

                set
                {
                    decimal oldValue = m_SommaPD;
                    if (oldValue == value)
                        return;
                    m_SommaPD = value;
                    DoChanged("SommaPD", value, oldValue);
                }
            }

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

            public int GARF
            {
                get
                {
                    return m_GARF;
                }

                set
                {
                    int oldValue = m_GARF;
                    if (oldValue == value)
                        return;
                    m_GARF = value;
                    DoChanged("GARF", value, oldValue);
                }
            }

            public decimal LimiteCumulativo
            {
                get
                {
                    return m_LimiteCumulativo;
                }

                set
                {
                    decimal oldValue = m_LimiteCumulativo;
                    if (oldValue == value)
                        return;
                    m_LimiteCumulativo = value;
                    DoChanged("LimiteCumulativo", value, oldValue);
                }
            }

            public decimal RataMassimaCessione
            {
                get
                {
                    return m_RataMaxC;
                }

                set
                {
                    decimal oldValue = m_RataMaxC;
                    if (oldValue == value)
                        return;
                    m_RataMaxC = value;
                    DoChanged("RataMessimaCessione", value, oldValue);
                }
            }

            public decimal RataMassimaDelega
            {
                get
                {
                    return m_RataMaxD;
                }

                set
                {
                    decimal oldValue = m_RataMaxD;
                    if (oldValue == value)
                        return;
                    m_RataMaxD = value;
                    DoChanged("RataMassimaDelega", value, oldValue);
                }
            }

            public Anagrafica.CImpiegato Impiego
            {
                get
                {
                    return m_Impiego;
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

            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
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
                    DoChanged("Richiesta", value, oldValue);
                }
            }

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

            public DateTime? OraInizio
            {
                get
                {
                    return m_OraInizio;
                }

                set
                {
                    var oldValue = m_OraInizio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraInizio = value;
                    DoChanged("OraInizio", value, oldValue);
                }
            }

            public DateTime? OraFine
            {
                get
                {
                    return m_OraFine;
                }

                set
                {
                    var oldValue = m_OraFine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraFine = value;
                    DoChanged("OraFine", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return StudiDiFattibilita.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDGrpConsulenze";
            }

            public override bool IsChanged()
            {
                return base.IsChanged() || m_Impiego.IsChanged();
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret)
                    m_Impiego.SetChanged(false);
                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_Data = reader.Read("Data", this.m_Data);
                m_IDRichiesta = reader.Read("IDRichiesta", this.m_IDRichiesta);
                m_OraInizio = reader.Read("OraInizio", this.m_OraInizio);
                m_OraFine = reader.Read("OraFine", this.m_OraFine);
                m_IDConsulente = reader.Read("IDConsulente", this.m_IDConsulente);
                m_NomeConsulente = reader.Read("NomeConsulente", this.m_NomeConsulente);
                m_Impiego.IDAzienda = reader.Read("IDAzienda", m_Impiego.IDAzienda);
                m_Impiego.NomeAzienda = reader.Read("NomeAzienda", m_Impiego.NomeAzienda);
                m_Impiego.IDEntePagante = reader.Read("IDEntePagante", m_Impiego.IDEntePagante);
                m_Impiego.NomeEntePagante = reader.Read("NomeEntePagante", m_Impiego.NomeEntePagante);
                m_Impiego.StipendioNetto = reader.Read("StipendioNetto", m_Impiego.StipendioNetto);
                m_Impiego.TFR = reader.Read("TFR", m_Impiego.TFR);
                m_Impiego.TipoRapporto = reader.Read("TipoRapporto", m_Impiego.TipoRapporto);
                m_Impiego.SetChanged(false);
                m_SommaTrattenuteVolontarie = reader.Read("SommaTrattenuteVolontarie", this.m_SommaTrattenuteVolontarie);
                m_SommaCQS = reader.Read("SommaCQS", this.m_SommaCQS);
                m_SommaPD = reader.Read("SommaPD", this.m_SommaPD);
                m_SommaPignoramenti = reader.Read("SommaPignoramenti", this.m_SommaPignoramenti);
                m_GARF = reader.Read("GARF", this.m_GARF);
                m_LimiteCumulativo = reader.Read("LimiteCumulativo", this.m_LimiteCumulativo);
                m_RataMaxC = reader.Read("RataMaxC", this.m_RataMaxC);
                m_RataMaxD = reader.Read("RataMaxD", this.m_RataMaxD);
                m_DecorrenzaPratica = reader.Read("DecorrenzaPratica", this.m_DecorrenzaPratica);
                m_TipoContesto = reader.Read("TipoContesto", this.m_TipoContesto);
                m_IDContesto = reader.Read("IDContesto", this.m_IDContesto);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("Data", m_Data);
                writer.Write("IDRichiesta", IDRichiesta);
                writer.Write("OraInizio", m_OraInizio);
                writer.Write("OraFine", m_OraFine);
                writer.Write("IDConsulente", IDConsulente);
                writer.Write("NomeConsulente", m_NomeConsulente);
                writer.Write("IDAzienda", m_Impiego.IDAzienda);
                writer.Write("NomeAzienda", m_Impiego.NomeAzienda);
                writer.Write("IDEntePagante", m_Impiego.IDEntePagante);
                writer.Write("NomeEntePagante", m_Impiego.NomeEntePagante);
                writer.Write("StipendioNetto", m_Impiego.StipendioNetto);
                writer.Write("TFR", m_Impiego.TFR);
                writer.Write("TipoRapporto", m_Impiego.TipoRapporto);
                writer.Write("SommaTrattenuteVolontarie", m_SommaTrattenuteVolontarie);
                writer.Write("SommaCQS", m_SommaCQS);
                writer.Write("SommaPD", m_SommaPD);
                writer.Write("SommaPignoramenti", m_SommaPignoramenti);
                writer.Write("GARF", m_GARF);
                writer.Write("LimiteCumulativo", m_LimiteCumulativo);
                writer.Write("RataMaxC", m_RataMaxC);
                writer.Write("RataMaxD", m_RataMaxD);
                writer.Write("DecorrenzaPratica", m_DecorrenzaPratica);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("IDRichiesta", IDRichiesta);
                writer.WriteAttribute("OraInizio", m_OraInizio);
                writer.WriteAttribute("OraFine", m_OraFine);
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("NomeConsulente", m_NomeConsulente);
                writer.WriteAttribute("SommaTrattenuteVolontarie", m_SommaTrattenuteVolontarie);
                writer.WriteAttribute("SommaCQS", m_SommaCQS);
                writer.WriteAttribute("SommaPD", m_SommaPD);
                writer.WriteAttribute("SommaPignoramenti", m_SommaPignoramenti);
                writer.WriteAttribute("GARF", m_GARF);
                writer.WriteAttribute("LimiteCumulativo", m_LimiteCumulativo);
                writer.WriteAttribute("RataMaxC", m_RataMaxC);
                writer.WriteAttribute("RataMaxD", m_RataMaxD);
                writer.WriteAttribute("DecorrenzaPratica", m_DecorrenzaPratica);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                base.XMLSerialize(writer);
                writer.WriteTag("Impiego", m_Impiego);
                writer.WriteTag("Proposte", m_Proposte);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                var x = DMD.XML.Utils.Serializer;
                switch (fieldName ?? "")
                {
                    case "IDCliente":
                        {
                            m_IDCliente = (int)x.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = x.DeserializeString(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            m_Data = (DateTime)x.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDRichiesta":
                        {
                            m_IDRichiesta = (int)x.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OraInizio":
                        {
                            m_OraInizio = x.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraFine":
                        {
                            m_OraFine = x.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDConsulente":
                        {
                            m_IDConsulente = (int)x.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConsulente":
                        {
                            m_NomeConsulente = x.DeserializeString(fieldValue);
                            break;
                        }

                    case "Impiego":
                        {
                            m_Impiego = (Anagrafica.CImpiegato)fieldValue;
                            break;
                        }

                    case "SommaTrattenuteVolontarie":
                        {
                            m_SommaTrattenuteVolontarie = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SommaCQS":
                        {
                            m_SommaCQS = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SommaPD":
                        {
                            m_SommaPD = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SommaPignoramenti":
                        {
                            m_SommaPignoramenti = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "GARF":
                        {
                            m_GARF = (int)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LimiteCumulativo":
                        {
                            m_LimiteCumulativo = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RataMaxC":
                        {
                            m_RataMaxC = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RataMaxD":
                        {
                            m_RataMaxD = (decimal)x.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DecorrenzaPratica":
                        {
                            m_DecorrenzaPratica = (DateTime)x.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Proposte":
                        {
                            m_Proposte = (CQSPDSoluzioniXStudioDiFattibilita)fieldValue;
                            if (m_Proposte is object)
                                m_Proposte.SetStudioDiFattibilita(this);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)x.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoContesto":
                        {
                            m_TipoContesto = x.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}