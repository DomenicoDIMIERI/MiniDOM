
namespace minidom.CallManagers.Events
{
    public class ParkedCallsComplete : AsteriskEvent
    {
        public ParkedCallsComplete() : base("ParkedCallsComplete")
        {
        }

        public ParkedCallsComplete(string[] rows) : base(rows)
        {
        }
    }
}