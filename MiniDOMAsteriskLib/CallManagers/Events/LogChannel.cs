
namespace minidom.CallManagers.Events
{
    public class LogChannel : AsteriskEvent
    {
        public LogChannel() : base("LogChannel")
        {
        }

        public LogChannel(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Enabled
        {
            get
            {
                return GetAttribute("Enabled");
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