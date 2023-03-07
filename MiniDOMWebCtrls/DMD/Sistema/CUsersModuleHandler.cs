
namespace minidom.Forms
{

    // Handler del module Utenti
    public class CUsersModuleHandler
        : CBaseModuleHandler
    {
        public CUsersModuleHandler()
        {
        }

        public CUsersModuleHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override object GetInternalItemById(int id)
        {
            // Return MyBase.GetItemById(id)
            return Sistema.Users.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Sistema.CUserCursor();
            cursor.Nominativo.SortOrder = Databases.SortEnum.SORT_ASC;
            return cursor;
        }
    }
}