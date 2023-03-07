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
    /// Relazione tra un prodotto ed una convenzione
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CProdottoXConvenzione 
            : Databases.DBObject, IComparable
        {
            private string m_Nome; // Descrizione
            private int m_IDProdotto; // ID del prodotto associato
            [NonSerialized] private CCQSPDProdotto m_Prodotto; // Oggetto Prodotto Associato
            private int m_IDConvenzione; // ID della convenzione
            [NonSerialized] private CQSPDConvenzione m_Convenzione;
            private int m_Flags;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProdottoXConvenzione()
            {
                m_Nome = "";
                m_IDProdotto = 0;
                m_Prodotto = null;
                m_IDConvenzione = 0;
                m_Convenzione = null;
                m_Flags = 0;
                m_DataInizio = default;
                m_DataFine = default;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// ID del prodotto associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDProdotto
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_IDProdotto);
                }

                set
                {
                    int oldValue = IDProdotto;
                    if (oldValue == value)
                        return;
                    m_IDProdotto = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Oggetto Prodotto Associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDProdotto Prodotto
            {
                get
                {
                    if (m_Prodotto is null)
                        m_Prodotto = Prodotti.GetItemById(m_IDProdotto);
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = Prodotto;
                    if (oldValue == value)
                        return;
                    m_Prodotto = value;
                    m_IDProdotto = DBUtils.GetID(value);
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            protected internal virtual void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                m_IDProdotto = DBUtils.GetID(value);
            }

            public int IDConvenzione
            {
                get
                {
                    return DBUtils.GetID(m_Convenzione, m_IDConvenzione);
                }

                set
                {
                    int oldValue = IDConvenzione;
                    if (oldValue == value)
                        return;
                    m_IDConvenzione = value;
                    m_Convenzione = null;
                    DoChanged("IDConvenzione", value, oldValue);
                }
            }

            public CQSPDConvenzione Convenzione
            {
                get
                {
                    if (m_Convenzione is null)
                        m_Convenzione = Convenzioni.GetItemById(m_IDConvenzione);
                    return m_Convenzione;
                }

                set
                {
                    var oldValue = Convenzione;
                    if (oldValue == value)
                        return;
                    m_Convenzione = value;
                    m_IDConvenzione = DBUtils.GetID(value);
                    DoChanged("Convenzione", value, oldValue);
                }
            }

            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXConvenzioni";
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                foreach (CCQSPDProdotto p in Prodotti.LoadAll())
                    p.InvalidateConvenzioni();
            }

            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            public bool IsValid(DateTime at)
            {
                return DMD.DateUtils.CheckBetween(at, m_DataInizio, m_DataFine);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_IDProdotto = reader.Read("IDProdotto", this.m_IDProdotto);
                m_IDConvenzione = reader.Read("IDConvenzione", this.m_IDConvenzione);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IDProdotto", IDProdotto);
                writer.Write("IDConvenzione", IDConvenzione);
                writer.Write("Flags", m_Flags);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDProdotto", IDProdotto);
                writer.WriteAttribute("IDConvenzione", IDConvenzione);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                base.XMLSerialize(writer);
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

                    case "IDProdotto":
                        {
                            m_IDProdotto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDConvenzione":
                        {
                            m_IDConvenzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public int CompareTo(CProdottoXConvenzione other)
            {
                return DMD.Strings.Compare(m_Nome, other.m_Nome, true);
            }

            public int CompareTo(object other)
            {
                return CompareTo((CProdottoXConvenzione)other);
            }

            public int _CompareTo(object other) => CompareTo(other);
        }
    }
}