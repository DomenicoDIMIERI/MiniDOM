using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// Check Mailbox Message Count
    /// </summary>
    /// <remarks></remarks>
    public class MailboxCount : Action
    {
        private string m_Mailbox;
        private int? m_ActionID;

        public MailboxCount() : base("MailboxCount")
        {
        }

        public MailboxCount(string mailbox) : this()
        {
            m_Mailbox = mailbox;
        }

        public MailboxCount(string mailbox, int actionID) : this()
        {
            m_Mailbox = mailbox;
            m_ActionID = actionID;
        }

        public string Mailbox
        {
            get
            {
                return m_Mailbox;
            }
        }

        public int? ActionID
        {
            get
            {
                return m_ActionID;
            }
        }

        protected override string GetCommandText()
        {
            if (m_ActionID.HasValue)
            {
                return base.GetCommandText() + "Mailbox: " + m_Mailbox + DMD.Strings.vbCrLf + "Actionid: " + m_ActionID;
            }
            else
            {
                return base.GetCommandText() + "Mailbox: " + m_Mailbox + DMD.Strings.vbCrLf;
            }
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new MailboxCountResponse(this);
        }

        public override ActionPrivilageFlag RequiredPrivilages
        {
            get
            {
                return ActionPrivilageFlag.call | ActionPrivilageFlag.all;
            }
        }
    }
}