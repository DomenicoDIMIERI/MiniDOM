
namespace minidom.Forms
{
    public class CQSPDObiettiviCategoriaProdottoHandler : CBaseModuleHandler
    {
        public CQSPDObiettiviCategoriaProdottoHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CObiettivoCategoriaProdottoCursor();
            return cursor;
        }
    }
}