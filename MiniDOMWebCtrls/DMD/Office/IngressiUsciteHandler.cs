
namespace minidom.Forms
{
    public class IngressiUsciteHandler : CBaseModuleHandler
    {
        public IngressiUsciteHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MarcatureIngressoUscitaCursor();
        }
    }
}