
namespace minidom.CallManagers.Events
{
    public class DNDState : AsteriskEvent
    {
        public DNDState() : base("DNDState")
        {
        }

        public DNDState(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Status
        {
            get
            {
                return GetAttribute("Status");
            }
        }
    }
}