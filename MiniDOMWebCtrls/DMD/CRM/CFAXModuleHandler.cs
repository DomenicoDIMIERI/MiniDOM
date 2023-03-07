
namespace minidom.Forms
{
    public class CFAXModuleHandler : CBaseModuleHandler
    {
        public CFAXModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.FaxDocumentsCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return CustomerCalls.CRM.GetItemById(id);
        }
    }
}