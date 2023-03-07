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
    /// Relazione tra un'azienda ed una convenzione
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class AziendaXConvenzione : Databases.DBObject, IComparable
        {
            private string m_Nome; // Descrizione
            private int m_IDAzienda; // ID del Azienda associato
            [NonSerialized]
            private Anagrafica.CAzienda m_Azienda; // Oggetto Azienda Associato
            private int m_IDConvenzione; // ID della convenzione
            [NonSerialized]
            private CQSPDConvenzione m_Convenzione;
            private int m_Flags;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;

            public AziendaXConvenzione()
            {
                m_Nome = "";
                m_IDAzienda = 0;
                m_Azienda = null;
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
        /// ID del Azienda associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID(m_Azienda, m_IDAzienda);
                }

                set
                {
                    int oldValue = IDAzienda;
                    if (oldValue == value)
                        return;
                    m_IDAzienda = value;
                    m_Azienda = null;
                    DoChanged("IDAzienda", value, oldValue);
                }
            }

            /// <summary>
        /// Oggetto Azienda Associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CAzienda Azienda
            {
                get
                {
                    if (m_Azienda is null)
                        m_Azienda = Anagrafica.Aziende.GetItemById(m_IDAzienda);
                    return m_Azienda;
                }

                set
                {
                    var oldValue = Azienda;
                    if (oldValue == value)
                        return;
                    m_Azienda = value;
                    m_IDAzienda = DBUtils.GetID(value);
                    DoChanged("Azienda", value, oldValue);
                }
            }

            protected internal virtual void SetAzienda(Anagrafica.CAzienda value)
            {
                m_Azienda = value;
                m_IDAzienda = DBUtils.GetID(value);
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

            internal void SetConvenzione(CQSPDConvenzione value)
            {
                m_Convenzione = value;
                m_IDConvenzione = DBUtils.GetID(value);
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
                return "tbl_FIN_AzieXConvenzioni";
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                foreach (CQSPDConvenzione p in Convenzioni.LoadAll())
                    p.InvalidateAziende();
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
                m_Nome = reader.Read("Nome", m_Nome);
                m_IDAzienda = reader.Read("IDAzienda", m_IDAzienda);
                m_IDConvenzione = reader.Read("IDConvenzione", m_IDConvenzione);
                m_Flags = reader.Read("Flags", m_Flags);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IDAzienda", IDAzienda);
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
                writer.WriteAttribute("IDAzienda", IDAzienda);
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

                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

            public int CompareTo(AziendaXConvenzione other)
            {
                return DMD.Strings.Compare(m_Nome, other.m_Nome, true);
            }

            public int CompareTo(object other)
            {
                return CompareTo((AziendaXConvenzione)other);
            }

            public int _CompareTo(object other) => CompareTo(other);
        }
    }
}