
namespace minidom.Forms
{
    public class OfficeMarcheArticoliHandler : CBaseModuleHandler
    {
        public OfficeMarcheArticoliHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MarcaArticoloCursor();
        }
    }
}