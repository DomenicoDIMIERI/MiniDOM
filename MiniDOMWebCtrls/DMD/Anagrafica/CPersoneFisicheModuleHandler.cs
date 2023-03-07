
namespace minidom.Forms
{
    public class CPersoneFisicheModuleHandler : CBaseModuleHandler
    {
        public CPersoneFisicheModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CPersonaCursor();
            ret.Nominativo.SortOrder = Databases.SortEnum.SORT_ASC;
            ret.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
            return ret;
        }
    }
}