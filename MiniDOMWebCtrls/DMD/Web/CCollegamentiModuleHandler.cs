
namespace minidom.Forms
{
    public class CCollegamentiModuleHandler : CBaseModuleHandler
    {
        public CCollegamentiModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new WebSite.CCollegamentiCursor();
        }
    }
}