
namespace minidom.CallManagers.Events
{
    public class ZapShowChannelsComplete : AsteriskEvent
    {
        public ZapShowChannelsComplete() : base("ZapShowChannelsComplete")
        {
        }

        public ZapShowChannelsComplete(string[] rows) : base(rows)
        {
        }
    }
}