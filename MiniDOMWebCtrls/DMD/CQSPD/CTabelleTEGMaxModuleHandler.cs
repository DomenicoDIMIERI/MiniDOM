
namespace minidom.Forms
{
    public class CTabelleTEGMaxModuleHandler : CBaseModuleHandler
    {
        public CTabelleTEGMaxModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CTabelleTEGMaxCursor();
        }
    }
}