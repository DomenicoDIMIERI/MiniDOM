
namespace minidom
{
    public partial class Finanziaria
    {
        public class CProfiloXUserAllowNegateCursor : UserAllowNegateCursor<CProfilo>
        {
            public CProfiloXUserAllowNegateCursor()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Preventivatore";
            }

            public override string GetTableName()
            {
                return "tbl_PreventivatoriXUser";
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
                return new CProfiloXUserAllowNegate();
            }
        }
    }
}