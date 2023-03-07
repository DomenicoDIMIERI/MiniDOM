
namespace minidom.CallManagers.Events
{
    public class Agentcallbacklogoff : AsteriskEvent
    {
        public Agentcallbacklogoff() : base("Agentcallbacklogoff")
        {
        }

        public Agentcallbacklogoff(string[] rows) : base(rows)
        {
        }

        public string Agent
        {
            get
            {
                return GetAttribute("Agent");
            }
        }

        public string Loginchan
        {
            get
            {
                return GetAttribute("Loginchan");
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

        public string Reason
        {
            get
            {
                return GetAttribute("Reason");
            }
        }
    }
}