using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class CSiteSession : Databases.DBObjectBase
        {
            private string m_SessionID;
            private DateTime m_StartTime;
            private DateTime? m_EndTime;
            private string m_RemoteIP;
            private int m_RemotePort;
            private string m_UserAgent;
            private string m_Cookie;
            private string m_InitialReferrer;
            private CKeyCollection m_Parameters;

            public CSiteSession()
            {
                m_SessionID = "";
                m_StartTime = DMD.DateUtils.Now();
                m_EndTime = default;
                m_RemoteIP = "";
                m_RemotePort = 0;
                m_UserAgent = "";
                m_Cookie = "";
                m_InitialReferrer = "";
                m_Parameters = null;
            }

            public string SessionID
            {
                get
                {
                    return m_SessionID;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SessionID;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SessionID = value;
                    DoChanged("SessionID", value, oldValue);
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return m_StartTime;
                }

                set
                {
                    var oldValue = m_StartTime;
                    if (oldValue == value)
                        return;
                    m_StartTime = value;
                    DoChanged("StartTime", value, oldValue);
                }
            }

            public DateTime? EndTime
            {
                get
                {
                    return m_EndTime;
                }

                set
                {
                    var oldValue = m_EndTime;
                    if (oldValue == value == true)
                        return;
                    m_EndTime = value;
                    DoChanged("EndTime", value, oldValue);
                }
            }

            public string RemoteIP
            {
                get
                {
                    return m_RemoteIP;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_RemoteIP;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_RemoteIP = value;
                    DoChanged("RemoteIP", value, oldValue);
                }
            }

            public int RemotePort
            {
                get
                {
                    return m_RemotePort;
                }

                set
                {
                    int oldValue = m_RemotePort;
                    if (oldValue == value)
                        return;
                    m_RemotePort = value;
                    DoChanged("RemotePort", value, oldValue);
                }
            }

            public string UserAgent
            {
                get
                {
                    return m_UserAgent;
                }

                set
                {
                    string oldValue = m_UserAgent;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_UserAgent = value;
                    DoChanged("UserAgent", value, oldValue);
                }
            }

            public string Cookie
            {
                get
                {
                    return m_Cookie;
                }

                set
                {
                    string oldValue = m_Cookie;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Cookie = value;
                    DoChanged("Cookie", value, oldValue);
                }
            }

            public string InitialReferrer
            {
                get
                {
                    return m_InitialReferrer;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_InitialReferrer;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_InitialReferrer = value;
                    DoChanged("InitialReferrer", value, oldValue);
                }
            }

            public CKeyCollection Parameters
            {
                get
                {
                    if (m_Parameters is null)
                        m_Parameters = new CKeyCollection();
                    return m_Parameters;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            public override Sistema.CModule GetModule()
            {
                return Sessions.Module;
            }

            public override string GetTableName()
            {
                return "tbl_SiteSessions";
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                m_SessionID = reader.Read("SessionID", ref m_SessionID);
                m_StartTime = reader.Read("StartTime", ref m_StartTime);
                m_EndTime = reader.Read("EndTime", ref m_EndTime);
                m_RemoteIP = reader.Read("RemoteIP", ref m_RemoteIP);
                m_RemotePort = reader.Read("RemotePort", ref m_RemotePort);
                m_UserAgent = reader.Read("UserAgent", ref m_UserAgent);
                m_Cookie = reader.Read("Cookie", ref m_Cookie);
                m_InitialReferrer = reader.Read("InitialReferrer1", ref m_InitialReferrer);
                if (string.IsNullOrEmpty(m_InitialReferrer))
                    m_InitialReferrer = reader.Read("InitialReferrer", ref m_InitialReferrer);
                string argvalue = "";
                string tmp = reader.Read("Parameters", ref argvalue);
                if (!string.IsNullOrEmpty(tmp))
                    m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("SessionID", m_SessionID);
                writer.Write("StartTime", m_StartTime);
                writer.Write("EndTime", m_EndTime);
                writer.Write("RemoteIP", m_RemoteIP);
                writer.Write("RemotePort", m_RemotePort);
                writer.Write("UserAgent", m_UserAgent);
                writer.Write("Cookie", m_Cookie);
                writer.Write("InitialReferrer", Strings.Left(m_InitialReferrer, 255));
                writer.Write("InitialReferrer1", m_InitialReferrer);
                writer.Write("Parameters", DMD.XML.Utils.Serializer.Serialize(Parameters));
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XML.XMLWriter writer)
            {
                writer.WriteAttribute("SessionID", m_SessionID);
                writer.WriteAttribute("StartTime", m_StartTime);
                writer.WriteAttribute("EndTime", m_EndTime);
                writer.WriteAttribute("RemoteIP", m_RemoteIP);
                writer.WriteAttribute("RemotePort", m_RemotePort);
                writer.WriteAttribute("Cookie", m_Cookie);
                writer.WriteAttribute("InitialReferrer", m_InitialReferrer);
                base.XMLSerialize(writer);
                writer.WriteTag("Parameters", Parameters);
                writer.WriteTag("UserAgent", m_UserAgent);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "SessionID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SessionID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "StartTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_StartTime = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "EndTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_EndTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "RemoteIP", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_RemoteIP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "RemotePort", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_RemotePort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "UserAgent", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UserAgent = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "Cookie", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Cookie = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case7 when CultureInfo.CurrentCulture.CompareInfo.Compare(case7, "InitialReferrer", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_InitialReferrer = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case8 when CultureInfo.CurrentCulture.CompareInfo.Compare(case8, "Parameters", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Parameters = (CKeyCollection)fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                return DMD.Strings.JoinW("SiteSession[", ID.ToString(), "] (", DMD.Strings.CStr(StartTime), ", ", SessionID, ", ", UserAgent, ")");
            }
        }
    }
}