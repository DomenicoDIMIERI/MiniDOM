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
        /// Descrive un'azione definita su una risorsa.
        /// All'azione possono essere associati uno o più handler ModuleActionHandler raccolti nella proprietà Handlers
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleAction 
            : Databases.DBObjectBase
        {
            private string m_ModuleName; 
            [NonSerialized] private CModule m_Module;
            private string m_AuthorizationName;
            private string m_AuthorizationDescription;
            private bool m_Visible;
            private string m_ClassHandler;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleAction()
            {
                m_ModuleName = "";
                m_Module = null;
                m_AuthorizationName = "";
                m_AuthorizationDescription = "";
                m_Visible = false;
                m_ClassHandler = "";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules.DefinedActions;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_AuthorizationName , " ( " , this.m_ModuleName , ")");
            }

            /// <summary>
            /// Restituisce o imposta il nome del modulo su cui è definita l'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ModuleName
            {
                get
                {
                    return m_ModuleName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ModuleName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ModuleName = value;
                    m_Module = null;
                    DoChanged("ModuleName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modulo su cui è definita l'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModule Module
            {
                get
                {
                    if (m_Module is null)
                        m_Module = Modules.GetItemByName(m_ModuleName);
                    return m_Module;
                }

                set
                {
                    var oldValue = Module;
                    if (oldValue == value)
                        return;
                    m_Module = value;
                    if (value is object)
                    {
                        m_ModuleName = value.ModuleName;
                    }
                    else
                    {
                        m_ModuleName = DMD.Strings.vbNullString;
                    }

                    DoChanged("Module", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il modulo
            /// </summary>
            /// <param name="value"></param>
            internal void SetModule(CModule value)
            {
                m_Module = value;
                this.m_ModuleName = (value is object)? value.ModuleName : DMD.Strings.vbNullString;
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AuthorizationName
            {
                get
                {
                    return m_AuthorizationName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AuthorizationName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AuthorizationName = value;
                    DoChanged("AuthorizationName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringe che descrive l'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AuthorizationDescription
            {
                get
                {
                    return m_AuthorizationDescription;
                }

                set
                {
                    string oldValue = m_AuthorizationDescription;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AuthorizationDescription = value;
                    DoChanged("AuthorizationDescription", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica specifica se si tratta di un'azione di interfaccia o nascota
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Visible
            {
                get
                {
                    return m_Visible;
                }

                set
                {
                    if (m_Visible == value)
                        return;
                    m_Visible = value;
                    DoChanged("Visible", value, !value);
                }
            }

            /// <summary>
            /// Nome della classe che esegue l'azione (non implementato)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ClassHandler
            {
                get
                {
                    return m_ClassHandler;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ClassHandler;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ClassHandler = value;
                    DoChanged("ClassHandler", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool UserCanDoAction(CUser user)
            {
                if (DBUtils.GetID(user) == DBUtils.GetID(Users.KnownUsers.GlobalAdmin))
                    return true;
                bool a = false;
                bool n = false;
                user.Authorizations.GetAllowNegate(this, ref a, ref n);
                foreach (CGroup g in user.Groups)
                    g.Authorizations.GetAllowNegate(this, ref a, ref n);
                return a && !n;
            }

            /// <summary>
            /// Restituisce true se l'utente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="userID"></param>
            /// <returns></returns>
            public bool UserCanDoAction(int userID)
            {
                return UserCanDoAction(Users.GetItemById(userID));
            }

            /// <summary>
            /// Imposta i permessi per l'utente sull'azione
            /// </summary>
            /// <param name="user"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public CUserAuthorization SetUserAllowNegate(CUser user, bool allow, bool negate)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                return user.Authorizations.SetAllowNegate(this, allow, negate);
            }

            /// <summary>
            /// Imposta i permessi per l'azione sul gruppo
            /// </summary>
            /// <param name="group"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public CGroupAuthorization SetGroupAllowNegate(CGroup group, bool allow, bool negate)
            {
                if (group is null)
                    throw new ArgumentNullException("group");
                return group.Authorizations.SetAllowNegate(this, allow, negate);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_DefinedAuthorizations";
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
                m_ModuleName = reader.Read("Modulo", this. m_ModuleName);
                m_AuthorizationName = reader.Read("AuthorizationName", this.m_AuthorizationName);
                m_AuthorizationDescription = reader.Read("AuthorizationDescription", this.m_AuthorizationDescription);
                m_Visible = reader.Read("Visible", this.m_Visible);
                m_ClassHandler = reader.Read("ClassHandler", this.m_ClassHandler);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Modulo", m_ModuleName);
                writer.Write("AuthorizationName", m_AuthorizationName);
                writer.Write("AuthorizationDescription", m_AuthorizationDescription);
                writer.Write("Visible", m_Visible);
                writer.Write("ClassHandler", m_ClassHandler);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara il db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Modulo", typeof(string), 255);
                c = table.Fields.Ensure("AuthorizationName", typeof(string), 255);
                c = table.Fields.Ensure("AuthorizationDescription", typeof(string), 0);
                c = table.Fields.Ensure("Visible", typeof(bool), 1);
                c = table.Fields.Ensure("ClassHandler", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxModulo", new string[] { "Modulo", "AuthorizationName" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDescr", new string[] { "AuthorizationDescription", "Visible" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxHandler", new string[] { "ClassHandler" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ModuleName", m_ModuleName);
                writer.WriteAttribute("AuthorizationName", m_AuthorizationName);
                writer.WriteAttribute("AuthorizationDescription", m_AuthorizationDescription);
                writer.WriteAttribute("Visible", m_Visible);
                writer.WriteAttribute("ClassHandler", m_ClassHandler);
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
                    case "ModuleName":
                        {
                            m_ModuleName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AuthorizationName":
                        {
                            m_AuthorizationName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AuthorizationDescription":
                        {
                            m_AuthorizationDescription = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Visible":
                        {
                            m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ClassHandler":
                        {
                            m_ClassHandler = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce il codice hash dell'ogetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_ModuleName, this.m_AuthorizationName);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CModuleAction) && this.Equals((CModuleAction)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CModuleAction obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_ModuleName, obj.m_ModuleName)
                    && DMD.Strings.EQ(this.m_AuthorizationName, obj.m_AuthorizationName)
                    && DMD.Strings.EQ(this.m_AuthorizationDescription, obj.m_AuthorizationDescription)
                    && DMD.Booleans.EQ(this.m_Visible, obj.m_Visible)
                    && DMD.Strings.EQ(this.m_ClassHandler, obj.m_ClassHandler)
                    ;
            }

             
        }
    }
}