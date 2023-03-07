using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;
using DMD.Net.Mail;
using DMD.Databases;
using DMD.Net.Mime;
using System.Collections.Generic;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Flag per un messaggio email
        /// </summary>
        [Flags]
        public enum MailFlags : int
        {
            NotSet = 0,

            /// <summary>
            /// Il messaggio non è ancora stato letto
            /// </summary>
            /// <remarks></remarks>
            Unread = 1,

            /// <summary>
            /// Se vero indica che la mail contiene degli allegati
            /// </summary>
            Attachments = 2
        }

        /// <summary>
        /// Messaggio email
        /// </summary>
        [Serializable]
        public class MailMessage 
            : Databases.DBObjectPO, Sistema.IIndexable
        {

            // Public Const EmailRegexPattern As String = "(['""]{1,}.+['""]{1,}\s+)?<?[\w\.\-]+@[^\.][\w\.\-]+\.[a-z]{2,}>?"

            private int m_ApplicationID;
            [NonSerialized] private MailApplication m_Application;
            private int m_FolderID;
            [NonSerialized] private MailFolder m_Folder;
            private int m_AccountID;
            private string m_AccountName;     // Account utilizzato per scaricare il messaggio
            [NonSerialized] private MailAccount m_Account;

            // Private m_Message1 As minidom.Net.Mail.MailMessageEx   'Messaggio
            [NonSerialized] internal MailAttachmentCollection m_Attachements;
            private string m_Categoria;
            private CKeyCollection<string> m_Headers;
            private MailAddress m_From;
            private MailAddress m_Sender;
            private MailAddress m_DeliveredTo;
            private MailAddress m_ReplyTo;
            private MailAddressCollection m_To;
            private MailAddressCollection m_Cc;
            private MailAddressCollection m_Bcc;
            private string m_Subject;
            private string m_Body;
            private DateTime? m_DeliveryDate;         // Data di consegna
            private DateTime? m_DownloadDate;         // Data di download
            private DateTime? m_ReadDate;             // Data di lettura
            private string m_MessageId;
            private string m_ReplyToMessageId = "";
            private bool m_IsBodyHtml;
            private DeliveryNotificationOptions m_DeliveryNotificationOptions;
            private MailPriority m_Priority;
            [NonSerialized] internal List<MailAddress> m_OriginalAddresses;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailMessage()
            {
                m_Application = null;
                m_ApplicationID = 0;
                m_FolderID = 0;
                m_Folder = null;
                m_AccountID = 0;
                m_AccountName = "";
                m_Account = null;
                m_Categoria = "";
                m_Attachements = null;
                m_Flags = (int)MailFlags.NotSet;
                m_Headers = null;
                m_From = null; // New MailAddress(Me, "from")
                m_DeliveredTo = null; // New MailAddress(Me, "delivered-to")
                m_Sender = null; // New MailAddress(Me, "sender")
                m_ReplyTo = null; // New MailAddress(Me, "reply-to")
                m_To = null;
                m_Cc = null;
                m_Bcc = null;
                m_Subject = "";
                m_Body = DMD.Strings.vbNullString;
                m_DeliveryDate = default;
                m_MessageId = "";
                m_IsBodyHtml = true;
                m_DeliveryNotificationOptions = DeliveryNotificationOptions.None;
                m_Priority = MailPriority.Normal;
                // Me.m_Object = Nothing
                m_DownloadDate = default;
                m_ReadDate = default;
                m_OriginalAddresses = null;
            }

            private string getDeliveredTo(string stri)
            {
                int p = stri.IndexOf(",");
                if (p > 0)
                {
                    return stri.Substring(0, p);
                }
                else
                {
                    return stri;
                }
            }

            /// <summary>
            /// Inizializza il messaggio
            /// </summary>
            /// <param name="msg"></param>
            protected internal void FromMessage(MailMessageEx msg)
            {
                Stato = ObjectStatus.OBJECT_TEMP;
                // Me.Save()

                m_Headers = new CKeyCollection<string>();
                string[] keys = (string[])msg.Headers.AllKeys.Clone();
                string deliveredTo = "";
                foreach (string k in keys)
                {
                    m_Headers.Add(k, msg.Headers.Get(k));
                    if (Strings.LCase(k) == "delivered-to")
                    {
                        deliveredTo = getDeliveredTo(msg.Headers.Get(k));
                    }
                }

                Attachments.Clear();
                foreach (var at in msg.Attachments)
                {
                    var at1 = new MailAttachment();
                    at1.SetApplication(Application);
                    at1.SetMessage(this);
                    at1.From(at);
                    at1.Stato = ObjectStatus.OBJECT_VALID;
                    Attachments.Add(at1);
                }

                m_OriginalAddresses = new  List<MailAddress> ();
                m_Flags = (int) MailFlags.Unread;
                From = ParseAddress(msg.From, "from");
                ReplyTo = ParseAddress(msg.ReplyTo, "reply-to");
                DeliveredTo = ParseAddress(deliveredTo, "delivered-to");
                // Me.Sender  = Me.ParseAddress(deliveredTo, "delivered-to")

                m_To = ParseAddressCollection(msg.To, "to");
                m_Cc = ParseAddressCollection(msg.CC, "cc");
                m_Bcc = ParseAddressCollection(msg.Bcc, "bcc");
                m_Subject = msg.Subject;
                m_Body = msg.Body;
                m_DeliveryDate = msg.DeliveryDate;
                m_MessageId = msg.MessageId;
                m_IsBodyHtml = msg.IsBodyHtml;
                m_DeliveryNotificationOptions = (DeliveryNotificationOptions)msg.DeliveryNotificationOptions;
                m_Priority = (MailPriority)msg.Priority;
                m_DownloadDate = DMD.DateUtils.Now();
                m_ReadDate = default;
                SetFlag(MailFlags.Attachments, msg.Attachments.Count > 0);
                SetChanged(true);
            }

            /// <summary>
            /// Interpreta gli indirizzi
            /// </summary>
            /// <param name="addr"></param>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            private MailAddress ParseAddress(MailAddressEx addr, string fieldName)
            {
                var ret = new MailAddress();
                ret.FieldName = fieldName;
                if (addr is object)
                {
                    ret.Address = addr.Address;
                    ret.DisplayName = addr.DisplayName;
                }

                ret.SetApplication(Application);
                ret.SetMessage(this);
                return ret;
            }

            /// <summary>
            /// Interpreta il singolo indirizzo
            /// </summary>
            /// <param name="addr"></param>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            private MailAddress ParseAddress(string addr, string fieldName)
            {
                var ret = new MailAddress();
                ret.FieldName = fieldName;
                ret.Address = addr;
                ret.DisplayName = "";
                ret.SetApplication(Application);
                ret.SetMessage(this);
                return ret;
            }

            /// <summary>
            /// Interpreta la collezione di indirizzi
            /// </summary>
            /// <param name="col"></param>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            private MailAddressCollection ParseAddressCollection(MailAddressCollectionEx col, string fieldName)
            {
                var ret = new MailAddressCollection();
                ret.SetMessage(this);
                ret.SetFieldName(fieldName);
                foreach (MailAddressEx a in col)
                {
                    var ad = ParseAddress(a, fieldName);
                    if (ad is object)
                        ret.Add(ad);
                }

                return ret;
            }

            /// <summary>
            /// ID dell'applicazione
            /// </summary>
            public int ApplicationID
            {
                get
                {
                    return DBUtils.GetID(m_Application, m_ApplicationID);
                }

                set
                {
                    int oldValue = ApplicationID;
                    if (oldValue == value)
                        return;
                    m_Application = null;
                    m_ApplicationID = value;
                    DoChanged("ApplicationID", value, oldValue);
                }
            }

            /// <summary>
            /// Applicazione
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    if (m_Application is null)
                        m_Application = Mails.Applications.GetItemById(m_ApplicationID);
                    return m_Application;
                }

                set
                {
                    var oldValue = Application;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Application = value;
                    m_ApplicationID = DBUtils.GetID(value, 0);
                    DoChanged("Application", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'applicazione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetApplication(MailApplication value)
            {
                m_Application = value;
                m_ApplicationID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restitusice o imposta il percorso in cui il messaggio è stato archiviato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Folder
            {
                get
                {
                    if (m_Folder is null && Application is object)
                        m_Folder = Application.GetFolderById(m_FolderID);
                    if (m_Folder is null)
                        m_Folder = Mails.Folders.GetItemById(m_FolderID);
                    return m_Folder;
                }

                set
                {
                    var oldValue = m_Folder;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Folder = value;
                    m_FolderID = DBUtils.GetID(value, 0);
                    DoChanged("Folder", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il folder
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetFolder(MailFolder value)
            {
                m_Folder = value;
                m_FolderID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// ID della cartella
            /// </summary>
            public int FolderID
            {
                get
                {
                    return DBUtils.GetID(m_Folder, m_FolderID);
                }

                set
                {
                    int oldValue = FolderID;
                    if (oldValue == value)
                        return;
                    m_FolderID = value;
                    m_Folder = null;
                    DoChanged("FolderID", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'account
            /// </summary>
            public int AccountID
            {
                get
                {
                    return DBUtils.GetID(m_Account, m_AccountID);
                }

                set
                {
                    int oldValue = AccountID;
                    if (oldValue == value)
                        return;
                    m_AccountID = value;
                    m_Account = null;
                    DoChanged("AccountID", value, oldValue);
                }
            }

            /// <summary>
            /// Account
            /// </summary>
            public MailAccount Account
            {
                get
                {
                    if (m_Account is null && Application is object)
                        m_Account = m_Application.Accounts.GetItemById(m_AccountID);
                    if (m_Account is null)
                        m_Account = Mails.Accounts.GetItemById(m_AccountID);
                    return m_Account;
                }

                internal set
                {
                    var oldValue = m_Account;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Account = value;
                    m_AccountID = DBUtils.GetID(value, 0);
                    m_AccountName = "";
                    if (value is object)
                        m_AccountName = value.AccountName;
                    DoChanged("Account", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'account
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetAccount(MailAccount value)
            {
                m_Account = value;
                m_AccountID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'account utilizzate per scaricare il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AccountName
            {
                get
                {
                    if (m_Account is object)
                        return m_Account.AccountName;
                    return m_AccountName;
                }

                internal set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AccountName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AccountName = value;
                    m_Account = null;
                    DoChanged("Account", value, oldValue);
                }
            }

             
            /// <summary>
            /// Gets the delivery date.
            /// </summary>
            /// <value>The delivery date.</value>
            public DateTime? DeliveryDate
            {
                get
                {
                    return m_DeliveryDate;
                }

                internal set
                {
                    var oldValue = m_DeliveryDate;
                    if (oldValue == value == true)
                        return;
                    m_DeliveryDate = value;
                    DoChanged("DeliveryDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui il messaggio è stato scaricato dal server
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DownloadDate
            {
                get
                {
                    return m_DownloadDate;
                }

                set
                {
                    var oldValue = m_DownloadDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DownloadDate = value;
                    DoChanged("DownloadDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui il messaggio è stato marcato come letto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? ReadDate
            {
                get
                {
                    return m_ReadDate;
                }

                set
                {
                    var oldValue = m_ReadDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_ReadDate = value;
                    DoChanged("ReadDate", value, oldValue);
                }
            }



            /// <summary>
            /// Gets the message id.
            /// </summary>
            /// <value>The message id.</value>
            public string MessageId
            {
                get
                {
                    return m_MessageId;
                }

                internal set
                {
                    string oldValue = m_MessageId;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MessageId = value;
                    DoChanged("MessageId", value, oldValue);
                }
            }

            /// <summary>
            /// ReplyToMessageId
            /// </summary>
            public string ReplyToMessageId
            {
                get
                {
                    return m_ReplyToMessageId;
                }

                internal set
                {
                    string oldValue = m_ReplyToMessageId;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ReplyToMessageId = value;
                    DoChanged("ReplyToMessageId", value, oldValue);
                }
            }

            /// <summary>
            /// Gets the MIME version.
            /// </summary>
            /// <value>The MIME version.</value>
            public string MimeVersion
            {
                get
                {
                    return GetHeader(MimeHeaders.MimeVersion);
                }
            }

            /// <summary>
            /// Gets the content id.
            /// </summary>
            /// <value>The content id.</value>
            public string ContentId
            {
                get
                {
                    return GetHeader(MimeHeaders.ContentId);
                }
            }

            /// <summary>
            /// Gets the content description.
            /// </summary>
            /// <value>The content description.</value>
            public string ContentDescription
            {
                get
                {
                    return GetHeader(MimeHeaders.ContentDescription);
                }
            }

            /// <summary>
            /// Gets the content disposition.
            /// </summary>
            /// <value>The content disposition.</value>
            public ContentDisposition ContentDisposition
            {
                get
                {
                    string contentDisposition1 = GetHeader(MimeHeaders.ContentDisposition);
                    if (string.IsNullOrEmpty(contentDisposition1))
                        return null;
                    return new ContentDisposition(contentDisposition1);
                }
            }

            /// <summary>
            /// Gets the type of the content.
            /// </summary>
            /// <value>The type of the content.</value>
            public ContentType ContentType
            {
                get
                {
                    string contentType1 = GetHeader(MimeHeaders.ContentType);
                    if (string.IsNullOrEmpty(contentType1))
                        return null;
                    return MimeReader.GetContentType(contentType1);
                }
            }

            /// <summary>
            /// Gets the header.
            /// </summary>
            /// <param name="header">The header.</param>
            /// <returns></returns>
            private string GetHeader(string header)
            {
                return GetHeader(header, false);
            }

            private string GetHeader(string header, bool stripBrackets)
            {
                if (stripBrackets)
                    return MimeEntity.TrimBrackets(Headers[header]);
                return Headers[header];
            }


            string[] IIndexable.GetIndexedWords() { return this.GetIndexedWords();  }
            string[] IIndexable.GetKeyWords() { return this.GetKeyWords();  }

            /// <summary>
            /// Restituisce la collezione degli allegati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailAttachmentCollection Attachments
            {
                get
                {
                    if (m_Attachements is null)
                        m_Attachements = new MailAttachmentCollection(this); // , Me.m_Message.Attachments)
                    return m_Attachements;
                }
            }

            /// <summary>
            /// Restituisce il corpo del messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Body
            {
                get
                {
                    if (string.IsNullOrEmpty(m_Body))
                    {
                        string path = @"mail\app\" + ApplicationID;
                        path = Path.Combine(Sistema.ApplicationContext.SystemDataFolder, path);
                        // FileSystem.CreateRecursiveFolder(path)
                        path = path + @"\msg" + ID + ".dat";
                        m_Body = File.ReadAllText(path);
                    }

                    return m_Body;
                }

                set
                {
                    string oldValue = Body;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Body = value;
                    DoChanged("Body", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce la collezione degli indirizzi destinatari in Copia Carbone
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailAddressCollection Cc
            {
                get
                {
                    if (m_Cc is null)
                    {
                        checkAddressies();
                        m_Cc = new MailAddressCollection(this, "cc");
                    }

                    return m_Cc;
                }
            }

            /// <summary>
            /// Restituisce la collezione degli indirizzi destinatari in Copia Carbone Nascosta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailAddressCollection Bcc
            {
                get
                {
                    if (m_Bcc is null)
                    {
                        checkAddressies();
                        m_Bcc = new MailAddressCollection(this, "bcc");
                    }

                    return m_Bcc;
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica se richiedere o meno la conferma di consegna
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public System.Net.Mail.DeliveryNotificationOptions DeliveryNotificationOptions
            {
                get
                {
                    return m_DeliveryNotificationOptions;
                }

                set
                {
                    var oldValue = m_DeliveryNotificationOptions;
                    if (oldValue == value)
                        return;
                    m_DeliveryNotificationOptions = value;
                    DoChanged("DeliveryNotificationOptions", value, oldValue);
                }
            }

            private void checkAddressies()
            {
                if (DBUtils.GetID(this, 0) != 0 && m_OriginalAddresses is null)
                {
                    SetOriginalAddressList(GetOriginalAdressies());
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo del mittente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailAddress From
            {
                get
                {
                    if (m_From is null)
                        checkAddressies();
                    if (m_From is null)
                        m_From = new MailAddress(this, "from");
                    return m_From;
                }

                set
                {
                    var oldValue = From;
                    if (value is null)
                        value = new MailAddress();
                    if (oldValue.Equals(value))
                        return;
                    m_From.Address = value.Address;
                    m_From.DisplayName = value.DisplayName;
                    DoChanged("From", value, oldValue);
                }
            }

            /// <summary>
            /// Headers del messaggio
            /// </summary>
            public CKeyCollection<string> Headers
            {
                get
                {
                    if (m_Headers is null)
                        m_Headers = new CKeyCollection<string>();
                    return m_Headers;
                }
            }

            /// <summary>
            /// Restituisce true se il messaggio definisce un corpo di tipo HTML
            /// </summary>
            public bool IsBodyHtml
            {
                get
                {
                    return m_IsBodyHtml;
                }

                set
                {
                    if (IsBodyHtml == value)
                        return;
                    m_IsBodyHtml = value;
                    DoChanged("IsBodyHtml", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la priorità del messaggio
            /// </summary>
            public MailPriority Priority
            {
                get
                {
                    return m_Priority;
                }

                set
                {
                    var oldValue = Priority;
                    if (oldValue == value)
                        return;
                    m_Priority = value;
                    DoChanged("Priority", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo da usare per le risposte
            /// </summary>
            public MailAddress ReplyTo
            {
                get
                {
                    if (m_ReplyTo is null)
                        checkAddressies();
                    if (m_ReplyTo is null)
                        m_ReplyTo = new MailAddress(this, "reply-to");
                    return m_ReplyTo;
                }

                set
                {
                    var oldValue = ReplyTo;
                    if (value is null)
                        value = new MailAddress();
                    if (oldValue.Equals(value))
                        return;
                    m_ReplyTo.Address = value.Address;
                    m_ReplyTo.DisplayName = value.DisplayName;
                    DoChanged("ReplyTo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo della persona a cui é stato recapitato il messaggio
            /// </summary>
            public MailAddress DeliveredTo
            {
                get
                {
                    if (m_DeliveredTo is null)
                        checkAddressies();
                    if (m_DeliveredTo is null)
                        m_DeliveredTo = new MailAddress(this, "delivered-to");
                    return m_DeliveredTo;
                }

                set
                {
                    var oldValue = DeliveredTo;
                    if (value is null)
                        value = new MailAddress();
                    if (oldValue.Equals(value))
                        return;
                    m_DeliveredTo.Address = value.Address;
                    m_DeliveredTo.DisplayName = value.DisplayName;
                    DoChanged("DeliveredTo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo del mittente
            /// </summary>
            public MailAddress Sender
            {
                get
                {
                    if (m_Sender is null)
                        checkAddressies();
                    if (m_Sender is null)
                        m_Sender = new MailAddress(this, "sender");
                    return m_Sender;
                }

                set
                {
                    var oldValue = Sender;
                    if (value is null)
                        value = new MailAddress();
                    if (oldValue.Equals(value))
                        return;
                    m_Sender.Address = value.Address;
                    m_Sender.DisplayName = value.DisplayName;
                    DoChanged("Sender", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto del messaggio
            /// </summary>
            public string Subject
            {
                get
                {
                    return m_Subject;
                }

                set
                {
                    string oldValue = Subject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Subject = value;
                    DoChanged("Subject", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione dei destinatari
            /// </summary>
            public MailAddressCollection To
            {
                get
                {
                    if (m_To is null)
                    {
                        checkAddressies();
                        m_To = new MailAddressCollection(this, "to");
                    }

                    return m_To;
                }
            }

            private CKeyCollection<string> CreateHeaders(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return new CKeyCollection<string>();
                }
                else
                {
                    CKeyCollection tmp = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(str);
                    var ret = new CKeyCollection<string>();
                    foreach (string k in tmp.Keys)
                        ret.Add(k, DMD.Strings.CStr(tmp[k]));
                    return ret;
                }
            }

            private System.Text.Encoding GetEncoding(string str)
            {
                str = Strings.Trim(str);
                if (string.IsNullOrEmpty(str))
                    return null;
                return System.Text.Encoding.GetEncoding(str);
            }

            private string GetStr(MailAddressEx addr)
            {
                if (addr is null)
                    return "";
                return addr.ToString();
            }

            private string GetStr(MailAddressCollectionEx addrs)
            {
                if (addrs is null)
                    return "";
                return addrs.ToString();
            }

            private string GetStr(System.Text.Encoding enc)
            {
                if (enc is null)
                    return "";
                return enc.WebName;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_EmailMessages";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_ApplicationID = reader.Read("ApplicationID", m_ApplicationID);
                m_AccountID = reader.Read("AccountID", m_AccountID);
                m_FolderID = reader.Read("FolderID", m_FolderID);
                m_Headers = CreateHeaders(reader.Read("Headers", ""));
                m_Subject = reader.Read("Subject",  m_Subject);
                m_DeliveryDate = reader.Read("DeliveryDate",  m_DeliveryDate);
                m_DownloadDate = reader.Read("DownloadDate",  m_DownloadDate);
                m_ReadDate = reader.Read("ReadDate",  m_ReadDate);
                m_MessageId = reader.Read("MessageId",  m_MessageId);
                m_ReplyToMessageId = reader.Read("ReplyToMessageId",  m_ReplyToMessageId);
                m_IsBodyHtml = reader.Read("BodyHtml",  m_IsBodyHtml);
                m_DeliveryNotificationOptions = reader.Read("DeliveryNotificationOptions", m_DeliveryNotificationOptions);
                m_Priority = reader.Read("Priority", m_Priority);
                m_Body = DMD.Strings.vbNullString;
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                if (m_Attachements is object)
                {
                    SetFlag(MailFlags.Attachments, m_Attachements.Count > 0);
                }

                writer.Write("ApplicationID", ApplicationID);
                writer.Write("AccountID", AccountID);
                writer.Write("FolderID", FolderID);
                if (m_Headers is object)
                    writer.Write("Headers", DMD.XML.Utils.Serializer.Serialize(Headers, DMD.XML.XMLSerializeMethod.Document));
                writer.Write("Subject", m_Subject);
                writer.Write("DeliveryDate", m_DeliveryDate);
                writer.Write("DownloadDate", m_DownloadDate);
                writer.Write("ReadDate", m_ReadDate);
                writer.Write("MessageId", m_MessageId);
                writer.Write("ReplyToMessageId", m_ReplyToMessageId);
                writer.Write("BodyHtml", m_IsBodyHtml);
                writer.Write("DeliveryNotificationOptions", m_DeliveryNotificationOptions);
                writer.Write("Priority", m_Priority);
                writer.Write("From", From.Address);
                writer.Write("DeliveredTo", DeliveredTo.Address);
                // writer.Write("To", DMD.XML.Utils.Serializer.Serialize(Me.To))
                // writer.Write("Cc", DMD.XML.Utils.Serializer.Serialize(Me.Cc))
                // writer.Write("Bcc", DMD.XML.Utils.Serializer.Serialize(Me.Bcc))


                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                m_ApplicationID = reader.Read("ApplicationID", m_ApplicationID);
                m_AccountID = reader.Read("AccountID", m_AccountID);
                m_FolderID = reader.Read("FolderID", m_FolderID);
                m_Headers = CreateHeaders(reader.Read("Headers", ""));
                m_Subject = reader.Read("Subject", m_Subject);
                m_DeliveryDate = reader.Read("DeliveryDate", m_DeliveryDate);
                m_DownloadDate = reader.Read("DownloadDate", m_DownloadDate);
                m_ReadDate = reader.Read("ReadDate", m_ReadDate);
                m_MessageId = reader.Read("MessageId", m_MessageId);
                m_ReplyToMessageId = reader.Read("ReplyToMessageId", m_ReplyToMessageId);
                m_IsBodyHtml = reader.Read("BodyHtml", m_IsBodyHtml);
                m_DeliveryNotificationOptions = reader.Read("DeliveryNotificationOptions", m_DeliveryNotificationOptions);
                m_Priority = reader.Read("Priority", m_Priority);
                m_Body = DMD.Strings.vbNullString;
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                m_ApplicationID = reader.Read("ApplicationID", m_ApplicationID);
                m_AccountID = reader.Read("AccountID", m_AccountID);
                m_FolderID = reader.Read("FolderID", m_FolderID);
                m_Headers = CreateHeaders(reader.Read("Headers", ""));
                m_Subject = reader.Read("Subject", m_Subject);
                m_DeliveryDate = reader.Read("DeliveryDate", m_DeliveryDate);
                m_DownloadDate = reader.Read("DownloadDate", m_DownloadDate);
                m_ReadDate = reader.Read("ReadDate", m_ReadDate);
                m_MessageId = reader.Read("MessageId", m_MessageId);
                m_ReplyToMessageId = reader.Read("ReplyToMessageId", m_ReplyToMessageId);
                m_IsBodyHtml = reader.Read("BodyHtml", m_IsBodyHtml);
                m_DeliveryNotificationOptions = reader.Read("DeliveryNotificationOptions", m_DeliveryNotificationOptions);
                m_Priority = reader.Read("Priority", m_Priority);
                m_Body = DMD.Strings.vbNullString;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                // SetFlag(MailFlags.Attachments, (Me.Attachments.Count > 0))
                writer.WriteAttribute("ApplicationID", ApplicationID);
                writer.WriteAttribute("AccountID", AccountID);
                writer.WriteAttribute("FolderID", FolderID);
                writer.WriteAttribute("DownloadDate", m_DownloadDate);
                writer.WriteAttribute("ReadDate", m_ReadDate);
                writer.WriteAttribute("DeliveryDate", m_DeliveryDate);
                writer.WriteAttribute("MessageId", m_MessageId);
                writer.WriteAttribute("ReplyToMessageId", m_ReplyToMessageId);
                writer.WriteAttribute("BodyHtml", m_IsBodyHtml);
                writer.WriteAttribute("DeliveryNotificationOptions", (int?)m_DeliveryNotificationOptions);
                writer.WriteAttribute("Priority", (int?)m_Priority);
                writer.WriteAttribute("Subject", m_Subject);
                base.XMLSerialize(writer);
                checkAddressies();
                // If (writer.Settings.GetValueBool("OnlyHeaders", False) = False) Then
                // writer.WriteTag("Headers", Me.Headers)
                // End If
                // writer.WriteTag("Attachments", Me.Attachments())
                writer.WriteTag("Addressies", GetCurrentAddressList());
                // writer.WriteTag("Body", Me.Body)
            }

            private MailAddressEx ParseXMLRetAddress(object fieldValue)
            {
                string str = DMD.Strings.Trim(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                if (string.IsNullOrEmpty(str))
                    return null;
                return new MailAddressEx(str);
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
                    case "ApplicationID":
                        {
                            m_ApplicationID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AccountID":
                        {
                            m_AccountID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "FolderID":
                        {
                            m_FolderID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DownloadDate":
                        {
                            m_DownloadDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ReadDate":
                        {
                            m_ReadDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DeliveryDate":
                        {
                            m_DeliveryDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MessageId":
                        {
                            m_MessageId = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ReplyToMessageId":
                        {
                            m_ReplyToMessageId = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); // Me.ReplyToMessageId)
                            break;
                        }
 
                    case "BodyHtml":
                        {
                            m_IsBodyHtml = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DeliveryNotificationOptions":
                        {
                            m_DeliveryNotificationOptions = (System.Net.Mail.DeliveryNotificationOptions)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Priority":
                        {
                            m_Priority = (MailPriority)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Subject":
                        {
                            m_Subject = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attachments":
                        {
                            m_Attachements = (MailAttachmentCollection)fieldValue;
                            m_Attachements.SetOwner(this);
                            break;
                        }

                    case "Headers":
                        {
                            fieldValue = DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            if (fieldValue is object)
                            {
                                m_Headers = new CKeyCollection<string>();
                                CKeyCollection tmp = (CKeyCollection)fieldValue;
                                foreach (string k in tmp.Keys)
                                    m_Headers.Add(k, DMD.Strings.CStr(tmp[k]));
                            }

                            break;
                        }

                    case "Addressies":
                        {
                            SetCurrentAddressList((CCollection)fieldValue);
                            break;
                        }

                    case "Body":
                        {
                            m_Body = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return 
                    DMD.Strings.ConcatArray(
                        Sistema.Formats.FormatUserDateTime(m_DeliveryDate) , 
                        ", to: " , m_To.ToString() ,
                        ", Subject: " , m_Subject
                        );
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new MailFlags Flags
            {
                get
                {
                    return (MailFlags) m_Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (value == oldValue)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Testa il flag
            /// </summary>
            /// <param name="flag"></param>
            /// <returns></returns>
            public bool TestFlag(MailFlags flag)
            {
                return DMD.RunTime.TestFlag(this.Flags, flag);
            }

            /// <summary>
            /// Imposta il flag
            /// </summary>
            /// <param name="flag"></param>
            /// <param name="value"></param>
            public void SetFlag(MailFlags flag, bool value)
            {
                if (TestFlag(flag) == value)
                    return;
                var oldValue = this.Flags;
                m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, flag, value);
                DoChanged("Flags", value, oldValue);
            }

            private string GetFileName()
            {
                lock (Sistema.ApplicationContext)
                {
                    string ret = @"mail\app\" + ApplicationID + @"\msg" + ID + ".dat";
                    string fileName = Path.Combine(Sistema.ApplicationContext.SystemDataFolder, ret);
                    File.WriteAllText(fileName, "");
                    return fileName;
                }
            }

             

            public override void Save(bool force = false)
            {
                base.Save(force);
                
                string path = @"mail\app\" + ApplicationID;
                path = Path.Combine(Sistema.ApplicationContext.SystemDataFolder, path);
                Sistema.FileSystem.CreateRecursiveFolder(path);
                path = path + @"\msg" + ID + ".dat";
                
                if (Strings.Len(Body) == 0)
                {
                    Debug.Print("oops");
                }
                File.WriteAllText(path, Body);

                if (m_Attachements is object)
                    m_Attachements.Save(force);

                var original = GetOriginalAdressies();
                var saved = SaveAddresses();
                foreach (MailAddress a in saved)
                {
                    int i = 0;
                    while (i < original.Count)
                    {
                        var o = (MailAddress)original[i];
                        if (ReferenceEquals(a, o) || DBUtils.GetID(a) == DBUtils.GetID(o)) // (a.Address = o.Address AndAlso a.FieldName = o.FieldName) Then
                        {
                            original.RemoveAt(i);
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                }

                foreach (MailAddress a in original)
                    a.Delete();


                if (Stato == ObjectStatus.OBJECT_VALID)
                {
                    Mails.Index.Index(this);
                }
                else
                {
                    Mails.Index.Unindex(this);
                }
            }

            private CCollection SaveAddresses()
            {
                var col = GetCurrentAddressList();
                foreach (MailAddress m in col)
                {
                    m.SetApplication(Application);
                    m.SetMessage(this);
                    m.Save(true);
                }

                return col;
            }

            internal CCollection GetCurrentAddressList()
            {
                var col = new CCollection();
                if (m_From is object)
                {
                    m_From.FieldName = "from";
                    col.Add(m_From);
                }

                if (m_ReplyTo is object)
                {
                    m_ReplyTo.FieldName = "reply-to";
                    col.Add(m_ReplyTo);
                }

                if (m_Sender is object)
                {
                    m_Sender.FieldName = "sender";
                    col.Add(m_Sender);
                }

                if (m_DeliveredTo is object)
                {
                    m_DeliveredTo.FieldName = "delivered-to";
                    col.Add(m_DeliveredTo);
                }

                if (m_To is object)
                {
                    foreach (MailAddress m in m_To)
                    {
                        m.FieldName = "to";
                        col.Add(m);
                    }
                }

                if (m_Cc is object)
                {
                    foreach (MailAddress m in m_Cc)
                    {
                        m.FieldName = "cc";
                        col.Add(m);
                    }
                }

                if (m_Bcc is object)
                {
                    foreach (MailAddress m in m_Bcc)
                    {
                        m.FieldName = "bcc";
                        col.Add(m);
                    }
                }

                return col;
            }

            internal void SetCurrentAddressList(CCollection value)
            {
                m_From = null;
                m_Sender = null;
                m_DeliveredTo = null;
                m_ReplyTo = null;
                m_To = new MailAddressCollection();
                m_To.SetFieldName("to");
                m_To.SetMessage(this);
                m_Cc = new MailAddressCollection();
                m_Cc.SetFieldName("cc");
                m_Cc.SetMessage(this);
                m_Bcc = new MailAddressCollection();
                m_Bcc.SetFieldName("bcc");
                m_Bcc.SetMessage(this);
                foreach (MailAddress a in value)
                {
                    a.SetApplication(Application);
                    a.SetMessage(this);
                    switch (a.FieldName ?? "")
                    {
                        case "from":
                            {
                                m_From = a;
                                break;
                            }

                        case "sender":
                            {
                                m_Sender = a;
                                break;
                            }

                        case "delivered-to":
                            {
                                m_DeliveredTo = a;
                                break;
                            }

                        case "reply-to":
                            {
                                m_ReplyTo = a;
                                break;
                            }

                        case "to":
                            {
                                m_To.Add(a);
                                break;
                            }

                        case "cc":
                            {
                                m_Cc.Add(a);
                                break;
                            }

                        case "bcc":
                            {
                                m_Bcc.Add(a);
                                break;
                            }
                    }
                }

                if (m_From is null)
                    m_From = new MailAddress(this, "from");
                if (m_Sender is null)
                    m_Sender = new MailAddress(this, "sender");
                if (m_DeliveredTo is null)
                    m_DeliveredTo = new MailAddress(this, "delivered-to");
                if (m_ReplyTo is null)
                    m_ReplyTo = new MailAddress(this, "reply-to");
            }

            /// <summary>
            /// Imposta l'elenco degli indirizzi originali
            /// </summary>
            /// <param name="value"></param>
            public void SetOriginalAddressList(IEnumerable<MailAddress> value)
            {
                m_From = null;
                m_Sender = null;
                m_DeliveredTo = null;
                m_ReplyTo = null;
                m_To = null;
                m_Cc = null;
                m_Bcc = null;
                foreach (MailAddress a in value)
                {
                    a.SetApplication(Application);
                    a.SetMessage(this);
                    switch (a.FieldName ?? "")
                    {
                        case "from":
                            {
                                m_From = a;
                                break;
                            }

                        case "sender":
                            {
                                m_Sender = a;
                                break;
                            }

                        case "delivered-to":
                            {
                                m_DeliveredTo = a;
                                break;
                            }

                        case "reply-to":
                            {
                                m_ReplyTo = a;
                                break;
                            }

                        case "to":
                            {
                                if (m_To is null)
                                {
                                    m_To = new MailAddressCollection();
                                    m_To.SetFieldName("to");
                                    m_To.SetMessage(this);
                                }

                                m_To.Add(a);
                                break;
                            }

                        case "cc":
                            {
                                if (m_Cc is null)
                                {
                                    m_Cc = new MailAddressCollection();
                                    m_Cc.SetFieldName("cc");
                                    m_Cc.SetMessage(this);
                                }

                                m_Cc.Add(a);
                                break;
                            }

                        case "bcc":
                            {
                                if (m_Bcc is null)
                                {
                                    m_Bcc = new MailAddressCollection();
                                    m_Bcc.SetFieldName("bcc");
                                    m_Bcc.SetMessage(this);
                                }

                                m_Bcc.Add(a);
                                break;
                            }
                    }
                }

                m_OriginalAddresses = value;
            }

            // Protected Function FindAddress(ByVal fieldName As String) as MailAddress
            // Dim items As System.Collections.ArrayList = Me.GetOriginalAdressies
            // For Each a as MailAddress In items
            // If a.FieldName = fieldName Then Return a
            // Next
            // Return Nothing
            // End Function

            protected internal List<MailAddress> GetOriginalAdressies()
            {
                if (m_OriginalAddresses is null)
                {
                    m_OriginalAddresses = new ArrayList();
                    var cursor = new MailAddressCursor();
                    cursor.IgnoreRights = true;
                    cursor.MessageID.Value = DBUtils.GetID(this);
                    cursor.ID.SortOrder = SortEnum.SORT_ASC;
                    while (!cursor.EOF())
                    {
                        var m = cursor.Item;
                        m.SetApplication(Application);
                        m.SetMessage(this);
                        m_OriginalAddresses.Add(m);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                }

                return m_OriginalAddresses;
            }






            // ' This code added by Visual Basic to correctly implement the disposable pattern.
            // Public Overridable Sub Dispose() Implements IDisposable.Dispose
            // If (Me._children IsNot Nothing) Then
            // For Each m As MailMessage In Me._children
            // m.Dispose()
            // Next
            // Me._children = Nothing
            // End If


            // If (Me.m_Attachements IsNot Nothing) Then
            // For Each a As MailAttachment In Me.m_Attachements
            // a.Dispose()
            // Next
            // Me.m_Attachements = Nothing
            // End If
            // Me.m_Headers = Nothing
            // Me.m_From = Nothing
            // Me.m_To = Nothing
            // Me.m_Cc = Nothing
            // Me.m_Bcc = Nothing
            // Me.m_ReturnAddress = Nothing
            // Me.m_Sender = Nothing
            // Me.m_ReplyTo = Nothing

            // Me.m_AccountName = vbNullString
            // Me.m_Account = Nothing
            // Me.m_Folder = Nothing
            // Me._children = Nothing
            // Me.m_AlternateViews = Nothing
            // Me.m_Attachements = Nothing
            // Me.m_Headers = Nothing
            // Me.m_From = Nothing
            // Me.m_To = Nothing
            // Me.m_Cc = Nothing
            // Me.m_Bcc = Nothing
            // Me.m_ReturnAddress = Nothing
            // Me.m_Subject = vbNullString
            // Me.m_Body = vbNullString
            // Me.m_DeliveryDate = Nothing
            // Me.m_Routing = vbNullString
            // Me.m_MessageId = vbNullString
            // Me.m_ReplyToMessageId = vbNullString
            // Me.m_BodyEncoding = Nothing
            // Me.m_SubjectEncoding = Nothing
            // Me.m_Sender = Nothing
            // Me.m_ReplyTo = Nothing
            // Me.m_DownloadDate = Nothing
            // Me.m_ReadDate = Nothing
            // End Sub


            // Friend Sub Process(ByVal message As Net.Mail.MailMessageEx)
            // 'Me.m_Object = message

            // Me.Headers.Clear()

            // For Each key As String In message.Headers.Keys
            // Me.Headers.Add(key, message.Headers(key))
            // Next

            // Me.Attachments.Clear()
            // For Each at As System.Net.Mail.Attachment In message.Attachments
            // Dim at1 As New MailAttachment
            // at1.From(at)
            // at1.Stato = ObjectStatus.OBJECT_VALID
            // Me.Attachments.Add(at1)
            // Next

            // Me.m_Flags = MailFlags.Unread
            // Me.m_From = message.From
            // Me.m_To = message.To
            // Me.m_Cc = message.CC
            // Me.m_Bcc = message.Bcc
            // Me.m_ReturnAddress = message.ReplyTo
            // Me.m_Subject = message.Subject
            // Me.m_Body = message.Body
            // Me.m_DeliveryDate = message.DeliveryDate
            // Me.m_MessageId = message.MessageId
            // Me.m_ReplyToMessageId = message.ReplyToMessageId
            // Me.m_Sender = message.Sender
            // Me.m_ReplyTo = message.ReplyTo
            // Me.m_IsBodyHtml = message.IsBodyHtml
            // Me.m_DeliveryNotificationOptions = message.DeliveryNotificationOptions
            // Me.m_Priority = message.Priority
            // End Sub

            // Function GetObject() As System.Net.Mail.MailMessage
            // Return Me.m_Object
            // End Function

            // ''' <summary>
            // ''' Sposta il messaggio nel cestino
            // ''' </summary>
            // ''' <remarks></remarks>
            // Sub TrashMe()
            // Me.MoveTo(Me.Folders.TrashBin)
            // End Sub

            public void MoveTo(MailFolder mailFolder)
            {
                Folder = mailFolder;
                Save();
            }

            public void CopyTo(MailFolder mailFolder)
            {
                MailMessage tmp = (MailMessage)MemberwiseClone();
                DBUtils.ResetID(tmp);
                tmp.Folder = mailFolder;
                foreach (MailAttachment at in Attachments)
                {
                    MailAttachment at1 = (MailAttachment)at.Clone();
                    tmp.Attachments.Add(at1);
                }

                tmp.Save();
            }

            protected virtual string[] GetIndexedWords()
            {
                var words = DMD.Arrays.Empty<string>();
                string str = Strings.UCase(Subject + " " + DMD.WebUtils.RemoveHTMLTags(Body));
                string word = "";
                int stato = 0;
                for (int i = 1, loopTo = Strings.Len(str); i <= loopTo; i++)
                {
                    string ch = Strings.Mid(str, i, 1);
                    switch (stato)
                    {
                        case 0:
                            {
                                if (char.IsDigit(DMD.Chars.CChar(ch)) || char.IsLetter(DMD.Chars.CChar(ch)))
                                {
                                    word += DMD.Strings.OnlyCharsAndNumbers(ch);
                                }
                                else if (ch == " " || (ch ?? "") == DMD.Strings.vbCr || (ch ?? "") == DMD.Strings.vbLf || (ch ?? "") == DMD.Strings.vbTab)
                                {
                                    if (DMD.Arrays.BinarySearch(words, word) < 0)
                                    {
                                        words = DMD.Arrays.InsertSorted(words, word);
                                        word = "";
                                    }
                                }
                                else
                                {
                                }

                                break;
                            }
                    }
                }

                return words;
            }

            private string[] GetKeyWords()
            {
                return DMD.Arrays.Empty<string>();
            }

            public override CModulesClass GetModule()
            {
                return Mails.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Mails.Database;
            }
        }
    }
}