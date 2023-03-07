using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{


    /// <summary>
    /// Show the IAX Peers on the server and their status
    /// </summary>
    /// <remarks>Will not work for built-in variables like LANGUAGE !</remarks>
    public class IAXpeers : Action
    {
        public IAXpeers() : base("IAXpeers")
        {
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText();
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new IAXpeersResponse(this);
        }
    }
}