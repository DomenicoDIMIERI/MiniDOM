
namespace minidom.Forms
{

    // Handler del module Utenti
    public class CPostazioniModuleHandler : CBaseModuleHandler
    {
        public CPostazioniModuleHandler()
        {
        }

        public CPostazioniModuleHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CPostazioniCursor();
        }
    }
}