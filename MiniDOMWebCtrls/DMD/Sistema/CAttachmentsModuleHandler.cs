
namespace minidom.Forms
{
    public class CAttachmentsModuleHandler : CBaseModuleHandler
    {
        public CAttachmentsModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SExport | ModuleSupportFlags.SImport)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.CAttachmentsCursor();
        }
    }
}