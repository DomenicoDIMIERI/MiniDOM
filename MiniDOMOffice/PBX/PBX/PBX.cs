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
        /// Flags per gli oggetti <see cref="PBX"/>
        /// </summary>
        [Flags]
        public enum PBXFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }

        /// <summary>
        /// PBX
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class PBX 
            : minidom.Databases.DBObjectPO
        {
            private string m_Nome;
            private string m_Tipo;
            private string m_Versione;
            private DateTime? m_DataInstallazione;
            private DateTime? m_DataDismissione;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public PBX()
            {
                m_Nome = "";
                m_Tipo = "";
                m_Versione = "";
                m_DataInstallazione = default;
                m_DataDismissione = default;
                m_Flags = (int)PBXFlags.None;
            }

            /// <summary>
            /// Nome
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    string oldValue = m_Nome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Tipo
            /// </summary>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    string oldValue = m_Tipo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Versione
            /// </summary>
            public string Versione
            {
                get
                {
                    return m_Versione;
                }

                set
                {
                    string oldValue = m_Versione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Versione = value;
                    DoChanged("Versione", value, oldValue);
                }
            }

            /// <summary>
            /// Data di installazione
            /// </summary>
            public DateTime? DataInstallazione
            {
                get
                {
                    return m_DataInstallazione;
                }

                set
                {
                    var oldValue = m_DataInstallazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInstallazione = value;
                    DoChanged("DataInstallazione", value, oldValue);
                }
            }

            /// <summary>
            /// Data di dismissione
            /// </summary>
            public DateTime? DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }

                set
                {
                    var oldValue = m_DataDismissione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataDismissione = value;
                    DoChanged("DataDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new PBXFlags Flags
            {
                get
                {
                    return (PBXFlags) this.m_Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (DMD.RunTime.EQ(oldValue , value))
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is PBX) && this.Equals((PBX)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(PBX obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Versione, obj.m_Versione)
                    && DMD.DateUtils.EQ(this.m_DataInstallazione, obj.m_DataInstallazione)
                    && DMD.DateUtils.EQ(this.m_DataDismissione, obj.m_DataDismissione)
                    ;
            }


            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.PBXs;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficePBX";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_Tipo = reader.Read("Tipo",  m_Tipo);
                m_Versione = reader.Read("Versione",  m_Versione);
                m_DataInstallazione = reader.Read("DataInstallazione",  m_DataInstallazione);
                m_DataDismissione = reader.Read("DataDismissione",  m_DataDismissione);
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
                writer.Write("Tipo", m_Tipo);
                writer.Write("Versione", m_Versione);
                writer.Write("DataInstallazione", m_DataInstallazione);
                writer.Write("DataDismissione", m_DataDismissione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Versione", typeof(string), 255);
                c = table.Fields.Ensure("DataInstallazione", typeof(DateTime), 255);
                c = table.Fields.Ensure("DataDismissione", typeof(DateTime), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxTipo", new string[] { "Tipo", "Versione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInstallazione", "DataDismissione" }, DBFieldConstraintFlags.None);                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Versione", m_Versione);
                writer.WriteAttribute("DataInstallazione", m_DataInstallazione);
                writer.WriteAttribute("DataDismissione", m_DataDismissione);
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

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Versione":
                        {
                            m_Versione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInstallazione":
                        {
                            m_DataInstallazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataDismissione":
                        {
                            m_DataDismissione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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