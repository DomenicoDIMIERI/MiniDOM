
namespace minidom.Forms
{
    public class UtenzeHandler : CBaseModuleHandler
    {
        public UtenzeHandler()
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Office.Utenze.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.UtenzeCursor();
        }
    }
}