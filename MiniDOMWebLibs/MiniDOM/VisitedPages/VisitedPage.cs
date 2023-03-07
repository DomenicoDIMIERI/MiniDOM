using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class VisitedPage : Databases.DBObjectBase
        {
            private int m_SessionID;
            [NonSerialized]
            private CSiteSession m_Session;
            private int m_UserID;
            [NonSerialized]
            private Sistema.CUser m_User;
            private string m_UserName;
            private DateTime m_Data;
            private bool m_Secure;
            private string m_Protocol;
            private string m_SiteName;
            private string m_PageName;
            private string m_QueryString;
            private string m_PostedData;
            private float m_ExecTime;    // Tempo in secondi che ha impiegato la pagina per essere eseguita
            private string m_StatusCode;
            private string m_StatusDescription;
            private string m_Referrer;
            private string m_IDAnnuncio;      // Campo di utilità usato per collegare la pagina ad una fonte (per le statistiche)
                                              // Private m_ReferrerDomain As String  'Restituisce solo la parte relativa al dominio principale del referrer

            public VisitedPage()
            {
                m_SessionID = 0;
                m_Session = null;
                m_UserID = 0;
                m_User = null;
                m_UserName = "";
                m_Data = DMD.DateUtils.Now();
                m_Secure = false;
                m_Protocol = "";
                m_SiteName = "";
                m_PageName = "";
                m_QueryString = "";
                m_PostedData = "";
                m_Referrer = "";
                m_ExecTime = 0.0f;
                m_StatusCode = "";
                m_StatusDescription = "";
                m_IDAnnuncio = "";
            }

            /// <summary>
        /// Restituisce o imposta il valore di un campo di utilità utilizzabile per collegare la visita alla pagina ad un annuncio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IDAnnuncio
            {
                get
                {
                    return m_IDAnnuncio;
                }

                set
                {
                    string oldValue = m_IDAnnuncio;
                    value = DMD.Strings.Trim(value);
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_IDAnnuncio = value;
                    DoChanged("IDAnnuncio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il codice che indica lo stato della richiesta (200 = "OK", 400, 500, ...)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string StatusCode
            {
                get
                {
                    return m_StatusCode;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_StatusCode;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_StatusCode = value;
                    DoChanged("StatusCode", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che descrive lo stato della richiesta in maniera pià dettagliata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string StatusDescription
            {
                get
                {
                    return m_StatusDescription;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_StatusDescription;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_StatusDescription = value;
                    DoChanged("StatusDescription", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta il tempo di esecuzione della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public float ExecTime
            {
                get
                {
                    return m_ExecTime;
                }

                set
                {
                    double oldValue = m_ExecTime;
                    if (oldValue == value)
                        return;
                    m_ExecTime = value;
                    DoChanged("ExecTime", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la URL completa della pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Location
            {
                get
                {
                    string qs, ret;
                    ret = PageName;
                    qs = QueryString;
                    if (!string.IsNullOrEmpty(qs))
                        ret = ret + "?" + qs;
                    return ret;
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la pagina è stata servita su connessione sicura (HTTPS)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Secure
            {
                get
                {
                    return m_Secure;
                }

                set
                {
                    if (m_Secure == value)
                        return;
                    m_Secure = value;
                    DoChanged("Secure", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il protocollo di accesso alla pagina (HTTP)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Protocol
            {
                get
                {
                    return m_Protocol;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Protocol;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Protocol = value;
                    DoChanged("Protocol", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del server che ha servito la pagina (dominio principale)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string SiteName
            {
                get
                {
                    return m_SiteName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SiteName;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SiteName = value;
                    DoChanged("SiteName", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della pagina (percorso relativo)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PageName
            {
                get
                {
                    return m_PageName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_PageName;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_PageName = value;
                    DoChanged("PageName", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta le informazioni inviate tramite il metodo GET
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string QueryString
            {
                get
                {
                    return m_QueryString;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_QueryString;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_QueryString = value;
                    DoChanged("QueryString", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce un singolo parametro della querystring
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetQueryStringValue(string name)
            {
                return VisitedPages.GetQueryStringValue(QueryString, name);
            }

            /// <summary>
        /// Restituisce o imposta le informazioni inviate tramite il metodo POST
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PostedData
            {
                get
                {
                    return m_PostedData;
                }

                set
                {
                    string oldValue = m_PostedData;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_PostedData = value;
                    DoChanged("PostedData", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la URL completa della pagina da cui è avvenuto l'accesso a questa pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Referrer
            {
                get
                {
                    return m_Referrer;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Referrer;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Referrer = value;
                    DoChanged("Referrer", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il dominio principale del referrer
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ReferrerDomain
            {
                get
                {
                    string page = m_Referrer;
                    if (string.IsNullOrEmpty(page))
                        return "";
                    int i = Strings.InStr(page, "://");
                    if (i > 0)
                        page = Strings.Trim(Strings.Mid(page, i + 3));
                    i = Strings.InStr(page, "?");
                    if (i == 1)
                        page = "";
                    if (i > 1)
                        page = Strings.Trim(Strings.Left(page, i - 1));
                    i = Strings.InStr(page, "/");
                    if (i == 1)
                        page = "";
                    if (i > 1)
                        page = Strings.Trim(Strings.Left(page, i - 1));
                    i = Strings.InStrRev(page, ".");
                    if (i > 0)
                    {
                        i = Strings.InStrRev(page, ".", i - 1);
                        if (i > 0)
                            page = Strings.Trim(Strings.Mid(page, i + 1));
                    }

                    return page;
                }
            }

            /// <summary>
        /// Restituisce o imposta la data e l'ora in cui la pagina è stata richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'ID della sessione (nel LOG) in cui è stata richiesta la pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int SessionID
            {
                get
                {
                    return Databases.GetID(m_Session, m_SessionID);
                }

                set
                {
                    int oldValue = SessionID;
                    if (oldValue == value)
                        return;
                    m_SessionID = value;
                    m_Session = null;
                    DoChanged("SessionID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la sessione in cui è stata richiesta la pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CSiteSession Session
            {
                get
                {
                    if (m_Session is null)
                        m_Session = Sessions.GetItemById(m_SessionID);
                    return m_Session;
                }

                set
                {
                    var oldValue = Session;
                    if (oldValue == value)
                        return;
                    m_Session = value;
                    m_SessionID = Databases.GetID(value);
                    DoChanged("Session", value, oldValue);
                }
            }

            protected internal void SetSession(CSiteSession value)
            {
                m_Session = value;
                m_SessionID = Databases.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha richiesto la pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (oldValue == value)
                        return;
                    m_User = value;
                    m_UserID = Databases.GetID(value);
                    if (value is object)
                        m_UserName = value.Nominativo;
                    DoChanged("User", value, oldValue);
                }
            }

            protected internal void SetUser(Sistema.CUser value)
            {
                m_User = value;
                m_UserID = Databases.GetID(value);
                m_UserName = DMD.Strings.vbNullString;
                if (value is object)
                    m_UserName = value.Nominativo;
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha richiesto la pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int UserID
            {
                get
                {
                    return Databases.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_User = null;
                    m_UserID = value;
                    DoChanged("UserID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nominativo dell'utente che ha richiesto la pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string UserName
            {
                get
                {
                    return m_UserName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_UserName;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_UserName = value;
                    DoChanged("UserName", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la data dell'ultima modifica effettuata al file. Il nome del file deve essere un percorso mappato e non una URL
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime GetFileLastModified()
            {
                return Sistema.FileSystem.GetLastAccessTime(PageName);
            }

            public override CModulesClass GetModule()
            {
                return VisitedPages.Module;
            }

            public int CountVisits()
            {
                return Sistema.Formats.ToInteger(GetConnection().ExecuteScalar("SELECT Count(*) FROM [" + GetTableName() + "] WHERE [ScriptName]=" + Databases.DBUtils.DBString(PageName)));
            }

            public override string GetTableName()
            {
                return "tbl_VisitedPages";
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                m_SessionID = reader.Read("Session", this.m_SessionID);
                m_UserID = reader.Read("UserID", this.m_UserID);
                m_UserName = reader.Read("UserName", this.m_UserName);
                m_Data = reader.Read("Data", this.m_Data);
                m_Secure = reader.Read("Secure", this.m_Secure);
                m_Protocol = reader.Read("Protocol", this.m_Protocol);
                m_SiteName = reader.Read("SiteName", this.m_SiteName);
                m_PageName = reader.Read("ScriptName", this.m_PageName);
                m_QueryString = reader.Read("QueryString", this.m_QueryString);
                m_PostedData = reader.Read("PostedData", this.m_PostedData);
                m_Referrer = reader.Read("Referrer1", this.m_Referrer);
                if (string.IsNullOrEmpty(this.m_Referrer))
                    this.m_Referrer = reader.Read("Referrer", this.m_Referrer);
                m_ExecTime = reader.Read("ExecTime", this.m_ExecTime);
                m_StatusDescription = reader.Read("PageStatus", this.m_StatusDescription);
                m_StatusCode = reader.Read("PageStatusCode", this.m_StatusCode);
                m_IDAnnuncio = reader.Read("IDAnnunction", this.m_IDAnnuncio);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("Session", SessionID);
                writer.Write("UserID", UserID);
                writer.Write("UserName", m_UserName);
                writer.Write("Data", m_Data);
                writer.Write("Secure", m_Secure);
                writer.Write("Protocol", m_Protocol);
                writer.Write("SiteName", m_SiteName);
                writer.Write("ScriptName", m_PageName);
                writer.Write("QueryString", m_QueryString);
                writer.Write("PostedData", m_PostedData);
                writer.Write("Referrer", Strings.Left(m_Referrer, 255));
                writer.Write("Referrer1", m_Referrer);
                writer.Write("ExecTime", m_ExecTime);
                writer.Write("PageStatus", m_StatusDescription);
                writer.Write("PageStatusCode", m_StatusCode);
                writer.Write("IDAnnunction", m_IDAnnuncio);
                writer.Write("ReferrerDomain", ReferrerDomain);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Session", SessionID);
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("UserName", m_UserName);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("Secure", m_Secure);
                writer.WriteAttribute("Protocol", m_Protocol);
                writer.WriteAttribute("SiteName", m_SiteName);
                writer.WriteAttribute("ScriptName", m_PageName);
                writer.WriteAttribute("ExecTime", m_ExecTime);
                writer.WriteAttribute("Referrer", m_Referrer);
                writer.WriteAttribute("PageStatus", m_PageName);
                writer.WriteAttribute("PageStatusCode", m_StatusCode);
                writer.WriteAttribute("IDAnnuncio", m_IDAnnuncio);
                base.XMLSerialize(writer);
                writer.WriteTag("QueryString", m_QueryString);
                writer.WriteTag("PostedData", m_PostedData);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "Session", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SessionID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "UserID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "UserName", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "Data", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "Secure", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Secure = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "Protocol", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Protocol = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "SiteName", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SiteName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case7 when CultureInfo.CurrentCulture.CompareInfo.Compare(case7, "ScriptName", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_PageName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case8 when CultureInfo.CurrentCulture.CompareInfo.Compare(case8, "QueryString", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_QueryString = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case9 when CultureInfo.CurrentCulture.CompareInfo.Compare(case9, "PostedData", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_PostedData = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case10 when CultureInfo.CurrentCulture.CompareInfo.Compare(case10, "Referrer", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Referrer = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case11 when CultureInfo.CurrentCulture.CompareInfo.Compare(case11, "ExecTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_ExecTime = (float)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case var case12 when CultureInfo.CurrentCulture.CompareInfo.Compare(case12, "PageStatus", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_StatusDescription = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case13 when CultureInfo.CurrentCulture.CompareInfo.Compare(case13, "PageStatusCode", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_StatusCode = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case14 when CultureInfo.CurrentCulture.CompareInfo.Compare(case14, "IDAnnuncio", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_IDAnnuncio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            public override string ToString()
            {
                return Location;
            }
        }
    }
}