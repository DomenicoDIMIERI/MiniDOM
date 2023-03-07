using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Store;
using static minidom.Office;



namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Flag validi per le cartucce
        /// </summary>
        [Flags]
        public enum CartucciaTonerFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Il toner è effettivamente utilizzato in qualche stampante
            /// </summary>
            InUso = 1,

            /// <summary>
            /// Si tratta di un toner originale
            /// </summary>
            Originale = 2,

            /// <summary>
            /// Il toner non è più utilizzabile
            /// </summary>
            Dismesso = 256
        }

        /// <summary>
        /// Rappresenta un consumabile
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CartucciaToner 
            : Databases.DBObjectPO
        {
            private int m_IDArticolo;
            [NonSerialized] private Articolo m_Articolo;
            private string m_NomeArticolo;
            private string m_CodiceArticolo;
            private string m_Modello;
            private string m_Descrizione;
            private int m_IDPostazione;
            [NonSerialized] private Anagrafica.CPostazione m_Postazione;
            private string m_NomePostazione;
            private DateTime? m_DataAcquisto;
            private DateTime? m_DataInstallazione;
            private DateTime? m_DataEsaurimento;
            private DateTime? m_DataRimozione;
            private int? m_StampeDisponibili;
            private int? m_StampeEffettuate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CartucciaToner()
            {
                m_IDArticolo = 0;
                m_Articolo = null;
                m_NomeArticolo = "";
                m_CodiceArticolo = "";
                m_Modello = "";
                m_Descrizione = "";
                m_IDPostazione = 0;
                m_Postazione = null;
                m_NomePostazione = "";
                m_DataAcquisto = default;
                m_DataInstallazione = default;
                m_DataEsaurimento = default;
                m_DataRimozione = default;
                m_StampeDisponibili = default;
                m_StampeEffettuate = default;
                m_Flags = (int) CartucciaTonerFlags.None;
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'articolo associato
            /// </summary>
            /// <returns></returns>
            public int IDArticolo
            {
                get
                {
                    return DBUtils.GetID(m_Articolo, m_IDArticolo);
                }

                set
                {
                    int oldValue = IDArticolo;
                    if (oldValue == value)
                        return;
                    m_IDArticolo = value;
                    m_Articolo = null;
                    DoChanged("IDArticolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'articolo associato
            /// </summary>
            /// <returns></returns>
            public Articolo Articolo
            {
                get
                {
                    if (m_Articolo is null)
                        m_Articolo = minidom.Store.Articoli.GetItemById(m_IDArticolo);
                    return m_Articolo;
                }

                set
                {
                    var oldValue = Articolo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Articolo = value;
                    m_IDArticolo = DBUtils.GetID(value, 0);
                    m_NomeArticolo = "";
                    if (value is object)
                        m_NomeArticolo = value.Nome;
                    DoChanged("Articolo", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'articolo
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetArticolo(Articolo value)
            {
                m_Articolo = value;
                m_IDArticolo = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'articolo associato
            /// </summary>
            /// <returns></returns>
            public string NomeArticolo
            {
                get
                {
                    return m_NomeArticolo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeArticolo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeArticolo = value;
                    DoChanged("NomeArticolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice articolo
            /// </summary>
            public string CodiceArticolo
            {
                get
                {
                    return m_CodiceArticolo;
                }

                set
                {
                    string oldValue = m_CodiceArticolo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceArticolo = value;
                    DoChanged("CodiceArticolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modello
            /// </summary>
            /// <returns></returns>
            public string Modello
            {
                get
                {
                    return m_Modello;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Modello;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Modello = value;
                    DoChanged("Modello", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta l'ID della postazione
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta la postazione
            /// </summary>
            /// <returns></returns>
            public Anagrafica.CPostazione Postazione
            {
                get
                {
                    if (m_Postazione is null)
                        m_Postazione = Anagrafica.Postazioni.GetItemById(m_IDPostazione);
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
            /// Restituisce o imposta il nome della postazione di lavoro in cui é montata la cartuccia
            /// </summary>
            public string NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePostazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePostazione = value;
                    DoChanged("NomePostazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di acquisto della cartuccia
            /// </summary>
            public DateTime? DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }

                set
                {
                    var oldValue = m_DataAcquisto;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAcquisto = value;
                    DoChanged("DataAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di installazione della cartuccia
            /// </summary>
            public DateTime? DataInstallazione
            {
                get
                {
                    return m_DataInstallazione;
                }

                set
                {
                    var oldValue = m_DataInstallazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInstallazione = value;
                    DoChanged("DataInstallazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di esaurimento della cartuccia
            /// </summary>
            public DateTime? DataEsaurimento
            {
                get
                {
                    return m_DataEsaurimento;
                }

                set
                {
                    var oldValue = m_DataEsaurimento;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataEsaurimento = value;
                    DoChanged("DataEsaurimento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui il toner é stato rimosso dalla stampante
            /// </summary>
            /// <returns></returns>
            public DateTime? DataRimozione
            {
                get
                {
                    return m_DataRimozione;
                }

                set
                {
                    var oldValue = m_DataRimozione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRimozione = value;
                    DoChanged("DataRimozione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di stampe disponibili dichiarate
            /// </summary>
            /// <returns></returns>
            public int? StampeDisponibili
            {
                get
                {
                    return m_StampeDisponibili;
                }

                set
                {
                    var oldValue = m_StampeDisponibili;
                    if (oldValue == value == true)
                        return;
                    m_StampeDisponibili = value;
                    DoChanged("StampeDisponibili", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di stampe effettuate
            /// </summary>
            public int? StampeEffettuate
            {
                get
                {
                    return m_StampeEffettuate;
                }

                set
                {
                    var oldValue = m_StampeEffettuate;
                    if (oldValue == value == true)
                        return;
                    m_StampeEffettuate = value;
                    DoChanged("StampeEffettuate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags aggiuntivi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CartucciaTonerFlags Flags
            {
                get
                {
                    return (CartucciaTonerFlags)base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int) value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_CodiceArticolo;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_CodiceArticolo);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.CartucceToners;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeCartucceToner";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDArticolo = reader.Read("IDArticolo", m_IDArticolo);
                m_NomeArticolo = reader.Read("NomeArticolo", m_NomeArticolo);
                m_CodiceArticolo = reader.Read("CodiceArticolo", m_CodiceArticolo);
                m_Modello = reader.Read("Modello", m_Modello);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_IDPostazione = reader.Read("IDPostazione", m_IDPostazione);
                m_NomePostazione = reader.Read("NomePostazione", m_NomePostazione);
                m_DataAcquisto = reader.Read("DataAcquisto", m_DataAcquisto);
                m_DataInstallazione = reader.Read("DataInstallazione", m_DataInstallazione);
                m_DataEsaurimento = reader.Read("DataEsaurimento", m_DataEsaurimento);
                m_DataRimozione = reader.Read("DataRimozione", m_DataRimozione);
                m_StampeDisponibili = reader.Read("StampeDisponibili", m_StampeDisponibili);
                m_StampeEffettuate = reader.Read("StampeEffettuate", m_StampeEffettuate);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDArticolo", IDArticolo);
                writer.Write("NomeArticolo", m_NomeArticolo);
                writer.Write("CodiceArticolo", m_CodiceArticolo);
                writer.Write("Modello", m_Modello);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDPostazione", IDPostazione);
                writer.Write("NomePostazione", m_NomePostazione);
                writer.Write("DataAcquisto", m_DataAcquisto);
                writer.Write("DataInstallazione", m_DataInstallazione);
                writer.Write("DataEsaurimento", m_DataEsaurimento);
                writer.Write("DataRimozione", m_DataRimozione);
                writer.Write("StampeDisponibili", m_StampeDisponibili);
                writer.Write("StampeEffettuate", m_StampeEffettuate);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDArticolo", typeof(int), 1);
                c = table.Fields.Ensure("NomeArticolo", typeof(string), 255);
                c = table.Fields.Ensure("CodiceArticolo", typeof(string), 255);
                c = table.Fields.Ensure("Modello", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("IDPostazione", typeof(int), 1);
                c = table.Fields.Ensure("NomePostazione", typeof(string), 255);
                c = table.Fields.Ensure("DataAcquisto", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataInstallazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataEsaurimento", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataRimozione", typeof(DateTime), 1);
                c = table.Fields.Ensure("StampeDisponibili", typeof(int), 1);
                c = table.Fields.Ensure("StampeEffettuate", typeof(int), 1);
                c = table.Fields.Ensure("Flags", typeof(int), 1);               

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxArticolo", new string[] { "IDArticolo", "NomeArticolo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCodici", new string[] { "CodiceArticolo", "Modello", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPostazione", new string[] { "IDPostazione", "NomePostazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInstallazione", new string[] { "DataAcquisto", "DataInstallazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRimozione", new string[] { "DataEsaurimento", "DataRimozione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStampte", new string[] { "StampeDisponibili", "StampeEffettuate" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializza l'oggetto
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDArticolo", IDArticolo);
                writer.WriteAttribute("NomeArticolo", m_NomeArticolo);
                writer.WriteAttribute("CodiceArticolo", m_CodiceArticolo);
                writer.WriteAttribute("Modello", m_Modello);
                writer.WriteAttribute("IDPostazione", IDPostazione);
                writer.WriteAttribute("NomePostazione", m_NomePostazione);
                writer.WriteAttribute("DataAcquisto", m_DataAcquisto);
                writer.WriteAttribute("DataInstallazione", m_DataInstallazione);
                writer.WriteAttribute("DataEsaurimento", m_DataEsaurimento);
                writer.WriteAttribute("DataRimozione", m_DataRimozione);
                writer.WriteAttribute("StampeDisponibili", m_StampeDisponibili);
                writer.WriteAttribute("StampeEffettuate", m_StampeEffettuate);
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
                    case "IDArticolo":
                        {
                            m_IDArticolo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeArticolo":
                        {
                            m_NomeArticolo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceArticolo":
                        {
                            m_CodiceArticolo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Modello":
                        {
                            m_Modello = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

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

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAcquisto":
                        {
                            m_DataAcquisto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataInstallazione":
                        {
                            m_DataInstallazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataEsaurimento":
                        {
                            m_DataEsaurimento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataRimozione":
                        {
                            m_DataRimozione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StampeDisponibili":
                        {
                            m_StampeDisponibili = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StampeEffettuate":
                        {
                            m_StampeEffettuate = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CartucciaToner) && this.Equals((CartucciaToner)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CartucciaToner obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDArticolo, obj.m_IDArticolo)
                    && DMD.Strings.EQ(this.m_NomeArticolo, obj.m_NomeArticolo)
                    && DMD.Strings.EQ(this.m_CodiceArticolo, obj.m_CodiceArticolo)
                    && DMD.Strings.EQ(this.m_Modello, obj.m_Modello)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Integers.EQ(this.m_IDPostazione, obj.m_IDPostazione)
                    && DMD.Strings.EQ(this.m_NomePostazione, obj.m_NomePostazione)
                    && DMD.DateUtils.EQ(this.m_DataAcquisto, obj.m_DataAcquisto)
                    && DMD.DateUtils.EQ(this.m_DataInstallazione, obj.m_DataInstallazione)
                    && DMD.DateUtils.EQ(this.m_DataEsaurimento, obj.m_DataEsaurimento)
                    && DMD.DateUtils.EQ(this.m_DataRimozione, obj.m_DataRimozione)
                    && DMD.Integers.EQ(this.m_StampeDisponibili, obj.m_StampeDisponibili)
                    && DMD.Integers.EQ(this.m_StampeEffettuate, obj.m_StampeEffettuate)
                    ;
            }
        }
    }
}