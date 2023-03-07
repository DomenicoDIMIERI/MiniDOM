using DMD;

namespace minidom.CallManagers.Responses
{
    public class ZapShowChannelsResponse : ActionResponse
    {
        public ZapShowChannelsResponse()
        {
        }

        public ZapShowChannelsResponse(Action action) : base(action)
        {
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