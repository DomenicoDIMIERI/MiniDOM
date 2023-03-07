using System;
using DMD;
using DMD.XML;
using DMD.Databases; 
using static minidom.Sistema;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flags validi per un impiego
        /// </summary>
        [Flags]
        public enum ImpiegoFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Indica che l'impiego non è più attivo
            /// </summary>
            Terminato = 1
        }

        /// <summary>
        /// Classe che descrive una relazione di impiego tra una persona ed un'azienda
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CImpiegato 
            : Databases.DBObject
        {
            /// <summary>
            /// Valore vuoto
            /// </summary>
            public static readonly CImpiegato Empty = new CImpiegato();

            private int m_PersonaID;
            [NonSerialized] private CPersonaFisica m_Persona;
            private string m_NomePersona;
            private int m_IDEntePagante;
            [NonSerialized] private CAzienda m_EntePagante;
            private string m_NomeEntePagante;
            private int m_AziendaID;
            [NonSerialized] private CAzienda m_Azienda;
            private string m_NomeAzienda;
            private int m_IDSede;
            [NonSerialized] private CUfficio m_Sede;
            private string m_NomeSede;
            private string m_Posizione;
            private string m_Ufficio;
            private DateTime? m_DataAssunzione;
            private DateTime? m_DataLicenziamento;
            private decimal? m_StipendioNetto;
            private decimal? m_StipendioLordo;
            private decimal? m_TFR;
            private int? m_MensilitaPercepite;
            private double? m_PercTFRAzienda;
            private string m_NomeFPC;
            private string m_TipoContratto;
            private string m_TipoRapporto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CImpiegato()
            {
                m_PersonaID = 0;
                m_Persona = null;
                m_AziendaID = 0;
                m_Azienda = null;
                m_NomeAzienda = "";
                m_IDSede = 0;
                m_Sede = null;
                m_NomeSede = "";
                m_Posizione = "";
                m_Ufficio = "";
                m_DataAssunzione = default;
                m_DataLicenziamento = default;
                m_StipendioNetto = default;
                m_StipendioLordo = default;
                m_TFR = default;
                m_MensilitaPercepite = default;
                m_PercTFRAzienda = default;
                m_NomeFPC = "";
                m_TipoContratto = "";
                m_TipoRapporto = "";
                m_IDEntePagante = 0;
                m_EntePagante = null;
                m_NomeEntePagante = "";
                m_Flags = (int) ImpiegoFlags.None;
                 
            }

            

            /// <summary>
            /// Restituisce o imposta dei falgs aggiuntivi
            /// </summary>
            /// <returns></returns>
            public new ImpiegoFlags Flags
            {
                get
                {
                    return (ImpiegoFlags)m_Flags;
                }

                set
                {
                    var oldValue = (ImpiegoFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'impiegato
            /// </summary>
            /// <returns></returns>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_PersonaID);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_Persona = null;
                    m_PersonaID = value;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'impiegato
            /// </summary>
            /// <returns></returns>
            public CPersonaFisica Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = (CPersonaFisica)minidom.Anagrafica.Persone.GetItemById(m_PersonaID);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_PersonaID = DBUtils.GetID(value, this.m_PersonaID);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona
            /// </summary>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azienda
            /// </summary>
            public CAzienda Azienda
            {
                get
                {
                    if (m_Azienda is null)
                        m_Azienda = Aziende.GetItemById(m_AziendaID);
                    return m_Azienda;
                }

                set
                {
                    var oldValue = m_Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Azienda = value;
                    m_AziendaID = DBUtils.GetID(value, this.m_AziendaID);
                    if (value is object)
                        m_NomeAzienda = value.Nominativo;
                    DoChanged("Azienda", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'azienda
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetAzienda(CAzienda value)
            {
                m_Azienda = value;
                m_AziendaID = DBUtils.GetID(value, this.m_AziendaID);
            }

            /// <summary>
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(CPersonaFisica value)
            {
                m_Persona = value;
                m_PersonaID = DBUtils.GetID(value, this.m_PersonaID);
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'azienda
            /// </summary>
            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID(m_Azienda, m_AziendaID);
                }

                set
                {
                    int oldValue = IDAzienda;
                    if (oldValue == value)
                        return;
                    m_Azienda = null;
                    m_AziendaID = value;
                    DoChanged("IDAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda
            /// </summary>
            public string NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAzienda;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAzienda = value;
                    DoChanged("NomeAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'ente principale
            /// </summary>
            public int IDEntePagante
            {
                get
                {
                    return DBUtils.GetID(m_EntePagante, m_IDEntePagante);
                }

                set
                {
                    int oldValue = IDEntePagante;
                    if (oldValue == value)
                        return;
                    m_IDEntePagante = value;
                    m_EntePagante = null;
                    DoChanged("IDEntePagante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ente principale
            /// </summary>
            public CAzienda EntePagante
            {
                get
                {
                    if (m_EntePagante is null)
                        m_EntePagante = Aziende.GetItemById(m_IDEntePagante);
                    return m_EntePagante;
                }

                set
                {
                    var oldValue = m_EntePagante;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_EntePagante = value;
                    m_IDEntePagante = DBUtils.GetID(value, this.m_IDEntePagante);
                    if (value is object)
                        m_NomeEntePagante = value.Nominativo;
                    DoChanged("EntePagante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda principale
            /// </summary>
            public string NomeEntePagante
            {
                get
                {
                    return m_NomeEntePagante;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeEntePagante;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeEntePagante = value;
                    DoChanged("NomeEntePagante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sede
            /// </summary>
            public CUfficio Sede
            {
                get
                {
                    if (m_Sede is null && Azienda is object)
                        m_Sede = Azienda.Uffici.GetItemById(m_IDSede);
                    return m_Sede;
                }

                set
                {
                    var oldValue = m_Sede;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Sede = value;
                    m_IDSede = DBUtils.GetID(value, this.m_IDSede);
                    m_NomeSede = "";
                    if (value is object)
                        m_NomeSede = value.Nome;
                    DoChanged("Sede", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della sede
            /// </summary>
            public int IDSede
            {
                get
                {
                    return DBUtils.GetID(m_Sede, m_IDSede);
                }

                set
                {
                    int oldValue = IDSede;
                    if (oldValue == value)
                        return;
                    m_Sede = null;
                    m_IDSede = value;
                    DoChanged("IDSede", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della sede
            /// </summary>
            public string NomeSede
            {
                get
                {
                    return m_NomeSede;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeSede;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSede = value;
                    DoChanged("NomeSede", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'ufficio
            /// </summary>
            public string Ufficio
            {
                get
                {
                    return m_Ufficio;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Ufficio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Ufficio = value;
                    DoChanged("Ufficio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la posizione occupata
            /// </summary>
            public string Posizione
            {
                get
                {
                    return m_Posizione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Posizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Posizione = value;
                    DoChanged("Posizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di assunzione
            /// </summary>
            public DateTime? DataAssunzione
            {
                get
                {
                    return m_DataAssunzione;
                }

                set
                {
                    var oldValue = m_DataAssunzione;
                    if (oldValue == value == true)
                        return;
                    m_DataAssunzione = value;
                    DoChanged("DataAssunzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di licenziamento
            /// </summary>
            public DateTime? DataLicenziamento
            {
                get
                {
                    return m_DataLicenziamento;
                }

                set
                {
                    var oldValue = m_DataLicenziamento;
                    if (oldValue == value == true)
                        return;
                    m_DataLicenziamento = value;
                    DoChanged("DataLicenziamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stipendio netto mensile
            /// </summary>
            public decimal? StipendioNetto
            {
                get
                {
                    return m_StipendioNetto;
                }

                set
                {
                    var oldValue = m_StipendioNetto;
                    if (oldValue == value == true)
                        return;
                    m_StipendioNetto = value;
                    DoChanged("StipendioNetto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stipendio lordo mensile
            /// </summary>
            public decimal? StipendioLordo
            {
                get
                {
                    return m_StipendioLordo;
                }

                set
                {
                    var oldValue = m_StipendioLordo;
                    if (oldValue == value == true)
                        return;
                    m_StipendioLordo = value;
                    DoChanged("StipendioLordo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tfr accumulato
            /// </summary>
            public decimal? TFR
            {
                get
                {
                    return m_TFR;
                }

                set
                {
                    var oldValue = m_TFR;
                    if (oldValue == value == true)
                        return;
                    m_TFR = value;
                    DoChanged("TFR", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di mensilità percepite (12, 13, 14, 15)
            /// </summary>
            public int? MensilitaPercepite
            {
                get
                {
                    return m_MensilitaPercepite;
                }

                set
                {
                    var oldValue = m_MensilitaPercepite;
                    if (oldValue == value == true)
                        return;
                    m_MensilitaPercepite = value;
                    DoChanged("MensilitaPercepite", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la percentuale del tfr tenuta in azeienda
            /// </summary>
            public double? PercTFRAzienda
            {
                get
                {
                    return m_PercTFRAzienda;
                }

                set
                {
                    var oldValue = m_PercTFRAzienda;
                    if (oldValue == value == true)
                        return;
                    m_PercTFRAzienda = value;
                    DoChanged("PercTFRAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la percentuale del tfr nel fondo pensionistico
            /// </summary>
            public double? PercTFRFPC
            {
                get
                {
                    if (m_PercTFRAzienda.HasValue)
                        return Maths.Max(0d, 100d - m_PercTFRAzienda.Value);
                    return default;
                }

                set
                {
                    var oldValue = PercTFRFPC;
                    if (oldValue == value == true)
                        return;
                    if (value.HasValue)
                    {
                        m_PercTFRAzienda = Maths.Max(0d, 100d - value.Value);
                    }
                    else
                    {
                        m_PercTFRAzienda = default;
                    }

                    DoChanged("PercTFRFPC", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del tfs accumulato tenuto in azienda
            /// </summary>
            public decimal? ValoreTFRAzienda
            {
                get
                {
                    if (m_TFR.HasValue && m_PercTFRAzienda.HasValue)
                        return (decimal?)((double)m_TFR.Value * m_PercTFRAzienda.Value / 100d);
                    return default;
                }

                set
                {
                    var oldValue = ValoreTFRAzienda;
                    if (oldValue == value == true)
                        return;
                    if (value.HasValue)
                    {
                        if (m_TFR.HasValue && m_TFR.Value > 0m)
                        {
                            m_PercTFRAzienda = (double?)(value.Value / m_TFR.Value * 100m);
                        }
                        else
                        {
                            m_PercTFRAzienda = default;
                        }
                    }
                    else
                    {
                        m_PercTFRAzienda = default;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del tfr accumulato nel fondo pensione
            /// </summary>
            public decimal? ValoreTFRFPC
            {
                get
                {
                    if (TFR.HasValue & ValoreTFRAzienda.HasValue)
                    {
                        return TFR.Value - ValoreTFRAzienda.Value;
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    if (value.HasValue)
                    {
                        if (TFR.HasValue)
                        {
                            ValoreTFRAzienda = TFR.Value - value.Value;
                        }
                        else
                        {
                            TFR = value;
                        }
                    }
                    else
                    {
                        ValoreTFRAzienda = default;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del fondo pensione
            /// </summary>
            public string NomeFPC
            {
                get
                {
                    return m_NomeFPC;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeFPC;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFPC = value;
                    DoChanged("NomeFPC", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la tipologia di contratto
            /// </summary>
            public string TipoContratto
            {
                get
                {
                    return m_TipoContratto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContratto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContratto = value;
                    DoChanged("TipoContratto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo rapporto
            /// </summary>
            public string TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoRapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoRapporto = value;
                    DoChanged("TipoRapporto", value, oldValue);
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("PersonaID", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("AziendaID", IDAzienda);
                writer.WriteAttribute("NomeAzienda", m_NomeAzienda);
                writer.WriteAttribute("IDSede", IDSede);
                writer.WriteAttribute("NomeSede", m_NomeSede);
                writer.WriteAttribute("Posizione", m_Posizione);
                writer.WriteAttribute("Ufficio", m_Ufficio);
                writer.WriteAttribute("DataAssunzione", m_DataAssunzione);
                writer.WriteAttribute("DataLicenziamento", m_DataLicenziamento);
                writer.WriteAttribute("StipendioNetto", m_StipendioNetto);
                writer.WriteAttribute("StipendioLordo", m_StipendioLordo);
                writer.WriteAttribute("TFR", m_TFR);
                writer.WriteAttribute("MensilitaPercepite", m_MensilitaPercepite);
                writer.WriteAttribute("PercTFRAzienda", m_PercTFRAzienda);
                writer.WriteAttribute("NomeFPC", m_NomeFPC);
                writer.WriteAttribute("TipoContratto", m_TipoContratto);
                writer.WriteAttribute("TipoRapporto", m_TipoRapporto);
                writer.WriteAttribute("IDEntePagante", IDEntePagante);
                writer.WriteAttribute("NomeEntePagante", m_NomeEntePagante);
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
                    case "PersonaID":
                        {
                            m_PersonaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AziendaID":
                        {
                            m_AziendaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAzienda":
                        {
                            m_NomeAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDSede":
                        {
                            m_IDSede = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeSede":
                        {
                            m_NomeSede = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Posizione":
                        {
                            m_Posizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Ufficio":
                        {
                            m_Ufficio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAssunzione":
                        {
                            m_DataAssunzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataLicenziamento":
                        {
                            m_DataLicenziamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StipendioNetto":
                        {
                            m_StipendioNetto = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "StipendioLordo":
                        {
                            m_StipendioLordo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TFR":
                        {
                            m_TFR = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MensilitaPercepite":
                        {
                            m_MensilitaPercepite = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PercTFRAzienda":
                        {
                            m_PercTFRAzienda = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeFPC":
                        {
                            m_NomeFPC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoContratto":
                        {
                            m_TipoContratto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoRapporto":
                        {
                            m_TipoRapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDEntePagante":
                        {
                            m_IDEntePagante = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeEntePagante":
                        {
                            m_NomeEntePagante = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Impiegati";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Impieghi;
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
                m_PersonaID = reader.Read("Persona",  m_PersonaID);
                m_NomePersona = reader.Read("NomePersona",  m_NomePersona);
                m_AziendaID = reader.Read("Azienda",  m_AziendaID);
                m_NomeAzienda = reader.Read("NomeAzienda",  m_NomeAzienda);
                m_IDSede = reader.Read("IDSede",  m_IDSede);
                m_NomeSede = reader.Read("NomeSede",  m_NomeSede);
                m_Ufficio = reader.Read("Ufficio",  m_Ufficio);
                m_Posizione = reader.Read("Posizione",  m_Posizione);
                m_DataAssunzione = reader.Read("DataAssunzione",  m_DataAssunzione);
                m_DataLicenziamento = reader.Read("DataLicenziamento",  m_DataLicenziamento);
                m_StipendioNetto = reader.Read("StipendioNetto",  m_StipendioNetto);
                m_StipendioLordo = reader.Read("StipendioLordo",  m_StipendioLordo);
                m_TFR = reader.Read("TFR",  m_TFR);
                m_MensilitaPercepite = reader.Read("MensilitaPercepite",  m_MensilitaPercepite);
                m_PercTFRAzienda = reader.Read("PercTFRAzienda",  m_PercTFRAzienda);
                m_NomeFPC = reader.Read("NomeFPC",  m_NomeFPC);
                m_TipoContratto = reader.Read("TipoContratto",  m_TipoContratto);
                m_TipoRapporto = reader.Read("TipoRapporto",  m_TipoRapporto);
                m_IDEntePagante = reader.Read("IDEntePagante",  m_IDEntePagante);
                m_NomeEntePagante = reader.Read("NomeEntePagante",  m_NomeEntePagante);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Persona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("Azienda", IDAzienda);
                writer.Write("NomeAzienda", m_NomeAzienda);
                writer.Write("IDSede", IDSede);
                writer.Write("NomeSede", m_NomeSede);
                writer.Write("Ufficio", m_Ufficio);
                writer.Write("Posizione", m_Posizione);
                writer.Write("DataAssunzione", m_DataAssunzione);
                writer.Write("DataLicenziamento", m_DataLicenziamento);
                writer.Write("StipendioNetto", m_StipendioNetto);
                writer.Write("StipendioLordo", m_StipendioLordo);
                writer.Write("TFR", m_TFR);
                writer.Write("MensilitaPercepite", m_MensilitaPercepite);
                writer.Write("PercTFRAzienda", m_PercTFRAzienda);
                writer.Write("NomeFPC", m_NomeFPC);
                writer.Write("TipoContratto", m_TipoContratto);
                writer.Write("TipoRapporto", m_TipoRapporto);
                writer.Write("IDEntePagante", IDEntePagante);
                writer.Write("NomeEntePagante", m_NomeEntePagante);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Persona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("IDEntePagante", typeof(int), 1);
                c = table.Fields.Ensure("NomeEntePagante", typeof(string), 255);
                c = table.Fields.Ensure("Azienda", typeof(int), 1);
                c = table.Fields.Ensure("NomeAzienda", typeof(string), 255);
                c = table.Fields.Ensure("IDSede", typeof(int), 1);
                c = table.Fields.Ensure("NomeSede", typeof(string), 255);
                c = table.Fields.Ensure("Ufficio", typeof(string), 255);
                c = table.Fields.Ensure("Posizione", typeof(string), 255);
                c = table.Fields.Ensure("DataAssunzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataLicenziamento", typeof(DateTime), 1);
                c = table.Fields.Ensure("StipendioNetto", typeof(Decimal), 1);
                c = table.Fields.Ensure("StipendioLordo", typeof(Decimal), 1);
                c = table.Fields.Ensure("TFR", typeof(Decimal), 1);
                c = table.Fields.Ensure("MensilitaPercepite", typeof(int), 1);
                c = table.Fields.Ensure("PercTFRAzienda", typeof(double), 1);
                c = table.Fields.Ensure("NomeFPC", typeof(string), 255);
                c = table.Fields.Ensure("TipoContratto", typeof(string), 255);
                c = table.Fields.Ensure("TipoRapporto", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPersona", new string[] { "Persona", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEnte", new string[] { "IDEntePagante", "NomeEntePagante" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAzienda", new string[] { "Azienda", "NomeAzienda" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSede", new string[] { "IDSede", "NomeSede", "Ufficio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFlags", new string[] { "Posizione", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContratto", new string[] { "TipoContratto", "TipoRapporto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRAL", new string[] { "MensilitaPercepite", "StipendioNetto", "StipendioLordo", "TFR" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataAssunzione", "DataLicenziamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFondo", new string[] { "PercTFRAzienda", "NomeFPC" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che descrive l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(
                    m_NomePersona , 
                    " impiegato presso " , m_NomeAzienda ,
                    " dal " ,
                    Sistema.Formats.FormatUserDate(m_DataAssunzione)
                    );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeAzienda, this.m_NomePersona, this.m_DataAssunzione, this.m_StipendioLordo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return base.Equals(obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CImpiegato obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.IDPersona, obj.IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.Integers.EQ(this.IDEntePagante, obj.IDEntePagante)
                    && DMD.Strings.EQ(this.m_NomeEntePagante, obj.m_NomeEntePagante)
                    && DMD.Integers.EQ(this.IDAzienda, obj.IDAzienda)
                    && DMD.Strings.EQ(this.m_NomeAzienda, obj.m_NomeAzienda)
                    && DMD.Integers.EQ(this.IDSede, obj.IDSede)
                    && DMD.Strings.EQ(this.m_NomeSede, obj.m_NomeSede)
                    && DMD.Strings.EQ(this.m_Posizione, obj.m_Posizione)
                    && DMD.Strings.EQ(this.m_Ufficio, obj.m_Ufficio)
                    && DMD.DateUtils.EQ(this.m_DataAssunzione, obj.m_DataAssunzione)
                    && DMD.DateUtils.EQ(this.m_DataLicenziamento, obj.m_DataLicenziamento)
                    && DMD.Decimals.EQ(this.m_StipendioNetto, obj.m_StipendioNetto)
                    && DMD.Decimals.EQ(this.m_StipendioLordo, obj.m_StipendioLordo)
                    && DMD.Decimals.EQ(this.m_TFR, obj.m_TFR)
                    && DMD.Integers.EQ(this.m_MensilitaPercepite, obj.m_MensilitaPercepite)
                    && DMD.Doubles.EQ(this.m_PercTFRAzienda, obj.m_PercTFRAzienda)
                    && DMD.Strings.EQ(this.m_NomeFPC, obj.m_NomeFPC)
                    && DMD.Strings.EQ(this.m_TipoContratto, obj.m_TipoContratto)
                    && DMD.Strings.EQ(this.m_TipoRapporto, obj.m_TipoRapporto)
                    && DMD.Doubles.EQ((int)this.m_Flags, (int)obj.m_Flags)
                    ;
            //private CKeyCollection m_Parameters;
            }

            /// <summary>
            /// Restituisce true se l'oggetto è uguale al valore vuoto
            /// </summary>
            /// <returns></returns>
            public bool IsEmpty()
            {
                return this.Equals(Empty);
            }

            /// <summary>
            /// Restituisce l'anzianità alla data odierna
            /// </summary>
            /// <returns></returns>
            public double? Anzianita()
            {
                return Anzianita(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Calcola l'anzianità alla data specifica
            /// </summary>
            /// <param name="al"></param>
            /// <returns></returns>
            public double? Anzianita(DateTime al)
            {
                return DMD. DateUtils.CalcolaAnzianita(this.m_DataAssunzione, DateUtils.Min(al, this.m_DataLicenziamento));
            }

            /// <summary>
            /// Unisce le informazioni dei due impieghi
            /// </summary>
            /// <param name="value"></param>
            public virtual void MergeWith(CImpiegato value)
            {
                if (m_PersonaID == 0)
                    m_PersonaID = value.m_PersonaID;
                if (m_Persona is null)
                    m_Persona = value.m_Persona;
                if (string.IsNullOrEmpty(m_NomePersona))
                    m_NomePersona = value.m_NomePersona;
                if (m_IDEntePagante == 0)
                    m_IDEntePagante = value.m_IDEntePagante;
                if (m_EntePagante is null)
                    m_EntePagante = value.m_EntePagante;
                if (string.IsNullOrEmpty(m_NomeEntePagante))
                    m_NomeEntePagante = value.m_NomeEntePagante;
                if (m_AziendaID == 0)
                    m_AziendaID = value.m_AziendaID;
                if (m_Azienda is null)
                    m_Azienda = value.m_Azienda;
                if (string.IsNullOrEmpty(m_NomeAzienda))
                    m_NomeAzienda = value.m_NomeAzienda;
                if (m_IDSede == 0)
                    m_IDSede = value.m_IDSede;
                if (m_Sede is null)
                    m_Sede = value.m_Sede;
                if (string.IsNullOrEmpty(m_NomeSede))
                    m_NomeSede = value.m_NomeSede;
                if (string.IsNullOrEmpty(m_Posizione))
                    m_Posizione = value.m_Posizione;
                if (string.IsNullOrEmpty(m_Ufficio))
                    m_Ufficio = value.m_Ufficio;
                if (m_DataAssunzione.HasValue == false)
                    m_DataAssunzione = value.m_DataAssunzione;
                if (m_DataLicenziamento.HasValue == false)
                    m_DataLicenziamento = value.m_DataLicenziamento;
                if (m_StipendioNetto.HasValue == false)
                    m_StipendioNetto = value.m_StipendioNetto;
                if (m_StipendioLordo.HasValue == false)
                    m_StipendioLordo = value.m_StipendioLordo;
                if (m_TFR.HasValue == false)
                    m_TFR = value.m_TFR;
                if (m_MensilitaPercepite.HasValue == false)
                    m_MensilitaPercepite = value.m_MensilitaPercepite;
                if (m_PercTFRAzienda.HasValue == false)
                    m_PercTFRAzienda = value.m_PercTFRAzienda;
                if (string.IsNullOrEmpty(m_NomeFPC))
                    m_NomeFPC = value.m_NomeFPC;
                if (string.IsNullOrEmpty(m_TipoContratto))
                    m_TipoContratto = value.m_TipoContratto;
                if (string.IsNullOrEmpty(m_TipoRapporto))
                    m_TipoRapporto = value.m_TipoRapporto;
                m_Flags = m_Flags | value.m_Flags;
                foreach(var k in value.Parameters.Keys )
                {
                    if (!this.Parameters.ContainsKey(k))
                        this.Parameters.SetItemByKey(k, value.Parameters[k]);
                }
            }
        }
    }
}