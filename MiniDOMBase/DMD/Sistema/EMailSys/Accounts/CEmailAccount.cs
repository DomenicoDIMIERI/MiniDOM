using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Account configurato su un server web
        /// </summary>
        [Serializable]
        public class CEmailAccount
            : Databases.DBObject
        {
            private bool m_Attivo;
            private string m_AccountType;
            private string m_AccountName;
            private string m_POPServer;
            private int m_POPPort;
            private string m_POPUserName;
            private string m_POPPassword;
            private bool m_POPUseSSL;
            private DateTime? m_LastSync;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEmailAccount()
            {
                m_Attivo = true;
                m_AccountName = "";
                m_AccountType = "POP3";
                m_POPServer = "localhost";
                m_POPPort = 110;
                m_POPUserName = "";
                m_POPPassword = "";
                m_POPUseSSL = false;
                m_LastSync = default;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="userName"></param>
            /// <param name="password"></param>
            public CEmailAccount(string userName, string password) : this()
            {
                m_POPUserName = DMD.Strings.Trim(userName);
                m_POPPassword = password;
                m_AccountName = m_POPUserName;
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora dell'ultima sincronizzazione avvenuta con il server
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? LastSync
            {
                get
                {
                    return m_LastSync;
                }

                set
                {
                    var oldValue = m_LastSync;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_LastSync = value;
                    DoChanged("LastSync", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se l'account è attivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo dell'account (POP3 e IMAP)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AccountType
            {
                get
                {
                    return m_AccountType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AccountType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AccountType = value;
                    DoChanged("AccountType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del server POP
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string POPServer
            {
                get
                {
                    return m_POPServer;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_POPServer;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_POPServer = value;
                    DoChanged("POPServer", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la porta del server POP
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int POPPort
            {
                get
                {
                    return m_POPPort;
                }

                set
                {
                    int oldValue = m_POPPort;
                    if (oldValue == value)
                        return;
                    m_POPPort = value;
                    DoChanged("POPPort", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la porta del server POP
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool POPUseSSL
            {
                get
                {
                    return m_POPUseSSL;
                }

                set
                {
                    if (m_POPUseSSL == value)
                        return;
                    m_POPUseSSL = value;
                    DoChanged("POPUseSSL", value, !value);
                }
            }


            /// <summary>
            /// Restituisce o imposta il nome utente per l'accesso al server POP
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string POPUserName
            {
                get
                {
                    return m_POPUserName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_POPUserName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_POPUserName = value;
                    DoChanged("POPUserName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la password per l'accesso al server POP
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string POPPassword
            {
                get
                {
                    return m_POPPassword;
                }

                set
                {
                    string oldValue = m_POPPassword;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_POPPassword = value;
                    DoChanged("POPPassword", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AccountName
            {
                get
                {
                    return m_AccountName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AccountName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AccountName = value;
                    DoChanged("AccountName", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.EMailer.MailAccounts;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_EmailAccounts";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Attivo = reader.Read("Attivo", m_Attivo);
                m_AccountName = reader.Read("AccountName", m_AccountName);
                m_AccountType = reader.Read("AccountType", m_AccountType);
                m_POPServer = reader.Read("POPServer", m_POPServer);
                m_POPPort = reader.Read("POPPort", m_POPPort);
                m_POPUserName = reader.Read("POPUserName", m_POPUserName);
                m_POPPassword = reader.Read("POPPassword", m_POPPassword);
                m_POPUseSSL = reader.Read("POPUseSSL", m_POPUseSSL);
                m_LastSync = reader.Read("LastSync", m_LastSync);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva sul db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Attivo", m_Attivo);
                writer.Write("AccountName", m_AccountName);
                writer.Write("AccountType", m_AccountType);
                writer.Write("POPServer", m_POPServer);
                writer.Write("POPPort", m_POPPort);
                writer.Write("POPUserName", m_POPUserName);
                writer.Write("POPPassword", m_POPPassword);
                writer.Write("POPUseSSL", m_POPUseSSL);
                writer.Write("LastSync", m_LastSync);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara il db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Attivo", typeof(bool), 1);
                c = table.Fields.Ensure("AccountName", typeof(string), 255);
                c = table.Fields.Ensure("AccountType", typeof(string), 255);
                c = table.Fields.Ensure("POPServer", typeof(string), 255);
                c = table.Fields.Ensure("POPPort", typeof(int), 1);
                c = table.Fields.Ensure("POPUserName", typeof(string), 255);
                c = table.Fields.Ensure("POPPassword", typeof(string), 255);
                c = table.Fields.Ensure("POPUseSSL", typeof(bool), 1);
                c = table.Fields.Ensure("LastSync", typeof(DateTime), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxAccountName", new string[] { "AccountName", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxParams", new string[] { "AccountType", "Attivo" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxPOP", new string[] { "POPServer", "POPPort", "POPUseSSL", "LastSync"  }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxUser", new string[] { "POPUserName", "POPPassword" }, DBFieldConstraintFlags.PrimaryKey);
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
                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "AccountName":
                        {
                            m_AccountName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AccountType":
                        {
                            m_AccountType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "POPServer":
                        {
                            m_POPServer = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "POPPort":
                        {
                            m_POPPort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "POPUserName":
                        {
                            m_POPUserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "POPPassword":
                        {
                            m_POPPassword = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "POPUseSSL":
                        {
                            m_POPUseSSL = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "LastSync":
                        {
                            m_LastSync = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Seralizzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("AccountName", m_AccountName);
                writer.WriteAttribute("AccountType", m_AccountType);
                writer.WriteAttribute("POPServer", m_POPServer);
                writer.WriteAttribute("POPPort", m_POPPort);
                writer.WriteAttribute("POPUserName", m_POPUserName);
                writer.WriteAttribute("POPPassword", m_POPPassword);
                writer.WriteAttribute("POPUseSSL", m_POPUseSSL);
                writer.WriteAttribute("LastSync", m_LastSync);
                base.XMLSerialize(writer);
            }

           
        }
    }
}