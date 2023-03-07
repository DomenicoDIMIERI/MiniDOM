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
        /// Rappresenta un curriculum vitae
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Curriculum 
            : minidom.Databases.DBObjectPO
        {
            private DateTime? m_DataPresentazione;
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_NomePersona;
            private string m_Descrizione;
            [NonSerialized] private CAttachment m_Allegato;
            private int m_IDAllegato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Curriculum()
            {
                m_DataPresentazione = default;
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_Allegato = null;
                m_IDAllegato = 0;
            }

            /// <summary>
            /// DataPresentazione
            /// </summary>
            public DateTime? DataPresentazione
            {
                get
                {
                    return m_DataPresentazione;
                }

                set
                {
                    var oldValue = m_DataPresentazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPresentazione = value;
                    DoChanged("DataPresentazione", value, oldValue);
                }
            }

            /// <summary>
            /// Giorni Dalla Presentazione
            /// </summary>
            /// <returns></returns>
            public int? GiorniDallaPresentazione()
            {
                if (m_DataPresentazione.HasValue == false)
                    return default;
                return (int?)DMD.DateUtils.DateDiff(DateTimeInterval.Day, DMD.DateUtils.Now(), m_DataPresentazione.Value);
            }

            /// <summary>
            /// IDPersona
            /// </summary>
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
            /// Persona
            /// </summary>
            public Anagrafica.CPersona Persona
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
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Nome Persona
            /// </summary>
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
            /// Descrizione
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Allegato
            /// </summary>
            public CAttachment Allegato
            {
                get
                {
                    if (m_Allegato is null)
                        m_Allegato = Sistema.Attachments.GetItemById(m_IDAllegato);
                    return m_Allegato;
                }

                set
                {
                    var oldValue = m_Allegato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Allegato = value;
                    m_IDAllegato = DBUtils.GetID(value, 0);
                    DoChanged("Allegato", value, oldValue);
                }
            }

            /// <summary>
            /// IDAllegato
            /// </summary>
            public int IDAllegato
            {
                get
                {
                    return DBUtils.GetID(m_Allegato, m_IDAllegato);
                }

                set
                {
                    int oldValue = IDAllegato;
                    if (oldValue == value)
                        return;
                    m_IDAllegato = value;
                    m_Allegato = null;
                    DoChanged("IDAllegato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_NomePersona , " " , Sistema.Formats.FormatUserDateTime(m_DataPresentazione));
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDPersona);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Curriculum) && this.Equals((Curriculum)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Curriculum obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataPresentazione, obj.m_DataPresentazione)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Integers.EQ(this.m_IDAllegato, obj.m_IDAllegato)
                    ;

            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Curricula;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeCurricula";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_DataPresentazione = reader.Read("DataPresentazione", m_DataPresentazione);
                m_IDPersona = reader.Read("IDPersona",  m_IDPersona);
                m_NomePersona = reader.Read("NomePersona",  m_NomePersona);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_IDAllegato = reader.Read("IDAllegato",  m_IDAllegato);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataPresentazione", m_DataPresentazione);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDAllegato", IDAllegato);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataPresentazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("IDAllegato", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDate", new string[] { "DataPresentazione" , "IDPersona", "NomePersona" , "IDAllegato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataPresentazione", m_DataPresentazione);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("IDAllegato", IDAllegato);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
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
                    case "DataPresentazione":
                        {
                            m_DataPresentazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAllegato":
                        {
                            m_IDAllegato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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