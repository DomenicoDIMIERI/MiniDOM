
namespace minidom.CallManagers.Events
{
    public class Agentlogoff : AsteriskEvent
    {
        public Agentlogoff() : base("Agentlogoff")
        {
        }

        public Agentlogoff(string[] rows) : base(rows)
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

        public string LoginTime
        {
            get
            {
                return GetAttribute("Logintime");
            }
        }
    }
}