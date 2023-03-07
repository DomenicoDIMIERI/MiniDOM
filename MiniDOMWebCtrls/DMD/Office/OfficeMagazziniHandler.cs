
namespace minidom.Forms
{
    public class OfficeMagazziniHandler : CBaseModuleHandler
    {
        public OfficeMagazziniHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MagazzinoCursor();
        }
    }
}