
namespace minidom.Forms
{
    public class CUfficiModuleHandler : CBaseModuleHandler
    {
        public CUfficiModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CUfficiCursor();
        }
    }
}