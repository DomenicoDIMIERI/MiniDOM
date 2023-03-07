
namespace minidom.Forms
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class CQSPDTempiPraticheHandler : CBaseModuleHandler
    {
        public CQSPDTempiPraticheHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }



    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}