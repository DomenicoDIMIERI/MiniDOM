using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{


    public partial class Messenger
    {

        /// <summary>
        /// Stato del messaggio
        /// </summary>
        public enum StatoMessaggio : int
        {
            /// <summary>
            /// Non consegnato
            /// </summary>
            NonConsegnato = 0,

            /// <summary>
            /// Non letto
            /// </summary>
            NonLetto = 1,

            /// <summary>
            /// Letto
            /// </summary>
            Letto = 2
        }

        /// <summary>
        /// Messaggio della chat
        /// </summary>
        [Serializable]
        public class CMessage
            : Databases.DBObject, IComparable, IComparable<CMessage>
        {
            private DateTime m_Time;           // [Date] Data e ora di invio del messaggio
            private int m_SourceID;
            [NonSerialized] private Sistema.CUser m_Source;
            private string m_SourceName;     // [Text] Nome del mittente
            private string m_SourceDescription; // [Text] Nominativo del mittente
            private int m_TargetID;
            [NonSerialized] private Sistema.CUser m_Target;
            private string m_TargetName; // [Text] Nome del destinatario
            private string m_Message; // [Text] Messaggio
            private bool m_Visible; // [Boolean]
            private DateTime? m_DeliveryTime; // [Date] Data ed ora di consegna
            private DateTime? m_ReadTime; // [Date] Data ed ora di lettura/chiusura
            private int m_SourceSession; // [Int] ID della sessione di invio
            private int m_TargetSession; // [Int] ID della sessione di ricezione
            private int m_IDStanza;
            [NonSerialized] private CChatRoom m_Stanza;
            private string m_NomeStanza;
            private StatoMessaggio m_StatoMessaggio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMessage()
            {
                m_Time = default;
                m_SourceID = 0;
                m_Source = null;
                m_SourceName = "";
                m_SourceDescription = "";
                m_TargetID = 0;
                m_Target = null;
                m_TargetName = "";
                m_Message = "";
                m_Visible = true;
                m_DeliveryTime = default;
                m_ReadTime = default;
                m_SourceSession = 0;
                m_TargetSession = 0;
                m_IDStanza = 0;
                m_Stanza = null;
                m_NomeStanza = "";
                m_StatoMessaggio = StatoMessaggio.NonConsegnato;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Messenger.Messages;
            }

            /// <summary>
            /// Restituisce o imposta lo stato del messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoMessaggio StatoMessaggio
            {
                get
                {
                    return m_StatoMessaggio;
                }

                set
                {
                    var oldValue = m_StatoMessaggio;
                    if (oldValue == value)
                        return;
                    m_StatoMessaggio = value;
                    DoChanged("StatoMessaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di invio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime Time
            {
                get
                {
                    return m_Time;
                }

                set
                {
                    var oldValue = m_Time;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_Time = value;
                    DoChanged("Time", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente sorgente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID(m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o impostal 'utente sorgente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Source
            {
                get
                {
                    if (m_Source is null)
                        m_Source = Sistema.Users.GetItemById(m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_SourceID = DBUtils.GetID(value, 0);
                    m_SourceName = "";
                    m_SourceDescription = "";
                    if (value is object)
                    {
                        m_SourceName = value.UserName;
                        m_SourceDescription = value.Nominativo;
                    }

                    DoChanged("Source", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha inviato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceName
            {
                get
                {
                    return m_SourceName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceName = value;
                    DoChanged("SourceName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome lungo dell'utente sorgente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceDescription
            {
                get
                {
                    return m_SourceDescription;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SourceDescription;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceDescription = value;
                    DoChanged("SourceDescription", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente destinatario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Target
            {
                get
                {
                    if (m_Target is null)
                        m_Target = Sistema.Users.GetItemById(m_TargetID);
                    return m_Target;
                }

                set
                {
                    var oldValue = Target;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Target = value;
                    m_TargetID = DBUtils.GetID(value, 0);
                    m_TargetName = "";
                    if (value is object)
                        m_TargetName = value.UserName;
                    DoChanged("Target", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int TargetID
            {
                get
                {
                    return DBUtils.GetID(m_Target, m_TargetID);
                }

                set
                {
                    int oldValue = TargetID;
                    if (oldValue == value)
                        return;
                    m_TargetID = value;
                    m_Target = null;
                    DoChanged("TargetID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente destinazione
            /// </summary>
            public string TargetName
            {
                get
                {
                    return m_TargetName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TargetName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TargetName = value;
                    DoChanged("TargetName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il corpo del messaggio
            /// </summary>
            public string Message
            {
                get
                {
                    return m_Message;
                }

                set
                {
                    string oldValue = m_Message;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Message = value;
                    DoChanged("Message", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di consegna del messaggio
            /// </summary>
            public DateTime? DeliveryTime
            {
                get
                {
                    return m_DeliveryTime;
                }

                set
                {
                    var oldValue = m_DeliveryTime;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DeliveryTime = value;
                    DoChanged("DeliveryTime", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di lettura del messaggio
            /// </summary>
            public DateTime? ReadTime
            {
                get
                {
                    return m_ReadTime;
                }

                set
                {
                    var oldValue = m_ReadTime;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_ReadTime = value;
                    DoChanged("ReadTime", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della sessione da cui é stato inviato il messaggio
            /// </summary>
            public int SourceSession
            {
                get
                {
                    return m_SourceSession;
                }

                set
                {
                    int oldValue = m_SourceSession;
                    if (oldValue == value)
                        return;
                    m_SourceSession = value;
                    DoChanged("SourceSession", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della sessione in cui é stato letto il messaggio
            /// </summary>
            public int TargetSession
            {
                get
                {
                    return m_TargetSession;
                }

                set
                {
                    int oldValue = m_TargetSession;
                    if (oldValue == value)
                        return;
                    m_TargetSession = value;
                    DoChanged("TargetSession", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della stanza in cui é stato inviato il messaggio
            /// </summary>
            public int IDStanza
            {
                get
                {
                    return DBUtils.GetID(m_Stanza, m_IDStanza);
                }

                set
                {
                    int oldValue = IDStanza;
                    if (oldValue == value)
                        return;
                    m_IDStanza = value;
                    m_Stanza = null;
                    DoChanged("IDStanza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la stanza in cui è stato inviato il messaggio
            /// </summary>
            public CChatRoom Stanza
            {
                get
                {
                    if (m_Stanza is null)
                        m_Stanza = minidom.Messenger.Rooms.GetItemById(m_IDStanza);
                    return m_Stanza;
                }

                set
                {
                    var oldValue = Stanza;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Stanza = value;
                    m_IDStanza = DBUtils.GetID(value, 0);
                    m_NomeStanza = "";
                    if (value is object)
                        m_NomeStanza = value.Name;
                    DoChanged("Stanza", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la stanza
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetStanza(CChatRoom value)
            {
                m_Stanza = value;
                m_IDStanza = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome della stanza in cui é stato inviato il messaggio
            /// </summary>
            public string NomeStanza
            {
                get
                {
                    return m_NomeStanza;
                }

                set
                {
                    string oldValue = m_NomeStanza;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeStanza = value;
                    DoChanged("NomeStanza", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Messenger";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Time = reader.Read("Time", m_Time);
                m_SourceID = reader.Read("SourceID", m_SourceID);
                m_SourceName = reader.Read("SourceName", m_SourceName);
                m_SourceDescription = reader.Read("SourceDescription", m_SourceDescription);
                m_TargetID = reader.Read("TargetID", m_TargetID);
                m_TargetName = reader.Read("TargetName", m_TargetName);
                m_Message = reader.Read("Message", m_Message);
                m_DeliveryTime = reader.Read("DeliveryTime", m_DeliveryTime);
                m_ReadTime = reader.Read("ReadTime", m_ReadTime);
                m_SourceSession = reader.Read("SourceSession", m_SourceSession);
                m_TargetSession = reader.Read("TargetSession", m_TargetSession);
                m_IDStanza = reader.Read("IDStanza", m_IDStanza);
                m_NomeStanza = reader.Read("Stanza",  m_NomeStanza);
                m_StatoMessaggio = reader.Read("StatoMessaggio",  m_StatoMessaggio);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Time", m_Time);
                //writer.Write("TimeStr", DBUtils.ToDBDateStr(m_Time));
                writer.Write("SourceID", SourceID);
                writer.Write("SourceName", m_SourceName);
                writer.Write("SourceDescription", m_SourceDescription);
                writer.Write("TargetID", TargetID);
                writer.Write("TargetName", m_TargetName);
                writer.Write("Message", m_Message);
                writer.Write("DeliveryTime", m_DeliveryTime);
                writer.Write("ReadTime", m_ReadTime);
                writer.Write("SourceSession", m_SourceSession);
                writer.Write("TargetSession", m_TargetSession);
                writer.Write("IDStanza", IDStanza);
                writer.Write("Stanza", m_Stanza);
                writer.Write("StatoMessaggio", m_StatoMessaggio);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Time", typeof(DateTime), 1);
                c = table.Fields.Ensure("SourceID", typeof(int), 1);
                c = table.Fields.Ensure("SourceName", typeof(string), 255);
                c = table.Fields.Ensure("SourceDescription", typeof(string), 255);
                c = table.Fields.Ensure("TargetID", typeof(int), 1);
                c = table.Fields.Ensure("TargetName", typeof(string), 255);
                c = table.Fields.Ensure("Message", typeof(string), 0);
                c = table.Fields.Ensure("DeliveryTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("ReadTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("SourceSession", typeof(int), 1);
                c = table.Fields.Ensure("TargetSession", typeof(int), 1);
                c = table.Fields.Ensure("IDStanza", typeof(int), 1);
                c = table.Fields.Ensure("Stanza", typeof(string), 255);
                c = table.Fields.Ensure("StatoMessaggio", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxTime", new string[] { "Time", "DeliveryTime" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceID", "SourceName", "SourceDescription" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTarget", new string[] { "TargetID", "TargetName"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMessage", new string[] { "Message" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "ReadTime", "StatoMessaggio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStanza", new string[] { "IDStanza", "Stanza" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSessions", new string[] { "SourceSession", "TargetSession" }, DBFieldConstraintFlags.None);
                 

            }


            /// <summary>
            /// Restituisce la stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Message;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Time", m_Time);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("SourceName", m_SourceName);
                writer.WriteAttribute("SourceDescription", m_SourceDescription);
                writer.WriteAttribute("TargetID", TargetID);
                writer.WriteAttribute("TargetName", m_TargetName);
                writer.WriteAttribute("DeliveryTime", m_DeliveryTime);
                writer.WriteAttribute("ReadTime", m_ReadTime);
                writer.WriteAttribute("SourceSession", m_SourceSession);
                writer.WriteAttribute("TargetSession", m_TargetSession);
                writer.WriteAttribute("IDStanza", IDStanza);
                writer.WriteAttribute("NomeStanza", m_NomeStanza);
                writer.WriteAttribute("StatoMessaggio", (int?)m_StatoMessaggio);
                base.XMLSerialize(writer);
                writer.WriteTag("Message", m_Message);
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
                    case "Time":
                        {
                            m_Time = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SourceName":
                        {
                            m_SourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceDescription":
                        {
                            m_SourceDescription = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TargetID":
                        {
                            m_TargetID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TargetName":
                        {
                            m_TargetName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Message":
                        {
                            m_Message = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DeliveryTime":
                        {
                            m_DeliveryTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ReadTime":
                        {
                            m_ReadTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "SourceSession":
                        {
                            m_SourceSession = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TargetSession":
                        {
                            m_TargetSession = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDStanza":
                        {
                            m_IDStanza = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeStanza":
                        {
                            m_NomeStanza = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoMessaggio":
                        {
                            m_StatoMessaggio = (StatoMessaggio)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CMessage obj)
            {
                int ret = DMD.DateUtils.Compare(this.m_Time, obj.m_Time);
                if (ret == 0) ret = DMD.Integers.Compare(this.m_SourceSession, obj.m_SourceSession);
                if (ret == 0) ret = DMD.Integers.Compare(this.m_TargetSession, obj.m_TargetSession);
                if (ret == 0) ret = DMD.Integers.Compare(this.m_SourceID, obj.m_SourceID);
                if (ret == 0) ret = DMD.Integers.Compare(this.m_TargetID, obj.m_TargetID);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CMessage)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CMessage) && this.Equals((CMessage)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CMessage obj)
            {
                return base.Equals(obj)
                        && DMD.DateUtils.EQ(this.m_Time, obj.m_Time)
                        && DMD.Integers.EQ(this.m_SourceID, obj.m_SourceID)
                        && DMD.Strings.EQ(this.m_SourceName, obj.m_SourceName)
                        && DMD.Integers.EQ(this.m_TargetID, obj.m_TargetID)
                        && DMD.Strings.EQ(this.m_TargetName, obj.m_TargetName)
                        && DMD.Strings.EQ(this.m_Message, obj.m_Message)
                        && DMD.Booleans.EQ(this.m_Visible, obj.m_Visible)
                        && DMD.DateUtils.EQ(this.m_DeliveryTime, obj.m_DeliveryTime)
                        && DMD.DateUtils.EQ(this.m_ReadTime, obj.m_ReadTime)
                        && DMD.Integers.EQ(this.m_SourceSession, obj.m_SourceSession)
                        && DMD.Integers.EQ(this.m_TargetSession, obj.m_TargetSession)
                        && DMD.Integers.EQ(this.m_IDStanza, obj.m_IDStanza)
                        && DMD.Strings.EQ(this.m_NomeStanza, obj.m_NomeStanza)
                        && DMD.Integers.EQ((int)this.m_StatoMessaggio, (int)obj.m_StatoMessaggio)
                        ;
            }
        }
    }
}