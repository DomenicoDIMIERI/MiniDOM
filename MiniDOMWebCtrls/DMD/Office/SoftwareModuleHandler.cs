
namespace minidom.Forms
{
    public class SoftwareModuleHandler : CBaseModuleHandler
    {
        public SoftwareModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.SoftwareCursor();
        }
    }
}