
namespace minidom.Forms
{
    public class TasksLavorazioneHandler : CBaseModuleHandler
    {
        public TasksLavorazioneHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.TaskLavorazioneCursor();
        }
    }
}