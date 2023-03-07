
namespace minidom.Forms
{
    public class CVisiteModuleHandler : CBaseModuleHandler
    {
        public CVisiteModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.CVisiteCursor();
        }
    }
}