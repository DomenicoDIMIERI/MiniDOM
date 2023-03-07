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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
        /// Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CAltroPreventivo
            : minidom.Databases.DBObject
        {
            private int m_IDRichiestaDiFinanziamento;
            [NonSerialized] private CRichiestaFinanziamento m_RichiestaDiFinanziamento;
            private DateTime? m_Data;
            private int m_IDIstituto;
            [NonSerialized] private CCQSPDCessionarioClass m_Istituto;
            private string m_NomeIstituto;
            private int m_IDAgenzia;
            [NonSerialized] private Anagrafica.CAzienda m_Agenzia;
            private string m_NomeAgenzia;
            private int m_IDAgente;
            [NonSerialized] private Anagrafica.CPersonaFisica m_Agente;
            private string m_NomeAgente;
            private decimal? m_Rata;
            private int? m_Durata;
            private decimal? m_MontanteLordo;
            private decimal? m_NettoRicavo;
            private decimal? m_NettoAllaMano;
            private double? m_TAN;
            private double? m_TAEG;
            private string m_Descrizione;
            private DateTime? m_DataAccettazione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAltroPreventivo()
            {
                m_IDRichiestaDiFinanziamento = 0;
                m_RichiestaDiFinanziamento = null;
                m_Data = default;
                m_IDIstituto = 0;
                m_Istituto = null;
                m_NomeIstituto = DMD.Strings.vbNullString;
                m_IDAgenzia = 0;
                m_Agenzia = null;
                m_NomeAgenzia = DMD.Strings.vbNullString;
                m_IDAgente = 0;
                m_Agente = null;
                m_NomeAgente = DMD.Strings.vbNullString;
                m_Rata = default;
                m_Durata = default;
                m_MontanteLordo = default;
                m_NettoRicavo = default;
                m_TAN = default;
                m_TAEG = default;
                m_Descrizione = DMD.Strings.vbNullString;
                m_DataAccettazione = default;
                m_NettoAllaMano = default;
            }

            /// <summary>
            /// Restituisdce o imposta l'ID della richiesta di finanziamento in cui è stato registrato questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRichiestaDiFinanziamento
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaDiFinanziamento, m_IDRichiestaDiFinanziamento);
                }

                set
                {
                    int oldValue = IDRichiestaDiFinanziamento;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaDiFinanziamento = value;
                    m_RichiestaDiFinanziamento = null;
                    DoChanged("IDRichiestaDiFinanziamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la richiesta di finanziamento in cui è stato registrato questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CRichiestaFinanziamento RichiestaDiFinanziamento
            {
                get
                {
                    if (m_RichiestaDiFinanziamento is null)
                        m_RichiestaDiFinanziamento = minidom.Finanziaria.RichiesteFinanziamento.GetItemById(m_IDRichiestaDiFinanziamento);
                    return m_RichiestaDiFinanziamento;
                }

                set
                {
                    var oldValue = m_RichiestaDiFinanziamento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaDiFinanziamento = value;
                    m_IDRichiestaDiFinanziamento = DBUtils.GetID(value, 0);
                    DoChanged("RichiestaDiFinanziamento", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la richiesta di finanziamento
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetRichiestaDiFinanziamento(CRichiestaFinanziamento value)
            {
                m_RichiestaDiFinanziamento = value;
                m_IDRichiestaDiFinanziamento = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta la data di rilascio del preventivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value == true)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta L'ID dell'istituto cessionario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDIstituto
            {
                get
                {
                    return DBUtils.GetID(m_Istituto, m_IDIstituto);
                }

                set
                {
                    int oldValue = IDIstituto;
                    if (oldValue == value)
                        return;
                    m_IDIstituto = value;
                    m_Istituto = null;
                    DoChanged("IDIstituto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'istituto cessionario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCQSPDCessionarioClass Istituto
            {
                get
                {
                    if (m_Istituto is null)
                        m_Istituto = minidom.Finanziaria.Cessionari.GetItemById(m_IDIstituto);
                    return m_Istituto;
                }

                set
                {
                    var oldValue = m_Istituto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Istituto = value;
                    m_IDIstituto = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeIstituto = value.Nome;
                    DoChanged("Istituto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'istituto cessionario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeIstituto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIstituto = value;
                    DoChanged("NomeIstituto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'agenzia che ha richiesto il conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAgenzia
            {
                get
                {
                    return DBUtils.GetID(m_Agenzia, m_IDAgenzia);
                }

                set
                {
                    int oldValue = IDAgenzia;
                    if (oldValue == value)
                        return;
                    m_IDAgenzia = value;
                    m_Agenzia = null;
                    DoChanged("IDAgenzia", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusce o imposta l'agenzia
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda Agenzia
            {
                get
                {
                    if (m_Agenzia is null)
                        m_Agenzia = minidom.Anagrafica.Aziende.GetItemById(m_IDAgenzia);
                    return m_Agenzia;
                }

                set
                {
                    var oldValue = m_Agenzia;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Agenzia = value;
                    m_IDAgenzia = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAgenzia = value.Nominativo;
                    DoChanged("Agenzia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'agenzia richiedente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAgenzia
            {
                get
                {
                    return m_NomeAgenzia;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgenzia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgenzia = value;
                    DoChanged("NomeAgenzia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'agente che ha richiesto il conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAgente
            {
                get
                {
                    return DBUtils.GetID(m_Agente, m_IDAgente);
                }

                set
                {
                    int oldValue = IDAgente;
                    if (oldValue == value)
                        return;
                    m_IDAgente = value;
                    m_Agente = null;
                    DoChanged("IDAgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersonaFisica Agente
            {
                get
                {
                    if (m_Agente is null)
                        m_Agente = (Anagrafica.CPersonaFisica)minidom.Anagrafica.Persone.GetItemById(m_IDAgente);
                    return m_Agente;
                }

                set
                {
                    var oldValue = m_Agente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Agente = value;
                    m_IDAgente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAgente = value.Nominativo;
                    DoChanged("Agente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'agente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAgente
            {
                get
                {
                    return m_NomeAgente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgente = value;
                    DoChanged("NomeAgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di evasione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataAccettazione
            {
                get
                {
                    return m_DataAccettazione;
                }

                set
                {
                    var oldValue = m_DataAccettazione;
                    if (oldValue == value == true)
                        return;
                    m_DataAccettazione = value;
                    DoChanged("DataAccettazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la rata proposta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    var oldValue = m_Rata;
                    if (oldValue == value == true)
                        return;
                    m_Rata = value;
                    DoChanged("Rata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la durata proposta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int? Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    var oldValue = m_Durata;
                    if (oldValue == value == true)
                        return;
                    m_Durata = value;
                    DoChanged("Durata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il netto ricavo proposto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? NettoRicavo
            {
                get
                {
                    return m_NettoRicavo;
                }

                set
                {
                    var oldValue = m_NettoRicavo;
                    if (oldValue == value == true)
                        return;
                    m_NettoRicavo = value;
                    DoChanged("NettoRicavo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il netto alla mano
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? NettoAllaMano
            {
                get
                {
                    return m_NettoAllaMano;
                }

                set
                {
                    var oldValue = m_NettoAllaMano;
                    if (oldValue == value == true)
                        return;
                    m_NettoAllaMano = value;
                    DoChanged("NettoAllaMano", value, oldValue);
                }
            }

            /// <summary>
            /// Montante lordo
            /// </summary>
            public decimal? MontanteLordo
            {
                get
                {
                    return m_MontanteLordo;
                }

                set
                {
                    var oldValue = m_MontanteLordo;
                    if (oldValue == value == true)
                        return;
                    m_MontanteLordo = value;
                    DoChanged("MontanteLordo", value, oldValue);
                }
            }

            /// <summary>
            /// TAN
            /// </summary>
            public double? TAN
            {
                get
                {
                    return m_TAN;
                }

                set
                {
                    var oldValue = m_TAN;
                    if (oldValue == value == true)
                        return;
                    m_TAN = value;
                    DoChanged("TAN", value, oldValue);
                }
            }

            /// <summary>
            /// TAEG
            /// </summary>
            public double? TAEG
            {
                get
                {
                    return m_TAEG;
                }

                set
                {
                    var oldValue = m_TAEG;
                    if (oldValue == value == true)
                        return;
                    m_TAEG = value;
                    DoChanged("TAEG", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

          
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.AltriPreventivi;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_RichiesteAltriPrev";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaDiFinanziamento", this.m_IDRichiestaDiFinanziamento);
                m_Data = reader.Read("Data", this.m_Data);
                m_IDIstituto = reader.Read("IDIstituto", this.m_IDIstituto);
                m_NomeIstituto = reader.Read("NomeIstituto", this.m_NomeIstituto);
                m_IDAgenzia = reader.Read("IDAgenzia", this.m_IDAgenzia);
                m_NomeAgenzia = reader.Read("NomeAgenzia", this.m_NomeAgenzia);
                m_IDAgente = reader.Read("IDAgente", this.m_IDAgente);
                m_NomeAgente = reader.Read("NomeAgente", this.m_NomeAgente);
                m_Rata = reader.Read("Rata", this.m_Rata);
                m_Durata = reader.Read("Durata", this.m_Durata);
                m_MontanteLordo = reader.Read("MontanteLordo", this.m_MontanteLordo);
                m_NettoRicavo = reader.Read("NettoRicavo", this.m_NettoRicavo);
                m_TAN = reader.Read("TAN", this.m_TAN);
                m_TAEG = reader.Read("TAEG", this.m_TAEG);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_DataAccettazione = reader.Read("DataAccettazione", this.m_DataAccettazione);
                m_NettoAllaMano = reader.Read("NettoAllaMano", this.m_NettoAllaMano);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDRichiestaDiFinanziamento", IDRichiestaDiFinanziamento);
                writer.Write("Data", m_Data);
                writer.Write("IDIstituto", IDIstituto);
                writer.Write("NomeIstituto", m_NomeIstituto);
                writer.Write("IDAgenzia", IDAgenzia);
                writer.Write("NomeAgenzia", m_NomeAgenzia);
                writer.Write("IDAgente", IDAgente);
                writer.Write("NomeAgente", m_NomeAgente);
                writer.Write("Rata", m_Rata);
                writer.Write("Durata", m_Durata);
                writer.Write("MontanteLordo", m_MontanteLordo);
                writer.Write("NettoRicavo", m_NettoRicavo);
                writer.Write("TAN", m_TAN);
                writer.Write("TAEG", m_TAEG);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("DataAccettazione", m_DataAccettazione);
                writer.Write("NettoAllaMano", m_NettoAllaMano);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDRichiestaDiFinanziamento", typeof(int), 1);
                c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDIstituto", typeof(int), 1);
                c = table.Fields.Ensure("NomeIstituto", typeof(string), 255);
                c = table.Fields.Ensure("IDAgenzia", typeof(int), 1);
                c = table.Fields.Ensure("NomeAgenzia", typeof(string), 255);
                c = table.Fields.Ensure("IDAgente", typeof(int), 1);
                c = table.Fields.Ensure("NomeAgente", typeof(string), 255);
                c = table.Fields.Ensure("Rata", typeof(Decimal), 1);
                c = table.Fields.Ensure("Durata", typeof(int), 1);
                c = table.Fields.Ensure("MontanteLordo", typeof(Decimal), 1);
                c = table.Fields.Ensure("NettoRicavo", typeof(Decimal), 1);
                c = table.Fields.Ensure("TAN", typeof(double), 1);
                c = table.Fields.Ensure("TAEG", typeof(double), 1);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("DataAccettazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("NettoAllaMano", typeof(decimal), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxRichFin", new string[] { "IDRichiestaDiFinanziamento", "Data" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIstituto", new string[] { "IDIstituto", "IDAgenzia", "NomeIstituto", "NomeAgenzia" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAgente", new string[] { "IDAgente", "NomeAgente", "NomeIstituto", "NomeAgenzia" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRataDurata", new string[] { "Rata", "Durata", "NettoRicavo", "NettoAllaMano" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTANTAEG", new string[] { "MontanteLordo", "TAN", "TAEG" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescriz", new string[] { "DataAccettazione", "Descrizione" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDRichiestaDiFinanziamento", IDRichiestaDiFinanziamento);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("DataAccettazione", m_DataAccettazione);
                writer.WriteAttribute("IDIstituto", IDIstituto);
                writer.WriteAttribute("NomeIstituto", m_NomeIstituto);
                writer.WriteAttribute("IDAgenzia", IDAgenzia);
                writer.WriteAttribute("NomeAgenzia", m_NomeAgenzia);
                writer.WriteAttribute("IDAgente", IDAgente);
                writer.WriteAttribute("NomeAgente", m_NomeAgente);
                writer.WriteAttribute("Rata", m_Rata);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("MontanteLordo", m_MontanteLordo);
                writer.WriteAttribute("NettoRicavo", m_NettoRicavo);
                writer.WriteAttribute("TAN", m_TAN);
                writer.WriteAttribute("TAEG", m_TAEG);
                writer.WriteAttribute("NettoAllaMano", m_NettoAllaMano);
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
                    case "IDRichiestaDiFinanziamento":
                        {
                            m_IDRichiestaDiFinanziamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            m_Data = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataAccettazione":
                        {
                            m_DataAccettazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDIstituto":
                        {
                            m_IDIstituto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeIstituto":
                        {
                            m_NomeIstituto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgenzia":
                        {
                            m_IDAgenzia = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgenzia":
                        {
                            m_NomeAgenzia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgente":
                        {
                            m_IDAgente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgente":
                        {
                            m_NomeAgente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Rata":
                        {
                            m_Rata = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MontanteLordo":
                        {
                            m_MontanteLordo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoRicavo":
                        {
                            m_NettoRicavo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoAllaMano":
                        {
                            m_NettoAllaMano = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAN":
                        {
                            m_TAN = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            m_TAEG = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che restituisce l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Preventivo della concorrenza: ", this.NomeIstituto , " del ", this.Data);
            }


            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDIstituto, this.m_IDAgenzia, this.m_Data, this.m_IDRichiestaDiFinanziamento);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CAltroPreventivo) && this.Equals((CAltroPreventivo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CAltroPreventivo obj)
            {
                return base.Equals(obj)
                       && DMD.Integers.EQ(this.m_IDRichiestaDiFinanziamento, obj.m_IDRichiestaDiFinanziamento)
                       && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                       && DMD.Integers.EQ(this.m_IDIstituto, obj.m_IDIstituto)
                       && DMD.Strings.EQ(this.m_NomeIstituto, obj.m_NomeIstituto)
                       && DMD.Integers.EQ(this.m_IDAgenzia, obj.m_IDAgenzia)
                       && DMD.Strings.EQ(this.m_NomeAgenzia, obj.m_NomeAgenzia)
                       && DMD.Integers.EQ(this.m_IDAgente, obj.m_IDAgente)
                       && DMD.Strings.EQ(this.m_NomeAgente, obj.m_NomeAgente)
                       && DMD.Decimals.EQ(this.m_Rata, obj.m_Rata)
                       && DMD.Integers.EQ(this.m_Durata, obj.m_Durata)
                       && DMD.Decimals.EQ(this.m_MontanteLordo, obj.m_MontanteLordo)
                       && DMD.Decimals.EQ(this.m_NettoRicavo, obj.m_NettoRicavo)
                       && DMD.Decimals.EQ(this.m_NettoAllaMano, obj.m_NettoAllaMano)
                       && DMD.Doubles.EQ(this.m_TAN, obj.m_TAN)
                       && DMD.Doubles.EQ(this.m_TAEG, obj.m_TAEG)
                       && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                       && DMD.DateUtils.EQ(this.m_DataAccettazione, obj.m_DataAccettazione)
                        ;
            }
        }
    }
}