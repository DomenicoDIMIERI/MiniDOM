
namespace minidom.Forms
{
    public class OfferteDiLavoroHandler : CBaseModuleHandler
    {
        public OfferteDiLavoroHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.OffertaDiLavoroCursor();
        }
    }
}