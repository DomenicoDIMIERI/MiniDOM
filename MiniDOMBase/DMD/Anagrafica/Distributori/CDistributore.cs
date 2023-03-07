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
        /// Distributore
        /// </summary>
        [Serializable]
        public class CDistributore 
            : Databases.DBObject, IFonte, ICloneable
        {
            private string m_Nome;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private bool m_Attivo;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CDistributore()
            {
                this.m_Nome = "";
                this.m_DataInizio = default;
                this.m_DataFine = default;
                this.m_Attivo = true;
                 
            }
              

            /// <summary>
            /// Restituisce o imposta il nome del distributore
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta la data di attivazione del distributore
            /// </summary>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di disattivazione del distributore
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
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il distributore è attivo
            /// </summary>
            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Restituisce true se il distributore è valido alla data specificata
            /// </summary>
            /// <param name="atDate"></param>
            /// <returns></returns>
            public bool IsValid(DateTime atDate)
            {
                return DMD.DateUtils.CheckBetween(atDate, m_DataInizio, m_DataFine);
            }

            /// <summary>
            /// Restituisce true se il distributore è valido alla data odierna
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Restitusice una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CDistributore) && this.Equals((CDistributore)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CDistributore obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Booleans.EQ(this.m_Attivo, obj.m_Attivo)
                    ;
            //private CKeyCollection m_Parameters;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Distributori; //.Module;
            }

            /// <summary>
            /// Discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Distributori";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", m_DataFine);
                this.m_Attivo = reader.Read("Attivo", m_Attivo);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Attivo", m_Attivo);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("Attivo", typeof(bool), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine", "Attivo", "Flags" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Attivo", m_Attivo);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CDistributore Clone()
            {
                return (CDistributore)this.MemberwiseClone();
            }

            object ICloneable.Clone() { return this.Clone(); }

            string IFonte.Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            string IFonte.IconURL
            {
                get
                {
                    return "/widgets/images/default.gif";
                }
            }
        }
    }
}