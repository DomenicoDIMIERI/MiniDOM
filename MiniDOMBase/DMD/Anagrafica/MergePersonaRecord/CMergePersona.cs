using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Informazioni sull'unione tra due persone
        /// </summary>
        [Serializable]
        public class CMergePersona 
            : Databases.DBObject
        {
            private int m_IDPersona1;
            [NonSerialized] private CPersona m_Persona1;
            private string m_NomePersona1;
            private int m_IDPersona2;
            [NonSerialized] private CPersona m_Persona2;
            private string m_NomePersona2;
            private DateTime m_DataOperazione;
            private int m_IDOperatore;
            [NonSerialized] private Sistema.CUser m_Operatore;
            private string m_NomeOperatore;
            private CCollection<CMergePersonaRecord> m_TabelleModificate;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CMergePersona()
            {
                m_IDPersona1 = 0;
                m_Persona1 = null;
                m_NomePersona1 = "";
                m_IDPersona2 = 0;
                m_Persona2 = null;
                m_NomePersona2 = "";
                m_DataOperazione = default;
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_TabelleModificate = null;
            }

            


            /// <summary>
            /// ID della persona principale
            /// </summary>
            public int IDPersona1
            {
                get
                {
                    return DBUtils.GetID(m_Persona1, m_IDPersona1);
                }

                set
                {
                    int oldValue = IDPersona1;
                    if (oldValue == value)
                        return;
                    m_IDPersona1 = value;
                    m_Persona1 = null;
                    DoChanged("IDPersona1", value, oldValue);
                }
            }

            /// <summary>
            /// Persona principale
            /// </summary>
            public CPersona Persona1
            {
                get
                {
                    if (m_Persona1 is null)
                        m_Persona1 = Persone.GetItemById(m_IDPersona1);
                    return m_Persona1;
                }

                set
                {
                    var oldValue = m_Persona1;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona1 = value;
                    m_IDPersona1 = DBUtils.GetID(value, 0);
                    m_NomePersona1 = "";
                    if (value is object)
                        m_NomePersona1 = value.Nominativo;
                    DoChanged("NomePersona1", value, oldValue);
                }
            }

            /// <summary>
            /// Nome della persona principale
            /// </summary>
            public string NomePersona1
            {
                get
                {
                    return m_NomePersona1;
                }

                set
                {
                    string oldValue = m_NomePersona1;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona1 = value;
                    DoChanged("NomePersona1", value, oldValue);
                }
            }

            /// <summary>
            /// ID della persona secondaria (eliminata dopo l'unione)
            /// </summary>
            public int IDPersona2
            {
                get
                {
                    return DBUtils.GetID(m_Persona2, m_IDPersona2);
                }

                set
                {
                    int oldValue = IDPersona2;
                    if (oldValue == value)
                        return;
                    m_IDPersona2 = value;
                    m_Persona2 = null;
                    DoChanged("IDPersona2", value, oldValue);
                }
            }

            /// <summary>
            /// Persona secondaria (eliminata dopo l'unione)
            /// </summary>
            public CPersona Persona2
            {
                get
                {
                    if (m_Persona2 is null)
                        m_Persona2 = Persone.GetItemById(m_IDPersona2);
                    return m_Persona2;
                }

                set
                {
                    var oldValue = m_Persona2;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona2 = value;
                    m_IDPersona2 = DBUtils.GetID(value, 0);
                    m_NomePersona2 = "";
                    if (value is object)
                        m_NomePersona2 = value.Nominativo;
                    DoChanged("NomePersona2", value, oldValue);
                }
            }

            /// <summary>
            /// Nome della persona secondaria (eliminata dopo l'unione)
            /// </summary>
            public string NomePersona2
            {
                get
                {
                    return m_NomePersona2;
                }

                set
                {
                    string oldValue = m_NomePersona2;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona2 = value;
                    DoChanged("NomePersona2", value, oldValue);
                }
            }

            /// <summary>
            /// Data dell'unione
            /// </summary>
            public DateTime DataOperazione
            {
                get
                {
                    return m_DataOperazione;
                }

                set
                {
                    var oldValue = m_DataOperazione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataOperazione = value;
                    DoChanged("DataOperazione", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'operatore che ha effettuato l'unione
            /// </summary>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Operatore che ha effettuato l'unione
            /// </summary>
            public Sistema.CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    m_Operatore = value;
                    m_NomeOperatore = "";
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'operatore che ha effettuato l'unione
            /// </summary>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    string oldValue = m_NomeOperatore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Informazioni sugli oggetti modificati nelle varie tabelle
            /// </summary>
            public CCollection<CMergePersonaRecord> TabelleModificate
            {
                get
                {
                    if (m_TabelleModificate is null)
                        m_TabelleModificate = new CCollection<CMergePersonaRecord>();
                    return m_TabelleModificate;
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(
                            "Unione tra " , 
                            m_NomePersona1 , "[" , m_IDPersona1 , "] e " ,
                            m_NomePersona2 + "[" + m_IDPersona2 + "]"
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.IDPersona1, this.IDPersona2, this.m_NomePersona1, this.m_NomePersona2, this.m_DataOperazione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CMergePersona) && this.Equals((CMergePersona)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CMergePersona obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.IDPersona1, obj.IDPersona1)
                     && DMD.Strings.EQ(this.m_NomePersona1, obj.m_NomePersona1)
                     && DMD.Integers.EQ(this.IDPersona2, obj.IDPersona2)
                     && DMD.Strings.EQ(this.m_NomePersona2, obj.m_NomePersona2)
                    && DMD.Integers.EQ(this.IDOperatore, obj.IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.DateUtils.EQ(this.m_DataOperazione, obj.m_DataOperazione)
                    ;
             //private CCollection<CMergePersonaRecord> m_TabelleModificate;
        }



            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.MergePersone; //.Module;
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_MergePersone";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPersona1 = reader.Read("IDPersona1", m_IDPersona1);
                m_NomePersona1 = reader.Read("NomePersona1", m_NomePersona1);
                m_IDPersona2 = reader.Read("IDPersona2", m_IDPersona2);
                m_NomePersona2 = reader.Read("NomePersona2", m_NomePersona2);
                m_DataOperazione = reader.Read("DataOperazione", m_DataOperazione);
                m_IDOperatore = reader.Read("IDOperatore", m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore", m_NomeOperatore);
                string tmp = reader.Read("Tabelle", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_TabelleModificate = new CCollection<CMergePersonaRecord>();
                    m_TabelleModificate.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                }
                

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPersona1", IDPersona1);
                writer.Write("NomePersona1", m_NomePersona1);
                writer.Write("IDPersona2", IDPersona2);
                writer.Write("NomePersona2", m_NomePersona2);
                writer.Write("DataOperazione", m_DataOperazione);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("Tabelle", DMD.XML.Utils.Serialize(this.TabelleModificate));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPersona1", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona1", typeof(string), 255);
                c = table.Fields.Ensure("IDPersona2", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona2", typeof(string), 255);
                c = table.Fields.Ensure("DataOperazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("Tabelle", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPersona1", new string[] { "IDPersona1", "NomePersona1"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona2", new string[] { "IDPersona2", "NomePersona2" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataOperazione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona1", IDPersona1);
                writer.WriteAttribute("NomePersona1", m_NomePersona1);
                writer.WriteAttribute("IDPersona2", IDPersona2);
                writer.WriteAttribute("NomePersona2", m_NomePersona2);
                writer.WriteAttribute("DataOperazione", m_DataOperazione);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                base.XMLSerialize(writer);
                writer.WriteTag("Tabelle", this.TabelleModificate);
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
                    case "IDPersona1":
                        {
                            m_IDPersona1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona1":
                        {
                            m_NomePersona1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersona2":
                        {
                            m_IDPersona2 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona2":
                        {
                            m_NomePersona2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataOperazione":
                        {
                            m_DataOperazione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tabelle":
                        {
                            m_TabelleModificate = new CCollection<CMergePersonaRecord>();
                            m_TabelleModificate.AddRange((IEnumerable)fieldValue);
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
            /// Restituisce gli ID dei record della tabella che sono stati modificati in seguito all'unione
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            public int[] GetAffectedRecorsIDs(string tableName, string fieldName)
            {
                var ret = new List<int>();
                foreach (var rec in TabelleModificate)
                {
                    if ((rec.NomeTabella ?? "") == (tableName ?? "") && rec.RecordID != 0 && (rec.FieldName ?? "") == (fieldName ?? ""))
                    {
                        ret.Add(rec.RecordID);
                    }
                }

                return ret.ToArray();
            }

            ///// <summary>
            ///// Restituisce i record 
            ///// </summary>
            ///// <param name="tableName"></param>
            ///// <param name="fieldName"></param>
            ///// <returns></returns>
            //public string GetAffectedRecors(string tableName, string fieldName)
            //{
            //    var ret = new System.Text.StringBuilder();
            //    foreach (CMergePersonaRecord rec in TabelleModificate)
            //    {
            //        if ((rec.NomeTabella ?? "") == (tableName ?? "") && rec.RecordID != 0 && (rec.FieldName ?? "") == (fieldName ?? ""))
            //        {
            //            if (ret.Length > 0)
            //                ret.Append(",");
            //            ret.Append(rec.RecordID);
            //        }
            //    }

            //    return ret.ToString();
            //}

            /// <summary>
            /// Aggiunge il record modificato in seguito all'unione
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="fieldName"></param>
            /// <param name="recordID"></param>
            /// <returns></returns>
            public CMergePersonaRecord Add(string tableName, string fieldName, int recordID)
            {
                var rec = new CMergePersonaRecord();
                rec.NomeTabella = DMD.Strings.Trim(tableName);
                rec.FieldName = DMD.Strings.Trim(fieldName);
                rec.RecordID = recordID;
                this.TabelleModificate.Add(rec);
                SetChanged(true);
                return rec;
            }
        }
    }
}