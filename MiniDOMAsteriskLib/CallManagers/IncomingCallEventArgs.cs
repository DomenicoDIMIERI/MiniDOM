using System;

namespace minidom.CallManagers
{

    /// <summary>
    /// Informazioni su una chiamata in ingresso
    /// </summary>
    /// <remarks></remarks>
    public class IncomingCallEventArgs : EventArgs
    {
        public IncomingCallEventArgs()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~IncomingCallEventArgs()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}