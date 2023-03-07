
namespace minidom.Forms
{
    public class VeicoliHandler : CBaseModuleHandler
    {
        public VeicoliHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.VeicoliCursor();
        }
    }
}