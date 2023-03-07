using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Forma giuridica azienda (Srl, Spa, ...)
        /// </summary>
        [Serializable]
        public class CFormaGiuridicaAzienda
            : Databases.DBObject
        {
            private string m_Nome;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CFormaGiuridicaAzienda()
            {
                this.m_Nome = "";
            }

            /// <summary>
            /// Restituisce o imposta il nome della forma giuridica (es. Srl)
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
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
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
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CFormaGiuridicaAzienda) && this.Equals((CFormaGiuridicaAzienda)obj);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CFormaGiuridicaAzienda obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                        ;
            //private CKeyCollection m_Parameters;
            }


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.FormeGiuridicheAzienda; //.Module;
            }

            /// <summary>
            /// Discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_FormeGiuridicheAzienda";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return minidom.Databases.APPConn;
            //}


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", this.m_Nome);
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
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.Unique);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
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