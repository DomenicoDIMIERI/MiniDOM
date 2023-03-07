
namespace minidom.CallManagers.Events
{
    public class Newchannel : AsteriskEvent
    {
        public Newchannel() : base("Newchannel")
        {
        }

        public Newchannel(string[] rows) : base(rows)
        {
        }

        public string State
        {
            get
            {
                return GetAttribute("State");
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

        public string CallerIDNum
        {
            get
            {
                return GetAttribute("CallerIDNum");
            }
        }

        public string CallerIDName
        {
            get
            {
                return GetAttribute("CallerIDName");
            }
        }

        public string UniqueID
        {
            get
            {
                return GetAttribute("UniqueID");
            }
        }
    }
}