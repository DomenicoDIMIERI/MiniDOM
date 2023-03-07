using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CQSPDConvenzione : Databases.DBObject, IComparable
        {
            private string m_Nome;
            private int m_IDProdotto;
            private CCQSPDProdotto m_Prodotto;
            private string m_NomeProdotto;
            private bool m_Attiva;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private decimal? m_MinimoCaricabile;
            private double? m_MinimoCaricabileP;
            private int m_IDAmministrazione;
            private Anagrafica.CAzienda m_Amministrazione;
            private string m_NomeAmministrazione;
            private string m_TipoRapporto;
            private AziendaXConvenzioneCollection m_Aziende;

            public CQSPDConvenzione()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_IDProdotto = 0;
                m_Prodotto = null;
                m_NomeProdotto = DMD.Strings.vbNullString;
                m_Attiva = true;
                m_DataInizio = default;
                m_DataFine = default;
                m_MinimoCaricabile = default;
                m_MinimoCaricabileP = default;
                m_IDAmministrazione = 0;
                m_Amministrazione = null;
                m_NomeAmministrazione = "";
                m_TipoRapporto = "";
                m_Aziende = null;
            }

            public AziendaXConvenzioneCollection Aziende
            {
                get
                {
                    if (m_Aziende is null)
                        m_Aziende = new AziendaXConvenzioneCollection(this);
                    return m_Aziende;
                }
            }

            internal void InvalidateAziende()
            {
                m_Aziende = null;
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'amministrazione
        /// </summary>
        /// <returns></returns>
            public int IDAmministrazione
            {
                get
                {
                    return DBUtils.GetID(m_Amministrazione, m_IDAmministrazione);
                }

                set
                {
                    int oldValue = IDAmministrazione;
                    if (oldValue == value)
                        return;
                    m_IDAmministrazione = value;
                    m_Amministrazione = null;
                    DoChanged("IDAmministrazione", value, oldValue);
                }
            }

            public Anagrafica.CAzienda Amministrazione
            {
                get
                {
                    if (m_Amministrazione is null)
                        m_Amministrazione = Anagrafica.Aziende.GetItemById(m_IDAmministrazione);
                    return m_Amministrazione;
                }

                set
                {
                    var oldValue = Amministrazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Amministrazione = value;
                    m_IDAmministrazione = DBUtils.GetID(value);
                    m_NomeAmministrazione = "";
                    if (value is object)
                        m_NomeAmministrazione = value.Nominativo;
                    DoChanged("Amministrazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'amministrazione
        /// </summary>
        /// <returns></returns>
            public string NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAmministrazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAmministrazione = value;
                    DoChanged("NomeAmministrazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo rapporto per cui è valida la convenzione.
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta il nome della convenzione
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del prodotto in convenzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDProdotto
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_IDProdotto);
                }

                set
                {
                    int oldValue = IDProdotto;
                    if (oldValue == value)
                        return;
                    m_IDProdotto = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il prodotto in convenzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDProdotto Prodotto
            {
                get
                {
                    if (m_Prodotto is null)
                        m_Prodotto = Prodotti.GetItemById(m_IDProdotto);
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = m_Prodotto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Prodotto = value;
                    m_IDProdotto = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeProdotto = value.Nome;
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del prodotto in convenzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeProdotto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProdotto = value;
                    DoChanged("NomeProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la convenzione è attiva oppure no
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
                    if (value == m_Attiva)
                        return;
                    m_Attiva = value;
                    DoChanged("Attiva", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di inizio della convenzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di fine della convenzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore minimo caricabile (in euro)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? MinimoCaricabile
            {
                get
                {
                    return m_MinimoCaricabile;
                }

                set
                {
                    var oldValue = m_MinimoCaricabile;
                    if (oldValue == value == true)
                        return;
                    m_MinimoCaricabile = value;
                    DoChanged("MinimoCaricabile", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il minimo caricabile (in % rispetto al montante lordo)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double? MinimoCaricabileP
            {
                get
                {
                    return m_MinimoCaricabileP;
                }

                set
                {
                    double? oldValue = (double?)m_MinimoCaricabile;
                    if (oldValue == value == true)
                        return;
                    m_MinimoCaricabile = (decimal?)value;
                    DoChanged("MinimoCaricabile", value, oldValue);
                }
            }

            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            public bool IsValid(DateTime at)
            {
                return DMD.DateUtils.CheckBetween(at, m_DataInizio, m_DataFine);
            }

            public override CModulesClass GetModule()
            {
                return Convenzioni.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDConvenzioni";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Attiva = reader.Read("Attiva", m_Attiva);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_IDProdotto = reader.Read("IDProdotto", m_IDProdotto);
                m_MinimoCaricabile = reader.Read("MinimoCaricabile", m_MinimoCaricabile);
                m_MinimoCaricabileP = reader.Read("SogliaMinimaP", m_MinimoCaricabileP);
                m_Nome = reader.Read("Nome", m_Nome);
                m_NomeProdotto = reader.Read("NomeProdotto", m_NomeProdotto);
                m_IDAmministrazione = reader.Read("IDAmministrazione", m_IDAmministrazione);
                m_NomeAmministrazione = reader.Read("NomeAmministrazione", m_NomeAmministrazione);
                m_TipoRapporto = reader.Read("TipoRapporto", m_TipoRapporto);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Attiva", m_Attiva);
                writer.Write("DataFine", m_DataFine);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("IDProdotto", IDProdotto);
                writer.Write("MinimoCaricabile", m_MinimoCaricabile);
                writer.Write("SogliaMinimaP", m_MinimoCaricabileP);
                writer.Write("Nome", m_Nome);
                writer.Write("NomeProdotto", m_NomeProdotto);
                writer.Write("IDAmministrazione", IDAmministrazione);
                writer.Write("NomeAmministrazione", m_NomeAmministrazione);
                writer.Write("TipoRapporto", m_TipoRapporto);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Attiva", m_Attiva);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("IDProdotto", IDProdotto);
                writer.WriteAttribute("MinimoCaricabile", m_MinimoCaricabile);
                writer.WriteAttribute("MinimoCaricabileP", m_MinimoCaricabileP);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("NomeProdotto", m_NomeProdotto);
                writer.WriteAttribute("IDAmministrazione", IDAmministrazione);
                writer.WriteAttribute("NomeAmministrazione", m_NomeAmministrazione);
                writer.WriteAttribute("TipoRapporto", m_TipoRapporto);
                base.XMLSerialize(writer);
                writer.WriteTag("Aziende", Aziende);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Attiva":
                        {
                            m_Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDProdotto":
                        {
                            m_IDProdotto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MinimoCaricabile":
                        {
                            m_MinimoCaricabile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "MinimoCaricabileP":
                        {
                            m_MinimoCaricabileP = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeProdotto":
                        {
                            m_NomeProdotto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAmministrazione":
                        {
                            m_IDAmministrazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAmministrazione":
                        {
                            m_NomeAmministrazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoRapporto":
                        {
                            m_TipoRapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Aziende":
                        {
                            m_Aziende = (AziendaXConvenzioneCollection)fieldValue;
                            m_Aziende.SetConvenzione(this);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CQSPDConvenzione)obj);
            }

            public int CompareTo(CQSPDConvenzione obj)
            {
                int a1 = Sistema.IIF(Attiva, 1, 0);
                int a2 = Sistema.IIF(obj.Attiva, 1, 0);
                int ret = a1 - a2;
                if (ret == 0)
                    ret = DMD.Strings.Compare(Nome, obj.Nome, true);
                return ret;
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                Convenzioni.UpdateCached(this);
            }
        }
    }
}