
namespace minidom.Forms
{
    public class CandidatureHandler : CBaseModuleHandler
    {
        public CandidatureHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.CandidaturaCursor();
        }
    }
}