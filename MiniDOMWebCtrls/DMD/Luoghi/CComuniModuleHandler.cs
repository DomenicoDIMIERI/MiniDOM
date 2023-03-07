
namespace minidom.Forms
{
    public class CComuniModuleHandler : CBaseModuleHandler
    {
        public CComuniModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CComuniCursor();
            ret.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            return ret;
        }
    }
}