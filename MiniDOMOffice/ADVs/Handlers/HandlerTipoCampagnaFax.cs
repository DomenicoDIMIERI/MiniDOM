using System;
using DMD;
using DMD.Databases.Collections;
using DMD.FAX;

namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Handler delle campagne pubblicitarie fatte via Fax
        /// </summary>
        public class HandlerTipoCampagnaFax 
            : HandlerTipoCampagna
        {
            private static HandlerTipoCampagnaFax m_Instance = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            private HandlerTipoCampagnaFax()
            {
                Sistema.FaxService.FaxReceived += handleFaxReceived;
            }

            private void handleFaxReceived(object sender, FaxReceivedEventArgs e)
            {
                //TODO gestire la ricezione dei fax
            }

            /// <summary>
            /// Restituisce l'istanza predefinita dell'handler
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static HandlerTipoCampagnaFax Instance
            {
                get
                {
                    if (m_Instance is null)
                        m_Instance = new HandlerTipoCampagnaFax();
                    return m_Instance;
                }
            }

            /// <summary>
            /// Restituisce il tipo dell'handler
            /// </summary>
            /// <returns></returns>
            public override TipoCampagnaPubblicitaria GetHandledType()
            {
                return TipoCampagnaPubblicitaria.Fax;
            }

            /// <summary>
            /// Restituisce il nome dell'handler
            /// </summary>
            /// <returns></returns>
            public override string GetNomeMezzoSpedizione()
            {
                return "Fax";
            }

            /// <summary>
            /// Restituisce true se l'indirizzo del fax é bannato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBanned(CRisultatoCampagna res)
            {
                return Configuration.BannedFaxAddresses.IsBanned(res.IndirizzoDestinatario);
            }

            /// <summary>
            /// Restituisce true se il fax è bloccato
            /// </summary>
            /// <param name="res"></param>
            /// <returns></returns>
            public override bool IsBlocked(CRisultatoCampagna res)
            {
                return CustomerCalls.BlackListAdresses.CheckBlocked("Fax", res.IndirizzoDestinatario) is object || CustomerCalls.BlackListAdresses.CheckBlocked("Telefono", res.IndirizzoDestinatario) is object;
            }

            /// <summary>
            /// Invia il fax
            /// </summary>
            /// <param name="item"></param>
            public override void Send(CRisultatoCampagna item)
            {
                string testo;
                if (item is null)
                    throw new ArgumentNullException("item");
                if (item.Campagna is null)
                    throw new ArgumentNullException("item.Campagna");
                //if (Sistema.FaxService.DefaultDriver is null)
                //    throw new InvalidOperationException("Nessun driver installato per inviare SMS");
                //if (!Sistema.FaxService.IsValidNumber(item.IndirizzoDestinatario))
                //    return;
                //var options = new Sistema.FaxDriverOptions();
                //options.SenderName = item.Campagna.NomeMittente;
                //// options.RichiediConfermaDiLettur = item.Campagna.RichiediConfermaDiLettura

#if (!DEBUG)
                try {
#endif
                    testo = item.ParseTemplate(item.Campagna.Testo);
#if (!DEBUG)
            } catch (Exception ex) {
                    throw new InvalidOperationException("Errore durante il parsing del testo della campagna", ex);
                }
#endif
                //TODO invio fax tramite campagna pubblicitaria

                // Dim msgID As String = FaxService.Send(item.IndirizzoDestinatario, testo, options)
                // item.MessageID = msgID
                throw new NotImplementedException();
            }

            /// <summary>
            /// Prepara i messaggi da inviare ad una singola persona
            /// </summary>
            /// <param name="campagna"></param>
            /// <param name="item"></param>
            /// <returns></returns>
            public override CCollection<CRisultatoCampagna> PrepareResults(
                            CCampagnaPubblicitaria campagna, 
                            Anagrafica.CPersona item
                            )
            {
                var ret = new CCollection<CRisultatoCampagna>();
                foreach (Anagrafica.CContatto contatto in item.Recapiti)
                {
                    if (
                           Strings.LCase(contatto.Tipo) == "fax" 
                        && !string.IsNullOrEmpty(contatto.Valore) 
                        && (!campagna.UsaSoloIndirizziVerificati ||  (contatto.Validated ?? true))
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
            // 'If (Me.UsaListaDinamica) Then
            // Dim items As CCollection(Of CPersonaInfo)
            // Dim ret As New CCollection(Of CRisultatoCampagna)
            // Dim cc As CCollection(Of CRisultatoCampagna)
            // Dim res As CRisultatoCampagna


            // If (campagna.UsaListaDinamica) Then
            // items = Anagrafica.Persone.Find(campagna.ParametriLista, True)
            // For Each item As CPersonaInfo In items
            // cc = Me.PrepareResults(campagna, item.Persona)
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
            /// Restituisce true se l'handler supporta la richiesta di conferma di lettura
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaLettura()
            {
                return false;
            }

            /// <summary>
            /// Restituisce true se l'handler supporta la richiesta di conferma di recapito
            /// </summary>
            /// <returns></returns>
            public override bool SupportaConfermaRecapito()
            {
                return false;
            }

            /// <summary>
            /// Aggiorna lo stato del messaggio
            /// </summary>
            /// <param name="res"></param>
            public override void UpdateStatus(CRisultatoCampagna res)
            {
                //TODO aggiornare il messaggio adv in funzione dello stato di invio del fax
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce true se il numero è valido per l'invio di un fax
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            public override bool IsValidAddress(string address)
            {
                return Sistema.FaxService.IsValidNumber(address);
            }
        }
    }
}