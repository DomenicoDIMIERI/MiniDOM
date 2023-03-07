using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta una tipologia aziendale
        /// </summary>
        [Serializable]
        public class CTipologiaAzienda
            : Databases.DBObject
        {
            private string m_Nome;
            private bool m_RichiedeValutazione;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTipologiaAzienda()
            {
                this.m_Nome = "";
                this.m_RichiedeValutazione = false;
                 
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Se true indica che il tipo di azienda richiede la validazione
            /// </summary>
            public bool RichiedeValutazione
            {
                get
                {
                    return m_RichiedeValutazione;
                }

                set
                {
                    if (m_RichiedeValutazione == value)
                        return;
                    m_RichiedeValutazione = value;
                    DoChanged("RichiedeValutazione", value, !value);
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
                return (obj is CTipologiaAzienda) && this.Equals((CTipologiaAzienda)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CTipologiaAzienda  obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Booleans.EQ(this.m_RichiedeValutazione, obj.m_RichiedeValutazione)
                    //&& this.m_Parameters
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.TipologieAzienda; //.Module;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_TipologieAzienda";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this. m_Nome);
                this.m_RichiedeValutazione = reader.Read("RichVal", this.m_RichiedeValutazione);
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
                writer.Write("RichVal", m_RichiedeValutazione);
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
                c = table.Fields.Ensure("RichVal", typeof(bool), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "RichVal", "Flags" }, DBFieldConstraintFlags.None);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("RichVal", m_RichiedeValutazione);
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

                    case "RichVal":
                        {
                            m_RichiedeValutazione = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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