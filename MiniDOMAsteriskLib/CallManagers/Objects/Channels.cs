using System;
using System.Collections;
using DMD.XML;

namespace minidom.CallManagers
{
    public class Channels : ArrayList // minidom.CSyncKeyCollection(Of Channel)
    {
        private AsteriskCallManager m_Owner;

        public Channels()
        {
            DMDObject.IncreaseCounter(this);
            m_Owner = null;
        }

        public Channels(AsteriskCallManager owner) : this()
        {
            m_Owner = owner;
        }

        public override bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        // Protected Overrides Sub OnInsert(index As Integer, value As Object)
        // If (Me.m_Owner IsNot Nothing) Then DirectCast(value, IDObject).SetOwner(Me.m_Owner)
        // MyBase.OnInsert(index, value)
        // End Sub

        internal void SetUpChannel(Events.Newchannel e)
        {
            Add(new ChannelType(e));
        }

        private bool ContainsKey(string key)
        {
            return IndexOf(key) >= 0;
        }

        public new ChannelType this[int index]
        {
            get
            {
                return (ChannelType)base[index];
            }
        }

        private int IndexOfKey(string key)
        {
            lock (SyncRoot)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if ((this[i].Channel ?? "") == (key ?? ""))
                        return i;
                }

                return -1;
            }
        }

        public new void Add(ChannelType value)
        {
            if (m_Owner is object)
                value.SetOwner(m_Owner);
            base.Add(value);
        }

        internal void HangUpChannel(Events.HangupEvent e)
        {
            lock (SyncRoot)
            {
                if (ContainsKey(e.Channel) == false)
                    return;
                this[DMD.Integers.ValueOf(e.Channel)].Hangup();
                RemoveByKey(e.Channel);
            }
        }

        public void RemoveByKey(string key)
        {
            lock (SyncRoot)
            {
                int i = DMD.Integers.ValueOf(IndexOfKey(key));
                if (i < 0)
                    throw new ArgumentOutOfRangeException();
                RemoveAt(i);
            }
        }

        ~Channels()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}