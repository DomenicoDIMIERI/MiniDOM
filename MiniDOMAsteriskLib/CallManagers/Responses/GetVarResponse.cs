using DMD;
using DMD.XML;

namespace minidom.CallManagers.Responses
{
    public class GetVarResponse : ActionResponse
    {
        private int m_varname;
        private string m_Value;

        public GetVarResponse()
        {
        }

        public GetVarResponse(Action action) : base(action)
        {
        }

        public int VarName
        {
            get
            {
                return m_varname;
            }
        }

        public string Value
        {
            get
            {
                return m_Value;
            }
        }

        protected override void ParseRow(RowEntry row)
        {
            switch (Strings.UCase(row.Command) ?? "")
            {
                case "VARNAME":
                    {
                        m_varname = DMD.Integers.ValueOf(row.Params);
                        break;
                    }

                case "VALUE":
                    {
                        m_Value = row.Params;
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
            return base.ToString() + "(Varname: " + m_varname + ", Value: " + m_Value + ")";
        }
    }
}