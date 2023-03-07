
namespace minidom.Forms
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    public class TeamManagersModuleHandler : CBaseModuleHandler
    {
        public TeamManagersModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CTeamManagersCursor();
        }
    }





    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

}