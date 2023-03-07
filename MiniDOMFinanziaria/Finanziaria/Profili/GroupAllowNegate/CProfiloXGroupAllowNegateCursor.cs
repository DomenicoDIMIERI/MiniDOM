
namespace minidom
{
    public partial class Finanziaria
    {
        public class CProfiloXGroupAllowNegateCursor : GroupAllowNegateCursor<CProfilo>
        {
            public CProfiloXGroupAllowNegateCursor()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Preventivatore";
            }

            protected override string GetGroupFieldName()
            {
                return "Gruppo";
            }

            public override string GetTableName()
            {
                return "tbl_PreventivatoriXGroup";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProfiloXGroupAllowNegate();
            }
        }
    }
}