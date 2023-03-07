
namespace minidom.Forms
{
    public class AssicurazioniModuleHandler : CBaseModuleHandler
    {
        public AssicurazioniModuleHandler() : base(Finanziaria.Assicurazioni.Module, ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CAssicurazioniCursor();
        }
    }
}