using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{


    /// <summary>
    /// Contol Event Flow: Enable/Disable sending of events to this manager client.
    /// </summary>
    /// <remarks></remarks>
    public class ExtensionState : Action
    {
        private string m_Context;
        private string m_Exten;
        private int m_ActionID;

        public ExtensionState() : base("ExtensionState")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <remarks></remarks>
        public ExtensionState(string exten, string context, int actionID) : this()
        {
            m_Context = context;
            m_Exten = exten;
            m_ActionID = actionID;
        }

        public string Context
        {
            get
            {
                return m_Context;
            }
        }

        public string Exten
        {
            get
            {
                return m_Exten;
            }
        }

        public int ActionID
        {
            get
            {
                return m_ActionID;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "Context: " + Context + DMD.Strings.vbCrLf + "Exten: " + Exten + DMD.Strings.vbCrLf + "ActionID: " + ActionID + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new ExtensionStateResponse(this);
        }
    }
}