
namespace minidom.Forms
{
    public class CQSPDMotiviScontoPraticaHandler : CBaseModuleHandler
    {
        public CQSPDMotiviScontoPraticaHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CMotivoScontoPraticaCursor();
            return cursor;
        }
    }
}