
namespace minidom.Forms
{
    public class StatiLavPraticaHandler : CBaseModuleHandler
    {
        public StatiLavPraticaHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CStatiLavorazionePraticaCursor();
        }
    }
}