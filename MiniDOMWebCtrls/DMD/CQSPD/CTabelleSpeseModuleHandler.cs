
namespace minidom.Forms
{
    public class CTabelleSpeseModuleHandler : CBaseModuleHandler
    {
        public CTabelleSpeseModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CTabellaSpeseCursor();
        }
    }
}