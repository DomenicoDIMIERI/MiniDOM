using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// Redirect (transfer) a call
    /// </summary>
    /// <remarks></remarks>
    public class Redirect : AsyncAction
    {
        private string m_Channel;
        private string m_Exten;
        private string m_Context;
        private int m_Priority;
        private string m_ExtraChannel;
        private string m_ExtraExten;
        private string m_ExtraContext;
        private int? m_ExtraPriority;

        public Redirect() : base("Redirect")
        {
            m_Channel = "";
            m_Exten = "";
            m_Context = "";
            m_Priority = 1;
            m_ExtraChannel = "";
            m_ExtraExten = "";
            m_ExtraContext = "";
            m_ExtraPriority = default;
        }

        public Redirect(string channel, string exten, string context, int priority, string extraChannel) : this()
        {
            m_Channel = channel;
            m_ExtraChannel = extraChannel;
            m_Exten = exten;
            m_Context = context;
            m_Priority = priority;
        }

        public Redirect(string channel, string exten, string context, int priority, string extraChannel, string extraExten, string extraContext, int extraPriority) : this(channel, exten, context, priority, extraChannel)
        {
            m_ExtraExten = extraExten;
            m_ExtraContext = extraContext;
            m_ExtraPriority = extraPriority;
        }

        public string Channel
        {
            get
            {
                return m_Channel;
            }
        }

        public string Exten
        {
            get
            {
                return m_Exten;
            }
        }

        public string Context
        {
            get
            {
                return m_Context;
            }
        }

        public int Priority
        {
            get
            {
                return m_Priority;
            }
        }

        public string ExtraChannel
        {
            get
            {
                return m_ExtraChannel;
            }
        }

        public string ExtraExten
        {
            get
            {
                return m_ExtraExten;
            }
        }

        public string ExtraContext
        {
            get
            {
                return m_ExtraContext;
            }
        }

        public int? ExtraPriority
        {
            get
            {
                return m_ExtraPriority;
            }
        }

        protected override string GetCommandText()
        {
            string ret = base.GetCommandText();
            ret += "Channel: " + Channel + DMD.Strings.vbCrLf;
            ret += "ExtraChannel: " + ExtraChannel + DMD.Strings.vbCrLf;
            ret += "Exten: " + Exten + DMD.Strings.vbCrLf;
            ret += "Context: " + Context + DMD.Strings.vbCrLf;
            ret += "Priority: " + Priority + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(ExtraExten))
                ret += "ExtraExten: " + ExtraExten + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(ExtraContext))
                ret += "ExtraContext: " + ExtraContext + DMD.Strings.vbCrLf;
            if (ExtraPriority.HasValue)
                ret += "ExtraPriority: " + ExtraPriority + DMD.Strings.vbCrLf;
            return ret;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new RedirectResponse(this);
        }
    }
}