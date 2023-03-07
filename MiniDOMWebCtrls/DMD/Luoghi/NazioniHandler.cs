
namespace minidom.Forms
{
    public class NazioniModuleHandler : CBaseModuleHandler
    {
        public NazioniModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CNazioniCursor();
            ret.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            return ret;
        }
    }
}