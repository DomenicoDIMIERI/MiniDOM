using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CProfiloXUserAllowNegate : UserAllowNegate<CProfilo>
        {
            public CProfiloXUserAllowNegate()
            {
            }

            protected override string GetItemFieldName()
            {
                return "Preventivatore";
            }

            protected override string GetUserFieldName()
            {
                return "Utente";
            }

            public override string GetTableName()
            {
                return "tbl_PreventivatoriXUser";
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