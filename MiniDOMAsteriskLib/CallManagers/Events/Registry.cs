
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// Fired when Asterisk registers with a peer
    /// </summary>
    /// <remarks>
    /// For an entry like: register => username:password:authname@sip.domain:port/local_contact
    /// Domain would reflect the value of sip.domain
    /// </remarks>
    public class Registry : AsteriskEvent
    {
        public Registry() : base("Registry")
        {
        }

        public Registry(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Domain
        {
            get
            {
                return GetAttribute("Domain");
            }
        }

        public string Status
        {
            get
            {
                return GetAttribute("Status");
            }
        }

        public string ChannelDriver
        {
            get
            {
                return GetAttribute("ChannelDriver");
            }
        }
    }
}