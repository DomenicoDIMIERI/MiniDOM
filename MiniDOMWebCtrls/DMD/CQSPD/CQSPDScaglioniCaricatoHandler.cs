
namespace minidom.Forms
{
    public class CQSPDScaglioniCaricatoHandler : CQSPDBaseStatsHandler
    {
        public CQSPDScaglioniCaricatoHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}