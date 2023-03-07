using System;
using DMD;
using System.Collections;
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
        /// Gruppo di utenti
        /// </summary>
        [Serializable]
        public sealed class CGroup 
            : Databases.DBObject, IComparable, ISupportsSingleNotes, IComparable<CGroup>
        {
            private string m_GroupName; // Nome del gruppo utente
            private string m_Description;
            private bool m_IsBuiltIn;
            [NonSerialized] private CGroupMembersCollection m_Members;
            [NonSerialized] private CGroupAuthorizationCollection m_Authorizations;
            [NonSerialized] private CModuleXGroupCollection m_Modules;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroup()
            {
                this.m_GroupName = "";
                this.m_Description = "";
                this.m_IsBuiltIn = false;
                this.m_Members = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="groupName"></param>
            public CGroup(string groupName)
                : this()
            {
                groupName = DMD.Strings.Trim(groupName);
                if (string.IsNullOrEmpty(groupName))
                    throw new ArgumentNullException("groupName");
                this.m_GroupName = groupName;                
            }

            /// <summary>
            /// Restituisce la collezione delle azioni consentite o negate esplicitamente al gruppo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CGroupAuthorizationCollection Authorizations
            {
                get
                {
                    lock (this)
                    {
                        if (m_Authorizations is null)
                            m_Authorizations = new CGroupAuthorizationCollection(this);
                        return m_Authorizations;
                    }
                }
            }

            /// <summary>
            /// Restituisce la collezione dei moduli esplicitamente autorizzati o negati per il gruppo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModuleXGroupCollection Modules
            {
                get
                {
                    if (m_Modules is null)
                        m_Modules = new CModuleXGroupCollection(this);
                    return m_Modules;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Groups; //.Module;
            }

            /// <summary>
            /// Restituisce il nome del gruppo
            /// </summary>
            public string GroupName
            {
                get
                {
                    return m_GroupName;
                }
            }

            /// <summary>
            /// Imposta il nome del gruppo
            /// </summary>
            /// <param name="value"></param>
            internal void SetGroupName(string value)
            {
                m_GroupName = DMD.Strings.Trim(value);
            }

            /// <summary>
            /// Restituisce o imposta la descrizione del gruppo
            /// </summary>
            public string Notes
            {
                get
                {
                    return m_Description;
                }

                set
                {
                    string oldValue = m_Description;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Description = value;
                    DoChanged("Description", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se il gruppo è un gruppo predefinito
            /// </summary>
            public bool IsBuiltIn
            {
                get
                {
                    return m_IsBuiltIn;
                }

                internal set
                {
                    if (m_IsBuiltIn == value)
                        return;
                    m_IsBuiltIn = value;
                    DoChanged("IsBuiltIn", value, !value);
                }
            }

            /// <summary>
            /// Restituisce la collezione degli utenti che appartengono al gruppo
            /// </summary>
            public CGroupMembersCollection Members
            {
                get
                {
                    lock (this)
                    {
                        if (m_Members is null)
                            m_Members = new CGroupMembersCollection(this);
                        return m_Members;
                    }
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Gruppi";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_GroupName = reader.Read("GroupName", this.m_GroupName);
                this.m_Description = reader.Read("Description", this.m_Description);
                this.m_IsBuiltIn = reader.Read("BuiltIn", this.m_IsBuiltIn);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("GroupName", m_GroupName);
                writer.Write("Description", m_Description);
                writer.Write("BuiltIn", m_IsBuiltIn);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("GroupName", typeof(string), 255);
                c = table.Fields.Ensure("Description", typeof(string), 0);
                c = table.Fields.Ensure("BuiltIn", typeof(bool), 0);
                c = table.Fields.Ensure("Flags", typeof(int), 0);
                c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxName", new string[] { "GroupName", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDescription", new string[] { "Description", "BuiltIn", "Flags" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("GroupName", m_GroupName);
                writer.WriteAttribute("IsBuiltIn", m_IsBuiltIn);
                base.XMLSerialize(writer);
                writer.WriteTag("Parameters", this.Parameters);
                writer.WriteTag("Description", m_Description);
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
                    case "GroupName":
                        {
                            m_GroupName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Description":
                        {
                            m_Description = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IsBuiltIn":
                        {
                            m_IsBuiltIn = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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
                return this.m_GroupName;
            }

            /// <summary>
            /// Restitusice il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_GroupName);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CGroup) && this.Equals((CGroup)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CGroup obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.Equals(this.m_GroupName, obj.m_GroupName)
                    && DMD.Strings.Equals(this.m_Description, obj.m_Description)
                    && DMD.Booleans.Equals(this.m_IsBuiltIn, obj.m_IsBuiltIn)
                    ;
            //[NonSerialized] private CGroupMembersCollection m_Members;
            //[NonSerialized] private CGroupAuthorizationCollection m_Authorizations;
            //[NonSerialized] private CModuleXGroupCollection m_Modules;
            }

            /// <summary>
            /// Compara due oggetti per nome
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CGroup obj)
            {
                return DMD.Strings.Compare(GroupName, obj.GroupName, true);
            }


            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CGroup)obj);
            }

            

           

            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    m_Authorizations = null;
            //    m_Members = null;
            //    m_Modules = null;
            //}
        }
    }
}