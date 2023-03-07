
namespace minidom.Forms
{
    public class CGDEModuleHandler : CBaseModuleHandler
    {
        public CGDEModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.DocumentiCaricatiCursor();
        }
    }
}