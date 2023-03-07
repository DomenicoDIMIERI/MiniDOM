using System;
using System.Diagnostics;
using DMD;
using DMD.Databases.Collections;
using DMD.SMS;

namespace minidom
{
    public partial class ADV
    {


        /// <summary>
        /// Gestore generico delle campagne SMS
        /// </summary>
        /// <remarks></remarks>
        public class HandlerTipoCampagnaSMS 
            : HandlerTipoCampagna
        {
            private static HandlerTipoCampagnaSMS m_Instance = null;

            /// <summary>
            /// Costrutore
            /// </summary>
            private HandlerTipoCampagnaSMS()
            {
                Sistema.SMSService.SMSReceived += handleSMSReceived;
                //Sistema.SMSService.SMSSent += handleSMSReceived;
            }

            /// <summary>
            /// Gestisce gli eventi SMS
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void handleSMSReceived(object sender, SMSReceivedEventArgs e)
            {
                //TODO Gestione dei messaggi SMS ricevuti
                System.Diagnostics.Debug.Print(e.MessageID);
            }


            /// <summary>
            /// Restituisce l'istanza predefinita dell'handler
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static HandlerTipoCampagnaSMS Instance
            {
                get
                {
                    if (m_Instance is null)
                        m_Instance = new HandlerTipoCampagnaSMS();
                    return m_Instance;
                }
            }

            /// <summary>
            /// Restituisce il tipo dell'handler
            /// </summary>
            /// <returns></returns>
            public override TipoCampagnaPubblicitaria GetHandledType()
            {
                return TipoCampagnaPubblicitaria.SMS;
            }

            /// <summary>
            /// Restituisce il nome dell'handler
            /// </summary>
            /// <returns></returns>
            public override string GetNomeMezzoSpedizione()
            {
                return "SMS";
            }

            /// <summary>
            /// Restituisce true se l'indirizzo di spedizione é bannato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBanned(CRisultatoCampagna res)
            {
                return minidom.ADV.Configuration.BannedSMSAddresses.IsBanned(res.IndirizzoDestinatario);
            }

            /// <summary>
            /// Restituisce true se l'indirizzo é bloccato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBlocked(CRisultatoCampagna res)
            {
                return CustomerCalls.BlackListAdresses.CheckBlocked("Cellulare", res.IndirizzoDestinatario) is object || CustomerCalls.BlackListAdresses.CheckBlocked("Telefono", res.IndirizzoDestinatario) is object;
            }

            /// <summary>
            /// Invia il messaggio
            /// </summary>
            /// <param name="item"></param>
            public override void Send(CRisultatoCampagna item)
            {
                //                string testo;
                //                if (item is null)
                //                    throw new ArgumentNullException("item");
                //                if (item.Campagna is null)
                //                    throw new ArgumentNullException("item.Campagna");

                //                //TODO invio del messaggio
                //                //if (Sistema.SMSService.DefaultDriver is null)
                //                //    throw new InvalidOperationException("Nessun driver installato per inviare SMS");
                //                //if (!Sistema.SMSService.IsValidNumber(item.IndirizzoDestinatario))
                //                //    return;
                //                //var options = new Sistema.SMSDriverOptions();
                //                //options.Mittente = item.Campagna.NomeMittente;
                //                //options.RichiediConfermaDiLettura = item.Campagna.RichiediConfermaDiLettura;
                //#if (!DEBUG)
                //                try {
                //#endif
                //                    testo = item.ParseTemplate(item.Campagna.Testo);
                //#if (!DEBUG)
                //            } catch (Exception ex) {
                //                    throw new InvalidOperationException("Errore durante il parsing del testo della campagna", ex);
                //            }
                //#endif

                //                string msgID = Sistema.SMSService.Send(item.IndirizzoDestinatario, testo, options);
                //                item.MessageID = msgID;
                //                var stato = Sistema.SMSService.GetStatus(msgID);
                //                switch (stato.MessageStatus)
                //                {
                //                    case MessageStatusEnum.BadNumber:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                //                            item.StatoSpedizioneEx = "Numero errato";
                //                            break;
                //                        }

                //                    case MessageStatusEnum.Delivered:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.Inviato;
                //                            item.DataConsegna = stato.DeliveryTime;
                //                            break;
                //                        }

                //                    case MessageStatusEnum.Error:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                //                            item.StatoSpedizioneEx = stato.MessageStatusEx;
                //                            break;
                //                        }

                //                    case MessageStatusEnum.Scheduled:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                //                            item.StatoSpedizioneEx = "Invio ritardato";
                //                            break;
                //                        }

                //                    case MessageStatusEnum.Sent:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.Inviato;
                //                            break;
                //                        }

                //                    case MessageStatusEnum.Timeout:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                //                            item.StatoSpedizioneEx = "Timeout";
                //                            break;
                //                        }

                //                    case MessageStatusEnum.Waiting:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                //                            item.StatoSpedizioneEx = "Ritardato";
                //                            break;
                //                        }

                //                    default:
                //                        {
                //                            item.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                //                            item.StatoSpedizioneEx = stato.MessageStatusEx;
                //                            break;
                //                        }
                //                }

                //TODO ADV send sms
                throw new NotImplementedException();
            }

            /// <summary>
            /// Prepara i messaggi da inviare
            /// </summary>
            /// <param name="campagna"></param>
            /// <param name="item"></param>
            /// <returns></returns>
            public override CCollection<CRisultatoCampagna> PrepareResults(CCampagnaPubblicitaria campagna, Anagrafica.CPersona item)
            {
                if (campagna is null)
                    throw new ArgumentNullException("campagna");
                if (item is null)
                    throw new ArgumentNullException("item");
                var ret = new CCollection<CRisultatoCampagna>();
                foreach (Anagrafica.CContatto contatto in item.Recapiti)
                {
                    if (
                           Strings.LCase(Strings.Left(contatto.Valore, 1)) == "3" 
                        && Sistema.SMSService.IsValidNumber(contatto.Valore) 
                        && (!campagna.UsaSoloIndirizziVerificati || (contatto.Validated ?? true))
                       )
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

            // Public Overrides Function GetListaInvio(ByVal campagna As CCampagnaPubblicitaria) As CCollection(Of CRisultatoCampagna)
            // If (campagna Is Nothing) Then Throw New ArgumentNullException("campagna")

            // Dim ret As New CCollection(Of CRisultatoCampagna)
            // Dim res As CRisultatoCampagna


            // If (campagna.UsaListaDinamica) Then
            // Dim items As CCollection(Of CPersonaInfo) = Anagrafica.Persone.Find(campagna.ParametriLista, True)
            // If (items Is Nothing) Then Throw New InvalidOperationException("La funzione Find ha restituito NULL")
            // For Each item As CPersonaInfo In items
            // Dim cc As CCollection(Of CRisultatoCampagna) = Me.PrepareResults(campagna, item.Persona)
            // For Each res In cc
            // res.Stato = ObjectStatus.OBJECT_VALID
            // res.Campagna = campagna
            // res.Destinatario = item.Persona
            // res.TipoCampagna = campagna.TipoCampagna
            // res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
            // ret.Add(res)
            // Next
            // Next
            // Else
            // Dim addresses() As String = Split(campagna.ParametriLista, ";")
            // For Each address In addresses
            // res = New CRisultatoCampagna
            // Me.ParseAddress(address, res.NomeDestinatario, res.IndirizzoDestinatario)
            // If (res.IndirizzoDestinatario <> "") Then
            // res.Stato = ObjectStatus.OBJECT_VALID
            // res.Campagna = campagna
            // res.TipoCampagna = campagna.TipoCampagna
            // res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione
            // ret.Add(res)
            // End If
            // Next
            // End If


            // Return ret
            // End Function

            /// <summary>
            /// Interpreta l'indirizzo
            /// </summary>
            /// <param name="str"></param>
            /// <param name="nome"></param>
            /// <param name="address"></param>
            public override void ParseAddress(
                                    string str, 
                                    ref string nome, 
                                    ref string address
                                    )
            {
                str = Strings.Replace(Strings.Trim(str), "  ", " ");
                str = Strings.Replace(str, DMD.Strings.vbCr, "");
                str = Strings.Replace(str, DMD.Strings.vbLf, "");
                nome = "";
                address = "";
                if (string.IsNullOrEmpty(str))
                    return;
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

                address = Sistema.Formats.ParsePhoneNumber(address);
            }

            /// <summary>
            /// Restituisce true se l'handler supporta la richiesta di conferma lettura
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaLettura()
            {
                return true;
            }

            /// <summary>
            /// Restituisce true se l'handler supporta la richiesta di conferma recapito
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaRecapito()
            {
                //if (Sistema.SMSService.DefaultDriver is null)
                //    throw new InvalidOperationException("Nessun driver installato per inviare SMS");
                //return Sistema.SMSService.DefaultDriver.SupportaConfermaDiRecapito();
                return true;
            }

            /// <summary>
            /// Aggionra lo stato del messaggio
            /// </summary>
            /// <param name="res"></param>
            public override void UpdateStatus(CRisultatoCampagna res)
            {
                var msg = Sistema.SMSService.GetMessageById(res.MessageID);
                switch (msg.MessageStatus)
                {
                    case MessageStatusEnum.BadNumber:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                            res.StatoSpedizioneEx = "Numero errato";
                            break;
                        }

                    case MessageStatusEnum.Delivered:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.Inviato;
                            res.DataConsegna = msg.DeliveryTime;
                            res.StatoSpedizioneEx = "Inviato";
                            break;
                        }

                    case MessageStatusEnum.Error:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                            res.StatoSpedizioneEx = msg.MessageStatusEx;
                            break;
                        }

                    case MessageStatusEnum.Scheduled:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                            res.StatoSpedizioneEx = "Invio ritardato";
                            break;
                        }

                    case MessageStatusEnum.Sent:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.Inviato;
                            res.StatoSpedizioneEx = "Inviato";
                            break;
                        }

                    case MessageStatusEnum.Timeout:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                            res.StatoSpedizioneEx = "Timeout";
                            break;
                        }

                    case MessageStatusEnum.Waiting:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.ProntoPerLaSpedizione;
                            res.StatoSpedizioneEx = "Ritardato";
                            break;
                        }

                    default:
                        {
                            res.StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
                            res.StatoSpedizioneEx = msg.MessageStatusEx;
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce true se il numero é valido per l'invio di un sms
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public override bool IsValidAddress(string address)
            {
                return Sistema.SMSService.IsValidNumber(address);
            }
        }
    }
}