
namespace minidom.Forms
{
    public class RilevatoriPresenzeHandler : CBaseModuleHandler
    {
        public RilevatoriPresenzeHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.RilevatoriPresenzeCursor();
        }
    }
}