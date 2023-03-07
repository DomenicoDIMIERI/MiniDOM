
namespace minidom.Forms
{
    public class CategorieProdottoModuleHandler : CBaseModuleHandler
    {
        public CategorieProdottoModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CCategorieProdottoCursor();
            return cursor;
        }
    }
}