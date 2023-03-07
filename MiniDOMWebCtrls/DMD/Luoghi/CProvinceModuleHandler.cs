
namespace minidom.Forms
{
    public class CProvinceModuleHandler : CBaseModuleHandler
    {
        public CProvinceModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CProvinceCursor();
            ret.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            return ret;
        }
    }
}