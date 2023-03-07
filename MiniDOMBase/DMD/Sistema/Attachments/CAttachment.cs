using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;
using minidom.internals;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Flag per un allegato
        /// </summary>
        [Flags]
        public enum AttachmentFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Archivio
            /// </summary>
            Archive = 1,

            /// <summary>
            /// Documento in sola lettura
            /// </summary>
            Readonly = 2,

            /// <summary>
            /// Di sistema
            /// </summary>
            System = 4,

            /// <summary>
            /// Nascosto
            /// </summary>
            Hidden = 8,

            /// <summary>
            /// Folder
            /// </summary>
            Folder = 256
        }

        /// <summary>
        /// Stato di un allegato
        /// </summary>
        public enum AttachmentStatus : int
        {
            /// <summary>
            /// Documento non validato
            /// </summary>
            STATUS_NOTVALIDATED = 0,

            /// <summary>
            /// Documento validato
            /// </summary>
            STATUS_VALIDATED = 1,

            /// <summary>
            /// Documento non leggibile
            /// </summary>
            STATUS_NOTREADABLE = 2,

            /// <summary>
            /// Documento non valido
            /// </summary>
            STATUS_INVALID = 4        
        }

        /// <summary>
        /// Allegato per un oggetto qualsiasi
        /// </summary>
        [Serializable]
        public class CAttachment 
            : Databases.DBObject, IComparable, ICloneable, IComparable<CAttachment>
        {
            private int m_OwnerID; // ID della persona associata
            private string m_OwnerType;
            [NonSerialized] private object m_Owner; // Oggetto CPersona associato
            private string m_Tipo; // Tipo del documento allegato
            private AttachmentStatus m_StatoDocumento; // Valore che indica lo stato del documento (0 NON VALIDATO, 1 VALIDATO, 2 NON LEGGIBILE, 3 NON VALIDO ...)         
            private int m_VerificatoDaID; // ID dell'utente che ha verificato il documento
            [NonSerialized] private CUser m_VerificatoDa; // Oggetto utente che ha verificato il documento
            private string m_NomeVerificatoDa; // Nome dell'utente che ha verificato il documento
            private DateTime? m_VerificatoIl; // Data di verifica
            private string m_Testo; // Testo visualizzato
            private string m_URL; // URL del file
            private int m_IDContesto;
            private string m_TipoContesto;
            private string m_Parametro;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private int m_IDDocumento;
            private int m_IDRilasciatoDa;
            [NonSerialized] private Anagrafica.CPersona m_RilasciatoDa;
            private string m_NomeRilasciatoDa;
            // Private m_IDProduttore As Integer
            // Private m_Produttore As CAzienda

            private string m_Categoria;
            private string m_SottoCategoria;
            [NonSerialized] private CAttachment m_Parent;
            private int m_ParentID;
            [NonSerialized] private CAttachmentChilds m_Childs;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CAttachment()
            {
                m_OwnerID = 0;
                m_OwnerType = "";
                m_Owner = null;
                m_Tipo = "";
                m_StatoDocumento = 0;
                m_VerificatoDaID = 0;
                m_VerificatoDa = null;
                m_NomeVerificatoDa = "";
                m_VerificatoIl = default;
                m_Testo = "";
                m_URL = "";
                m_IDContesto = 0;
                m_TipoContesto = DMD.Strings.vbNullString;
                m_Parametro = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_IDRilasciatoDa = 0;
                m_RilasciatoDa = null;
                m_NomeRilasciatoDa = "";
                // Me.m_IDProduttore = 0
                // Me.m_Produttore = Nothing
                m_Categoria = "";
                m_SottoCategoria = "";
                m_Flags = (int) AttachmentFlags.None;
                m_Parent = null;
                m_ParentID = 0;
                m_Childs = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CAttachment(object owner) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                m_OwnerID = DBUtils.GetID(owner, 0);
                m_OwnerType = DMD.RunTime.vbTypeName(owner);
                m_Owner = owner;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="context"></param>
            public CAttachment(object owner, object context) : this(owner)
            {
                if (context is null)
                    throw new ArgumentNullException("context");
                m_IDContesto = DBUtils.GetID(context, 0);
                m_TipoContesto = DMD.RunTime.vbTypeName(context);
            }

            /// <summary>
            /// Restituisce o imposta i documenti collegati
            /// </summary>
            public CAttachmentChilds Childs
            {
                get
                {
                    if (m_Childs is null)
                        m_Childs = new CAttachmentChilds(this);
                    return m_Childs;
                }
            }


            /// <summary>
            /// Restituisce o imposta dei flag aggiuntivi
            /// </summary>
            public new AttachmentFlags Flags
            {
                get
                {
                    return (AttachmentFlags)m_Flags;
                }

                set
                {
                    var oldValue = (AttachmentFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il parent
            /// </summary>
            public CAttachment Parent
            {
                get
                {
                    if (m_Parent is null)
                        m_Parent = Attachments.GetItemById(m_ParentID);
                    return m_Parent;
                }

                set
                {
                    var oldValue = m_Parent;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Parent = value;
                    m_ParentID = DBUtils.GetID(value, 0);
                    DoChanged("Parent", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del parent
            /// </summary>
            public int ParentID
            {
                get
                {
                    return DBUtils.GetID(m_Parent, m_ParentID);
                }

                set
                {
                    int oldValue = ParentID;
                    if (oldValue == value)
                        return;
                    m_ParentID = value;
                    m_Parent = null;
                    DoChanged("ParentID", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il parent
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetParent(CAttachment value)
            {
                m_Parent = value;
                m_ParentID = DBUtils.GetID(value, 0);
            }



            /// <summary>
            /// Restituisce o imposta la categoria del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria secondaria del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SottoCategoria
            {
                get
                {
                    return m_SottoCategoria;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SottoCategoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SottoCategoria = value;
                    DoChanged("Sottocategoria", value, oldValue);
                }
            }
             

            /// <summary>
            /// Restituisce o imposta l'id dell'ente che ha rilasciato il documento
            /// </summary>
            public int IDRilasciatoDa
            {
                get
                {
                    return DBUtils.GetID(m_RilasciatoDa, m_IDRilasciatoDa);
                }

                set
                {
                    int oldValue = IDRilasciatoDa;
                    if (oldValue == value)
                        return;
                    m_IDRilasciatoDa = value;
                    m_RilasciatoDa = null;
                    DoChanged("IDRilasciatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ente che ha rilasciato il documento
            /// </summary>
            public Anagrafica.CPersona RilasciatoDa
            {
                get
                {
                    if (m_RilasciatoDa is null)
                        m_RilasciatoDa = Anagrafica.Persone.GetItemById(m_IDRilasciatoDa);
                    return m_RilasciatoDa;
                }

                set
                {
                    var oldValue = m_RilasciatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RilasciatoDa = value;
                    m_IDRilasciatoDa = DBUtils.GetID(value, 0);
                    m_NomeRilasciatoDa = "";
                    if (value is object)
                        m_NomeRilasciatoDa = value.Nominativo;
                    DoChanged("RilasciatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'ente che ha rilasciato il documento
            /// </summary>
            public string NomeRilasciatoDa
            {
                get
                {
                    return m_NomeRilasciatoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRilasciatoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRilasciatoDa = value;
                    DoChanged("NomeRilasciatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del template del documento
            /// </summary>
            public int IDDocumento
            {
                get
                {
                    return m_IDDocumento;
                }

                set
                {
                    int oldValue = m_IDDocumento;
                    if (oldValue == value)
                        return;
                    m_IDDocumento = value;
                    DoChanged("IDDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un parametro aggiuntivo
            /// </summary>
            public string Parametro
            {
                get
                {
                    return m_Parametro;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Parametro;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Parametro = value;
                    DoChanged("Parametro", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio validità del documento
            /// </summary>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine validità del documento
            /// </summary>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se il documento è valido alla data odierna
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Restituisce true se il documento è valido alla data specificata
            /// </summary>
            /// <param name="atDate"></param>
            /// <returns></returns>
            public bool IsValid(DateTime atDate)
            {
                return DMD.DateUtils.CheckBetween(atDate, DataInizio, DataFine);
            }

           

            /// <summary>
            /// Restituisce il proprietario
            /// </summary>
            public object Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta il proprietario
            /// </summary>
            /// <param name="value"></param>
            public void SetOwner(object value)
            {
                m_Owner = value;
                m_OwnerID = DBUtils.GetID(value, 0);
                m_OwnerType = DMD.RunTime.vbTypeName(value);
                SetChanged(true);
            }

            /// <summary>
            /// Restituisce o imposta l'id del contesto
            /// </summary>
            public int IDContesto
            {
                get
                {
                    return m_IDContesto;
                }

                set
                {
                    int oldValue = m_IDContesto;
                    if (oldValue == value)
                        return;
                    m_IDContesto = value;
                    DoChanged("IDContesto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'id del contesto
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetIDContesto(int value)
            {
                m_IDContesto = value;
            }

            /// <summary>
            /// Restituisce o imposta il tipo del contesto
            /// </summary>
            public string TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContesto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContesto = value;
                    DoChanged("TipoContesto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il tipo contesto
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetTipoContesto(string value)
            {
                m_TipoContesto = value;
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è associato il documento
            /// </summary>
            public int IDOwner
            {
                get
                {
                    return DBUtils.GetID(m_Owner, m_OwnerID);
                }

                set
                {
                    int oldValue = IDOwner;
                    if (oldValue == value)
                        return;
                    m_OwnerID = value;
                    m_Owner = null;
                    DoChanged("IDOwner", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il tipo del proprietario
            /// </summary>
            public string OwnerType
            {
                get
                {
                    return m_OwnerType;
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del doumento
            /// </summary>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del documento
            /// </summary>
            public string Testo
            {
                get
                {
                    return m_Testo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Testo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Testo = value;
                    DoChanged("Testo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la url da cui è possibile scaricare il documento
            /// </summary>
            public string URL
            {
                get
                {
                    return m_URL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_URL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_URL = value;
                    DoChanged("URL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato del documento
            /// </summary>
            public AttachmentStatus StatoDocumento
            {
                get
                {
                    return m_StatoDocumento;
                }

                set
                {
                    var oldValue = m_StatoDocumento;
                    if (oldValue == value)
                        return;
                    m_StatoDocumento = value;
                    DoChanged("StatoDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'utente che ha validato l'oggetto
            /// </summary>
            public int VerificatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_VerificatoDa, m_VerificatoDaID);
                }

                set
                {
                    int oldValue = VerificatoDaID;
                    if (value == oldValue)
                        return;
                    m_VerificatoDaID = value;
                    m_VerificatoDa = null;
                    DoChanged("VerificatoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha validato l'oggetto
            /// </summary>
            public CUser VerificatoDa
            {
                get
                {
                    if (m_VerificatoDa is null)
                        m_VerificatoDa = Users.GetItemById(m_VerificatoDaID);
                    return m_VerificatoDa;
                }

                set
                {
                    var oldValue = VerificatoDa;
                    if (oldValue == value)
                        return;
                    m_VerificatoDa = value;
                    m_VerificatoDaID = DBUtils.GetID(value, 0);
                    m_NomeVerificatoDa = "";
                    if (value is object)
                        m_NomeVerificatoDa = value.Nominativo;
                    DoChanged("VerificatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha validato l'oggetto
            /// </summary>
            public string NomeVerificatoDa
            {
                get
                {
                    return m_NomeVerificatoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeVerificatoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeVerificatoDa = value;
                    DoChanged("NomeVerificatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di validazione
            /// </summary>
            public DateTime? VerificatoIl
            {
                get
                {
                    return m_VerificatoIl;
                }

                set
                {
                    var oldValue = m_VerificatoIl;
                    if (oldValue == value == true)
                        return;
                    m_VerificatoIl = value;
                    DoChanged("VerificatoIl", value, oldValue);
                }
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public int CompareTo(CAttachment b)
            {
                return DMD.Strings.Compare(this.Testo, b.Testo, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CAttachment)obj);
            }

           

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OwnerID", IDOwner);
                writer.WriteAttribute("OwnerType", m_OwnerType);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("StatoDocumento", (int?)m_StatoDocumento);
                writer.WriteAttribute("VerificatoDaID", VerificatoDaID);
                writer.WriteAttribute("NomeVerificatoDa", m_NomeVerificatoDa);
                writer.WriteAttribute("VerificatoIl", m_VerificatoIl);
                writer.WriteAttribute("Testo", m_Testo);
                writer.WriteAttribute("URL", m_URL);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("Parametro", m_Parametro);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("IDDocumento", m_IDDocumento);
                writer.WriteAttribute("IDRilasciatoDa", IDRilasciatoDa);
                writer.WriteAttribute("RilasciatoDa", m_NomeRilasciatoDa);
                // writer.WriteAttribute("IDProduttore", Me.IDProduttore)
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("SottoCategoria", m_SottoCategoria);
                writer.WriteAttribute("ParentID", ParentID);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OwnerID":
                        {
                            m_OwnerID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OwnerType":
                        {
                            m_OwnerType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoDocumento":
                        {
                            m_StatoDocumento = (AttachmentStatus)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "VerificatoDaID":
                        {
                            m_VerificatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeVerificatoDa":
                        {
                            m_NomeVerificatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "VerificatoIl":
                        {
                            m_VerificatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Testo":
                        {
                            m_Testo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "URL":
                        {
                            m_URL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoContesto":
                        {
                            m_TipoContesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Parametro":
                        {
                            m_Parametro = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "IDDocumento":
                        {
                            m_IDDocumento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRilasciatoDa":
                        {
                            m_IDRilasciatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RilasciatoDa":
                        {
                            m_NomeRilasciatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // Case "IDProduttore" : Me.m_IDProduttore = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SottoCategoria":
                        {
                            m_SottoCategoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ParentID":
                        {
                            m_ParentID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Attachments";
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Attachments; //.Module;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Attachments.Database;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_OwnerID = reader.Read("OwnerID",  m_OwnerID);
                m_OwnerType = reader.Read("OwnerType",  m_OwnerType);
                m_Tipo = reader.Read("Tipo",  m_Tipo);
                m_Testo = reader.Read("Testo",  m_Testo);
                m_URL = reader.Read("URL",  m_URL);
                m_StatoDocumento = reader.Read("StatoDocumento",  m_StatoDocumento);
                m_VerificatoDaID = reader.Read("VerificatoDa",  m_VerificatoDaID);
                m_NomeVerificatoDa = reader.Read("NomeVerificatoDa",  m_NomeVerificatoDa);
                m_VerificatoIl = reader.Read("VerificatoIl",  m_VerificatoIl);
                m_IDContesto = reader.Read("IDContesto",  m_IDContesto);
                m_TipoContesto = reader.Read("TipoContesto",  m_TipoContesto);
                m_TipoContesto = reader.Read("TipoContesto",  m_TipoContesto);
                m_Parametro = reader.Read("Parametro",  m_Parametro);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataFine = reader.Read("DataFine",  m_DataFine);
                m_IDDocumento = reader.Read("IDDocumento",  m_IDDocumento);
                m_IDRilasciatoDa = reader.Read("IDRilasciatoDa",  m_IDRilasciatoDa);
                m_NomeRilasciatoDa = reader.Read("RilasciatoDa",  m_NomeRilasciatoDa);
                // Me.m_IDProduttore = reader.Read("IDProduttore", Me.m_IDProduttore)
                m_Categoria = reader.Read("Categoria",  m_Categoria);
                m_SottoCategoria = reader.Read("SottoCategoria",  m_SottoCategoria);
                m_ParentID = reader.Read("ParentID",  m_ParentID);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel DB
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("OwnerID", DBUtils.GetID(m_Owner, m_OwnerID));
                writer.Write("OwnerType", m_OwnerType);
                writer.Write("Testo", m_Testo);
                writer.Write("URL", m_URL);
                writer.Write("Tipo", m_Tipo);
                writer.Write("StatoDocumento", m_StatoDocumento);
                writer.Write("VerificatoDa", VerificatoDaID);
                writer.Write("NomeVerificatoDa", m_NomeVerificatoDa);
                writer.Write("VerificatoIl", m_VerificatoIl);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("Parametro", m_Parametro);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("IDDocumento", m_IDDocumento);
                writer.Write("IDRilasciatoDa", IDRilasciatoDa);
                writer.Write("RilasciatoDa", m_NomeRilasciatoDa);
                // writer.Write("IDProduttore", Me.IDProduttore)
                writer.Write("Categoria", m_Categoria);
                writer.Write("SottoCategoria", m_SottoCategoria);
                // writer.Write("ParentID", Me.ParentID)
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Fields.Ensure("OwnerID", typeof(int), 1);
                c = table.Fields.Ensure("OwnerType", typeof(string), 255);
                c = table.Fields.Ensure("Testo", typeof(string), 255);
                c = table.Fields.Ensure("URL", typeof(string), 255);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("StatoDocumento", typeof(int), 1);
                c = table.Fields.Ensure("VerificatoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeVerificatoDa", typeof(string), 255);
                c = table.Fields.Ensure("VerificatoIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDContesto", typeof(int), 1);
                c = table.Fields.Ensure("TipoContesto", typeof(string), 255);
                c = table.Fields.Ensure("Parametro", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDDocumento", typeof(int), 1);
                c = table.Fields.Ensure("IDRilasciatoDa", typeof(int), 1);
                c = table.Fields.Ensure("RilasciatoDa", typeof(string), 255);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("SottoCategoria", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Constraints.Ensure("idxOwner", new string[] { "OwnerID", "OwnerType" }, DBFieldConstraintFlags.None);
                
                c = table.Constraints.Ensure("idxTemplate", new string[] { "Tipo", "IDDocumento", "Testo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri", new string[] { "Parametro", "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoDoc", new string[] { "StatoDocumento", "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxVerifiche", new string[] { "VerificatoDa", "VerificatoIl", "NomeVerificatoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRilasciato", new string[] { "IDRilasciatoDa", "RilasciatoDa"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "SottoCategoria" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContesto", new string[] { "TipoContesto", "IDContesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxURL", new string[] { "URL" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);

            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{", this.m_Tipo , " : " , this.m_Testo, " }");
            }

             
            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CAttachment) && this.Equals((CAttachment)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="a"></param>
            /// <returns></returns>
            public virtual bool Equals(CAttachment a)
            {

                return   DMD.Strings.EQ(this.OwnerType, a.OwnerType)
                      && DMD.Integers.EQ(this.IDOwner, a.IDOwner)
                      && DMD.Integers.EQ(this.IDDocumento , a.IDDocumento )
                      && DMD.DateUtils.EQ(this.DataInizio, a.DataInizio) 
                      && DMD.DateUtils.EQ(this.DataFine, a.DataFine) 
                      && DMD.Strings.EQ(this.Tipo , a.Tipo )
                      && DMD.Strings.EQ(this.Testo , a.Testo )
                      && DMD.Strings.EQ(this.Categoria , a.Categoria ) 
                      && DMD.Strings.EQ(this.Parametro , a.Parametro ) 
                      && DMD.Strings.EQ(this.NomeRilasciatoDa , a.NomeRilasciatoDa);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.OwnerType, this.IDOwner, this.IDDocumento, this.Tipo, this.Categoria, this.Testo);
            }

            /// <summary>
            /// Clonea l'oggetto
            /// </summary>
            /// <returns></returns>
            public CAttachment Clone()
            {
                return (CAttachment) this.MemberwiseClone();
            }

            object ICloneable.Clone() { return this.Clone(); }

           
        }
    }
}