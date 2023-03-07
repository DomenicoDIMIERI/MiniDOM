using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class SessionEventArgs : EventArgs
        {
            [NonSerialized]
            private CSiteSession m_Session;

            public SessionEventArgs()
            {
                DMDObject.IncreaseCounter(this);
            }

            public SessionEventArgs(CSiteSession session) : this()
            {
                if (session is null)
                    throw new ArgumentNullException("session");
                m_Session = session;
            }

            public CSiteSession Session
            {
                get
                {
                    return m_Session;
                }
            }

            ~SessionEventArgs()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}