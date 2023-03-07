
namespace minidom.Forms
{
    public class OfficeListiniArticoliHandler : CBaseModuleHandler
    {
        public OfficeListiniArticoliHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.ListinoCursor();
        }
    }
}