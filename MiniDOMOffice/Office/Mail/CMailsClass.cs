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

namespace minidom
{
    namespace repositories
    {


        /// <summary>
        /// Repository di <see cref="MailMessage"/>
        /// </summary>
        [Serializable]
        public class CMailsClass 
            : CModulesClass<MailMessage>
        {
             
            /// <summary>
            /// Evento generato quando viene terminato il download di una nuova email
            /// </summary>            
            /// <remarks></remarks>
            public event EmailReceivedEventHandler EmailReceived;

            

            /// <summary>
            /// Evento generato quando viene terminato l'invio di una email
            /// </summary>            
            /// <remarks></remarks>
            public event EmailSentEventHandler EmailSent;

           

            /// <summary>
            /// Evento generato quando si verifica un errore in fase di download dei messaggi
            /// </summary>           
            /// <remarks></remarks>
            public event DownloadExceptionEventHandler DownloadException;

         
            private CMailApplications m_Applications;
            private CMailAccounts m_Accounts;
            private CFoldersClass m_Folders;
            private CMailAttachments m_Attachments;
            private MailApplicationRules m_Rules;
             

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMailsClass() 
                : base("modOfficeEMails", typeof(MailMessageCursor), 0)
            {
                m_Applications = null;
                m_Accounts = null;
                m_Attachments = null;
                m_Folders = null;
                m_Rules = null;
            }

         
            //public CIndexingService Index
            //{
            //    get
            //    {
            //        lock (this)
            //        {
            //            if (m_Index is null)
            //            {
            //                m_Index = new CIndexingService(Database);
            //                m_Index.WordIndexFolder = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, @"mails\wordindex\");
            //                Sistema.FileSystem.CreateRecursiveFolder(m_Index.WordIndexFolder);
            //            }

            //            return m_Index;
            //        }
            //    }
            //}
            /// <summary>
            /// Repository di oggetti <see cref="MailApplication"/>
            /// </summary>

            public CMailApplications Applications
            {
                get
                {
                    lock (this)
                    {
                        if (m_Applications is null)
                            m_Applications = new CMailApplications();
                        return m_Applications;
                    }
                }
            }

            /// <summary>
            /// Repository di oggetti <see cref="MailAttachment"/>
            /// </summary>
            public CMailAttachments Attachments
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attachments is null)
                            m_Attachments = new CMailAttachments();
                        return m_Attachments;
                    }
                }
            }

            /// <summary>
            /// Repository di oggetti <see cref="MailRule"/>
            /// </summary>
            public MailApplicationRules Rules
            {
                get
                {
                    lock (this)
                    {
                        if (m_Rules is null)
                            m_Rules = new minidom.Office.MailApplicationRules();
                        return m_Rules;
                    }
                }
            }

            protected string[] SplitWords(string text)
            {
                var words = DMD.Arrays.Empty<string>();
                string str = Strings.UCase(text);
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
                                else if (DMD.Arrays.BinarySearch(words, word) < 0)
                                {
                                    words = DMD.Arrays.InsertSorted(words, word);
                                    word = "";
                                }

                                break;
                            }
                    }
                }

                return words;
            }

            public CCollection<MailMessage> FindMessages(
                                            object what, 
                                            FindMessageWhereEnum where, 
                                            bool findExact = true
                                            )
            {
                var ret = new CCollection<MailMessage>();
                FindMessageWhereEnum[] flags = (FindMessageWhereEnum[])Enum.GetValues(typeof(FindMessageWhereEnum));
                var addedids = DMD.Arrays.Empty<int>();
                 
                foreach (var flag in flags)
                {
                    switch (flag)
                    {
                        case minidom.Office.FindMessageWhereEnum.Body:
                            {
                                var res = Index.Find(DMD.Strings.CStr(what), -1);
                                if (res.Count > 0)
                                {
                                    var addingids = DMD.Arrays.Empty<object>();
                                    foreach (var r in res)
                                    {
                                        if (DMD.Arrays.BinarySearch(addedids, r.OwnerID) < 0)
                                        {
                                            addingids = DMD.Arrays.Append(addingids, (object)r.OwnerID);
                                        }
                                    }

                                    using (var cursor = new MailMessageCursor())
                                    {
                                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                                        cursor.ID.ValueIn(addingids);
                                        while (cursor.Read())
                                        {
                                            ret.Add(cursor.Item);
                                            addedids = DMD.Arrays.InsertSorted(addedids, DBUtils.GetID(cursor.Item, 0));
                                        }
                                    }
                                }

                                break;
                            }

                        case FindMessageWhereEnum.CCField:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.CCNField:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.FromField:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.SendDate:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.Subject:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.SubjectOrBody:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.ToField:
                            {
                                //TODO
                                break;
                            }

                        case FindMessageWhereEnum.ToOrCCOrCCn:
                            {
                                //TODO
                                break;
                            }
                    }
                }

                return ret;
            }

          

            /// <summary>
            /// Genera l'evento EMailSent
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnEmailSent(minidom.Office.EmailEventArg e)
            {
                EmailSent?.Invoke(this, e);
            }

            /// <summary>
            /// Repository di <see cref="MailFolder"/>
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CFoldersClass Folders
            {
                get
                {
                    lock (this)
                    {
                        if (m_Folders is null)
                            m_Folders = new CFoldersClass();
                        return m_Folders;
                    }
                }
            }
             

            // Protected Overridable Function OpenDatabase() As CDBConnection
            // Dim ret As New COleDBConnection

            // 'Assicuriamoci che la cartella esista
            // minidom.Sistema.FileSystem.CreateRecursiveFolder(Me.WorkingDir)

            // ret.Path = FileSystem.CombinePath(Me.WorkingDir, "db.mdb")

            // 'Assicuriamoci che il file esista
            // If (FileSystem.FileExists(ret.Path) = False) Then FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/maildb.mdb"), ret.Path)

            // 'Apriamo il database
            // ret.OpenDB()



            // Return ret
            // End Function

            //private void CheckAccountsTable()
            //{
            //    // Accounts
            //    var tbl = this.Database.HasTable("tbl_EmailAccounts");
            //    var col = tbl.Fields.GetItemByKey("TimeOut");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("TimeOut", typeof(int));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("LastSync");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("LastSync", typeof(DateTime));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("SMTPCrittografia");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("SMTPCrittografia", typeof(int));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("FirmaPerNuoviMessaggi");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("FirmaPerNuoviMessaggi", typeof(string));
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("FirmaPerRisposte");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("FirmaPerRisposte", typeof(string));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    // Messaggi
            //    tbl = Database.Tables.GetItemByKey("tbl_EmailMessages");
            //    col = tbl.Fields.GetItemByKey("DownloadDate");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("DownloadDate", typeof(DateTime));
            //        col.AllowDBNull = true;
            //        col.Create();
            //        Database.ExecuteCommand("CREATE INDEX [idxMessagesDWNLDT] ON [tbl_EmailMessages] ([DownloadDate])");
            //    }

            //    col = tbl.Fields.GetItemByKey("ReadDate");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("ReadDate", typeof(DateTime));
            //        col.AllowDBNull = true;
            //        col.Create();
            //        Database.ExecuteCommand("CREATE INDEX [idxMessagesRDTDT] ON [tbl_EmailMessages] ([ReadDate])");
            //    }

            //    // AlternateView
            //    tbl = Database.Tables.GetItemByKey("tbl_eMailAlternateViews");
            //    col = tbl.Fields.GetItemByKey("BaseUri");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("BaseUri", typeof(string));
            //        col.MaxLength = 255;
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("TransferEncoding");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("TransferEncoding", typeof(string));
            //        col.MaxLength = 255;
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    // Folders
            //    tbl = Database.Tables.GetItemByKey("tbl_eMailFolders");
            //    col = tbl.Fields.GetItemByKey("TotalMessages");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("TotalMessages", typeof(int));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("TotalUnread");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("TotalUnread", typeof(int));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("Flags");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("Flags", typeof(int));
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    col = tbl.Fields.GetItemByKey("Attributi");
            //    if (col is null)
            //    {
            //        col = tbl.Fields.Add("Attributi", typeof(string));
            //        col.MaxLength = 0;
            //        col.AllowDBNull = true;
            //        col.Create();
            //    }

            //    tbl = Database.Tables.GetItemByKey("tbl_Index");
            //    if (tbl is null)
            //    {
            //        tbl = Database.Tables.Add("tbl_Index");
            //        col = tbl.Fields.Add("ID", typeof(int));
            //        col.AutoIncrement = true;
            //        col = tbl.Fields.Add("ObjectType", typeof(string));
            //        col.MaxLength = 255;
            //        col = tbl.Fields.Add("ObjectID", typeof(int));
            //        col = tbl.Fields.Add("Word", typeof(string));
            //        col.MaxLength = 255;
            //        col = tbl.Fields.Add("Rank", typeof(int));
            //        tbl.Create();
            //        Database.ExecuteCommand("ALTER TABLE [tbl_Index] ADD PRIMARY KEY ([ID])");
            //        Database.ExecuteCommand("CREATE INDEX [idxIndexObjType] ON [tbl_Index] ([ObjectType])");
            //        Database.ExecuteCommand("CREATE INDEX [idxIndexObjID] ON [tbl_Index] ([ObjectID])");
            //        Database.ExecuteCommand("CREATE INDEX [idxIndexWord] ON [tbl_Index] ([Word])");
            //    }

            //    tbl = Database.Tables.GetItemByKey("tbl_WordStats");
            //    if (tbl is null)
            //    {
            //        tbl = Database.Tables.Add("tbl_WordStats");
            //        col = tbl.Fields.Add("ID", typeof(int));
            //        col.AutoIncrement = true;
            //        col = tbl.Fields.Add("Word", typeof(string));
            //        col.MaxLength = 255;
            //        col = tbl.Fields.Add("Frequenza", typeof(int));
            //        col = tbl.Fields.Add("Indice", typeof(object));
            //        tbl.Create();
            //        Database.ExecuteCommand("ALTER TABLE [tbl_WordStats] ADD PRIMARY KEY ([ID])");
            //        Database.ExecuteCommand("CREATE INDEX [idxWordStatsWord] ON [tbl_WordStats] ([Word])");
            //        Database.ExecuteCommand("CREATE INDEX [idxWordStatsFreq] ON [tbl_WordStats] ([Frequenza])");
            //    }
            //}

            /// <summary>
            /// Repository di <see cref="MailAccount"/>
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CMailAccounts Accounts
            {
                get
                {
                    lock (this)
                    {
                        if (m_Accounts is null)
                            m_Accounts = new CMailAccounts();
                        return m_Accounts;
                    }
                }
            }

            /// <summary>
            /// Scarica le nuove email
            /// </summary>
            public void CheckEMails()
            {
                //TODO

                // SyncLock Me
                // If (Me.m_DownloadWorker Is Nothing) Then Me.m_DownloadWorker = New DownloadWorker(Me, -1)
                // If (Me.m_DownloadWorker.IsBusy) Then Return
                // Me.m_DownloadWorker.RunWorkerAsync()
                // End SyncLock
            }

            // Public Function DownloadEmails() As CCollection(Of MailMessage)
            // Dim ret As New CCollection(Of MailMessage)
            // For Each a As MailAccount In Me.Accounts
            // ret.AddRange(Me.DownloadEmails(a, -1))
            // Next
            // Return ret
            // End Function

            // Public Function DownloadEmails(ByVal nItems As Integer) As CCollection(Of MailMessage)
            // Dim ret As New CCollection(Of MailMessage)
            // If (nItems <= 0) Then Return ret
            // For Each a As MailAccount In Me.Accounts
            // ret.AddRange(Me.DownloadEmails(a, nItems - ret.Count))
            // Next
            // Return ret
            // End Function


            // Public Function DownloadEmails(ByVal account As MailAccount, ByVal nItems As Integer) As CCollection(Of MailMessage)
            // Dim ret As New CCollection(Of MailMessage)
            // If (nItems = 0) Then Return ret

            // Select Case account.Protocol
            // Case "POP3"
            // Dim client As New minidom.Net.Mail.Pop3Client(account.UserName, account.Password, account.ServerName, account.ServerPort, account.UseSSL)
            // Dim message As minidom.Net.Mail.MailMessageEx
            // Dim msg As MailMessage
            // Dim folder As MailFolder = Nothing
            // client.TimeOut = IIf(account.TimeOut <= 100, 100, account.TimeOut)

            // If (account.DefaultFolderName <> "") Then folder = Me.GetFolderByName(account.DefaultFolderName)
            // If (folder Is Nothing) Then folder = Me.Folders.Inbox
            // 'Dim ret As New CCollection(Of MailMessage)
            // Dim dataInizio As Date = Calendar.Now
            // client.Connect()
            // client.Authenticate()
            // client.Stat()

            // 'Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
            // Dim ultimaData As Date? = account.LastStync

            // 'Scarichiamo i messaggi successivi
            // Dim list As System.Collections.Generic.List(Of Protocols.POP3.Pop3ListItem) = client.List
            // Dim toDelete As New CCollection

            // Dim item As minidom.Net.Mail.Protocols.POP3.Pop3ListItem
            // For i As Integer = list.Count - 1 To 0 Step -1
            // item = list(i)
            // message = client.Top(item.MessageId, 5)
            // msg = New MailMessage(Me)
            // msg.Account = account
            // msg.Folder = folder
            // msg.Process(message) ' = New MailMessage(Me, message, account.UserName, folder)
            // 'If (Not Me.IsDownloaded(msg)) Then
            // 'If (ultimaData.HasValue = True AndAlso msg.DeliveryDate >= ultimaData.Value) OrElse _
            // '   ((ultimaData.HasValue = False OrElse (ultimaData.HasValue AndAlso ultimaData = msg.DeliveryDate)) AndAlso Not Me.IsDownloaded(msg)) Then
            // If (Not Me.IsDownloaded(msg)) Then
            // message = client.RetrMailMessageEx(item)
            // msg.Process(message)
            // msg.Stato = ObjectStatus.OBJECT_VALID
            // msg.SetFlag(MailFlags.Unread, True)
            // msg.Save()
            // ret.Add(msg)
            // Me.OnEmailReceived(New EmailEventArg(msg))
            // End If

            // If account.DelServerAfterNDays Then
            // If msg.DeliveryDate.HasValue AndAlso Calendar.DateDiff(DateTimeInterval.Day, msg.DeliveryDate.Value, Calendar.Now) >= account.DelServerAfterDays Then
            // toDelete.Add(item)
            // End If
            // End If
            // If (nItems > 0 AndAlso ret.Count >= nItems) Then Exit For
            // Next

            // For i As Integer = toDelete.Count - 1 To 0 Step -1
            // item = toDelete(i)
            // client.Dele(item)
            // Next

            // client.Disconnect()

            // account.LastStync = dataInizio
            // If (account.ID <> 0) Then account.Save()
            // Case Else
            // Throw New NotSupportedException("Protocollo e-mail non supportato: [" & account.Protocol & "]")
            // End Select

            // Return ret
            // End Function

            /// <summary>
            /// Restituisce true se il messaggio é già stato scaricato
            /// </summary>
            /// <param name="m"></param>
            /// <returns></returns>
            public bool IsDownloaded(MailMessage m)
            {
                using (var cursor = new MailMessageCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.DeliveryDate.Value = m.DeliveryDate;
                    cursor.From.Value = DMD.Strings.CStr(_strtodb(m.From.ToString()));
                    cursor.To.Value = DMD.Strings.CStr(_strtodb(m.To.ToString()));
                    cursor.Cc.Value = DMD.Strings.CStr(_strtodb(m.Cc.ToString()));
                    cursor.Bcc.Value = DMD.Strings.CStr(_strtodb(m.Bcc.ToString()));
                    cursor.Subject.Value = m.Subject;

                    // cursor.WhereClauses.Add("Len([Body])=" & Len(m.Body))
                    return !cursor.EOF();
                }
                 
            }

            private object _strtodb(string value)
            {
                return value;
            }

            public void SendReceive()
            {

                // Me.DownloadEmails()
                CheckEMails();
                // Me.SendEmails()
            }

            // Public Sub SendEmails()
            // For Each a As MailAccount In Me.Accounts
            // Me.SendEMails(a)
            // Next
            // End Sub

            public void SendEMails(minidom.Office.MailAccount account)
            {
                //TODO
                throw new NotImplementedException();
            }

           

            // Private Function GetFolderById(ByVal parent As MailFolder, ByVal id As Integer) As MailFolder
            // Dim f As MailFolder = parent.Childs.GetItemById(id)
            // If (f Is Nothing) Then
            // For Each c As MailFolder In parent.Childs
            // f = GetFolderById(c, id)
            // If (f IsNot Nothing) Then Exit For
            // Next
            // End If
            // Return f
            // End Function

            // Public Function GetFolderById(ByVal id As Integer) As MailFolder
            // Dim f As MailFolder = Me.Folders.GetItemById(id)
            // If (f Is Nothing) Then
            // For Each c As MailFolder In Me.Folders
            // f = GetFolderById(c, id)
            // If (f IsNot Nothing) Then Exit For
            // Next
            // End If
            // Return f
            // End Function

            // Public Function GetFolderByName(ByVal path As String) As MailFolder
            // Return Me.Folders.GetItemByName(path)
            // End Function


            // Public Function GetMessageById(ByVal id As Integer) As MailMessage
            // Dim cursor As MailMessageCursor = Nothing
            // Try
            // cursor = New MailMessageCursor(Me)
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
            // cursor.ID.Value = id
            // Dim ret As MailMessage = cursor.Item
            // cursor.Dispose()
            // Return ret
            // Catch ex As Exception
            // Sistema.Events.NotifyUnhandledException(ex)
            // Throw
            // Finally
            // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            // End Try
            // End Function


            // Public Overrides Function GetTableName() As String
            // Return "tbl_eMailApps"
            // End Function

            // Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            // Me.m_Name = reader.Read("Name", Me.m_Name)
            // Me.m_DisplayName = reader.Read("DisplayName", Me.m_DisplayName)
            // Me.m_WorkingDir = reader.Read("WorkingDir", Me.m_WorkingDir)
            // Me.m_OwnerID = reader.Read("OwnerID", Me.m_OwnerID)
            // Me.m_OwnerName = reader.Read("OwnerName", Me.m_OwnerName)
            // Try
            // Me.m_Rules = DMD.XML.Utils.Serializer.Deserialize(reader.Read("Rules", ""))
            // Me.m_Rules.SetApplication(Me)
            // Catch ex As Exception
            // Me.m_Rules = Nothing
            // End Try
            // Return MyBase.LoadFromRecordset(reader)
            // End Function

            // Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            // writer.Write("Name", Me.m_Name)
            // writer.Write("DisplayName", Me.m_DisplayName)
            // writer.Write("WorkingDir", Me.m_WorkingDir)
            // writer.Write("OwnerID", Me.OwnerID)
            // writer.Write("OwnerName", Me.m_OwnerName)
            // writer.Write("Rules", DMD.XML.Utils.Serializer.Serialize(Me.Rules))
            // Return MyBase.SaveToRecordset(writer)
            // End Function

            // Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            // Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            // If (ret AndAlso m_Accounts IsNot Nothing) Then m_Accounts.Save(force)
            // 'If (ret AndAlso m_Settings IsNot Nothing) Then dbConn.Save(m_Settings)
            // If (ret AndAlso m_Folders IsNot Nothing) Then m_Folders.Save(force)
            // Return ret
            // End Function

            // Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            // writer.WriteAttribute("Name", Me.m_Name)
            // writer.WriteAttribute("DisplayName", Me.m_DisplayName)
            // writer.WriteAttribute("WorkingDir", Me.m_WorkingDir)
            // writer.WriteAttribute("OwnerID", Me.OwnerID)
            // writer.WriteAttribute("OwnerName", Me.m_OwnerName)
            // MyBase.XMLSerialize(writer)
            // writer.WriteTag("Rules", Me.Rules)
            // End Sub

            // Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            // Select Case fieldName
            // Case "Name" : Me.m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            // Case "DisplayName" : Me.m_DisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            // Case "WorkingDir" : Me.m_WorkingDir = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            // Case "OwnerID" : Me.m_OwnerID = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
            // Case "OwnerName" : Me.m_OwnerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            // Case "Rules" : Me.m_Rules = CType(fieldValue, MailApplicationRules) : Me.m_Rules.SetApplication(Me)
            // Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            // End Select
            // End Sub

            /// <summary>
            /// Informa l'applicazione che deve ricaricare accounts e folders a causa di cambiamenti esterni
            /// </summary>
            /// <remarks></remarks>
            public void NotifyChanges()
            {
                m_Accounts = null;
                m_Folders = null;
            }



            // Sub Create()
            // Dim path As String = FileSystem.CombinePath(Me.WorkingDir, "db.mdb")
            // If (FileSystem.FileExists(path)) Then Throw New InvalidOperationException("Impossibile sovrascrivere il database esistente")
            // minidom.Sistema.FileSystem.CreateRecursiveFolder(Me.WorkingDir)
            // FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/maildb.mdb"), path)
            // End Sub

            /// <summary>
            /// Genera l'evento MailReceived
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnEmailReceived(EmailEventArg e)
            {
                //TODO spostare?
                Index.Index(e.Message);
                EmailReceived?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento DownloadError
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnDownloadError(minidom.Office.DownloadErrorEventArgs e)
            {
                DownloadException?.Invoke(this, e);
            }
        }
    }

    public partial class Office
    {
        [Flags]
        public enum FindMessageWhereEnum : int
        {
            FromField = 1,
            ToField = 2,
            CCField = 4,
            CCNField = 8,
            Subject = 16,
            Body = 32,
            ToOrCCOrCCn = 2 | 4 | 8,
            SubjectOrBody = 16 | 32,
            SendDate = 64
        }

        private static CMailsClass m_Mails = null;

        public static CMailsClass Mails
        {
            get
            {
                if (m_Mails is null)
                    m_Mails = new CMailsClass();
                return m_Mails;
            }
        }
    }
}