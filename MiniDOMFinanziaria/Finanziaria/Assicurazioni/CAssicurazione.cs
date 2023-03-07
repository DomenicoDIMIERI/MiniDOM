using System;
using DMD;
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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Agenzia assicurativa
        /// </summary>

        [Serializable]
        public class CAssicurazione 
            : Databases.DBObject 
        {
            private string m_Nome;
            private string m_descrizione;
            private int m_mesescattoeta;
            private int m_mesescattoanzianita;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAssicurazione()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_descrizione = DMD.Strings.vbNullString;
                m_mesescattoeta = 0;
                m_mesescattoanzianita = 0;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Assicurazioni;
            }

            /// <summary>
            /// Nome univoco dell'oggetto
            /// </summary>
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
            /// Descrizione
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_descrizione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Mese in cui l'assicurazione considera lo scatto di età (rispetto alla data di sottoscrizione)
            /// </summary>
            public int MeseScattoEta
            {
                get
                {
                    return m_mesescattoeta;
                }

                set
                {
                    int oldValue = m_mesescattoeta;
                    if (oldValue == value)
                        return;
                    m_mesescattoeta = value;
                    DoChanged("MeseScattoEta", value, oldValue);
                }
            }

            /// <summary>
            /// Mese scatto anzianità 
            /// </summary>
            public int MeseScattoAnzianita
            {
                get
                {
                    return m_mesescattoanzianita;
                }

                set
                {
                    int oldValue = m_mesescattoanzianita;
                    if (oldValue == value)
                        return;
                    m_mesescattoanzianita = value;
                    DoChanged("MeseScattoAnzianita", value, oldValue);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Assicurazioni";
            }

            //protected override Databases.CDBConnection GetConnection()
            //{
            //    return Database;
            //}


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_descrizione = reader.Read("Descrizione", m_descrizione);
                m_mesescattoeta = reader.Read("MeseScattoEta", m_mesescattoeta);
                m_mesescattoanzianita = reader.Read("MeseScattoAnzianita", m_mesescattoanzianita);
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
                writer.Write("Descrizione", m_descrizione);
                writer.Write("MeseScattoEta", m_mesescattoeta);
                writer.Write("MeseScattoAnzianita", m_mesescattoanzianita);
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
                c = table.Fields.Ensure("MeseScattoEta", typeof(int), 1);
                c = table.Fields.Ensure("MeseScattoAnzianita", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxParams", new string[] { "MeseScattoEta" , "MeseScattoAnzianita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("MeseScattoEta", m_mesescattoeta);
                writer.WriteAttribute("MeseScattoAnzianita", m_mesescattoanzianita);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_descrizione);
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

                    case "Descrizione":
                        {
                            m_descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MeseScattoEta":
                        {
                            m_mesescattoeta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MeseScattoAnzianita":
                        {
                            m_mesescattoanzianita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return HashCalculator(this.m_Nome);
            }



            /// <summary>
            /// Restituisce una copia dell'oggetto
            /// </summary>
            /// <returns></returns>
            public CAssicurazione Clone()
            {
                return (CAssicurazione)base._Clone();
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CAssicurazione) && this.Equals((CAssicurazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CAssicurazione obj)
            {
                return base.Equals(obj)
                        && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                        && DMD.Strings.EQ(this.m_descrizione, obj.m_descrizione)
                        && DMD.Integers.EQ(this.m_mesescattoeta, obj.m_mesescattoeta)
                        && DMD.Integers.EQ(this.m_mesescattoanzianita, obj.m_mesescattoanzianita)
                        ;
            }
        }
    }
}