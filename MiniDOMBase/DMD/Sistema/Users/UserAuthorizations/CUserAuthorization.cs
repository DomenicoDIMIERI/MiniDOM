using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Rappresenta una autorizzazione o una negazione esplicita di un'azione ad uno specifico utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CUserAuthorization 
            : Databases.DBObjectBase
        {
            private int m_ActionID;
            [NonSerialized] private CModuleAction m_Action;
            private int m_UserID;
            [NonSerialized] private CUser m_User;
            private bool m_Allow;
            private bool m_Negate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserAuthorization()
            {
                this.m_ActionID = 0;
                this.m_Action = null;
                this.m_UserID = 0;
                this.m_User = null;
                this.m_Allow = false;
                this.m_Negate = false;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", this.UserID, ", ", this.ActionID, ", ", this.Allow, ", ", this.m_Negate, " }");
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CUserAuthorization) && this.Equals((CUserAuthorization)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CUserAuthorization obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_ActionID, obj.m_ActionID)
                    && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                    && DMD.Booleans.EQ(this.m_Allow, obj.m_Allow)
                    && DMD.Booleans.EQ(this.m_Negate, obj.m_Negate)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Users.UserAuthorizations;
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azione associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ActionID
            {
                get
                {
                    return DBUtils.GetID(m_Action, m_ActionID);
                }

                set
                {
                    int oldValue = ActionID;
                    if (oldValue == value)
                        return;
                    m_ActionID = value;
                    m_Action = null;
                    DoChanged("ActionID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azione associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModuleAction Action
            {
                get
                {
                    if (m_Action is null)
                        m_Action = minidom.Sistema.Modules.DefinedActions.GetItemById(m_ActionID);
                    return m_Action;
                }

                set
                {
                    var oldValue = Action;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_Action = value;
                    m_ActionID = DBUtils.GetID(value, 0);
                    DoChanged("Action", value);
                }
            }

            /// <summary>
            /// Imposta l'azione
            /// </summary>
            /// <param name="value"></param>
            internal void SetAction(CModuleAction value)
            {
                m_Action = value;
                m_ActionID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente associato
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
                    var oldValue = User;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value, 0);
                    DoChanged("User", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="user"></param>
            internal void SetUser(CUser user)
            {
                m_User = user;
                m_UserID = DBUtils.GetID(user, 0);
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che consente esplicitamente l'azione all'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Allow
            {
                get
                {
                    return m_Allow;
                }

                set
                {
                    if (m_Allow == value)
                        return;
                    m_Allow = value;
                    DoChanged("Allow", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che nega esplicitamente l'azione all'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Negate
            {
                get
                {
                    return m_Negate;
                }

                set
                {
                    if (m_Negate == value)
                        return;
                    m_Negate = value;
                    DoChanged("Negate", value, !value);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_UserAuthorizations";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_UserID = reader.Read("User", this.m_UserID);
                this.m_ActionID = reader.Read("Action", this.m_ActionID);
                this.m_Allow = reader.Read("Allow", this.m_Allow);
                this.m_Negate = reader.Read("Negate", this.m_Negate);
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
                writer.Write("Action", this.ActionID);
                writer.Write("Allow", this.m_Allow);
                writer.Write("Negate", this.m_Negate);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Preapra lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("User", typeof(int), 1);
                c = table.Fields.Ensure("Action", typeof(int), 1);
                c = table.Fields.Ensure("Allow", typeof(bool), 1);
                c = table.Fields.Ensure("Negate", typeof(bool), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUserAction", new string[] { "User", "Action" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("Allow", typeof(bool), 1);
                //c = table.Fields.Ensure("Negate", typeof(bool), 1);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("User", this.UserID);
                writer.WriteAttribute("Action", this.ActionID);
                writer.WriteAttribute("Allow", this.m_Allow);
                writer.WriteAttribute("Negate", this.m_Negate);
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

                    case "Action":
                        {
                            m_ActionID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Allow":
                        {
                            m_Allow = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }
                    case "Negate":
                        {
                            m_Negate = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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