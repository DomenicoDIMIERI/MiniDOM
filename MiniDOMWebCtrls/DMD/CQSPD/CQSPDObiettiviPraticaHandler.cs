
namespace minidom.Forms
{
    public class CQSPDObiettiviPraticaHandler : CBaseModuleHandler
    {
        public CQSPDObiettiviPraticaHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CObiettivoPraticaCursor();
            return cursor;
        }

        public override object GetInternalItemById(int id)
        {
            return Finanziaria.Obiettivi.GetItemById(id);
        }
    }
}