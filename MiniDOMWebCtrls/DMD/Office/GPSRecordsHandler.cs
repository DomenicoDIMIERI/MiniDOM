
namespace minidom.Forms
{
    public class GPSRecordsHandler : CBaseModuleHandler
    {
        public GPSRecordsHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.GPSRecordCursor();
        }
    }
}