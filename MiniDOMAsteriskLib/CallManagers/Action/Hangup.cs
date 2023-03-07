using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{


    /// <summary>
    /// Contol Event Flow: Enable/Disable sending of events to this manager client.
    /// </summary>
    /// <remarks>Will not work for built-in variables like LANGUAGE !</remarks>
    public class Hangup : Action
    {
        private string m_Channel;

        public Hangup() : base("Hangup")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <remarks></remarks>
        public Hangup(string channel) : this()
        {
            m_Channel = channel;
        }

        public string Channel
        {
            get
            {
                return m_Channel;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new HangupResponse(this);
        }
    }
}