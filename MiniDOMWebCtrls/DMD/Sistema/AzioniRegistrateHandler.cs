
namespace minidom.Forms
{


    // --------------------------------------------------------
    public class AzioniRegistrateHandler : CBaseModuleHandler
    {
        public AzioniRegistrateHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Sistema.Notifiche.RegisteredActions.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.AzioneRegistrataCursor();
        }
    }
}