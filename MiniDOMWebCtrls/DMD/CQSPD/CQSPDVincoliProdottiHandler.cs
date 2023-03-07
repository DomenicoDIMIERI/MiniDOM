
namespace minidom.Forms
{
    public class CQSPDVincoliProdottiHandler : CBaseModuleHandler
    {
        public CQSPDVincoliProdottiHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return Finanziaria.VincoliProdotto.CreateCursor();
        }
    }
}