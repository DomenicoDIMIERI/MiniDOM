
namespace minidom.Forms
{
    public class CAnnotazioniModuleHandler : CBaseModuleHandler
    {
        public CAnnotazioniModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SExport | ModuleSupportFlags.SImport)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.CAnnotazioniCursor();
        }
    }
}