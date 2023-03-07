using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{


    /// <summary>
    /// Contol Event Flow: Enable/Disable sending of events to this manager client.
    /// </summary>
    /// <remarks>Will not work for built-in variables like LANGUAGE !</remarks>
    public class GetVar : Action
    {
        private string m_Channel;
        private string m_Variable;
        private int? m_ActionID;

        public GetVar() : base("GetVar")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <remarks></remarks>
        public GetVar(string channel, string variable) : this()
        {
            m_Channel = channel;
            m_Variable = variable;
        }


        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <remarks></remarks>
        public GetVar(string channel, string variable, int actionID) : this()
        {
            m_Channel = channel;
            m_Variable = variable;
            m_ActionID = actionID;
        }

        public string Channel
        {
            get
            {
                return m_Channel;
            }
        }

        public string Variable
        {
            get
            {
                return m_Variable;
            }
        }

        public int? ActionID
        {
            get
            {
                return m_ActionID;
            }
        }

        protected override string GetCommandText()
        {
            if (ActionID.HasValue)
            {
                return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf + "Variable: " + Variable + DMD.Strings.vbCrLf + "ActionID: " + ActionID + DMD.Strings.vbCrLf;
            }
            else
            {
                return base.GetCommandText() + "Channel: " + Channel + DMD.Strings.vbCrLf + "Variable: " + Variable + DMD.Strings.vbCrLf;
            }
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new GetVarResponse(this);
        }
    }
}