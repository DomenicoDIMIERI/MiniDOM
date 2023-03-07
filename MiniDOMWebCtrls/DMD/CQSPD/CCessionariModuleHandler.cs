
namespace minidom.Forms
{
    public class CCessionariModuleHandler : CBaseModuleHandler
    {
        public CCessionariModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CCessionariCursor();
            return cursor;
        }
    }
}