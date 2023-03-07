using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class Monitor : Action
    {
        private string m_Channel;
        private string m_File;
        private string m_Format;
        private int m_Mix;

        public Monitor() : base("Monitor")
        {
        }

        public Monitor(string channel, string file, string format, int mix) : this()
        {
            m_Channel = channel;
            m_File = file;
            m_Format = format;
            m_Mix = mix;
        }

        public Monitor(string channel, string file, int mix) : this()
        {
            m_Channel = channel;
            m_File = file;
            m_Format = "";
            m_Mix = mix;
        }

        public string Channel
        {
            get
            {
                return m_Channel;
            }
        }

        public string File
        {
            get
            {
                return m_File;
            }
        }

        public string Format
        {
            get
            {
                return m_Format;
            }
        }

        public int Mix
        {
            get
            {
                return m_Mix;
            }
        }

        protected override string GetCommandText()
        {
            if (string.IsNullOrEmpty(m_Format))
            {
                return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf + "File: " + File + DMD.Strings.vbCrLf + "Mix: " + Mix;
            }
            else
            {
                return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf + "File: " + File + DMD.Strings.vbCrLf + "Format: " + Format + DMD.Strings.vbCrLf + "Mix: " + Mix;
            }
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new MonitorResponse(this);
        }
    }
}