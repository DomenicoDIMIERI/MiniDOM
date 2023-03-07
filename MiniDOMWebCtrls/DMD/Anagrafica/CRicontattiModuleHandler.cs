
namespace minidom.Forms
{
    public class CRicontattiModuleHandler : CBaseModuleHandler
    {
        public CRicontattiModuleHandler() : base(ModuleSupportFlags.SDelete)
        {

            // AddHandler CustomerCalls.CRM.NuovoContatto, AddressOf Me.handleNuovaTelefonata
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CRicontattiCursor();
        }
    }
}