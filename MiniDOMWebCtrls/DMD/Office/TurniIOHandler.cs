
namespace minidom.Forms
{
    public class TurniIOHandler : CBaseModuleHandler
    {
        public TurniIOHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.TurniCursor();
        }
    }
}