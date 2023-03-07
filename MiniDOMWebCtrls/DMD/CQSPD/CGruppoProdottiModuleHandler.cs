
namespace minidom.Forms
{
    public class CGruppoProdottiModuleHandler : CBaseModuleHandler
    {
        public CGruppoProdottiModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CGruppoProdottiCursor();
            return cursor;
        }
    }
}