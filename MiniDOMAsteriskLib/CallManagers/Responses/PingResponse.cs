using DMD;

namespace minidom.CallManagers.Responses
{
    public class PingResponse : ActionResponse
    {
        public PingResponse()
        {
        }

        public PingResponse(Action action) : base(action)
        {
        }

        public override bool IsSuccess()
        {
            return Response == "Pong";
        }

        protected override void ParseRow(RowEntry row)
        {
            switch (Strings.UCase(row.Command) ?? "")
            {
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
            return ret;
        }
    }
}