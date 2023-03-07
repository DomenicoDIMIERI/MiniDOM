
namespace minidom.Forms
{
    public class CQSPDPreventiviConclusiHandler : CQSPDBaseStatsHandler
    {
        public CQSPDPreventiviConclusiHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}