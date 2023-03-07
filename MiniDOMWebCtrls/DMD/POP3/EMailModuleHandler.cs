
namespace minidom.Forms
{
    public class EMailModuleHandler : CBaseModuleHandler
    {
        public EMailModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MailMessageCursor();
        }

        public string DownloadMessages(object renderer)
        {
            int appID = (int)this.n2int(renderer, "appid");
            // Dim app As MailApplication = MailApplications.GetItemById(appID)
            var nitems = this.n2int(renderer, "nitems");
            var downloadedItems = new CCollection<Office.MailMessage>();
            // If (nitems.HasValue) Then
            // downloadedItems = app.DownloadEmails(nitems.Value)
            // Else
            // downloadedItems = app.DownloadEmails()
            // End If
            // app.CheckEMails()
            return DMD.XML.Utils.Serializer.Serialize(downloadedItems);
        }
         
        public string NotifyChanges(object renderer)
        {
            int appID = (int)this.n2int(renderer, "appid");
            // Dim app As MailApplication = MailApplications.GetItemById(appID)
            // app.NotifyChanges()
            return "";
        }
    }
}