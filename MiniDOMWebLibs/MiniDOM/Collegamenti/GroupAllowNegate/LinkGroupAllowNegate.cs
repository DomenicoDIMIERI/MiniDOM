using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class LinkGroupAllowNegate 
            : GroupAllowNegate<CCollegamento>
        {
            public LinkGroupAllowNegate()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Collegamento";
            }

            public override string GetTableName()
            {
                return "tbl_CollegamentiXGruppo";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }
        }
    }
}