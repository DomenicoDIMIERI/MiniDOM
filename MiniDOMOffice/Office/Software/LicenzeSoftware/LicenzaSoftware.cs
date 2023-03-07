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
        /// Stato degli oggetti <see cref="LicenzaSoftware"/>
        /// </summary>
        public enum StatoLicenzaSoftware : int
        {
            /// <summary>
            /// Sconosciuto
            /// </summary>
            Sconosciuto = 0,

            /// <summary>
            /// Non utilizzata
            /// </summary>
            NonUtilizzato = 1,

            /// <summary>
            /// Utilizzata
            /// </summary>
            Utilizzato = 2,

            /// <summary>
            /// Dismesso
            /// </summary>
            Dismessa = 3
        }

        /// <summary>
        /// Flags su <see cref="LicenzaSoftware"/>
        /// </summary>
        [Flags]
        public enum FlagsLicenzaSoftware : int
        {
            None = 0,

            /// <summary>
            /// Licenza di tipo OEM
            /// </summary>
            OEM = 1,

            /// <summary>
            /// Licenza di tipo "volume"
            /// </summary>
            Volume = 2
        }

        /// <summary>
        /// Rappresenta una licenza di un software
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class LicenzaSoftware 
            : Databases.DBObjectPO
        {
            private int m_IDSoftware;
            [NonSerialized] private Software m_Software;
            private string m_NomeSoftware;
            private int m_IDDispositivo;
            [NonSerialized] private Dispositivo m_Dispositivo;
            private string m_NomeDispositivo;
            private string m_CodiceLicenza;
            private DateTime? m_DataAcquisto;
            private DateTime? m_DataInstallazione;
            private DateTime? m_DataDismissione;
            private string m_DettaglioStato;
            private string m_ScaricatoDa;
            private StatoLicenzaSoftware m_StatoUtilizzo;
            private int m_IDProprietario;
            [NonSerialized] private CPersona m_Proprietario;
            private string m_NomeProprietario;
            private int m_IDDocumentoAcquisto;
            [NonSerialized] private DocumentoContabile m_DocumentoAcquisto;
            private string m_NumeroDocumentoAcquisto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public LicenzaSoftware()
            {
                m_IDSoftware = 0;
                m_Software = null;
                m_NomeSoftware = "";
                m_IDDispositivo = 0;
                m_Dispositivo = null;
                m_NomeDispositivo = "";
                m_CodiceLicenza = "";
                m_DataAcquisto = default;
                m_DataInstallazione = default;
                m_DataDismissione = default;
                m_DettaglioStato = "";
                m_ScaricatoDa = "";
                m_StatoUtilizzo = StatoLicenzaSoftware.Sconosciuto;
                m_Flags = (int)FlagsLicenzaSoftware.None;
                m_IDProprietario = 0;
                m_Proprietario = null;
                m_NomeProprietario = "";
                m_IDDocumentoAcquisto = 0;
                m_DocumentoAcquisto = null;
                m_NumeroDocumentoAcquisto = "";
            }

            /// <summary>
            /// ID del software
            /// </summary>
            public int IDSoftware
            {
                get
                {
                    return DBUtils.GetID(m_Software, m_IDSoftware);
                }

                set
                {
                    int oldValue = IDSoftware;
                    if (oldValue == value)
                        return;
                    m_IDSoftware = value;
                    m_Software = null;
                    DoChanged("IDSoftware", value, oldValue);
                }
            }

            /// <summary>
            /// Software
            /// </summary>
            public Software Software
            {
                get
                {
                    if (m_Software is null)
                        m_Software = Softwares.GetItemById(m_IDSoftware);
                    return m_Software;
                }

                set
                {
                    var oldValue = Software;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Software = value;
                    m_IDSoftware = DBUtils.GetID(value, 0);
                    m_NomeSoftware = "";
                    if (value is object)
                        m_NomeSoftware = value.Nome + " " + value.Versione;
                    DoChanged("Software", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del software
            /// </summary>
            public string NomeSoftware
            {
                get
                {
                    return m_NomeSoftware;
                }

                set
                {
                    string oldValue = m_NomeSoftware;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSoftware = value;
                    DoChanged("NomeSoftware", value, oldValue);
                }
            }

            /// <summary>
            /// ID del dispositivo
            /// </summary>
            public int IDDispositivo
            {
                get
                {
                    return DBUtils.GetID(m_Dispositivo, m_IDDispositivo);
                }

                set
                {
                    int oldValue = IDDispositivo;
                    if (oldValue == value)
                        return;
                    m_IDDispositivo = value;
                    m_Dispositivo = null;
                    DoChanged("IDDispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Dispositivo
            /// </summary>
            public Dispositivo Dispositivo
            {
                get
                {
                    if (m_Dispositivo is null)
                        m_Dispositivo = Dispositivi.GetItemById(m_IDDispositivo);
                    return m_Dispositivo;
                }

                set
                {
                    var oldValue = Dispositivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Dispositivo = value;
                    m_IDDispositivo = DBUtils.GetID(value, 0);
                    m_NomeDispositivo = "";
                    if (value is object)
                        m_NomeDispositivo = value.Nome;
                    DoChanged("Dispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del dispositivo
            /// </summary>
            public string NomeDispositivo
            {
                get
                {
                    return m_NomeDispositivo;
                }

                set
                {
                    string oldValue = m_NomeDispositivo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDispositivo = value;
                    DoChanged("NomeDispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Codice della licenza
            /// </summary>
            public string CodiceLicenza
            {
                get
                {
                    return m_CodiceLicenza;
                }

                set
                {
                    string oldValue = m_CodiceLicenza;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceLicenza = value;
                    DoChanged("CodiceLicenza", value, oldValue);
                }
            }

            /// <summary>
            /// Data di acquisto
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
            /// Data di installazione
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
            /// Data di dismissione
            /// </summary>
            public DateTime? DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }

                set
                {
                    var oldValue = m_DataDismissione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataDismissione = value;
                    DoChanged("DataDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Dettaglio stato
            /// </summary>
            public string DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }

                set
                {
                    string oldValue = m_DettaglioStato;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato = value;
                    DoChanged("DettaglioStato", value, oldValue);
                }
            }

            /// <summary>
            /// Scaricado da
            /// </summary>
            public string ScaricatoDa
            {
                get
                {
                    return m_ScaricatoDa;
                }

                set
                {
                    string oldValue = m_ScaricatoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ScaricatoDa = value;
                    DoChanged("ScaricatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Stato di utiizzo
            /// </summary>
            public StatoLicenzaSoftware StatoUtilizzo
            {
                get
                {
                    return m_StatoUtilizzo;
                }

                set
                {
                    var oldValue = m_StatoUtilizzo;
                    if (oldValue == value)
                        return;
                    m_StatoUtilizzo = value;
                    DoChanged("StatoUtilizzo", value, oldValue);
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new FlagsLicenzaSoftware Flags
            {
                get
                {
                    return (FlagsLicenzaSoftware) base.Flags;
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
            /// ID del proprietario
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
            /// Proprietario
            /// </summary>
            public CPersona Proprietario
            {
                get
                {
                    if (m_Proprietario is null)
                        m_Proprietario = Anagrafica.Persone.GetItemById(m_IDProprietario);
                    return m_Proprietario;
                }

                set
                {
                    var oldValue = Proprietario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDProprietario = DBUtils.GetID(value, 0);
                    m_Proprietario = value;
                    m_NomeProprietario = "";
                    if (value is object)
                        m_NomeProprietario = value.Nominativo;
                    DoChanged("Proprietario", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del proprietario
            /// </summary>
            public string NomeProprietario
            {
                get
                {
                    return m_NomeProprietario;
                }

                set
                {
                    string oldValue = m_NomeProprietario;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProprietario = value;
                    DoChanged("NomeProprietario", value, oldValue);
                }
            }

            /// <summary>
            /// ID del documento di acquisto
            /// </summary>
            public int IDDocumentoAcquisto
            {
                get
                {
                    return DBUtils.GetID(m_DocumentoAcquisto, m_IDDocumentoAcquisto);
                }

                set
                {
                    int oldValue = IDDocumentoAcquisto;
                    if (oldValue == value)
                        return;
                    m_IDDocumentoAcquisto = value;
                    m_DocumentoAcquisto = null;
                    DoChanged("IDDocumenntoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Documento di acquisto
            /// </summary>
            public DocumentoContabile DocumentoAcquisto
            {
                get
                {
                    if (m_DocumentoAcquisto is null)
                        m_DocumentoAcquisto = DocumentiContabili.GetItemById(m_IDDocumentoAcquisto);
                    return m_DocumentoAcquisto;
                }

                set
                {
                    var oldValue = DocumentoAcquisto;
                    if (oldValue == value)
                        return;
                    m_DocumentoAcquisto = value;
                    m_IDDocumentoAcquisto = DBUtils.GetID(value, 0);
                    m_NumeroDocumentoAcquisto = "";
                    if (value is object)
                        m_NumeroDocumentoAcquisto = value.NumeroDocumento;
                    DoChanged("DocumentoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Numero del documento di acquisto
            /// </summary>
            public string NumeroDocumentoAcquisto
            {
                get
                {
                    return m_NumeroDocumentoAcquisto;
                }

                set
                {
                    string oldValue = m_NumeroDocumentoAcquisto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroDocumentoAcquisto = value;
                    DoChanged("NumeroDocumentoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_NomeSoftware , " (" , m_CodiceLicenza , ")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeSoftware);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is LicenzaSoftware) && this.Equals((LicenzaSoftware)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(LicenzaSoftware obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDSoftware, obj.m_IDSoftware)
                    && DMD.Strings.EQ(this.m_NomeSoftware, obj.m_NomeSoftware)
                    && DMD.Integers.EQ(this.m_IDDispositivo, obj.m_IDDispositivo)
                    && DMD.Strings.EQ(this.m_NomeDispositivo, obj.m_NomeDispositivo)
                    && DMD.Strings.EQ(this.m_CodiceLicenza, obj.m_CodiceLicenza)
                    && DMD.DateUtils.EQ(this.m_DataAcquisto, obj.m_DataAcquisto)
                    && DMD.DateUtils.EQ(this.m_DataInstallazione, obj.m_DataInstallazione)
                    && DMD.DateUtils.EQ(this.m_DataDismissione, obj.m_DataDismissione)
                    && DMD.Strings.EQ(this.m_DettaglioStato, obj.m_DettaglioStato)
                    && DMD.Strings.EQ(this.m_ScaricatoDa, obj.m_ScaricatoDa)
                    && DMD.RunTime.EQ(this.m_StatoUtilizzo, obj.m_StatoUtilizzo)
                    && DMD.Integers.EQ(this.m_IDProprietario, obj.m_IDProprietario)
                    && DMD.Strings.EQ(this.m_NomeProprietario, obj.m_NomeProprietario)
                    && DMD.Integers.EQ(this.m_IDDocumentoAcquisto, obj.m_IDDocumentoAcquisto)
                    && DMD.Strings.EQ(this.m_NumeroDocumentoAcquisto, obj.m_NumeroDocumentoAcquisto)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.LicenzeSoftware;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeLicenzeSoftware";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDSoftware = reader.Read("IDSoftware", this.m_IDSoftware);
                this.m_NomeSoftware = reader.Read("NomeSoftware", this.m_NomeSoftware);
                this.m_IDDispositivo = reader.Read("IDDispositivo", this.m_IDDispositivo);
                this.m_NomeDispositivo = reader.Read("NomeDispositivo", this.m_NomeDispositivo);
                this.m_CodiceLicenza = reader.Read("CodiceLicenza", this.m_CodiceLicenza);
                this.m_DataAcquisto = reader.Read("DataAcquisto", this.m_DataAcquisto);
                this.m_DataInstallazione = reader.Read("DataInstallazione", this.m_DataInstallazione);
                this.m_DataDismissione = reader.Read("DataDismissione", this.m_DataDismissione);
                this.m_DettaglioStato = reader.Read("DettaglioStato", this.m_DettaglioStato);
                this.m_ScaricatoDa = reader.Read("ScaricatoDa", this.m_ScaricatoDa);
                this.m_StatoUtilizzo = reader.Read("StatoUtilizzo", this.m_StatoUtilizzo);
                this.m_IDProprietario = reader.Read("IDProprietario", this.m_IDProprietario);
                this.m_NomeProprietario = reader.Read("NomeProprietario", this.m_NomeProprietario);
                this.m_IDDocumentoAcquisto = reader.Read("IDDocumentoAcquisto", this.m_IDDocumentoAcquisto);
                this.m_NumeroDocumentoAcquisto = reader.Read("NumeroDocumentoAcquisto", this.m_NumeroDocumentoAcquisto);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDSoftware", IDSoftware);
                writer.Write("NomeSoftware", m_NomeSoftware);
                writer.Write("IDDispositivo", IDDispositivo);
                writer.Write("NomeDispositivo", m_NomeDispositivo);
                writer.Write("CodiceLicenza", m_CodiceLicenza);
                writer.Write("DataAcquisto", m_DataAcquisto);
                writer.Write("DataInstallazione", m_DataInstallazione);
                writer.Write("DataDismissione", m_DataDismissione);
                writer.Write("DettaglioStato", m_DettaglioStato);
                writer.Write("ScaricatoDa", m_ScaricatoDa);
                writer.Write("StatoUtilizzo", m_StatoUtilizzo);
                writer.Write("IDProprietario", IDProprietario);
                writer.Write("NomeProprietario", m_NomeProprietario);
                writer.Write("IDDocumentoAcquisto", IDDocumentoAcquisto);
                writer.Write("NumeroDocumentoAcquisto", m_NumeroDocumentoAcquisto);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDSoftware", typeof(int), 1);
                c = table.Fields.Ensure("NomeSoftware", typeof(string), 255);
                c = table.Fields.Ensure("IDDispositivo", typeof(int), 1);
                c = table.Fields.Ensure("NomeDispositivo", typeof(string), 255);
                c = table.Fields.Ensure("CodiceLicenza", typeof(string), 255);
                c = table.Fields.Ensure("DataAcquisto", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataInstallazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataDismissione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DettaglioStato", typeof(string), 255);
                c = table.Fields.Ensure("ScaricatoDa", typeof(string), 255);
                c = table.Fields.Ensure("StatoUtilizzo", typeof(int), 1);
                c = table.Fields.Ensure("IDProprietario", typeof(int), 1);
                c = table.Fields.Ensure("NomeProprietario", typeof(string), 255);
                c = table.Fields.Ensure("IDDocumentoAcquisto", typeof(int), 1);
                c = table.Fields.Ensure("NumeroDocumentoAcquisto", typeof(string), 255);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxSoftware", new string[] { "IDSoftware", "IDDispositivo", "DataInstallazione", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxNomi", new string[] { "NomeSoftware", "NomeDispositivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCodice", new string[] { "CodiceLicenza" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataAcquisto", "DataDismissione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDettaglioStato", new string[] { "DettaglioStato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxScaricatoDa", new string[] { "ScaricatoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoUtilizzo", new string[] { "StatoUtilizzo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxProprietario", new string[] { "IDProprietario", "NomeProprietario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDocumento", new string[] { "IDDocumentoAcquisto", "NumeroDocumentoAcquisto" }, DBFieldConstraintFlags.None);
                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDSoftware", IDSoftware);
                writer.WriteAttribute("NomeSoftware", m_NomeSoftware);
                writer.WriteAttribute("IDDispositivo", IDDispositivo);
                writer.WriteAttribute("NomeDispositivo", m_NomeDispositivo);
                writer.WriteAttribute("CodiceLicenza", m_CodiceLicenza);
                writer.WriteAttribute("DataAcquisto", m_DataAcquisto);
                writer.WriteAttribute("DataInstallazione", m_DataInstallazione);
                writer.WriteAttribute("DataDismissione", m_DataDismissione);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
                writer.WriteAttribute("ScaricatoDa", m_ScaricatoDa);
                writer.WriteAttribute("StatoUtilizzo", (int?)m_StatoUtilizzo);
                writer.WriteAttribute("IDProprietario", IDProprietario);
                writer.WriteAttribute("NomeProprietario", m_NomeProprietario);
                writer.WriteAttribute("IDDocumentoAcquisto", IDDocumentoAcquisto);
                writer.WriteAttribute("NumeroDocumentoAcquisto", m_NumeroDocumentoAcquisto);
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
                    case "IDSoftware":
                        {
                            m_IDSoftware = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeSoftware":
                        {
                            m_NomeSoftware = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDispositivo":
                        {
                            m_IDDispositivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeDispositivo":
                        {
                            m_NomeDispositivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceLicenza":
                        {
                            m_CodiceLicenza = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "DataDismissione":
                        {
                            m_DataDismissione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ScaricatoDa":
                        {
                            m_ScaricatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoUtilizzo":
                        {
                            m_StatoUtilizzo = (StatoLicenzaSoftware)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "IDDocumentoAcquisto":
                        {
                            m_IDDocumentoAcquisto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroDocumentoAcquisto":
                        {
                            m_NumeroDocumentoAcquisto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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