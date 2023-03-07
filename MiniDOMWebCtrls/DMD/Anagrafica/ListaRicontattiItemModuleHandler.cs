
namespace minidom.Forms
{
    public class ListaRicontattiItemModuleHandler : CBaseModuleHandler
    {
        public ListaRicontattiItemModuleHandler() : base(ModuleSupportFlags.SDelete)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.ListaRicontattoItemCursor();
        }
    }
}