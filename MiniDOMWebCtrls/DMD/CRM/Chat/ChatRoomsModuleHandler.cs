
namespace minidom.Forms
{
    public class ChatRoomsModuleHandler : CBaseModuleHandler
    {
        public ChatRoomsModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Messenger.CChatRoomCursor();
        }
    }
}