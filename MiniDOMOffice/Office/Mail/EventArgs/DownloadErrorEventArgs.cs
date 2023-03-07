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
        /// Evento generato quando si verifica un errore durante il download dei messaggi di posta elettronica
        /// </summary>
        [Serializable]
        public class DownloadErrorEventArgs
            : DMDEventArgs
        {
            [NonSerialized] private MailAccount m_Account;
            [NonSerialized] private Exception m_Exception;
            [NonSerialized] private string m_Description;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DownloadErrorEventArgs()
            {
                
            }
             
            /// <summary>
            /// Account
            /// </summary>
            public MailAccount Account
            {
                get
                {
                    return m_Account;
                }
            }

            /// <summary>
            /// Eccezione
            /// </summary>
            public Exception Exception
            {
                get
                {
                    return m_Exception;
                }
            }

            /// <summary>
            /// Descrizione dell'errore
            /// </summary>
            public string Description
            {
                get
                {
                    return m_Description;
                }
            }
        }

        /// <summary>
        /// Firma dell'evento DownloadException
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DownloadExceptionEventHandler(object sender,  DownloadErrorEventArgs e);

    }
}