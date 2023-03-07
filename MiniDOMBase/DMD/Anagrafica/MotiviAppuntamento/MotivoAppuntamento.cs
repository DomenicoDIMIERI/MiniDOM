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
        /// Flags per un Motivo appuntamento
        /// </summary>
        [Flags]
        public enum MotivoAppuntamentoFlags : int
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
            PersoneGiuridiche = 4
        }

        /// <summary>
        /// Rappresenta un elemento nel menu di scelta a tendina per lo scopo degli appuntamenti
        /// </summary>
        [Serializable]
        public class MotivoAppuntamento 
            : Databases.DBObject
        {
            private string m_Nome;
            private string m_Descrizione;
            private string m_TipoAppuntamento;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public MotivoAppuntamento()
            {
                this.m_Nome = "";
                this.m_Descrizione = "";
                this.m_Flags = (int)MotivoAppuntamentoFlags.Attivo;
                this.m_TipoAppuntamento = "";
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
            public string TipoAppuntamento
            {
                get
                {
                    return m_TipoAppuntamento;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoAppuntamento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoAppuntamento = value;
                    DoChanged("TipoAppuntamento", value, oldValue);
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new MotivoAppuntamentoFlags Flags
            {
                get
                {
                    return (MotivoAppuntamentoFlags) this.m_Flags;
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
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is MotivoAppuntamento) && this.Equals((MotivoAppuntamento)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MotivoAppuntamento obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                     && DMD.Strings.EQ(this.m_TipoAppuntamento, obj.m_TipoAppuntamento)
                    ;
            //private CKeyCollection m_Parameters;
            }



            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.MotiviAppuntamento; //.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CRMMotiviAppuntamento";
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
                this.m_TipoAppuntamento = reader.Read("TipoAppuntamento", this.m_TipoAppuntamento);
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
                writer.Write("TipoAppuntamento", m_TipoAppuntamento);
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
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("TipoAppuntamento", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTipo", new string[] { "TipoAppuntamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("TipoAppuntamento", m_TipoAppuntamento);
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

                    case "TipoAppuntamento":
                        {
                            m_TipoAppuntamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            //    MotiviAppuntamento.UpdateCached(this);
            //}

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}
        }
    }
}