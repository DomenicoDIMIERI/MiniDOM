using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// Effettua il login nel manager
    /// </summary>
    /// <remarks></remarks>
    public class Login : Action
    {
        private string m_Username;
        private string m_Secret;

        public Login() : base("Login")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <remarks></remarks>
        public Login(string userName, string secret) : this()
        {
            m_Username = userName;
            m_Secret = secret;
        }

        public string UserName
        {
            get
            {
                return m_Username;
            }
        }

        public string Secret
        {
            get
            {
                return m_Secret;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "Username: " + UserName + DMD.Strings.vbCrLf + "Secret: " + Secret + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new LoginResponse(this);
        }

        public override bool RequiresAuthentication()
        {
            return false;
        }
    }
}