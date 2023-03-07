
namespace minidom.Forms
{
    public class CSysEventsModuleHandler : CBaseModuleHandler
    {
        public CSysEventsModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.CEventsCursor();
        }
    }
}