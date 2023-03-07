using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Registrazione di un handler di un evento
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RegisteredEventHandler 
            : minidom.Databases.DBObjectBase, IComparable, IComparable<RegisteredEventHandler>
        {
            private bool m_Active;
            private int m_ModuleID;
            private string m_ModuleName;
            [NonSerialized] private CModule m_Module;
            private string m_EventName;
            [NonSerialized] private IEventHandler m_Handler;
            private int m_Priority;
            private string m_ClassName;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegisteredEventHandler()
            {
                m_Active = true;
                m_ModuleID = 0;
                m_Module = null;
                m_EventName = "";
                m_Handler = null;
                m_ModuleName = "";
                m_ClassName = "";
                m_Priority = 0;
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se l'evento è attivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Active
            {
                get
                {
                    return m_Active;
                }

                set
                {
                    if (value == m_Active)
                        return;
                    m_Active = value;
                    DoChanged("Active", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del modulo di cui l'handler gestisce l'evento
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
            /// Restituisce o imposta il modulo di cui l'handler gestisce l'evento
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
                    var oldValue = Module;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Module = value;
                    m_ModuleID = DBUtils.GetID(value, 0);
                    m_ModuleName = "";
                    if (value is object)
                        m_ModuleName = value.ModuleName;
                    DoChanged("Module", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del modulo  
            /// </summary>
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
                    DoChanged("ModuleName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'evento gestito da questo handler
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string EventName
            {
                get
                {
                    return m_EventName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_EventName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EventName = value;
                    DoChanged("EventName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'handler
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public object CreateHandler()
            {
                if (m_Handler is null)
                {
#if (!DEBUG)
                try {
#endif
                    m_Handler = (IEventHandler)DMD.RunTime.CreateInstance(m_ClassName);
#if (!DEBUG)
                } catch (Exception) {
                    Events.NotifyUnhandledException(new Exception("Non riesco a creare l'handler [" + m_ClassName + "] per l'azione (" + ModuleName + ", " + m_EventName + ")"));
                }
#endif
                }

                return m_Handler;
            }

            /// <summary>
            /// Restituisce o imposta il nome della classe gestore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
                    m_Handler = null;
                    DoChanged("ClassName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un numero che indica la priorità di esecuzione del gestore. Numero maggiori indicano priorità minore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.RegisteredEventHandlers;
            }

            /// <summary>
            /// Table name
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_EventsHandlers";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append("{ ");
                if (Module is object)
                {
                    ret.Append(Module.ModuleName);
                }
                else
                {
                    ret.Append("Modulo non valido: " + ModuleID);
                }

                ret.Append(", ");
                ret.Append(m_EventName);
                ret.Append(" }");
                return ret.ToString();
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is RegisteredEventHandler) && this.Equals((RegisteredEventHandler)obj);
                  
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RegisteredEventHandler obj)
            {
                return base.Equals(obj)
                    && DMD.Booleans.EQ(this.m_Active, obj.m_Active)
                    && DMD.Integers.EQ(this.m_ModuleID, obj.m_ModuleID)
                    && DMD.Strings.EQ(this.m_ModuleName, obj.m_ModuleName)
                    && DMD.Strings.EQ(this.m_EventName, obj.m_EventName)
                    && DMD.Integers.EQ(this.m_Priority, obj.m_Priority)
                    && DMD.Strings.EQ(this.m_ClassName, obj.m_ClassName)
                    ;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_ModuleID, this.m_EventName, this.m_ClassName);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(RegisteredEventHandler obj)
            {
                int ret = DMD.Integers.Compare(this.Priority, obj.Priority);
                if (ret == 0) ret = DMD.Strings.Compare(this.ModuleName, obj.ModuleName, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.ClassName, obj.ClassName, false);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((RegisteredEventHandler)obj); }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Active = reader.Read("Active", m_Active);
                m_ModuleID = reader.Read("Module", m_ModuleID);
                m_ModuleName = reader.Read("ModuleName", m_ModuleName);
                m_EventName = reader.Read("EventName", m_EventName);
                m_ClassName = reader.Read("ClassName", m_ClassName);
                m_Priority = reader.Read("Priority", m_Priority);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Active", this.m_Active);
                writer.Write("Module", this.ModuleID);
                writer.Write("ModuleName", this.m_ModuleName);
                writer.Write("EventName", this.m_EventName);
                writer.Write("ClassName", this.m_ClassName);
                writer.Write("Priority", this.m_Priority);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Active", typeof(bool), 1);
                c = table.Fields.Ensure("Module", typeof(int), 1);
                c = table.Fields.Ensure("ModuleName", typeof(string), 255);
                c = table.Fields.Ensure("EventName", typeof(string), 255);
                c = table.Fields.Ensure("ClassName", typeof(string), 255);
                c = table.Fields.Ensure("Priority", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxModule", new string[] { "Module", "ModuleName" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEvent", new string[] { "EventName", "Priority", "ClassName" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Active", this.m_Active);
                writer.WriteAttribute("Module", this.ModuleID);
                writer.WriteAttribute("ModuleName", this.m_ModuleName);
                writer.WriteAttribute("EventName", this.m_EventName);
                writer.WriteAttribute("ClassName", this.m_ClassName);
                writer.WriteAttribute("Priority", this.m_Priority);
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
                    case "Active":
                        {
                            m_Active = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Module":
                        {
                            m_ModuleID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ModuleName":
                        {
                            m_ModuleName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EventName":
                        {
                            m_EventName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ClassName":
                        {
                            m_ClassName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Priority":
                        {
                            m_Priority = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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