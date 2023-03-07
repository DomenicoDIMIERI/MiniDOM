
namespace minidom.Forms
{
    public class CGroupsModuleHandler : CBaseModuleHandler
    {
        public CGroupsModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Sistema.CGroupCursor();
            cursor.GroupName.SortOrder = Databases.SortEnum.SORT_ASC;
            return cursor;
        }
    }
}