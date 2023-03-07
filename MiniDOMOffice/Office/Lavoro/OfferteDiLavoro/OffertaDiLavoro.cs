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
        /// Rappresenta un'offerta di lavoro
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class OffertaDiLavoro 
            : minidom.Databases.DBObjectPO
        {
            private DateTime? m_DataInserzione;
            private DateTime? m_ValidaDal;
            private DateTime? m_ValidaAl;
            private bool m_Attiva;
            private string m_NomeOfferta;
            private string m_Descrizione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public OffertaDiLavoro()
            {
                m_DataInserzione = default;
                m_ValidaAl = default;
                m_ValidaAl = default;
                m_Attiva = false;
                m_NomeOfferta = "";
                m_Descrizione = "";
            }

            /// <summary>
            /// Data di inserzione
            /// </summary>
            public DateTime? DataInserzione
            {
                get
                {
                    return m_DataInserzione;
                }

                set
                {
                    var oldValue = m_DataInserzione;
                    if (DMD.DateUtils.EQ(value, oldValue))
                        return;
                    m_DataInserzione = value;
                    DoChanged("DataPresentazione", value, oldValue);
                }
            }

            /// <summary>
            /// Valida da 
            /// </summary>
            public DateTime? ValidaDal
            {
                get
                {
                    return m_ValidaDal;
                }

                set
                {
                    var oldValue = m_ValidaDal;
                    if (DMD.DateUtils.EQ(oldValue, value))
                        return;
                    m_ValidaDal = value;
                    DoChanged("ValidaDal", value, oldValue);
                }
            }

            /// <summary>
            /// Valida al
            /// </summary>
            public DateTime? ValidaAl
            {
                get
                {
                    return m_ValidaAl;
                }

                set
                {
                    var oldValue = m_ValidaAl;
                    if (DMD.DateUtils.EQ(oldValue, value))
                        return;
                    m_ValidaAl = value;
                    DoChanged("ValidaAl", value, oldValue);
                }
            }

            /// <summary>
            /// Offerta attiva
            /// </summary>
            public bool Attiva
            {
                get
                {
                    return m_Attiva;
                }

                set
                {
                    if (DMD.Booleans.EQ(value , m_Attiva))
                        return;
                    m_Attiva = value;
                    DoChanged("Attiva", value, !value);
                }
            }

            /// <summary>
            /// Nome dell'offerta
            /// </summary>
            public string NomeOfferta
            {
                get
                {
                    return m_NomeOfferta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeOfferta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOfferta = value;
                    DoChanged("NomeOfferta", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione dell'offerta
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }
            /// <summary>
            /// Restituisce true se l'offerta é valida alla data corrente
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Restituisce true se l'offerta é valida alla data specificata
            /// </summary>
            /// <param name="at"></param>
            /// <returns></returns>
            public bool IsValid(DateTime at)
            {
                return m_Attiva && DMD.DateUtils.CheckBetween(at, m_ValidaDal, m_ValidaAl);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_NomeOfferta;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DataInserzione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is OffertaDiLavoro) && this.Equals((OffertaDiLavoro)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(OffertaDiLavoro obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataInserzione, obj.m_DataInserzione)
                    && DMD.DateUtils.EQ(this.m_ValidaDal, obj.m_ValidaDal)
                    && DMD.DateUtils.EQ(this.m_ValidaAl, obj.m_ValidaAl)
                    && DMD.Booleans.EQ(this.m_Attiva, obj.m_Attiva)
                    && DMD.Strings.EQ(this.m_NomeOfferta, obj.m_NomeOfferta)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.OfferteDiLavoro;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeOfferteLavoro";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_DataInserzione = reader.Read("DataInserzione", this.m_DataInserzione);
                this.m_ValidaDal = reader.Read("ValidaDal", this.m_ValidaDal);
                this.m_ValidaAl = reader.Read("ValidaAl", this.m_ValidaAl);
                this.m_Attiva = reader.Read("Attiva", this.m_Attiva);
                this.m_NomeOfferta = reader.Read("NomeOfferta", this.m_NomeOfferta);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataInserzione", m_DataInserzione);
                writer.Write("ValidaDal", m_ValidaDal);
                writer.Write("ValidaAl", m_ValidaAl);
                writer.Write("Attiva", m_Attiva);
                writer.Write("NomeOfferta", m_NomeOfferta);
                writer.Write("Descrizione", m_Descrizione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataInserzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("ValidaDal", typeof(DateTime), 1);
                c = table.Fields.Ensure("ValidaAl", typeof(DateTime), 1);
                c = table.Fields.Ensure("Attiva", typeof(bool), 1);
                c = table.Fields.Ensure("NomeOfferta", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxData", new string[] { "DataInserzione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValidita", new string[] { "ValidaDal", "ValidaAl" , "Attiva" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNome", new string[] { "NomeOfferta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataInserzione", m_DataInserzione);
                writer.WriteAttribute("ValidaDal", m_ValidaDal);
                writer.WriteAttribute("ValidaAl", m_ValidaAl);
                writer.WriteAttribute("Attiva", m_Attiva);
                writer.WriteAttribute("NomeOfferta", m_NomeOfferta);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
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
                    case "DataInserzione":
                        {
                            m_DataInserzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ValidaDal":
                        {
                            m_ValidaDal = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ValidaAl":
                        {
                            m_ValidaAl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Attiva":
                        {
                            m_Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "NomeOfferta":
                        {
                            m_NomeOfferta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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