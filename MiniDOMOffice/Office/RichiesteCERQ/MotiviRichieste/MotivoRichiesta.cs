using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Flag per <see cref="MotivoRichiesta"/>
        /// </summary>
        [Flags]
        public enum MotivoRichiestaFlags
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }

        /// <summary>
        /// Rappresenta un motivo di una commissione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class MotivoRichiesta
            : Databases.DBObject, IComparable, IComparable<MotivoRichiesta>
        {
            private string m_Motivo;                      // Motivo della commissione
            private string m_HandlerName;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotivoRichiesta()
            {
                m_Motivo = DMD.Strings.vbNullString;
                m_Flags = (int)MotivoRichiestaFlags.None;
                m_HandlerName = "";
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
            /// Flags
            /// </summary>
            public new MotivoRichiestaFlags Flags
            {
                get
                {
                    return (MotivoRichiestaFlags) base.Flags;
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
            /// Handler
            /// </summary>
            public string HandlerName
            {
                get
                {
                    return m_HandlerName;
                }

                set
                {
                    string oldValue = m_HandlerName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_HandlerName = value;
                    DoChanged("HandlerName", value, oldValue);
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is MotivoRichiesta) && this.Equals((MotivoRichiesta)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MotivoRichiesta obj)
            {
                return base.Equals(obj)
                      && DMD.Strings.EQ(this.m_Motivo, obj.m_Motivo)
                      && DMD.Strings.EQ(this.m_HandlerName, obj.m_HandlerName)
                    ;

            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(MotivoRichiesta obj)
            {
                return DMD.Strings.Compare(this.m_Motivo, obj.m_Motivo, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((MotivoRichiesta)obj);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.RichiesteCERQ.MotiviRichieste;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeRichiesteM";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Motivo = reader.Read("Motivo", this.m_Motivo);
                this.m_HandlerName = reader.Read("NomeHandler", this.m_HandlerName);
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
                writer.Write("NomeHandler", this.m_HandlerName);
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
                c = table.Fields.Ensure("NomeHandler", typeof(string), 255);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.Ensure("idxMotivo", new string[] { "Motivo", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxHandler", new string[] { "NomeHandler" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Motivo", m_Motivo);
                writer.WriteAttribute("NomeHandler", m_HandlerName);
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
                    case "Motivo":
                        {
                            m_Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 
                    case "NomeHandler":
                        {
                            m_HandlerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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