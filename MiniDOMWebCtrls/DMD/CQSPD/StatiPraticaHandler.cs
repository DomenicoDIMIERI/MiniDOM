
namespace minidom.Forms
{
    public class StatiPraticaHandler : CBaseModuleHandler
    {
        public StatiPraticaHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CStatoPraticaCursor();
        }
    }
}