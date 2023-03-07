using DMD;
using DMD.Databases.Collections;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {
        public class CCurrentSiteSession : CSiteSession
        {

            // Private m_Contents As CKeyCollection

            public CCurrentSiteSession()
            {
            }

            // Public ReadOnly Property Contents As CKeyCollection
            // Get
            // SyncLock Me
            // If (Me.m_Contents Is Nothing) Then Me.m_Contents = New CKeyCollection
            // Return Me.m_Contents
            // End SyncLock
            // End Get
            // End Property

            /// <summary>
            /// Notifica la fine della sessione corrente
            /// </summary>
            public void NotifyEnd()
            {
                EndTime = DMD.DateUtils.Now();
                if (Instance.Configuration.LogSessions)
                    Save(true);
                var e = new SessionEventArgs(this);
                Sessions.NotifySessionEnd(e);
            }

            /// <summary>
            /// Notifica la nuova sessione
            /// </summary>
            /// <param name="parameters"></param>
            public void NotifyStart(CKeyCollection parameters)
            {
                StartTime = DMD.DateUtils.Now();
                foreach (string k in parameters.Keys)
                    Parameters.SetItemByKey(k, parameters[k]);
                SessionID = DMD.Strings.CStr(parameters.GetItemByKey("SessionID")); // ASP_Session.SessionID
                RemoteIP = DMD.Strings.CStr(parameters.GetItemByKey("RemoteIP")); // WebSite.ASP_Request.ServerVariables("REMOTE_ADDR")
                RemotePort = Sistema.Formats.ToInteger(parameters.GetItemByKey("RemotePort")); // WebSite.ASP_Request.ServerVariables("REMOTE_PORT")
                UserAgent = DMD.Strings.CStr(parameters.GetItemByKey("RemoteUserAgent")); // WebSite.ASP_Request.ServerVariables("HTTP_USER_AGENT")
                Cookie = DMD.Strings.CStr(parameters.GetItemByKey("SiteCookie")); // Cookies.GetCookie("SiteCookie")
                InitialReferrer = DMD.Strings.CStr(parameters.GetItemByKey("InitialReferrer")); // CStr(ASP_Session("InitialReferrer"))
                if (Instance.Configuration.LogSessions)
                    Save(true);
                var e = new SessionEventArgs(this);
                Sessions.NotifySessionStart(e);
            }
        }
    }
}