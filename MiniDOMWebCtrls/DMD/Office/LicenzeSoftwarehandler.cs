
namespace minidom.Forms
{
    public class LicenzeSoftwarehandler : CBaseModuleHandler
    {
        public LicenzeSoftwarehandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.LicenzeSoftwareCursor();
        }
    }
}