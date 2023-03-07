
namespace minidom.CallManagers.Events
{
    public class ChannelReload : AsteriskEvent
    {
        public ChannelReload() : base("ChannelReload")
        {
        }

        public ChannelReload(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string ReloadReason
        {
            get
            {
                return GetAttribute("ReloadReason");
            }
        }

        public string RegistryCount
        {
            get
            {
                return GetAttribute("Registry_Count");
            }
        }

        public string PeerCount
        {
            get
            {
                return GetAttribute("Peer_Count");
            }
        }

        public string UserCount
        {
            get
            {
                return GetAttribute("User_Count");
            }
        }
    }
}