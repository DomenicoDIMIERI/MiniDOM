
namespace minidom.Forms
{
    public class CQSPDBaseStatsHandler : CBaseModuleHandler
    {
        public CQSPDBaseStatsHandler() : base()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}