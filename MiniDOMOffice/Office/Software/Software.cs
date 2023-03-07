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
        /// Rappresenta un programma o un'applicazione installata su un Dispositivo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Software 
            : Databases.DBObject
        {
            private string m_Nome;
            private string m_Versione;
            private CCollection<string> m_SupportedOS;
            private string m_IconURL;
            private string m_Autore;
            private DateTime? m_DataPubblicazione;
            private DateTime? m_DataRitiro;
            private string m_Classe;
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public Software()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_Versione = DMD.Strings.vbNullString;
                m_IconURL = DMD.Strings.vbNullString;
                m_SupportedOS = null;
                m_DataPubblicazione = default;
                m_DataRitiro = default;
                m_Autore = DMD.Strings.vbNullString;
                m_Classe = "";
                m_Flags = 0;
            }

            /// <summary>
            /// Restituisce o imposta il nome del prodotto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la versione del prodotto
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta il percorso dell'icona associata al prodotto
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
                    value = Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di pubblicazione del software
            /// </summary>
            /// <returns></returns>
            public DateTime? DataPubblicazione
            {
                get
                {
                    return m_DataPubblicazione;
                }

                set
                {
                    var oldValue = m_DataPubblicazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPubblicazione = value;
                    DoChanged("DataPubblicazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di ritiro (intesa come ultimo giorno disponibile per l'acquisto) del software
            /// </summary>
            /// <returns></returns>
            public DateTime? DataRitiro
            {
                get
                {
                    return m_DataRitiro;
                }

                set
                {
                    var oldValue = m_DataRitiro;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRitiro = value;
                    DoChanged("DataRitiro", value, oldValue);
                }
            }

            /// <summary>
            /// Classe del software
            /// </summary>
            public string Classe
            {
                get
                {
                    return m_Classe;
                }

                set
                {
                    string oldValue = m_Classe;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Classe = value;
                    DoChanged("Class", value, oldValue);
                }
            }

            /// <summary>
            /// Autore del software
            /// </summary>
            public string Autore
            {
                get
                {
                    return m_Autore;
                }

                set
                {
                    string oldValue = m_Autore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Autore = value;
                    DoChanged("Autore", value, oldValue);
                }
            }

            /// <summary>
            /// Sistemi operativi supportati
            /// </summary>
            public CCollection<string> SuppoertedOSs
            {
                get
                {
                    if (m_SupportedOS is null)
                        m_SupportedOS = new CCollection<string>();
                    return m_SupportedOS;
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_Nome, " ", this.m_Versione);
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
                return (obj is Software) && this.Equals((Software)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Software obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Versione, obj.m_Versione)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.Strings.EQ(this.m_Autore, obj.m_Autore)
                    && DMD.DateUtils.EQ(this.m_DataPubblicazione, obj.m_DataPubblicazione)
                    && DMD.DateUtils.EQ(this.m_DataRitiro, obj.m_DataRitiro)
                    && DMD.Strings.EQ(this.m_Classe, obj.m_Classe)
                   // private CCollection<string> m_SupportedOS;
                    ;

            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Softwares;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeSoftware";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_Versione = reader.Read("Versione", m_Versione);
                m_IconURL = reader.Read("IconURL", m_IconURL);
                m_Classe = reader.Read("Classe", m_Classe);
                m_Autore = reader.Read("Autore", m_Autore);
                m_DataPubblicazione = reader.Read("DataPubblicazione", m_DataPubblicazione);
                m_DataRitiro = reader.Read("DataRitiro", m_DataRitiro);
                this.m_SupportedOS = new CCollection<string>();
                string tmp = reader.Read("SupportedOSs", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_SupportedOS.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
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
                writer.Write("Versione", m_Versione);
                writer.Write("IconURL", m_IconURL);
                writer.Write("Classe", m_Classe);
                writer.Write("Autore", m_Autore);
                writer.Write("DataPubblicazione", m_DataPubblicazione);
                writer.Write("DataRitiro", m_DataRitiro);
                writer.Write("SupportedOSs", DMD.XML.Utils.Serializer.Serialize(SuppoertedOSs));
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
                c = table.Fields.Ensure("Versione", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("Classe", typeof(string), 255);
                c = table.Fields.Ensure("Autore", typeof(string), 255);
                c = table.Fields.Ensure("DataPubblicazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataRitiro", typeof(DateTime), 1);
                c = table.Fields.Ensure("SupportedOSs", typeof(string), 0);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Versione", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxTipo", new string[] { "Classe", "Autore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataPubblicazione", "DataRitiro" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParams", new string[] { "IconURL" }, DBFieldConstraintFlags.None);
                c = table.Fields.Ensure("SupportedOSs", typeof(string), 0);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Versione", m_Versione);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("Classe", m_Classe);
                writer.WriteAttribute("Autore", m_Autore);
                writer.WriteAttribute("DataPubblicazione", m_DataPubblicazione);
                writer.WriteAttribute("DataRitiro", m_DataRitiro);
                base.XMLSerialize(writer);
                writer.WriteTag("SupportedOSs", SuppoertedOSs);
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

                    case "Versione":
                        {
                            m_Versione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Classe":
                        {
                            m_Classe = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Autore":
                        {
                            m_Autore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataPubblicazione":
                        {
                            m_DataPubblicazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataRitiro":
                        {
                            m_DataRitiro = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }
 
                    case "SupportedOSs":
                        {
                            m_SupportedOS = new CCollection<string>();
                            m_SupportedOS.AddRange((CCollection)fieldValue);
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