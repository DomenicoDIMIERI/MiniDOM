
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// sent following an Action: Queues
    /// </summary>
    /// <remarks></remarks>
    public class QueueParams 
        : AsteriskEvent
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public QueueParams() : base("QueueParams")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="rows"></param>
        public QueueParams(string[] rows) : base(rows)
        {
        }


        /// <summary>
        /// Nome della coda
        /// </summary>
        public string Queue
        {
            get
            {
                return GetAttribute("Queue");
            }
        }

        /// <summary>
        /// Max
        /// </summary>
        public int Max
        {
            get
            {
                return GetAttribute("Max", 0);
            }
        }

        public int Calls
        {
            get
            {
                return GetAttribute("Calls", 0);
            }
        }

        public int Holdtime
        {
            get
            {
                return GetAttribute("Holdtime", 0);
            }
        }

        public int Completed
        {
            get
            {
                return GetAttribute("Completed", 0);
            }
        }

        public int Abandoned
        {
            get
            {
                return GetAttribute("Abandoned", 0);
            }
        }

        public int ServiceLevel
        {
            get
            {
                return GetAttribute("ServiceLevel", 0);
            }
        }

        public float ServicelevelPerf
        {
            get
            {
                return GetAttribute("ServicelevelPerf", 0.0f);
            }
        }
    }
}