using System;
using System.Collections;

namespace minidom.CallManagers.Actions
{
    public class ActionResponseQueue
    {
        private AsteriskCallManager m_Owner;
        private AsyncAction m_Action;
        private ArrayList m_Items; // CCollection(Of AsteriskEvent)

        public ActionResponseQueue()
        {
            DMDObject.IncreaseCounter(this);
        }

        public ActionResponseQueue(AsyncAction action) : this()
        {
            if (action is null)
                throw new ArgumentException("Action");
            m_Action = action;
        }

        public AsyncAction Action
        {
            get
            {
                return m_Action;
            }
        }

        public ArrayList Items // CCollection(Of AsteriskEvent)
        {
            get
            {
                if (m_Items is null)
                    m_Items = new ArrayList(); // CCollection(Of AsteriskEvent)
                return m_Items;
            }
        }

        public AsteriskCallManager Owner
        {
            get
            {
                return m_Owner;
            }
        }

        protected internal virtual void SetOwner(AsteriskCallManager owner)
        {
            m_Owner = owner;
        }

        ~ActionResponseQueue()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}