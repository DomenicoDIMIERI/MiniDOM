
namespace minidom.Forms
{
    public class CRapportiniCreateEventHandler : Sistema.CBaseEventHandler
    {
        public override void NotifyEvent(Sistema.EventDescription e)
        {
            string msg = Sistema.Users.CurrentUser.UserName + " (" + Sistema.Users.CurrentUser.Nominativo + ") - " + e.Descrizione;
            System.Net.Mail.MailMessage m = Sistema.EMailer.PrepareMessage(Finanziaria.Configuration.FromAddress, Finanziaria.Configuration.NotifyChangesTo, "", "", msg, msg, "", true);
            Sistema.EMailer.SendMessageAsync((Net.Mail.MailMessageEx)m, true);
        }
    }
}