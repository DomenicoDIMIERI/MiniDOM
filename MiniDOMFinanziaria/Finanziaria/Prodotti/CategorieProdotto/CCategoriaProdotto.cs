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
        public class CCategoriaProdotto 
            : Databases.DBObject, IComparable
        {
            private string m_Nome; // Descrizione
            private string m_NomeGruppo; // Supercategoria
            private int m_Flags;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private CKeyCollection m_Attributi;

            public CCategoriaProdotto()
            {
                m_Nome = "";
                m_Flags = 0;
                m_NomeGruppo = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_Attributi = new CKeyCollection();
            }

            public override CModulesClass GetModule()
            {
                return CategorieProdotto.Module;
            }

            /// <summary>
        /// Restituisce o imposta il nome del gruppo a cui appartiene la categoria di prodotti
        /// Questo valore viene usato per raggruppare ulteriormente i prodotti nei reports
        /// </summary>
        /// <returns></returns>
            public string NomeGruppo
            {
                get
                {
                    return m_NomeGruppo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeGruppo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeGruppo = value;
                    DoChanged("NomeGruppo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della categoria di prodotti
        /// </summary>
        /// <returns></returns>
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

            public CKeyCollection Attributi
            {
                get
                {
                    if (m_Attributi is null)
                        m_Attributi = new CKeyCollection();
                    return m_Attributi;
                }
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDCatProd";
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                CategorieProdotto.UpdateCached(this);
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
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_NomeGruppo = reader.Read("NomeGruppo", this.m_NomeGruppo);
                this.m_Flags = reader.Read("Flags", this.m_Flags);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("NomeGruppo", m_NomeGruppo);
                writer.Write("Flags", m_Flags);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("NomeGruppo", m_NomeGruppo);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                base.XMLSerialize(writer);
                writer.WriteTag("Attributi", Attributi);
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

                    case "NomeGruppo":
                        {
                            m_NomeGruppo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public int CompareTo(CCategoriaProdotto other)
            {
                return DMD.Strings.Compare(m_Nome, other.m_Nome, true);
            }

            public int CompareTo(object other)
            {
                return CompareTo((CCategoriaProdotto)other);
            }

            public int _CompareTo(object other) => CompareTo(other);

            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
            }
        }
    }
}