
namespace minidom.Forms
{
    public class CRMEditActionHandler : Sistema.CBaseEventHandler
    {
        public override void NotifyEvent(Sistema.EventDescription e)
        {
            string toAddress = CustomerCalls.CRM.Module.Settings.GetValueString("NotifyChangesTo", "");
            if (!string.IsNullOrEmpty(toAddress))
            {
                CustomerCalls.CContattoUtente item = (CustomerCalls.CContattoUtente)e.Descrittore;
                if (item.Persona.TipoPersona == Anagrafica.TipoPersona.PERSONA_FISICA)
                {
                    switch (item.Scopo ?? "")
                    {
                        case "Richiesta Conteggio Estintivo":
                            {
                                string text = "Il cliente " + item.NomePersona + " (CF: " + Sistema.Formats.FormatCodiceFiscale(item.Persona.CodiceFiscale) + ") ha richiesto un conteggio estintivo all'operatore " + item.NomeOperatore + " dell'ufficio di " + item.NomePuntoOperativo;
                                System.Net.Mail.MailMessage m = Sistema.EMailer.PrepareMessage("robot@minidom.net", toAddress, "", "", text, e.Descrizione, "", false);
                                Sistema.EMailer.SendMessageAsync((Net.Mail.MailMessageEx)m, true);
                                break;
                            }
                    }
                }
            }
        }
    }
}