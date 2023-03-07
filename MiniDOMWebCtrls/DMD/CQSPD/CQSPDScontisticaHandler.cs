
namespace minidom.Forms
{
    public class CQSPDScontisticaHandler : CQSPDBaseStatsHandler
    {
        public CQSPDScontisticaHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}