
namespace minidom.Forms
{
    public class MotiviAppuntamentoModuleHandler : CBaseModuleHandler
    {
        public MotiviAppuntamentoModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.MotiviAppuntamentoCursor();
        }
    }
}