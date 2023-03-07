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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Evento registrato
        /// </summary>
        [Serializable]
        public class CEventLog
            : Databases.DBObjectBase, IComparable, IComparable<CEventLog>
        {
            private DateTime m_Data; // Data e ora in cui si è verificato l'evento
            private string m_Source; // Nome del modulo che ha generato l'evento
            private int m_UserID; // ID dell'utente nel cui contesto si è verificato l'evento
            [NonSerialized] private CUser m_User; // Utente nel cui contesto si è verificato l'evento
            private string m_UserName; // Nome dell'operatore	
            private string m_EventName; // Nome dell'evento
            private string m_Description; // Descrizione dell'evento
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CEventLog()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="data"></param>
            /// <param name="source"></param>
            /// <param name="user"></param>
            /// <param name="eventName"></param>
            /// <param name="description"></param>
            public CEventLog(DateTime data, string source, CUser user, string eventName, string description)
                : this(data, source, user, eventName, description, new CKeyCollection())
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="e"></param>
            public CEventLog(EventDescription e) 
                : this(e.Data, e.Module.ModuleName, e.Utente, e.EventName, e.Descrizione, e.Parametri)
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="data"></param>
            /// <param name="source"></param>
            /// <param name="user"></param>
            /// <param name="eventName"></param>
            /// <param name="description"></param>
            /// <param name="parameters"></param>
            public CEventLog(DateTime data, string source, CUser user, string eventName, string description, CKeyCollection parameters)
            {
                m_Data = data;
                m_Source = source;
                m_UserID = DBUtils.GetID(user, 0);
                m_User = user;
                m_UserName = user.Nominativo;
                m_EventName = eventName;
                m_Description = description;
                foreach(var k in parameters.Keys )
                {
                    this.Parameters.Add(k, parameters[k]);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Events;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CEventLog obj)
            {
                int ret = DMD.DateUtils.Compare(this.m_Data, obj.m_Data);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_Source, obj.m_Source, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_UserName, obj.m_UserName, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_EventName, obj.m_EventName, true);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((CEventLog)obj); }

            /// <summary>
            /// Data e ora in cui è stato generato l'evento
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
            }

            /// <summary>
            /// Nome della classe che ha generato l'evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Source
            {
                get
                {
                    return m_Source;
                }
            }

            /// <summary>
            /// Utente nel cui contesto è stato generato l'evento
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
            /// Restituisce il nome utente
            /// </summary>
            public string UserDisplayName
            {
                get
                {
                    return m_UserName;
                }
            }

            /// <summary>
            /// Nome dell'evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string EventName
            {
                get
                {
                    return m_EventName;
                }
            }

            /// <summary>
            /// Descrizione dell'evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Description
            {
                get
                {
                    return m_Description;
                }
            }

             

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_Data", m_Data);
                writer.WriteAttribute("m_Source", m_Source);
                writer.WriteAttribute("m_UserID", m_UserID);
                writer.WriteAttribute("m_UserName", m_UserName);
                writer.WriteAttribute("m_EventName", m_EventName);
                base.XMLSerialize(writer);
                writer.WriteTag("m_Description", m_Description);
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
                    case "m_Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "m_Source":
                        {
                            m_Source = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_UserID":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_EventName":
                        {
                            m_EventName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Description":
                        {
                            m_Description = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_EventsLog";
            }
             

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Data = reader.Read("Data", this.m_Data);
                m_Source = reader.Read("Source", this.m_Source);
                m_UserID = reader.Read("User", this.m_UserID);
                m_UserName = reader.Read("UserName", this.m_UserName);
                m_EventName = reader.Read("EventName", this.m_EventName);
                m_Description = reader.Read("Description", this.m_Description);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Data", m_Data);
                writer.Write("Source", m_Source);
                writer.Write("User", DBUtils.GetID(m_User, m_UserID));
                writer.Write("UserName", m_UserName);
                writer.Write("EventName", m_EventName);
                writer.Write("Description", m_Description);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("Source", typeof(string), 255);
                c = table.Fields.Ensure("User", typeof(int), 1);
                c = table.Fields.Ensure("UserName", typeof(string), 255);
                c = table.Fields.Ensure("EventName", typeof(string), 255);
                c = table.Fields.Ensure("Description", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxData", new string[] { "Data", "EventName" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "Source", "Description" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUser", new string[] { "User", "UserName" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che descrive l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.Data, " ", this.Source, ":", this.EventName, " ", this.UserDisplayName);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_Data, this.m_Source, this.m_EventName, this.m_UserID);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CEventLog) && this.Equals((CEventLog)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CEventLog obj)
            {
                return base.Equals(obj)
                     && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                     && DMD.Strings.EQ(this.m_Source, obj.m_Source)
                     && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                     && DMD.Strings.EQ(this.m_UserName, obj.m_UserName)
                     && DMD.Strings.EQ(this.m_EventName, obj.m_EventName)
                     && DMD.Strings.EQ(this.m_Description, obj.m_Description)
                     ;  
            }


        }
    }
}