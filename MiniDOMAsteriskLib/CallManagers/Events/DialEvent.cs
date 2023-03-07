
namespace minidom.CallManagers
{
    public class DialEvent : AsteriskEvent
    {
        public DialEvent()
        {
        }

        public DialEvent(AsteriskCallManager server, string[] rows) : base(server, rows)
        {
        }

        public DialEvent(AsteriskCallManager server, AsteriskEvent e) : base(server, e)
        {
        }

        public string CallerIDNumber
        {
            get
            {
                return GetAttribute("CALLERIDNUM");
            }
        }

        public string Channel
        {
            get
            {
                return GetAttribute("CHANNEL");
            }
        }

        public string ConnectedLineName
        {
            get
            {
                return GetAttribute("CONNECTEDLINENAME");
            }
        }

        public string ConnectedLineNum
        {
            get
            {
                return GetAttribute("CONNECTEDLINENUM");
            }
        }

        public string Destination
        {
            get
            {
                return GetAttribute("DESTINATION");
            }
        }

        public string DestUniqueID
        {
            get
            {
                return GetAttribute("DESTUNIQUEID");
            }
        }

        public string DialString
        {
            get
            {
                return GetAttribute("DIALSTRING");
            }
        }

        public string SubEvent
        {
            get
            {
                return GetAttribute("SUBEVENT");
            }
        }

        public string UniqueID
        {
            get
            {
                return GetAttribute("UNIQUEID");
            }
        }
    }
}