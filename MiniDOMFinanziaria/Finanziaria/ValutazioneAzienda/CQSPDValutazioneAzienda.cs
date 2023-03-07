using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Flags]
        public enum CQSPDValutazioneAziendaFlags : int
        {
            None = 0,
            CQS_Disponibile = 1,
            PD_Disponibile = 2
        }

        [Serializable]
        public class CQSPDValutazioneAzienda : Databases.DBObject
        {
            private int m_IDAzienda;
            [NonSerialized] private Anagrafica.CAzienda m_Azienda;
            private string m_NomeAzienda;
            private int m_IDOperatore;
            [NonSerialized] private Sistema.CUser m_Operatore;
            private string m_NomeOperatore;
            private string m_TipoFonte;
            private int m_IDFonte;
            [NonSerialized] private IFonte m_Fonte;
            private string m_NomeFonte;
            private decimal? m_CapitaleSociale;
            private int? m_NumeroDipendenti;
            private decimal? m_FatturatoAnnuo;
            private decimal? m_RapportoTFR_VN;
            private int? m_Rating;
            private DateTime? m_DataRevisione;
            private DateTime? m_DataScadenzaRevisione;
            private string m_StatoAzienda;
            private string m_DettaglioStatoAzienda;
            private int m_GiorniAnticipoEstinzione; // L'amministrazione accetta di estinguere prima del 40% di n giorni
            private CQSPDValutazioneAziendaFlags m_Flags;
            private CKeyCollection m_Parameters;
            [NonSerialized] private CCollection<CQSPDValutazioneAssicurazione> m_Assicurazioni;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CQSPDValutazioneAzienda()
            {
                m_IDAzienda = 0;
                m_Azienda = null;
                m_NomeAzienda = "";
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_TipoFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = "";
                m_CapitaleSociale = default;
                m_NumeroDipendenti = default;
                m_FatturatoAnnuo = default;
                m_RapportoTFR_VN = default;
                m_Rating = default;
                m_DataRevisione = default;
                m_DataScadenzaRevisione = default;
                m_StatoAzienda = null;
                m_DettaglioStatoAzienda = null;
                m_Parameters = null;
                m_GiorniAnticipoEstinzione = 0;
                m_Flags = CQSPDValutazioneAziendaFlags.None;
                m_Assicurazioni = null;
            }

            public CCollection<CQSPDValutazioneAssicurazione> Assicurazioni
            {
                get
                {
                    if (m_Assicurazioni is null)
                        m_Assicurazioni = new CCollection<CQSPDValutazioneAssicurazione>();
                    return m_Assicurazioni;
                }
            }

            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID(m_Azienda, m_IDAzienda);
                }

                set
                {
                    int oldValue = IDAzienda;
                    if (oldValue == value)
                        return;
                    m_IDAzienda = value;
                    m_Azienda = null;
                    DoChanged("IDAzienda", value, oldValue);
                }
            }

            public Anagrafica.CAzienda Azienda
            {
                get
                {
                    if (m_Azienda is null)
                        m_Azienda = Anagrafica.Aziende.GetItemById(m_IDAzienda);
                    return m_Azienda;
                }

                set
                {
                    var oldValue = m_Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Azienda = value;
                    m_IDAzienda = DBUtils.GetID(value);
                    m_NomeAzienda = "";
                    if (value is object)
                        m_NomeAzienda = value.Nominativo;
                    DoChanged("Azienda", value, oldValue);
                }
            }

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
        /// Restituisce o imposta l'ID dell'operatore che ha effettuato la verifica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Restituisce o imposta l'utente che ha effettuato la verifica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
                    m_IDOperatore = DBUtils.GetID(value);
                    m_Operatore = value;
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'operatore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    m_Fonte = null;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

            public int IDFonte
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Fonte, m_IDFonte);
                }

                set
                {
                    int oldValue = IDFonte;
                    if (oldValue == value)
                        return;
                    m_IDFonte = value;
                    m_Fonte = null;
                    DoChanged("IDFonte", value, oldValue);
                }
            }

            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null)
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_TipoFonte, m_TipoFonte, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    var oldValue = Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID((Databases.IDBObjectBase)value);
                    m_NomeFonte = "";
                    if (value is object)
                        m_NomeFonte = value.Nome;
                    DoChanged("Fonte", value, oldValue);
                }
            }

            public string NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFonte = value;
                    DoChanged("NomeFonte", value, oldValue);
                }
            }

            public decimal? CapitaleSociale
            {
                get
                {
                    return m_CapitaleSociale;
                }

                set
                {
                    var oldValue = m_CapitaleSociale;
                    if (oldValue == value == true)
                        return;
                    m_CapitaleSociale = value;
                    DoChanged("CapitaleSociale", value, oldValue);
                }
            }

            public int? NumeroDipendenti
            {
                get
                {
                    return m_NumeroDipendenti;
                }

                set
                {
                    var oldValue = m_NumeroDipendenti;
                    if (oldValue == value == true)
                        return;
                    m_NumeroDipendenti = value;
                    DoChanged("NumeroDipendenti", value, oldValue);
                }
            }

            public decimal? FatturatoAnnuo
            {
                get
                {
                    return m_FatturatoAnnuo;
                }

                set
                {
                    var oldValue = m_FatturatoAnnuo;
                    if (oldValue == value == true)
                        return;
                    m_FatturatoAnnuo = value;
                    DoChanged("FatturatoAnnuo", value, oldValue);
                }
            }

            public decimal? RapportoTFR_VN
            {
                get
                {
                    return m_RapportoTFR_VN;
                }

                set
                {
                    var oldValue = m_RapportoTFR_VN;
                    if (oldValue == value == true)
                        return;
                    m_RapportoTFR_VN = value;
                    DoChanged("RapportoTFR_VN", value, oldValue);
                }
            }

            public int? Rating
            {
                get
                {
                    return m_Rating;
                }

                set
                {
                    int oldValue = (int)m_Rating;
                    if (oldValue == value == true)
                        return;
                    m_Rating = value;
                    DoChanged("Rating", value, oldValue);
                }
            }

            public DateTime? DataRevisione
            {
                get
                {
                    return m_DataRevisione;
                }

                set
                {
                    var oldValue = m_DataRevisione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRevisione = value;
                    DoChanged("DataRevisione", value, oldValue);
                }
            }

            public DateTime? DataScadenzaRevisione
            {
                get
                {
                    return m_DataScadenzaRevisione;
                }

                set
                {
                    var oldValue = m_DataScadenzaRevisione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataScadenzaRevisione = value;
                    DoChanged("DataScadenzaRevisione", value, oldValue);
                }
            }

            public string StatoAzienda
            {
                get
                {
                    return m_StatoAzienda;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_StatoAzienda;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StatoAzienda = value;
                    DoChanged("StatoAzienda", value, oldValue);
                }
            }

            public string DettaglioStatoAzienda
            {
                get
                {
                    return m_DettaglioStatoAzienda;
                }

                set
                {
                    string oldValue = m_DettaglioStatoAzienda;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStatoAzienda = value;
                    DoChanged("DettaglioStatoAzienda", value, oldValue);
                }
            }

            /// <summary>
        /// L'amministrazione accetta di estinguere prima del 40% di n giorni
        /// </summary>
        /// <returns></returns>
            public int GiorniAnticipoEstinzione
            {
                get
                {
                    return m_GiorniAnticipoEstinzione;
                }

                set
                {
                    int oldValue = m_GiorniAnticipoEstinzione;
                    if (oldValue == value)
                        return;
                    m_GiorniAnticipoEstinzione = value;
                    DoChanged("GiorniAnticipoEstinzione", value, oldValue);
                }
            }

            public CQSPDValutazioneAziendaFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            public CKeyCollection Parameters
            {
                get
                {
                    if (m_Parameters is null)
                        m_Parameters = new CKeyCollection();
                    return m_Parameters;
                }
            }

            public override CModulesClass GetModule()
            {
                return ValutazioniAzienda.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDValutazioniAzienda";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDAzienda = reader.Read("IDAzienda", this.m_IDAzienda);
                m_NomeAzienda = reader.Read("NomeAzienda", this.m_NomeAzienda);
                m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                m_TipoFonte = reader.Read("TipoFonte", this.m_TipoFonte);
                m_IDFonte = reader.Read("IDFonte", this.m_IDFonte);
                m_NomeFonte = reader.Read("NomeFonte", this.m_NomeFonte);
                m_CapitaleSociale = reader.Read("CapitaleSociale", this.m_CapitaleSociale);
                m_NumeroDipendenti = reader.Read("NumeroDipendenti", this.m_NumeroDipendenti);
                m_FatturatoAnnuo = reader.Read("FatturatoAnnuo", this.m_FatturatoAnnuo);
                m_RapportoTFR_VN = reader.Read("RapportoTFR_VN", this.m_RapportoTFR_VN);
                m_Rating = reader.Read("Rating", this.m_Rating);
                m_DataRevisione = reader.Read("DataRevisione", this.m_DataRevisione);
                m_DataScadenzaRevisione = reader.Read("DataScadenzaRevisione", this.m_DataScadenzaRevisione);
                m_StatoAzienda = reader.Read("StatoAzienda", this.m_StatoAzienda);
                m_DettaglioStatoAzienda = reader.Read("DettaglioStatoAzienda", this.m_DettaglioStatoAzienda);
                m_GiorniAnticipoEstinzione = reader.Read("GiorniAnticipoEstinzione", this.m_GiorniAnticipoEstinzione);
                m_Flags = reader.Read("Flags", this.m_Flags);
                string tmp = reader.Read("Parameters", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

                tmp = reader.Read("Assicurazioni", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.m_Assicurazioni = new CCollection<CQSPDValutazioneAssicurazione>();
                    this.m_Assicurazioni.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                }

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDAzienda", IDAzienda);
                writer.Write("NomeAzienda", m_NomeAzienda);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("NomeFonte", m_NomeFonte);
                writer.Write("CapitaleSociale", m_CapitaleSociale);
                writer.Write("NumeroDipendenti", m_NumeroDipendenti);
                writer.Write("FatturatoAnnuo", m_FatturatoAnnuo);
                writer.Write("RapportoTFR_VN", m_RapportoTFR_VN);
                writer.Write("Rating", m_Rating);
                writer.Write("DataRevisione", m_DataRevisione);
                writer.Write("DataScadenzaRevisione", m_DataScadenzaRevisione);
                writer.Write("StatoAzienda", m_StatoAzienda);
                writer.Write("DettaglioStatoAzienda", m_DettaglioStatoAzienda);
                writer.Write("GiorniAnticipoEstinzione", m_GiorniAnticipoEstinzione);
                writer.Write("Flags", m_Flags);
                writer.Write("Parameters", DMD.XML.Utils.Serializer.Serialize(Parameters));
                writer.Write("Assicurazioni", DMD.XML.Utils.Serializer.Serialize(Assicurazioni));
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDAzienda", IDAzienda);
                writer.WriteAttribute("NomeAzienda", m_NomeAzienda);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("NomeFonte", m_NomeFonte);
                writer.WriteAttribute("CapitaleSociale", m_CapitaleSociale);
                writer.WriteAttribute("NumeroDipendenti", m_NumeroDipendenti);
                writer.WriteAttribute("FatturatoAnnuo", m_FatturatoAnnuo);
                writer.WriteAttribute("RapportoTFR_VN", m_RapportoTFR_VN);
                writer.WriteAttribute("Rating", m_Rating);
                writer.WriteAttribute("DataRevisione", m_DataRevisione);
                writer.WriteAttribute("DataScadenzaRevisione", m_DataScadenzaRevisione);
                writer.WriteAttribute("StatoAzienda", m_StatoAzienda);
                writer.WriteAttribute("DettaglioStatoAzienda", m_DettaglioStatoAzienda);
                writer.WriteAttribute("GiorniAnticipoEstinzione", m_GiorniAnticipoEstinzione);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("Parameters", Parameters);
                writer.WriteTag("Assicurazioni", Assicurazioni);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAzienda":
                        {
                            m_NomeAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "TipoFonte":
                        {
                            m_TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            m_IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeFonte":
                        {
                            m_NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CapitaleSociale":
                        {
                            m_CapitaleSociale = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NumeroDipendenti":
                        {
                            m_NumeroDipendenti = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "FatturatoAnnuo":
                        {
                            m_FatturatoAnnuo = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RapportoTFR_VN":
                        {
                            m_RapportoTFR_VN = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rating":
                        {
                            m_Rating = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRevisione":
                        {
                            m_DataRevisione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataScadenzaRevisione":
                        {
                            m_DataScadenzaRevisione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (CQSPDValutazioneAziendaFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoAzienda":
                        {
                            m_StatoAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioStatoAzienda":
                        {
                            m_DettaglioStatoAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "GiorniAnticipoEstinzione":
                        {
                            m_GiorniAnticipoEstinzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Parameters":
                        {
                            m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "Assicurazioni":
                        {
                            m_Assicurazioni = new CCollection<CQSPDValutazioneAssicurazione>();
                            m_Assicurazioni.AddRange((IEnumerable)DMD.XML.Utils.Serializer.ToObject(fieldValue));
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