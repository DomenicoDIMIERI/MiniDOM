
namespace minidom.CallManagers.Events
{
    public class OriginateResponse : AsteriskEvent
    {
        public OriginateResponse() : base("OriginateResponse")
        {
        }

        public OriginateResponse(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Context
        {
            get
            {
                return GetAttribute("Context");
            }
        }

        public string Exten
        {
            get
            {
                return GetAttribute("Exten");
            }
        }

        public string Reason
        {
            get
            {
                return GetAttribute("Reason");
            }
        }

        public string UniqueID
        {
            get
            {
                return GetAttribute("Uniqueid");
            }
        }

        public string CallerIDNum
        {
            get
            {
                return GetAttribute("CallerIDNum");
            }
        }

        public string CallerIDName
        {
            get
            {
                return GetAttribute("CallerIDName");
            }
        }
    }
}