
namespace minidom.Forms
{
    public class CQSPDPraticheXAmmHandler : CQSPDBaseStatsHandler
    {
        public CQSPDPraticheXAmmHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}