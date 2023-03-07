using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flags per un Motivo Contatto
        /// </summary>
        [Flags]
        public enum MotivoContattoFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Attivo
            /// </summary>
            Attivo = 1,


            /// <summary>
            /// Valido per le persone fisiche
            /// </summary>
            PersoneFisiche = 2,

            /// <summary>
            /// Valido per le persone giuridiche
            /// </summary>
            PersoneGiuridiche = 4,

            /// <summary>
            /// Lo scopo é valido per i contatti in uscita
            /// </summary>
            InUscita = 8,

            /// <summary>
            /// Lo scopo é valido per i contatti in ingresso
            /// </summary>
            InEntrata = 16
        }

        /// <summary>
        /// Rappresenta un elemento nel menu di scelta a tendina per lo scopo dei contatti (telefonate, visite, ecc..)
        /// </summary>
        [Serializable]
        public class MotivoContatto 
            : Databases.DBObject
        {
            private string m_Nome;
            private string m_Descrizione;
            private string m_TipoContatto;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public MotivoContatto()
            {
                this.m_Nome = "";
                this.m_Descrizione = "";
                this.m_Flags = (int) MotivoContattoFlags.Attivo;
                this.m_TipoContatto = "";
                 
            }

             
            /// <summary>
            /// Restituisce o imposta il nome della lista
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione della lista
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del proprietario
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
            /// Flags
            /// </summary>
            public new MotivoContattoFlags Flags
            {
                get
                {
                    return (MotivoContattoFlags) this.m_Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    this.m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
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
                return (obj is MotivoContatto) && this.Equals((MotivoContatto)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MotivoContatto obj)
            {
                return  base.Equals(obj)
                        && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                        && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                        && DMD.Strings.EQ(this.m_TipoContatto, obj.m_TipoContatto)
                        ;
                //TODO
            //private CKeyCollection m_Parameters;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.MotiviContatto; //.Module;
            }


            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CRMMotiviContatto";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                this.m_TipoContatto = reader.Read("TipoContatto", this.m_TipoContatto);
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
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("TipoContatto", m_TipoContatto);
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
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("TipoContatto", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "TipoContatto", "Nome" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                //TODO
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("TipoContatto", m_TipoContatto);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldNome"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldNome, object fieldValue)
            {
                switch (fieldNome ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                         
                    case "TipoContatto":
                        {
                            m_TipoContatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    

                    default:
                        {
                            base.SetFieldInternal(fieldNome, fieldValue);
                            break;
                        }
                }
            }

            //protected override void OnAfterSave(SystemEvent e)
            //{
            //    base.OnAfterSave(e);
            //    MotiviContatto.UpdateCached(this);
            //}

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}
        }
    }
}