
namespace minidom.Forms
{
    public class RichiesteFinanziamentoModuleHandler : CBaseModuleHandler
    {
        public RichiesteFinanziamentoModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CRichiesteFinanziamentoCursor();
        }
    }
}