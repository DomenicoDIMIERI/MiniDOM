using DMD.XML;
using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;


namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Oggetto "light" che racchiude informazioni su una persona o un'azienda
        /// </summary>
        [Serializable]
        public class CPersonaInfo 
            : IDMDXMLSerializable, IComparable, IComparable<CPersonaInfo>
        {
            private CKeyCollection m_Attributes;
            private string m_IconURL;
            private bool m_Deceduto;
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_NomePersona;
            private string m_Notes;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonaInfo()
            {
                ////DMDObject.IncreaseCounter(this);
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_Notes = "";
                m_IconURL = "";
                m_Deceduto = false;
                this.m_Attributes = new CKeyCollection();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="persona"></param>
            public CPersonaInfo(CPersona persona) : this()
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                Parse(persona);
            }

            /// <summary>
            /// Inizializza l'oggetto con la persona e prepara le info
            /// </summary>
            /// <param name="persona"></param>
            protected virtual void Parse(CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");

                m_Persona = persona;
                m_IconURL = persona.IconURL;
                m_Deceduto = persona.Deceduto;
                m_IDPersona = DBUtils.GetID(persona, 0);
                m_NomePersona = persona.Nominativo;
                m_Notes = "";
                string txtN = Strings.EQ(m_Persona.Sesso , "F", true)? "Nata" : "Nato";
                // If TypeOf (Me.m_Persona) Is CPersonaFisica Then
                // If Me.m_Persona.ImpiegoPrincipale IsNot Nothing Then
                // With DirectCast(Me.m_Persona, CPersonaFisica).ImpiegoPrincipale.ToString <> ""
                // If 
                // End With

                // End If
                // End If
                if (!string.IsNullOrEmpty(m_Persona.NatoA.ToString()))
                    m_Notes = DMD.Strings.Combine(m_Notes, txtN + " a: " + m_Persona.NatoA.ToString(), ", ");
                if (m_Persona.DataNascita.HasValue)
                    m_Notes = DMD.Strings.Combine(m_Notes, txtN + " il: " + Sistema.Formats.FormatUserDate(m_Persona.DataNascita), ", ");
                if (!string.IsNullOrEmpty(m_Persona.DomiciliatoA.ToString()))
                {
                    // Me.m_Notes = DMD.Strings.Combine(Me.m_Notes, "Domiciliato a: " & Me.m_Persona.DomiciliatoA.ToString, ", ")
                    m_Notes = DMD.Strings.Combine(m_Notes, m_Persona.DomiciliatoA.ToString(), ", ");
                }
                else if (!string.IsNullOrEmpty(m_Persona.ResidenteA.ToString()))
                {
                    // Me.m_Notes = DMD.Strings.Combine(Me.m_Notes, "Residente a: " & Me.m_Persona.ResidenteA.ToString, ", ")
                    m_Notes = DMD.Strings.Combine(m_Notes, m_Persona.ResidenteA.ToString(), ", ");
                }

                if (!string.IsNullOrEmpty(m_Persona.CodiceFiscale))
                    m_Notes = DMD.Strings.Combine(m_Notes, "C.F.: " + m_Persona.CodiceFiscale, ", ");
                Attributes.Add("Titolo", m_Persona.Titolo);
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }

            /// <summary>
            /// Attributi aggiuntivi
            /// </summary>
            public CKeyCollection Attributes
            {
                get
                {
                    return m_Attributes;
                }
            }

            /// <summary>
            /// Icona della persona
            /// </summary>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    m_IconURL = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Se vero indica che la persona è deceduta
            /// </summary>
            public bool Deceduto
            {
                get
                {
                    return m_Deceduto;
                }

                set
                {
                    m_Deceduto = value;
                }
            }

            /// <summary>
            /// ID della persona
            /// </summary>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    if (IDPersona == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                }
            }

            /// <summary>
            /// Persona
            /// </summary>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                }
            }

            /// <summary>
            /// Nominativo della persona
            /// </summary>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    m_NomePersona = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public string Notes
            {
                get
                {
                    return m_Notes;
                }

                set
                {
                    m_Notes = value;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", m_IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("Deceduto", m_Deceduto);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteTag("Attributes", Attributes);
                writer.WriteTag("Notes", m_Notes);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Notes":
                        {
                            m_Notes = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Deceduto":
                        {
                            m_Deceduto = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attributes":
                        {
                            this.m_Attributes = (CKeyCollection)fieldValue;
                            break;
                        }
                }
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CPersonaInfo obj)
            {
                return DMD.Strings.Compare(NomePersona, obj.NomePersona, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CPersonaInfo)obj);
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CPersonaInfo()
            {
               // //DMDObject.DecreaseCounter(this);
            }
        }
    }
}