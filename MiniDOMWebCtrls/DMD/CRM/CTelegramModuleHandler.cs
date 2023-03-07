
namespace minidom.Forms
{
    public class CTelegramModuleHandler : CBaseModuleHandler
    {
        public CTelegramModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.CTelegrammiCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return CustomerCalls.CRM.GetItemById(id);
        }
    }
}