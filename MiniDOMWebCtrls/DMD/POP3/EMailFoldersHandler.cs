
namespace minidom.Forms
{
    public class EMailFoldersHandler : CBaseModuleHandler
    {
        public EMailFoldersHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MailFolderCursor();
        }
    }
}