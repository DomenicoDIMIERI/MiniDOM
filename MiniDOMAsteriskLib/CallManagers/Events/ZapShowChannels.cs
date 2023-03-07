
namespace minidom.CallManagers.Events
{
    public class ZapShowChannels : AsteriskEvent
    {
        public ZapShowChannels() : base("ZapShowChannels")
        {
        }

        public ZapShowChannels(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Signalling
        {
            get
            {
                return GetAttribute("Signalling");
            }
        }

        public string Context
        {
            get
            {
                return GetAttribute("Context");
            }
        }

        public string DND
        {
            get
            {
                return GetAttribute("DND");
            }
        }

        public string Alarm
        {
            get
            {
                return GetAttribute("Alarm");
            }
        }
    }
}