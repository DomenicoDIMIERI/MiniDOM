
namespace minidom.Forms
{
    public class SpedizioniHandler : CBaseModuleHandler
    {
        public SpedizioniHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.SpedizioniCursor();
        }
    }
}