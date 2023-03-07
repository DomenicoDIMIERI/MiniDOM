
namespace minidom.Forms
{
    public class MailAppsHandler : CBaseModuleHandler
    {
        public MailAppsHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MailApplicationCursor();
        }

         
    }
}