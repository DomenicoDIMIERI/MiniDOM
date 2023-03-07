
namespace minidom.Forms
{

    // -------------------------------------------------------
    public class CCalendarProcsHandler : CBaseModuleHandler
    {
        public CCalendarProcsHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Sistema.Procedure.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.CProcedureCursor();
        }
    }
}