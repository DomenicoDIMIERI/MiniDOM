using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoVerificaAmministrativa : int
        {
            DaVerificare = 0,
            ValutazioneInCorso = 1,
            ValutazioneConclusa = 2,
            Rimandata = 3
        }

        public enum EsitoVerificaAmministrativa : int
        {
            Sconosciuto = 0,
            Confermato = 1,
            NonConfermato = 2
        }

        [Serializable]
        public class VerificaAmministrativa 
            : Databases.DBObject
        {
            private int m_IDOperatore;
            [NonSerialized] private Sistema.CUser m_Operatore;
            private string m_NomeOperatore;
            private StatoVerificaAmministrativa m_StatoVerifica;
            private EsitoVerificaAmministrativa m_EsitoVerifica;
            private string m_DettaglioEsitoVerifica;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private int m_Flags;
            private int m_IDOggettoVerificato;
            private string m_TipoOggettoVerificato;
            private object m_OggettoVerificato;

            public VerificaAmministrativa()
            {
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_StatoVerifica = StatoVerificaAmministrativa.DaVerificare;
                m_EsitoVerifica = EsitoVerificaAmministrativa.Sconosciuto;
                m_DettaglioEsitoVerifica = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_Flags = default;
                m_IDOggettoVerificato = 0;
                m_TipoOggettoVerificato = "";
                m_OggettoVerificato = null;
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

            /// <summary>
        /// Restituisce o imposta lo stato della verifica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoVerificaAmministrativa StatoVerifica
            {
                get
                {
                    return m_StatoVerifica;
                }

                set
                {
                    var oldValue = m_StatoVerifica;
                    if (oldValue == value)
                        return;
                    m_StatoVerifica = value;
                    DoChanged("StatoVerifica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'esito della verifica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public EsitoVerificaAmministrativa EsitoVerifica
            {
                get
                {
                    return m_EsitoVerifica;
                }

                set
                {
                    var oldValue = m_EsitoVerifica;
                    if (oldValue == value)
                        return;
                    m_EsitoVerifica = value;
                    DoChanged("EsitoVerifica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il dettaglio dell'esito della verifica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DettaglioEsitoVerifica
            {
                get
                {
                    return m_DettaglioEsitoVerifica;
                }

                set
                {
                    string oldValue = m_DettaglioEsitoVerifica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsitoVerifica = value;
                    DoChanged("DettaglioEsitoVerifica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data e l'ora di inizio della verifica
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di conclusione della verifica
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto verificato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOggettoVerificato
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_OggettoVerificato, m_IDOggettoVerificato);
                }

                set
                {
                    int oldValue = IDOggettoVerificato;
                    if (oldValue == value)
                        return;
                    m_IDOggettoVerificato = value;
                    m_OggettoVerificato = null;
                    DoChanged("IDOggettoVerificato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo dell'oggetto verificato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoOggettoVerificato
            {
                get
                {
                    return m_TipoOggettoVerificato;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoOggettoVerificato;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoOggettoVerificato = value;
                    m_OggettoVerificato = null;
                    DoChanged("TipoOggettoVerificato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto verificato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public object OggettoVerificato
            {
                get
                {
                    if (m_OggettoVerificato is null)
                        m_OggettoVerificato = Sistema.Types.GetItemByTypeAndId(m_TipoOggettoVerificato, m_IDOggettoVerificato);
                    return m_OggettoVerificato;
                }

                set
                {
                    var oldValue = OggettoVerificato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDOggettoVerificato = DBUtils.GetID((Databases.IDBObjectBase)value);
                    if (value is null)
                    {
                        m_TipoOggettoVerificato = "";
                    }
                    else
                    {
                        m_TipoOggettoVerificato = DMD.RunTime.vbTypeName(value);
                    }

                    DoChanged("OggettoVerificato", value, oldValue);
                }
            }

            protected internal void SetOggettoVerificato(object value)
            {
                m_OggettoVerificato = value;
            }

            public override CModulesClass GetModule()
            {
                return VerificheAmministrative.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDVerificheAmministrative";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                this.m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                this.m_StatoVerifica = reader.Read("StatoVerifica", this.m_StatoVerifica);
                this.m_EsitoVerifica = reader.Read("EsitoVerifica", this.m_EsitoVerifica);
                this.m_DettaglioEsitoVerifica = reader.Read("DettaglioEsitoVerifica", this.m_DettaglioEsitoVerifica);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                this.m_Flags = reader.Read("Flags", this.m_Flags);
                this.m_IDOggettoVerificato = reader.Read("IDOggettoVerificato", this.m_IDOggettoVerificato);
                this.m_TipoOggettoVerificato = reader.Read("TipoOggettoVerificato", this.m_TipoOggettoVerificato);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDOperatore", this.IDOperatore);
                writer.Write("NomeOperatore", this.m_NomeOperatore);
                writer.Write("StatoVerifica", this.m_StatoVerifica);
                writer.Write("EsitoVerifica", this.m_EsitoVerifica);
                writer.Write("DettaglioEsitoVerifica", this.m_DettaglioEsitoVerifica);
                writer.Write("DataInizio", this.m_DataInizio);
                writer.Write("DataFine", this.m_DataFine);
                writer.Write("Flags", this.m_Flags);
                writer.Write("IDOggettoVerificato", this.IDOggettoVerificato);
                writer.Write("TipoOggettoVerificato", this.m_TipoOggettoVerificato);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", this.IDOperatore);
                writer.WriteAttribute("NomeOperatore", this.m_NomeOperatore);
                writer.WriteAttribute("StatoVerifica", this.m_StatoVerifica);
                writer.WriteAttribute("EsitoVerifica", this.m_EsitoVerifica);
                writer.WriteAttribute("DataInizio", this.m_DataInizio);
                writer.WriteAttribute("DataFine", this.m_DataFine);
                writer.WriteAttribute("Flags", this.m_Flags);
                writer.WriteAttribute("IDOggettoVerificato", this.IDOggettoVerificato);
                writer.WriteAttribute("TipoOggettoVerificato", this.m_TipoOggettoVerificato);
                base.XMLSerialize(writer);
                writer.WriteTag("DettaglioEsitoVerifica", this.m_DettaglioEsitoVerifica);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
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

                    case "StatoVerifica":
                        {
                            m_StatoVerifica = (StatoVerificaAmministrativa)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "EsitoVerifica":
                        {
                            m_EsitoVerifica = (EsitoVerificaAmministrativa)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioEsitoVerifica":
                        {
                            m_DettaglioEsitoVerifica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOggettoVerificato":
                        {
                            m_IDOggettoVerificato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoOggettoVerificato":
                        {
                            m_TipoOggettoVerificato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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