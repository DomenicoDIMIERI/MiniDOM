
namespace minidom.Forms
{
    public class ContiCorrenteHandler : CBaseModuleHandler
    {
        public ContiCorrenteHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.ContoCorrenteCursor();
        }
    }
}