using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// The "Queues" request returns configuration and statistical information about the existing queues.
    /// </summary>
    /// <remarks></remarks>
    public class Queues : Action
    {
        public Queues() : base("Queues")
        {
        }

        protected override string GetCommandText()
        {
            string ret = base.GetCommandText();
            return ret;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new QueuesResponse(this);
        }
    }
}