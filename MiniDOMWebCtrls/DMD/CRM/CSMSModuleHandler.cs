
namespace minidom.Forms
{
    public class CSMSModuleHandler : CBaseModuleHandler
    {
        public CSMSModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.SMSMessageCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return CustomerCalls.CRM.GetItemById(id);
        }
    }
}