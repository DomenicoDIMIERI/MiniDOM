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
using DMD.Exceptions;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Argomenti dell'eccezione
        /// </summary>
        [Serializable]
        public class EventHandlerException : 
            RuntimeException
        {
            private EventDescription m_E;
            private IEventHandler m_Handler;
            private System.Exception m_Exception;

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="e"></param>
            /// <param name="handler"></param>
            /// <param name="ex"></param>
            public EventHandlerException(EventDescription e, IEventHandler handler, System.Exception ex) 
                : base("Eccezione generata per l'azione (" + e.Module.ModuleName + ", " + e.EventName + ") dall'handler " + DMD.RunTime.vbTypeName(handler), ex)
            {
                m_E = e;
                m_Handler = handler;
                m_Exception = ex;
            }

            /// <summary>
            /// Modulo
            /// </summary>
            public CModule Module
            {
                get
                {
                    return m_E.Module;
                }
            }

            /// <summary>
            /// Nome dell'evento
            /// </summary>
            public string EventName
            {
                get
                {
                    return m_E.EventName;
                }
            }

            /// <summary>
            /// Descrizione dell'eccezione
            /// </summary>
            public string Description
            {
                get
                {
                    return m_E.Descrizione;
                }
            }

            /// <summary>
            /// Descrittore
            /// </summary>
            public object Descrittore
            {
                get
                {
                    return m_E.Descrittore;
                }
            }

            /// <summary>
            /// Handler
            /// </summary>
            public object Handler
            {
                get
                {
                    return m_Handler;
                }
            }

            /// <summary>
            /// Eccezione
            /// </summary>
            public Exception Exeption
            {
                get
                {
                    return m_Exception;
                }
            }
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    }
}