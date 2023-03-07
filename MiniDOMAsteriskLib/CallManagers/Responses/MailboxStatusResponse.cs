using DMD;
using DMD.XML;

namespace minidom.CallManagers.Responses
{
    public class MailboxStatusResponse : ActionResponse
    {
        private string m_MailBox;
        private int m_Waiting;

        public MailboxStatusResponse()
        {
            m_MailBox = "";
            m_Waiting = 0;
        }

        public MailboxStatusResponse(Action action) : base(action)
        {
        }

        public int Waiting
        {
            get
            {
                return m_Waiting;
            }
        }

        public string MailBox
        {
            get
            {
                return m_MailBox;
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

                case "WAITING":
                    {
                        m_Waiting = DMD.Integers.ValueOf(row.Params);
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
            return base.ToString() + "(MailBox: " + MailBox + ", Waiting: " + Waiting + ")";
        }
    }
}