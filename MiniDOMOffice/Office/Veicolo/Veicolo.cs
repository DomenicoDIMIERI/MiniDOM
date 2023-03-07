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
        /// Stato di un veicolo
        /// </summary>
        public enum StatoVeicolo 
            : int
        {
            /// <summary>
            /// L'oggetto non presenta difetti
            /// </summary>
            /// <remarks></remarks>
            NUOVO = 0,

            /// <summary>
            /// L'oggetto presenta qualche segno di usura ma è ancora funzionale
            /// </summary>
            /// <remarks></remarks>
            USURATO = 1,

            /// <summary>
            /// L'oggetto è danneggiato
            /// </summary>
            /// <remarks></remarks>
            DANNEGGIATO = 2,

            /// <summary>
            /// L'oggetto è stato dismesso
            /// </summary>
            /// <remarks></remarks>
            DISMESSO = 3
        }

        /// <summary>
        /// Flag per un veicolo
        /// </summary>
        [Flags]
        public enum VeicoloFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Indica che il veicolo è disponibile per l'azienda
            /// </summary>
            /// <remarks></remarks>
            Disponibile = 1
        }

        /// <summary>
        /// Rappresenta un veicolo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Veicolo 
            : minidom.Databases.DBObjectPO
        {
            private string m_Nome;
            private string m_Tipo;
            private string m_Modello;
            private string m_Seriale;
            private string m_Alimentazione;
            private double? m_KmALitro;
            private string m_IconURL;
            private DateTime? m_DataAcquisto;
            private DateTime? m_DataDismissione;
            private StatoVeicolo m_StatoVeicolo;
            private string m_Targa;
            private DateTime? m_DataImmatricolazione;
            private double? m_ConsumoUrbano;
            private double? m_ConsumoExtraUrbano;
            private double? m_ConsumoCombinato;
            private string m_Note;
            //private VeicoloFlags m_Flags;
            private int m_IDProprietario;
            [NonSerialized] private Anagrafica.CPersona m_Proprietario;
            private string m_NomeProprietario;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Veicolo()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_Tipo = DMD.Strings.vbNullString;
                m_Modello = DMD.Strings.vbNullString;
                m_Seriale = DMD.Strings.vbNullString;
                m_Alimentazione = DMD.Strings.vbNullString;
                m_KmALitro = default;
                m_IconURL = DMD.Strings.vbNullString;
                m_DataAcquisto = default;
                m_DataDismissione = default;
                m_StatoVeicolo = StatoVeicolo.NUOVO;
                m_Targa = "";
                m_DataImmatricolazione = default;
                m_ConsumoUrbano = default;
                m_ConsumoExtraUrbano = default;
                m_ConsumoCombinato = default;
                m_Note = "";
                m_Flags = (int) VeicoloFlags.None;
                m_IDProprietario = 0;
                m_Proprietario = null;
                m_NomeProprietario = "";
            }

            /// <summary>
            /// ID del proprietario del veicolo
            /// </summary>
            public int IDProprietario
            {
                get
                {
                    return DBUtils.GetID(m_Proprietario, m_IDProprietario);
                }

                set
                {
                    int oldValue = IDProprietario;
                    if (oldValue == value)
                        return;
                    m_IDProprietario = value;
                    m_Proprietario = null;
                    DoChanged("IDProprietario", value, oldValue);
                }
            }

            /// <summary>
            /// Proprietario del veicolo
            /// </summary>
            public Anagrafica.CPersona Proprietario
            {
                get
                {
                    if (m_Proprietario is null)
                        m_Proprietario = Anagrafica.Persone.GetItemById(m_IDProprietario);
                    return m_Proprietario;
                }

                set
                {
                    var oldValue = m_Proprietario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Proprietario = value;
                    m_IDProprietario = DBUtils.GetID(value, 0);
                    m_NomeProprietario = (value is object )? value.Nominativo : "";
                    DoChanged("Proprietario", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del proprietario del veicolo
            /// </summary>
            public string NomeProprietario
            {
                get
                {
                    return m_NomeProprietario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeProprietario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProprietario = value;
                    DoChanged("NomeProprietario", value, oldValue);
                }
            }

            /// <summary>
            /// Flags per il veicolo
            /// </summary>
            public VeicoloFlags Flags
            {
                get
                {
                    return (VeicoloFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    this.m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Targa del veicolo
            /// </summary>
            public string Targa
            {
                get
                {
                    return m_Targa;
                }

                set
                {
                    value = Veicoli.ParseTarga(value);
                    string oldValue = m_Targa;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Targa = value;
                    DoChanged("Targa", value, oldValue);
                }
            }

            /// <summary>
            /// Data di immatricolazione del veicolo
            /// </summary>
            public DateTime? DataImmatricolazione
            {
                get
                {
                    return m_DataImmatricolazione;
                }

                set
                {
                    var oldValue = m_DataImmatricolazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataImmatricolazione = value;
                    DoChanged("DataImmatricolazione", value, oldValue);
                }
            }

            /// <summary>
            /// Consumo in ambito urbano (litri su 100 km)
            /// </summary>
            public double? ConsumoUrbano
            {
                get
                {
                    return m_ConsumoUrbano;
                }

                set
                {
                    var oldValue = m_ConsumoUrbano;
                    if (oldValue == value == true)
                        return;
                    m_ConsumoUrbano = value;
                    DoChanged("ConsumoUrbano", value, oldValue);
                }
            }

            /// <summary>
            /// Consumo su strade extraurbane (l / 100 km)
            /// </summary>
            public double? ConsumoExtraUrbano
            {
                get
                {
                    return m_ConsumoExtraUrbano;
                }

                set
                {
                    var oldValue = m_ConsumoExtraUrbano;
                    if (oldValue == value == true)
                        return;
                    m_ConsumoExtraUrbano = value;
                    DoChanged("ConsumoExtraUrbano", value, oldValue);
                }
            }

            /// <summary>
            /// Consumo su strade generiche (l / 100 km)
            /// </summary>
            public double? ConsumoCombinato
            {
                get
                {
                    return m_ConsumoCombinato;
                }

                set
                {
                    var oldValue = m_ConsumoCombinato;
                    if (oldValue == value == true)
                        return;
                    m_ConsumoCombinato = value;
                    DoChanged("ConsumoCombinato", value, oldValue);
                }
            }

            /// <summary>
            /// Note 
            /// </summary>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("value", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di alimentazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Alimentazione
            {
                get
                {
                    return m_Alimentazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Alimentazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Alimentazione = value;
                    DoChanged("Alimentazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i km/l medi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? KmALitro
            {
                get
                {
                    return m_KmALitro;
                }

                set
                {
                    var oldValue = m_KmALitro;
                    if (oldValue == value == true)
                        return;
                    m_KmALitro = value;
                    DoChanged("KmALitro", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il nome del dispositivo
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
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del dispositivo
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
                    value = Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modello del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Modello
            {
                get
                {
                    return m_Modello;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Modello;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Modello = value;
                    DoChanged("Modello", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di serie del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Seriale
            {
                get
                {
                    return m_Seriale;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Seriale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Seriale = value;
                    DoChanged("Seriale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso dell'icona associata al dispositivo
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
                    value = Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta un valore che indica lo stato del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoVeicolo StatoVeicolo
            {
                get
                {
                    return m_StatoVeicolo;
                }

                set
                {
                    var oldValue = m_StatoVeicolo;
                    if (oldValue == value)
                        return;
                    m_StatoVeicolo = value;
                    DoChanged("StatoVeicolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di acquisto del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }

                set
                {
                    var oldValue = m_DataAcquisto;
                    if (oldValue == value == true)
                        return;
                    m_DataAcquisto = value;
                    DoChanged("DataAcquisto", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta la data di dismissione del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }

                set
                {
                    var oldValue = m_DataDismissione;
                    if (oldValue == value == true)
                        return;
                    m_DataDismissione = value;
                    DoChanged("DataDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray( m_Nome , " (" , m_Tipo , ", " , m_Modello , ", " , m_Seriale ,")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Targa);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Veicoli;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeVeicoli";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_Tipo = reader.Read("Tipo",  m_Tipo);
                m_Modello = reader.Read("Modello",  m_Modello);
                m_Seriale = reader.Read("Seriale",  m_Seriale);
                m_Alimentazione = reader.Read("Alimentazione",  m_Alimentazione);
                m_KmALitro = reader.Read("KmALitro",  m_KmALitro);
                m_IconURL = reader.Read("IconURL",  m_IconURL);
                m_DataAcquisto = reader.Read("DataAcquisto",  m_DataAcquisto);
                m_DataDismissione = reader.Read("DataDismissione",  m_DataDismissione);
                m_StatoVeicolo = reader.Read("StatoVeicolo",  m_StatoVeicolo);
                m_Targa = reader.Read("Targa",  m_Targa);
                m_DataImmatricolazione = reader.Read("DataImmatricolazione",  m_DataImmatricolazione);
                m_ConsumoUrbano = reader.Read("ConsumoUrbano",  m_ConsumoUrbano);
                m_ConsumoExtraUrbano = reader.Read("ConsumoExtraUrbano",  m_ConsumoExtraUrbano);
                m_ConsumoCombinato = reader.Read("ConsumoCombinato",  m_ConsumoCombinato);
                m_Note = reader.Read("Note",  m_Note);
                m_IDProprietario = reader.Read("IDProprietario",  m_IDProprietario);
                m_NomeProprietario = reader.Read("NomeProprietario",  m_NomeProprietario);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Tipo", m_Tipo);
                writer.Write("Modello", m_Modello);
                writer.Write("Seriale", m_Seriale);
                writer.Write("Alimentazione", m_Alimentazione);
                writer.Write("KmALitro", m_KmALitro);
                writer.Write("IconURL", m_IconURL);
                writer.Write("DataAcquisto", m_DataAcquisto);
                writer.Write("DataDismissione", m_DataDismissione);
                writer.Write("StatoVeicolo", m_StatoVeicolo);
                writer.Write("Targa", m_Targa);
                writer.Write("DataImmatricolazione", m_DataImmatricolazione);
                writer.Write("ConsumoUrbano", m_ConsumoUrbano);
                writer.Write("ConsumoExtraUrbano", m_ConsumoExtraUrbano);
                writer.Write("ConsumoCombinato", m_ConsumoCombinato);
                writer.Write("Note", m_Note);
                writer.Write("IDProprietario", IDProprietario);
                writer.Write("NomeProprietario", m_NomeProprietario);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Modello", typeof(string), 255);
                c = table.Fields.Ensure("Seriale", typeof(string), 255);
                c = table.Fields.Ensure("Alimentazione", typeof(string), 255);
                c = table.Fields.Ensure("KmALitro", typeof(double), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("DataAcquisto", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataDismissione", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoVeicolo", typeof(int), 1);
                c = table.Fields.Ensure("Targa", typeof(string), 255);
                c = table.Fields.Ensure("DataImmatricolazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("ConsumoUrbano", typeof(double), 1);
                c = table.Fields.Ensure("ConsumoExtraUrbano", typeof(double), 1);
                c = table.Fields.Ensure("ConsumoCombinato", typeof(double), 1);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("IDProprietario", typeof(int), 1);
                c = table.Fields.Ensure("NomeProprietario", typeof(string), 255);
                
            }


            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Tipo", "Modello" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSeriale", new string[] { "Seriale", "StatoVeicolo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri", new string[] { "Alimentazione", "KmALitro", "ConsumoUrbano", "ConsumoExtraUrbano", "ConsumoCombinato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataAcquisto", "DataDismissione", "DataImmatricolazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTarga", new string[] { "Targa", "IconURL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxProprietario", new string[] { "IDProprietario" , "NomeProprietario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxProprietario", new string[] { "IDProprietario", "NomeProprietario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "Note" }, DBFieldConstraintFlags.None);
                  
                  
                   
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Modello", m_Modello);
                writer.WriteAttribute("Seriale", m_Seriale);
                writer.WriteAttribute("Alimentazione", m_Alimentazione);
                writer.WriteAttribute("KmALitro", m_KmALitro);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("DataAcquisto", m_DataAcquisto);
                writer.WriteAttribute("DataDismissione", m_DataDismissione);
                writer.WriteAttribute("StatoVeicolo", (int?)m_StatoVeicolo);
                writer.WriteAttribute("Targa", m_Targa);
                writer.WriteAttribute("DataImmatricolazione", m_DataImmatricolazione);
                writer.WriteAttribute("ConsumoUrbano", m_ConsumoUrbano);
                writer.WriteAttribute("ConsumoExtraUrbano", m_ConsumoExtraUrbano);
                writer.WriteAttribute("ConsumoCombinato", m_ConsumoCombinato);
                writer.WriteAttribute("IDProprietario", IDProprietario);
                writer.WriteAttribute("NomeProprietario", m_NomeProprietario);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
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

                    case "Modello":
                        {
                            m_Modello = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Seriale":
                        {
                            m_Seriale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Alimentazione":
                        {
                            m_Alimentazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "KmALitro":
                        {
                            m_KmALitro = (double?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAcquisto":
                        {
                            m_DataAcquisto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataDismissione":
                        {
                            m_DataDismissione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoVeicolo":
                        {
                            m_StatoVeicolo = (StatoVeicolo)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Targa":
                        {
                            m_Targa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataImmatricolazione":
                        {
                            m_DataImmatricolazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ConsumoUrbano":
                        {
                            m_ConsumoUrbano = (double?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ConsumoExtraUrbano":
                        {
                            m_ConsumoExtraUrbano = (double?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ConsumoCombinato":
                        {
                            m_ConsumoCombinato = (double?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                 
                    case "IDProprietario":
                        {
                            m_IDProprietario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProprietario":
                        {
                            m_NomeProprietario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return (obj is Veicolo) && this.Equals((Veicolo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Veicolo obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Modello, obj.m_Modello)
                    && DMD.Strings.EQ(this.m_Seriale, obj.m_Seriale)
                    && DMD.Strings.EQ(this.m_Alimentazione, obj.m_Alimentazione)
                    && DMD.Doubles.EQ(this.m_KmALitro, obj.m_KmALitro)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.DateUtils.EQ(this.m_DataAcquisto, obj.m_DataAcquisto)
                    && DMD.DateUtils.EQ(this.m_DataDismissione, obj.m_DataDismissione)
                    && DMD.Integers.EQ((int)this.m_StatoVeicolo, (int)obj.m_StatoVeicolo)
                    && DMD.Strings.EQ(this.m_Targa, obj.m_Targa)
                    && DMD.DateUtils.EQ(this.m_DataImmatricolazione, obj.m_DataImmatricolazione)
                    && DMD.Doubles.EQ(this.m_ConsumoUrbano, obj.m_ConsumoUrbano)
                    && DMD.Doubles.EQ(this.m_ConsumoExtraUrbano, obj.m_ConsumoExtraUrbano)
                    && DMD.Doubles.EQ(this.m_ConsumoCombinato, obj.m_ConsumoCombinato)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Integers.EQ(this.m_IDProprietario, obj.m_IDProprietario)
                    && DMD.Strings.EQ(this.m_NomeProprietario, obj.m_NomeProprietario)
                    ;

            }

        }
    }
}