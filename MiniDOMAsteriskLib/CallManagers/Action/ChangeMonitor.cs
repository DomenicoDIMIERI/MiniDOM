using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// Changes the file name of a recording occuring on a channel
    /// </summary>
    /// <remarks></remarks>
    public class ChangeMonitor : Action
    {
        private string m_Channel;
        private string m_File;

        public ChangeMonitor() : base("ChangeMonitor")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <param name="channel">[in] Which channel to hangup, e.g. SIP/123-1c20</param>
        /// <param name="file">[in] File name</param>
        /// <remarks></remarks>
        public ChangeMonitor(string channel, string file) : this()
        {
            m_Channel = Strings.Trim(channel);
            m_File = Strings.Trim(file);
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
        /// File name
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string File
        {
            get
            {
                return m_File;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf + "File: " + File + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new ChangeMonitorResponse(this);
        }
    }
}