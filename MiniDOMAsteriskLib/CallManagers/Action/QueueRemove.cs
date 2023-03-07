using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class QueueRemove : AsyncAction
    {
        private string m_Queue;
        private string m_Interface;

        public QueueRemove() : base("QueueRemove")
        {
        }

        /// <summary>
        /// QueueRemove
        /// </summary>
        /// <param name="queue">Existing queue to remove member from</param>
        /// <param name="interface">Member interface (sip/1000, zap/1-1, etc)?</param>
        /// <remarks></remarks>
        public QueueRemove(string queue, string @interface) : this()
        {
            m_Queue = queue;
            m_Interface = @interface;
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

        protected override string GetCommandText()
        {
            string ret = base.GetCommandText();
            ret += "Queue: " + Queue + DMD.Strings.vbCrLf;
            ret += "Interface: " + Interface + DMD.Strings.vbCrLf;
            return ret;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new QueueRemoveResponse(this);
        }
    }
}