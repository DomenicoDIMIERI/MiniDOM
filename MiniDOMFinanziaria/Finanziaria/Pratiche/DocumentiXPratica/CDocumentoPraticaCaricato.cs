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
    /// Rappresenta il caricamento di un Documento per una Pratica
    /// </summary>
    /// <remarks></remarks>
        public class CDocumentoPraticaCaricato : Databases.DBObject
        {
            private int m_IDDocumento; // [INT] ID del tipo di documento (CDocumentoPerPratica)
            private CDocumentoXGruppoProdotti m_Documento;  // [CDocumentoPerPratica] Documento 
            private int m_IDPratica; // ID della pratica
            private CPraticaCQSPD m_Pratica; // Pratica
            private DateTime? m_DataCaricamento; // [Date] Data ed ora di caricamento
            private int m_IDOperatoreCaricamento; // [INT] ID dell'operatore che ha caricato il documento
            private Sistema.CUser m_OperatoreCaricamento; // [CUser] Operatore che ha caricato il documento
            private string m_NomeOperatoreCaricamento;
            private DateTime? m_DataInizioSpedizione; // [Date] Data di inizio spedizione
            private int m_IDOperatoreSpedizione; // [INT] ID dell'operatore che ha preso in carico la spedizione
            private Sistema.CUser m_OperatoreSpedizione; // [CUser] Operatore che ha preso in carico la spedizione
            private string m_NomeOperatoreSpedizione;
            private DateTime? m_DataConsegna; // [Date] Data di consegna
            private int m_IDOperatoreConsegna; // [INT] ID dell'operatore che consegnato il documento
            private Sistema.CUser m_OperatoreConsegna; // [CUser] Operatore che ha consegnato il documento
            private string m_NomeOperatoreConsegna;
            private bool m_Firmato; // [BOOL] Se vero indica che il documento è stato firmato
            private int m_StatoConsegna; // [INT] 0 in preaccettazione, 1 in gestione, 2 spedito, 3 fermo, 4 in consegna, 5 consegnato, 255 errore
            private string m_Note; // [TEXT]
            private int m_Progressivo;
            private bool m_Verificato;
            private int m_IDDocumentoCaricato;
            private Sistema.CAttachment m_DocumentoCaricato;

            public CDocumentoPraticaCaricato()
            {
                m_IDDocumento = 0;
                m_Documento = null;
                m_IDPratica = 0;
                m_Pratica = null;
                m_DataCaricamento = default;
                m_IDOperatoreCaricamento = 0;
                m_OperatoreCaricamento = null;
                m_NomeOperatoreCaricamento = "";
                m_DataInizioSpedizione = default;
                m_IDOperatoreSpedizione = 0;
                m_OperatoreSpedizione = null;
                m_NomeOperatoreSpedizione = "";
                m_DataConsegna = default;
                m_IDOperatoreConsegna = 0;
                m_OperatoreConsegna = null;
                m_NomeOperatoreConsegna = "";
                m_Firmato = false;
                m_StatoConsegna = 0;
                m_Note = "";
                m_Progressivo = 0;
                m_Verificato = false;
                m_IDDocumentoCaricato = 0;
                m_DocumentoCaricato = null;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public int IDDocumentoCaricato
            {
                get
                {
                    return DBUtils.GetID(m_DocumentoCaricato, m_IDDocumentoCaricato);
                }

                set
                {
                    int oldValue = IDDocumentoCaricato;
                    if (oldValue == value)
                        return;
                    m_DocumentoCaricato = null;
                    m_IDDocumentoCaricato = value;
                    DoChanged("IDDocumentoCaricato", value, oldValue);
                }
            }

            public Sistema.CAttachment DocumentoCaricato
            {
                get
                {
                    if (m_DocumentoCaricato is null)
                        m_DocumentoCaricato = Sistema.Attachments.GetItemById(m_IDDocumentoCaricato);
                    return m_DocumentoCaricato;
                }

                set
                {
                    var oldValue = m_DocumentoCaricato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DocumentoCaricato = value;
                    m_IDDocumentoCaricato = DBUtils.GetID(value);
                    DoChanged("DocumentoCaricato", value, oldValue);
                }
            }

            public bool Verificato
            {
                get
                {
                    return m_Verificato;
                }

                set
                {
                    if (m_Verificato == value)
                        return;
                    m_Verificato = value;
                    DoChanged("Verificato", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDDocumento
            {
                get
                {
                    return DBUtils.GetID(m_Documento, m_IDDocumento);
                }

                set
                {
                    int oldValue = IDDocumento;
                    if (oldValue == value)
                        return;
                    m_IDDocumento = value;
                    m_Documento = null;
                    DoChanged("IDDocumento", value, oldValue);
                }
            }

            /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CDocumentoXGruppoProdotti Documento
            {
                get
                {
                    if (m_Documento is null)
                        m_Documento = VincoliProdotto.GetItemById(m_IDDocumento);
                    return m_Documento;
                }

                set
                {
                    var oldValue = Documento;
                    if (oldValue == value)
                        return;
                    m_Documento = value;
                    m_IDDocumento = DBUtils.GetID(value);
                    DoChanged("Documento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica per cui è stato caricato il documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPratica
            {
                get
                {
                    return DBUtils.GetID(m_Pratica, m_IDPratica);
                }

                set
                {
                    int oldValue = IDPratica;
                    if (oldValue == value)
                        return;
                    m_IDPratica = value;
                    m_Pratica = null;
                    DoChanged("IDPratica", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la pratica per cui è stato caricato il documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD Pratica
            {
                get
                {
                    if (m_Pratica is null)
                        m_Pratica = Pratiche.GetItemById(m_IDPratica);
                    return m_Pratica;
                }

                set
                {
                    var oldValue = Pratica;
                    if (oldValue == value)
                        return;
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                    DoChanged("Pratica", value, oldValue);
                }
            }

            protected internal void SetPratica(CPraticaCQSPD value)
            {
                m_Pratica = value;
                m_IDPratica = DBUtils.GetID(value);
            }

            protected internal void SetProgressivo(int value)
            {
                m_Progressivo = value;
            }

            public int Progressivo
            {
                get
                {
                    return m_Progressivo;
                }

                set
                {
                    int oldValue = m_Progressivo;
                    if (oldValue == value)
                        return;
                    m_Progressivo = value;
                    DoChanged("Progressivo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di caricamento del documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }

                set
                {
                    var oldValue = m_DataCaricamento;
                    if (oldValue == value == true)
                        return;
                    m_DataCaricamento = value;
                    DoChanged("DataCaricamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'operatore che ha caricato il documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOperatoreCaricamento
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreCaricamento, m_IDOperatoreCaricamento);
                }

                set
                {
                    int oldValue = IDOperatoreCaricamento;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreCaricamento = value;
                    m_OperatoreCaricamento = null;
                    DoChanged("IDOperatoreCaricamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'operatore che ha caricato il documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser OperatoreCaricamento
            {
                get
                {
                    if (m_OperatoreCaricamento is null)
                        m_OperatoreCaricamento = Sistema.Users.GetItemById(m_IDOperatoreCaricamento);
                    return m_OperatoreCaricamento;
                }

                set
                {
                    var oldValue = OperatoreCaricamento;
                    if (oldValue == value)
                        return;
                    m_OperatoreCaricamento = value;
                    m_IDOperatoreCaricamento = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeOperatoreCaricamento = value.Nominativo;
                    DoChanged("OperatoreCaricamento", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'operatore che ha caricato il documento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeOperatoreCaricamento
            {
                get
                {
                    return m_NomeOperatoreCaricamento;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatoreCaricamento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreCaricamento = value;
                    DoChanged("NomeOperatoreCaricamento", value, oldValue);
                }
            }

            /// <summary>
        /// Se il documento è stato spedito restituisce o imposta la data di spedizione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizioSpedizione
            {
                get
                {
                    return m_DataInizioSpedizione;
                }

                set
                {
                    var oldValue = m_DataInizioSpedizione;
                    if (oldValue == value == true)
                        return;
                    m_DataInizioSpedizione = value;
                    DoChanged("DataInizioSpedizione", value, oldValue);
                }
            }

            /// <summary>
        /// Se il documento è stato spedito restituisce o imposta l'operatore che ha effettuato la spedizione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOperatoreSpedizione
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreSpedizione, m_IDOperatoreSpedizione);
                }

                set
                {
                    int oldValue = IDOperatoreSpedizione;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreSpedizione = value;
                    m_OperatoreSpedizione = null;
                    DoChanged("IDOperatoreSpedizione", value, oldValue);
                }
            }

            public Sistema.CUser OperatoreSpedizione
            {
                get
                {
                    if (m_OperatoreSpedizione is null)
                        m_OperatoreSpedizione = Sistema.Users.GetItemById(m_IDOperatoreSpedizione);
                    return m_OperatoreSpedizione;
                }

                set
                {
                    var oldValue = OperatoreSpedizione;
                    m_OperatoreSpedizione = value;
                    m_IDOperatoreSpedizione = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeOperatoreSpedizione = value.Nominativo;
                    DoChanged("OperatoreSpedizione", value, oldValue);
                }
            }

            public string NomeOperatoreSpedizione
            {
                get
                {
                    return m_NomeOperatoreSpedizione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatoreSpedizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreSpedizione = value;
                    DoChanged("NomeOperatoreSpedizione", value, oldValue);
                }
            }

            public DateTime? DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }

                set
                {
                    var oldValue = m_DataConsegna;
                    if (oldValue == value == true)
                        return;
                    m_DataConsegna = value;
                    DoChanged("DataConsegna", value, oldValue);
                }
            }

            public int IDOperatoreConsegna
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreConsegna, m_IDOperatoreConsegna);
                }

                set
                {
                    int oldValue = IDOperatoreConsegna;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreConsegna = value;
                    m_OperatoreConsegna = null;
                    DoChanged("IDOperatoreConsegna", value, oldValue);
                }
            }

            public Sistema.CUser OperatoreConsegna
            {
                get
                {
                    if (m_OperatoreConsegna is null)
                        m_OperatoreConsegna = Sistema.Users.GetItemById(m_IDOperatoreConsegna);
                    return m_OperatoreConsegna;
                }

                set
                {
                    var oldValue = OperatoreConsegna;
                    if (oldValue == value)
                        return;
                    m_OperatoreConsegna = value;
                    m_IDOperatoreConsegna = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeOperatoreConsegna = value.Nominativo;
                    DoChanged("OperatoreConsegna", value, oldValue);
                }
            }

            public string NomeOperatoreConsegna
            {
                get
                {
                    return m_NomeOperatoreConsegna;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatoreConsegna;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreConsegna = value;
                    DoChanged("NomeOperatoreConsegna", value, oldValue);
                }
            }

            public bool Firmato
            {
                get
                {
                    return m_Firmato;
                }

                set
                {
                    if (m_Firmato == value)
                        return;
                    m_Firmato = value;
                    DoChanged("Firmato", value, !value);
                }
            }

            public int StatoConsegna
            {
                get
                {
                    return m_StatoConsegna;
                }

                set
                {
                    int oldValue = m_StatoConsegna;
                    if (oldValue == value)
                        return;
                    m_StatoConsegna = value;
                    DoChanged("StatoConsegna", value, oldValue);
                }
            }

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
                    DoChanged("Note", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_DocXPrat";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDDocumento = reader.Read("Documento",  m_IDDocumento);
                m_IDPratica = reader.Read("Pratica",  m_IDPratica);
                m_DataCaricamento = reader.Read("DataCaricamento",  m_DataCaricamento);
                m_IDOperatoreCaricamento = reader.Read("IDOpCaricamento",  m_IDOperatoreCaricamento);
                m_NomeOperatoreCaricamento = reader.Read("NmOpCaricamento",  m_NomeOperatoreCaricamento);
                m_DataInizioSpedizione = reader.Read("DataInizioSpedizione",  m_DataInizioSpedizione);
                m_IDOperatoreSpedizione = reader.Read("IDOpSpedizione",  m_IDOperatoreSpedizione);
                m_NomeOperatoreSpedizione = reader.Read("NmOpSpedizione",  m_NomeOperatoreSpedizione);
                m_DataConsegna = reader.Read("DataConsegna",  m_DataConsegna);
                m_IDOperatoreConsegna = reader.Read("IDOpConsegna",  m_IDOperatoreConsegna);
                m_NomeOperatoreConsegna = reader.Read("NmOpConsegna",  m_NomeOperatoreConsegna);
                m_Firmato = reader.Read("Firmato",  m_Firmato);
                m_StatoConsegna = reader.Read("StatoConsegna",  m_StatoConsegna);
                m_Note = reader.Read("Notes",  m_Note);
                m_Progressivo = reader.Read("Progressivo",  m_Progressivo);
                m_Verificato = reader.Read("Verificato",  m_Verificato);
                m_IDDocumentoCaricato = reader.Read("IDDocumentoCaricato",  m_IDDocumentoCaricato);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Documento", IDDocumento);
                writer.Write("Pratica", IDPratica);
                writer.Write("DataCaricamento", m_DataCaricamento);
                writer.Write("IDOpCaricamento", IDOperatoreCaricamento);
                writer.Write("NmOpCaricamento", m_NomeOperatoreCaricamento);
                writer.Write("DataInizioSpedizione", m_DataInizioSpedizione);
                writer.Write("IDOpSpedizione", IDOperatoreSpedizione);
                writer.Write("NmOpSpedizione", m_NomeOperatoreSpedizione);
                writer.Write("DataConsegna", m_DataConsegna);
                writer.Write("IDOpConsegna", IDOperatoreConsegna);
                writer.Write("NmOpConsegna", m_NomeOperatoreConsegna);
                writer.Write("Firmato", m_Firmato);
                writer.Write("StatoConsegna", m_StatoConsegna);
                writer.Write("Notes", m_Note);
                writer.Write("Progressivo", m_Progressivo);
                writer.Write("Verificato", m_Verificato);
                writer.Write("IDDocumentoCaricato", IDDocumentoCaricato);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Note;
            }

            // ---------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataCaricamento", m_DataCaricamento);
                writer.WriteAttribute("IDOperatoreCaricamento", IDOperatoreCaricamento);
                writer.WriteAttribute("NomeOperatoreCaricamento", m_NomeOperatoreCaricamento);
                writer.WriteAttribute("DataInizioSpedizione", m_DataInizioSpedizione);
                writer.WriteAttribute("IDOperatoreSpedizione", IDOperatoreSpedizione);
                writer.WriteAttribute("NomeOperatoreSpedizione", m_NomeOperatoreSpedizione);
                writer.WriteAttribute("DataConsegna", m_DataConsegna);
                writer.WriteAttribute("IDOperatoreConsegna", IDOperatoreConsegna);
                writer.WriteAttribute("NomeOperatoreConsegna", m_NomeOperatoreConsegna);
                writer.WriteAttribute("Firmato", m_Firmato);
                writer.WriteAttribute("StatoConsegna", m_StatoConsegna);
                writer.WriteAttribute("Progressivo", m_Progressivo);
                writer.WriteAttribute("Verificato", m_Verificato);
                writer.WriteAttribute("IDDocumentoCaricato", IDDocumentoCaricato);
                writer.WriteAttribute("IDDocumento", IDDocumento);
                writer.WriteAttribute("IDPratica", IDPratica);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
                writer.WriteTag("Documento", Documento);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "DataCaricamento":
                        {
                            m_DataCaricamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatoreCaricamento":
                        {
                            m_IDOperatoreCaricamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreCaricamento":
                        {
                            m_NomeOperatoreCaricamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizioSpedizione":
                        {
                            m_DataInizioSpedizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatoreSpedizione":
                        {
                            m_IDOperatoreSpedizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreSpedizione":
                        {
                            m_NomeOperatoreSpedizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataConsegna":
                        {
                            m_DataConsegna = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatoreConsegna":
                        {
                            m_IDOperatoreConsegna = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreConsegna":
                        {
                            m_NomeOperatoreConsegna = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Firmato":
                        {
                            m_Firmato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "StatoConsegna":
                        {
                            m_StatoConsegna = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Progressivo":
                        {
                            m_Progressivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Verificato":
                        {
                            m_Verificato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDDocumentoCaricato":
                        {
                            m_IDDocumentoCaricato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDocumento":
                        {
                            m_IDDocumento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Documento":
                        {
                            m_Documento = (CDocumentoXGruppoProdotti)DMD.XML.Utils.Serializer.ToObject(fieldValue);
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