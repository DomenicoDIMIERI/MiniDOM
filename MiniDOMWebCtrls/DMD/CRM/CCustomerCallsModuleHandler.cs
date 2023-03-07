using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CCustomerCallsModuleHandler : CBaseModuleHandler
    {
        public CCustomerCallsModuleHandler()
        {
        }

        public string EsportaContatti(object renderer)
        {
            if (!CanExport())
                throw new PermissionDeniedException(Module, "export");
            string testo = this.n2str(renderer, "text", "");
            string fmt = this.n2str(renderer, "fmt", "");
            CCollection srcItems = (CCollection)DMD.XML.Utils.Serializer.Deserialize(testo);
            var dstItems = new CCollection();
            string tmpURL = "";
            foreach (CustomerCalls.StoricoAction a in srcItems)
            {
                object c = Sistema.Types.GetItemByTypeAndId(a.ActionType, a.ActionID);
                dstItems.Add(c);
            }

            switch (Strings.LCase(fmt) ?? "")
            {
                case "xls":
                    {
                        break;
                    }

                case "xml":
                    {
                        testo = DMD.XML.Utils.Serializer.Serialize(dstItems);
                        tmpURL = WebSite.Instance.Configuration.TempFolder + Sistema.ASPSecurity.GetRandomKey(10) + ".xml";
                        Sistema.FileSystem.SetTextFileContents(Sistema.ApplicationContext.MapPath(tmpURL), testo);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return DMD.XML.Utils.Serializer.SerializeString(tmpURL);
        }

        public string ImportaContatti(object renderer)
        {
            if (!CanImport())
                throw new PermissionDeniedException(Module, "import");
            string fName = this.n2str(renderer, "fname", "");
            string testo = Sistema.FileSystem.GetTextFileContents(Sistema.ApplicationContext.MapPath(fName));
            CCollection items = (CCollection)DMD.XML.Utils.Serializer.Deserialize(testo);
            int pid = (int)this.n2int(renderer, "pid");
            var p = Anagrafica.Persone.GetItemById(pid);
            int cnt = 0;
            foreach (object c in items)
            {
                if (c is CustomerCalls.CContattoUtente)
                {
                    {
                        var withBlock = (CustomerCalls.CContattoUtente)c;
                        Databases.DBUtils.ResetID(c);
                        withBlock.Azienda = Anagrafica.Aziende.AziendaPrincipale;
                        withBlock.Persona = p;
                        withBlock.Save();
                        cnt += 1;
                    }
                }
                else
                {
                    Debug.Print("Not supported: " + DMD.RunTime.vbTypeName(c));
                }
            }

            return DMD.XML.Utils.Serializer.SerializeInteger(cnt);
        }

        public string getArrayNomiPOXUser(object renderer)
        {
            var items = Anagrafica.Uffici.GetPuntiOperativiConsentiti();
            var writer = new System.Text.StringBuilder();
            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            {
                var item = items[i];
                if (writer.Length > 0)
                    writer.Append(DMD.Strings.vbCr);
                writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
            }

            return writer.ToString();
        }

        public string AppendNumber(string html, string nome, string value)
        {
            string ret;
            ret = html;
            if (!string.IsNullOrEmpty(ret))
                ret = ret + DMD.Strings.vbCr;
            ret = ret + nome + " (" + value + ") " + DMD.Strings.vbCr + value;
            return ret;
        }

        public string getArrayContattiTelefonici(object renderer)
        {
            Anagrafica.CPersona persona;
            int idPersona;
            idPersona = (int)this.n2int(renderer, "pid");
            persona = Anagrafica.Persone.GetItemById(idPersona);
            string html = "";
            foreach (Anagrafica.CContatto c in persona.Recapiti)
            {
                if (c.Tipo == "Telefono" && !string.IsNullOrEmpty(c.Valore))
                    html = AppendNumber(html, c.Nome, c.Valore);
            }

            return html;
        }

        public string getArrayPersoneContattate(object renderer)
        {
            int idUfficio = (int)this.n2int(renderer, "uf");
            var inizio = this.n2date(renderer, "ini");
            var fine = this.n2date(renderer, "end");
            bool ir = (bool)this.n2bool(renderer, "ir");
            var idOperatori = DMD.Strings.Split(this.n2str(renderer, "op", ""), ",");
            var operatori = new ArrayList();
            foreach (string Str in idOperatori)
            {
                if (!string.IsNullOrEmpty(Str))
                    operatori.Add(Sistema.Formats.ToInteger(Str));
            }

            int[] arr = (int[])operatori.ToArray(typeof(int));
            var items = CustomerCalls.Telefonate.GetIDPersoneContattate(idUfficio, arr, inizio, fine, ir);
            if (DMD.Arrays.Len(items) > 0)
            {
                return DMD.XML.Utils.Serializer.SerializeArray(items);
            }
            else
            {
                return "";
            }
        }

        public string contaPersoneContattate(object renderer)
        {
            int idUfficio = (int)this.n2int(renderer, "uf");
            var inizio = this.n2date(renderer, "ini");
            var fine = this.n2date(renderer, "end");
            bool ir = (bool)this.n2bool(renderer, "ir");
            var idOperatori = Strings.Split(this.n2str(renderer, "op", ""), ",");
            var operatori = new ArrayList();
            foreach (string Str in idOperatori)
            {
                if (!string.IsNullOrEmpty(Str))
                    operatori.Add(Sistema.Formats.ToInteger(Str));
            }

            return DMD.XML.Utils.Serializer.SerializeInteger(CustomerCalls.Telefonate.ContaPersoneContattate(idUfficio, (int[])operatori.ToArray(typeof(int)), inizio, fine, ir));
        }

        public override bool SupportsDuplicate
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsEdit
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsAnnotations
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsDelete
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsCreate
        {
            get
            {
                return false;
            }
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.CContattoUtenteCursor();
        }

        public override CCollection<ExportableColumnInfo> GetExportableColumnsList()
        {
            var ret = base.GetExportableColumnsList();
            ret.Add(new ExportableColumnInfo("ID", "ID", TypeCode.Int32, true));
            ret.Add(new ExportableColumnInfo("ClassName", "Tipo Oggetto", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("DirezioneChiamata", "I/O", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("NomeOperatore", "Operatore", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Data", "Data", TypeCode.DateTime, true));
            ret.Add(new ExportableColumnInfo("NomePersona", "Nominativo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("CFPersona", "Codice Fiscale", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("IndirizzoContatto", "Indirizzo/Numero", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Scopo", "Scopo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Note", "Note", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Esito", "Esito", TypeCode.String, true));
            return ret;
        }

        protected override object GetColumnValue(object renderer, object item, string key)
        {
            CustomerCalls.CContattoUtente t = (CustomerCalls.CContattoUtente)item;
            switch (key ?? "")
            {
                case "ClassName":
                    {
                        if (t is CustomerCalls.CTelefonata)
                        {
                            return "Telefonata";
                        }
                        else if (t is CustomerCalls.CVisita)
                        {
                            return "Visita";
                        }
                        else
                        {
                            return DMD.RunTime.vbTypeName(t);
                        }

                        break;
                    }

                case "DirezioneChiamata":
                    {
                        return Sistema.IIF(t.Ricevuta, "Ricevuta", "Effettuata");
                    }

                case "IndirizzoContatto":
                    {
                        if (t is CustomerCalls.CTelefonata)
                        {
                            return ((CustomerCalls.CTelefonata)t).NumeroOIndirizzo;
                        }
                        else if (t is CustomerCalls.CVisita)
                        {
                            return ((CustomerCalls.CVisita)t).Luogo.ToString();
                        }
                        else
                        {
                            return "";
                        }

                        break;
                    }

                case "Esito":
                    {
                        return Sistema.IIF(t.Esito == CustomerCalls.EsitoChiamata.OK, "OK", "NO: " + t.DettaglioEsito);
                    }

                case "CFPersona":
                    {
                        return t.Persona.CodiceFiscale;
                    }

                default:
                    {
                        return base.GetColumnValue(renderer, item, key);
                    }
            }
        }

        protected override void SetColumnValue(object renderer, object item, string key, object value)
        {
            CustomerCalls.CContattoUtente t = (CustomerCalls.CContattoUtente)item;
            switch (key ?? "")
            {
                case "DirezioneChiamata":
                    {
                        t.Ricevuta = Sistema.Formats.ToString(value) == "I";
                        break;
                    }

                case "IndirizzoContatto":
                    {
                        if (t is CustomerCalls.CTelefonata)
                        {
                            ((CustomerCalls.CTelefonata)t).NumeroOIndirizzo = Sistema.Formats.ParsePhoneNumber(DMD.Strings.CStr(value));
                        }
                        else
                        {
                            ((CustomerCalls.CVisita)t).Luogo = new Anagrafica.CIndirizzo(Sistema.Formats.ToString(value));
                        }

                        break;
                    }

                case "CFPersona":
                    {
                        break;
                    }

                default:
                    {
                        base.SetColumnValue(renderer, item, key, value);
                        break;
                    }
            }
        }

        public override bool SupportsExport
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsImport
        {
            get
            {
                return false;
            }
        }

        public string getElencoAppuntamenti(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            var ret = new CCollection<Anagrafica.CRicontatto>();
            if (pid != 0)
            {
                using (var cursor = new Anagrafica.CRicontattiCursor())
                {
                    cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                    cursor.DataPrevista.SortOrder = Databases.SortEnum.SORT_DESC;
                    cursor.IDPersona.Value = pid;
                    cursor.IgnoreRights = true;
                    cursor.TipoAppuntamento.Value = "Appuntamento";
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                }
            }

            if (ret.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(ret.ToArray(), XMLSerializeMethod.Document);
            }
            else
            {
                return DMD.Strings.vbNullString;
            }
        }

        public bool CanSendSMS(CustomerCalls.SMSMessage item)
        {
            return Module.UserCanDoAction("send_sms");
        }

        public string SendSMS(object renderer)
        {
            string text = this.n2str(renderer, "text", "");
            CustomerCalls.SMSMessage sms = (CustomerCalls.SMSMessage)DMD.XML.Utils.Serializer.Deserialize(text);
            if (!CanSendSMS(sms))
                throw new PermissionDeniedException(Module, "SendSMS");
            var options = new Sistema.SMSDriverOptions();
            options.Mittente = sms.Options.GetValueString("Mittente", "");
            options.RichiediConfermaDiLettura = sms.Options.GetValueBool("RichiedeConfermaDiLettura", false);
            sms.MessageID = Sistema.SMSService.Send(sms.NumeroOIndirizzo, sms.Note, options);
            if (!string.IsNullOrEmpty(sms.MessageID))
            {
                var stato = Sistema.SMSService.GetStatus(sms.MessageID);
                sms.Options.SetValueString("DriverID", Sistema.SMSService.DefaultDriver.GetUniqueID());
                sms.Options.SetValueInt("StatoMessaggio", (int?)stato.MessageStatus);
                sms.Options.SetValueString("StatoMessaggioEx", stato.MessageStatusEx);
                sms.Options.SetValueDate("DataConsegna", stato.DeliveryTime);
                sms.DettaglioEsito = stato.MessageStatusEx;
                switch (stato.MessageStatus)
                {
                    case Sistema.MessageStatusEnum.Scheduled:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                            break;
                        }

                    case Sistema.MessageStatusEnum.Sent:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.OK;
                            break;
                        }

                    case Sistema.MessageStatusEnum.Delivered:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.OK;
                            break;
                        }

                    case Sistema.MessageStatusEnum.Error:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            break;
                        }

                    case Sistema.MessageStatusEnum.Timeout:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            break;
                        }

                    case Sistema.MessageStatusEnum.BadNumber:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            break;
                        }

                    case Sistema.MessageStatusEnum.Waiting:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                            break;
                        }

                    case Sistema.MessageStatusEnum.Unknown:
                        {
                            sms.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                            break;
                        }
                }
            }

            sms.Stato = Databases.ObjectStatus.OBJECT_VALID;
            sms.Save();
            return DMD.XML.Utils.Serializer.Serialize(sms, XMLSerializeMethod.Document);
        }

        public bool CanSendFax(CustomerCalls.FaxDocument item)
        {
            return Module.UserCanDoAction("send_fax");
        }

        public string SendFax(object renderer)
        {
            string text = this.n2str(renderer, "text", "");
            CustomerCalls.FaxDocument fax = (CustomerCalls.FaxDocument)DMD.XML.Utils.Serializer.Deserialize(text);
            if (!CanSendFax(fax))
                throw new PermissionDeniedException(Module, "SendFax");
            string driverName = fax.Options.GetValueString("FAXDriver");
            string modemName = fax.Options.GetValueString("FAXModem");
            Sistema.BaseFaxDriver driver = null;
            Sistema.FaxDriverModem modem = null;
            if (!string.IsNullOrEmpty(driverName))
            {
                driver = Sistema.FaxService.GetDriver(driverName);
                if (driver is null)
                    throw new InvalidOperationException("Driver non installato: " + driverName);
            }

            if (!string.IsNullOrEmpty(modemName))
            {
                foreach (Sistema.FaxDriverModem modem1 in driver.Modems)
                {
                    if ((modem1.Name ?? "") == (modemName ?? ""))
                    {
                        modem = modem1;
                        break;
                    }
                }

                if (modem is null)
                    throw new InvalidOperationException("Modem non installato: " + modemName);
                modemName = modem.Name;
            }

            var job = Sistema.FaxService.NewJob();
            job.Options.SenderName = fax.Options.GetValueString("Mittente", "");
            job.Options.TargetNumber = fax.NumeroOIndirizzo;
            job.Options.FileName = Sistema.ApplicationContext.MapPath(fax.Attachment.URL);
            // options.RichiediConfermaDiLettura = fax.Options.GetValueBool("RichiedeConfermaDiLettura", False)
            job.Options.ModemName = modemName;
            if (driver is null)
            {
                fax.MessageID = Sistema.FaxService.Send(job);
            }
            else
            {
                fax.MessageID = Sistema.FaxService.Send(driver, job);
            }

            switch (job.JobStatus)
            {
                case Sistema.FaxJobStatus.CANCELLED:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                        fax.DettaglioEsito = "Annullato: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.COMPLETED:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.OK;
                        fax.DettaglioEsito = "Inviato: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.DIALLING:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                        fax.DettaglioEsito = "Chiamata in corso: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.ERROR:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                        fax.DettaglioEsito = "Errore: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.PREPARING:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                        fax.DettaglioEsito = "In preparazione: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.QUEUED:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                        fax.DettaglioEsito = "Messo in coda: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.SENDING:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                        fax.DettaglioEsito = "Trasmissione in corsor: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.TIMEOUT:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                        fax.DettaglioEsito = "Errore, Timeout della trasmissione: " + job.JobStatusMessage;
                        break;
                    }

                case Sistema.FaxJobStatus.WAITRETRY:
                    {
                        fax.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                        fax.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                        fax.DettaglioEsito = "Errore, in attesa di ritrasmissione: " + job.JobStatusMessage;
                        break;
                    }
            }
            // If (fax.MessageID <> "") Then
            // fax.StatoConversazione = StatoConversazione.INATTESA
            // End If
            fax.Stato = Databases.ObjectStatus.OBJECT_VALID;
            fax.Save();
            return DMD.XML.Utils.Serializer.Serialize(fax, XMLSerializeMethod.Document);
        }

        public string CheckBackListed(object renderer)
        {
            string indirizzo = this.n2str(renderer, "addr", "");
            string tipo = this.n2str(renderer, "taddr", "");
            var ret = CustomerCalls.BlackListAdresses.CheckBlocked(tipo, indirizzo);
            if (ret is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string BlackListAddress(object renderer)
        {
            string indirizzo = this.n2str(renderer, "addr", "");
            CustomerCalls.BlackListType tipo = (CustomerCalls.BlackListType)this.n2int(renderer, "tp");
            string tipoContatto = this.n2str(renderer, "taddr", "");
            string motivo = this.n2str(renderer, "mot", "");
            var ret = CustomerCalls.BlackListAdresses.BlockAddress(tipoContatto, indirizzo, tipo, motivo);
            if (ret is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string UnBlackListAddress(object renderer)
        {
            string indirizzo = this.n2str(renderer, "addr", "");
            string tipo = this.n2str(renderer, "taddr", "");
            var ret = CustomerCalls.BlackListAdresses.UnblockAddress(tipo, indirizzo);
            if (ret is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string GetStats(object renderer)
        {
            var ret = new CKeyCollection<CustomerCalls.CRMStatsAggregation>();
            Anagrafica.CRMFilter filter = (Anagrafica.CRMFilter)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "filter", ""));
            ret.Add("Telefonate", CustomerCalls.Telefonate.GetStats(filter));
            ret.Add("Visite", CustomerCalls.Visite.GetStats(filter));
            ret.Add("SMS", CustomerCalls.SMS.GetStats(filter));
            ret.Add("FAX", CustomerCalls.FAX.GetStats(filter));
            ret.Add("Telegrammi", CustomerCalls.Telegrammi.GetStats(filter));
            return DMD.XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document);
        }

        public string GetActivePersons(object renderer)
        {
            Anagrafica.CPersonaCursor cursor = null;
            try
            {
                Anagrafica.CRMFilter filter = (Anagrafica.CRMFilter)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "filter", ""));
                if (filter is null)
                    throw new ArgumentNullException("filter");
                if (!string.IsNullOrEmpty(this.n2str(renderer, "cursor", "")))
                {
                    cursor = (Anagrafica.CPersonaCursor)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "cursor", ""));
                }

                if (cursor is null)
                {
                    cursor = new Anagrafica.CPersonaCursor();
                    cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                }

                CCollection<Sistema.CActivePerson> ret = null;
                if ((filter.NomeLista ?? "") == DMD.Strings.vbCrLf + "[Suggeriti]")
                {
                    // ret = CustomerCalls.Telefonate.GetSuggeriti(filter, cursor)
                    filter.NomeLista = "Suggeriti";
                    ret = Anagrafica.Ricontatti.GetActivePersons(filter, cursor);
                }
                else
                {
                    ret = Anagrafica.Ricontatti.GetActivePersons(filter, cursor);
                }

                 

                foreach (Sistema.CActivePerson r in ret)
                {
                    if (r.Person is object)
                    {
                        if (r.Person.TipoPersona == Anagrafica.TipoPersona.PERSONA_FISICA)
                        {
                            {
                                var withBlock = (Anagrafica.CPersonaFisica)r.Person;
                                r.MoreInfo.Add("Nato A:", withBlock.NatoA.NomeComune + " il " + Sistema.Formats.FormatUserDate(withBlock.DataNascita));
                                if (withBlock.ImpiegoPrincipale is object)
                                {
                                    r.MoreInfo.Add("Impiego", FormatImpiego(withBlock.ImpiegoPrincipale));
                                }
                            }
                        }
                        // Dim u As minidom.CustomerCalls.CContattoUtente = ultimiContatti.GetItemByKey("K" & r.PersonID)
                        // If (u IsNot Nothing) Then r.MoreInfo.Add("UltimaChiamata", u)
                    }
                }

                // For i As Integer = 0 To ret.Count - 1
                // Dim r As CActivePerson = ret(i)
                // If (r.Person IsNot Nothing) Then
                // If (r.Person.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                // With DirectCast(r.Person, CPersonaFisica)
                // r.MoreInfo.Add("Nato A:", .NatoA.NomeComune & " il " & Formats.FormatUserDate(.DataNascita))
                // If .ImpiegoPrincipale IsNot Nothing Then
                // r.MoreInfo.Add("Impiego", .ImpiegoPrincipale.NomeAzienda)
                // r.MoreInfo.Add("TipoRapporto", .ImpiegoPrincipale.TipoRapporto)
                // End If
                // End With
                // End If
                // 'Dim u As minidom.CustomerCalls.CContattoUtente = CustomerCalls.Telefonate.GetUltimaChiamata(r.Person)
                // 'If (u IsNot Nothing) Then
                // '    r.MoreInfo.Add("UltimaChiamata", u)
                // 'End If

                // End If
                // Next


                return DMD.XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
                throw;
            }
            finally
            {
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
            }
        }

        private string FormatImpiego(Anagrafica.CImpiegato impiego)
        {
            string ret = "";
            if (!string.IsNullOrEmpty(impiego.Posizione))
                ret = impiego.Posizione;
            if (!string.IsNullOrEmpty(impiego.TipoRapporto))
                ret = DMD.Strings.Combine(ret, "(" + impiego.TipoRapporto + ")", " ");
            if (!string.IsNullOrEmpty(impiego.NomeAzienda))
                ret = DMD.Strings.Combine(ret, impiego.NomeAzienda, " presso ");
            if (impiego.DataAssunzione.HasValue)
                ret = DMD.Strings.Combine(ret, Sistema.Formats.FormatUserDate(impiego.DataAssunzione), " dal ");
            return ret;
        }

        public string GetContattiInAttesa(object renderer)
        {
            var po = this.n2int(renderer, "po");
            var op = this.n2int(renderer, "op");
            var items = CustomerCalls.CRM.GetTelefonateInAttesa(po, op);
            if (items.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(items.ToArray());
            }
            else
            {
                return "";
            }
        }

        public string ContaContattiInAttesa(object renderer)
        {
            var po = this.n2int(renderer, "po");
            var op = this.n2int(renderer, "op");
            return DMD.XML.Utils.Serializer.SerializeInteger(CustomerCalls.CRM.ContaContattiInAttesa(po, op));
        }

        public string GetUltimaVisitaInCorso(object renderer)
        {
            int pID = (int)this.n2int(renderer, "pid");
            var visita = CustomerCalls.Visite.GetUltimaVisitaInCorso(pID);
            if (visita is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(visita);
        }

        public string GetUltimaTelefonataInCorso(object renderer)
        {
            int pID = (int) this.n2int(renderer, "pid");
            var visita = CustomerCalls.Telefonate.GetUltimaTelefonataInCorso(pID);
            if (visita is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(visita);
        }

        // Public Overrides Function ExecuteAction(renderer As Object, actionName As String) As String
        // Select Case actionName
        // Case "getStoricoContatti" : Return Me.getStoricoContatti
        // Case "EsportaContatti" : Return Me.EsportaContatti
        // Case "ImportaContatti" : Return Me.ImportaContatti
        // Case Else : Return MyBase.ExecuteAction(renderer, actionName)
        // End Select
        // End Function

        public string GetRicontattiModificati(object renderer)
        {
            Anagrafica.CRMFilter filter = (Anagrafica.CRMFilter)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "filter", ""));
            return DMD.XML.Utils.Serializer.Serialize(Anagrafica.Ricontatti.GetRicontattiModificati(filter));
        }

        public string GetPersoneContattate(object renderer)
        {
            CustomerCalls.CContattoUtenteCursor cursor = null;
            IDataReader dbRis = null;
            try
            {
                cursor = (CustomerCalls.CContattoUtenteCursor)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "c", ""));
                string dbSQL = "SELECT [IDPersona] FROM (" + cursor.GetSQL() + ") GROUP BY [IDPersona]";
                cursor.Dispose();
                cursor = null;
                dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL);
                var buffer = new System.Text.StringBuilder();
                while (dbRis.Read())
                {
                    int id = Sistema.Formats.ToInteger(dbRis["IDPersona"]);
                    if (id != 0)
                    {
                        if (buffer.Length > 0)
                            buffer.Append(",");
                        buffer.Append(Databases.DBUtils.DBNumber(id));
                    }
                }

                dbRis.Dispose();
                dbRis = null;
                var ret = new CCollection<Anagrafica.CPersonaFisica>();
                if (buffer.Length > 0)
                {
                    dbSQL = "SELECT * FROM [tbl_Persone] WHERE [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + " AND [ID] In (" + buffer.ToString() + ")";
                    dbRis = Databases.APPConn.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        Anagrafica.TipoPersona tipoPersona = (Anagrafica.TipoPersona)Sistema.Formats.ToInteger(dbRis["TipoPersona"]);
                        var p = Anagrafica.Persone.Instantiate(tipoPersona);
                        Databases.APPConn.Load(p, dbRis);
                        ret.Add((Anagrafica.CPersonaFisica)p);
                    }

                    dbRis.Dispose();
                    dbRis = null;
                }

                return DMD.XML.Utils.Serializer.Serialize(ret);
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
                throw;
            }
            finally
            {
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                if (dbRis is object)
                {
                    dbRis.Dispose();
                    dbRis = null;
                }
            }
        }

        public string GetIDPersoneContattate(object renderer)
        {
            CustomerCalls.CContattoUtenteCursor cursor = null;
            IDataReader dbRis = null;
            try
            {
                cursor = (CustomerCalls.CContattoUtenteCursor)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "c", ""));
                string dbSQL = "SELECT [IDPersona] FROM (" + cursor.GetSQL() + ") GROUP BY [IDPersona]";
                cursor.Dispose();
                cursor = null;
                dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL);
                var ret = new CCollection<int>();
                while (dbRis.Read())
                {
                    int id = Sistema.Formats.ToInteger(dbRis["IDPersona"]);
                    if (id != 0)
                        ret.Add(id);
                }

                dbRis.Dispose();
                dbRis = null;
                return DMD.XML.Utils.Serializer.Serialize(ret);
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
                throw;
            }
            finally
            {
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                if (dbRis is object)
                {
                    dbRis.Dispose();
                    dbRis = null;
                }
            }
        }

        public string AggregatoContatti(object renderer)
        {
            using (var cursor = (CustomerCalls.CContattoUtenteCursor)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "cursor", "")))
            {
                string periodo = this.n2str(renderer, "periodo");
                var ret = CustomerCalls.CRM.AggregatoContatti(cursor, periodo);
                return DMD.XML.Utils.Serializer.Serialize(ret);
            }            
        }
    }
}