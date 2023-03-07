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
        /// Rappresenta una autorizzazione o una negazione esplicita di un'azione ad uno specifico gruppo di utenti
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CGroupAuthorization 
            : Databases.DBObjectBase
        {
            private int m_ActionID;
            [NonSerialized] private CModuleAction m_Action;
            private int m_GroupID;
            [NonSerialized] private CGroup m_Group;
            private bool m_Allow;
            private bool m_Negate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupAuthorization()
            {
                this.m_ActionID = 0;
                this.m_Action = null;
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
                return minidom.Sistema.Groups.Authorizations;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Group[", this.m_GroupID, "], Action[", this.m_ActionID, "] allow: ", this.m_Allow, ", negate: ", this.m_Negate);
            }

            /// <summary>
            /// Restitusice il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_GroupID, this.m_ActionID);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CGroupAuthorization) && this.Equals((CGroupAuthorization)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CGroupAuthorization obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_ActionID, obj.m_ActionID)
                    && DMD.Integers.EQ(this.m_GroupID, obj.m_GroupID)
                    && DMD.Booleans.EQ(this.m_Allow, obj.m_Allow)
                    && DMD.Booleans.EQ(this.m_Negate, obj.m_Negate)
                    ;
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
                    DoChanged("Action", value, oldValue);
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
                        m_Group = minidom.Sistema.Groups.GetItemById(m_GroupID);
                    return m_Group;
                }

                set
                {
                    var oldValue = Group;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Group = value;
                    m_GroupID = DBUtils.GetID(value, 0);
                    DoChanged("Group", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il gruppo
            /// </summary>
            /// <param name="value"></param>
            internal void SetGroup(CGroup value)
            {
                m_Group = value;
                m_GroupID = DBUtils.GetID(value, 0);
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
                return "tbl_GroupAuthorizations";
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
                this.m_GroupID = reader.Read("Gruppo", this. m_GroupID);
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
                writer.Write("Gruppo", GroupID);
                writer.Write("Action", ActionID);
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

                var c = table.Fields.Ensure("Gruppo", typeof(int), 1);
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

                var c = table.Constraints.Ensure("idxGruppo", new string[] { "Gruppo", "Action" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("Allow", typeof(bool), 1);
                //c = table.Fields.Ensure("Negate", typeof(bool), 1);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Gruppo", GroupID);
                writer.WriteAttribute("Action", ActionID);
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
                    case "Gruppo":
                        {
                            m_GroupID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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