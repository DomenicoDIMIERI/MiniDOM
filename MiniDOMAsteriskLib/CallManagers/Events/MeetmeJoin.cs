
namespace minidom.CallManagers.Events
{
    public class MeetmeJoin : AsteriskEvent
    {
        public MeetmeJoin() : base("MeetmeJoin")
        {
        }

        public MeetmeJoin(string[] rows) : base(rows)
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