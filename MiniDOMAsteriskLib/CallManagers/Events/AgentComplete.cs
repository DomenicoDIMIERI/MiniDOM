
namespace minidom.CallManagers.Events
{
    public class AgentComplete : AsteriskEvent
    {
        public AgentComplete() : base("AgentComplete")
        {
        }

        public AgentComplete(string[] rows) : base(rows)
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

        public string HoldTime
        {
            get
            {
                return GetAttribute("HoldTime");
            }
        }

        public string TalkTime
        {
            get
            {
                return GetAttribute("TalkTime");
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