
namespace minidom.Forms
{
    public class PrimaNotaHandler : CBaseModuleHandler
    {
        public PrimaNotaHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SDelete | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.RigaPrimaNotaCursor();
        }
    }
}