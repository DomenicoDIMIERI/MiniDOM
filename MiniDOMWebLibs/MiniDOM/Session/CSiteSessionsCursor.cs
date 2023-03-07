using System;

namespace minidom
{
    public partial class WebSite
    {
        public class CSiteSessionsCursor : Databases.DBObjectCursorBase<CSiteSession>
        {
            private Databases.CCursorFieldObj<string> m_SessionID = new Databases.CCursorFieldObj<string>("SessionID");
            private Databases.CCursorField<DateTime> m_StartTime = new Databases.CCursorField<DateTime>("StartTime");
            private Databases.CCursorField<DateTime> m_EndTime = new Databases.CCursorField<DateTime>("EndTime");
            private Databases.CCursorFieldObj<string> m_RemoteIP = new Databases.CCursorFieldObj<string>("RemoteIP");
            private Databases.CCursorField<int> m_RemotePort = new Databases.CCursorField<int>("RemotePort");
            private Databases.CCursorFieldObj<string> m_UserAgent = new Databases.CCursorFieldObj<string>("UserAgent");
            private Databases.CCursorFieldObj<string> m_Cookie = new Databases.CCursorFieldObj<string>("Cookie");
            private Databases.CCursorFieldObj<string> m_InitialReferrer = new Databases.CCursorFieldObj<string>("InitialReferrer");

            public CSiteSessionsCursor()
            {
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            protected override Sistema.CModule GetModule()
            {
                return Sessions.Module;
            }

            public override string GetTableName()
            {
                return "tbl_SiteSessions";
            }

            public Databases.CCursorFieldObj<string> SessionID
            {
                get
                {
                    return m_SessionID;
                }
            }

            public Databases.CCursorField<DateTime> StartTime
            {
                get
                {
                    return m_StartTime;
                }
            }

            public Databases.CCursorField<DateTime> EndTime
            {
                get
                {
                    return m_EndTime;
                }
            }

            public Databases.CCursorFieldObj<string> RemoteIP
            {
                get
                {
                    return m_RemoteIP;
                }
            }

            public Databases.CCursorField<int> RemotePort
            {
                get
                {
                    return m_RemotePort;
                }
            }

            public Databases.CCursorFieldObj<string> UserAgent
            {
                get
                {
                    return m_UserAgent;
                }
            }

            public Databases.CCursorFieldObj<string> Cookie
            {
                get
                {
                    return m_Cookie;
                }
            }

            public Databases.CCursorFieldObj<string> InitialReferrer
            {
                get
                {
                    return m_InitialReferrer;
                }
            }
        }
    }
}