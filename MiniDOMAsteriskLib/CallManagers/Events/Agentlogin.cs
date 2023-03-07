
namespace minidom.CallManagers.Events
{
    public class Agentlogin : AsteriskEvent
    {
        public Agentlogin() : base("Agentlogin")
        {
        }

        public Agentlogin(string[] rows) : base(rows)
        {
        }

        public string Agent
        {
            get
            {
                return GetAttribute("Agent");
            }
        }

        public string UniqueID
        {
            get
            {
                return GetAttribute("Uniqueid");
            }
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }
    }
}