using System.Collections;
using DMD;

namespace minidom.CallManagers
{
    public class Peers : ArrayList // CSyncKeyCollection(Of Peer)
    {
        private AsteriskCallManager m_Owner;

        public Peers()
        {
            DMDObject.IncreaseCounter(this);
        }

        public override bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public Peers(AsteriskCallManager owner) : this()
        {
            m_Owner = owner;
        }

        // Protected Overrides Sub OnInsert(index As Integer, value As Object)
        // If (Me.m_Owner IsNot Nothing) Then DirectCast(value, Peer).SetOwner(Me.m_Owner)
        // MyBase.OnInsert(index, value)
        // End Sub

        public new void Add(Peer value)
        {
            if (m_Owner is object)
                value.SetOwner(m_Owner);
            base.Add(value);
        }

        public Peer GetItemByKey(string name)
        {
            lock (SyncRoot)
            {
                foreach (Peer item in this)
                {
                    if ((item.Name ?? "") == (name ?? ""))
                        return item;
                }

                return null;
            }
        }

        protected internal void UpdatePeerStatus(Events.PeerStatusType e)
        {
            lock (SyncRoot)
            {
                var peer = GetItemByKey(e.Peer);
                if (peer is null)
                {
                    peer = new Peer();
                    peer.Name = e.Peer;
                    Add(peer); // e.Peer) ', peer)
                }

                peer.LastUpdated = DMD.DateUtils.Now();
                peer.Status = e.PeerStatus;
            }
        }

        ~Peers()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}