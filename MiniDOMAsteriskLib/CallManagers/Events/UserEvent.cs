
namespace minidom.CallManagers.Events
{
    public class UserEvent : AsteriskEvent
    {
        public UserEvent() : base("UserEvent")
        {
        }

        public UserEvent(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Uniqueid
        {
            get
            {
                return GetAttribute("Uniqueid");
            }
        }
    }
}