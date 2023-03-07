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
        public class CQSPDValutazioneAssicurazione 
            : Databases.DBObject
        {
            private int m_IDAssicurazione;
            [NonSerialized] private CAssicurazione m_Assicurazione;
            private string m_NomeAssicurazione;
            private string m_StatoAssicurazione;
            private decimal? m_RapportoTFR_VN;
            private int? m_Rating;
            private int m_Flags;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CQSPDValutazioneAssicurazione()
            {
                m_IDAssicurazione = 0;
                m_Assicurazione = null;
                m_NomeAssicurazione = "";
                m_StatoAssicurazione = "";
                m_RapportoTFR_VN = default;
                m_Rating = default;
                m_Flags = 0;
            }

            public int IDAssicurazione
            {
                get
                {
                    return DBUtils.GetID(m_Assicurazione, m_IDAssicurazione);
                }

                set
                {
                    int oldValue = IDAssicurazione;
                    if (oldValue == value)
                        return;
                    m_IDAssicurazione = value;
                    m_Assicurazione = null;
                    DoChanged("IDAssicurazione", value, oldValue);
                }
            }

            public CAssicurazione Assicurazione
            {
                get
                {
                    if (m_Assicurazione is null)
                        m_Assicurazione = Assicurazioni.GetItemById(m_IDAssicurazione);
                    return m_Assicurazione;
                }

                set
                {
                    var oldValue = m_Assicurazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Assicurazione = value;
                    m_IDAssicurazione = DBUtils.GetID(value);
                    m_NomeAssicurazione = "";
                    if (value is object)
                        m_NomeAssicurazione = value.Nome;
                    DoChanged("Assicurazione", value, oldValue);
                }
            }

            public string NomeAssicurazione
            {
                get
                {
                    return m_NomeAssicurazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAssicurazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssicurazione = value;
                    DoChanged("NomeAssicurazione", value, oldValue);
                }
            }

            public decimal? RapportoTFR_VN
            {
                get
                {
                    return m_RapportoTFR_VN;
                }

                set
                {
                    var oldValue = m_RapportoTFR_VN;
                    if (oldValue == value == true)
                        return;
                    m_RapportoTFR_VN = value;
                    DoChanged("RapportoTFR_VN", value, oldValue);
                }
            }

            public int? Rating
            {
                get
                {
                    return m_Rating;
                }

                set
                {
                    int oldValue = (int)m_Rating;
                    if (oldValue == value == true)
                        return;
                    m_Rating = value;
                    DoChanged("Rating", value, oldValue);
                }
            }

            public string StatoAssicurazione
            {
                get
                {
                    return m_StatoAssicurazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_StatoAssicurazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StatoAssicurazione = value;
                    DoChanged("StatoAssicurazione", value, oldValue);
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

            public override CModulesClass GetModule()
            {
                return null;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDValutazioniAssicurazione";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDAssicurazione = reader.Read("IDAssicurazione", this.m_IDAssicurazione);
                this.m_NomeAssicurazione = reader.Read("NomeAssicurazione", this.m_NomeAssicurazione);
                this.m_RapportoTFR_VN = reader.Read("RapportoTFR_VN", this.m_RapportoTFR_VN);
                this.m_Rating = reader.Read("Rating", this.m_Rating);
                this.m_StatoAssicurazione = reader.Read("StatoAssicurazione", this.m_StatoAssicurazione);
                this.m_Flags = reader.Read("Flags", this.m_Flags);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDAssicurazione", IDAssicurazione);
                writer.Write("NomeAssicurazione", m_NomeAssicurazione);
                writer.Write("RapportoTFR_VN", m_RapportoTFR_VN);
                writer.Write("Rating", m_Rating);
                writer.Write("StatoAssicurazione", m_StatoAssicurazione);
                writer.Write("Flags", m_Flags);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDAssicurazione", IDAssicurazione);
                writer.WriteAttribute("NomeAssicurazione", m_NomeAssicurazione);
                writer.WriteAttribute("RapportoTFR_VN", m_RapportoTFR_VN);
                writer.WriteAttribute("Rating", m_Rating);
                writer.WriteAttribute("StatoAssicurazione", m_StatoAssicurazione);
                writer.WriteAttribute("Flags", m_Flags);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDAssicurazione":
                        {
                            m_IDAssicurazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAssicurazione":
                        {
                            m_NomeAssicurazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RapportoTFR_VN":
                        {
                            m_RapportoTFR_VN = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rating":
                        {
                            m_Rating = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoAssicurazione":
                        {
                            m_StatoAssicurazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}