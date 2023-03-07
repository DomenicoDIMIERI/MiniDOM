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
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Condizione di attenzione associata ad una persona
        /// </summary>
        [Serializable]
        public class CPersonWatchCond 
            : DMD.XML.DMDBaseXMLObject
        {
            private string m_SourceType;
            private int m_SourceID;
            private object m_Source;
            private string m_Descrizione;
            private int m_Gravita;
            private string m_Tag;
            private DateTime m_Data;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonWatchCond()
            {
                m_SourceType = "";
                m_SourceID = 0;
                m_Source = null;
                m_Descrizione = "";
                m_Gravita = 0;
                m_Tag = "";
                m_Data = default;
            }

            /// <summary>
            /// Restituisce o imposta la data in cui si è verificata la condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo dell'oggetto che ha generato la condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceType
            {
                get
                {
                    if (m_Source is object)
                        return DMD.RunTime.vbTypeName(m_Source);
                    return m_SourceType;
                }

                set
                {
                    string oldValue = SourceType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceType = value;
                    DoChanged("SourceType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'oggetto che ha generato la condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto che ha generato la condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object Source
            {
                get
                {
                    if (m_Source is null && !string.IsNullOrEmpty(m_SourceType))
                        m_Source = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_SourceType, m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = m_Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    if (value is null)
                    {
                        m_SourceType = "";
                    }
                    else
                    {
                        m_SourceType = DMD.RunTime.vbTypeName(value);
                    }

                    m_SourceID = DBUtils.GetID(value, 0);
                    DoChanged("Source", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una descrizione della condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta dei parametri per la condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Tag
            {
                get
                {
                    return m_Tag;
                }

                set
                {
                    string oldValue = m_Tag;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tag = value;
                    DoChanged("Tag", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la gravità (crescente) della condizione di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Gravita
            {
                get
                {
                    return m_Gravita;
                }

                set
                {
                    int oldValue = m_Gravita;
                    if (oldValue == value)
                        return;
                    m_Gravita = value;
                    DoChanged("Gravita", value, oldValue);
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("SourceType", SourceType);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Tag", m_Tag);
                writer.WriteAttribute("Gravita", m_Gravita);
                writer.WriteAttribute("Data", m_Data);
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
                    case "SourceType":
                        {
                            m_SourceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tag":
                        {
                            m_Tag = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Gravita":
                        {
                            m_Gravita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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