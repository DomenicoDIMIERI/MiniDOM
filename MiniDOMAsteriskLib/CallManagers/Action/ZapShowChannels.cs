using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class ZapShowChannels : AsyncAction
    {
        public ZapShowChannels() : base("ZapShowChannels")
        {
        }

        protected override string GetCommandText()
        {
            string ret = base.GetCommandText();
            return ret;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new ZapShowChannelsResponse(this);
        }
    }
}