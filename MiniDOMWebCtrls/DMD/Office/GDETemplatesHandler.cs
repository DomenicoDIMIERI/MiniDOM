
namespace minidom.Forms
{
    public class GDETemplatesHandler : CBaseModuleHandler
    {
        public GDETemplatesHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.DocumentTemplateCursor();
        }



        // Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
        // Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
        // ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
        // ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
        // Return ret
        // End Function


    }
}