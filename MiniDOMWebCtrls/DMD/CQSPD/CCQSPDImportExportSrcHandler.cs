
namespace minidom.Forms
{
    public class CCQSPDImportExportSrcHandler : CBaseModuleHandler
    {
        public CCQSPDImportExportSrcHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SExport)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CImportExportSourceCursor();
        }
    }
}