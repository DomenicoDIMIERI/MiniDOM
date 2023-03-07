
namespace minidom.Forms
{
    public class RichiesteCERQHandler : CBaseModuleHandler
    {
        public RichiesteCERQHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.RichiestaCERQCursor();
        }
    }
}