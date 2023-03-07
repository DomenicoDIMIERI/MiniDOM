using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// Effettua il logout
    /// </summary>
    /// <remarks></remarks>
    public class Logoff : Action
    {
        private string m_Username;
        private string m_Secret;

        public Logoff() : base("Logoff")
        {
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new LogoffResponse(this);
        }
    }
}