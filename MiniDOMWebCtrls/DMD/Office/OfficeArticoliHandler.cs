
namespace minidom.Forms
{
    public class OfficeArticoliHandler : CBaseModuleHandler
    {
        public OfficeArticoliHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.ArticoloCursor();
        }
    }
}