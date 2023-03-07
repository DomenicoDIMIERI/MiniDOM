
namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Handler base degli eventi di sistema
        /// </summary>
        public abstract class CBaseEventHandler 
            : IEventHandler
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBaseEventHandler()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Riceve la notifica dell'evento
            /// </summary>
            /// <param name="e"></param>
            public abstract void NotifyEvent(EventDescription e);

             
        }
    }
}