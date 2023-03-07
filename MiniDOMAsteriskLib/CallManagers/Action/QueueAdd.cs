using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class QueueAdd : AsyncAction
    {
        private string m_Queue;
        private string m_Interface;
        private int m_Penalty;
        private bool m_Paused;
        private string m_MemberName;
        private string m_StateInterface;

        public QueueAdd() : base("QueueAdd")
        {
        }

        public QueueAdd(string queue, string @interface, int penalty, bool paused, string memberName, string stateInterface) : this()
        {
            m_Queue = queue;
            m_Interface = @interface;
            m_Penalty = penalty;
            m_Paused = paused;
            m_MemberName = memberName;
            m_StateInterface = stateInterface;
        }

        public string Queue
        {
            get
            {
                return m_Queue;
            }
        }

        public string Interface
        {
            get
            {
                return m_Interface;
            }
        }

        public int Penalty
        {
            get
            {
                return m_Penalty;
            }
        }

        public bool Paused
        {
            get
            {
                return m_Paused;
            }
        }

        public string MemberName
        {
            get
            {
                return m_MemberName;
            }
        }

        public string StateInterface
        {
            get
            {
                return m_StateInterface;
            }
        }

        protected override string GetCommandText()
        {
            return DMD.Strings.JoinW(
                    base.GetCommandText(),
                    "Queue: " , Queue , DMD.Strings.vbCrLf,
                    "Interface: ", Interface , DMD.Strings.vbCrLf,
                    "Penalty: " , Penalty , DMD.Strings.vbCrLf,
                    "Paused: ", ((Paused)? "Yes" : "No"), DMD.Strings.vbCrLf,
                    "MemberName: " , MemberName , DMD.Strings.vbCrLf,
                    "StateInterface: " , StateInterface , DMD.Strings.vbCrLf
                    );
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new QueueAddResponse(this);
        }
    }
}