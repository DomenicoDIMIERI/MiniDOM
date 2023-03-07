
namespace minidom.CallManagers.Events
{
    public class Newstate : AsteriskEvent
    {
        public Newstate() : base("Newstate")
        {
        }

        public Newstate(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string State
        {
            get
            {
                return GetAttribute("State");
            }
        }

        public string CallerID
        {
            get
            {
                return GetAttribute("Callerid");
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