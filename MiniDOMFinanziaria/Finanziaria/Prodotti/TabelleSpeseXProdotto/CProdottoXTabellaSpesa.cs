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
    /// Relazione Prodotti x Tabellea Spese ( 1 - molti )
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CProdottoXTabellaSpesa : Databases.DBObject, IComparable, ICloneable
        {
            private string m_Nome; // Descrizione
            private int m_IDProdotto; // ID del prodotto associato
            [NonSerialized]
            private CCQSPDProdotto m_Prodotto; // Oggetto Prodotto Associato
            private int m_IDTabellaSpese; // ID della tablla spese
            [NonSerialized]
            private CTabellaSpese m_TabellaSpese;
            private int m_Flags;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;

            public CProdottoXTabellaSpesa()
            {
                m_Nome = "";
                m_IDProdotto = 0;
                m_Prodotto = null;
                m_IDTabellaSpese = 0;
                m_TabellaSpese = null;
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

            public int IDTabellaSpese
            {
                get
                {
                    return DBUtils.GetID(m_TabellaSpese, m_IDTabellaSpese);
                }

                set
                {
                    int oldValue = IDTabellaSpese;
                    if (oldValue == value)
                        return;
                    m_IDTabellaSpese = value;
                    m_TabellaSpese = null;
                    DoChanged("IDTabellaSpese", value, oldValue);
                }
            }

            public CTabellaSpese TabellaSpese
            {
                get
                {
                    if (m_TabellaSpese is null)
                        m_TabellaSpese = TabelleSpese.GetItemById(m_IDTabellaSpese);
                    return m_TabellaSpese;
                }

                set
                {
                    var oldValue = TabellaSpese;
                    if (oldValue == value)
                        return;
                    m_TabellaSpese = value;
                    m_IDTabellaSpese = DBUtils.GetID(value);
                    DoChanged("TabellaSpese", value, oldValue);
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
                return "tbl_FIN_ProdXTabSpes";
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                foreach (CCQSPDProdotto p in Prodotti.LoadAll())
                    p.InvalidateTabelleSpese();
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
                m_IDTabellaSpese = reader.Read("IDTabellaSpese", this.m_IDTabellaSpese);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IDProdotto", IDProdotto);
                writer.Write("IDTabellaSpese", IDTabellaSpese);
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
                writer.WriteAttribute("IDTabellaSpese", IDTabellaSpese);
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

                    case "IDTabellaSpese":
                        {
                            m_IDTabellaSpese = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

            public int CompareTo(CProdottoXTabellaSpesa other)
            {
                return DMD.Strings.Compare(m_Nome, other.m_Nome, true);
            }

            public int CompareTo(object other)
            {
                return CompareTo((CProdottoXTabellaSpesa)other);
            }

            public int _CompareTo(object other) => CompareTo(other);

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}