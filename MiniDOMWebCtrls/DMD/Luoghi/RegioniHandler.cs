
namespace minidom.Forms
{
    public class RegioniModuleHandler : CBaseModuleHandler
    {
        public RegioniModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CRegioniCursor();
            ret.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            return ret;
        }
    }
}