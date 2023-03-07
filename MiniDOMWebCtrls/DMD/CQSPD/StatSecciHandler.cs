
namespace minidom.Forms
{
    public class StatSecciHandler : CQSPDBaseStatsHandler
    {
        public StatSecciHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}