
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// This requires the T option on the meetme application
    /// </summary>
    /// <remarks></remarks>
    public class MeetmeStopTalking : AsteriskEvent
    {
        public MeetmeStopTalking() : base("MeetmeStopTalking")
        {
        }

        public MeetmeStopTalking(string[] rows) : base(rows)
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