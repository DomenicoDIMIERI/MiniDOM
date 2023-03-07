
namespace minidom.Forms
{
    public class CQSPDPratPerMeseChartMHandler : CQSPDBaseStatsHandler
    {
        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}