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
        /// Rappresenta una provincia
        /// </summary>
        [Serializable]
        public class CProvincia 
            : Luogo
        {
            private string m_Sigla;
            private string m_Regione;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CProvincia()
            {
                this.m_Sigla = "";
                this.m_Regione = "";                 
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Province; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta la sigla della provincia
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
            /// Restituisce o imposta la regione
            /// </summary>
            public string Regione
            {
                get
                {
                    return m_Regione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Regione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Regione = value;
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
                return "tbl_Luoghi_Province";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Sigla = reader.Read("Sigla", this.m_Sigla);
                this.m_Regione = reader.Read("Regione", this.m_Regione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Sigla", m_Sigla);
                writer.Write("Regione", m_Regione);
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
                c = table.Fields.Ensure("Regione", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxSigla", new string[] { "Sigla", "Regione" }, DBFieldConstraintFlags.None);
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
                writer.WriteAttribute("m_Sigla", m_Sigla);
                writer.WriteAttribute("m_Regione", m_Regione);
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
                    case "m_Sigla":
                        {
                            m_Sigla = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Regione":
                        {
                            m_Regione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Luogo obj)
            {
                return (obj is CProvincia) && this.Equals((CProvincia)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CProvincia obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Sigla, obj.m_Sigla)
                    && DMD.Strings.EQ(this.m_Regione, obj.m_Regione)
                    ;
            }

        }
    }
}