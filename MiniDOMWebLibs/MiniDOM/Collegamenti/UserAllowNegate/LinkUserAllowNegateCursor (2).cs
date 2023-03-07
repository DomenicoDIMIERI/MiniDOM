
namespace minidom
{
    public partial class WebSite
    {
        public class LinkUserAllowNegateCursor : UserAllowNegateCursor<CCollegamento>
        {
            public LinkUserAllowNegateCursor()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Collegamento";
            }

            public override string GetTableName()
            {
                return "tbl_CollegamentiXUtente";
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
                return new LinkUserAllowNegate();
            }
        }
    }
}