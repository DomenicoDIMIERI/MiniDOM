
namespace minidom
{
    public partial class Finanziaria
    {
        public class CProfiloXGroupAllowNegate : GroupAllowNegate<CProfilo>
        {
            public CProfiloXGroupAllowNegate()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Preventivatore";
            }

            public override string GetTableName()
            {
                return "tbl_PreventivatoriXGroup";
            }

            protected override string GetGroupFieldName()
            {
                return "Gruppo";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            protected internal new void SetItem(CProfilo item)
            {
                base.SetItem(item);
            }
        }
    }
}