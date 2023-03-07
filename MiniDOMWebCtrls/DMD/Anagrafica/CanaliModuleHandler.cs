
namespace minidom.Forms
{
    public class CanaliModuleHandler : CBaseModuleHandler
    {
        public CanaliModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CCanaleCursor();
        }
    }
}