
namespace minidom.Forms
{
    public class RichiesteECHandler : CBaseModuleHandler
    {
        public RichiesteECHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.EstrattiContributiviCursor();
        }
    }
}