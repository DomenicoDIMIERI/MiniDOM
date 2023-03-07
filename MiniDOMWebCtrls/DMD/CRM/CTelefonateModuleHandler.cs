
namespace minidom.Forms
{
    public class CTelefonateModuleHandler : CBaseModuleHandler
    {
        public CTelefonateModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.CTelefonateCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return CustomerCalls.CRM.GetItemById(id);
        }
    }
}