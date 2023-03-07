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
        /// Rappresenta l'assegnazioni di un modulo ad un gruppo di utenti
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXGroup 
            : Databases.DBObjectBase
        {
            private int m_ModuleID;
            [NonSerialized] private CModule m_Module;
            private int m_GroupID;
            [NonSerialized] private CGroup m_Group;
            private bool m_Allow;
            private bool m_Negate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXGroup()
            {
                this.m_ModuleID = 0;
                this.m_Module = null;
                this.m_GroupID = 0;
                this.m_Group = null;
                this.m_Allow = false;
                this.m_Negate = false;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules.ModulesXGroupRepository;
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azione associata
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
                        m_Module = Modules.GetItemById(m_ModuleID);
                    return m_Module;
                }

                set
                {
                    if (ReferenceEquals(value, m_Module))
                        return;
                    m_Module = value;
                    m_ModuleID = DBUtils.GetID(value, 0);
                    DoChanged("Module", value);
                }
            }

            /// <summary>
            /// Imposta il modulo
            /// </summary>
            /// <param name="value"></param>
            internal void SetModule(CModule value)
            {
                this.m_Module = value;
                this.m_ModuleID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID del gruppo associato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
                    if (ReferenceEquals(m_Group, value))
                        return;
                    m_Group = value;
                    m_GroupID = DBUtils.GetID(value, 0);
                    DoChanged("Group", value);
                }
            }

            /// <summary>
            /// Imposta il gruppo
            /// </summary>
            /// <param name="value"></param>
            internal void SetGroup(CGroup value)
            {
                this.m_Group = value;
                this.m_GroupID = DBUtils.GetID(value, 0);
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
                return "tbl_ModulesXGroup";
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
                this.m_GroupID = reader.Read("Group", this.m_GroupID);
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
                writer.Write("Group", GroupID);
                writer.Write("Module", ModuleID);
                writer.Write("Allow", m_Allow);
                writer.Write("Negate", m_Negate);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Group", typeof(int), 1);
                c = table.Fields.Ensure("Module", typeof(int), 1);
                c = table.Fields.Ensure("Allow", typeof(bool), 1);
                c = table.Fields.Ensure("Negate", typeof(bool), 1);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxGroup", new string[] { "Group", "Module" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("Allow", typeof(bool), 1);
                //c = table.Fields.Ensure("Negate", typeof(bool), 1);
            }

           
            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Group", GroupID);
                writer.WriteAttribute("Module", ModuleID);
                writer.WriteAttribute("Allow", m_Allow);
                writer.WriteAttribute("Negate", m_Negate);
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
                    case "Group":
                        {
                            m_GroupID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return DMD.Strings.ConcatArray(this.ModuleID, ", ", this.GroupID, ", ", this.m_Allow, ", ", this.m_Negate);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_ModuleID, this.m_GroupID);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CModuleXGroup) && this.Equals((CModuleXGroup)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CModuleXGroup obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.m_ModuleID, obj.m_ModuleID)
                     && DMD.Integers.EQ(this.m_GroupID, obj.m_GroupID)
                     && DMD.Booleans.EQ(this.m_Allow, obj.m_Allow)
                     && DMD.Booleans.EQ(this.m_Negate, obj.m_Negate)
                     ;
            }
        }
    }
}