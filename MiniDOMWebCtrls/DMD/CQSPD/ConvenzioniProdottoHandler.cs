
namespace minidom.Forms
{
    public class ConvenzioniProdottoHandler : CBaseModuleHandler
    {
        public ConvenzioniProdottoHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CQSPDConvenzioniCursor();
        }
    }
}