
namespace minidom.Forms
{
    public class VerificheAmministrativeHandler : CBaseModuleHandler
    {
        public VerificheAmministrativeHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.VerificheAmministrativeCursor();
        }
    }
}