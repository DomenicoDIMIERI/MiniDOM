
namespace minidom.Forms
{
    public class CQSPDRichiesteApprovazioneHandler : CBaseModuleHandler
    {
        public CQSPDRichiesteApprovazioneHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CRichiestaApprovazioneCursor();
        }
    }
}