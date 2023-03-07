using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;
using DMD.Net.Mail;
using DMD.Net.Mail.Protocols.POP3;
using System.Net.Mail;
using System.Net.Mime;

namespace minidom
{
    public partial class Office
    {
        
        /// <summary>
        /// Applicazione email associata ad un utente
        /// </summary>
        [Serializable]
        public class MailApplication 
            : minidom.Databases.DBObjectPO
        {
            private int m_UserID;
            [NonSerialized] private Sistema.CUser m_User;
            private string m_UserName;
            private MailApplicationRules m_Rules;
            private int m_RootID;
            [NonSerialized] private MailRootFolder m_Root;
            [NonSerialized] private MailApplicationAccounts m_Accounts;
            private readonly object receiveLock = new object();

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailApplication()
            {
                m_UserID = 0;
                m_User = null;
                m_UserName = "";
                m_Rules = null;
                m_RootID = 0;
                m_Root = null;
                m_Accounts = null;
            }

            /// <summary>
            /// Collezione di account associati all'applicazione
            /// </summary>
            public MailApplicationAccounts Accounts
            {
                get
                {
                    if (m_Accounts is null)
                        m_Accounts = new MailApplicationAccounts(this);
                    return m_Accounts;
                }
            }

            /// <summary>
            /// Utente a cui appartiene l'applicazione
            /// </summary>
            public int UserID
            {
                get
                {
                    return DBUtils.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_UserID = value;
                    m_User = null;
                    DoChanged("UserID", value, oldValue);
                }
            }

            /// <summary>
            /// Utente a cui appartiene l'applicazione
            /// </summary>
            public CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value, 0);
                    m_UserName = "";
                    if (value is object)
                        m_UserName = value.Nominativo;
                    DoChanged("User", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'utente a cui appartiene l'applicazione
            /// </summary>
            public string UserName
            {
                get
                {
                    return m_UserName;
                }

                set
                {
                    string oldValue = m_UserName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UserName = value;
                    DoChanged("UserName", value, oldValue);
                }
            }
              
            /// <summary>
            /// Regole definite per la gestione dei messaggi
            /// </summary>
            public MailApplicationRules Rules
            {
                get
                {
                    if (m_Rules is null)
                        m_Rules = new MailApplicationRules(this);
                    return m_Rules;
                }
            }

            /// <summary>
            /// ID della cartella radice 
            /// </summary>
            public int RootID
            {
                get
                {
                    return DBUtils.GetID(m_Root, m_RootID);
                }

                set
                {
                    int oldValue = RootID;
                    if (oldValue == value)
                        return;
                    m_RootID = value;
                    m_Root = null;
                    DoChanged("Root", value, oldValue);
                }
            }

            /// <summary>
            /// Cartella radice
            /// </summary>
            public MailRootFolder Root
            {
                get
                {
                    if (m_Root is null)
                    {
                        if (m_RootID != 0)
                        {
                            var cursor = new MailFolderCursor();
                            cursor.ID.Value = m_RootID;
                            cursor.IgnoreRights = true;
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            var dbRis = Mails.Database.ExecuteReader(cursor.GetSQL());
                            if (dbRis.Read())
                            {
                                m_Root = new MailRootFolder();
                                m_Root.SetApplication(this);
                                m_Root.SetUtente(User);
                                this.GetConnection().Load(m_Root, dbRis);
                            }

                            dbRis.Dispose();
                            dbRis = null;
                        }

                        if (m_Root is null)
                        {
                            m_Root = new MailRootFolder();
                            m_Root.Stato = ObjectStatus.OBJECT_VALID;
                            m_Root.Name = UserName;
                            m_Root.SetApplication(this);
                            m_Root.Utente = User;
                            m_Root.Save();
                        }
                    }

                    m_Root.SetApplication(this);
                    m_Root.SetUtente(User);
                    return m_Root;
                }
            }

            /// <summary>
            /// Restituisce la cartella in base al suo id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public MailFolder GetFolderById(int id)
            {
                if (id == 0)
                    return null;
                if (id == RootID)
                    return Root;
                var ret = Root.Childs.GetItemById(id);
                if (ret is null)
                    ret = GetFolderById(Root, id);
                return ret;
            }


            private MailFolder GetFolderById(MailFolder owner, int id)
            {
                MailFolder ret = null;
                foreach (MailFolder f in owner.Childs)
                {
                    ret = f.Childs.GetItemById(id);
                    if (ret is object)
                        break;
                }

                return ret;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Applications;
            }
 
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_MailApps";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_UserID = reader.Read("UserID", m_UserID);
                m_UserName = reader.Read("UserName", m_UserName);
                var tmp = reader.Read("Rules", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_Rules = (MailApplicationRules)DMD.XML.Utils.Serializer.Deserialize(tmp);
                this.m_RootID = reader.Read("RootID", this.m_RootID);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("UserID", UserID);
                writer.Write("UserName", m_UserName);
                writer.Write("RootID", RootID);
                writer.Write("Rules", DMD.XML.Utils.Serializer.Serialize(Rules));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("UserID", typeof(int), 1);
                c = table.Fields.Ensure("UserName", typeof(string), 255);
                c = table.Fields.Ensure("RootID", typeof(int), 1);
                c = table.Fields.Ensure("Rules", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUser", new string[] { "UserID", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxUserName", new string[] { "UserName", "RootID" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Rules", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("UserName", m_UserName);
                writer.WriteAttribute("RootID", RootID);
                base.XMLSerialize(writer);                 
                writer.WriteTag("Rules", Rules);
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
                    case "UserID":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                  
                    case "RootID":
                        {
                            m_RootID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Rules":
                        {
                            m_Rules = (MailApplicationRules)fieldValue;
                            m_Rules.SetApplication(this);
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
                return this.m_UserName;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_UserName);
            }

            private string GetDelDateStr(DateTime d)
            {
                return
                    DMD.Strings.ConcatArray(
                        Strings.PadLeft(DMD.DateUtils.Year(d), '0', 4),
                        Strings.PadLeft(DMD.DateUtils.Month(d), '0', 2),
                        Strings.PadLeft(DMD.DateUtils.Day(d), '0', 2),
                        Strings.PadLeft(DMD.DateUtils.Hour(d), '0', 2),
                        Strings.PadLeft(DMD.DateUtils.Minute(d), '0', 2),
                        Strings.PadLeft(DMD.DateUtils.Second(d), '0', 2)
                        );
            }

            private string GetTargetName(
                                    MailMessageEx message, 
                                    Sistema.CEmailAccount account = null
                                    )
            {
                string path = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "Mail");
                if (account is object)
                    path = System.IO.Path.Combine(path, account.AccountName);
                string strID = GetDelDateStr(message.DeliveryDate) + "_" + message.MessageId;
                strID = Sistema.FileSystem.RemoveSpecialChars(strID);
                return System.IO.Path.Combine(path, strID + @"\message.xml");
            }

            /// <summary>
            /// Restituisce true se il messaggio è nuovo
            /// </summary>
            /// <param name="message"></param>
            /// <param name="account"></param>
            /// <returns></returns>
            public bool IsNewMessage(
                                    MailMessageEx message, 
                                    MailAccount account
                                    )
            {
                bool ret = false;

                using (var cursor = new MailMessageCursor())
                {
                    cursor.IgnoreRights = true;
                    var d1 = message.DeliveryDate;
                    var d2 = message.DeliveryDate;
                    if (d1 != default)
                        d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Second, -5, d1);
                    d2 = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 5d, d2);
                    cursor.DeliveryDate.Between(d1, d2);
                    // cursor.DeliveryDate.Value = message.DeliveryDate
                    cursor.From.Value = Strings.Trim(message.From.Address);
                    if (string.IsNullOrEmpty(Strings.Trim(message.From.Address)))
                        cursor.From.IncludeNulls = true;
                    cursor.ApplicationID.Value = DBUtils.GetID(this, 0);
                    if (account is object)
                        cursor.AccountID.Value = DBUtils.GetID(account, 0);
                    cursor.MessageID.Value = Strings.Trim(message.MessageId);
                    if (string.IsNullOrEmpty(Strings.Trim(message.MessageId)))
                        cursor.MessageID.IncludeNulls = true;
                    // cursor.Subject.Value = message.Subject
                    // If (message.Subject = "") Then cursor.Subject.IncludeNulls = True
                    cursor.Stato.Value = ObjectStatus.OBJECT_TEMP;
                    cursor.Stato.Operator = OP.OP_NE;
                    ret = cursor.EOF();
                }
 
                if (ret == true)
                {
                    bool ret1 = IsNewMessageS(message, account);
                    if (ret1 == false)
                    {
                        System.Diagnostics.Debug.Print("Attenzione");
                    }
                }

                return ret;                

            }

            private bool IsNewMessageS(
                                    MailMessageEx message, 
                                    MailAccount account
                                    )
            {
                bool ret = false;
                using (var cursor = new MailMessageCursor()) { 
                    cursor.IgnoreRights = true;
                    // Dim d1 As Date = Calendar.DateAdd(DateTimeInterval.Second, -2, message.DeliveryDate)
                    // Dim d2 As Date = Calendar.DateAdd(DateTimeInterval.Second, 2, message.DeliveryDate)
                    // cursor.DeliveryDate.Between(d1, d2)
                    cursor.DeliveryDate.Value = message.DeliveryDate;
                    cursor.From.Value = Strings.Trim(message.From.Address);
                    if (string.IsNullOrEmpty(Strings.Trim(message.From.Address)))
                        cursor.From.IncludeNulls = true;
                    cursor.ApplicationID.Value = DBUtils.GetID(this, 0);
                    if (account is object)
                        cursor.AccountID.Value = DBUtils.GetID(account, 0);
                    cursor.MessageID.Value = Strings.Trim(message.MessageId);
                    if (string.IsNullOrEmpty(Strings.Trim(message.MessageId)))
                        cursor.MessageID.IncludeNulls = true;
                    // cursor.Subject.Value = message.Subject
                    // If (message.Subject = "") Then cursor.Subject.IncludeNulls = True
                    cursor.Stato.Value = ObjectStatus.OBJECT_TEMP;
                    cursor.Stato.Operator = OP.OP_NE;
                    ret = cursor.EOF();
                }
                
                return ret;
            }

            /// <summary>
            /// Salva il messaggio
            /// </summary>
            /// <param name="message"></param>
            /// <param name="account"></param>
            /// <returns></returns>
            public MailMessage SaveMessage(
                                        MailMessageEx message, 
                                        MailAccount account
                                        )
            {
                var mail = new MailMessage();
                mail.SetApplication(this);
                mail.Account = account;
                if (account.DefaultFolder is null || account.DefaultFolder.Stato != ObjectStatus.OBJECT_VALID)
                {
                    mail.Folder = Root.Inbox;
                }
                else
                {
                    mail.Folder = account.DefaultFolder;
                }

                mail.FromMessage(message);
                mail.Stato = ObjectStatus.OBJECT_VALID;
                mail.ForceUser(User);
                mail.Save(true);
                return mail;
            }

            private CCollection<MailMessage> DownloadPOP3(MailAccount account, int maxItems)
            {
                List<Pop3ListItem> list;
                Pop3ListItem item;
                
                var oraInizio = DMD.DateUtils.Now();

                var ret = new CCollection<MailMessage>();
                var messagesToDispose = new CCollection<MailMessageEx>();

                using (var client = new Pop3Client(account.UserName, account.Password, account.ServerName, account.ServerPort, account.UseSSL))
                {
                    client.CustomCerfificateValidation = true; // Not account.val.SMTPValidateCertificate
                    client.Connect();
                    client.Authenticate();
                    client.Stat();

                    // Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
                    // Scarichiamo i messaggi successivi
                    list = client.List();
                    for (int i = list.Count - 1; i >= 0; i -= 1)
                    {
                        if (maxItems > 0 && ret.Count >= maxItems)
                            break;
                        item = list[i];
                        var message = client.Top(item.MessageId, 10);
                        if (IsNewMessage(message, account))
                        {
                            message = client.RetrMailMessageEx(item);
                            var mail = SaveMessage(message, account);
                            ret.Add(mail);
                        }

                        if (account.DelServerAfterNDays && DMD.DateUtils.DateDiff(DateTimeInterval.Day, message.DeliveryDate, DMD.DateUtils.ToDay()) > account.DelServerAfterDays)
                        {
                            client.Dele(item);
                        }

                        messagesToDispose.Add(message);
                    }

                    // For Each message In messagesToDelete
                    // client.Dele(message)
                    // Next

                    client.Quit();
                    client.Disconnect();
                }

                foreach (var m in messagesToDispose)
                    m.Dispose();

                return ret;
            }

            /// <summary>
            /// Scarica le email
            /// </summary>
            /// <param name="maxItems"></param>
            /// <returns></returns>
            public CCollection<MailMessage> DownloadEMails(int maxItems = 0)
            {
                lock (receiveLock)
                {
                    var ret = new CCollection<MailMessage>();
                    if (maxItems > 0)
                    {
                        foreach (var account in this.Accounts)
                        {
                            var tmp = DownloadEMails(account, maxItems);
                            ret.AddRange(tmp);
                            maxItems -= tmp.Count;
                            if (maxItems <= 0)
                                break;
                        }
                    }
                    else
                    {
                        foreach (var account in this.Accounts)
                        {
                            var tmp = DownloadEMails(account);
                            ret.AddRange(tmp);
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Scarica le email
            /// </summary>
            /// <param name="account"></param>
            /// <param name="maxItems"></param>
            /// <returns></returns>
            public CCollection<MailMessage> DownloadEMails(MailAccount account, int maxItems = 0)
            {
                lock (receiveLock)
                {
                    if (account is null)
                        throw new ArgumentNullException("account");
                    // If (Arrays.BinarySearch(Me.downloadingAccounts, account.AccountName) >= 0) Then Throw New Exception("Già si sta scaricando la posta da " & account.ID)

                    switch (Strings.LCase(account.Protocol) ?? "")
                    {
                        case "pop3":
                            {
                                return DownloadPOP3(account, maxItems);
                            }

                        default:
                            {
                                throw new NotSupportedException("Protocol: " + account.Protocol);                                 
                            }
                    }
                }
            }

            /// <summary>
            /// Restituisce il messaggio in base all'id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public MailMessage GetMessageById(int id)
            {
                var m = this.Mails.GetItemById(id);
                if (m is object)
                    m.SetApplication(this);
                return m;
            }

            /// <summary>
            /// Cerca i messaggi
            /// </summary>
            /// <param name="text"></param>
            /// <param name="maxItems"></param>
            /// <returns></returns>
            public CCollection<MailMessage> FindMessages(string text, int maxItems)
            {
                var ret = new CCollection<MailMessage>();
                var items = Mails.Index.Find(text, default); // filter.nMax)
                var tmp = new ArrayList();
                
                foreach (var res in items)
                    tmp.Add(res.OwnerID);

                int[] arr = (int[])tmp.ToArray(typeof(int));

                using (var cursor = new MailMessageCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ID.ValueIn(arr);
                    cursor.ApplicationID.Value = DBUtils.GetID(this, 0);
                    while (cursor.Read() && (maxItems <= 0 || ret.Count < maxItems))
                    {
                        var mail = cursor.Item;
                        mail.SetApplication(this);
                        ret.Add(mail);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Imposta il messaggio dal file
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="folder"></param>
            /// <param name="account"></param>
            /// <param name="skipExisting"></param>
            /// <returns></returns>
            public MailMessage ImportMessageFromFile(
                                            string fileName, 
                                            MailFolder folder, 
                                            MailAccount account = null, 
                                            bool skipExisting = true
                                            )
            {
                fileName = Strings.Trim(fileName);
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentNullException("fileName");
                if (folder is null)
                    throw new ArgumentNullException("folder");
                
                
                var ret = new MailMessage();
                using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
                {
                    using (var m = MailMessageEx.FromStream(stream))
                    {
                        ret.SetApplication(this);
                        ret.FromMessage(m);
                        if (!skipExisting || IsNewMessage(m, account))
                        {
                            ret.Account = account;
                            ret.Folder = folder;
                            ret.Stato = ObjectStatus.OBJECT_VALID;
                            ret.Save();
                        }
                    }
                }
                return ret;
            }

            /// <summary>
            /// Invia il messaggo e ne salva una copia nella cartella sent
            /// </summary>
            /// <param name="msg"></param>
            public void SendMessage(MailMessage msg)
            {
                if (msg is null)
                    throw new ArgumentNullException("msg");
                if (msg.Account is null)
                    throw new ArgumentNullException("account non definito");

                MailAccount sendAcc = null;
                foreach (var acc in this.Accounts)
                {
                    if (DBUtils.GetID(msg.Account, 0) == DBUtils.GetID(acc, 0))
                    {
                        sendAcc = acc;
                        break;
                    }
                }

                if (sendAcc is null)
                    throw new Exception("account non valido");

                string addr = msg.Account.eMailAddress;
                string userName = msg.Account.SMTPUserName;
                string userPass = msg.Account.SMTPPassword;
                if (string.IsNullOrEmpty(userName))
                {
                    userName = msg.Account.UserName;
                    userPass = msg.Account.Password;
                }

                string dispName = msg.Account.DisplayName;
                string svrName = msg.Account.SMTPServerName;
                int svrPort = msg.Account.SMTPPort;
                var useCryp = msg.Account.SMTPCrittografia;

                using (var client = new SmtpClient(svrName, svrPort))
                {
                    client.UseDefaultCredentials = string.IsNullOrEmpty(userName);
                    client.EnableSsl = useCryp == SMTPTipoCrittografica.SSL;
                    client.Credentials = new System.Net.NetworkCredential(userName, userPass);
                    if (string.IsNullOrEmpty(msg.From.Address))
                        msg.From = new MailAddress(addr, dispName);
                    if (string.IsNullOrEmpty(msg.Sender.Address))
                        msg.Sender = new MailAddress(addr, dispName);
                    msg.DeliveryDate = DMD.DateUtils.Now();
                    
                    var m = new MailMessageEx();
                    m.From = new MailAddressEx(msg.From.Address, msg.From.DisplayName);
                    m.Sender = new MailAddressEx(msg.Sender.Address, msg.Sender.DisplayName);
                    m.Subject = msg.Subject;
                    m.IsBodyHtml = msg.IsBodyHtml;
                    m.Body = msg.Body;
                    foreach (var att in msg.Attachments)
                    {
                        var fileName = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, att.FileName);
                        var a = new AttachmentEx(fileName, new ContentType(att.ContentType));
                        m.Attachments.Add(a);
                    }

                    foreach (var madd in msg.To)
                        m.To.Add(new MailAddressEx(madd.Address, madd.DisplayName));
                    foreach (var madd in msg.Cc)
                        m.CC.Add(new MailAddressEx(madd.Address, madd.DisplayName));
                    foreach (var madd in msg.Bcc)
                        m.Bcc.Add(new MailAddressEx(madd.Address, madd.DisplayName));
                    client.Send(m);

                    msg.Folder = Root.Sent;
                    msg.Stato = ObjectStatus.OBJECT_VALID;
                    msg.Save();
                    
                    if (m is object)
                    {
                        foreach (Attachment a in m.Attachments)
                            a.Dispose();
                        m.Dispose();
                        m = null;
                    }

                }
            }

            /// <summary>
            /// Aggiorna la cartella
            /// </summary>
            /// <param name="folder"></param>
            protected internal void UpdateFolder(MailFolder folder)
            {
                if (DBUtils.GetID(folder) == DBUtils.GetID(Root))
                {
                    m_Root = (MailRootFolder)folder;
                }
                else
                {
                    Root.UpdateFolder(folder);
                }
            }
        }
    }
}