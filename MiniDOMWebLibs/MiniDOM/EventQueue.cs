using System;

namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
    /// Questa classe serve per stabilire un sistema di comunicazione ad eventi
    /// con
    /// </summary>
    /// <remarks></remarks>
        public class EventQueue
        {
            private System.Text.StringBuilder m_Buffer = new System.Text.StringBuilder();

            public EventQueue()
            {
                DMDObject.IncreaseCounter(this);
            }

            public void Enque(string eventName, object e)
            {
                throw new NotImplementedException();
            }

            ~EventQueue()
            {
                DMDObject.DecreaseCounter(this);
            }

            public string Deque()
            {
                throw new NotImplementedException();
            }
        }
    }
}