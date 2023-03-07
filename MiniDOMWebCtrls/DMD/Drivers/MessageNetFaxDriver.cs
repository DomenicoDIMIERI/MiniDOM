using System;
using System.Diagnostics;
using System.Net.Mail;
using DMD;
using DMD.XML;
using minidom.Net.Mail;

namespace minidom.Drivers
{
    public class MessageNetFaxDriverModem 
        : IDMDXMLSerializable
    {
        private string m_NumeroGeografico;
        private string m_UserID;
        private string m_Password;
        private string m_eMail;
        private string m_NotifyTo;

        public MessageNetFaxDriverModem()
        {
            DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Restituisce o imposta il numero geografico associato al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string NumeroGeografico
        {
            get
            {
                return m_NumeroGeografico;
            }

            set
            {
                value = Sistema.Formats.ParsePhoneNumber(value);
                if ((m_NumeroGeografico ?? "") == (value ?? ""))
                    return;
                m_NumeroGeografico = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta la UserID associata al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UserID
        {
            get
            {
                return m_UserID;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                if ((m_UserID ?? "") == (value ?? ""))
                    return;
                m_UserID = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta la password associata al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Password
        {
            get
            {
                return m_Password;
            }

            set
            {
                if ((m_Password ?? "") == (value ?? ""))
                    return;
                m_Password = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta la mail utilizzata come mittente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SenderEMail
        {
            get
            {
                return m_eMail;
            }

            set
            {
                value = Sistema.Formats.ParseEMailAddress(value);
                if ((m_eMail ?? "") == (value ?? ""))
                    return;
                m_eMail = value;
            }
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }


        ~MessageNetFaxDriverModem()
        {
            DMDObject.DecreaseCounter(this);
        }
    }

    public class MessageNetFaxDriverConfig 
        : Sistema.FaxDriverOptions
    {

        /// <summary>
        /// Restituisce o imposta il numero geografico associato al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string NumeroGeografico
        {
            get
            {
                return GetValueString("NumeroGeografico", "");
            }

            set
            {
                SetValueString("NumeroGeografico", Sistema.Formats.ParsePhoneNumber(value));
            }
        }

        /// <summary>
        /// Restituisce o imposta la UserID associata al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UserID
        {
            get
            {
                return GetValueString("UserID", "");
            }

            set
            {
                SetValueString("UserID", DMD.Strings.Trim(value));
            }
        }

        /// <summary>
        /// Restituisce o imposta la password associata al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Password
        {
            get
            {
                return GetValueString("Password", "");
            }

            set
            {
                SetValueString("Password", value);
            }
        }

        /// <summary>
        /// Restituisce o imposta la mail utilizzata come mittente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SenderEMail
        {
            get
            {
                return GetValueString("SenderEMail", "");
            }

            set
            {
                SetValueString("SenderEMail", DMD.Strings.Trim(value));
            }
        }
    }

    public class MessageNetFaxDriver : Sistema.BaseFaxDriver
    {
        public MessageNetFaxDriver() : base()
        {
        }

        public new MessageNetFaxDriverConfig Config
        {
            get
            {
                return (MessageNetFaxDriverConfig)base.Config;
            }
        }

        public override string Description
        {
            get
            {
                return "Messagenet Fax";
            }
        }

        public override string GetUniqueID()
        {
            return "MNETFAXSVC";
        }

        protected override void InternalSend(Sistema.FaxJob job)
        {
            MailMessage m;
            // Dim p As New minidom.PDF.PDFWriter
            var options = job.Options;
            // Dim tempFileName As String = minidom.FileSystem.GetTempFileName("pdf")
            string ccnAddress = "";

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            ccnAddress = "tecnico@minidom.net";
            /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            string codice = job.JobID;
            var modem = GetModem(job.Options.ModemName);
            if (modem is null)
                throw new InvalidOperationException("Non è stato definito alcun modem per il driver MessageNetFaxDriver");
            if (modem.SendEnabled == false)
                throw new PermissionDeniedException("L'invio dei fax è stato disabilitato su questo modem");
            Sistema.ApplicationContext.Log("MessageNetFaxDriver.InternalSend(" + modem.eMailInvio + ", " + options.TargetNumber + ", " + codice + ")");
            m = Sistema.EMailer.PrepareMessage(modem.eMailInvio, options.TargetNumber + "@fax.messagenet.it", "", ccnAddress, codice, "", "", false);

            // For i = 0 To UBound(images)
            // p.AddPage()
            // p.DrawImage(images(i), 0, 0)
            // Next

            var att = new Attachment(options.FileName);
            m.Attachments.Add(att);
            m.IsBodyHtml = false;
            Sistema.EMailer.SendMessage((MailMessageEx)m);
            att.Dispose();
            m.Dispose();
            // p.Dispose()

        }

        protected override DriverOptions InstantiateNewOptions()
        {
            return new MessageNetFaxDriverConfig();
        }

        protected override void CancelJobInternal(string jobID)
        {
            throw new NotSupportedException("Impossibile annullare l'invio");
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

        private const string fromPart = "@fax.messagenet.it";


        /// <summary>
        /// Restituisce vero se il messaggio proviene dal servizio di ricezione fax di messagenet
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool IsFromMessageNet(string address)
        {
            address = Strings.LCase(Strings.Trim(address));
            return (Strings.Right(address, Strings.Len(fromPart)) ?? "") == fromPart;
        }

        /// <summary>
        /// Estrae il numero dal mittente dall'indirizzo
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetNumeroMittente(string address)
        {
            address = Strings.LCase(Strings.Trim(address));
            if (!IsFromMessageNet(address))
                return "";
            return Strings.Left(address, Strings.Len(address) - Strings.Len(fromPart));
        }

        private bool handleMessage(object sender, MailMessageEx m, Sistema.FaxDriverModem modem)
        {
            string fName;
            string ext;
            Sistema.FaxJob job;

            // Fax ricevuto
            if (Strings.InStr(m.Subject, "fax per l'utente " + modem.UserName) > 0 || (m.Subject ?? "") == ("F@X IN: fax per il numero " + modem.Numero ?? ""))
            {
                if (m.Attachments.Count == 0)
                    return false;
                string mittente = GetNumeroMittente(m.From.Address);

                // Salviamo l'allegato
                lock (inqueueLock)
                {
                    fName = Sistema.FileSystem.GetBaseName(m.Attachments[0].FileName);
                    ext = Sistema.FileSystem.GetExtensionName(m.Attachments[0].FileName);
                    if (string.IsNullOrEmpty(fName))
                        fName = GetUniqueID() + "I" + Sistema.ASPSecurity.GetRandomKey(12);
                    if (Sistema.FileSystem.FileExists(GetQueueFile("InQueue", fName + "." + ext)))
                    {
                        int i = 1;
                        while (Sistema.FileSystem.FileExists(GetQueueFile("InQueue", fName + i + ".xml")))
                            i += 1;
                        fName = fName + i;
                    }

                    m.Attachments[0].SaveToFile(GetQueueFile("InQueue", fName + "." + ext));

                    // Estriamoa il numero del mittente
                    job = NewJob();
                    SetDriver(job, this);
                    SetDate(job, m.DeliveryDate);
                    job.Options.FromUser = mittente;
                    job.Options.FileName = GetQueueFile("InQueue", fName + "." + ext);
                    job.Options.SenderNumber = mittente;
                    job.Options.SenderName = mittente;
                    job.Options.RecipientName = modem.eMailRicezione;
                    job.Options.TargetNumber = modem.Numero;
                    job.Options.ModemName = modem.Name;
                    if (modem.PuntoOperativo is object)
                        job.Options.NomePuntoOperativo = modem.PuntoOperativo.Nome;
                    string t = DMD.XML.Utils.Serializer.Serialize(job);
                    Sistema.FileSystem.CreateTextFile(GetQueueFile("InQueue", fName + ".xml"), t);
                }

                doFaxReceived(job);
                return true;
            }

            // Controlliamo se il messaggio si riferisce ad un fax inviato
            string jobID = Strings.Split(m.Subject, " ")[0];
            job = (Sistema.FaxJob)OutQueue.GetItemByKey(jobID);
            if (job is object)
            {
                string stato = Strings.Trim(Strings.Mid(m.Subject, Strings.Len(job.JobID) + 1));
                switch (Strings.LCase(stato) ?? "")
                {
                    case "[ok]": // Fax Inviato correttamente
                        {
                            ParseOkMessage(m, job);
                            doFaxDelivered(job);
                            return true;
                        }

                    case "[in consegna]": // Il fax sta per essere inviato
                        {
                            ParseInConsegnaMessage(m, job);
                            return true;
                        }

                    case "[destinatario errato]":
                        {
                            ParseDestinatarioErrato(m, job);
                            return true;
                        }

                    case "[cancelled na]": // Risposta non di un apparecchio fax, ad esempio voce
                        {
                            ParseDispositivoNonFax(m, job);
                            return true;
                        }

                    case "[cancelled occ]": // Numero occupato(utente)
                        {
                            ParseNumeroOccupato(m, job);
                            return true;
                        }

                    case "[cancelled cre]": // Credito insufficiente
                        {
                            ParseCreditoInsufficiente(m, job);
                            return true;
                        }

                    case "[cancelled nc]": // Numero occupato (rete) o malfunzionante
                        {
                            ParseProblemaDiLinea(m, job);
                            return true;
                        }

                    case "[cencelled abs]": // Non risponde nessuno
                        {
                            ParseNessunaRisposta(m, job);
                            return true;
                        }

                    case "[cancelled inv]": // Il file inviato o non è valid
                        {
                            ParseFormatoNonValido(m, job);
                            return true;
                        }

                    case "[cancelled]": // Il file inviato o non è valid
                        {
                            ParseCancelled(m, job);
                            return true;
                        }

                    case "[credito insufficiente]": // credito insufficiente
                        {
                            ParseCreditoInsufficiente(m, job);
                            return true;
                        }

                    default:
                        {
                            Debug.Print("Caso strano?");
                            break;
                        }
                }
            }
            // msg.Dispose()
            return false;
        }

        private void handleMessageReceived(object sender, Sistema.MailMessageEventArgs e)
        {
            var m = e.Message;

            // Verifichiamo che la mail provenga dal servizio di ricezione fax di messagenet
            if (m.From is null || !IsFromMessageNet(m.From.Address))
                return;
            foreach (Sistema.FaxDriverModem modem in Modems)
            {
                if (handleMessage(sender, m, modem))
                    return;
            }
        }

        protected void ParseOkMessage(MailMessageEx m, Sistema.FaxJob job)
        {
            // Gentile Utente FAXout,
            // Il fax che hai inviato è stato consegnato con successo:
            // * Riferimento: 14812000
            // * Destinazione: +39 089338754
            // * Stato: OK
            // * Data e ora: 2015-06-16 18:37:38
            // * Numero pagine: 1
            // * Prezzo: 0,11 €
            // ---------------------------------------------------------------------
            // Il tuo credito residuo è di: 5,68 € [ Ricarica [http://www.messagenet.it/it/ricarica/]]
            // Ti ricordiamo che con il tuo credito puoi anche TELEFONARE [http://www.messagenet.it/it/comunica/?chiama=chiama]
            // e INVIARE SMS [http://www.messagenet.it/it/comunica/?sms=sms].
            // Per qualsiasi chiarimento scrivi al supporto [mailto:support@messagenet.it].
            // Cordiali saluti,
            // Il team Messagenet
            // MESSAGENET S.p.A.
            // E-mail: support@messagenet.it [mailto:support@messagenet.it]
            // Web: www.messagenet.it [http://www.messagenet.it/]

            string text = GetText(m);
            const string CMDriferimento = "* Riferimento:";
            const string CMDdestinazione = "* Destinazione:";
            const string CMDstato = "* Stato:";
            const string CMDdataeora = "* Data e ora:";
            const string CMDnumeroPagine = "* Numero pagine:";
            const string CMDprezzo = "* Prezzo:";
            const string CMDCredito = "Il tuo credito residuo è di:";
            var lines = Strings.Split(text, DMD.Strings.vbCr);
            string riferimento = "";
            string destinazione = "";
            string stato = "";
            string dataeora = "";
            string numeroPagine = "";
            string prezzo = "";
            string credito = "";
            int p;
            for (int i = 0, loopTo = DMD.Arrays.Len(lines) - 1; i <= loopTo; i++)
            {
                string line = Strings.Trim(lines[i]);
                if ((Strings.Left(line, Strings.Len(CMDriferimento)) ?? "") == CMDriferimento)
                {
                    riferimento = Strings.Trim(Strings.Mid(line, Strings.Len(CMDriferimento) + 1));
                }
                else if ((Strings.Left(line, Strings.Len(CMDdestinazione)) ?? "") == CMDdestinazione)
                {
                    destinazione = Strings.Trim(Strings.Mid(line, Strings.Len(CMDdestinazione) + 1));
                }
                else if ((Strings.Left(line, Strings.Len(CMDstato)) ?? "") == CMDstato)
                {
                    stato = Strings.Trim(Strings.Mid(line, Strings.Len(CMDstato) + 1));
                }
                else if ((Strings.Left(line, Strings.Len(CMDdataeora)) ?? "") == CMDdataeora)
                {
                    dataeora = Strings.Trim(Strings.Mid(line, Strings.Len(CMDdataeora) + 1));
                }
                else if ((Strings.Left(line, Strings.Len(CMDnumeroPagine)) ?? "") == CMDnumeroPagine)
                {
                    numeroPagine = Strings.Trim(Strings.Mid(line, Strings.Len(CMDnumeroPagine) + 1));
                }
                else if ((Strings.Left(line, Strings.Len(CMDprezzo)) ?? "") == CMDprezzo)
                {
                    prezzo = Strings.Trim(Strings.Mid(line, Strings.Len(CMDprezzo) + 1));
                }
                else if ((Strings.Left(line, Strings.Len(CMDCredito)) ?? "") == CMDCredito)
                {
                    credito = Strings.Trim(Strings.Mid(line, Strings.Len(CMDCredito) + 1));
                    p = Strings.InStr(credito, " ");
                    if (p > 0)
                        credito = Strings.Trim(Strings.Left(credito, p));
                }
            }

            job.Options.SetValueString("MsgNet_Riferimento", riferimento);
            job.Options.SetValueString("MsgNet_Destinazione", destinazione);
            job.Options.SetValueString("MsgNet_Stato", stato);
            job.Options.SetValueString("MsgNet_DataEOra", dataeora);
            job.Options.SetValueString("MsgNet_NumeroPagine", numeroPagine);
            job.Options.SetValueString("MsgNet_Prezzo", prezzo);
            job.Options.SetValueString("MsgNet_CreditoResiduo", credito);
            job.Options.NumberOfPages = Sistema.Formats.ToInteger(numeroPagine);
            SetDate(job, (DateTime)ParseDate(dataeora));
        }

        // "YYYY-MM-DD HH:mm:ss")
        private DateTime? ParseDate(string value)
        {
            try
            {
                var dh = Strings.Split(value, " ");
                var dd = Strings.Split(dh[0], "-");
                var hh = Strings.Split(dh[1], ":");
                return DMD.DateUtils.MakeDate(DMD.Integers.ValueOf(dd[0]), DMD.Integers.ValueOf(dd[1]), DMD.Integers.ValueOf(dd[2]), DMD.Integers.ValueOf(hh[0]), DMD.Integers.ValueOf(hh[1]), DMD.Integers.ValueOf(hh[2]));
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        protected void ParseInConsegnaMessage(MailMessageEx m, Sistema.FaxJob job)
        {
            // Gentile Utente FAXout,
            // il tuo messaggio per il numero +39 089338754 è stato preso in carico dal nostro sistema, che ora provvederà ad inoltrarlo al destinatario. 
            // Il codice assegnato al messaggio è 14812000
            // Ti ricordiamo che puoi sempre verificare se il tuo fax è stato consegnato visitando la pagina dei log di FAXout accedendo col tuo codice utente e la tua password. 
            // Per qualsiasi chiarimento scrivi al supporto. 

            string text = GetText(m);
            const string CMDriferimento = "Il codice assegnato al messaggio è";
            var lines = Strings.Split(text, DMD.Strings.vbCr);
            string riferimento = "";
            // Dim p As Integer

            for (int i = 0, loopTo = DMD.Arrays.Len(lines) - 1; i <= loopTo; i++)
            {
                string line = Strings.Trim(lines[i]);
                if ((Strings.Left(line, Strings.Len(CMDriferimento)) ?? "") == CMDriferimento)
                {
                    riferimento = Strings.Trim(Strings.Mid(line, Strings.Len(CMDriferimento) + 1));
                }
            }

            job.Options.SetValueString("MsgNet_Riferimento", riferimento);
        }

        private string GetText(MailMessageEx m)
        {
            string text = "";
            if (m.IsBodyHtml)
            {
                foreach (AlternateViewEx v in m.AlternateViews)
                {
                    if (v.ContentType is object && v.ContentType.MediaType == "text/plain")
                    {
                        text = v.GetContentAsText();
                        break;
                    }
                }
            }
            else
            {
                text = m.Body;
            }

            var buffer = new System.Text.StringBuilder();
            for (int i = 1, loopTo = Strings.Len(text); i <= loopTo; i++)
            {
                char ch = DMD.Chars.CChar(Strings.Mid(text, i, 1));
                switch (ch)
                {
                    case DMD.Chars.vbLf:
                    case DMD.Chars.vbFormFeed:
                    case DMD.Chars.vbTab:
                        {
                            break;
                        }

                    default:
                        {
                            buffer.Append(ch);
                            break;
                        }
                }
            }

            text = buffer.ToString();
            return text;
        }

        protected void ParseCancelled(MailMessageEx m, Sistema.FaxJob job)
        {
            const string CMDRiferimento = "* Riferimento:";
            const string CMDDestinazione = "* Destinazione:";
            const string CMDStato = "* Stato:";
            const string CMDDataEOra = "* Data e ora:";
            const string CMDNumeroPagine = "* Numero pagine:";
            const string CMDPrezzo = "* Prezzo:";
            string text = GetText(m);
            var lines = Strings.Split(text, DMD.Strings.vbCr);
            string riferimento = "";
            string destinazione = "";
            string stato = "";
            string dataeora = "";
            string numeroPagine = "";
            string prezzo = "";
            for (int i = 0, loopTo = DMD.Arrays.Len(lines) - 1; i <= loopTo; i++)
            {
                string line = Strings.Trim(lines[i]);
                if (DMD.Strings.Compare(Strings.Left(line, Strings.Len(CMDRiferimento)), CMDRiferimento, true) == 0)
                {
                    riferimento = Strings.Trim(Strings.Mid(line, Strings.Len(CMDRiferimento) + 1));
                }
                else if (DMD.Strings.Compare(Strings.Left(line, Strings.Len(CMDDestinazione)), CMDDestinazione, true) == 0)
                {
                    destinazione = Strings.Trim(Strings.Mid(line, Strings.Len(CMDDestinazione) + 1));
                }
                else if (DMD.Strings.Compare(Strings.Left(line, Strings.Len(CMDStato)), CMDStato, true) == 0)
                {
                    stato = Strings.Trim(Strings.Mid(line, Strings.Len(CMDStato) + 1));
                }
                else if (DMD.Strings.Compare(Strings.Left(line, Strings.Len(CMDDataEOra)), CMDDataEOra, true) == 0)
                {
                    dataeora = Strings.Trim(Strings.Mid(line, Strings.Len(CMDDataEOra) + 1));
                }
                else if (DMD.Strings.Compare(Strings.Left(line, Strings.Len(CMDNumeroPagine)), CMDNumeroPagine, true) == 0)
                {
                    numeroPagine = Strings.Trim(Strings.Mid(line, Strings.Len(CMDNumeroPagine) + 1));
                }
                else if (DMD.Strings.Compare(Strings.Left(line, Strings.Len(CMDPrezzo)), CMDPrezzo, true) == 0)
                {
                    prezzo = Strings.Trim(Strings.Mid(line, Strings.Len(CMDPrezzo) + 1));
                }
                else
                {
                }
            }

            switch (Strings.LCase(Strings.Trim(stato)) ?? "")
            {
                case "[na]": // Risposta non di un apparecchio fax, ad esempio voce
                    {
                        ParseDispositivoNonFax(m, job);
                        break;
                    }

                case "[occ]": // Numero occupato(utente)
                    {
                        ParseNumeroOccupato(m, job);
                        break;
                    }

                case "[cre]": // Credito insufficiente
                    {
                        ParseCreditoInsufficiente(m, job);
                        break;
                    }

                case "[nc]": // Numero occupato (rete) o malfunzionante
                    {
                        ParseProblemaDiLinea(m, job);
                        break;
                    }

                case "[abs]": // Non risponde nessuno
                    {
                        ParseNessunaRisposta(m, job);
                        break;
                    }

                case "[inv]": // Il file inviato o non è valid
                    {
                        ParseFormatoNonValido(m, job);
                        break;
                    }

                default:
                    {
                        SetDate(job, m.DeliveryDate);
                        doFaxError(job, "Cancelled " + stato);
                        break;
                    }
            }
        }

        protected void ParseDestinatarioErrato(MailMessageEx m, Sistema.FaxJob job)
        {
            // Gentile Utente FAXout,
            // il tuo fax verso il numero 3470815531 non è stato inoltrato, in quanto il numero non è stato riconosciuto come un destinatario valido. 
            // Per qualsiasi chiarimento scrivi al supporto. 


            string text = GetText(m);
            var lines = Strings.Split(text, DMD.Strings.vbCr);
            string cmd = "il tuo fax verso il numero ";
            string cmd1 = "non è stato inoltrato, in quanto";
            string numero = "";
            string errore = "";
            for (int i = 0, loopTo = DMD.Arrays.Len(lines) - 1; i <= loopTo; i++)
            {
                string line = Strings.Trim(lines[i]);
                if ((Strings.Left(line, Strings.Len(cmd)) ?? "") == (cmd ?? ""))
                {
                    numero = Strings.Trim(Strings.Mid(line, Strings.Len(cmd) + 1));
                    int p = Strings.InStr(numero, " ");
                    if (p > 0)
                    {
                        numero = Strings.Trim(Strings.Left(numero, p - 1));
                    }

                    if (i + 1 < DMD.Arrays.Len(lines))
                        errore = Strings.Trim(lines[i + 1]);
                    break;
                }
            }

            SetDate(job, m.DeliveryDate);
            doFaxError(job, errore);
        }

        private void ParseDispositivoNonFax(MailMessageEx m, Sistema.FaxJob job)
        {
            SetDate(job, m.DeliveryDate);
            doFaxError(job, "Il dispositivo remoto non è un fax");
        }

        private void ParseNumeroOccupato(MailMessageEx m, Sistema.FaxJob job)
        {
            SetDate(job, m.DeliveryDate);
            doFaxError(job, "Numero Occupato");
        }

        private void ParseCreditoInsufficiente(MailMessageEx m, Sistema.FaxJob job)
        {
            SetDate(job, m.DeliveryDate);
            doFaxError(job, "Credito Insufficiente");
        }

        private void ParseProblemaDiLinea(MailMessageEx m, Sistema.FaxJob job)
        {
            SetDate(job, m.DeliveryDate);
            doFaxError(job, "Problemi di Linea");
        }

        private void ParseNessunaRisposta(MailMessageEx m, Sistema.FaxJob job)
        {
            SetDate(job, m.DeliveryDate);
            doFaxError(job, "Nessuna Risposta");
        }

        private void ParseFormatoNonValido(MailMessageEx m, Sistema.FaxJob job)
        {
            SetDate(job, m.DeliveryDate);
            doFaxError(job, "Formato del documento inviato non valido");
        }
    }
}