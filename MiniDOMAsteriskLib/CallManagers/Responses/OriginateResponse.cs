using DMD;

namespace minidom.CallManagers.Responses
{
    public class OriginateResponse : ActionResponse
    {
        private string m_Channel;
        private string m_Context;
        private string m_Exten;
        private string m_Reason;
        private string m_Uniqueid;
        private string m_CallerIDNum;
        private string m_CallerIDName;

        public OriginateResponse()
        {
            m_Channel = "";
            m_Context = "";
            m_Exten = "";
            m_Reason = "";
            m_Uniqueid = "";
            m_CallerIDNum = "";
            m_CallerIDName = "";
        }

        public OriginateResponse(Action action) : base(action)
        {
        }

        public string Channel
        {
            get
            {
                return m_Channel;
            }
        }

        public string Context
        {
            get
            {
                return m_Context;
            }
        }

        public string Exten
        {
            get
            {
                return m_Exten;
            }
        }

        public string Reason
        {
            get
            {
                return m_Reason;
            }
        }

        public string UniqueID
        {
            get
            {
                return m_Uniqueid;
            }
        }

        public string CallerIDNum
        {
            get
            {
                return m_CallerIDNum;
            }
        }

        public string CallerIDName
        {
            get
            {
                return m_CallerIDName;
            }
        }

        protected override void ParseRow(RowEntry row)
        {
            switch (Strings.UCase(row.Command) ?? "")
            {
                case "CHANNEL":
                    {
                        m_Channel = row.Params;
                        break;
                    }

                case "CONTEXT":
                    {
                        m_Context = row.Params;
                        break;
                    }

                case "EXTEN":
                    {
                        m_Exten = row.Params;
                        break;
                    }

                case "REASON":
                    {
                        m_Reason = row.Params;
                        break;
                    }

                case "UNIQUEID":
                    {
                        m_Uniqueid = row.Params;
                        break;
                    }

                case "CALLERIDNUM":
                    {
                        m_CallerIDNum = row.Params;
                        break;
                    }

                case "CALLERIDNAME":
                    {
                        m_CallerIDName = row.Params;
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
            string ret = base.ToString();
            ret += "Channel: " + Channel + DMD.Strings.vbCrLf;
            ret += "Context: " + Context + DMD.Strings.vbCrLf;
            ret += "Exten: " + Exten + DMD.Strings.vbCrLf;
            ret += "Reason: " + Reason + DMD.Strings.vbCrLf;
            ret += "UniqueID: " + UniqueID + DMD.Strings.vbCrLf;
            ret += "CallerIDNum: " + CallerIDNum + DMD.Strings.vbCrLf;
            ret += "CallerIDName: " + CallerIDName + DMD.Strings.vbCrLf;
            return ret;
        }
    }
}