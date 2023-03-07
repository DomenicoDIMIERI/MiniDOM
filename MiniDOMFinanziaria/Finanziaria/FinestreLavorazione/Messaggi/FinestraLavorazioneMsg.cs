/* TODO ERROR: Skipped DefineDirectiveTrivia */
using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum FinestraLavorazioneMsgTipoDst : int
        {
            /// <summary>
        /// Messaggio destinato al cliente
        /// </summary>
        /// <remarks></remarks>
            Cliente = 0,

            /// <summary>
        /// Messaggio destinato all'operatore CRM
        /// </summary>
        /// <remarks></remarks>
            Operatore = 1,

            /// <summary>
        /// Messaggio destinato al consulente
        /// </summary>
        /// <remarks></remarks>
            Consulente = 2
        }

        [Serializable]
        public class FinestraLavorazioneMsg : Databases.DBObjectBase, IComparable
        {
            private DateTime m_DataInvio;
            private int m_IDOperatoreInvio;
            [NonSerialized]
            private Sistema.CUser m_OperatoreInvio;
            private string m_NomeOperatoreInvio;
            private DateTime? m_DataRicezione;
            private DateTime? m_DataLettura;
            private string m_Messaggio;
            private int m_Flags;
            private FinestraLavorazioneMsgTipoDst m_TipoDestinatario;

            public FinestraLavorazioneMsg()
            {
                m_DataInvio = default;
                m_IDOperatoreInvio = 0;
                m_OperatoreInvio = null;
                m_NomeOperatoreInvio = "";
                m_DataRicezione = default;
                m_DataLettura = default;
                m_Messaggio = "";
                m_Flags = 0;
                m_TipoDestinatario = FinestraLavorazioneMsgTipoDst.Cliente;
            }

            public DateTime DataInvio
            {
                get
                {
                    return m_DataInvio;
                }

                set
                {
                    var oldValue = m_DataInvio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInvio = value;
                    DoChanged("DataInvio", value, oldValue);
                }
            }

            public int IDOperatoreInvio
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreInvio, m_IDOperatoreInvio);
                }

                set
                {
                    int oldValue = IDOperatoreInvio;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreInvio = value;
                    m_OperatoreInvio = null;
                    DoChanged("IDOperatoreInvio", value, oldValue);
                }
            }

            public Sistema.CUser OperatoreInvio
            {
                get
                {
                    if (m_OperatoreInvio is null)
                        m_OperatoreInvio = Sistema.Users.GetItemById(m_IDOperatoreInvio);
                    return m_OperatoreInvio;
                }

                set
                {
                    var oldValue = OperatoreInvio;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_OperatoreInvio = value;
                    m_IDOperatoreInvio = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeOperatoreInvio = value.Nominativo;
                    DoChanged("OperatoreInvio", value, oldValue);
                }
            }

            public string NomeOperatoreInvio
            {
                get
                {
                    return m_NomeOperatoreInvio;
                }

                set
                {
                    string oldValue = m_NomeOperatoreInvio;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreInvio = value;
                    DoChanged("NomeOperatoreInvio", value, oldValue);
                }
            }

            public DateTime? DataRicezione
            {
                get
                {
                    return m_DataRicezione;
                }

                set
                {
                    DateTime oldValue = (DateTime)m_DataRicezione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRicezione = value;
                    DoChanged("DataRicezione", value, oldValue);
                }
            }

            public DateTime? DataLettura
            {
                get
                {
                    return m_DataLettura;
                }

                set
                {
                    var oldValue = m_DataLettura;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataLettura = value;
                    DoChanged("DataLettura", value, oldValue);
                }
            }

            public string Messaggio
            {
                get
                {
                    return m_Messaggio;
                }

                set
                {
                    string oldValue = m_Messaggio;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Messaggio = value;
                    DoChanged("Messaggio", value, oldValue);
                }
            }

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

            public FinestraLavorazioneMsgTipoDst TipoDestinatario
            {
                get
                {
                    return m_TipoDestinatario;
                }

                set
                {
                    var oldValue = m_TipoDestinatario;
                    if (oldValue == value)
                        return;
                    m_TipoDestinatario = value;
                    DoChanged("TipoDestinatario", value, oldValue);
                }
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataInvio", m_DataInvio);
                writer.WriteAttribute("IDOperatoreInvio", IDOperatoreInvio);
                writer.WriteAttribute("NomeOperatoreInvio", m_NomeOperatoreInvio);
                writer.WriteAttribute("DataRicezione", m_DataRicezione);
                writer.WriteAttribute("DataLettura", m_DataLettura);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("TipoDestinatario", (int?)m_TipoDestinatario);
                base.XMLSerialize(writer);
                writer.WriteTag("Messaggio", m_Messaggio);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "DataInvio":
                        {
                            m_DataInvio = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatoreInvio":
                        {
                            m_IDOperatoreInvio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreInvio":
                        {
                            m_NomeOperatoreInvio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRicezione":
                        {
                            m_DataRicezione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataLettura":
                        {
                            m_DataLettura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Messaggio":
                        {
                            m_Messaggio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoDestinatario":
                        {
                            m_TipoDestinatario = (FinestraLavorazioneMsgTipoDst)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_FinestraLavorazioneMsgs";
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataInvio", m_DataInvio);
                writer.Write("IDOperatoreInvio", IDOperatoreInvio);
                writer.Write("NomeOperatoreInvio", m_NomeOperatoreInvio);
                writer.Write("DataRicezione", m_DataRicezione);
                writer.Write("DataLettura", m_DataLettura);
                writer.Write("Messaggio", m_Messaggio);
                writer.Write("Flags", m_Flags);
                writer.Write("TipoDestinatario", m_TipoDestinatario);
                return base.SaveToRecordset(writer);
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_DataInvio = reader.Read("DataInvio", this.m_DataInvio);
                this.m_IDOperatoreInvio = reader.Read("IDOperatoreInvio", this.m_IDOperatoreInvio);
                this.m_NomeOperatoreInvio = reader.Read("NomeOperatoreInvio", this.m_NomeOperatoreInvio);
                this.m_DataRicezione = reader.Read("DataRicezione", this.m_DataRicezione);
                this.m_DataLettura = reader.Read("DataLettura", this.m_DataLettura);
                this.m_Messaggio = reader.Read("Messaggio", this.m_Messaggio);
                this.m_Flags = reader.Read("Flags", this.m_Flags);
                this.m_TipoDestinatario = reader.Read("TipoDestinatario", this.m_TipoDestinatario);
                return base.LoadFromRecordset(reader);
            }

            public int CompareTo(FinestraLavorazioneMsg obj)
            {
                return DMD.DateUtils.Compare(this.m_DataInvio, obj.m_DataInvio);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((FinestraLavorazioneMsg)obj);
            }
        }
    }
}