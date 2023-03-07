
namespace minidom.Forms
{
    public class ClasseDispositivoHandler : CBaseModuleHandler
    {
        public ClasseDispositivoHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.ClassiDispositivoCursor();
        }
    }
}