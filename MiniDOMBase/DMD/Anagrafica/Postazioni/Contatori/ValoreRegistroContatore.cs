using System;
using DMD.XML;
using DMD;
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
        /// Definisce una registrazione del valore di un contatore valido per una postazione (es. il numero di copie di una stampante)
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ValoreRegistroContatore 
            : Databases.DBObjectPO
        {
            private int m_IDPostazione;
            [NonSerialized] private CPostazione m_Postazione;
            private string m_NomePostazione;
            private DateTime m_DataRegistrazione;
            private string m_NomeRegistro;
            private double? m_ValoreRegistro;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ValoreRegistroContatore()
            {
                m_IDPostazione = 0;
                m_Postazione = null;
                m_NomePostazione = "";
                m_DataRegistrazione = DMD.DateUtils.Now();
                m_NomeRegistro = "";
                m_ValoreRegistro = default;
            }

            /// <summary>
            /// IDPostazione
            /// </summary>
            public int IDPostazione
            {
                get
                {
                    return DBUtils.GetID(m_Postazione, m_IDPostazione);
                }

                set
                {
                    int oldValue = IDPostazione;
                    if (oldValue == value)
                        return;
                    m_IDPostazione = value;
                    m_Postazione = null;
                    DoChanged("IDPostazione", value, oldValue);
                }
            }

            /// <summary>
            /// Postazione
            /// </summary>
            public CPostazione Postazione
            {
                get
                {
                    if (m_Postazione is null)
                        m_Postazione = Postazioni.GetItemById(m_IDPostazione);
                    return m_Postazione;
                }

                set
                {
                    var oldValue = Postazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Postazione = value;
                    m_IDPostazione = DBUtils.GetID(value, 0);
                    m_NomePostazione = "";
                    if (value is object)
                        m_NomePostazione = value.Nome;
                    DoChanged("Postazione", value, oldValue);
                }
            }

            /// <summary>
            /// NomePostazione
            /// </summary>
            public string NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }

                set
                {
                    string oldValue = m_NomePostazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePostazione = value;
                    DoChanged("NomePostazione", value, oldValue);
                }
            }

            /// <summary>
            /// DataRegistrazione
            /// </summary>
            public DateTime DataRegistrazione
            {
                get
                {
                    return m_DataRegistrazione;
                }

                set
                {
                    var oldValue = m_DataRegistrazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRegistrazione = value;
                    DoChanged("DataRegistrazione", value, oldValue);
                }
            }

            /// <summary>
            /// NomeRegistro
            /// </summary>
            public string NomeRegistro
            {
                get
                {
                    return m_NomeRegistro;
                }

                set
                {
                    string oldValue = m_NomeRegistro;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRegistro = value;
                    DoChanged("NomeRegistro", value, oldValue);
                }
            }

            /// <summary>
            /// ValoreRegistro
            /// </summary>
            public double? ValoreRegistro
            {
                get
                {
                    return m_ValoreRegistro;
                }

                set
                {
                    var oldValue = m_ValoreRegistro;
                    if (oldValue == value == true)
                        return;
                    m_ValoreRegistro = value;
                    DoChanged("ValoreRegistro", value, oldValue);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDPostazione = reader.Read("IDPostazione", this. m_IDPostazione);
                this.m_NomePostazione = reader.Read("NomePostazione", this.m_NomePostazione);
                this.m_DataRegistrazione = reader.Read("DataRegistrazione", this.m_DataRegistrazione);
                this.m_NomeRegistro = reader.Read("NomeRegistro", this.m_NomeRegistro);
                this.m_ValoreRegistro = reader.Read("ValoreRegistro", this.m_ValoreRegistro);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPostazione", IDPostazione);
                writer.Write("NomePostazione", m_NomePostazione);
                writer.Write("DataRegistrazione", m_DataRegistrazione);
                writer.Write("NomeRegistro", m_NomeRegistro);
                writer.Write("ValoreRegistro", m_ValoreRegistro);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPostazione", typeof(int), 1);
                c = table.Fields.Ensure("NomePostazione", typeof(string), 255);
                c = table.Fields.Ensure("DataRegistrazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("NomeRegistro", typeof(string), 255);
                c = table.Fields.Ensure("ValoreRegistro", typeof(double), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPK", new string[] { "IDPostazione", "DataRegistrazione", "NomeRegistro" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxNomi", new string[] { "NomePostazione", "ValoreRegistro", }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPostazione", IDPostazione);
                writer.WriteAttribute("NomePostazione", m_NomePostazione);
                writer.WriteAttribute("DataRegistrazione", m_DataRegistrazione);
                writer.WriteAttribute("NomeRegistro", m_NomeRegistro);
                writer.WriteAttribute("ValoreRegistro", m_ValoreRegistro);
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
                    case "IDPostazione":
                        {
                            m_IDPostazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePostazione":
                        {
                            m_NomePostazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRegistrazione":
                        {
                            m_DataRegistrazione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NomeRegistro":
                        {
                            m_NomeRegistro = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreRegistro":
                        {
                            m_ValoreRegistro = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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
                return
                    DMD.Strings.ConcatArray("{ " , m_NomePostazione , ", " , m_NomeRegistro , ", " , m_DataRegistrazione, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomePostazione, this.m_NomeRegistro, this.m_DataRegistrazione);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Postazioni.ValoriRegistri;
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
                return "tbl_PostazoniRegistri";
            }
        }
    }
}