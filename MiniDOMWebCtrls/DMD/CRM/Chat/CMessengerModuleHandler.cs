
namespace minidom.Forms
{
    public class CMessengerModuleHandler : CBaseModuleHandler
    {
        public CMessengerModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Messenger.CMessagesCursor();
        }
    }
}