
namespace minidom.CallManagers.Events
{
    public class Cdr : AsteriskEvent
    {
        public Cdr() : base("Cdr")
        {
        }

        public Cdr(string[] rows) : base(rows)
        {
        }

        public string AccountCode
        {
            get
            {
                return GetAttribute("AccountCode");
            }
        }

        public string Source
        {
            get
            {
                return GetAttribute("Source");
            }
        }

        public string Destination
        {
            get
            {
                return GetAttribute("Destination");
            }
        }

        public string DestinationContext
        {
            get
            {
                return GetAttribute("DestinationContext");
            }
        }

        public string CallerID
        {
            get
            {
                return GetAttribute("CallerID");
            }
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string DestinationChannel
        {
            get
            {
                return GetAttribute("DestinationChannel");
            }
        }

        public string LastApplication
        {
            get
            {
                return GetAttribute("LastApplication");
            }
        }

        public string LastData
        {
            get
            {
                return GetAttribute("LastData");
            }
        }

        public string StartTime
        {
            get
            {
                return GetAttribute("StartTime");
            }
        }

        public string AnswerTime
        {
            get
            {
                return GetAttribute("AnswerTime");
            }
        }

        public string EndTime
        {
            get
            {
                return GetAttribute("EndTime");
            }
        }

        public string Duration
        {
            get
            {
                return GetAttribute("Duration");
            }
        }

        public string BillableSeconds
        {
            get
            {
                return GetAttribute("BillableSeconds");
            }
        }

        public string Disposition
        {
            get
            {
                return GetAttribute("Disposition");
            }
        }

        public string AMAFlags
        {
            get
            {
                return GetAttribute("AMAFlags");
            }
        }

        public string UniqueID
        {
            get
            {
                return GetAttribute("UniqueID");
            }
        }

        public string UserField
        {
            get
            {
                return GetAttribute("UserField");
            }
        }
    }
}