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
        /// Rappresenta una stanza della chat
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CChatRoom 
            : minidom.Databases.DBObject, IComparable, IComparable<CChatRoom>
        {
            private string m_Name;
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatRoom()
            {
                m_Name = "";                
            }

            /// <summary>
            /// Restituisce o imposta il nome della stanza
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }
             

            /// <summary>
            /// Restituisce la collezione dei membri che fanno parte del gruppo
            /// </summary>
            /// <returns></returns>
            public CCollection<CChatRoomUser> GetMembers()
            {
                var ret = new CCollection<CChatRoomUser>();
                using (var cursor = new CChatRoomUserCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStanza.Value = DBUtils.GetID(this, 0);
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }
                return ret;
            }

            /// <summary>
            /// Restituisce il membro specifico
            /// </summary>
            /// <param name="u"></param>
            /// <returns></returns>
            public CChatRoomUser GetMember(Sistema.CUser u)
            {
                if (u is null)
                    throw new ArgumentNullException("user");

                using (var cursor = new CChatRoomUserCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStanza.Value = DBUtils.GetID(this, 0);
                    cursor.UserID.Value = DBUtils.GetID(u, 0);
                    cursor.IgnoreRights = true;
                    cursor.PageSize = 1;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce true se la stanza contiene il membro
            /// </summary>
            /// <param name="u"></param>
            /// <returns></returns>
            public bool HasMember(Sistema.CUser u)
            {
                return GetMember(u) is object;
            }

            /// <summary>
            /// Aggiunge il membro alla stanza
            /// </summary>
            /// <param name="u"></param>
            /// <returns></returns>
            public CChatRoomUser AddMember(Sistema.CUser u)
            {
                if (HasMember(u))
                    throw new ArgumentException("utente già nella stanza");
                var item = new CChatRoomUser();
                item.Stanza = this;
                item.User = u;
                item.Stato = ObjectStatus.OBJECT_VALID;
                item.Save();
                return item;
            }

            /// <summary>
            /// Rimuove il membro dalla stanza
            /// </summary>
            /// <param name="u"></param>
            /// <returns></returns>
            public CChatRoomUser RemoveMember(Sistema.CUser u)
            {
                var item = GetMember(u);
                if (item is null)
                    throw new ArgumentException("utente non nella stanza");
                item.Delete();
                return item;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Name);
            }


            /// <summary>
            /// Repostory
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Messenger.Rooms;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ChatRooms";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Name = reader.Read("Name", this.m_Name);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Name", this.m_Name);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Name", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxName", new string[] { "Name", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", this.m_Name);
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
                    case "Name":
                        {
                            this.m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Compara due stanze per nome
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CChatRoom obj)
            {
                return DMD.Strings.Compare(this.m_Name, obj.m_Name, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CChatRoom)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CChatRoom) && this.Equals((CChatRoom)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CChatRoom obj)
            {
                return base.Equals(obj)
                    && Strings.EQ(this.m_Name, obj.m_Name);
            }
        }
    }
}