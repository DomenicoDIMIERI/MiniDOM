using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// Changes the file name of a recording occuring on a channel
    /// </summary>
    /// <remarks></remarks>
    public class CommandType : Action
    {
        private string m_Command;

        public CommandType() : base("Command")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <param name="command">[in] Comando da inviare al CLI</param>
        /// <remarks></remarks>
        public CommandType(string command) : this()
        {
            m_Command = Strings.Trim(command);
        }

        public string Command
        {
            get
            {
                return m_Command;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "command: " + Command + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new CommandResponse(this);
        }
    }
}