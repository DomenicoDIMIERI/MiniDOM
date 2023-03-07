
namespace minidom.Forms
{
    public class OfficeCategorieArticoliHandler : CBaseModuleHandler
    {
        public OfficeCategorieArticoliHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.CategoriaArticoloCursor();
        }
    }

    public class OfficePBXHandler : CBaseModuleHandler
    {
        public OfficePBXHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.PBXCursor();
        }
    }
}