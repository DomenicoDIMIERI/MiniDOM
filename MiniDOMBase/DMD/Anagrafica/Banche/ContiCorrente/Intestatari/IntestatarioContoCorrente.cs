using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Intestatario di un conto corrente
        /// </summary>
        [Serializable]
        public class IntestatarioContoCorrente 
            : Databases.DBObject
        {
            private int m_IDContoCorrente;
            [NonSerialized] private ContoCorrente m_ContoCorrente;
            private string m_NomeConto;
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_NomePersona;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
             

            /// <summary>
            /// Costruttore
            /// </summary>
            public IntestatarioContoCorrente()
            {
                m_IDContoCorrente = 0;
                m_ContoCorrente = null;
                m_NomeConto = "";
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_Flags = 0;                 
            }

            

            /// <summary>
            /// Restituisce o imposta l'ID del conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContoCorrente
            {
                get
                {
                    return DBUtils.GetID(m_ContoCorrente, m_IDContoCorrente);
                }

                set
                {
                    int oldValue = IDContoCorrente;
                    if (oldValue == value)
                        return;
                    m_IDContoCorrente = value;
                    m_ContoCorrente = null;
                    DoChanged("IDContoCorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il conto corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public ContoCorrente ContoCorrente
            {
                get
                {
                    if (m_ContoCorrente is null)
                        m_ContoCorrente = ContiCorrente.GetItemById(m_IDContoCorrente);
                    return m_ContoCorrente;
                }

                set
                {
                    var oldValue = m_ContoCorrente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ContoCorrente = value;
                    m_IDContoCorrente = DBUtils.GetID(value, this.m_IDContoCorrente);
                    m_NomeConto = "";
                    if (value is object)
                        m_NomeConto = value.Nome;
                    DoChanged("ContoCorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del conto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeConto
            {
                get
                {
                    return m_NomeConto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeConto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeConto = value;
                    DoChanged("NomeConto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il conto corrente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetContoCorrente(ContoCorrente value)
            {
                m_ContoCorrente = value;
                m_IDContoCorrente = DBUtils.GetID(value, m_IDContoCorrente);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'intestatario
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
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, this.m_IDPersona);
                    m_NomePersona = "";
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta il nome della persona
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetPersona(CPersona value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value, this.m_IDPersona);
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio intestazione
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInzio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di recesso dal conto corrente
            /// </summary>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

             

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ContiCorrente.Intestatari;
            }

            /// <summary>
            /// Restituisce il discriminatore
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ContiCorrentiInt";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta un oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.NomeConto , " / " , this.NomePersona);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeConto, this.m_NomePersona);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is IntestatarioContoCorrente) && this.Equals((IntestatarioContoCorrente)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(IntestatarioContoCorrente obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.IDContoCorrente, obj.IDContoCorrente)
                    && DMD.Strings.EQ(this.m_NomeConto, obj.m_NomeConto)
                    && DMD.Integers.EQ(this.IDPersona, obj.IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    ; // CKeyCollection m_Parameters;

        }

        /// <summary>
        /// Carica dal db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDContoCorrente = reader.Read("IDContoCorrente", m_IDContoCorrente);
                m_NomeConto = reader.Read("NomeConto",  m_NomeConto);
                m_IDPersona = reader.Read("IDPersona",  m_IDPersona);
                m_NomePersona = reader.Read("NomePersona",  m_NomePersona);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataFine = reader.Read("DataFine",  m_DataFine);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDContoCorrente", IDContoCorrente);
                writer.Write("NomeConto", m_NomeConto);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDContoCorrente", typeof(int), 1);
                c = table.Fields.Ensure("NomeConto", typeof(string), 255);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("Flags", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxConto", new string[] { "IDContoCorrente", "NomeConto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine", "Flags" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDContoCorrente", IDContoCorrente);
                writer.WriteAttribute("NomeConto", m_NomeConto);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
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
                    case "IDContoCorrente":
                        {
                            m_IDContoCorrente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConto":
                        {
                            m_NomeConto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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