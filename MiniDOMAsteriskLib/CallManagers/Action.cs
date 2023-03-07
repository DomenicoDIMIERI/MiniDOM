using DMD;

namespace minidom.CallManagers
{
    public class Action
    {
        private AsteriskCallManager m_Owner;
        private string m_ActionName;
        private ActionResponse m_Response;

        public Action()
        {
            DMDObject.IncreaseCounter(this);
            m_ActionName = "";
            m_Response = null;
        }

        public Action(string actionName) : this()
        {
            m_ActionName = Strings.Trim(actionName);
            m_Response = null;
        }

        public string ActionName
        {
            get
            {
                return m_ActionName;
            }
        }

        public virtual ActionPrivilageFlag RequiredPrivilages
        {
            get
            {
                return ActionPrivilageFlag.none;
            }
        }

        protected virtual string GetCommandText()
        {
            string ret = "";
            ret += "Action: " + m_ActionName + DMD.Strings.vbCrLf;
            return ret;
        }

        protected internal virtual void Send(AsteriskCallManager manager)
        {
            manager.Send(GetCommandText());
        }

        public ActionResponse Response
        {
            get
            {
                return m_Response;
            }
        }

        public override string ToString()
        {
            return GetCommandText();
        }

        public virtual bool IsAsync()
        {
            return false;
        }

        protected virtual ActionResponse AllocateResponse(string[] rows)
        {
            return new ActionResponse(this);
        }

        protected internal void ParseResponse(string[] rows)
        {
            m_Response = AllocateResponse(rows);
            m_Response.Parse(rows);
        }

        public virtual bool RequiresAuthentication()
        {
            return true;
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

        ~Action()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}