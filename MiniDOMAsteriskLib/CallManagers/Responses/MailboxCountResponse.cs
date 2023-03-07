using DMD;
using DMD.XML;

namespace minidom.CallManagers.Responses
{
    public class MailboxCountResponse : ActionResponse
    {
        private string m_MailBox;
        private int m_NewMessages;
        private int m_OldMessages;

        public MailboxCountResponse()
        {
            m_MailBox = "";
            m_NewMessages = 0;
            m_OldMessages = 0;
        }

        public MailboxCountResponse(Action action) : base(action)
        {
        }

        public string MailBox
        {
            get
            {
                return m_MailBox;
            }
        }

        public int NewMessages
        {
            get
            {
                return m_NewMessages;
            }
        }

        public int OldMessages
        {
            get
            {
                return m_OldMessages;
            }
        }

        protected override void ParseRow(RowEntry row)
        {
            switch (Strings.UCase(row.Command) ?? "")
            {
                case "MAILBOX":
                    {
                        m_MailBox = row.Params;
                        break;
                    }

                case "NEWMESSAGES":
                    {
                        m_NewMessages = DMD.Integers.ValueOf(row.Params);
                        break;
                    }

                case "OLDMESSAGES":
                    {
                        m_NewMessages = DMD.Integers.ValueOf(row.Params);
                        break;
                    }

                default:
                    {
                        base.ParseRow(row);
                        break;
                    }
            }
        }

        public override string ToString()
        {
            return base.ToString() + "(MailBox: " + MailBox + ", Newmessages: " + NewMessages + ", Oldmessages: " + OldMessages + ")";
        }
    }
}