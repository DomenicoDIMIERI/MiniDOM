
namespace minidom.CallManagers.Events
{
    public class Join : AsteriskEvent
    {
        public Join() : base("Join")
        {
        }

        public Join(string[] rows) : base(rows)
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

        public int Position
        {
            get
            {
                return GetAttribute("Position", 0);
            }
        }

        public int Count
        {
            get
            {
                return GetAttribute("Count", 0);
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