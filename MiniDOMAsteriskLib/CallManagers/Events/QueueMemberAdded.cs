
namespace minidom.CallManagers.Events
{
    public class QueueMemberAdded : AsteriskEvent
    {
        public QueueMemberAdded() : base("QueueMemberAdded")
        {
        }

        public QueueMemberAdded(string[] rows) : base(rows)
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

        public string Membership
        {
            get
            {
                return GetAttribute("Membership");
            }
        }

        public int Penalty
        {
            get
            {
                return GetAttribute("Penalty", 0);
            }
        }

        public int CallsTaken
        {
            get
            {
                return GetAttribute("CallsTaken", 0);
            }
        }

        public int LastCall
        {
            get
            {
                return GetAttribute("LastCall", 0);
            }
        }

        public QueueStatusFlags Status
        {
            get
            {
                return (QueueStatusFlags)GetAttribute("Status", 0);
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