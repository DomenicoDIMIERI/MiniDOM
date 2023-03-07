
namespace minidom.Forms
{
    public class OfferteModuleHandler : CBaseModuleHandler
    {
        public OfferteModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CCQSPDOfferteCursor();
        }
    }
}