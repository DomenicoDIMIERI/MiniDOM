
namespace minidom.CallManagers.Events
{
    public class Agentcallbacklogin : AsteriskEvent
    {
        public Agentcallbacklogin() : base("Agentcallbacklogin")
        {
        }

        public Agentcallbacklogin(string[] rows) : base(rows)
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
    }
}