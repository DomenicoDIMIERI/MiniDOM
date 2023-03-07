using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Evento relativo ad un messaggio email
        /// </summary>
        [Serializable]
        public class EmailEventArg 
            : DMDEventArgs
        {
            [NonSerialized]  private MailMessage m_Message;

            /// <summary>
            /// Costruttore
            /// </summary>
            public EmailEventArg()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="msg"></param>
            public EmailEventArg(MailMessage msg)
            {
                if (msg is null)
                    throw new ArgumentNullException("msg");
                m_Message = msg;
            }

            /// <summary>
            /// Messaggio
            /// </summary>
            public MailMessage Message
            {
                get
                {
                    return m_Message;
                }
            }
        }

        
        /// <summary>
        /// Firma dell'evento EmailReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EmailReceivedEventHandler(object sender, EmailEventArg e);

        /// <summary>
        /// Firma dell'evento EmailSent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EmailSentEventHandler(object sender, EmailEventArg e);
    }
}