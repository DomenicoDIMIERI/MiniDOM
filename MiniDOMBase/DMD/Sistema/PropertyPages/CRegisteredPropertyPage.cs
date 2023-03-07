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
        /// Pagina di proprietà registrata per un oggetto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CRegisteredPropertyPage 
            : Databases.DBObjectBase, IComparable, IComparable<CRegisteredPropertyPage>
        {
            private string m_ClassName;
            private string m_TabPageClass;
            private Type m_TabPageType;
            private string m_Context;
            private int m_Priority;

            [NonSerialized] private PropPageUserAllowNegateCollection m_UsersAuth;
            [NonSerialized] private PropPageGroupAllowNegateCollection m_GroupAuth;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredPropertyPage()
            {
                m_ClassName = "";
                m_TabPageClass = "";
                m_Context = "";
                m_Priority = 0;
                m_UsersAuth = null;
                m_GroupAuth = null;
            }


            /// <summary>
            /// Rimuove l'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public PropPageUserAllowNegate RemoveUserAllowNegate(CUser user)
            {

                lock (this)
                {
                    PropPageUserAllowNegate item = null;
                    int i = 0;
                    while (i < this.UserAuth.Count && item is null)
                    {
                        if (this.UserAuth[i].UserID == DBUtils.GetID(user, 0))
                        {
                            item = UserAuth[i];
                        }
                        else
                        {
                            i += 1;
                        }
                    }

                    if (item is null)
                        return null;
                    item.Delete();
                    UserAuth.Remove(item);
                    item.Save();
                    return item;
                }
            }
            /// <summary>
            /// Assegna le autorizzazione utente per la pagina
            /// </summary>
            /// <param name="user"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public PropPageUserAllowNegate SetUserAllowNegate(CUser user, bool allow, bool negate)
            {
                lock (this)
                {
                    PropPageUserAllowNegate item = null;
                    int i = 0;
                    while (i < UserAuth.Count && item is null)
                    {
                        if (UserAuth[i].UserID == DBUtils.GetID(user, 0))
                        {
                            item = UserAuth[i];
                        }
                        else
                        {
                            i += 1;
                        }
                    }

                    if (item is null)
                    {
                        item = new PropPageUserAllowNegate();
                        UserAuth.Add(item);
                    }

                    item.Item = this;
                    item.User = user;
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                    PropertyPages.UpdateCached(this);
                    return item;
                }
            }

            /// <summary>
            /// Imposta l'autorizzazione per il gruppo
            /// </summary>
            /// <param name="group"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public PropPageGroupAllowNegate SetGroupAllowNegate(CGroup group, bool allow, bool negate)
            {
                // SyncLock Me
                PropPageGroupAllowNegate item = null;
                int i = 0;
                while (i < GroupAuth.Count && item is null)
                {
                    if (GroupAuth[i].GroupID == DBUtils.GetID(group, 0))
                    {
                        item = GroupAuth[i];
                    }
                    else
                    {
                        i += 1;
                    }
                }

                if (item is null)
                {
                    if (allow)
                    {
                        item = new PropPageGroupAllowNegate();
                        item.Item = this;
                        item.Group = group;
                        item.Allow = allow;
                        item.Negate = negate;
                        item.Save();
                        GroupAuth.Add(item);
                    }
                }
                else if (!allow)
                {
                    GroupAuth.Remove(item);
                    item.Delete();
                    item = null;
                }

                PropertyPages.UpdateCached(this);
                return item;
                // End SyncLock
            }

            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    m_GroupAuth = null;
            //    m_UsersAuth = null;
            //}

            /// <summary>
            /// Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsVisibleToUser(CUser user)
            {
                lock (this)
                {
                    bool a = false;
                    bool n = false;
                    int i = 0;
                    PropPageUserAllowNegate ua;
                    PropPageGroupAllowNegate ga;
                    while (i < UserAuth.Count)
                    {
                        ua = UserAuth[i];
                        if (ua.UserID == DBUtils.GetID(user, 0))
                        {
                            a = a | ua.Allow;
                            n = n | ua.Negate;
                        }

                        i += 1;
                    }

                    i = 0;
                    while (i < GroupAuth.Count)
                    {
                        ga = GroupAuth[i];
                        foreach (CGroup gp in user.Groups)
                        {
                            if (ga.GroupID == DBUtils.GetID(gp, 0))
                            {
                                a = a | ga.Allow;
                                n = n | ga.Negate;
                                break;
                            }
                        }

                        i += 1;
                    }

                    return a && !n;
                }
            }

            /// <summary>
            /// Restituisce vero se l'utente è autorizzato a vedere questo link
            /// </summary>
            /// <param name="userID"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsVisibleToUser(int userID)
            {
                return IsVisibleToUser(minidom.Sistema.Users.GetItemById(userID));
            }

            /// <summary>
            /// Restituisce la collezione degli oggetti autorizzazione/negazione definiti per gli utenti specifici
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PropPageUserAllowNegateCollection UserAuth
            {
                get
                {
                    lock (this)
                    {
                        if (m_UsersAuth is null)
                            m_UsersAuth = new PropPageUserAllowNegateCollection(this);
                        return m_UsersAuth;
                    }
                }
            }

            /// <summary>
            /// Restituisce la collezione degli oggetti autorizzazione/negazione definiti per i gruppi specifici
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PropPageGroupAllowNegateCollection GroupAuth
            {
                get
                {
                    lock (this)
                    {
                        if (m_GroupAuth is null)
                            m_GroupAuth = new PropPageGroupAllowNegateCollection(this);
                        return m_GroupAuth;
                    }
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.PropertyPages; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta il nome della classe per cui é registrata la pagina di proprietà
            /// </summary>
            public string ClassName
            {
                get
                {
                    return m_ClassName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ClassName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ClassName = value;
                    DoChanged("ClassName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della classe pagina di proprietà
            /// </summary>
            public string TabPageClass
            {
                get
                {
                    return m_TabPageClass;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TabPageClass;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TabPageClass = value;
                    DoChanged("TabPageClass", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della pagina di proprietà
            /// </summary>
            public Type TabPageType
            {
                get
                {
                    if (m_TabPageType is null)
                        m_TabPageType = DMD.RunTime.FindType(m_TabPageClass);
                    return m_TabPageType;
                }

                set
                {
                    var oldValue = m_TabPageType;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TabPageType = value;
                    m_TabPageClass = value.Name;
                    DoChanged("TabPageType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il contesto
            /// </summary>
            public string Context
            {
                get
                {
                    return m_Context;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Context;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Context = value;
                    DoChanged("Context", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la priorità di visualizzazione della pagina
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_Priority;
                }

                set
                {
                    int oldValue = m_Priority;
                    if (oldValue == value)
                        return;
                    m_Priority = value;
                    DoChanged("Priority", value, oldValue);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_RegisteredTabPages";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_ClassName = reader.Read("ClassName", this.m_ClassName);
                this.m_TabPageClass = reader.Read("TabPageClass", this.m_TabPageClass);
                this.m_Context = reader.Read("Context", this.m_Context);
                this.m_Priority = reader.Read("Priority", this.m_Priority);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ClassName", m_ClassName);
                writer.Write("TabPageClass", m_TabPageClass);
                writer.Write("Context", m_Context);
                writer.Write("Priority", m_Priority);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("ClassName", typeof(string), 255);
                c = table.Fields.Ensure("TabPageClass", typeof(string), 255);
                c = table.Fields.Ensure("Context", typeof(string), 255);
                c = table.Fields.Ensure("Priority", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxClassName", new string[] { "ClassName", "TabPageClass", "Context" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxPriority", new string[] { "Priority" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_ClassName", m_ClassName);
                writer.WriteAttribute("m_TabPageClass", m_TabPageClass);
                writer.WriteAttribute("m_Context", m_Context);
                writer.WriteAttribute("m_Priority", m_Priority);
                base.XMLSerialize(writer);
                writer.WriteTag("UserAuth", this.UserAuth);
                writer.WriteTag("GroupAuth", this.GroupAuth);
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
                    case "m_ClassName":
                        {
                            m_ClassName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_TabPageClass":
                        {
                            m_TabPageClass = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Context":
                        {
                            m_Context = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Priority":
                        {
                            m_Priority = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserAuth":
                        {
                            m_UsersAuth = (PropPageUserAllowNegateCollection)fieldValue;
                            m_UsersAuth.SetPropPage(this);
                            break;
                        }

                    case "GroupAuth":
                        {
                            m_GroupAuth = (PropPageGroupAllowNegateCollection)fieldValue;
                            m_GroupAuth.SetPropPage(this);
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
                return DMD.Strings.ConcatArray(
                                this.m_ClassName , "/" , this.m_TabPageClass , "(" , this.m_Context , ", " , this.m_Priority , ")");
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public int CompareTo(CRegisteredPropertyPage value)
            {
                int ret;
                ret = DMD.Strings.Compare(Context, value.Context, true);
                if (ret != 0)
                    return ret;
                ret = DMD.Strings.Compare(ClassName, value.ClassName, true);
                if (ret != 0)
                    return ret;
                ret = Priority - value.Priority;
                if (ret != 0)
                    return ret;
                ret = DMD.Strings.Compare(TabPageClass, value.TabPageClass, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CRegisteredPropertyPage)obj);
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            //protected override void OnAfterSave(SystemEvent e)
            //{
            //    base.OnAfterSave(e);
            //    PropertyPages.UpdateCached(this);
            //}

            /// <summary>
            /// Invalida gli utenti
            /// </summary>
            protected internal void InvalidateUserAuth()
            {
                m_UsersAuth = null;
            }

            /// <summary>
            /// Invalida i gruppi
            /// </summary>
            protected internal void InvalidateGroupAuth()
            {
                m_GroupAuth = null;
            }
        }
    }
}