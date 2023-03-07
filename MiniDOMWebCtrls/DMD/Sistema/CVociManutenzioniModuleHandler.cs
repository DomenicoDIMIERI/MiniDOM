
namespace minidom.Forms
{
    public class CVociManutenzioniModuleHandler : CBaseModuleHandler
    {
        public CVociManutenzioniModuleHandler()
        {
        }

        public CVociManutenzioniModuleHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override object GetInternalItemById(int id)
        {
            // Return MyBase.GetItemById(id)
            return Anagrafica.Manutenzioni.Voci.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.VociManutenzioneCursor();
        }
    }
}