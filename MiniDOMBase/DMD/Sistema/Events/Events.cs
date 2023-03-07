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

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CEventLog"/>
        /// </summary>
        public class CEventsClass
            : CModulesClass<Sistema.CEventLog>
        {
            /// <summary>
            /// Gestisce l'evento UnhandledException
            /// </summary>
            public event UnhandledExceptionEventHandler UnhandledException;

            /// <summary>
            /// Firma dell'evento UnhandledException
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            public delegate void UnhandledExceptionEventHandler(object sender, System.Exception e);

            private bool m_StopEvents;
            private bool m_LogEvents;
            // Private m_EventHandlers As RegisteredEventHandlers

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEventsClass() 
                : base("modSysEventsModule", typeof(Sistema.CEventsCursor))
            {
                m_LogEvents = false;
                m_StopEvents = false;
            }

            /// <summary>
            /// Restituisce o impopsta un valore booleano che indica se gli eventi devono essere bloccati a livello glibale.
            /// Questo impedisce sia la memorizzazione nel log che l'esecuzione degli handlers associati ad un evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool StopEvents
            {
                get
                {
                    return m_StopEvents;
                }

                set
                {
                    m_StopEvents = value;
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se gli eventi devono essere memorizzati nel file di log
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool LogEvents
            {
                get
                {
                    return m_LogEvents;
                }

                set
                {
                    m_LogEvents = value;
                }
            }

            /// <summary>
            /// Memorizza i dettagli dell'evento e lo notifica a tutti gli handlers registrati
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            protected internal void DispatchEvent(Sistema.EventDescription e)
            {
                if (StopEvents == true)
                    return;
                if (e.Parametri.ContainsKey("CurrentUser") == false)
                    e.Parametri.Add("CurrentUser", Sistema.Users.CurrentUser);
                if (m_LogEvents)
                {
                    var objLog = new Sistema.CEventLog(e);
                    objLog.Save();
                }

                var items = Sistema.RegisteredEventHandlers.GetHandlers(e.Module, e.EventName);
                foreach (Sistema.IEventHandler handler in items)
                    handler.NotifyEvent(e);
            }

            
            /// <summary>
            /// Questo memodo genera l'evento UnhandledException
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public void NotifyUnhandledException(Exception e)
            {
                UnhandledException?.Invoke(this, e);
            }
        }
    }

    public partial class Sistema
    {




        
        private static CEventsClass m_Events = null;


        /// <summary>
        /// Repository di oggetti di tipo <see cref="CEventLog"/>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CEventsClass Events
        {
            get
            {
                if (m_Events is null)
                    m_Events = new CEventsClass();
                return m_Events;
            }
        }
    }
}