
namespace minidom.Forms
{
    public class ImportFromCLHandler : CBaseModuleHandler
    {
        public ImportFromCLHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CCQSPDOfferteCursor();
        }
    }
}