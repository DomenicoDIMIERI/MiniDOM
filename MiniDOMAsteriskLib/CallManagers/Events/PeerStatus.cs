
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// Rappresenta un evento generico quando una oggetto cambia stato
    /// </summary>
    /// <remarks></remarks>
    public class PeerStatusType : AsteriskEvent
    {
        public PeerStatusType() : base("PeerStatus")
        {
        }

        public PeerStatusType(string peer, string peerStatus) : base("PeerStatus")
        {
            SetAttribute("Peer", peer);
            SetAttribute("PeerStatus", peerStatus);
        }

        public PeerStatusType(string[] rows) : base(rows)
        {
        }

        /// <summary>
        /// Restituisce l'oggetto che ha cambiato stato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Peer
        {
            get
            {
                return GetAttribute("Peer");
            }
        }

        /// <summary>
        /// Restituisce lo stato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PeerStatus
        {
            get
            {
                return GetAttribute("PeerStatus");
            }
        }

        public string Cause
        {
            get
            {
                return GetAttribute("Cause");
            }
        }

        public string Time
        {
            get
            {
                return GetAttribute("Time");
            }
        }
    }
}