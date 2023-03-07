
namespace minidom.CallManagers.Events
{
    public class QueueEntry : AsteriskEvent
    {
        public QueueEntry() : base("QueueEntry")
        {
        }

        public QueueEntry(string[] rows) : base(rows)
        {
        }

        public string Queue
        {
            get
            {
                return GetAttribute("Queue");
            }
        }

        public int Position
        {
            get
            {
                return GetAttribute("Position", 0);
            }
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string CallerID
        {
            get
            {
                return GetAttribute("CallerID");
            }
        }

        public string CallerIDName
        {
            get
            {
                return GetAttribute("CallerIDName");
            }
        }

        public int Wait
        {
            get
            {
                return GetAttribute("Wait", 0);
            }
        }
    }
}