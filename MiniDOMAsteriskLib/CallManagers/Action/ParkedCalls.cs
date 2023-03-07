using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class ParkedCalls : Action
    {
        public ParkedCalls() : base("ParkedCalls")
        {
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new ParkedCallsResponse(this);
        }
    }
}