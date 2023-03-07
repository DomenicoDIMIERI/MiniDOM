using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta una regione
        /// </summary>
        [Serializable]
        public class CRegione 
            : Luogo
        {
            private string m_Sigla;
            private string m_Nazione;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegione()
            {
                m_Sigla = "";
                m_Nazione = "";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Regioni; //.Module;
            }

              
            /// <summary>
            /// Sigla
            /// </summary>
            public string Sigla
            {
                get
                {
                    return m_Sigla;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Sigla;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sigla = value;
                    DoChanged("Sigla", value, oldValue);
                }
            }

            /// <summary>
            /// Nazione
            /// </summary>
            public string Nazione
            {
                get
                {
                    return m_Nazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nazione = value;
                    DoChanged("Regione", value, oldValue);
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Luoghi_Regioni";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Sigla = reader.Read("Sigla", m_Sigla);
                m_Nazione = reader.Read("Nazione", m_Nazione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Sigla", this.m_Sigla);
                writer.Write("Nazione", this.m_Nazione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Sigla", typeof(string), 255);
                c = table.Fields.Ensure("Nazione", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNazione", new string[] { "Sigla", "Nazione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Sigla", m_Sigla);
                writer.WriteAttribute("Nazione", m_Nazione);
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
                    case "Sigla":
                        {
                            m_Sigla = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nazione":
                        {
                            m_Nazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Luogo obj)
            {
                return (obj is CRegione) && this.Equals((CRegione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRegione obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Sigla, obj.m_Sigla)
                    && DMD.Strings.EQ(this.m_Nazione, obj.m_Nazione)
                    ;
            }

        }
    }
}