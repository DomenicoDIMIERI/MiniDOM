
namespace minidom.Forms
{
    public class CQSPDZoneHandler : CBaseModuleHandler
    {
        public CQSPDZoneHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CZonaCursor();
        }
    }
}