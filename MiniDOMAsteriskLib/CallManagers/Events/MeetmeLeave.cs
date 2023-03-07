
namespace minidom.CallManagers.Events
{
    public class MeetmeLeave : AsteriskEvent
    {
        public MeetmeLeave() : base("MeetmeLeave")
        {
        }

        public MeetmeLeave(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Meetme
        {
            get
            {
                return GetAttribute("Meetme");
            }
        }

        public string Usernum
        {
            get
            {
                return GetAttribute("Usernum");
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