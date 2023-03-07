
namespace minidom.Forms
{
    public class StatiTasksLavorazioneHandler : CBaseModuleHandler
    {
        public StatiTasksLavorazioneHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.StatoTaskLavorazioneCursor();
        }
    }
}