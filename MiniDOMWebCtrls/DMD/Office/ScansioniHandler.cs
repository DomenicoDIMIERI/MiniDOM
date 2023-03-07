
namespace minidom.Forms
{
    public class ScansioniHandler : CBaseModuleHandler
    {
        public ScansioniHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.ScansioneCursor();
        }
    }
}