using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class Ping : Action
    {
        public Ping() : base("Ping")
        {
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new PingResponse(this);
        }
    }
}