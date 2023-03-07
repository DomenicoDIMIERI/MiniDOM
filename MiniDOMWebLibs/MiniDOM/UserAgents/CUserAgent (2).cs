using System.Globalization;
using DMD;

namespace minidom
{
    public partial class WebSite
    {
        public class CUserAgent : Databases.DBObjectBase
        {
            private string m_UserAgent;
            private string m_DeviceType;
            private string m_Device;
            private int m_Bits;
            private string m_SistemaOperativo;
            private string m_VersioneSistemaOperativo;
            private string m_Browser;
            private string m_VersioneBrowser;

            public CUserAgent()
            {
                m_UserAgent = "";
                m_DeviceType = "";
                m_Device = "";
                m_Bits = 0;
                m_SistemaOperativo = "";
                m_VersioneSistemaOperativo = "";
                m_Browser = "";
                m_VersioneBrowser = "";
            }

            public string UserAgent
            {
                get
                {
                    return m_UserAgent;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_UserAgent;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_UserAgent = value;
                    DoChanged("UserAgent", value, oldValue);
                }
            }

            public string DeviceType
            {
                get
                {
                    return m_DeviceType;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_DeviceType;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_DeviceType = value;
                    DoChanged("DeviceType", value, oldValue);
                }
            }

            public string Device
            {
                get
                {
                    return m_Device;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Device;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Device = value;
                    DoChanged("Device", value, oldValue);
                }
            }

            public int Bits
            {
                get
                {
                    return m_Bits;
                }

                set
                {
                    int oldValue = m_Bits;
                    if (oldValue == value)
                        return;
                    m_Bits = value;
                    DoChanged("Bits", value, oldValue);
                }
            }

            public string SistemaOperativo
            {
                get
                {
                    return m_SistemaOperativo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SistemaOperativo;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SistemaOperativo = value;
                    DoChanged("SistemaOperativo", value, oldValue);
                }
            }

            public string VersioneSistemaOperativo
            {
                get
                {
                    return m_VersioneSistemaOperativo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_VersioneSistemaOperativo;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_VersioneSistemaOperativo = value;
                    DoChanged("VersioneSistemaOperativo", value, oldValue);
                }
            }

            public string Browser
            {
                get
                {
                    return m_Browser;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Browser;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Browser = value;
                    DoChanged("Browser", value, oldValue);
                }
            }

            public string VersioneBrowser
            {
                get
                {
                    return m_VersioneBrowser;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_VersioneBrowser;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_VersioneBrowser = value;
                    DoChanged("VersioneBrowser", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            public override Sistema.CModule GetModule()
            {
                return UserAgents.Module;
            }

            public override string GetTableName()
            {
                return "tbl_UserAgents";
            }

            public override string ToString()
            {
                return m_UserAgent;
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                reader.Read("UserAgent", ref m_UserAgent);
                reader.Read("DeviceType", ref m_DeviceType);
                reader.Read("Device", ref m_Device);
                reader.Read("Bits", ref m_Bits);
                reader.Read("SistemaOperativo", ref m_SistemaOperativo);
                reader.Read("VersioneSistemaOperativo", ref m_VersioneSistemaOperativo);
                reader.Read("Browser", ref m_Browser);
                reader.Read("VersioneBrowser", ref m_VersioneBrowser);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("UserAgent", m_UserAgent);
                writer.Write("Device", m_Device);
                writer.Write("DeviceType", m_DeviceType);
                writer.Write("Bits", m_Bits);
                writer.Write("SistemaOperativo", m_SistemaOperativo);
                writer.Write("VersioneSistemaOperativo", m_VersioneSistemaOperativo);
                writer.Write("Browser", m_Browser);
                writer.Write("VersioneBrowser", m_VersioneBrowser);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XML.XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("UserAgent", m_UserAgent);
                writer.WriteTag("Device", m_Device);
                writer.WriteTag("DeviceType", m_DeviceType);
                writer.WriteTag("Bits", m_Bits);
                writer.WriteTag("SistemaOperativo", m_SistemaOperativo);
                writer.WriteTag("VersioneSistemaOperativo", m_VersioneSistemaOperativo);
                writer.WriteTag("Browser", m_Browser);
                writer.WriteTag("VersioneBrowser", m_VersioneBrowser);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "UserAgent", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UserAgent = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "Device", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Device = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "DeviceType", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_DeviceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "Bits", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Bits = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "SistemaOperativo", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SistemaOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "VersioneSistemaOperativo", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_VersioneSistemaOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "Browser", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Browser = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case7 when CultureInfo.CurrentCulture.CompareInfo.Compare(case7, "VersioneBrowser", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_VersioneBrowser = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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