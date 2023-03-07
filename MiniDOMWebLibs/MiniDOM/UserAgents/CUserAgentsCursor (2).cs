
namespace minidom
{
    public partial class WebSite
    {
        public class CUserAgentsCursor : Databases.DBObjectCursorBase<CUserAgent>
        {
            private Databases.CCursorFieldObj<string> m_UserAgent = new Databases.CCursorFieldObj<string>("UserAgent");
            private Databases.CCursorFieldObj<string> m_Device = new Databases.CCursorFieldObj<string>("Device");
            private Databases.CCursorFieldObj<string> m_DeviceType = new Databases.CCursorFieldObj<string>("DeviceType");
            private Databases.CCursorField<int> m_Bits = new Databases.CCursorField<int>("Bits");
            private Databases.CCursorFieldObj<string> m_SistemaOperativo = new Databases.CCursorFieldObj<string>("SistemaOperativo");
            private Databases.CCursorFieldObj<string> m_VersioneSistemaOperativo = new Databases.CCursorFieldObj<string>("VersioneSistemaOperativo");
            private Databases.CCursorFieldObj<string> m_Browser = new Databases.CCursorFieldObj<string>("Browser");
            private Databases.CCursorFieldObj<string> m_VersioneBrowser = new Databases.CCursorFieldObj<string>("VersioneBrowser");

            public CUserAgentsCursor()
            {
            }

            public Databases.CCursorFieldObj<string> UserAgent
            {
                get
                {
                    return m_UserAgent;
                }
            }

            public Databases.CCursorFieldObj<string> Device
            {
                get
                {
                    return m_Device;
                }
            }

            public Databases.CCursorFieldObj<string> DeviceType
            {
                get
                {
                    return m_DeviceType;
                }
            }

            public Databases.CCursorField<int> Bits
            {
                get
                {
                    return m_Bits;
                }
            }

            public Databases.CCursorFieldObj<string> SistemaOperativo
            {
                get
                {
                    return m_SistemaOperativo;
                }
            }

            public Databases.CCursorFieldObj<string> VersioneSistemaOperativo
            {
                get
                {
                    return m_VersioneSistemaOperativo;
                }
            }

            public Databases.CCursorFieldObj<string> Browser
            {
                get
                {
                    return m_Browser;
                }
            }

            public Databases.CCursorFieldObj<string> VersioneBrowser
            {
                get
                {
                    return m_VersioneBrowser;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            protected override Sistema.CModule GetModule()
            {
                return UserAgents.Module;
            }

            public override string GetTableName()
            {
                return "tbl_UserAgents";
            }
        }
    }
}