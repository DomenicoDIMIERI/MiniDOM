
namespace minidom.Forms
{

    // Handler del module Utenti
    public class CValoriRegistriModuleHandler : CBaseModuleHandler
    {
        public CValoriRegistriModuleHandler()
        {
        }

        public CValoriRegistriModuleHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override object GetInternalItemById(int id)
        {
            // Return MyBase.GetItemById(id)
            return Anagrafica.Postazioni.ValoriRegistri.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.ValoreRegistroCursor();
        }
    }
}