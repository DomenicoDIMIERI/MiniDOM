
namespace minidom.Forms
{
    public class CManutenzioniModuleHandler : CBaseModuleHandler
    {
        public CManutenzioniModuleHandler()
        {
        }

        public CManutenzioniModuleHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override object GetInternalItemById(int id)
        {
            // Return MyBase.GetItemById(id)
            return Anagrafica.Manutenzioni.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CManutenzioniCursor();
        }
    }
}