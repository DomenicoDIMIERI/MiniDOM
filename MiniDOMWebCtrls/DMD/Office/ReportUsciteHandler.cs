
namespace minidom.Forms
{
    public class ReportUsciteHandler : CBaseModuleHandler
    {
        public ReportUsciteHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.UsciteCursor();
        }
    }
}