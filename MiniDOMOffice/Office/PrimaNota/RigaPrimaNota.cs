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
        /// Rappresenta la registrazione di una voce di entrata/uscita nella prima nota
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RigaPrimaNota 
            : Databases.DBObjectPO, IComparable, IComparable<RigaPrimaNota>
        {
            private string m_DescrizioneMovimento;
            private DateTime m_Data;
            private decimal? m_Entrate;
            private decimal? m_Uscite;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RigaPrimaNota()
            {
                m_DescrizioneMovimento = DMD.Strings.vbNullString;
                m_Data = default;
                m_Entrate = default;
                m_Uscite = default;
            }

            /// <summary>
            /// Data della registrazione
            /// </summary>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione del movimento
            /// </summary>
            public string DescrizioneMovimento
            {
                get
                {
                    return m_DescrizioneMovimento;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_DescrizioneMovimento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneMovimento = value;
                    DoChanged("DescrizioneMovimento", value, oldValue);
                }
            }

            /// <summary>
            /// Entrate
            /// </summary>
            public decimal? Entrate
            {
                get
                {
                    return m_Entrate;
                }

                set
                {
                    var oldValue = m_Entrate;
                    if (oldValue.Equals(value))
                        return;
                    m_Entrate = value;
                    DoChanged("Entrate", value, oldValue);
                }
            }

            /// <summary>
            /// Uscite
            /// </summary>
            public decimal? Uscite
            {
                get
                {
                    return m_Uscite;
                }

                set
                {
                    var oldValue = m_Uscite;
                    if (oldValue.Equals(value))
                        return;
                    m_Uscite = value;
                    DoChanged("Uscite", value, oldValue);
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.PrimaNota;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficePrimaNota";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Data = reader.Read("Data", this.m_Data);
                this.m_DescrizioneMovimento = reader.Read("DescrizioneMovimento", this.m_DescrizioneMovimento);
                this.m_Entrate = reader.Read("Entrate", this.m_Entrate);
                this.m_Uscite = reader.Read("Uscite", this.m_Uscite);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Data", m_Data);
                writer.Write("DescrizioneMovimento", m_DescrizioneMovimento);
                writer.Write("Entrate", m_Entrate);
                writer.Write("Uscite", m_Uscite);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("DescrizioneMovimento", typeof(string), 255);
                c = table.Fields.Ensure("Entrate", typeof(decimal), 1);
                c = table.Fields.Ensure("Uscite", typeof(decimal), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.Ensure("idxData", new string[] { "Data" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "DescrizioneMovimento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValute", new string[] { "Entrate", "Uscite" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("DescrizioneMovimento", m_DescrizioneMovimento);
                writer.WriteAttribute("Entrate", m_Entrate);
                writer.WriteAttribute("Uscite", m_Uscite);
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
                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DescrizioneMovimento":
                        {
                            m_DescrizioneMovimento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Entrate":
                        {
                            m_Entrate = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Uscite":
                        {
                            m_Uscite = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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
                return DMD.Strings.ConcatArray(DateUtils.FormatDateYYYYMMDD(this.Data), " - " , this.NomePuntoOperativo , ": IN: ", Decimals.Format(this.m_Entrate ), " OUT: ", Decimals.Format(this.m_Uscite));
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Data);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is RigaPrimaNota) && this.Equals((RigaPrimaNota)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RigaPrimaNota obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_DescrizioneMovimento, obj.m_DescrizioneMovimento)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.Decimals.EQ(this.m_Entrate, obj.m_Entrate)
                    && DMD.Decimals.EQ(this.m_Uscite, obj.m_Uscite)
                    ;
            }

            /// <summary>
            /// Compara due oggetti per punto operativo e per data
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(RigaPrimaNota obj)
            {
                var ret = Strings.Compare(this.NomePuntoOperativo, obj.NomePuntoOperativo, true);
                if (ret == 0) ret = DateTime.Compare(this.m_Data, obj.m_Data);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((RigaPrimaNota)obj);
            }
            
        }
    }
}