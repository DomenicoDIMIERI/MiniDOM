
namespace minidom
{
    public partial class WebSite
    {
        public class LinkGroupAllowNegateCursor : GroupAllowNegateCursor<CCollegamento>
        {
            public LinkGroupAllowNegateCursor()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Collegamento";
            }

            protected override string GetGroupFieldName()
            {
                return "Gruppo";
            }

            public override string GetTableName()
            {
                return "tbl_CollegamentiXGruppo";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public override object InstantiateNew(Databases.DBReader dbRis)
            {
                return new LinkGroupAllowNegate();
            }
        }
    }
}