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
        /// Repository di oggetti di tipo <see cref="RegisteredEventHandler"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CRegisteredEventHandlersClass 
            : CModulesClass<Sistema.RegisteredEventHandler>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredEventHandlersClass()
                : base("modRegisteredEventsHandlers", typeof(Sistema.RegisteredEventHandlerCursor), -1)
            {
            }

            /// <summary>
            /// Registra un gestore di eventi
            /// </summary>
            /// <param name="handler"></param>
            /// <param name="m"></param>
            /// <param name="eventName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.RegisteredEventHandler Register(object handler, Sistema.CModule m, string eventName)
            {
                if (handler is null)
                    throw new ArgumentNullException("handler");
                eventName = DMD.Strings.Trim(eventName);
                var r = GetItem(handler, m, eventName);
                if (r is null)
                {
                    r = new Sistema.RegisteredEventHandler();
                    r.Module = m;
                    r.EventName = eventName;
                    r.ClassName = handler.GetType().FullName;
                    r.Save();
                }

                UpdateCached(r);
                return r;
            }


            private Sistema.RegisteredEventHandler GetItem(object handler, Sistema.CModule m, string eventName)
            {
                eventName = DMD.Strings.Trim(eventName);
                var items = LoadAll();
                foreach (Sistema.RegisteredEventHandler r in items)
                {
                    if (ReferenceEquals(r.Module, m) && (r.EventName ?? "") == (eventName ?? "") && ReferenceEquals(r.CreateHandler(), handler.GetType()))
                        return r;
                }

                return null;
            }

            /// <summary>
            /// Cancella un gestore di eventi
            /// </summary>
            /// <param name="handler"></param>
            /// <param name="m"></param>
            /// <param name="eventName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.RegisteredEventHandler Unregister(object handler, Sistema.CModule m, string eventName)
            {
                if (handler is null)
                    throw new ArgumentNullException("handler");
                eventName = DMD.Strings.Trim(eventName);
                var r = GetItem(handler, m, eventName);
                if (r is object)
                {
                    r.Delete();
                    UpdateCached(r);
                }

                return r;
            }


            /// <summary>
        /// Restituisce la collezione di handlers registrati per l'evento specifico del modulo
        /// </summary>
        /// <param name="m"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Sistema.IEventHandler> GetHandlers(Sistema.CModule m, string eventName)
            {
                var items = LoadAll();
                var ret = new CCollection<Sistema.IEventHandler>();
                int mID = DBUtils.GetID(m, 0);
                object handler;
                eventName = DMD.Strings.Trim(eventName);
                foreach (Sistema.RegisteredEventHandler r in items)
                {
                    if (r.Active && (r.ModuleID == mID || r.ModuleID == 0) && ((r.EventName ?? "") == (eventName ?? "") || string.IsNullOrEmpty(r.EventName)))
                    {
#if (!DEBUG)
                        try {
#endif
                            handler = r.CreateHandler();
                            if (handler is object)
                                ret.Add((Sistema.IEventHandler)r.CreateHandler());
#if (!DEBUG)
                        } catch (Exception ex) {
                            Sistema.Events.NotifyUnhandledException(ex);
                        }
#endif             
                    }
                }

                return ret;
            }
        }
    }

    public partial class Sistema
    {
        private static CRegisteredEventHandlersClass m_RegisteredEventHandler = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="RegisteredEventHandler"/>
        /// </summary>
        public static CRegisteredEventHandlersClass RegisteredEventHandlers
        {
            get
            {
                if (m_RegisteredEventHandler is null)
                    m_RegisteredEventHandler = new CRegisteredEventHandlersClass();
                return m_RegisteredEventHandler;
            }
        }
    }
}