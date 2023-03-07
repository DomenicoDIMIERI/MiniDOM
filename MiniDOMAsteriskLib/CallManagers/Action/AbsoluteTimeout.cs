using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// This command will request Asterisk to hangup a given channel after the specified number of seconds, thereby effectively ending the active call.
    /// If the channel is linked with another channel (an active connected call is in progress), the other channel will continue it's path through the dialplan (if any further steps remains).
    /// </summary>
    /// <remarks></remarks>
    public class AbsoluteTimeout : Action
    {
        private string m_Channel;
        private int m_Timeout;

        public AbsoluteTimeout() : base("AbsoluteTimeout")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <param name="channel">[in] Which channel to hangup, e.g. SIP/123-1c20</param>
        /// <param name="timeout">[in] The number of seconds until the channel should hangup</param>
        /// <remarks></remarks>
        public AbsoluteTimeout(string channel, int timeout) : this()
        {
            m_Channel = Strings.Trim(channel);
            m_Timeout = timeout;
        }

        /// <summary>
        /// Which channel to hangup, e.g. SIP/123-1c20
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Channel
        {
            get
            {
                return m_Channel;
            }
        }

        /// <summary>
        /// The number of seconds until the channel should hangup
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Timeout
        {
            get
            {
                return m_Timeout;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf + "Timeout: " + Timeout + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new AbsoluteTimeoutResponse(this);
        }
    }
}