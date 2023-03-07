
namespace minidom.CallManagers.Events
{
    public class AgentCalledType : AsteriskEvent
    {
        public AgentCalledType() : base("AgentCalled")
        {
        }

        public AgentCalledType(string[] rows) : base(rows)
        {
        }

        public string AgentCalled
        {
            get
            {
                return GetAttribute("AgentCalled");
            }
        }

        public string ChannelCalling
        {
            get
            {
                return GetAttribute("ChannelCalling");
            }
        }

        public string CallerID
        {
            get
            {
                return GetAttribute("CallerID");
            }
        }

        public string Context
        {
            get
            {
                return GetAttribute("Context");
            }
        }

        public string Extension
        {
            get
            {
                return GetAttribute("Extension");
            }
        }

        public string Priority
        {
            get
            {
                return GetAttribute("Priority");
            }
        }
    }
}