
namespace minidom.Forms
{
    public class MotiviRichiesteHandler : CBaseModuleHandler
    {
        public MotiviRichiesteHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MotivoRichiestaCursor();
        }
    }
}