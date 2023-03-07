
namespace minidom
{
    public partial class WebSite
    {
        public class CAllowedRemoteIPsCursor : Databases.DBObjectCursor<CAllowedRemoteIPs>
        {
            private Databases.CCursorFieldObj<string> m_Name;
            private Databases.CCursorFieldObj<string> m_RemoteIP;
            private Databases.CCursorField<bool> m_Negate;

            public CAllowedRemoteIPsCursor()
            {
                m_Name = new Databases.CCursorFieldObj<string>("Name");
                m_RemoteIP = new Databases.CCursorFieldObj<string>("RemoteIP");
                m_Negate = new Databases.CCursorField<bool>("Negate");
            }

            public Databases.CCursorFieldObj<string> Name
            {
                get
                {
                    return m_Name;
                }
            }

            public Databases.CCursorFieldObj<string> RemoteIP
            {
                get
                {
                    return m_RemoteIP;
                }
            }

            public Databases.CCursorField<bool> Negate
            {
                get
                {
                    return m_Negate;
                }
            }

            public override object InstantiateNew(Databases.DBReader dbRis)
            {
                return new CAllowedRemoteIPs();
            }

            public override string GetTableName()
            {
                return "tbl_Pratiche_Allow";
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}