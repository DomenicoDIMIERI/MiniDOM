
namespace minidom.Forms
{



    // ----------------------
    public class CModulePraticheCQSPDHandler : CBaseModuleHandler
    {
        public CModulePraticheCQSPDHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public CModulePraticheCQSPDHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}