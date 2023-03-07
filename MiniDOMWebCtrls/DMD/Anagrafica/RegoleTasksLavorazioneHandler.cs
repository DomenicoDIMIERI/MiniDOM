
namespace minidom.Forms
{
    public class RegoleTasksLavorazioneHandler : CBaseModuleHandler
    {
        public RegoleTasksLavorazioneHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.RegolaTaskLavorazioneCursor();
        }
    }
}