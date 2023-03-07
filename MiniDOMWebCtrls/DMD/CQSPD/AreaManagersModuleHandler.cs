
namespace minidom.Forms
{


    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    public class AreaManagersModuleHandler : CBaseModuleHandler
    {
        public AreaManagersModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CAreaManagerCursor();
        }
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

}