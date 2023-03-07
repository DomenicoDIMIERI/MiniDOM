
namespace minidom.Forms
{
    public class CarteDiCreditoHandler : CBaseModuleHandler
    {
        public CarteDiCreditoHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CartaDiCreditoCursor();
        }
    }
}