
namespace minidom.Forms
{
    public class CEstinzioniModuleHandler : CBaseModuleHandler
    {
        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CEstinzioniCursor();
        }
    }
}