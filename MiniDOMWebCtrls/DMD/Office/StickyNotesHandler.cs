
namespace minidom.Forms
{
    public class StickyNotesHandler : CBaseModuleHandler
    {
        public StickyNotesHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.StickyNotesCursor();
        }
    }
}