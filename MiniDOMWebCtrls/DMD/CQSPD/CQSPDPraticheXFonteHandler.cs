
namespace minidom.Forms
{
    public class CQSPDPraticheXFonteHandler : CQSPDBaseStatsHandler
    {
        public CQSPDPraticheXFonteHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}