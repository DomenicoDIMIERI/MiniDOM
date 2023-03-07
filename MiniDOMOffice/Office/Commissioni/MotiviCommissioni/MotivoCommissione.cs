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
        /// Rappresenta un motivo di una commissione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class MotivoCommissione 
            : minidom.Databases.DBObject, IComparable, IComparable<MotivoCommissione>
        {

            private string m_Motivo;                      // Motivo della commissione

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotivoCommissione()
            {
                m_Motivo = DMD.Strings.vbNullString;
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is MotivoCommissione) && this.Equals((MotivoCommissione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MotivoCommissione obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Motivo, obj.m_Motivo);
            }

            /// <summary>
            /// Restituisce o imposta il motivo della commissione (descrizione breve)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Motivo
            {
                get
                {
                    return m_Motivo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Motivo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Motivo = value;
                    DoChanged("Motivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Motivo;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Motivo);
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                //TODO cambiare il percorso del repository
                return minidom.Office.Commissioni.Motivi;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeCommissioniM";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Motivo = reader.Read("Motivo", this.m_Motivo);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Motivo", this.m_Motivo);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Motivo", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxMotivo", new string[] { "Motivo", "Stato" }, DBFieldConstraintFlags.PrimaryKey); ;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Motivo", m_Motivo);
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
                    case "Motivo": this.m_Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((MotivoCommissione)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual int CompareTo(MotivoCommissione obj)
            {
                return DMD.Strings.Compare(m_Motivo, obj.m_Motivo, true);
            }

           
        }
    }
}