
namespace minidom.CallManagers.Events
{
    public class UnParkedCall : AsteriskEvent
    {
        public UnParkedCall() : base("UnParkedCall")
        {
        }

        public UnParkedCall(string[] rows) : base(rows)
        {
        }
    }
}