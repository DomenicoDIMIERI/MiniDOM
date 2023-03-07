using DMD;

namespace minidom.CallManagers
{
    public class AsyncAction : Action
    {
        private string m_ActionID;

        public AsyncAction()
        {
            m_ActionID = "";
        }

        public AsyncAction(string actionName, string actionID) : base(actionName)
        {
            m_ActionID = actionID;
        }

        public AsyncAction(string actionName) : base(actionName)
        {
            m_ActionID = "";
        }

        public string ActionID
        {
            get
            {
                return m_ActionID;
            }
        }

        protected internal void SetActionID(string id)
        {
            m_ActionID = id;
        }

        protected override string GetCommandText()
        {
            string ret = base.GetCommandText();
            if (!string.IsNullOrEmpty(m_ActionID))
                ret += "Actionid: " + m_ActionID + DMD.Strings.vbCrLf;
            return ret;
        }

        public override bool IsAsync()
        {
            return true;
        }

        protected internal virtual void NotifyEvent(AsteriskEvent e)
        {
        }
    }
}