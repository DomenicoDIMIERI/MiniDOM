
namespace minidom.Forms
{
    public class CCampagnaCRMModuleHandler : CBaseModuleHandler
    {
        public CCampagnaCRMModuleHandler()
        {
        }

        public override bool SupportsDuplicate
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsEdit
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsAnnotations
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsDelete
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsCreate
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsExport
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsImport
        {
            get
            {
                return false;
            }
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new CustomerCalls.CCampagnaCRMCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return CustomerCalls.CampagneCRM.GetItemById(id);
        }
    }
}