using System;
using System.Collections;

namespace minidom.CallManagers.Actions
{
    // Inherits System.Collections.Specialized.NameValueCollection  '(Of ActionResponseQueue)
    public class ActionResponseQueues : ArrayList
    {
        private AsteriskCallManager m_Owner;

        public ActionResponseQueues()
        {
            DMDObject.IncreaseCounter(this);
        }

        public ActionResponseQueues(AsteriskCallManager owner) : this()
        {
            if (owner is null)
                throw new ArgumentException("Owner");
            m_Owner = owner;
        }

        public AsteriskCallManager Owner
        {
            get
            {
                return m_Owner;
            }
        }

        ~ActionResponseQueues()
        {
            DMDObject.DecreaseCounter(this);
        }

        public ActionResponseQueue GetItemByKey(string key)
        {
            lock (SyncRoot)
            {
                foreach (ActionResponseQueue item in this)
                {
                    if ((item.Action.ActionID ?? "") == (key ?? ""))
                        return item;
                }

                return null;
            }
        }

        // Protected Overrides Sub OnInsert(index As Integer, value As Object)
        // If (Me.m_Owner IsNot Nothing) Then DirectCast(value, ActionResponseQueue).SetOwner(Me.m_Owner)
        // MyBase.OnInsert(index, value)
        // End Sub


        // Default Public ReadOnly Property Item(ByVal index As Integer) As ActionResponseQueue
        // Get
        // Return MyBase.InnerList(index)
        // End Get
        // End Property

        // Friend Sub Add(ByVal actionID As String, ByVal item As ActionResponseQueue)
        // Me.InnerList.Add(item)
        // End Sub

    }
}