using System;
using System.Diagnostics;
using DMD;

namespace minidom.Drivers
{
    public class FaxatorFaxConfiguration : Sistema.FaxDriverOptions
    {
        public FaxatorFaxConfiguration()
        {
        }

        public bool FaxGratis
        {
            get
            {
                return GetValueBool("FaxGratis", true);
            }

            set
            {
                SetValueBool("FaxGratis", value);
            }
        }

        public bool UseDefaultSMTP
        {
            get
            {
                return GetValueBool("UseDefaultSMTP", true);
            }

            set
            {
                SetValueBool("UseDefaultSMTP", value);
            }
        }

        public string SMTPServer
        {
            get
            {
                return GetValueString("SMTPServer", "localhost");
            }

            set
            {
                SetValueString("SMTPServer", DMD.Strings.Trim(value));
            }
        }

        public int SMTPPort
        {
            get
            {
                return GetValueInt("SMTPPort", 25);
            }

            set
            {
                SetValueInt("SMTPPort", value);
            }
        }

        public string SMTPUserName
        {
            get
            {
                return GetValueString("SMTPUserName", "");
            }

            set
            {
                SetValueString("SMTPUserName", DMD.Strings.Trim(value));
            }
        }

        public string SMTPPassword
        {
            get
            {
                return GetValueString("SMTPPassword", "");
            }

            set
            {
                SetValueString("SMTPPassword", value);
            }
        }

        public bool SMTPUseSSL
        {
            get
            {
                return GetValueBool("SMTPUseSSL", false);
            }

            set
            {
                SetValueBool("SMTPUseSSL", value);
            }
        }

        public bool SMTPBypassCertificateValidation
        {
            get
            {
                return GetValueBool("SMTPBypassCertificateValidation", false);
            }

            set
            {
                SetValueBool("SMTPBypassCertificateValidation", value);
            }
        }

        public string UserCertificateURL
        {
            get
            {
                return GetValueString("UserCertificateURL", "");
            }

            set
            {
                GetValueString("UserCertificateURL", value);
            }
        }
    }

    public class FaxatorFaxDriver : Sistema.BaseFaxDriver
    {
        private const int FXTTIMEDIFF = -2;        // Faxator riporta orari 2 ore indietro

        public FaxatorFaxDriver()
        {
        }

        public new FaxatorFaxConfiguration Config
        {
            get
            {
                return (FaxatorFaxConfiguration)base.Config;
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
                return "Faxator Fax Driver";
            }
        }

        protected override DriverOptions InstantiateNewOptions()
        {
            return new FaxatorFaxConfiguration();
        }

        public override string GetUniqueID()
        {
            return "FXTRFXDRV";
        }

