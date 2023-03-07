
namespace minidom.Forms
{




    // --------------------------------------------------------
    public class CPropPagesModuleHandler : CBaseModuleHandler
    {
        public CPropPagesModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.CRegisteredPropertyPageCursor();
        }
           
        public override object GetInternalItemById(int id)
        {
            return Sistema.PropertyPages.GetItemById(id);
        }
    }
}