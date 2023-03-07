
namespace minidom.Forms
{
    public class TemplatesModuleHandler : CBaseModuleHandler
    {
        public TemplatesModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.CContattoUtenteTemplateCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return CustomerCalls.Templates.GetItemById(id);
        }

        // Public Overrides Function GetItemByName() As String
        // Return CustomerCalls.Templates.GetItemById(id)
        // End Function

    }
}