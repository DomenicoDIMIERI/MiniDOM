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
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class CustomerCalls
    {
        /// <summary>
        /// Flag validi per i template dei contatti utente
        /// </summary>
        [Flags]
        public enum TemplateFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Template attivo
            /// </summary>
            Attivo = 1,

            /// <summary>
            /// Template utilizzabile per i contatti in ingresso
            /// </summary>
            UsabilePerRicevuti = 2,

            /// <summary>
            /// Template utilizzabile per i contatti in uscita
            /// </summary>
            UsabilePerEffettuati = 4
        }

        /// <summary>
        /// Rappresenta un modello di messaggio o di sms che viene caricato automaticamente quando si seleziona uno scopo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CContattoUtenteTemplate 
            : minidom.Databases.DBObject
        {
            //private TemplateFlags m_Flags;
            private string m_Nome;
            private string m_Testo;
            private string m_Scopo;
            private string m_TipoContatto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoUtenteTemplate()
            {
                this.m_Flags = (int) ( TemplateFlags.Attivo | TemplateFlags.UsabilePerEffettuati | TemplateFlags.UsabilePerRicevuti );
                this.m_Nome = "";
                this.m_Testo = "";
                this.m_Scopo = "";
                this.m_TipoContatto = "";
            }

            /// <summary>
            /// Restituisce il nome del template
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash del template
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Se vero i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CContattoUtenteTemplate) && this.Equals((CContattoUtenteTemplate)obj);
            }

            /// <summary>
            /// Se vero i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CContattoUtenteTemplate obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_Testo, obj.m_Testo)
                     && DMD.Strings.EQ(this.m_Scopo, obj.m_Scopo)
                     && DMD.Strings.EQ(this.m_TipoContatto, obj.m_TipoContatto)
                        ;
            }


            /// <summary>
            /// Se vero indica che il template é attivo
            /// </summary>
            public bool Attivo
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, TemplateFlags.Attivo);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, TemplateFlags.Attivo, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Se vero incia che il template é utilizzabile per i contatti in ingresso
            /// </summary>
            public bool UsabilePerRicevuti
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, TemplateFlags.UsabilePerRicevuti);
                }

                set
                {
                    if (UsabilePerRicevuti == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, TemplateFlags.UsabilePerRicevuti, value);
                    DoChanged("UsabilePerRicevuti", value, !value);
                }
            }

            /// <summary>
            /// Se vero indica che il template é utilizzabile per i contatti in uscita
            /// </summary>
            public bool UsabilePerEffettuati
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, TemplateFlags.UsabilePerEffettuati);
                }

                set
                {
                    if (UsabilePerEffettuati == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, TemplateFlags.UsabilePerEffettuati, value);
                    DoChanged("UsabilePerEffettuati", value, !value);
                }
            }

            /// <summary>
            /// Restituisce
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new TemplateFlags Flags
            {
                get
                {
                    return (TemplateFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del template
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
                    string oldValue = m_Nome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il testo del modello
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Testo
            {
                get
                {
                    return m_Testo;
                }

                set
                {
                    string oldValue = m_Testo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Testo = value;
                    DoChanged("Testo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo scopo a cui si applica il modello
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Scopo
            {
                get
                {
                    return m_Scopo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Scopo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Scopo = value;
                    DoChanged("Scopo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo contatto a cui si applica il modello
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoContatto
            {
                get
                {
                    return m_TipoContatto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContatto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContatto = value;
                    DoChanged("TipoContatto", value, oldValue);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CRMTemplates";
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.Templates;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_Testo = reader.Read("Testo", m_Testo);
                m_Scopo = reader.Read("Scopo", m_Scopo);
                m_TipoContatto = reader.Read("TipoContatto", m_TipoContatto);
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
                writer.Write("Testo", m_Testo);
                writer.Write("Scopo", m_Scopo);
                writer.Write("TipoContatto", m_TipoContatto);
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
                c = table.Fields.Ensure("Testo", typeof(string), 0);
                c = table.Fields.Ensure("Scopo", typeof(string), 255);
                c = table.Fields.Ensure("TipoContatto", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxScopo", new string[] { "TipoContatto", "Scopo" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxTesto", new string[] { "Testo" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Scopo", m_Scopo);
                writer.WriteAttribute("TipoContatto", m_TipoContatto);
                base.XMLSerialize(writer);
                writer.WriteTag("Testo", m_Testo);
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

                    case "Scopo":
                        {
                            m_Scopo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoContatto":
                        {
                            m_TipoContatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Testo":
                        {
                            m_Testo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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