using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class LinkUserAllowNegate : UserAllowNegate<CCollegamento>
        {
            public LinkUserAllowNegate()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Collegamento";
            }

            protected override string GetUserFieldName()
            {
                return "Utente";
            }

            public override string GetTableName()
            {
                return "tbl_CollegamentiXUtente";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            public override Sistema.CModule GetModule()
            {
                return null;
            }
        }
    }
}