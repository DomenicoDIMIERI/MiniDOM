
namespace minidom.Forms
{
    public class CQSPDPratPerYearChartMHandler : CQSPDBaseStatsHandler
    {
        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}