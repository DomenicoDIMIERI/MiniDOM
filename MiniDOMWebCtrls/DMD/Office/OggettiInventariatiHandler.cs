
namespace minidom.Forms
{
    public class OggettiInventariatiHandler : CBaseModuleHandler
    {
        public OggettiInventariatiHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.OggettoInventariatoCursor();
        }
    }
}