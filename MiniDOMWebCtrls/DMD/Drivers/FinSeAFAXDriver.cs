using System;
using DMD;

namespace minidom.Drivers
{
    public class FSEFaxConfiguration : Sistema.FaxDriverOptions
    {
        public FSEFaxConfiguration()
        {
        }

        public string ServiceAddress
        {
            get
            {
                return GetValueString("FSEServiceAddress", "faxserver@minidom.net");
            }

            set
            {
                SetValueString("FSEServiceAddress", DMD.Strings.Trim(value));
            }
        }
    }

    public class FSEFaxDriver : Sistema.BaseFaxDriver
    {
        public FSEFaxDriver()
        {
        }

        public new FSEFaxConfiguration Config
        {
            get
            {
                return (FSEFaxConfiguration)base.Config;
            }
        }

        protected override void CancelJobInternal(string jobID)
        {
            throw new NotSupportedException();
        }

        public override string Description
        {
            get
            {
                return "DMD Fax Driver";
            }
        }

        protected override DriverOptions InstantiateNewOptions()
        {
            return new FSEFaxConfiguration();
        }

        public override string GetUniqueID()
        {
            return "DMDFXDRV";
        }

        protected override void InternalSend(Sistema.FaxJob job)
        {
            string toAddress = Config.ServiceAddress;
            string fromAddress = Sistema.EMailer.Config.SMTPUserName;
            string subject = "MSGID:" + job.JobID + "|SENDTO:" + job.Options.TargetNumber;
            System.Net.Mail.MailMessage m = Sistema.EMailer.PrepareMessage(fromAddress, toAddress, "", "", subject, "", fromAddress, false);
            var a = Sistema.EMailer.AddAttachments((Net.Mail.MailMessageEx)m, job.Options.FileName);
            a.ContentDisposition.Inline = false;
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            m.Bcc.Add("tecnico@minidom.net");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Sistema.EMailer.SendMessage((Net.Mail.MailMessageEx)m);
            m.Dispose();
            a.Dispose();
        }

        protected override void InternalConnect()
        {
            base.InternalConnect();
            Sistema.EMailer.MessageReceived += handleMessageReceived;
        }

        protected override void InternalDisconnect()
        {
            Sistema.EMailer.MessageReceived -= handleMessageReceived;
        }

        private void handleMessageReceived(object sender, Sistema.MailMessageEventArgs e)
        {
            System.IO.StreamReader reader = null;
            try
            {
                System.Net.Mail.MailMessage msg = e.Message;
                string text;
                if (msg.From is null || (msg.From.Address ?? "") != (Config.ServiceAddress ?? ""))
                    return;
                if (msg.IsBodyHtml)
                {
                    if (Parse(msg.Subject, msg.Body) == false)
                    {
                        foreach (System.Net.Mail.AlternateView view in msg.AlternateViews)
                        {
                            // If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
                            view.ContentStream.Position = 0L;
                            reader = new System.IO.StreamReader(view.ContentStream);
                            text = reader.ReadToEnd();
                            reader.Dispose();
                            reader = null;
                            if (Parse(msg.Subject, text))
                                break;
                            // End If
                        }
                    }
                }
                else
                {
                    text = msg.Body;
                    Parse(msg.Subject, text);
                }
            }

            // msg.Dispose()
            catch (Exception ex)
            {
                Sistema.ApplicationContext.Log("Errore in FSEFaxDriver");
                Sistema.ApplicationContext.Log(e.ToString());
                Sistema.Events.NotifyUnhandledException(ex);
            }
            finally
            {
                if (reader is object)
                {
                    reader.Dispose();
                    reader = null;
                }
            }
        }

        private DateTime? ParseDate(string value)
        {
            return DMD.DateUtils.ParseISODate(value);
        }

        protected virtual bool Parse(string subject, string text)
        {
            subject = Strings.Trim(subject);
            if (string.IsNullOrEmpty(subject))
                return false;
            var items = Strings.Split(subject, "|");
            string msgid = "";
            string status = "";
            string errormsg = "";
            DateTime? dlvtime = default;
            for (int i = 0, loopTo = DMD.Arrays.Len(items) - 1; i <= loopTo; i++)
            {
                string item = Strings.Trim(items[i]);
                if (Strings.Left(item, Strings.Len("MSGID:")) == "MSGID:")
                {
                    msgid = Strings.Mid(item, Strings.Len("MSGID:") + 1);
                }
                else if (Strings.Left(item, Strings.Len("STATUS:")) == "STATUS:")
                {
                    status = Strings.Mid(item, Strings.Len("STATUS:") + 1);
                }
                else if (Strings.Left(item, Strings.Len("ERRORMSG:")) == "ERRORMSG:")
                {
                    errormsg = Strings.Mid(item, Strings.Len("ERRORMSG:") + 1);
                }
                else if (Strings.Left(item, Strings.Len("DLVTIME:")) == "DLVTIME:")
                {
                    dlvtime = ParseDate(Strings.Mid(item, Strings.Len("DLVTIME:") + 1));
                }
            }

            Sistema.FaxJob j = null;
            switch (status ?? "")
            {
                case "QUEUED":
                    {
                        lock (outQueueLock)
                        {
                            foreach (Sistema.FaxJob job in OutQueue)
                            {
                                if ((job.JobID ?? "") == (msgid ?? ""))
                                {
                                    j = job;
                                    break;
                                }
                            }
                        }

                        if (j is object)
                        {
                            // Me.setQueueDate(j, dlvtime)
                            doFaxChangeStatus(j, Sistema.FaxJobStatus.QUEUED, "Queued");
                        }

                        return true;
                    }

                case "OK":
                    {
                        lock (outQueueLock)
                        {
                            foreach (Sistema.FaxJob job in OutQueue)
                            {
                                if ((job.JobID ?? "") == (msgid ?? ""))
                                {
                                    j = job;
                                    break;
                                }
                            }
                        }

                        if (j is object)
                        {
                            setDeliveryDate(j, dlvtime);
                            doFaxDelivered(j);
                        }

                        return true;
                    }

                case "ERROR":
                    {
                        lock (outQueueLock)
                        {
                            foreach (Sistema.FaxJob job in OutQueue)
                            {
                                if ((job.JobID ?? "") == (msgid ?? ""))
                                {
                                    j = job;
                                    break;
                                }
                            }
                        }

                        if (j is object)
                        {
                            setFailDate(j, dlvtime);
                            doFaxError(j, errormsg);
                        }

                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }
    }
}