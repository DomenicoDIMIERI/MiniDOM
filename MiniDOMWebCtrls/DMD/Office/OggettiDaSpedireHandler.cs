
namespace minidom.Forms
{
    public class OggettiDaSpedireHandler : CBaseModuleHandler
    {
        public OggettiDaSpedireHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.OggettiDaSpedireCursor();
        }
    }
}