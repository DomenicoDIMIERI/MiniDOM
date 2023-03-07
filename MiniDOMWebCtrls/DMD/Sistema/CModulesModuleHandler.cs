
namespace minidom.Forms
{

    // -------------------------------------------------------
    public class CModulesModuleHandler : CBaseModuleHandler
    {
        public CModulesModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.CModulesCursor();
        }
    }
}