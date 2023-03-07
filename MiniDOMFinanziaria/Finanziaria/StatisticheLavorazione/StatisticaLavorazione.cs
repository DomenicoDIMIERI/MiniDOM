using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.Office;
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum CQSPDStatoLavorazioneLevel : int
        {
            NonIniziata = 0,
            Contatto = 10,
            Visita = 20,
            Consulenza = 30,
            Pratica = 40
        }

        public enum CQSPDStatoLavorazioneEsito : int
        {
            Nessuno = 0,
            InLavorazione = 10,
            Concluso = 20,
            Annullato = 30
        }

        [Serializable]
        public class StatisticaLavorazione 
            : IDMDXMLSerializable, IComparable
        {
            private int m_IDOperatoreContatto;
            [NonSerialized] private Sistema.CUser m_OperatoreContatto;
            private int m_IDConsulente;
            [NonSerialized] private CConsulentePratica m_Consulente;
            private int m_IDCollaboratore;
            [NonSerialized] private CCollaboratore m_Collaboratore;
            private int m_IDCliente;
            [NonSerialized] private Anagrafica.CPersonaFisica m_Cliente;
            private string m_NomeCliente;
            private string m_IconURL;
            private DateTime? m_InizioLavorazione;
            private DateTime? m_FineLavorazione;
            private DateTime? m_DataContatto;
            private DateTime? m_DataVisita;
            private DateTime? m_DataConsulenza;
            private DateTime? m_DataPratica;
            private CQSPDStatoLavorazioneLevel m_Livello;
            private CQSPDStatoLavorazioneEsito m_Esito;
            private string m_DettaglioEsito;
            internal CCollection<CContattoUtente> Contatti = new CCollection<CContattoUtente>();
            internal CCollection<CVisita> Visite = new CCollection<CVisita>();
            internal CCollection<CQSPDConsulenza> Consulenze = new CCollection<CQSPDConsulenza>();
            internal CCollection<CPraticaCQSPD> Pratiche = new CCollection<CPraticaCQSPD>();

            public StatisticaLavorazione()
            {
                DMDObject.IncreaseCounter(this);
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_IconURL = "";
                m_InizioLavorazione = default;
                m_FineLavorazione = default;
                m_DataContatto = default;
                m_DataVisita = default;
                m_DataConsulenza = default;
                m_DataPratica = default;
                m_Livello = CQSPDStatoLavorazioneLevel.NonIniziata;
                m_Esito = CQSPDStatoLavorazioneEsito.Nessuno;
                m_DettaglioEsito = "";
                m_IDOperatoreContatto = 0;
                m_OperatoreContatto = null;
                m_IDConsulente = 0;
                m_Consulente = null;
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }


            public int IDOperatoreContatto
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreContatto, m_IDOperatoreContatto);
                }

                set
                {
                    int oldValue = IDOperatoreContatto;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreContatto = value;
                    m_OperatoreContatto = null;
                    DoChanged("IDOperatoreContatto", value, oldValue);
                }
            }

            public Sistema.CUser OperatoreContatto
            {
                get
                {
                    if (m_OperatoreContatto is null)
                        m_OperatoreContatto = Sistema.Users.GetItemById(m_IDOperatoreContatto);
                    return m_OperatoreContatto;
                }

                set
                {
                    var oldValue = OperatoreContatto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OperatoreContatto = value;
                    m_IDOperatoreContatto = DBUtils.GetID(value);
                    DoChanged("OperatoreContatto", value, oldValue);
                }
            }

            public int IDConsulente
            {
                get
                {
                    return DBUtils.GetID(m_Consulente, m_IDConsulente);
                }

                set
                {
                    int oldValue = IDConsulente;
                    if (oldValue == value)
                        return;
                    m_IDConsulente = value;
                    m_Consulente = null;
                    DoChanged("IDConsulente", value, oldValue);
                }
            }

            public CConsulentePratica Consulente
            {
                get
                {
                    if (m_Consulente is null)
                        m_Consulente = Consulenti.GetItemById(m_IDConsulente);
                    return m_Consulente;
                }

                set
                {
                    var oldValue = Consulente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Consulente = value;
                    m_IDConsulente = DBUtils.GetID(value);
                    DoChanged("Consulente", value, oldValue);
                }
            }

            public int IDCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_Collaboratore, m_IDCollaboratore);
                }

                set
                {
                    int oldValue = IDCollaboratore;
                    if (oldValue == value)
                        return;
                    m_Collaboratore = null;
                    m_IDCollaboratore = value;
                    DoChanged("IDCollaboratore", value, oldValue);
                }
            }

            public CCollaboratore Collaboratore
            {
                get
                {
                    if (m_Collaboratore is null)
                        m_Collaboratore = Collaboratori.GetItemById(m_IDCollaboratore);
                    return m_Collaboratore;
                }

                set
                {
                    var oldValue = Collaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Collaboratore = value;
                    m_IDCollaboratore = DBUtils.GetID(value);
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            private void DoChanged(string propName, object newValue, object oldValue)
            {
            }

            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value);
                    m_NomeCliente = "";
                    m_IconURL = "";
                    if (value is object)
                    {
                        m_NomeCliente = value.Nominativo;
                        m_IconURL = value.IconURL;
                    }

                    DoChanged("Cliente", value, oldValue);
                }
            }

            protected internal void SetCliente(Anagrafica.CPersonaFisica value)
            {
                m_Cliente = value;
                m_IDCliente = DBUtils.GetID(value);
            }

            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    string oldValue = m_NomeCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    string oldValue = m_IconURL;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            public DateTime? InizioLavorazione
            {
                get
                {
                    return m_InizioLavorazione;
                }

                set
                {
                    var oldValue = m_InizioLavorazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_InizioLavorazione = value;
                    DoChanged("InizioLavorazione", value, oldValue);
                }
            }

            public DateTime? FineLavorazione
            {
                get
                {
                    return m_FineLavorazione;
                }

                set
                {
                    var oldValue = m_FineLavorazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_FineLavorazione = value;
                    DoChanged("FineLavorazione", value, oldValue);
                }
            }

            public DateTime? DataContatto
            {
                get
                {
                    return m_DataContatto;
                }

                set
                {
                    var oldValue = m_DataContatto;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataContatto = value;
                    DoChanged("DataContatto", value, oldValue);
                }
            }

            public DateTime? DataVisita
            {
                get
                {
                    return m_DataVisita;
                }

                set
                {
                    var oldValue = m_DataVisita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataVisita = value;
                    DoChanged("DataVisita", value, oldValue);
                }
            }

            public DateTime? DataConsulenza
            {
                get
                {
                    return m_DataConsulenza;
                }

                set
                {
                    var oldValue = m_DataConsulenza;
                    if (oldValue == value == true)
                        return;
                    m_DataConsulenza = value;
                    DoChanged("DataConsulenza", value, oldValue);
                }
            }

            public DateTime DataPratica
            {
                get
                {
                    return (DateTime)m_DataPratica;
                }

                set
                {
                    var oldValue = m_DataPratica;
                    if (oldValue == value == true)
                        return;
                    m_DataPratica = value;
                    DoChanged("DataPratica", value, oldValue);
                }
            }

            public CQSPDStatoLavorazioneLevel Livello
            {
                get
                {
                    return m_Livello;
                }

                set
                {
                    var oldValue = m_Livello;
                    if (oldValue == value)
                        return;
                    m_Livello = value;
                    DoChanged("Livello", value, oldValue);
                }
            }

            public CQSPDStatoLavorazioneEsito Esito
            {
                get
                {
                    return m_Esito;
                }

                set
                {
                    var oldValue = m_Esito;
                    if (oldValue == value)
                        return;
                    m_Esito = value;
                    DoChanged("Esito", value, oldValue);
                }
            }

            public string DettaglioEsito
            {
                get
                {
                    return m_DettaglioEsito;
                }

                set
                {
                    string oldValue = m_DettaglioEsito;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsito = value;
                    DoChanged("DettaglioEsito", value, oldValue);
                }
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "InizioLavorazione":
                        {
                            m_InizioLavorazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "FineLavorazione":
                        {
                            m_FineLavorazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataContatto":
                        {
                            m_DataContatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataVisita":
                        {
                            m_DataVisita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataConsulenza":
                        {
                            m_DataConsulenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataPratica":
                        {
                            m_DataPratica = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Livello":
                        {
                            m_Livello = (CQSPDStatoLavorazioneLevel)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            m_Esito = (CQSPDStatoLavorazioneEsito)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            m_DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOperatoreContatto":
                        {
                            m_IDOperatoreContatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDConsulente":
                        {
                            m_IDConsulente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("InizioLavorazione", m_InizioLavorazione);
                writer.WriteAttribute("FineLavorazione", m_FineLavorazione);
                writer.WriteAttribute("DataContatto", m_DataContatto);
                writer.WriteAttribute("DataVisita", m_DataVisita);
                writer.WriteAttribute("DataConsulenza", m_DataConsulenza);
                writer.WriteAttribute("DataPratica", m_DataPratica);
                writer.WriteAttribute("Livello", (int?)m_Livello);
                writer.WriteAttribute("Esito", (int?)m_Esito);
                writer.WriteAttribute("IDOperatoreContatto", IDOperatoreContatto);
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteTag("DettaglioEsito", m_DettaglioEsito);
            }

            ~StatisticaLavorazione()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int WorkingProgressPercentage
            {
                get
                {
                    // NonIniziata = 0
                    // Contatto = 10
                    // Visita = 20
                    // Consulenza = 30
                    // Pratica = 40
                    return (int)((int)Livello * 100 / 40d);
                }
            }

            internal DateTime? GetDataPrimaConsulenza()
            {
                DateTime? ret = default;
                foreach (CQSPDConsulenza c in Consulenze)
                    ret = DMD.DateUtils.Min(ret, c.DataConsulenza);
                return ret;
            }

            public void Update()
            {
                DataConsulenza = GetDataPrimaConsulenza();
                if (Pratiche.Count > 0)
                {
                    Livello = CQSPDStatoLavorazioneLevel.Pratica;
                    Esito = CQSPDStatoLavorazioneEsito.Concluso;
                    DettaglioEsito = "Pratica";
                }
                else if (Consulenze.Count > 0)
                {
                    Livello = CQSPDStatoLavorazioneLevel.Consulenza;
                    Esito = CQSPDStatoLavorazioneEsito.Concluso;
                    DettaglioEsito = "Consulenza";
                }
                else if (Visite.Count > 0)
                {
                    Livello = CQSPDStatoLavorazioneLevel.Visita;
                    Esito = CQSPDStatoLavorazioneEsito.Concluso;
                    DettaglioEsito = "Visita";
                }
                else if (Contatti.Count > 0)
                {
                    Livello = CQSPDStatoLavorazioneLevel.Contatto;
                    Esito = CQSPDStatoLavorazioneEsito.Concluso;
                    DettaglioEsito = "Contatto";
                }
                else
                {
                    Livello = CQSPDStatoLavorazioneLevel.NonIniziata;
                    Esito = CQSPDStatoLavorazioneEsito.Nessuno;
                    DettaglioEsito = "";
                }
            }

            public int CompareTo(StatisticaLavorazione obj)
            {
                return obj.WorkingProgressPercentage - WorkingProgressPercentage;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((StatisticaLavorazione)obj);
            }
        }
    }
}