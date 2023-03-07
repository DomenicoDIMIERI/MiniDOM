
namespace minidom.CallManagers.Events
{
    public class Unlink : AsteriskEvent
    {
        public Unlink() : base("Unlink")
        {
        }

        public Unlink(string[] rows) : base(rows)
        {
        }

        public string Channel1
        {
            get
            {
                return GetAttribute("Channel1");
            }
        }

        public string Channel2
        {
            get
            {
                return GetAttribute("Channel2");
            }
        }

        public string UniqueID1
        {
            get
            {
                return GetAttribute("UniqueID1");
            }
        }

        public string UniqueID2
        {
            get
            {
                return GetAttribute("UniqueID2");
            }
        }

        public string CallerID1
        {
            get
            {
                return GetAttribute("CallerID1");
            }
        }

        public string CallerID2
        {
            get
            {
                return GetAttribute("CallerID2");
            }
        }
    }
}