
namespace minidom.CallManagers.Events
{
    public enum QueueStatusFlags : int
    {
        NotInUse = 1,
        InUse = 2,
        Busy = 3,
        FLAG4 = 4,
        Unavailable = 5,
        Ringing = 6
    }

    public class QueueMember : AsteriskEvent
    {
        public QueueMember() : base("QueueMember")
        {
        }

        public QueueMember(string[] rows) : base(rows)
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