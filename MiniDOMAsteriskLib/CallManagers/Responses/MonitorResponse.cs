using DMD;

namespace minidom.CallManagers.Responses
{
    public class MonitorResponse : ActionResponse
    {
        public MonitorResponse()
        {
        }

        public MonitorResponse(Action action) : base(action)
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
            return base.ToString();
        }
    }
}