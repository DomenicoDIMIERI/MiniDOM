using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Rappresenta una relazione di parentela/affinità tra due persone fisiche
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CRelazioneParentale
            : Databases.DBObject, IComparable, IComparable<CRelazioneParentale>
        {
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private string m_NomeRelazione;
            private int m_IDPersona1;
            [NonSerialized] private CPersonaFisica m_Persona1;
            private string m_NomePersona1;
            private int m_IDPersona2;
            [NonSerialized] private CPersonaFisica m_Persona2;
            private string m_NomePersona2;
            private int m_Ordine1;
            private string m_Descrizione1;
            private int m_Ordine2;
            private string m_Descrizione2;
             

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRelazioneParentale()
            {
                m_DataInizio = default;
                m_DataFine = default;
                m_NomeRelazione = "";
                m_Persona1 = null;
                m_IDPersona1 = 0;
                m_NomePersona1 = "";
                m_Persona2 = null;
                m_IDPersona2 = 0;
                m_NomePersona2 = "";
                m_Ordine1 = 0;
                m_Descrizione1 = "";
                m_Ordine2 = 0;
                m_Descrizione2 = "";
                 
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica l'ordine di precedenza nell'ordinamento (crescente)
            /// </summary>
            /// <returns></returns>
            public int Ordine1
            {
                get
                {
                    return m_Ordine1;
                }

                set
                {
                    int oldValue = m_Ordine1;
                    if (oldValue == value)
                        return;
                    m_Ordine1 = value;
                    DoChanged("Ordine1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una descrizione per la relazione
            /// </summary>
            /// <returns></returns>
            public string Descrizione1
            {
                get
                {
                    return m_Descrizione1;
                }

                set
                {
                    string oldValue = m_Descrizione1;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione1 = value;
                    DoChanged("Descrizione1", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta un valore che indica l'ordine di precedenza nell'ordinamento (crescente)
            /// </summary>
            /// <returns></returns>
            public int Ordine2
            {
                get
                {
                    return m_Ordine2;
                }

                set
                {
                    int oldValue = m_Ordine2;
                    if (oldValue == value)
                        return;
                    m_Ordine2 = value;
                    DoChanged("Ordine2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una descrizione per la relazione
            /// </summary>
            /// <returns></returns>
            public string Descrizione2
            {
                get
                {
                    return m_Descrizione2;
                }

                set
                {
                    string oldValue = m_Descrizione2;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione2 = value;
                    DoChanged("Descrizione2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la data di inizio della relazione (valida in particolare per le affinità)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la data di fine della relazione (valida in particolare per le affinità)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della relazione (es. Coniuge, Figlio, Genitore, ecc..)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeRelazione
            {
                get
                {
                    return m_NomeRelazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRelazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRelazione = value;
                    DoChanged("NomeRelazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona (soggetto della relazione)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersonaFisica Persona1
            {
                get
                {
                    if (m_Persona1 is null)
                        m_Persona1 = (CPersonaFisica)Persone.GetItemById(m_IDPersona1);
                    return m_Persona1;
                }

                set
                {
                    var oldValue = m_Persona1;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona1 = value;
                    m_IDPersona1 = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersona1 = value.Nominativo;
                    DoChanged("Persona1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della Persona1
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersona1
            {
                get
                {
                    return DBUtils.GetID(m_Persona1, m_IDPersona1);
                }

                set
                {
                    int oldValue = IDPersona1;
                    if (oldValue == value)
                        return;
                    m_IDPersona1 = value;
                    m_Persona1 = null;
                    DoChanged("IDPersona1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della Persona1
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePersona1
            {
                get
                {
                    return m_NomePersona1;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona1;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona1 = value;
                    DoChanged("NomePersona1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona (oggetto della relazione)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersonaFisica Persona2
            {
                get
                {
                    if (m_Persona2 is null)
                        m_Persona2 = (CPersonaFisica)Persone.GetItemById(m_IDPersona2);
                    return m_Persona2;
                }

                set
                {
                    var oldValue = m_Persona2;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona2 = value;
                    m_IDPersona2 = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersona2 = value.Nominativo;
                    DoChanged("Persona2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della Persona2
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersona2
            {
                get
                {
                    return DBUtils.GetID(m_Persona2, m_IDPersona2);
                }

                set
                {
                    int oldValue = IDPersona2;
                    if (oldValue == value)
                        return;
                    m_IDPersona2 = value;
                    m_Persona2 = null;
                    DoChanged("IDPersona2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della Persona1
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePersona2
            {
                get
                {
                    return m_NomePersona2;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona2;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona2 = value;
                    DoChanged("NomePersona2", value, oldValue);
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.RelazioniParentali; //.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PersoneRelazioni";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                m_NomeRelazione = reader.Read("NomeRelazione", this.m_NomeRelazione);
                m_IDPersona1 = reader.Read("IDPersona1", this.m_IDPersona1);
                m_NomePersona1 = reader.Read("NomePersona1", this.m_NomePersona1);
                m_IDPersona2 = reader.Read("IDPersona2", this.m_IDPersona2);
                m_NomePersona2 = reader.Read("NomePersona2", this.m_NomePersona2);
                m_Ordine1 = reader.Read("Ordine1", this.m_Ordine1);
                m_Descrizione1 = reader.Read("Descrizione1", this.m_Descrizione1);
                m_Ordine2 = reader.Read("Ordine2", this.m_Ordine2);
                m_Descrizione2 = reader.Read("Descrizione2", this.m_Descrizione2);
                 
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("NomeRelazione", m_NomeRelazione);
                writer.Write("IDPersona1", IDPersona1);
                writer.Write("NomePersona1", m_NomePersona1);
                writer.Write("IDPersona2", IDPersona2);
                writer.Write("NomePersona2", m_NomePersona2);
                writer.Write("Ordine1", m_Ordine1);
                writer.Write("Descrizione1", m_Descrizione1);
                writer.Write("Ordine2", m_Ordine2);
                writer.Write("Descrizione2", m_Descrizione2);
                 
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("NomeRelazione", typeof(string), 255);
                c = table.Fields.Ensure("IDPersona1", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona1", typeof(string), 255);
                c = table.Fields.Ensure("IDPersona2", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona2", typeof(string), 255);
                c = table.Fields.Ensure("Ordine1", typeof(int), 1);
                c = table.Fields.Ensure("Descrizione1", typeof(string), 255);
                c = table.Fields.Ensure("Ordine2", typeof(int), 1);
                c = table.Fields.Ensure("Descrizione2", typeof(string), 255);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNomi", new string[] { "NomeRelazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona1", new string[] { "IDPersona1", "NomePersona1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona2", new string[] { "IDPersona2", "NomePersona2" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescr1", new string[] { "Ordine1", "Descrizione1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescr2", new string[] { "Ordine2", "Descrizione2" }, DBFieldConstraintFlags.None);
                
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("NomeRelazione", m_NomeRelazione);
                writer.WriteAttribute("IDPersona1", IDPersona1);
                writer.WriteAttribute("NomePersona1", m_NomePersona1);
                writer.WriteAttribute("IDPersona2", IDPersona2);
                writer.WriteAttribute("NomePersona2", m_NomePersona2);
                writer.WriteAttribute("Ordine1", m_Ordine1);
                writer.WriteAttribute("Descrizione1", m_Descrizione1);
                writer.WriteAttribute("Ordine2", m_Ordine2);
                writer.WriteAttribute("Descrizione2", m_Descrizione2);
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
                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NomeRelazione":
                        {
                            m_NomeRelazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersona1":
                        {
                            m_IDPersona1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona1":
                        {
                            m_NomePersona1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersona2":
                        {
                            m_IDPersona2 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona2":
                        {
                            m_NomePersona2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Ordine1":
                        {
                            m_Ordine1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Descrizione1":
                        {
                            m_Descrizione1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Ordine2":
                        {
                            m_Ordine2 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Descrizione2":
                        {
                            m_Descrizione2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return DMD.Strings.ConcatArray(
                            m_NomePersona1, " ", m_NomeRelazione, " ", m_NomePersona2,
                            " dal ", Sistema.Formats.FormatUserDate(m_DataInizio),
                            " al ", Sistema.Formats.FormatUserDate(m_DataFine)
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'ogggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeRelazione, this.m_NomePersona1, this.m_NomePersona2, this.m_DataInizio);
            }

            /// <summary>
            /// Compara i due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CRelazioneParentale obj)
            {
                int ret = m_Ordine1.CompareTo(obj.m_Ordine1);
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_NomePersona2, obj.m_NomePersona2, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CRelazioneParentale)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CRelazioneParentale) && this.Equals((CRelazioneParentale)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRelazioneParentale obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Strings.EQ(this.m_NomeRelazione, obj.m_NomeRelazione)
                    && DMD.Integers.EQ(this.IDPersona1, obj.IDPersona1)
                    && DMD.Strings.EQ(this.m_NomePersona1, obj.m_NomePersona1)
                    && DMD.Integers.EQ(this.IDPersona2, obj.IDPersona2)
                    && DMD.Strings.EQ(this.m_NomePersona2, obj.m_NomePersona2)
                    && DMD.Integers.EQ(this.m_Ordine1, obj.m_Ordine1)
                    && DMD.Strings.EQ(this.m_Descrizione1, obj.m_Descrizione1)
                    && DMD.Integers.EQ(this.m_Ordine2, obj.m_Ordine2)
                    && DMD.Strings.EQ(this.m_Descrizione2, obj.m_Descrizione2)
                    ; //private CKeyCollection m_Parameters;
            }


        }
    }
}