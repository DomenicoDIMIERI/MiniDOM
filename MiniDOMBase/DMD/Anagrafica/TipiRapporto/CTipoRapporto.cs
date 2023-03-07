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
        /// Flag validi per i tipi rapporto
        /// </summary>
        public enum TipoRapportoFlags : int
        {

            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Il tipo rapporto è valido
            /// </summary>
            Attivo = 1
        }

        /// <summary>
        /// Rappresenta un tipo rapporto lavorativo (es. Pensionato, Privato, ecc..)
        /// </summary>

        [Serializable]
        public class CTipoRapporto
            : Databases.DBObjectBase
        {
            private string m_Descrizione;
            private string m_IdTipoRapporto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTipoRapporto()
            {
                this.m_Descrizione = "";
                this.m_IdTipoRapporto = "";
                this.m_Flags = (int)TipoRapportoFlags.Attivo;                 
            }

            
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.TipiRapporto; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta la descrizione del tipo rapporto
            /// </summary>
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
            /// Restituisce o imposta il nome del tipo rapporto
            /// </summary>
            public string IdTipoRapporto
            {
                get
                {
                    return m_IdTipoRapporto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IdTipoRapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IdTipoRapporto = value;
                    DoChanged("IdTipoRapporto", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta i flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new TipoRapportoFlags Flags
            {
                get
                {
                    return (TipoRapportoFlags)base.m_Flags;
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
            /// Restituisce o imposta un valore che indica se questo oggetto è arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, TipoRapportoFlags.Attivo);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = (int) DMD.RunTime.SetFlag(this.Flags, TipoRapportoFlags.Attivo, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (!string.IsNullOrEmpty(m_IdTipoRapporto))
                {
                    return DMD.Strings.ConcatArray(this.m_Descrizione , " (" , this.m_IdTipoRapporto , ")" );
                }
                else
                {
                    return this.m_Descrizione;
                }
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IdTipoRapporto, this.m_Descrizione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CTipoRapporto) && this.Equals((CTipoRapporto)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CTipoRapporto obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_IdTipoRapporto, obj.m_IdTipoRapporto)
                     && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                     //parameters
                     ;
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
                return "Tiporapporto";
            }


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Descrizione = reader.Read("descrizione", this.m_Descrizione);
                this.m_IdTipoRapporto = reader.Read("IdTipoRapporto", this.m_IdTipoRapporto);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("descrizione", m_Descrizione);
                writer.Write("IdTipoRapporto", m_IdTipoRapporto);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("descrizione", typeof(string), 255);
                c = table.Fields.Ensure("IdTipoRapporto", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDescrizione", new string[] { "descrizione", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "IdTipoRapporto" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);

            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("IdTipoRapporto", m_IdTipoRapporto);
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
                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IdTipoRapporto":
                        {
                            m_IdTipoRapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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