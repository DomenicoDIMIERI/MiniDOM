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
        /// Rappresenta l'assegnazioni di un modulo ad un utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXUser
            : Databases.DBObjectBase
        {
            private int m_ModuleID;
            [NonSerialized] private CModule m_Module;
            private int m_UserID;
            [NonSerialized] private CUser m_User;
            private bool m_Allow;
            private bool m_Negate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXUser()
            {
                m_ModuleID = 0;
                m_Module = null;
                m_UserID = 0;
                m_User = null;
                m_Allow = false;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules.ModulesXUserRepository;
            }

            /// <summary>
            /// Restituisce o imposta l'ID del modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ModuleID
            {
                get
                {
                    return DBUtils.GetID(m_Module, m_ModuleID);
                }

                set
                {
                    int oldValue = ModuleID;
                    if (oldValue == value)
                        return;
                    m_ModuleID = value;
                    m_Module = null;
                    DoChanged("ModuleID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModule Module
            {
                get
                {
                    if (m_Module is null)
                        m_Module = minidom.Sistema.Modules.GetItemById(m_ModuleID);
                    return m_Module;
                }

                set
                {
                    if (ReferenceEquals(value, m_Module))
                        return;
                    m_Module = value;
                    m_ModuleID = DBUtils.GetID(value, 0);
                    DoChanged("Module", value, null);
                }
            }

            internal void SetModule(CModule value)
            {
                m_Module = value;
                m_ModuleID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente
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
            /// User
            /// </summary>
            public CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = minidom.Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    if (ReferenceEquals(m_User, value))
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value, 0);
                    DoChanged("User", value, null);
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="value"></param>
            internal void SetUser(CUser value)
            {
                m_User = value;
                m_UserID = DBUtils.GetID(value, 0);
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
                return "tbl_ModulesXUser";
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
                this.m_ModuleID = reader.Read("Module", this.m_ModuleID);
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
                writer.Write("Module", this.ModuleID);
                writer.Write("Allow", this.m_Allow);
                writer.Write("Negate", this.m_Negate);
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
                c = table.Fields.Ensure("Module", typeof(int), 1);
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

                var c = table.Constraints.Ensure("idxUser", new string[] { "User", "Module" }, DBFieldConstraintFlags.PrimaryKey);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("User", this.UserID);
                writer.WriteAttribute("Module", this.ModuleID);
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

                    case "Module":
                        {
                            m_ModuleID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.ModuleID, ", ", this.UserID, ", ", this.m_Allow, ", ", this.m_Negate);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_ModuleID, this.m_UserID);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CModuleXUser) && this.Equals((CModuleXUser)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CModuleXUser obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.m_ModuleID, obj.m_ModuleID)
                     && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                     && DMD.Booleans.EQ(this.m_Allow, obj.m_Allow)
                     && DMD.Booleans.EQ(this.m_Negate, obj.m_Negate)
                     ;
            }
        }
    }
}