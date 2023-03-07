
namespace minidom.CallManagers.Events
{
    public class ParkedCall : AsteriskEvent
    {
        public ParkedCall() : base("ParkedCall")
        {
        }

        public ParkedCall(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Exten
        {
            get
            {
                return GetAttribute("Exten");
            }
        }

        public string From
        {
            get
            {
                return GetAttribute("From");
            }
        }

        public int Timeout
        {
            get
            {
                return GetAttribute("Timeout", 0);
            }
        }

        public string CallerID
        {
            get
            {
                return GetAttribute("CallerID");
            }
        }
    }
}