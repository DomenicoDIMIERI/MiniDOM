
namespace minidom.Forms
{
    public class ADVResModuleHandler : CBaseModuleHandler
    {
        public ADVResModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new ADV.CRisultatoCampagnaCursor();
        }
    }
}