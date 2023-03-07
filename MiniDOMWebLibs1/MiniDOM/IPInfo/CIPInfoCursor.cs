
namespace minidom
{
    public partial class WebSite
    {
        public class CIPInfoCursor : Databases.DBObjectCursorBase<CIPInfo>
        {
            private Databases.CCursorFieldObj<string> m_IP = new Databases.CCursorFieldObj<string>("IP");
            private Databases.CCursorFieldObj<string> m_Descrizione = new Databases.CCursorFieldObj<string>("Descrizione");

            public CIPInfoCursor()
            {
            }

            public Databases.CCursorFieldObj<string> IP
            {
                get
                {
                    return m_IP;
                }
            }

            public Databases.CCursorFieldObj<string> Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            protected override Sistema.CModule GetModule()
            {
                return IPInfo.Module;
            }

            public override string GetTableName()
            {
                return "tbl_IPInfo";
            }
        }
    }
}