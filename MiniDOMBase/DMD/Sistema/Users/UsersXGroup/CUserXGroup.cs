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
        /// Relazione tra un utente ed un gruppo di utenti
        /// </summary>
        [Serializable]
        public class CUserXGroup 
            : Databases.DBObjectBase
        {
            private int m_UserID;
            [NonSerialized] private CUser m_User;
            private int m_GroupID;
            [NonSerialized] private CGroup m_Group;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserXGroup()
            {
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", this.UserID, ", ", this.GroupID, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_UserID, this.m_GroupID);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CUserXGroup) && this.Equals((CUserXGroup)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CUserXGroup obj)
            {
                return base.Equals(obj)
                        && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                        && DMD.Integers.EQ(this.m_GroupID, obj.m_GroupID)
                        ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Users.UsersXGroupsRepository;
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'utente
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
            /// Restituisce o imposta l'utente
            /// </summary>
            public CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = m_User;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    SetUser(value);
                    DoChanged("User", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="value"></param>
            internal virtual void SetUser(CUser value)
            {
                m_User = value;
                m_UserID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'id del gruppo
            /// </summary>
            public int GroupID
            {
                get
                {
                    return DBUtils.GetID(m_Group, m_GroupID);
                }

                set
                {
                    int oldValue = GroupID;
                    if (oldValue == value)
                        return;
                    m_GroupID = value;
                    m_Group = null;
                    DoChanged("GroupID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il gruppo
            /// </summary>
            public CGroup Group
            {
                get
                {
                    if (m_Group is null)
                        m_Group = Groups.GetItemById(m_GroupID);
                    return m_Group;
                }

                set
                {
                    var oldValue = m_Group;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Group = value;
                    m_GroupID = DBUtils.GetID(value, 0);
                    DoChanged("Group", value, oldValue);
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Driscriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_UsersXGroup";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_UserID = reader.Read("User", this. m_UserID);
                this.m_GroupID = reader.Read("Group", this.m_GroupID);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("User", UserID);
                writer.Write("Group", GroupID);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("User", typeof(int), 1);
                c = table.Fields.Ensure("Group", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUserXGroup", new string[] { "User", "Group" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("User", UserID);
                writer.WriteAttribute("Group", GroupID);
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
                    case "User":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Group":
                        {
                            m_GroupID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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