        protected override void InternalSend(Sistema.FaxJob job)
        {
            string toAddress = (Config.FaxGratis)? "\"faxgratis@faxator.com\" <faxgratis@faxator.com>" : "\"<fax@faxator.com>\" <fax@faxator.com>";
            string fromAddress = Config.SMTPUserName;
            System.Net.Mail.MailMessage m = Sistema.EMailer.PrepareMessage(fromAddress, toAddress, "", "", job.Options.TargetNumber, "", fromAddress, false);
            m.Headers.Add("Content-Language", "it");
            m.Headers.Add("X-Mailer", "Microsoft Outlook 15.0");
            m.Headers.Add("Message-ID", job.JobID + "@faxator.com");
            var a = Sistema.EMailer.AddAttachments((Net.Mail.MailMessageEx)m, job.Options.FileName);
            a.ContentDisposition.Inline = false;
            var a1 = Sistema.EMailer.AddAttachments((Net.Mail.MailMessageEx)m, Sistema.ApplicationContext.MapPath(Config.UserCertificateURL));
            a1.ContentDisposition.Inline = false;
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            m.Bcc.Add("tecnico@minidom.net");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Sistema.EMailer.SendMessage((Net.Mail.MailMessageEx)m);
            m.Dispose();
            a.Dispose();
            a1.Dispose();
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
            System.Net.Mail.MailMessage msg = e.Message;
            string text;
            if (msg.From is null || msg.From.Address != "noreply@faxator.com")
                return;
            if (msg.IsBodyHtml)
            {
                if (Parse(msg.Subject, msg.Body) == false)
                {
                    foreach (System.Net.Mail.AlternateView view in msg.AlternateViews)
                    {
                        // If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
                        view.ContentStream.Position = 0L;
                        var reader = new System.IO.StreamReader(view.ContentStream);
                        text = reader.ReadToEnd();
                        reader.Dispose();
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

            // msg.Dispose()
        }

        protected virtual bool Parse(string subject, string text)
        {
            const string SPEDIZIONEACCETTATA = "FAXATOR - Spedizione fax accettata";
            const string NOTIFICATXERRATA = "NOTIFICA TX ERRATA";
            const string NOTIFICATXOK = "NOTIFICA TX OK";
            const string MANCAFAX = "FAXATOR - Manca allegato fax.";
            const string MANCAREGISTRAZIONE = "FAXATOR - Manca registrazione";
            string documento;
            DateTime? dataInvio;
            string numero;
            string[] items;
            int i;
            int p;
            Sistema.FaxJob fax;
            text = DMD.Strings.Replace(text, "<br>", DMD.Strings.vbCr, true);
            text = DMD.Strings.Replace(text, "<br/>", DMD.Strings.vbCr, true);
            if (DMD.Strings.Compare(Strings.Left(subject, Strings.Len(NOTIFICATXOK)), NOTIFICATXOK, true) == 0)
            {
                // Documento: Test.pdf
                // Data di invio: 15/05/2015 17:20:04
                // Inviato al numero: 0975739900
                text = DMD.WebUtils.RemoveHTMLTags(text);
                text = Strings.Replace(text, DMD.Strings.vbCrLf, DMD.Strings.vbCr);
                text = Strings.Replace(text, DMD.Strings.vbLf, DMD.Strings.vbCr);
                items = Strings.Split(text, DMD.Strings.vbCr);
                documento = "";
                dataInvio = default;
                numero = "";
                var loopTo = DMD.Arrays.Len(items) - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    p = Strings.InStr(items[i], ":");
                    string name;
                    string value;
                    if (p > 0)
                    {
                        name = Strings.Left(items[i], p - 1);
                        value = Strings.Mid(items[i], p + 1);
                    }
                    else
                    {
                        name = items[i];
                        value = "";
                    }

                    switch (Strings.LCase(Strings.Trim(name)) ?? "")
                    {
                        case "documento":
                            {
                                documento = Strings.Trim(value);
                                break;
                            }

                        case "data di invio":
                            {
                                dataInvio = Sistema.Formats.ParseDate(value);
                                break;
                            }

                        case "inviato al numero":
                            {
                                numero = Sistema.Formats.ParsePhoneNumber(value);
                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty(documento) && dataInvio.HasValue && !string.IsNullOrEmpty(numero))
                {
                    // dataInvio = dataInvio.Value.ToLocalTime

                    Sistema.FaxJob jobSent = null;
                    lock (outQueueLock)
                    {
                        foreach (Sistema.FaxJob job in OutQueue)
                        {
                            if (checkDates(job.Date, (DateTime)dataInvio) && (job.Options.TargetNumber ?? "") == (numero ?? "") && IsSameDocument(job.Options.FileName, documento))

                            {
                                jobSent = job;
                                break;
                            }
                        }
                    }

                    if (jobSent is object)
                    {
                        doFaxDelivered(jobSent);
                    }
                    else
                    {
                    }
                }

                return true;
            }
            else if (DMD.Strings.Compare(Strings.Left(subject, Strings.Len(SPEDIZIONEACCETTATA)), SPEDIZIONEACCETTATA, true) == 0)
            {
                DateTime dataSpedizione;
                string nomeDocumento = "";
                string numeroDestinatario = "";
                text = DMD.WebUtils.RemoveHTMLTags(text);
                text = Strings.Replace(text, DMD.Strings.vbCrLf, DMD.Strings.vbCr);
                text = Strings.Replace(text, DMD.Strings.vbLf, DMD.Strings.vbCr);
                p = Strings.InStr(text, "La richiesta spedizione del ");
                text = Strings.Mid(text, p + Strings.Len("La richiesta spedizione del "));
                p = Strings.InStr(text, "è stata accettata con successo.");
                dataSpedizione = (DateTime)Sistema.Formats.ParseDate(Strings.Left(text, p - 1));
                dataSpedizione = dataSpedizione.ToLocalTime();
                text = Strings.Mid(text, p + Strings.Len("è stata accettata con successo.") + 1);
                p = Strings.InStr(text, "Documento:");
                text = Strings.Mid(text, p + Strings.Len("Documento:") + 1);
                p = Strings.InStr(text, "Inviato al numero:");
                nomeDocumento = Strings.Trim(Strings.Left(text, p - 1));
                text = Strings.Mid(text, p + Strings.Len("Inviato al numero:") + 1);
                p = Strings.InStr(text, "Faxator.com");
                numeroDestinatario = Sistema.Formats.ParsePhoneNumber(Strings.Left(text, p - 1));
                fax = null;
                lock (outQueueLock)
                {
                    foreach (Sistema.FaxJob job in OutQueue)
                    {
                        if ((job.Options.TargetNumber ?? "") == (numeroDestinatario ?? "") && IsSameDocument(job.Options.FileName, nomeDocumento) && checkDates(dataSpedizione, job.Date))

                        {
                            fax = job;
                            break;
                        }
                    }
                }

                if (fax is object)
                {
                    doFaxChangeStatus(fax, Sistema.FaxJobStatus.SENDING, "Faxator ha preso in carico il documento");
                }

                return true;
            }
            else if (DMD.Strings.Compare(Strings.Left(subject, Strings.Len(NOTIFICATXERRATA)), NOTIFICATXERRATA, true) == 0)
            {
                return true;
            }
            else if (DMD.Strings.Compare(Strings.Left(subject, Strings.Len(MANCAFAX)), MANCAFAX, true) == 0)
            {
                // Si è verificato un errore di invio: Manca l'allegato Fax
                const string lookFor = "abbiamo riscontrato il tentativo di un invio fax scorretto dalla tua e-mail del "; // 01/06/2015 10:19.
                p = Strings.InStr(text, lookFor);
                if (p > 0)
                {
                    text = Strings.Mid(text, p + Strings.Len(lookFor) + 1);
                    p = Strings.InStr(text, DMD.Strings.vbCr);
                    if (p > 0)
                    {
                        text = Strings.Left(text, p - 1);
                        if (Strings.Right(text, 1) == ".")
                            text = Strings.Left(text, Strings.Len(text) - 1);
                        dataInvio = Sistema.Formats.ParseDate(text);
                        if (dataInvio.HasValue)
                        {
                            fax = FindBySentDate((DateTime)dataInvio, 5);
                            if (fax is object)
                            {
                                doFaxError(fax, subject);
                            }
                        }
                    }
                }
            }
            else if (DMD.Strings.Compare(Strings.Left(subject, Strings.Len(MANCAREGISTRAZIONE)), MANCAREGISTRAZIONE, true) == 0)
            {
                // Manca la registrazione a Faxator
                // Abortiamo tutti i fax inviati
                lock (outQueueLock)
                {
                    while (OutQueue.Count > 0)
                        doFaxError(OutQueue[0], "Servizio Faxator non sottoscritto");
                }
            }
            else
            {
                Debug.Print("Soggetto non riconosciuto: " + subject);
            }

            return false;
        }

        private bool checkDates(DateTime d1, DateTime d2, int minutesTolerance = 5)
        {
            int diff = (int)Maths.Abs(DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Minute, d1, d2));
            return diff <= minutesTolerance;
        }

        private Sistema.FaxJob FindBySentDate(DateTime d, int minutesTolerance)
        {
            Sistema.FaxJob ret = null;
            int minDiff = int.MaxValue;
            d = DMD.DateUtils.DateAdd(DMD.DateTimeInterval.Hour, -FXTTIMEDIFF, d);
            lock (outQueueLock)
            {
                foreach (Sistema.FaxJob job in OutQueue)
                {
                    long diff = DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Minute, d, job.Date);
                    diff = (long)Maths.Abs(diff);
                    if (diff <= minutesTolerance && diff < minDiff)
                    {
                        ret = job;
                        minDiff = (int)diff;
                    }
                }
            }

            return ret;
        }

        private bool IsSameDocument(string d1, string d2)
        {
            d1 = Sistema.FileSystem.GetBaseName(d1);
            d2 = Sistema.FileSystem.GetBaseName(d1);
            return DMD.Strings.Compare(d1, d2, true) == 0;
        }
    }
}