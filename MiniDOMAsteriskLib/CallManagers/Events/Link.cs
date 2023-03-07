
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// Fired when two voice channels are linked together and voice data exchange commences.
    /// </summary>
    /// <remarks>Several Link events may be seen for a single call. This can occur when Asterisk fails to setup a native bridge for the call. As far as I can tell, this is when Asterisk must sit between two telephones and perform CODEC conversion on their behalf.</remarks>
    public class Link : AsteriskEvent
    {
        public Link() : base("Link")
        {
        }

        public Link(string[] rows) : base(rows)
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

        public string BridgeState
        {
            get
            {
                return GetAttribute("Bridgestate");
            }
        }

        public string BridgeType
        {
            get
            {
                return GetAttribute("Bridgetype");
            }
        }

        public string TimeStamp
        {
            get
            {
                return GetAttribute("Timestamp");
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