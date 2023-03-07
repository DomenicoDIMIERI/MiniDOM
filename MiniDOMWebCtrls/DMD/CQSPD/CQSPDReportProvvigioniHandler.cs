
namespace minidom.Forms
{
    public class CQSPDReportProvvigioniHandler : CQSPDBaseStatsHandler
    {
        public CQSPDReportProvvigioniHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}