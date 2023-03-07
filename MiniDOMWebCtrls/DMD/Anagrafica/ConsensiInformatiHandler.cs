
namespace minidom.Forms
{
    public class ConsensiInformatiHandler : CBaseModuleHandler
    {
        public ConsensiInformatiHandler() : base()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.ConsensoInformatoCursor();
        }
    }
}