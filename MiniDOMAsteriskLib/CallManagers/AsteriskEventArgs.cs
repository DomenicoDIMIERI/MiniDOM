using System;

namespace minidom.CallManagers
{
    [Serializable]
    public class AsteriskEventArgs : EventArgs
    {
        [NonSerialized]
        private AsteriskCallManager m_Server;

        public AsteriskEventArgs()
        {
            DMDObject.IncreaseCounter(this);
            m_Server = null;
        }

        public AsteriskEventArgs(AsteriskCallManager server) : this()
        {
            if (server is null)
                throw new ArgumentNullException("server");
            m_Server = server;
        }

        public AsteriskCallManager Server
        {
            get
            {
                return m_Server;
            }
        }

        ~AsteriskEventArgs()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}