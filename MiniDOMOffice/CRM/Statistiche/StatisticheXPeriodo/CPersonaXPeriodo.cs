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
        /// Persona x periodo
        /// </summary>
        [Serializable]
        public class CPersonaXPeriodo 
            : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<CPersonaXPeriodo>
        {
            /// <summary>
            /// ID della persona
            /// </summary>
            public int IDPersona;

            /// <summary>
            /// Nome della persona
            /// </summary>
            public string NomePersona;
            [NonSerialized] private Anagrafica.CPersona m_Persona;

            /// <summary>
            /// Data (dd/MM/yyyy)
            /// </summary>
            public DateTime Data;

            /// <summary>
            /// Numero chiamate effettuate
            /// </summary>
            public int ConteggioChiamate;

            /// <summary>
            /// Numero chiamate a cui ha risposto
            /// </summary>
            public int ConteggioRisposte;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonaXPeriodo()
            {
                this.IDPersona = 0;
                this.NomePersona = "";
                this.m_Persona = null;
                this.Data = default;
            }

            /// <summary>
            /// Persona
            /// </summary>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(IDPersona);
                    return m_Persona;
                }

                set
                {
                    m_Persona = value;
                    IDPersona = DBUtils.GetID(value, 0);
                    NomePersona = (value is object)? value.Nominativo : "";
                }
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
                    case "IDPersona":
                        {
                            IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ConteggioChiamate":
                        {
                            ConteggioChiamate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConteggioRisposte":
                        {
                            ConteggioRisposte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", NomePersona);
                writer.WriteAttribute("Data", Data);
                base.XMLSerialize(writer);
                writer.WriteAttribute("ConteggioChiamate", ConteggioChiamate);
                writer.WriteAttribute("ConteggioRisposte", ConteggioRisposte);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CPersonaXPeriodo obj)
            {
                int ret = DMD.DateUtils.Compare(Data, obj.Data);
                if (ret == 0)
                    ret = DMD.Strings.Compare(NomePersona, obj.NomePersona, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CPersonaXPeriodo)obj);
            }
 
        }
    }
}