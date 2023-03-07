
namespace minidom.CallManagers.Events
{
    public class Newcallerid : AsteriskEvent
    {
        public Newcallerid() : base("Newcallerid")
        {
        }

        public Newcallerid(string[] rows) : base(rows)
        {
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

        public string UniqueID
        {
            get
            {
                return GetAttribute("UniqueID");
            }
        }

        public string CallerIDName
        {
            get
            {
                return GetAttribute("CallerIDName");
            }
        }

        public string CIDCallingPres
        {
            get
            {
                return GetAttribute("CID-CallingPres");
            }
        }
    }
}