
namespace minidom.Forms
{
    public class CurriculaHandler : CBaseModuleHandler
    {
        public CurriculaHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.CurriculumCursor();
        }
    }
}