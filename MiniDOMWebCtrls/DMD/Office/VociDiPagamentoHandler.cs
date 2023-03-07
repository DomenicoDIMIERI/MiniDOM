
namespace minidom.Forms
{
    public class VociDiPagamentoHandler : CBaseModuleHandler
    {
        public VociDiPagamentoHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.VoceDiPagamentoCursor();
        }
    }
}