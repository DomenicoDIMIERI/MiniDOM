using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Tipi di logout
        /// </summary>
        public enum LogOutMethods : int
        {
            /// <summary>
            /// Informazioni sul logout sconosciute
            /// </summary>
            LOGOUT_UNKNOWN = 0,

            /// <summary>
            /// Logout effettuato dall'utente 
            /// </summary>
            LOGOUT_LOGOUT = 1,

            /// <summary>
            /// Logout effettuato dal sistema in per lo scadere della sessione
            /// </summary>
            LOGOUT_TIMEOUT = 2,

            /// <summary>
            /// Logout forzato da un altro utente o per altri motivi
            /// </summary>
            LOGOUT_REMOTEDISCONNECT = 3
        }


        /// <summary>
        /// Informazioni sull'accesso 
        /// </summary>
        [Serializable]
        public class CLoginHistory 
            : Databases.DBObjectBase
        {
            private int m_UserID;
            [NonSerialized] private CUser m_User;
            private DateTime? m_LogInTime;
            private DateTime? m_LogOutTime;
            private string m_RemoteIP;
            private int m_RemotePort;
            private string m_Session;
            private string m_UserAgent;
            private LogOutMethods m_LogoutMethod;
            private int m_IDUfficio;
            [NonSerialized] private CUfficio m_Ufficio;
            private string m_NomeUfficio;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CLoginHistory()
            {
                m_UserID = 0;
                m_User = null;
                m_LogInTime = default;
                m_LogOutTime = default;
                m_RemoteIP = "";
                m_RemotePort = 0;
                m_Session = "";
                m_UserAgent = "";
                m_LogoutMethod = LogOutMethods.LOGOUT_UNKNOWN;
                m_IDUfficio = 0;
                m_Ufficio = null;
                m_NomeUfficio = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="user"></param>
            /// <param name="parameters"></param>
            public CLoginHistory(CUser user, CKeyCollection parameters)
            {
                foreach (string k in parameters.Keys)
                    Parameters.SetItemByKey(k, parameters[k]);
                m_UserID = DBUtils.GetID(user, 0); // Session("UserID")
                m_User = user;
                m_LogInTime = DMD.DateUtils.Now();
                m_LogOutTime = default;
                m_Session = DMD.Strings.CStr(parameters.GetItemByKey("SessionID"));
                m_RemoteIP = DMD.Strings.CStr(parameters.GetItemByKey("RemoteIP"));
                m_RemotePort = Formats.ToInteger(parameters.GetItemByKey("RemotePort"));
                m_UserAgent = DMD.Strings.CStr(parameters.GetItemByKey("RemoteUserAgent"));
                m_NomeUfficio = DMD.Strings.CStr(parameters.GetItemByKey("CurrentUfficio"));
                m_Ufficio = Anagrafica.Uffici.GetItemByName(m_NomeUfficio);
                m_IDUfficio = DBUtils.GetID(m_Ufficio, 0);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray("{ ", this.User, " login: ", this.LogInTime, " logout: ", this.LogOutTime, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.User, this.LogInTime, this.LogOutTime);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CLoginHistory) && this.Equals((CLoginHistory)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CLoginHistory obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                    && DMD.DateUtils.EQ(this.m_LogInTime, obj.m_LogInTime)
                    && DMD.DateUtils.EQ(this.m_LogOutTime, obj.m_LogOutTime)
                    && DMD.Strings.EQ(this.m_RemoteIP, obj.m_RemoteIP)
                    && DMD.Integers.EQ(this.m_RemotePort, obj.m_RemotePort)
                    && DMD.Strings.EQ(this.m_Session, obj.m_Session)
                    && DMD.Strings.EQ(this.m_UserAgent, obj.m_UserAgent)
                    && DMD.Integers.EQ((int)this.m_LogoutMethod, (int)obj.m_LogoutMethod)
                    && DMD.Integers.EQ(this.m_IDUfficio, obj.m_IDUfficio)
                    && DMD.Strings.EQ(this.m_NomeUfficio, obj.m_NomeUfficio)
             ;
            }


            /// <summary>
            /// Restituisce vero se l'oggetto costituisce il login della sessione corrente
            /// </summary>
            /// <returns></returns>
            public bool IsActive()
            {
                var cl = Sistema.ApplicationContext.CurrentLogin;
                return (cl is object) && (cl.ID == this.ID);
            }

            /// <summary>
            /// Restituisce l'ID dell'ufficio da cui l'utente ha effettuato il login
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDUfficio
            {
                get
                {
                    return DBUtils.GetID(m_Ufficio, m_IDUfficio);
                }
            }

            /// <summary>
            /// Restituisce l'ufficio da cui l'utente ha effettuate l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUfficio Ufficio
            {
                get
                {
                    if (m_Ufficio is null)
                        m_Ufficio = Anagrafica.Uffici.GetItemById(m_IDUfficio);
                    return m_Ufficio;
                }
            }

            /// <summary>
            /// Restituisce il nome dell'ufficio da cui l'utente ha effettuato l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeUfficio
            {
                get
                {
                    return m_NomeUfficio;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Users.LoginHistories;
            }

            /// <summary>
            /// Restituisce l'ID dell'utente che ha effettuato l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int UserID
            {
                get
                {
                    return DBUtils.GetID(m_User, m_UserID);
                }
            }

            /// <summary>
            /// Restituisce l'utente che ha effettuate l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Users.GetItemById(m_UserID);
                    return m_User;
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUser(CUser value)
            {
                m_User = value;
                m_UserID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restitusice la data e l'ora di accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? LogInTime
            {
                get
                {
                    return m_LogInTime;
                }
            }

            /// <summary>
            /// Imposta la data e l'ora di ingresso
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetLogInTime(DateTime value)
            {
                m_LogInTime = value;
                DoChanged("LogInTime", value);
            }

            /// <summary>
            /// Restituisce la data e l'ora del logout
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? LogOutTime
            {
                get
                {
                    return m_LogOutTime;
                }
            }

            /// <summary>
            /// Imposta la data e l'ora di uscita
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetLogOutTime(DateTime value)
            {
                m_LogOutTime = value;
                DoChanged("LogOutTime", value);
            }

            /// <summary>
            /// Restituisce l'IP del dispositivo remoto da cui l'utente ha effettuato l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string RemoteIP
            {
                get
                {
                    return m_RemoteIP;
                }
            }

            /// <summary>
            /// Imposta l'IP della macchina di accesso
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetRemoteIP(string value)
            {
                m_RemoteIP = DMD.Strings.Trim(value);
                DoChanged("RemoteIP", value);
            }

            /// <summary>
            /// Restituisce la porta del dispositivo remoto da cui l'utente ha effettuato l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int RemotePort
            {
                get
                {
                    return m_RemotePort;
                }
            }

            /// <summary>
            /// Imposta la porta della macchina remota
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetRemotePort(int value)
            {
                m_RemotePort = value;
                DoChanged("RemotePort", value, null);
            }

            /// <summary>
            /// Restituisce la stringa identificativa della sessione iniziata dall'utente remoto sul sito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SessionID
            {
                get
                {
                    return m_Session;
                }
            }

            /// <summary>
            /// Imposta l'id della sessione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetSessionID(string value)
            {
                m_Session = DMD.Strings.Trim(value);
                DoChanged("SessionID", value, null);
            }

            /// <summary>
            /// Restituisce la useragent del browser con cui l'utente ha effettuato l'accesso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UserAgent
            {
                get
                {
                    return m_UserAgent;
                }
            }

            /// <summary>
            /// Imposta la stringa che identifica la macchina remota
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUserAgent(string value)
            {
                m_UserAgent = DMD.Strings.Trim(value);
                DoChanged("UserAgent", value, null);
            }

            /// <summary>
            /// Restituisce il metodo di uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public LogOutMethods LogoutMethod
            {
                get
                {
                    return m_LogoutMethod;
                }
            }

            /// <summary>
            /// Imposta il metodo di uscita
            /// </summary>
            /// <param name="value"></param>
            internal void SetLogoutMethod(LogOutMethods value)
            {
                m_LogoutMethod = value;
                DoChanged("LogoutMethod", value, null);
            }

             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_LoginHistory";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.LOGConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_UserID = reader.Read("User", this. m_UserID);
                m_LogInTime = reader.Read("LogInTime", this.m_LogInTime);
                m_LogOutTime = reader.Read("LogOutTime", this.m_LogOutTime);
                m_RemoteIP = reader.Read("RemoteIP", this.m_RemoteIP);
                m_RemotePort = reader.Read("RemotePort", this.m_RemotePort);
                m_Session = reader.Read("Session", this.m_Session);
                m_UserAgent = reader.Read("UserAgent", this.m_UserAgent);
                m_LogoutMethod = reader.Read("LogoutMethod", this.m_LogoutMethod);
                m_IDUfficio = reader.Read("IDUfficio", this.m_IDUfficio);
                m_NomeUfficio = reader.Read("NomeUfficio", this.m_NomeUfficio);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("User", this.UserID);
                writer.Write("LogInTime", m_LogInTime);
                writer.Write("LogOutTime", m_LogOutTime);
                writer.Write("RemoteIP", m_RemoteIP);
                writer.Write("RemotePort", m_RemotePort);
                writer.Write("Session", m_Session);
                writer.Write("UserAgent", m_UserAgent);
                writer.Write("LogoutMethod", m_LogoutMethod);
                writer.Write("IDUfficio", IDUfficio);
                writer.Write("NomeUfficio", m_NomeUfficio);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("User", typeof(int), 1);
                c = table.Fields.Ensure("LogInTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("LogOutTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("RemoteIP", typeof(string), 255);
                c = table.Fields.Ensure("RemotePort", typeof(int), 1);
                c = table.Fields.Ensure("Session", typeof(string), 255);
                c = table.Fields.Ensure("UserAgent", typeof(string), 0);
                c = table.Fields.Ensure("LogoutMethod", typeof(int), 1);
                c = table.Fields.Ensure("IDUfficio", typeof(int), 1);
                c = table.Fields.Ensure("NomeUfficio", typeof(string), 255);

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUser", new string[] { "User", "LogoutMethod" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDates", new string[] { "LogInTime", "LogOutTime" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRemote", new string[] { "RemoteIP", "RemotePort" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSession", new string[] { "Session" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUserAgent", new string[] { "UserAgent" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUfficio", new string[] { "IDUfficio", "NomeUfficio" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("LogInTime", m_LogInTime);
                writer.WriteAttribute("LogOutTime", m_LogOutTime);
                writer.WriteAttribute("RemoteIP", m_RemoteIP);
                writer.WriteAttribute("RemotePort", m_RemotePort);
                writer.WriteAttribute("Session", m_Session);
                writer.WriteAttribute("LogoutMethod", (int?)m_LogoutMethod);
                writer.WriteAttribute("IDUfficio", IDUfficio);
                writer.WriteAttribute("NomeUfficio", m_NomeUfficio);
                base.XMLSerialize(writer);
                writer.WriteTag("UserAgent", m_UserAgent);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "UserID":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "LogInTime":
                        {
                            m_LogInTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "LogOutTime":
                        {
                            m_LogOutTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RemoteIP":
                        {
                            m_RemoteIP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RemotePort":
                        {
                            m_RemotePort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Session":
                        {
                            m_Session = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UserAgent":
                        {
                            m_UserAgent = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LogoutMethod":
                        {
                            m_LogoutMethod = (LogOutMethods)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUfficio":
                        {
                            m_IDUfficio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUfficio":
                        {
                            m_NomeUfficio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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