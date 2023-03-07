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
    public partial class Messenger
    {

        /// <summary>
        /// Flag della relazione user x stanza
        /// </summary>
        public enum ChatUserFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Se true indica che l'utente é online
            /// </summary>
            Online = 1
            // Hidden = 2
        }

        /// <summary>
        /// Rappresenta le statistiche utente per una singola stanza
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CChatRoomUser 
            : minidom.Databases.DBObject 
        {
            //private ChatUserFlags m_Flags;
            private int m_IDStanza;
            [NonSerialized] private CChatRoom m_Stanza;
            private int m_UserID;
            [NonSerialized] private Sistema.CUser m_User;
            private DateTime? m_LastVisit;
            private DateTime? m_FirstVisit;
           
            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatRoomUser()
            {
                m_Flags = (int)ChatUserFlags.None;
                m_IDStanza = 0;
                m_Stanza = null;
                m_UserID = 0;
                m_User = null;
                m_LastVisit = default;
                m_FirstVisit = default;
                 
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new ChatUserFlags Flags
            {
                get
                {
                    return (ChatUserFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// IDStanza
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
            /// Stanza
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
                    DoChanged("Stanza", value, oldValue);
                }
            }

            /// <summary>
            /// UserID
            /// </summary>
            public int UserID
            {
                get
                {
                    return DBUtils.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_UserID = value;
                    m_User = null;
                    DoChanged("UserID", value, oldValue);
                }
            }

            /// <summary>
            /// User
            /// </summary>
            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = minidom.Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value, 0);
                    DoChanged("User", value, oldValue);
                }
            }

            /// <summary>
            /// FirstVisit
            /// </summary>
            public DateTime? FirstVisit
            {
                get
                {
                    return m_FirstVisit;
                }

                set
                {
                    var oldValue = m_FirstVisit;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_FirstVisit = value;
                    DoChanged("FirstVisit", value, oldValue);
                }
            }

            /// <summary>
            /// LastVisit
            /// </summary>
            public DateTime? LastVisit
            {
                get
                {
                    return m_LastVisit;
                }

                set
                {
                    var oldValue = m_LastVisit;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_LastVisit = value;
                    DoChanged("LastVisit", value, oldValue);
                }
            }

            /// <summary>
            /// Conta il numero di messaggi non letti nella stanza
            /// </summary>
            /// <returns></returns>
            public int CountUnreadMessages()
            {
                using (var cursor = new CMessagesCursor())
                {
                    cursor.TargetID.Value = 0;
                    cursor.TargetID.IncludeNulls = true;
                    // cursor.ReadTime.Value = Nothing
                    cursor.StatoMessaggio.ValueIn(new[] { StatoMessaggio.NonConsegnato, StatoMessaggio.NonLetto });
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStanza.Value = DBUtils.GetID(this, 0);
                    return (int)cursor.Count();
                }
            }

            /// <summary>
            /// Restituisce la collezione di tutti i messaggi non letti nella stanza
            /// </summary>
            /// <returns></returns>
            public CCollection<CMessage> GetUnreadMessages()
            {
                var ret = new CCollection<CMessage>();
                using (var cursor = new CMessagesCursor())
                {
                    cursor.TargetID.Value = 0;
                    cursor.TargetID.IncludeNulls = true;
                    // cursor.ReadTime.Value = Nothing
                    cursor.StatoMessaggio.ValueIn(new[] { StatoMessaggio.NonConsegnato, StatoMessaggio.NonLetto });
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStanza.Value = DBUtils.GetID(this, 0);
                    while (cursor.Read())
                    {
                        var m = cursor.Item;
                        m.SetStanza(Stanza);
                        ret.Add(m);
                    }
                }
                return ret;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Messenger.Rooms.UsersXRoom;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ChatRoomUsers";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDStanza = reader.Read("IDStanza", m_IDStanza);
                m_UserID = reader.Read("UserID", m_UserID);
                m_LastVisit = reader.Read("LastVisit", m_LastVisit);
                m_FirstVisit = reader.Read("FirstVisit", m_FirstVisit);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDStanza", IDStanza);
                writer.Write("UserID", UserID);
                writer.Write("LastVisit", m_LastVisit);
                writer.Write("FirstVisit", m_FirstVisit);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDStanza", typeof(int), 1);
                c = table.Fields.Ensure("UserID", typeof(int), 1);
                c = table.Fields.Ensure("LastVisit", typeof(DateTime), 1);
                c = table.Fields.Ensure("FirstVisit", typeof(DateTime), 1);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxStanza", new string[] { "IDStanza", "UserID", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDate", new string[] { "LastVisit", "FirstVisit" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("RoomID", IDStanza);
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("LastVisit", m_LastVisit);
                writer.WriteAttribute("FirstVisit", m_FirstVisit);
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
                   

                    case "IDStanza":
                        {
                            m_IDStanza = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserID":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "LastVisit":
                        {
                            m_LastVisit = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "FirstVisit":
                        {
                            m_FirstVisit = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
                return DMD.Strings.ConcatArray("{ ", this.m_IDStanza , ", ", this.m_UserID , " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDStanza);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CChatRoomUser) && this.Equals((CChatRoomUser)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CChatRoomUser obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDStanza, obj.m_IDStanza)
                    && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                    && DMD.DateUtils.EQ(this.m_LastVisit, obj.m_LastVisit)
                    && DMD.DateUtils.EQ(this.m_FirstVisit, obj.m_FirstVisit)
                    ;

            }
        }
    }
}