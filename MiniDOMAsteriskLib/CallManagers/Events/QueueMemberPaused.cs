
namespace minidom.CallManagers.Events
{
    public class QueueMemberPaused : AsteriskEvent
    {
        public QueueMemberPaused() : base("QueueMemberPaused")
        {
        }

        public QueueMemberPaused(string[] rows) : base(rows)
        {
        }

        public string Queue
        {
            get
            {
                return GetAttribute("Queue");
            }
        }

        public string Location
        {
            get
            {
                return GetAttribute("Location");
            }
        }

        public string MemberName
        {
            get
            {
                return GetAttribute("MemberName");
            }
        }

        public int Paused
        {
            get
            {
                return GetAttribute("Paused", 0);
            }
        }
    }
}