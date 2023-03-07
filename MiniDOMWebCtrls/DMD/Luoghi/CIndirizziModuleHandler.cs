
namespace minidom.Forms
{
    public class CIndirizziModuleHandler : CBaseModuleHandler
    {
        public CIndirizziModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CIndirizziCursor();
        }
    }
}