
namespace minidom.Forms
{
    public class VisureHandler : CBaseModuleHandler
    {
        public VisureHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.VisureCursor();
        }
    }
}