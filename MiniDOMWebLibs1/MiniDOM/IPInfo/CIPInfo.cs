using System.Globalization;
using DMD;

namespace minidom
{
    public partial class WebSite
    {
        public class CIPInfo : Databases.DBObjectBase
        {
            private string m_IP;
            private string m_Descrizione;

            public CIPInfo()
            {
                m_IP = "";
                m_Descrizione = "";
            }

            public string IP
            {
                get
                {
                    return m_IP;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IP;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_IP = value;
                    DoChanged("IP", value, oldValue);
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
                    value = Strings.Trim(value);
                    string oldValue = m_Descrizione;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            public override Sistema.CModule GetModule()
            {
                return IPInfo.Module;
            }

            public override string GetTableName()
            {
                return "tbl_IPInfo";
            }

            public override string ToString()
            {
                return "(" + m_IP + ", " + m_Descrizione + ")";
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                m_IP = reader.Read("IP", ref m_IP);
                m_Descrizione = reader.Read("Descrizione", ref m_Descrizione);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("IP", m_IP);
                writer.Write("Descrizione", m_Descrizione);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XML.XMLWriter writer)
            {
                writer.WriteAttribute("IP", m_IP);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "IP", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_IP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "Descrizione", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
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
        }
    }
}