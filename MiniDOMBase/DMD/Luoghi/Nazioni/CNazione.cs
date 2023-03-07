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
        /// Nazione
        /// </summary>
        [Serializable]
        public class CNazione 
            : LuogoISTAT
        {
            private string m_SantoPatrono;
            private string m_GiornoFestivo;
            private string m_Prefisso;
            private string m_Sigla;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CNazione()
            {
                this.m_SantoPatrono = "";
                this.m_GiornoFestivo = "";
                this.m_Prefisso = "";
                this.m_Sigla = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="nome"></param>
            public CNazione(string nome)
                : base(nome)
            {
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(LuogoISTAT obj)
            {
                return (obj is CNazione) && this.Equals((CNazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CNazione obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_SantoPatrono, obj.m_SantoPatrono)
                    && DMD.Strings.EQ(this.m_GiornoFestivo, obj.m_GiornoFestivo)
                    && DMD.Strings.EQ(this.m_Prefisso, obj.m_Prefisso)
                    && DMD.Strings.EQ(this.m_Sigla, obj.m_Sigla)
                    ;
            }

            /// <summary>
            /// Restituisce o imposta il nome del santo patrono
            /// </summary>
            public string SantoPatrono
            {
                get
                {
                    return m_SantoPatrono;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SantoPatrono;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SantoPatrono = value;
                    DoChanged("SantoPatrono", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive la data del giorno festivo per il santo patrono
            /// </summary>
            public string GiornoFestivo
            {
                get
                {
                    return m_GiornoFestivo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_GiornoFestivo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_GiornoFestivo = value;
                    DoChanged("GiornoFestivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il prefisso telefonico principale
            /// </summary>
            public string Prefisso
            {
                get
                {
                    return m_Prefisso;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Prefisso;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Prefisso = value;
                    DoChanged("Prefisso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sigla della nazione
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

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Nazioni; //.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Luoghi_Nazioni";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_SantoPatrono = reader.Read("SantoPatrono", m_SantoPatrono);
                m_GiornoFestivo = reader.Read("GiornoFestivo", m_GiornoFestivo);
                m_Prefisso = reader.Read("Prefisso", m_Prefisso);
                m_Sigla = reader.Read("Sigla", m_Sigla);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("SantoPatrono", m_SantoPatrono);
                writer.Write("GiornoFestivo", m_GiornoFestivo);
                writer.Write("Prefisso", m_Prefisso);
                writer.Write("Sigla", m_Sigla);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("SantoPatrono", typeof(string), 255);
                c = table.Fields.Ensure("GiornoFestivo", typeof(string), 255);
                c = table.Fields.Ensure("Prefisso", typeof(string), 255);
                c = table.Fields.Ensure("Sigla", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxSanto", new string[] { "SantoPatrono", "GiornoFestivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPrefisso", new string[] { "Sigla", "Prefisso" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("SantoPatrono", m_SantoPatrono);
                writer.WriteAttribute("GiornoFestivo", m_GiornoFestivo);
                writer.WriteAttribute("Prefisso", m_Prefisso);
                writer.WriteAttribute("Sigla", m_Sigla);
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
                    case "SantoPatrono":
                        {
                            this.m_SantoPatrono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue, this.m_SantoPatrono);
                            break;
                        }

                    case "GiornoFestivo":
                        {
                            this.m_GiornoFestivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue, this.m_GiornoFestivo);
                            break;
                        }

                    case "Prefisso":
                        {
                            this.m_Prefisso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue, this.m_Prefisso);
                            break;
                        }

                    case "Sigla":
                        {
                            this.m_Sigla = DMD.XML.Utils.Serializer.DeserializeString(fieldValue, this.m_Prefisso);
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