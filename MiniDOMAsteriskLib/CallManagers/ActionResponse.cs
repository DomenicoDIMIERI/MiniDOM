using System.Diagnostics;
using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers
{
    public class ActionResponse
    {
        private Action m_Action;
        private string m_Response;
        private string m_Message;
        private string m_ActionID;

        public ActionResponse()
        {
            DMDObject.IncreaseCounter(this);
        }

        public ActionResponse(Action action) : this()
        {
            m_Action = action;
        }

        public Action Action
        {
            get
            {
                return m_Action;
            }
        }

        public string Response
        {
            get
            {
                return m_Response;
            }
        }

        public string Message
        {
            get
            {
                return m_Message;
            }
        }

        public string ActionID
        {
            get
            {
                return m_ActionID;
            }
        }

        public virtual bool IsSuccess()
        {
            return m_Response == "Success";
        }

        public override string ToString()
        {
            string ret = "";
            ret += "Response: " + Response + DMD.Strings.vbCrLf;
            ret += "Message: " + Message + DMD.Strings.vbCrLf;
            return ret;
        }

        protected internal virtual void Parse(string[] rows)
        {
            foreach (string r in rows)
            {
                var item = new RowEntry(r);
                ParseRow(item);
            }
        }

        protected virtual void ParseRow(RowEntry row)
        {
            switch (Strings.UCase(row.Command) ?? "")
            {
                case "RESPONSE":
                    {
                        m_Response = row.Params;
                        break;
                    }

                case "MESSAGE":
                    {
                        m_Message = row.Params;
                        break;
                    }

                case "ACTIONID":
                    {
                        m_ActionID = row.Params;
                        break;
                    }

                default:
                    {
                        Debug.Print("Unsupported response: " + row.Params);
                        break;
                    }
            }
        }

        ~ActionResponse()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}