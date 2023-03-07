
namespace minidom.Forms
{
    public class CollaboratoriModuleHandler : CBaseModuleHandler
    {
        public CollaboratoriModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.CCollaboratoriCursor();
            return cursor;
        }
    }
}