using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {


        /// <summary>
        /// Rappresenta una persona "attiva" cioè per cui è stata definita almeno una attività nel calendario
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CActivePerson
            : IComparable, IDMDXMLSerializable, IComparable<CActivePerson>
        {
            private const int GIORNATA_INTERA = 1;    // Flag utilizzato nel campo m_Flags (proprietà GiornataIntera)
            private const int PERSONA_GIURIDICA = 2;     // Settato se si tratta di una persona giuridica

            // Private m_IDRicontatto As Integer   'ID dell'oggetto ricontatto
            [NonSerialized] private Anagrafica.CRicontatto m_Ricontatto; // Oggetto ricontatto
            private int m_PersonID;       // ID della persona da ricontattare
            [NonSerialized] private Anagrafica.CPersona m_Person;        // Persona da ricontattare
            private string m_Nominativo;      // Nome della persona o dell'azienda da ricontattare
            private CCollection m_Activities; // Azioni proposte
            private string m_Notes;           // Stringa descrittiva del ricontatto
            private DateTime? m_Data; // Data del ricontatto
            private int m_Flags;          // Flags
            private int m_Promemoria;     // Promemoria in minuti
            private CKeyCollection m_MoreInfo; // (Of String)
            private string m_Categoria;
            private string m_IconURL;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CActivePerson()
            {
               // //DMDObject.IncreaseCounter(this);
                // Me.m_IDRicontatto = 0
                m_Ricontatto = null;
                m_PersonID = 0;
                m_Person = null;
                m_Nominativo = "";
                m_Activities = null;
                m_Notes = "";
                m_Data = default;
                m_Flags = 0;
                m_Promemoria = 0;
                m_MoreInfo = new CKeyCollection(); // (Of Object)
                m_Categoria = "";
                m_IconURL = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="ric"></param>
            public CActivePerson(Anagrafica.CRicontatto ric) : this()
            {
                if (ric is null)
                    throw new ArgumentNullException("ric");
                Ricontatto = ric;
                Data = ric.DataPrevista;
                Notes = ric.Note;
                GiornataIntera = ric.GiornataIntera;
                Promemoria = ric.Promemoria;
                Categoria = ric.Categoria;
                PersonID = ric.IDPersona;
                Person = ric.Persona;
                if (Person is object)
                {
                    IconURL = Person.IconURL;
                }
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="p"></param>
            public CActivePerson(Anagrafica.CPersona p) : this()
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                Ricontatto = new Anagrafica.CRicontatto();
                Ricontatto.Persona = p;
                Ricontatto.DataPrevista = DMD.DateUtils.Now();
                Ricontatto.StatoRicontatto = (Anagrafica.StatoRicontatto)1;
                Data = Ricontatto.DataPrevista;
                Notes = "";
                GiornataIntera = true;
                Promemoria = 0;
                Categoria = "Normale";
                PersonID = DBUtils.GetID(p, 0);
                Person = p;
                IconURL = Person.IconURL;
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
            /// Restituisce o imposta la categoria del ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    m_Categoria = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'icona associata al contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Vero se si tratta di un evento per cui non è stato fissato un orario particolare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool GiornataIntera
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, GIORNATA_INTERA);
                }

                set
                {
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, GIORNATA_INTERA, value);
                }
            }

            /// <summary>
            /// Vero se la persona è una persona giuridica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool PersonaGiuridica
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, PERSONA_GIURIDICA);
                }

                set
                {
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, PERSONA_GIURIDICA, value);
                }
            }

            // ''' <summary>
            // ''' ID del ricontatto corrispondente
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property IDRicontatto As Integer
            // Get
            // Return GetID(Me.m_Ricontatto, Me.m_IDRicontatto)
            // End Get
            // Set(value As Integer)
            // If Me.IDRicontatto = value Then Exit Property
            // Me.m_IDRicontatto = value
            // Me.m_Ricontatto = Nothing
            // End Set
            // End Property

            /// <summary>
            /// Ricontatto corrispondente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CRicontatto Ricontatto
            {
                get
                {
                    // If Me.m_Ricontatto Is Nothing Then Me.m_Ricontatto = Ricontatti.GetItemById(Me.m_IDRicontatto)
                    return m_Ricontatto;
                }

                set
                {
                    m_Ricontatto = value;
                    // Me.m_IDRicontatto = GetID(value)
                }
            }

            /// <summary>
            /// ID della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int PersonID
            {
                get
                {
                    return DBUtils.GetID(m_Person, m_PersonID);
                }

                set
                {
                    if (PersonID == value)
                        return;
                    m_PersonID = value;
                    m_Person = null;
                }
            }

            /// <summary>
            /// Persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Person
            {
                get
                {
                    if (m_Person is null)
                        m_Person = Anagrafica.Persone.GetItemById(m_PersonID);
                    return m_Person;
                }

                set
                {
                    m_Person = value;
                    m_PersonID = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_Nominativo = value.Nominativo;
                        PersonaGiuridica = value.TipoPersona != Anagrafica.TipoPersona.PERSONA_FISICA;
                    }
                }
            }

            /// <summary>
            /// Nominativo della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nominativo
            {
                get
                {
                    return m_Nominativo;
                }

                set
                {
                    m_Nominativo = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Azioni possibili sul ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection Activities
            {
                get
                {
                    if (m_Activities is null)
                        m_Activities = new CCollection();
                    return m_Activities;
                }
            }

            /// <summary>
            /// Data per cui è stato fissato il ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    m_Data = value;
                }
            }

            /// <summary>
            /// Informazioni sul ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Notes
            {
                get
                {
                    return m_Notes;
                }

                set
                {
                    m_Notes = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Promemoria in minuti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Promemoria
            {
                get
                {
                    return m_Promemoria;
                }

                set
                {
                    m_Promemoria = value;
                }
            }

            /// <summary>
            /// Eventuali informazioni aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CKeyCollection MoreInfo // (Of String)
            {
                get
                {
                    return m_MoreInfo;
                }
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("PersonID", PersonID);
                writer.WriteAttribute("Nominativo", m_Nominativo);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("Promemoria", m_Promemoria);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteTag("Ricontatto", Ricontatto);
                writer.WriteTag("Activities", Activities);
                writer.WriteTag("Notes", m_Notes);
                writer.WriteTag("MoreInfo", m_MoreInfo);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "PersonID":
                        {
                            m_PersonID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Ricontatto":
                        {
                            m_Ricontatto = (Anagrafica.CRicontatto)fieldValue; // DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                            break;
                        }

                    case "Nominativo":
                        {
                            m_Nominativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Activities":
                        {
                            m_Activities = (CCollection)fieldValue;
                            break;
                        }

                    case "Data":
                        {
                            m_Data = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Notes":
                        {
                            m_Notes = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Promemoria":
                        {
                            m_Promemoria = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MoreInfo":
                        {
                            m_MoreInfo = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CActivePerson obj)
            {
                var d = DMD.DateUtils.Now();
                var d1 = (this.GiornataIntera)? DMD.DateUtils.GetDatePart(this.m_Data) : this.m_Data;
                var d2 = (obj.GiornataIntera) ? DMD.DateUtils.GetDatePart(obj.m_Data) : obj.m_Data;
                long? diff1 = DMD.Longs.Add(DMD.DateUtils.DateDiff("m", d1, d), this.Promemoria);
                long? diff2 = DMD.Longs.Add(DMD.DateUtils.DateDiff("m", d2, d), obj.Promemoria);
                int ret = DMD.Longs.Compare(diff1 , diff2);
                if (ret == 0) ret = DMD.Strings.Compare(m_Nominativo, obj.m_Nominativo, true);
                return ret;
                 
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CActivePerson)obj);
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CActivePerson()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}