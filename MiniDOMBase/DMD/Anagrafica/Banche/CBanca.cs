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
        /// Rappresenta una filiale di una banca
        /// </summary>
        [Serializable]
        public class CBanca 
            : Databases.DBObjectPO
        {
            private string m_Descrizione;
            private string m_Filiale;
            private CIndirizzo m_Indirizzo;
            private string m_ABI;
            private string m_CAB;
            private string m_SWIFT;
            private DateTime? m_DataApertura;
            private DateTime? m_DataChiusura;
            private bool m_Attiva;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBanca()
            {
                this.m_Descrizione = "";
                this.m_Filiale = "";
                this.m_Indirizzo = new CIndirizzo();
                this.m_ABI = "";
                this.m_CAB = "";
                this.m_SWIFT = "";
                this.m_DataApertura = default;
                this.m_DataChiusura = default;
                this.m_Attiva = true;
            }

              
            /// <summary>
            /// Restituisce o imposta il nome dell'istututo bancario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta il nome della filiale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Filiale
            {
                get
                {
                    return m_Filiale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Filiale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Filiale = value;
                    DoChanged("Filiale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo della filiale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIndirizzo Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice dell'istituto bancario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ABI
            {
                get
                {
                    return m_ABI;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ABI;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ABI = value;
                    DoChanged("ABI", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice della filiale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CAB
            {
                get
                {
                    return m_CAB;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CAB;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CAB = value;
                    DoChanged("CAB", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice SWIFT
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SWIFT
            {
                get
                {
                    return m_SWIFT;
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Replace(value, " ", ""), 11);
                    string oldValue = m_SWIFT;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_SWIFT = value;
                    DoChanged("SWIFT", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di apertura della banca
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataApertura
            {
                get
                {
                    return m_DataApertura;
                }

                set
                {
                    var oldValue = m_DataApertura;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataApertura = value;
                    DoChanged("DataApertura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di chiusura della filiale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }

                set
                {
                    var oldValue = m_DataChiusura;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataChiusura = value;
                    DoChanged("DataChiusura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la filiale della banca è attiva
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attiva
            {
                get
                {
                    return m_Attiva;
                }

                set
                {
                    if (m_Attiva == value)
                        return;
                    m_Attiva = value;
                    DoChanged("Attiva", value, !value);
                }
            }

            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Banche; //.Module;
            }

            /// <summary>
            /// Restituisce il discriminante nel repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Banche";
            }

            /// <summary>
            /// Restituisce true se l'oggetto è stato cambiato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || m_Indirizzo.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_Indirizzo.SetChanged(false);
            }

           
            /// <summary>
            /// Carica dal DB
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_Filiale = reader.Read("Filiale", m_Filiale);
                m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", m_Indirizzo.Citta);
                m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", m_Indirizzo.Provincia);
                m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", m_Indirizzo.CAP);
                m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                m_Indirizzo.SetChanged(false);
                m_ABI = reader.Read("ABI", m_ABI);
                m_CAB = reader.Read("CAB", m_CAB);
                m_SWIFT = reader.Read("SWIFT", m_SWIFT);
                m_DataApertura = reader.Read("DataApertura", m_DataApertura);
                m_DataChiusura = reader.Read("DataChiusura", m_DataChiusura);
                m_Attiva = reader.Read("Attivo", m_Attiva);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Filiale", m_Filiale);
                writer.Write("Indirizzo_Citta", m_Indirizzo.Citta);
                writer.Write("Indirizzo_Provincia", m_Indirizzo.Provincia);
                writer.Write("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                writer.Write("Indirizzo_CAP", m_Indirizzo.CAP);
                writer.Write("ABI", m_ABI);
                writer.Write("CAB", m_CAB);
                writer.Write("SWIFT", m_SWIFT);
                writer.Write("DataApertura", m_DataApertura);
                writer.Write("DataChiusura", m_DataChiusura);
                writer.Write("Attivo", m_Attiva);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi della tabella
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Descrizione", typeof(string), 255);
                c = table.Fields.Ensure("Filiale", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Citta", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Via", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_CAP", typeof(string), 255);
                c = table.Fields.Ensure("ABI", typeof(string), 255);
                c = table.Fields.Ensure("CAB", typeof(string), 255);
                c = table.Fields.Ensure("SWIFT", typeof(string), 255);
                c = table.Fields.Ensure("DataApertura", typeof(DateTime), 0);
                c = table.Fields.Ensure("DataChiusura", typeof(DateTime), 0);
                c = table.Fields.Ensure("Attivo", typeof(bool), 0);
            }

            /// <summary>
            /// Prepara i vincoli della tabella
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxNome", new string[] { "Descrizione", "Filiale", "Stato" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxIndirizzo", new string[] { "Indirizzo_Provincia", "Indirizzo_Citta", "Indirizzo_CAP", "Indirizzo_Via" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCodici", new string[] { "ABI", "CAB", "SWIFT" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataApertura", "DataChiusura", "Attivo" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Filiale", m_Filiale);
                writer.WriteAttribute("ABI", m_ABI);
                writer.WriteAttribute("CAB", m_CAB);
                writer.WriteAttribute("SWIFT", m_SWIFT);
                writer.WriteAttribute("DataApertura", m_DataApertura);
                writer.WriteAttribute("DataChiusura", m_DataChiusura);
                writer.WriteAttribute("Attiva", m_Attiva);
                base.XMLSerialize(writer);
                writer.WriteTag("Indirizzo", this.m_Indirizzo);
            }

            /// <summary>
            /// Deserializzazione XML
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

                    case "Filiale":
                        {
                            m_Filiale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = (CIndirizzo)fieldValue;
                            break;
                        }

                    case "ABI":
                        {
                            m_ABI = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CAB":
                        {
                            m_CAB = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SWIFT":
                        {
                            m_SWIFT = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataApertura":
                        {
                            m_DataApertura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataChiusura":
                        {
                            m_DataApertura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Attiva":
                        {
                            m_Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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
                return DMD.Strings.ConcatArray(this.m_Descrizione , ", " , m_Filiale , " (" , m_ABI , ", " , m_CAB  , ")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Descrizione, this.m_Filiale, this.m_ABI, this.m_CAB);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CBanca) && this.Equals((CBanca)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CBanca obj)
            {
                return     base.Equals(obj)
                        && DMD.Strings.EQ(this.m_Filiale, obj.m_Filiale)
                        && this.m_Indirizzo.Equals(obj.m_Indirizzo)
                        && DMD.Strings.EQ(this.m_ABI, obj.m_ABI)
                        && DMD.Strings.EQ(this.m_CAB, obj.m_CAB)
                        && DMD.Strings.EQ(this.m_SWIFT, obj.m_SWIFT)
                        && DMD.DateUtils.EQ(this.m_DataApertura, obj.m_DataApertura)
                        && DMD.DateUtils.EQ(this.m_DataChiusura, obj.m_DataChiusura)
                        && DMD.Booleans.EQ(this.m_Attiva, obj.m_Attiva)
                        
                        ;

            }
        }
    }
}