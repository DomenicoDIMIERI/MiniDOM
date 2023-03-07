using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Gestori di ricerca nel CRM
        /// </summary>
        [Serializable]
        public class CRegisteredFindHandler 
            : Databases.DBObject, IComparable, IComparable<CRegisteredFindHandler>
        {
            private string m_HandlerClass;
            private string m_EditorClass;
            private string m_Context;
            private int m_Priority;
            


            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredFindHandler()
            {
                this.m_HandlerClass = "";
                this.m_EditorClass = "";
                this.m_Context = "";
                this.m_Priority = 0;
                 
            }

            /// <summary>
            /// Nome della classe istanziata per gestire la ricerca
            /// </summary>
            public string HandlerClass
            {
                get
                {
                    return m_HandlerClass;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_HandlerClass;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_HandlerClass = value;
                    DoChanged("HandlerClass", value, oldValue);
                }
            }
 
            /// <summary>
            /// Nome della classe dell'editor
            /// </summary>
            public string EditorClass
            {
                get
                {
                    return m_EditorClass;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_EditorClass;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_EditorClass = value;
                    DoChanged("EditorClass", value, oldValue);
                }
            }

            /// <summary>
            /// Contesto in cui è valido l'handler
            /// </summary>
            public string Context
            {
                get
                {
                    return m_Context;
                }

                set
                {
                    string oldValue = m_Context;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Context = value;
                    DoChanged("Context", value, oldValue);
                }
            }

            /// <summary>
            /// Priorietà dell'handler
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
            /// Compara gli handler per ordinarli in funzione della priorità
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CRegisteredFindHandler other)
            {
                int ret = 0;
                if (ret == 0) ret = m_Priority.CompareTo(other.m_Priority);
                if (ret == 0) ret = m_HandlerClass.CompareTo(other.m_HandlerClass);
                return ret;
            }

            int IComparable.CompareTo(object other)
            {
                return CompareTo((CRegisteredFindHandler)other);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var sb = new System.Text.StringBuilder();
                sb.Append(m_HandlerClass);
                return sb.ToString();
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CRegisteredFindHandler) && this.Equals((CRegisteredFindHandler)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRegisteredFindHandler obj)
            {
                return DMD.Strings.EQ(this.m_Context , obj.m_Context)
                     && DMD.Strings.EQ(this.m_HandlerClass, obj.m_HandlerClass )
                     && DMD.Integers.EQ(this.m_Priority, obj.m_Priority);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_HandlerClass, this.m_EditorClass, this.m_Context);
            }

            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.RegisteredFindPersonaHandlers;
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CRMRegFindHandler";
            }

        
            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("HandlerClass", m_HandlerClass);
                writer.WriteAttribute("EditorClass", m_EditorClass);
                writer.WriteAttribute("Context", m_Context);
                writer.WriteAttribute("Priority", m_Priority);
                base.XMLSerialize(writer);
                 
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "HandlerClass":
                        {
                            m_HandlerClass = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EditorClass":
                        {
                            m_EditorClass = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Context":
                        {
                            m_Context = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

           

            /// <summary>
            /// Carica dal DB
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_HandlerClass = reader.Read("HandlerClass", m_HandlerClass);
                m_EditorClass = reader.Read("EditorClass", m_HandlerClass);
                m_Context = reader.Read("Context", m_Context);
                m_Priority = reader.Read("Priority", m_Priority);
                
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel DB
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("HandlerClass", m_HandlerClass);
                writer.Write("EditorClass", m_EditorClass);
                writer.Write("Context", m_Context);
                writer.Write("Priority", m_Priority);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema per l'oggetto
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("HandlerClass", typeof(string), 255);
                c = table.Fields.Ensure("EditorClass", typeof(string), 255);
                c = table.Fields.Ensure("Context", typeof(string), 255);
                c = table.Fields.Ensure("Priority", typeof(int), 0);
            }

            /// <summary>
            /// Prepara gli indici
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxClass", new string[] {"HandlerClass", "EditorClass", "Context", "Priority" }, DBFieldConstraintFlags.None);

            }
        }
    }
}