using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {



        /// <summary>
        /// Rappresenta il canale usato per comunicare un contatto (diverso dalla fonte in quanto la fonte rappresenta il modo in cui la persona ci ha conosciuti, mentre il canale rappresenta chi ci ha comunicato il contatto)
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCanale 
            : Databases.DBObject, ICloneable
        {
            private string m_Nome;
            private string m_Tipo;
            private string m_IconURL;
            private bool m_Valid;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCanale()
            {
                this.m_Nome = DMD.Strings.vbNullString;
                this.m_Tipo = DMD.Strings.vbNullString;
                this.m_IconURL = DMD.Strings.vbNullString;
                this.m_Valid = true;
            }

            /// <summary>
            /// Restituisce o imposta il nome della fonte
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
            /// Restituisce o imposta il percorso dell'immagine utilizzata come icona per la fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce o imposta il tipo della fonte (Radio, TV, Cartaceo, ecc)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il canale è valido
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Valido
            {
                get
                {
                    return m_Valid;
                }

                set
                {
                    if (m_Valid == value)
                        return;
                    m_Valid = value;
                    DoChanged("Valid", value, !value);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray("{ ", this.m_Tipo , " : ", this.m_Nome , " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Tipo, this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CCanale) && this.Equals((CCanale)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CCanale obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                     && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                     && DMD.Booleans.EQ(this.m_Valid, obj.m_Valid)
                    ;

            }

            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Canali; //.Module;
            }

            /// <summary>
            /// Restituisce il nome del discrimante nel repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ANACanali";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Tipo = reader.Read("Tipo", this.m_Tipo);
                this.m_Valid = reader.Read("Valid", this.m_Valid);
                this.m_IconURL = reader.Read("IconURL", this.m_IconURL);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salve nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", this.m_Nome);
                writer.Write("Tipo", this.m_Tipo);
                writer.Write("Valid", this.m_Valid);
                writer.Write("IconURL", this.m_IconURL);
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
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Valid", typeof(bool), 0);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
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
                c = table.Constraints.Ensure("idxNome", new string[] { "Tipo", "Nome", "Stato" }, DBFieldConstraintFlags.Unique);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", this.m_Nome);
                writer.WriteAttribute("Tipo", this.m_Tipo);
                writer.WriteAttribute("Valid", this.m_Valid);
                writer.WriteAttribute("IconURL", this.m_IconURL);
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

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valid":
                        {
                            m_Valid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una copia esatta dell'oggetto
            /// </summary>
            /// <returns></returns>
            public CCanale Clone()
            {
                return (CCanale) this.MemberwiseClone();
            }
             
            object ICloneable.Clone()
            {
                return this.Clone();
            }
        }
    }
}