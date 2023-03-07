
namespace minidom.Forms
{
    public class CliXCollabModuleHandler : CBaseModuleHandler
    {
        public CliXCollabModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.ClienteXCollaboratoreCursor();
        }
    }
}