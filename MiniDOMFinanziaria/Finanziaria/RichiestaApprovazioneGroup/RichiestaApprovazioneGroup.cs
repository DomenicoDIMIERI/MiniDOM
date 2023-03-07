using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Oggetto per richiesta approvazione
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class RichiestaApprovazioneGroup : Databases.DBObjectPO, ICloneable
        {
            private int m_IDCliente;
            private Anagrafica.CPersona m_Cliente;
            private string m_NomeCliente;
            private DateTime m_DataRichiesta;
            private int m_IDRichiedente;
            private Sistema.CUser m_Richiedente;
            private string m_NomeRichiedente;
            private int m_IDMotivoRichiesta;
            private CMotivoScontoPratica m_MotivoRichiesta;
            private string m_Motivo;
            private string m_DettaglioRichiesta;
            private int m_IDSupervisore;
            private Sistema.CUser m_Supervisore;
            private DateTime? m_DataEsito;
            private RichiesteApprovazioneCollection m_Richieste;

            public RichiestaApprovazioneGroup()
            {
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_DataRichiesta = DMD.DateUtils.Now();
                m_IDRichiedente = 0;
                m_Richiedente = null;
                m_NomeRichiedente = "";
                m_IDMotivoRichiesta = 0;
                m_MotivoRichiesta = null;
                m_Motivo = "";
                m_DettaglioRichiesta = "";
                m_IDSupervisore = 0;
                m_Supervisore = null;
                m_DataEsito = default;
                m_Richieste = null;
            }

            public RichiesteApprovazioneCollection Richieste
            {
                get
                {
                    if (m_Richieste is null)
                        m_Richieste = new RichiesteApprovazioneCollection(this);
                    return m_Richieste;
                }
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

            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
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
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            protected internal void SetCliente(Anagrafica.CPersona p)
            {
                m_Cliente = p;
                m_IDCliente = DBUtils.GetID(p);
            }

            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            public DateTime DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }

                set
                {
                    var oldValue = m_DataRichiesta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiesta = value;
                    DoChanged("DataRichiesta", value, oldValue);
                }
            }

            public int IDRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_Richiedente, m_IDRichiedente);
                }

                set
                {
                    int oldValue = IDRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDRichiedente = value;
                    m_Richiedente = null;
                    DoChanged("IDRichiedente", value, oldValue);
                }
            }

            public Sistema.CUser Richiedente
            {
                get
                {
                    if (m_Richiedente is null)
                        m_Richiedente = Sistema.Users.GetItemById(m_IDRichiedente);
                    return m_Richiedente;
                }

                set
                {
                    var oldValue = Richiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiedente = value;
                    m_IDRichiedente = DBUtils.GetID(value);
                    m_NomeRichiedente = "";
                    if (value is object)
                        m_NomeRichiedente = value.Nominativo;
                    DoChanged("Richiedente", value, oldValue);
                }
            }

            public string NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiedente = value;
                    DoChanged("NomeRichiedente", value, oldValue);
                }
            }

            public int IDMotivoRichiesta
            {
                get
                {
                    return DBUtils.GetID(m_MotivoRichiesta, m_IDMotivoRichiesta);
                }

                set
                {
                    int oldValue = IDMotivoRichiesta;
                    if (oldValue == value)
                        return;
                    m_IDMotivoRichiesta = value;
                    m_MotivoRichiesta = null;
                    DoChanged("IDMotivoRichiesta", value, oldValue);
                }
            }

            public CMotivoScontoPratica MotivoRichiesta
            {
                get
                {
                    if (m_MotivoRichiesta is null)
                        m_MotivoRichiesta = MotiviSconto.GetItemById(m_IDMotivoRichiesta);
                    return m_MotivoRichiesta;
                }

                set
                {
                    var oldValue = MotivoRichiesta;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_MotivoRichiesta = value;
                    m_IDMotivoRichiesta = DBUtils.GetID(value);
                    m_Motivo = "";
                    if (value is object)
                        m_Motivo = value.Nome;
                    DoChanged("MotivoRichiesta", value, oldValue);
                }
            }

            public string Motivo
            {
                get
                {
                    return m_Motivo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Motivo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Motivo = value;
                    DoChanged("Motivo", value, oldValue);
                }
            }

            public string DettaglioRichiesta
            {
                get
                {
                    return m_DettaglioRichiesta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioRichiesta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioRichiesta = value;
                    DoChanged("DettaglioRichiesta", value, oldValue);
                }
            }

            public int IDSupervisore
            {
                get
                {
                    return DBUtils.GetID(m_Supervisore, m_IDSupervisore);
                }

                set
                {
                    int oldValue = IDSupervisore;
                    if (oldValue == value)
                        return;
                    m_IDSupervisore = value;
                    m_Supervisore = null;
                    DoChanged("IDSupervisore", value, oldValue);
                }
            }

            public Sistema.CUser Supervisore
            {
                get
                {
                    if (m_Supervisore is null)
                        m_Supervisore = Sistema.Users.GetItemById(m_IDSupervisore);
                    return m_Supervisore;
                }

                set
                {
                    var oldValue = Supervisore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Supervisore = value;
                    m_IDSupervisore = DBUtils.GetID(value);
                    DoChanged("Supervisore", value, oldValue);
                }
            }

            public DateTime? DataEsito
            {
                get
                {
                    return m_DataEsito;
                }

                set
                {
                    var oldValue = m_DataEsito;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEsito = value;
                    DoChanged("DataEsito", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return RichiesteApprovazioneGroups.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDGrpRichApp";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente", m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                m_DataRichiesta = reader.Read("DataRichiesta", m_DataRichiesta);
                m_IDRichiedente = reader.Read("IDRichiedente", m_IDRichiedente);
                m_NomeRichiedente = reader.Read("NomeRichiedente", m_NomeRichiedente);
                m_IDMotivoRichiesta = reader.Read("IDMotivoRichiesta", m_IDMotivoRichiesta);
                m_Motivo = reader.Read("Motivo", m_Motivo);
                m_DettaglioRichiesta = reader.Read("DettaglioRichiesta", m_DettaglioRichiesta);
                m_IDSupervisore = reader.Read("IDSupervisore", m_IDSupervisore);
                m_DataEsito = reader.Read("DataEsito", m_DataEsito);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("IDRichiedente", IDRichiedente);
                writer.Write("NomeRichiedente", m_NomeRichiedente);
                writer.Write("IDMotivoRichiesta", IDMotivoRichiesta);
                writer.Write("Motivo", m_Motivo);
                writer.Write("DettaglioRichiesta", m_DettaglioRichiesta);
                writer.Write("IDSupervisore", IDSupervisore);
                writer.Write("DataEsito", m_DataEsito);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("IDRichiedente", IDRichiedente);
                writer.WriteAttribute("NomeRichiedente", m_NomeRichiedente);
                writer.WriteAttribute("IDMotivoRichiesta", IDMotivoRichiesta);
                writer.WriteAttribute("Motivo", m_Motivo);
                writer.WriteAttribute("IDSupervisore", IDSupervisore);
                writer.WriteAttribute("DataEsito", m_DataEsito);
                base.XMLSerialize(writer);
                writer.WriteTag("DettaglioRichiesta", m_DettaglioRichiesta);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
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

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDRichiedente":
                        {
                            m_IDRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRichiedente":
                        {
                            m_NomeRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDMotivoRichiesta":
                        {
                            m_IDMotivoRichiesta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Motivo":
                        {
                            m_Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioRichiesta":
                        {
                            m_DettaglioRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDSupervisore":
                        {
                            m_IDSupervisore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataEsito":
                        {
                            m_DataEsito = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public void Richiedi()
            {
                foreach (CRichiestaApprovazione ra in Richieste)
                {
                    if (ra.Stato == ObjectStatus.OBJECT_VALID && ra.StatoRichiesta == StatoRichiestaApprovazione.NONCHIESTA)
                    {
                        ra.StatoRichiesta = StatoRichiestaApprovazione.ATTESA;
                        ra.Save();
                    }
                }

                RichiesteApprovazioneGroups.doOnRichiedi(new ItemEventArgs<RichiestaApprovazioneGroup>(this));
            }

            public void Approva()
            {
                RichiesteApprovazioneGroups.doOnApprova(new ItemEventArgs<RichiestaApprovazioneGroup>(this));
            }

            public void Rifiuta()
            {
                RichiesteApprovazioneGroups.doOnRifiuta(new ItemEventArgs<RichiestaApprovazioneGroup>(this));
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}