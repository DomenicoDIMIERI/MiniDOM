using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class ListCommands : Action
    {
        private int m_ActionID;

        public ListCommands() : base("IAXpeers")
        {
        }

        public ListCommands(int actionID) : this()
        {
            m_ActionID = actionID;
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
            return base.GetCommandText() + "ActionID: " + ActionID + DMD.Strings.vbCrLf;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new ListCommandsResponse(this);
        }
    }
}