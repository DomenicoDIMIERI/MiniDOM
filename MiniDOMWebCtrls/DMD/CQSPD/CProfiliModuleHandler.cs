
namespace minidom.Forms
{
    public class CProfiliModuleHandler : CBaseModuleHandler
    {
        public CProfiliModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CProfiliCursor();
        }
    }
}