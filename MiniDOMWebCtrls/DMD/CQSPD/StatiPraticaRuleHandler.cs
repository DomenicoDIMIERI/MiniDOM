
namespace minidom.Forms
{
    public class StatiPraticaRuleHandler : CBaseModuleHandler
    {
        public StatiPraticaRuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CStatoPratRuleCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return Finanziaria.StatiPratRules.GetItemById(id);
        }
    }
}