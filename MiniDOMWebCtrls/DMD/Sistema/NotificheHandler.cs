
namespace minidom.Forms
{


    // --------------------------------------------------------
    public class NotificheHandler : CBaseModuleHandler
    {
        public NotificheHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.NotificaCursor();
        }
    }
}