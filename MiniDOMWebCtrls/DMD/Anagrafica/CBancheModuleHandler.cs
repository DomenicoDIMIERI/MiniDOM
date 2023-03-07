
namespace minidom.Forms
{
    public class CBancheModuleHandler : CBaseModuleHandler
    {
        public CBancheModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CBancheCursor();
        }
    }
}