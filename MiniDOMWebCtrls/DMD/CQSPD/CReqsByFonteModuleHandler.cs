
namespace minidom.Forms
{
    public class CReqsByFonteModuleHandler : CQSPDBaseStatsHandler
    {
        public CReqsByFonteModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}