using System;

namespace minidom
{
    public partial class WebSite
    {
        public class VisitedPagesCursor : Databases.DBObjectCursorBase<VisitedPage>
        {
            private Databases.CCursorField<int> m_SessionID = new Databases.CCursorField<int>("Session");
            private Databases.CCursorField<int> m_UserID = new Databases.CCursorField<int>("UserID");
            private Databases.CCursorFieldObj<string> m_UserName = new Databases.CCursorFieldObj<string>("UserName");
            private Databases.CCursorField<DateTime> m_Data = new Databases.CCursorField<DateTime>("Data");
            private Databases.CCursorField<bool> m_Secure = new Databases.CCursorField<bool>("Secure");
            private Databases.CCursorFieldObj<string> m_Protocol = new Databases.CCursorFieldObj<string>("Protocol");
            private Databases.CCursorFieldObj<string> m_SiteName = new Databases.CCursorFieldObj<string>("SiteName");
            private Databases.CCursorFieldObj<string> m_PageName = new Databases.CCursorFieldObj<string>("ScriptName");
            private Databases.CCursorFieldObj<string> m_QueryString = new Databases.CCursorFieldObj<string>("QueryString");
            private Databases.CCursorFieldObj<string> m_PostedData = new Databases.CCursorFieldObj<string>("PostedData");
            private Databases.CCursorFieldObj<string> m_Referrer = new Databases.CCursorFieldObj<string>("Referrer");
            private Databases.CCursorFieldObj<string> m_StatusDescription = new Databases.CCursorFieldObj<string>("PageStatus");
            private Databases.CCursorFieldObj<string> m_StatusCode = new Databases.CCursorFieldObj<string>("PageStatusCode");
            private Databases.CCursorFieldObj<string> m_IDAnnuncio = new Databases.CCursorFieldObj<string>("IDAnnunction");
            private Databases.CCursorFieldObj<string> m_ReferrerDomain = new Databases.CCursorFieldObj<string>("ReferrerDomain");

            public VisitedPagesCursor()
            {
            }

            public Databases.CCursorFieldObj<string> ReferrerDomain
            {
                get
                {
                    return m_ReferrerDomain;
                }
            }

            public Databases.CCursorFieldObj<string> IDAnnuncio
            {
                get
                {
                    return m_IDAnnuncio;
                }
            }

            public Databases.CCursorField<int> SessionID
            {
                get
                {
                    return m_SessionID;
                }
            }

            public Databases.CCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
                }
            }

            public Databases.CCursorFieldObj<string> UserName
            {
                get
                {
                    return m_UserName;
                }
            }

            public Databases.CCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            public Databases.CCursorField<bool> Secure
            {
                get
                {
                    return m_Secure;
                }
            }

            public Databases.CCursorFieldObj<string> Protocol
            {
                get
                {
                    return m_Protocol;
                }
            }

            public Databases.CCursorFieldObj<string> SiteName
            {
                get
                {
                    return m_SiteName;
                }
            }

            public Databases.CCursorFieldObj<string> PageName
            {
                get
                {
                    return m_PageName;
                }
            }

            public Databases.CCursorFieldObj<string> QueryString
            {
                get
                {
                    return m_QueryString;
                }
            }

            public Databases.CCursorFieldObj<string> PostedData
            {
                get
                {
                    return m_PostedData;
                }
            }

            public Databases.CCursorFieldObj<string> Referrer
            {
                get
                {
                    return m_Referrer;
                }
            }

            public Databases.CCursorFieldObj<string> StatusDescription
            {
                get
                {
                    return m_StatusDescription;
                }
            }

            public Databases.CCursorFieldObj<string> StatusCode
            {
                get
                {
                    return m_StatusCode;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            protected override Sistema.CModule GetModule()
            {
                return VisitedPages.Module;
            }

            public override string GetTableName()
            {
                return "tbl_VisitedPages";
            }
        }
    }
}