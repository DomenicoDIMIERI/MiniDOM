using System;
using DMD;
using DMD.XML;
using minidom;
using minidom.Internals;
using static minidom.Sistema;

namespace minidom
{
    namespace Internals
    {

        [Serializable]
        public sealed class CSessionsClass 
            : Sistema.CModulesClass<WebSite.CSiteSession>
        {

            /// <summary>
        /// Evento generato quando viene iniziata una nuova sessione remota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            public event SessionStartEventHandler SessionStart;

            public delegate void SessionStartEventHandler(object sender, WebSite.SessionEventArgs e);


            /// <summary>
            /// Evento generato quando viene terminata una sessione remota
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public event SessionEndEventHandler SessionEnd;

            public delegate void SessionEndEventHandler(object sender, WebSite.SessionEventArgs e);

            private int m_ActiveSessions = 0;

            internal CSessionsClass() : base("modSiteSessions", typeof(WebSite.CSiteSessionsCursor), 0)
            {
            }

            public int CountActiveSessions()
            {
                return m_ActiveSessions;
            }

            public void NotifySessionStart(WebSite.SessionEventArgs e)
            {
                m_ActiveSessions += 1;
                SessionStart?.Invoke(null, e);
            }

            public void NotifySessionEnd(WebSite.SessionEventArgs e)
            {
                m_ActiveSessions -= 1;
                SessionEnd?.Invoke(null, e);
            }
        }
    }

    public partial class WebSite
    {
        private static CSessionsClass m_Sessions = null;

        public static CSessionsClass Sessions
        {
            get
            {
                if (m_Sessions is null)
                    m_Sessions = new CSessionsClass();
                return m_Sessions;
            }
        }
    }
}