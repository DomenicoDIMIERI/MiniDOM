
namespace minidom.CallManagers.Events
{
    public class AgentDump : AsteriskEvent
    {
        public AgentDump() : base("AgentDump")
        {
        }

        public AgentDump(string[] rows) : base(rows)
        {
        }

        public string Queue
        {
            get
            {
                return GetAttribute("Queue");
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

        public string Member
        {
            get
            {
                return GetAttribute("Member");
            }
        }

        public string MemberName
        {
            get
            {
                return GetAttribute("MemberName");
            }
        }
    }
}