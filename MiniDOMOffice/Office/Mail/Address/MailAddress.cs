using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Indirizzo email associato ad un messaggio ricevuto o inviato
        /// </summary>
        [Serializable]
        public class MailAddress 
            : minidom.Databases.DBObjectBase
        {
            private int m_MessageID;
            [NonSerialized] private MailApplication m_Application;
            [NonSerialized] private MailMessage m_Message;
            private string m_FieldName;
            private string m_Address;
            private string m_DisplayName;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAddress()
            {
                m_Application = null;
                m_MessageID = 0;
                m_Message = null;
                m_FieldName = "";
                m_Address = "";
                m_DisplayName = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="address"></param>
            /// <param name="displayName"></param>
            public MailAddress(string address, string displayName) 
                : this()
            {
                m_Address = DMD.Strings.Trim(address);
                m_DisplayName = DMD.Strings.Trim(displayName);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="msg"></param>
            /// <param name="fieldName"></param>
            public MailAddress(MailMessage msg, string fieldName)
            {
                if (msg is null)
                    throw new ArgumentNullException("msg");
                fieldName = Strings.LCase(Strings.Trim(fieldName));
                if (string.IsNullOrEmpty(fieldName))
                    throw new ArgumentNullException("fieldName");
                m_Application = null;
                m_MessageID = DBUtils.GetID(msg, 0);
                m_Message = msg;
                m_FieldName = fieldName;
                m_Address = "";
                m_DisplayName = "";
            }

            /// <summary>
            /// Restituisce un riferimento all'applicazione
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    return this.GetApplication();
                }
            }

            /// <summary>
            /// Restituisce un riferimento all'applicazione
            /// </summary>
            /// <returns></returns>
            protected virtual MailApplication GetApplication()
            {
                return (this.m_Message is null) ? this.m_Application : this.m_Message.Application;
            }

            /// <summary>
            /// Imposta l'applicazione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetApplication(MailApplication value)
            {
                m_Application = value;
            }

            /// <summary>
            /// ID del messaggio
            /// </summary>
            public int MessageID
            {
                get
                {
                    return DBUtils.GetID(m_Message, m_MessageID);
                }

                set
                {
                    int oldValue = MessageID;
                    if (oldValue == value)
                        return;
                    m_Message = null;
                    m_MessageID = value;
                    DoChanged("MessageID", value, oldValue);
                }
            }

            /// <summary>
            /// Messaggio
            /// </summary>
            public MailMessage Message
            {
                get
                {
                    if (m_Message is null)
                        m_Message = Application.GetMessageById(m_MessageID);
                    return m_Message;
                }

                set
                {
                    var oldValue = m_Message;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Message = value;
                    m_MessageID = DBUtils.GetID(value, 0);
                    DoChanged("Message", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il messaggio
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetMessage(MailMessage value)
            {
                m_Message = value;
                m_MessageID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Nome del campo (C, CC, TO o CCN)
            /// </summary>
            public string FieldName
            {
                get
                {
                    return m_FieldName;
                }

                set
                {
                    string oldValue = m_FieldName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FieldName = value;
                    DoChanged("FieldName", value, oldValue);
                }
            }

            /// <summary>
            /// Indirizzo
            /// </summary>
            public string Address
            {
                get
                {
                    return m_Address;
                }

                set
                {
                    string oldValue = m_Address;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Address = value;
                    DoChanged("Address", value, oldValue);
                }
            }

            /// <summary>
            /// Nome visualizzato
            /// </summary>
            public string DisplayName
            {
                get
                {
                    return m_DisplayName;
                }

                set
                {
                    string oldValue = m_DisplayName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DisplayName = value;
                    DoChanged("DisplayName", value, oldValue);
                }
            }

            /// <summary>
            /// Host
            /// </summary>
            public string Host
            {
                get
                {
                    string str = Address;
                    int p = str.LastIndexOf("@");
                    if (p > 0)
                        return str.Substring(p);
                    return DMD.Strings.vbNullString;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Mails.MailAddressies;
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_MailAddresses";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_MessageID = reader.Read("MessageID", m_MessageID);
                m_FieldName = reader.Read("FieldName",  m_FieldName);
                m_Address = reader.Read("Address", m_Address);
                m_DisplayName = reader.Read("DisplayName",  m_DisplayName);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("MessageID", MessageID);
                writer.Write("FieldName", m_FieldName);
                writer.Write("Address", m_Address);
                writer.Write("DisplayName", m_DisplayName);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("MessageID", typeof(int), 1);
                c = table.Fields.Ensure("FieldName", typeof(string), 255);
                c = table.Fields.Ensure("Address", typeof(string), 255);
                c = table.Fields.Ensure("DisplayName", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxMessaggio", new string[] { "MessageID", "FieldName", "Address", "DisplayName" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("MessageID", MessageID);
                writer.WriteAttribute("FieldName", m_FieldName);
                writer.WriteAttribute("Address", m_Address);
                writer.WriteAttribute("DisplayName", m_DisplayName);
                base.XMLSerialize(writer);
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
                    case "MessageID":
                        {
                            m_MessageID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "FieldName":
                        {
                            m_FieldName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Address":
                        {
                            m_Address = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DisplayName":
                        {
                            m_DisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder(1024);
                if (!string.IsNullOrEmpty(m_DisplayName))
                {
                    ret.Append(m_DisplayName);
                    ret.Append(" <");
                    ret.Append(m_Address);
                    ret.Append(">");
                }
                else
                {
                    ret.Append(m_Address);
                }

                return ret.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Address);
            }

            /// <summary>
            /// Restitusice true se due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is MailAddress) && this.Equals((MailAddress)obj);
            }

            /// <summary>
            /// Restitusice true se due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MailAddress obj)
            {
                if (!(obj is MailAddress) || obj is null)
                    return false;
                {
                    var withBlock = (MailAddress)obj;
                    return DMD.Strings.Compare(m_FieldName, withBlock.m_FieldName, true) == 0 && DMD.Strings.Compare(m_Address, withBlock.m_Address, true) == 0 && DMD.Strings.Compare(m_DisplayName, withBlock.m_DisplayName, true) == 0;
                }
            }
        }
    }
}