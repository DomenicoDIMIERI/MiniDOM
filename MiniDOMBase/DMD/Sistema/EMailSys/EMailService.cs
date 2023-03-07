using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net.Mail; // importo il Namespace
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;
using DMD;
using DMD.Net;
using DMD.Net.Mail;
using DMD.Net.Mail.Protocols.POP3;
using DMD.Net.Mail.Serialization;
using minidom.internals;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{

  

    namespace internals
    {

        /// <summary>
        /// Sistema di invio/ricezione email
        /// </summary>
        public class CEMailerClass
        {


            /// <summary>
            /// Evento generato quando viene modificata la configurazione di questo oggetto
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event ConfigurationChangedEventHandler ConfigurationChanged;

            /// <summary>
            /// Evento generato quando viene modificata la configurazione di questo oggetto
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            public delegate void ConfigurationChangedEventHandler(object sender, DMDEventArgs e);

            /// <summary>
            /// Evento generato quando viene completato l'invio di un messaggio (sincrono o asincrono)
            /// </summary>             
            /// <remarks></remarks>
            public event MessageSentEventHandler MessageSent;

         
            /// <summary>
            /// Evento generato quando si verifica un errore nell'invio del messaggio
            /// </summary>            
            /// <remarks></remarks>
            public event SendErrorEventHandler SendError;

            /// <summary>
            /// Evento generato quando viene ricevuto un messaggio
            /// </summary>            
            /// <remarks></remarks>
            public event MessageReceivedEventHandler MessageReceived;

          

            // Private m_ErrorCode As Integer
            private readonly object configLock = new object();
            private CEmailConfig m_Config = null;
            private CEmailAccounts m_Accounts = null;
            //private bool m_Checking = false;
            private const int DEFAULT_INTERVAL = 1000 * 60 * 10; // Controlla le email ogni 10 minuti
            private System.Timers.Timer m_ReceiveTimer;

            /// <summary>
            /// Lunghezza massima dell'oggetto di una mail da inviare
            /// </summary>
            public const int SUBJECTMAXLEN = 128;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEMailerClass()
            {
                m_ReceiveTimer = new System.Timers.Timer();
                m_ReceiveTimer.Enabled = false;
                m_ReceiveTimer.Interval = DEFAULT_INTERVAL;
                m_ReceiveTimer.Elapsed += m_Timer_Elapsed;
            }

            private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                this.CheckMails();
            }

            /// <summary>
            /// Genera l'evento MessageReceived
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnMessageReceived(MailMessageReceivedEventArgs e)
            {
                this.MessageReceived?.Invoke(this, e);
            }

            /// <summary>
            /// Restituisce o imposta l'intervallo di tempo in minuti entro cui controllare la posta in arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int SynchronizeInterval
            {
                get
                {
                    return (int)(m_ReceiveTimer.Interval / (60 * 1000));
                }

                set
                {
                    if (value <= 0)
                        value = 10;
                    m_ReceiveTimer.Interval = value * 60 * 1000;
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se sincronizzare automaticamente le emails
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool AutoSynchronize
            {
                get
                {
                    return m_ReceiveTimer.Enabled;
                }

                set
                {
                    m_ReceiveTimer.Enabled = value;
                    // If (value) Then Me.CheckMailsAsync()
                }
            }


            public string GetOutPath()
            {
                return Path.Combine(Sistema.ApplicationContext.SystemDataFolder, @"mails\OutQueue\");
            }


          


            /// <summary>
            /// Accede alla configurazione di sistema per le email
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CEmailConfig Config
            {
                get
                {
                    lock (configLock)
                    {
                        if (m_Config is null)
                        {
                            var str = minidom.Sistema.ApplicationContext.Settings.GetValueString("Sistema.EMailService.Config", "");
                            m_Config = DMD.XML.Utils.Deserialize<CEmailConfig>(str);                             
                        }
                        return m_Config;
                    }
                }
            }

            /// <summary>
            /// Imposta la configurazione
            /// </summary>
            /// <param name="value"></param>
            public void SetConfig(CEmailConfig value)
            {
                lock (configLock)
                {
                    var str = DMD.XML.Utils.Serialize(value);
                    minidom.Sistema.ApplicationContext.Settings.SetValueString("Sistema.EMailService.Config", str);
                    m_Config = value;
                    if (m_DefaultSMTPClient is object)
                    {
                        m_DefaultSMTPClient.Dispose();
                        m_DefaultSMTPClient = null;
                    }
                }

                this.doConfigChanged(new DMDEventArgs());
            }

            /// <summary>
            /// Genera l'evento ConfigurationChanged
            /// </summary>
            /// <param name="e"></param>
            protected virtual void doConfigChanged(DMDEventArgs e)
            {
                ConfigurationChanged?.Invoke(this, e);
            }

            /// <summary>
            /// Accede ad una collezione di accounts aggiuntivi utilizzati per scaricare la posta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CEmailAccounts MailAccounts
            {
                get
                {
                    if (m_Accounts is null) m_Accounts = new CEmailAccounts();
                    return m_Accounts;
                }
            }

            

            

            public MailAddress[] GetAddresses(string text, string separator = ",")
            {
                var targets = DMD.Strings.Split(text, separator);
                var ret = new ArrayList();
                for (int i = 0, loopTo = DMD.Arrays.UBound(targets); i <= loopTo; i++)
                    ret.Add(new MailAddress(targets[i]));
                return (MailAddress[])ret.ToArray(typeof(MailAddress));
            }

            /// <summary>
        /// Rimuove tutti i collegamenti esterni e li incorpora nel corpo del messaggio secondo la codifica MIME opportuna
        /// </summary>
        /// <param name="m"></param>
        /// <remarks></remarks>
            public void EmbedResources(MailMessageEx m)
            {
                string htmlBody = m.Body;
                var pics = new Dictionary<string, string>();
                int i = DMD.Strings.InStr(htmlBody, "<img ");
                while (i > 0)
                {
                    int j = DMD.Strings.InStr(i + 1, htmlBody, ">");
                    if (j > i)
                    {
                        i = DMD.Strings.InStr(i, htmlBody, " src=");
                        if (i > 0)
                        {
                            i += 5;
                            string ch = DMD.Strings.Mid(htmlBody, i, 1);
                            while (ch != DMD.Strings.CStr('"') & ch != "'" & i < DMD.Strings.Len(htmlBody))
                            {
                                i += 1;
                                ch = DMD.Strings.Mid(htmlBody, i, 1);
                            }

                            if (ch == DMD.Strings.CStr('"') | ch == "'")
                            {
                                j = DMD.Strings.InStr(i + 1, htmlBody, ch);
                                if (j > i)
                                {
                                    var src = DMD.Strings.Mid(htmlBody, i + 1, j - i - 1);
                                    if (DMD.Strings.Left(src, 4) != "cid:")
                                    {
                                        var k = "pic" + pics.Count;
                                        pics.Add(k, src);
                                        htmlBody = DMD.Strings.Replace(htmlBody, ch + src + ch, ch + "cid:" + k + ch);
                                    }
                                }
                            }
                        }
                    }

                    if (i > 0)
                        i = DMD.Strings.InStr(i + 1, htmlBody, "<img ");
                }

                AlternateView avHtml;
                if (!m.IsBodyHtml)
                {
                    m.IsBodyHtml = true;
                }

                avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html"); // MediaTypeNames.Text.Html)
                m.AlternateViews.Add(avHtml);
                foreach(var k in pics.Keys)
                {
                    using (var mStream = new MemoryStream())
                    {
                        string url = DMD.Strings.LCase(pics[k]);
                        var req = System.Net.WebRequest.Create(url);
                        using (var stream = req.GetResponse().GetResponseStream())
                        {
                            stream.Position = 0L;
                            Sistema.FileSystem.CopyStream(stream, mStream);
                        }
                        mStream.Position = 0L;

                        LinkedResource pic1;
                        if (url.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                        {
                            pic1 = new LinkedResource(mStream, MediaTypeNames.Image.Jpeg);
                        }
                        else if (url.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                        {
                            pic1 = new LinkedResource(mStream, MediaTypeNames.Image.Gif);
                        }
                        else if (url.EndsWith(".tiff", StringComparison.InvariantCultureIgnoreCase))
                        {
                            pic1 = new LinkedResource(mStream, MediaTypeNames.Image.Tiff);
                        }
                        else
                        {
                            pic1 = new LinkedResource(mStream);
                        }

                        pic1.ContentId = k;
                        avHtml.LinkedResources.Add(pic1);
                    }
                      
                }
            }

        
            /// <summary>
            /// Prepara un messaggio per l'invio
            /// </summary>
            /// <param name="fromAddress"></param>
            /// <param name="toAddresses"></param>
            /// <param name="ccAddresses"></param>
            /// <param name="ccnAddresses"></param>
            /// <param name="subject"></param>
            /// <param name="body"></param>
            /// <param name="fromAddressDisplayName"></param>
            /// <param name="embedResource"></param>
            /// <returns></returns>
            public MailMessageEx PrepareMessage(
                            string fromAddress, 
                            string toAddresses, 
                            string ccAddresses, 
                            string ccnAddresses, 
                            string subject, 
                            string body, 
                            string fromAddressDisplayName, 
                            bool embedResource
                            )
            {
                MailAddressEx mittente;
                MailMessageEx mail; // questa dichiarazione deve essere globale
                mail = null;
                mittente = new MailAddressEx(fromAddress, fromAddressDisplayName);
                mail = new MailMessageEx();
                mail.From = mittente;
                mail.To.Add(toAddresses);
                if (!string.IsNullOrEmpty(ccAddresses))
                    mail.CC.Add(ccAddresses);
                if (!string.IsNullOrEmpty(ccnAddresses))
                    mail.Bcc.Add(ccnAddresses);
                mail.Subject = DMD.Strings.TrimTo(subject, SUBJECTMAXLEN);
                mail.IsBodyHtml = true;
                mail.Body = body;
                if (embedResource)
                    EmbedResources(mail);
                return mail;
            }

            /// <summary>
            /// Invia il messaggio con i parametri di invio predefiniti
            /// </summary>
            /// <param name="message"></param>
            /// <remarks></remarks>
            public void SendMessage(MailMessage message)
            {
               this.DefaultSMTPClient.SendMessage(message);
            }

            /// <summary>
            /// Invia il messaggio usando il client specificato
            /// </summary>
            /// <param name="mail"></param>
            /// <param name="client"></param>
            public void SendMessage(MailMessage mail, SmtpClient client)
            {
                this.DefaultSMTPClient.SendMessage(mail, client);
            }

           


            // Private Delegate Sub SendMessageFun(ByVal mail As MailMessageEx, ByVal client As SmtpClient)
            // Private ms2 As SendMessageFun = AddressOf SendMessageWithClient2
            private SMTPClient m_DefaultSMTPClient = null;

            /// <summary>
            /// Restituisce il client predefinito usato per l'invio
            /// </summary>
            public SMTPClient DefaultSMTPClient
            {
                get
                {
                    lock (configLock)
                    {
                        if (m_DefaultSMTPClient is null)
                        {
                            m_DefaultSMTPClient = new SMTPClient();
                            m_DefaultSMTPClient.MessageSent += (s, e) => { this.OnMessageSent(e); };
                            m_DefaultSMTPClient.Config = this.Config.SMTPConfig;
                        }
                        return m_DefaultSMTPClient;
                    }
                }
            }

            /// <summary>
            /// Invia un messaggio in maniera asincrona usando il client predefinito
            /// </summary>
            /// <param name="mail"></param>
            /// <param name="autoDispose"></param>
            /// <returns></returns>
            public object SendMessageAsync(MailMessageEx mail, bool autoDispose = false)
            {
                return this.DefaultSMTPClient.SendMessageAsync(mail, autoDispose);
            }

            /// <summary>
            /// Invia un messaggio in maniera asincrona usando il client specificato
            /// </summary>
            /// <param name="mail"></param>
            /// <param name="client"></param>
            /// <param name="autoDispose"></param>
            /// <param name="maxRetries"></param>
            /// <returns></returns>
            public object SendMessageAsync(
                                    MailMessage mail, 
                                    SmtpClient client, 
                                    bool autoDispose = false, 
                                    int maxRetries = 3
                                    )
            {
                return this.DefaultSMTPClient.SendMessageAsync(mail, client, autoDispose, maxRetries);
            }

            /// <summary>
            /// Genera l'evento MessageSent
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnMessageSent(MailMessageEventArgs e)
            {
                MessageSent?.Invoke(this, e);
            }

          
            /// <summary>
            /// Restituisce true se la stringa rappresenta un indirizzo email valido
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool IsValidAddress(string value)
            {
                int i;
                string user = DMD.Strings.vbNullString;
                string server = DMD.Strings.vbNullString;
                string suffix = DMD.Strings.vbNullString;
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return false;
                i = DMD.Strings.InStr(value, "@");
                if (i < 1)
                    return false;
                if (i > 1)
                    user = DMD.Strings.Left(value, i - 1);
                if (i < DMD.Strings.Len(value))
                    server = DMD.Strings.Mid(value, i + 1);
                user = DMD.Strings.Trim(user);
                if (string.IsNullOrEmpty(user))
                    return false;
                server = DMD.Strings.Trim(server);
                if (string.IsNullOrEmpty(server))
                    return false;
                i = DMD.Strings.InStrRev(server, ".");
                if (i < 1)
                    return false;
                if (i < DMD.Strings.Len(server) - 1)
                    suffix = DMD.Strings.Mid(server, i + 1);
                if (i > 1)
                    server = DMD.Strings.Left(server, i - 1);
                server = DMD.Strings.Trim(server);
                if (string.IsNullOrEmpty(server))
                    return false;
                suffix = DMD.Strings.Trim(suffix);
                if (string.IsNullOrEmpty(suffix))
                    return false;
                return true;
            }

            
            /// <summary>
            /// Effettua la verifica delle email ricevute sul server
            /// </summary>
            /// <remarks></remarks>
            public void CheckMails()
            {
                var messagesToDispose = new List<MailMessage>();

                m_Checking = true;
                var oraInizio = DMD.DateUtils.Now();

                MailMessage message;
                List<Pop3ListItem> list;
                Pop3ListItem item;

                var Config = this.Config.POP3Config;
                using (var client = new Pop3Client(Config.POPUserName, Config.POPPassword, Config.POPServer, Config.POPPort, Config.POPUseSSL))
                {
                    // Dim messagesToDelete As New CCollection

                    client.CustomCerfificateValidation = true; // !Config..SMTPValidateCertificate;
                    client.Connect();
                    client.Authenticate();
                    client.Stat();
                    // Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
                    // Scarichiamo i messaggi successivi
                    list = client.List();
                    for (int i = list.Count - 1; i >= 0; i -= 1)
                    {
                        item = list[i];
                        message = client.Top(item.MessageId, 10);
                        bool delOnServer = false;
                        if (IsNewMessage(message))
                        {
                            message = client.RetrMailMessageEx(item);
                            var e = new MailMessageReceivedEventArgs(message);
                            Exception exp = null;
                            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                            OnMessageReceived(e);
                            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                            if (exp is null)
                            {
                                SaveMessage(message);
                                delOnServer = e.DeleteOnServer;
                            }
                        }

                        if (delOnServer 
                            || 
                            Config.RemoveFromServerAfterDownload 
                            && 
                            DMD.DateUtils.DateDiff(
                                        DateTimeInterval.Day, 
                                        message.GetDeliveryDate().Value, 
                                        DMD.DateUtils.ToDay()
                                        ) > Config.RemoveFromServerAfterNDays)
                        {
                            // messagesToDelete.Add(message)
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

                Config.LastSync = oraInizio;
                lock (configLock)
                {
                    var str = DMD.XML.Utils.Serialize(minidom.Sistema.EMailer.Config);
                    minidom.Sistema.ApplicationContext.Settings.SetValueString("Sistema.EMailService.Config", str);                     
                }


                // Controlliamo tutti gli altri account
                foreach (var account in this.MailAccounts.LoadAll())
                {
                    if (account.Attivo)
                    {
                        oraInizio = DMD.DateUtils.Now();
                        // messagesToDelete = New CCollection

                        using (var client = new Pop3Client(Config.POPUserName, Config.POPPassword, Config.POPServer, Config.POPPort, Config.POPUseSSL))
                        {
                            // Dim ret As New CCollection(Of MailMessageEx)
                            client.Connect();
                            client.Authenticate();
                            client.Stat();
                            // Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
                            // Scarichiamo i messaggi successivi
                            list = client.List();
                            for (int i = list.Count - 1; i >= 0; i -= 1)
                            {
                                item = list[i];
                                message = client.Top(item.MessageId, 5);
                                bool delOnServer = false;
                                if (IsNewMessage(message, account))
                                {
                                    message = client.RetrMailMessageEx(item);
                                    var e = new MailMessageReceivedEventArgs(account.AccountName, message);
                                    Exception exp = null;
#if (!DEBUG)
                                    try {
#endif
                                        OnMessageReceived(e);
#if (!DEBUG)
                                    } catch (Exception ex) {
                                        exp = ex;
                                    }
#endif
                                    if (exp is null)
                                    {
                                        SaveMessage(message);
                                        delOnServer = e.DeleteOnServer;
                                    }

                                    if (delOnServer)
                                    {
                                        Debug.Print("Preparo il messaggio da eliminare: [" + message.Subject + "]");
                                        client.Dele(item); // messagesToDelete.Add(message)
                                    }
                                }

                                messagesToDispose.Add(message);
                            }

                            // For Each message In messagesToDelete
                            // client.Dele(message)
                            // Next

                            client.Quit();
                            client.Disconnect();
                        }

                        account.LastSync = oraInizio;
                        if (account.ID != 0)
                            account.Save();
                    }
                }
                 

                m_Checking = false;
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped ElifDirectiveTrivia */
                
                /* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                var errors = new Dictionary<string, System.Exception>();
                foreach (MailMessage m in messagesToDispose)
                {
                    this.DefaultSMTPClient.DisposeMessage(m);
                }

                if (errors.Count > 0)
                {
                    var buff = new System.Text.StringBuilder();
                    foreach(var obj in errors)
                    {
                        if (buff.Length > 0)
                            buff.Append("; ");
                        buff.Append(obj.Key);
                        buff.Append(DMD.Chars.vbTab);
                        buff.Append(":");
                        buff.Append(obj.Value);                         
                    }
                    throw new System.Exception(DMD.Strings.JoinW("EMailService error while releasing", buff.ToString()));
                }

            }

            private string GetDelDateStr(DateTime d)
            {
                return DMD.Strings.Right("0000" + DMD.DateUtils.Year(d), 4) + DMD.Strings.Right("00" + DMD.DateUtils.Month(d), 2) + DMD.Strings.Right("00" + DMD.DateUtils.Day(d), 2) + DMD.Strings.Right("00" + DMD.DateUtils.Hour(d), 2) + DMD.Strings.Right("00" + DMD.DateUtils.Minute(d), 2) + DMD.Strings.Right("00" + DMD.DateUtils.Second(d), 2);
            }

            private string RemoveCharsFromFolder(string str)
            {
                return DMD.Strings.OnlyCharsAndNumbers(str);
            }

            private string GetTargetName(MailMessage message, Sistema.CEmailAccount account = null)
            {
                string path = Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "Mail");
                if (account is object)
                    path = Path.Combine(path, account.AccountName);
                string strID;
                if (message is MailMessageEx)
                {
                    var messagex = message as MailMessageEx;
                    strID = DMD.Strings.Left(GetDelDateStr(messagex.DeliveryDate) 
                        + "_" 
                        + RemoveCharsFromFolder(messagex.MessageId), 256);
                }
                else
                {
                    strID = message.From + "_" + message.To + "_" + message.Subject;
                }
                strID = DMD.Strings.Left(Sistema.FileSystem.RemoveSpecialChars(strID), 128);
                return Path.Combine(path, strID + @"\message.xml");
            }

            /// <summary>
            /// Controlla se il messaggio è stato già scaricato
            /// </summary>
            /// <param name="message"></param>
            /// <param name="account"></param>
            /// <returns></returns>
            public bool IsNewMessage(MailMessage message, Sistema.CEmailAccount account = null)
            {
                // Return Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
                if (account is null)
                {
                    // Return ((Me.Config.LastSync.HasValue = False) OrElse (Calendar.DateDiff(DateTimeInterval.Minute, Me.Config.LastSync.Value, message.DeliveryDate) > -5)) AndAlso _
                    // Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
                    return !File.Exists(GetTargetName(message, account));
                }
                else
                {
                    // Return ((account.LastSync.HasValue = False) OrElse (Calendar.DateDiff(DateTimeInterval.Minute, account.LastSync.Value, message.DeliveryDate) > -5)) AndAlso _
                    // Not Global.System.IO.File.Exists(Me.GetTargetName(message, account))
                    return !File.Exists(GetTargetName(message, account));
                }
            }

            /// <summary>
            /// Salva il messaggio
            /// </summary>
            /// <param name="message"></param>
            /// <param name="account"></param>
            /// <returns></returns>
            public string SaveMessage(MailMessage message, Sistema.CEmailAccount account = null)
            {
                string targetName = GetTargetName(message, account);
                string folder = Sistema.FileSystem.GetFolderName(targetName);
                Sistema.FileSystem.CreateRecursiveFolder(folder);
                EMailSerializer.Serialize(message, targetName);
                return targetName;
            }

            
            
        
        }



        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Using System;
        // Using System.Collections.Generic;
        // Using System.Text;
        // Using System.Text.RegularExpressions;
        // Using System.Net;
        // Using System.IO;
        // Using System.Net.Mime;
        // Using System.Collections.Specialized;
        // Using System.Net.Mail;

        // Namespace Mail.Serialization
        // {
     
      
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    public partial class Sistema
    {
        private static CEMailerClass m_Emailer = null;

        public static CEMailerClass EMailer
        {
            get
            {
                if (m_Emailer is null)
                    m_Emailer = new CEMailerClass();
                return m_Emailer;
            }
        }
    }
}