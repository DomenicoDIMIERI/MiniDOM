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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Rappresenta un collegamento tra una persona ed un messaggio email
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class PersonaPerEMail
            : Databases.DBObjectBase
        {
            private int m_IDMessaggio;
            [NonSerialized] private MailMessage m_Messaggio;
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_NomePersona;
            private string m_IconURL;
            private string m_Indirizzo;
            private DateTime? m_DataMessaggio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public PersonaPerEMail()
            {
                m_IDMessaggio = 0;
                m_Messaggio = null;
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_IconURL = "";
                m_Indirizzo = "";
                m_Flags = 0;
                m_DataMessaggio = default;
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new MailFlags Flags
            {
                get
                {
                    return (MailFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (value == oldValue)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Testa il flag
            /// </summary>
            /// <param name="flag"></param>
            /// <returns></returns>
            public bool TestFlag(MailFlags flag)
            {
                return DMD.RunTime.TestFlag(this.Flags, flag);
            }

            /// <summary>
            /// Setta il flag
            /// </summary>
            /// <param name="flag"></param>
            /// <param name="value"></param>
            public void SetFlag(MailFlags flag, bool value)
            {
                if (TestFlag(flag) == value)
                    return;
                var oldValue = this.Flags;
                m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, flag, value);
                DoChanged("Flags", value, oldValue);
            }

            /// <summary>
            /// Restituisce o imposta l'ID del messaggio associato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDMessaggio
            {
                get
                {
                    return DBUtils.GetID(m_Messaggio, m_IDMessaggio);
                }

                set
                {
                    int oldValue = IDMessaggio;
                    if (oldValue == value)
                        return;
                    m_IDMessaggio = value;
                    m_Messaggio = null;
                    DoChanged("IDMessaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il messaggio associato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailMessage Messaggio
            {
                get
                {
                    if (m_Messaggio is null)
                        m_Messaggio = Mails.GetItemById(m_IDMessaggio);
                    return m_Messaggio;
                }

                set
                {
                    var oldValue = Messaggio;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Messaggio = value;
                    m_IDMessaggio = DBUtils.GetID(value, 0);
                    DoChanged("Messaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona a cui è associato il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    m_Persona = value;
                    if (value is object)
                    {
                        m_NomePersona = value.Nominativo;
                        m_IconURL = value.IconURL;
                    }

                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    string oldValue = m_NomePersona;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'icona associata alla persona
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
                    string oldValue = m_IconURL;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo email che correla la persona al messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }

                set
                {
                    string oldValue = m_Indirizzo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Indirizzo = value;
                    DoChanged("Indirizzo", value, oldValue);
                }
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PersoneXEMail";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDMessaggio = reader.Read("IDMessaggio", m_IDMessaggio);
                m_IDPersona = reader.Read("IDPersona", m_IDPersona);
                m_NomePersona = reader.Read("NomePersona", m_NomePersona);
                m_IconURL = reader.Read("IconURL", m_IconURL);
                m_Indirizzo = reader.Read("Indirizzo", m_Indirizzo);
                m_DataMessaggio = reader.Read("DataMessaggio", m_DataMessaggio);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDMessaggio", IDMessaggio);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("IconURL", m_IconURL);
                writer.Write("Indirizzo", m_Indirizzo);
                writer.Write("DataMessaggio", m_DataMessaggio);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDMessaggio", typeof(int), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo", typeof(string), 255);
                c = table.Fields.Ensure("DataMessaggio", typeof(DateTime), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxMessaggio", new string[] { "IDMessaggio", "DataMessaggio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona", "IconURL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo", new string[] { "Indirizzo" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDMessaggio", IDMessaggio);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("Indirizzo", m_Indirizzo);
                writer.WriteAttribute("DataMessaggio", m_DataMessaggio);
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
                    case "IDMessaggio":
                        {
                            m_IDMessaggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

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

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                  
                    case "DataMessaggio":
                        {
                            m_DataMessaggio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
                return DMD.Strings.ConcatArray(this.m_NomePersona , " <" , m_Indirizzo , ">");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_Indirizzo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is PersonaPerEMail) && this.Equals((PersonaPerEMail)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(PersonaPerEMail obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDMessaggio, obj.m_IDMessaggio)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.Strings.EQ(this.m_Indirizzo, obj.m_Indirizzo)
                    && DMD.DateUtils.EQ(this.m_DataMessaggio, obj.m_DataMessaggio)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Mails.PersonePerEMail;
            }

        }
    }
}