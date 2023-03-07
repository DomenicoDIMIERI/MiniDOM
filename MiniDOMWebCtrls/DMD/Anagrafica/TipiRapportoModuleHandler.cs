
namespace minidom.Forms
{
    public class TipiRapportoModuleHandler : CBaseModuleHandler
    {
        public TipiRapportoModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CTipoRapportoCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return Anagrafica.TipiRapporto.GetItemById(id);
        }
    }
}