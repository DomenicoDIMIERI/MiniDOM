
namespace minidom.Forms
{
    public class DocumentiContabiliHandler : CBaseModuleHandler
    {
        public DocumentiContabiliHandler()
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Office.DocumentiContabili.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.DocumentoContabileCursor();
        }
    }
}