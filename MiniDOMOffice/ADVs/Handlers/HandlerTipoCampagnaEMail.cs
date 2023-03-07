using System;
using System.Diagnostics;
using System.Net.Mail;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.Net;

namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Handler delle campagne pubblicitarie di tipo email
        /// </summary>
        public class HandlerTipoCampagnaEMail 
            : HandlerTipoCampagna
        {
            private static HandlerTipoCampagnaEMail m_Instance = null;


            /// <summary>
            /// Restituisce l'istanza predefinita dell'handler
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static HandlerTipoCampagnaEMail Instance
            {
                get
                {
                    if (m_Instance is null)
                        m_Instance = new HandlerTipoCampagnaEMail();
                    return m_Instance;
                }
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            private HandlerTipoCampagnaEMail()
            {
                Sistema.EMailer.MessageReceived += handleMessageReceived;
            }

            /// <summary>
            /// Gestisce i messaggi ricevuti via email
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void handleMessageReceived(object sender, MailMessageReceivedEventArgs e)
            {
                string key = "";
                key = e.Message.Headers["FSEMLHNDLR"];
                Debug.Print("Message received: " + key);
            }

            /// <summary>
            /// Restituisce il tipo dell'handler
            /// </summary>
            /// <returns></returns>
            public override TipoCampagnaPubblicitaria GetHandledType()
            {
                return TipoCampagnaPubblicitaria.eMail;
            }

            /// <summary>
            /// Restituisce il nome dell'handler
            /// </summary>
            /// <returns></returns>
            public override string GetNomeMezzoSpedizione()
            {
                return "e-mail";
            }

            /// <summary>
            /// Restituisce true se l'indirizzo è bannato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBanned(CRisultatoCampagna res)
            {
                return Configuration.BannedEMailAddresses.IsBanned(res.IndirizzoDestinatario);
            }

            /// <summary>
            /// Restituisce true se l'indirizzo é bloccato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBlocked(CRisultatoCampagna res)
            {
                return CustomerCalls.BlackListAdresses.CheckBlocked("e-Mail", res.IndirizzoDestinatario) is object;
            }

            /// <summary>
            /// Invia il messaggio
            /// </summary>
            /// <param name="item"></param>
            public override void Send(CRisultatoCampagna item)
            {
                if (!Sistema.EMailer.IsValidAddress(item.IndirizzoDestinatario))
                    return;
                string fromAddress = Strings.Trim(item.Campagna.IndirizzoMittente);
                if (string.IsNullOrEmpty(fromAddress))
                    throw new ArgumentNullException("Il mittente non può essere nullo");
                string titolo = item.ParseTemplate(item.Campagna.Titolo);
                string testo = item.ParseTemplate(item.Campagna.Testo);
                string toAddress = (Sistema.ApplicationContext.IsDebug())? "tecnico@minidom.net" : item.IndirizzoDestinatario;
                using (var m = Sistema.EMailer.PrepareMessage(fromAddress, toAddress, "", "", titolo, testo, item.Campagna.NomeMittente, true))
                {
                    if (item.Campagna.RichiediConfermaDiLettura)
                    {
                        m.Headers.Add("Disposition-Notification-To", item.Campagna.IndirizzoMittente);
                        string key = Strings.GetRandomString(25);
                        m.Headers.Add("FSEMLHNDLR", key);
                        // m.DeliveryNotificationOptions =
                    }

                    Sistema.EMailer.SendMessage(m);
                }

                item.StatoMessaggio = StatoMessaggioCampagna.Inviato;
                item.StatoSpedizioneEx = "Inviato";
                item.MessageID = "";
            }

            /// <summary>
            /// Prepara l'elenco dei messaggi da inviare alla persona
            /// </summary>
            /// <param name="campagna"></param>
            /// <param name="item"></param>
            /// <returns></returns>
            public override CCollection<CRisultatoCampagna> PrepareResults(CCampagnaPubblicitaria campagna, Anagrafica.CPersona item)
            {
                var ret = new CCollection<CRisultatoCampagna>();
                foreach (Anagrafica.CContatto contatto in item.Recapiti)
                {
                    if (Strings.LCase(contatto.Tipo) == "e-mail" && Sistema.EMailer.IsValidAddress(contatto.Valore) && (!campagna.UsaSoloIndirizziVerificati || contatto.Validated.HasValue == false || contatto.Validated.Value))
                    {
                        bool t = false;
                        foreach (CRisultatoCampagna tmp in ret)
                        {
                            if ((tmp.IndirizzoDestinatario ?? "") == (contatto.Valore ?? ""))
                            {
                                t = true;
                                break;
                            }
                        }

                        if (!t)
                        {
                            var r = new CRisultatoCampagna();
                            r.IndirizzoDestinatario = contatto.Valore;
                            ret.Add(r);
                        }

                        if (campagna.UsaUnSoloContattoPerPersona)
                            break;
                    }
                }

                return ret;
            }

            /// <summary>
            /// Interpreta l'indirizzo
            /// </summary>
            /// <param name="str"></param>
            /// <param name="nome"></param>
            /// <param name="address"></param>
            public override void ParseAddress(string str, ref string nome, ref string address)
            {
                str = Strings.Replace(Strings.Trim(str), "  ", " ");
                str = Strings.Replace(str, DMD.Strings.vbCr, "");
                str = Strings.Replace(str, DMD.Strings.vbLf, "");
                int i = Strings.InStr(str, "<");
                nome = "";
                address = "";
                if (i > 0)
                {
                    if (i > 1)
                    {
                        nome = Strings.Left(str, i - 1);
                        address = Strings.Trim(Strings.Mid(str, i + 1));
                        i = Strings.InStr(address, ">");
                        if (i > 0)
                        {
                            address = Strings.Trim(Strings.Left(address, i - 1));
                        }
                    }
                    else
                    {
                        address = str;
                    }
                }
                else
                {
                    address = str;
                }
            }

            /// <summary>
            /// COnferma la richiesta di lettura
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaLettura()
            {
                return true;
            }

            /// <summary>
            /// Conferma la richiesta di recapito
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaRecapito()
            {
                return false;
            }

            /// <summary>
            /// Aggionra lo stato del messaggio
            /// </summary>
            /// <param name="res"></param>
            public override void UpdateStatus(CRisultatoCampagna res)
            {
            }

            /// <summary>
            /// Restituisce true se l'indirizzo email é valido
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public override bool IsValidAddress(string address)
            {
                return Sistema.EMailer.IsValidAddress(address);
            }

            /// <summary>
            /// Restituisce la lista di invio
            /// </summary>
            /// <param name="campagna"></param>
            /// <returns></returns>
            public override CCollection<CRisultatoCampagna> GetListaInvio(CCampagnaPubblicitaria campagna)
            {
                var ret = new CCollection<CRisultatoCampagna>();
                CCollection<CRisultatoCampagna> cc;
                if (campagna.UsaListaDinamica && campagna.ParametriLista == "*")
                {
                    using (var cursor = new Anagrafica.CPersonaCursor())
                    {   
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.eMail.Value = "";
                        cursor.eMail.Operator = OP.OP_NE;
                        cursor.eMail.IncludeNulls = false;
                        while (cursor.Read()) // dbRis.Read
                        {
                            var persona = cursor.Item; // Anagrafica.Persone.Instantiate(Formats.ToInteger(dbRis("TipoPersona")))
                                                       // CRM.Database.Load(persona, dbRis)
                            var consenso = persona.GetFlag(Anagrafica.PFlags.CF_CONSENSOADV);
                            if (!consenso.HasValue || consenso.Value)
                            {
                                cc = PrepareResults(campagna, persona);
                                foreach (var res in cc)
                                {
                                    res.Stato = ObjectStatus.OBJECT_VALID;
                                    res.Campagna = campagna;
                                    res.Destinatario = persona;
                                    res.TipoCampagna = campagna.TipoCampagna;
                                    res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                                    if (!IsExcluded(res))
                                        ret.Add(res);
                                }
                            }

                            
                        }
                    }
                     
                }
                else
                {
                    ret = base.GetListaInvio(campagna);
                }

                return ret;
            }
        }
    }
}