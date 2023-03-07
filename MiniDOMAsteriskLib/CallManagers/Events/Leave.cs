
namespace minidom.CallManagers.Events
{
    public class Leave : AsteriskEvent
    {
        public Leave() : base("Leave")
        {
        }

        public Leave(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Queue
        {
            get
            {
                return GetAttribute("Queue");
            }
        }

        public int Count
        {
            get
            {
                return GetAttribute("Count", 0);
            }
        }
    }
}