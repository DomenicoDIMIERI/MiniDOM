
namespace minidom.Forms
{
    public class TipiContrattoModuleHandler : CBaseModuleHandler
    {
        public TipiContrattoModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CTipoContrattoCursor();
        }
    }
}