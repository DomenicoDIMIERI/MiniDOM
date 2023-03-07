
namespace minidom.CallManagers.Events
{
    public class MeetmeTalking : AsteriskEvent
    {
        public MeetmeTalking() : base("MeetmeTalking")
        {
        }

        public MeetmeTalking(string[] rows) : base(rows)
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