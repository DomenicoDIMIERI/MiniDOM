
namespace minidom.Forms
{
    public class MotiviContattoModuleHandler : CBaseModuleHandler
    {
        public MotiviContattoModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.MotiviContattoCursor();
        }
    }
}