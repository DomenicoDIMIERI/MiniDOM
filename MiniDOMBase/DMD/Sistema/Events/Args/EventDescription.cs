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
    public partial class Sistema
    {

        /// <summary>
        /// Evento generato da un modulo di sistema
        /// </summary>
        [Serializable]
        public class EventDescription 
            : DMDEventArgs
        {

            private DateTime m_Data;
            private string m_EventName;
            [NonSerialized] private CModule m_Module;
            [NonSerialized] private CUser m_Utente;
            private string m_Descrizione;
            private object m_Descrittore;
            private CKeyCollection m_Parametri;

            /// <summary>
            /// Costruttore
            /// </summary>
            public EventDescription()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="eventName"></param>
            /// <param name="descrizione"></param>
            /// <param name="oggetto"></param>
            public EventDescription(string eventName, string descrizione, object oggetto)
            {
                m_Data = DMD.DateUtils.Now();
                m_EventName = eventName;
                m_Utente = Users.CurrentUser;
                m_Descrizione = descrizione;
                m_Descrittore = oggetto;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="data"></param>
            /// <param name="eventName"></param>
            /// <param name="m"></param>
            /// <param name="u"></param>
            /// <param name="descrizione"></param>
            /// <param name="descrittore"></param>
            public EventDescription(DateTime data, string eventName, CModule m, CUser u, string descrizione, object descrittore)
            {
                m_Data = data;
                m_EventName = eventName;
                m_Module = m;
                m_Utente = u;
                m_Descrizione = descrizione;
                m_Descrittore = descrittore;
            }

            /// <summary>
            /// Parametri
            /// </summary>
            public CKeyCollection Parametri
            {
                get
                {
                    if (m_Parametri is null)
                        m_Parametri = new CKeyCollection();
                    return m_Parametri;
                }
            }

            /// <summary>
            /// Data dell'evento
            /// </summary>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }
            }

            /// <summary>
            /// Nome dell'evento
            /// </summary>
            public string EventName
            {
                get
                {
                    return m_EventName;
                }
            }

            /// <summary>
            /// Utente che ha generato l'evento
            /// </summary>
            public CUser Utente
            {
                get
                {
                    return m_Utente;
                }
            }

            /// <summary>
            /// Descrizione dell'evento
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// Oggetto associato all'evento
            /// </summary>
            public object Descrittore
            {
                get
                {
                    return m_Descrittore;
                }
            }

            /// <summary>
            /// Modulo su cui é stato generato l'evento
            /// </summary>
            public CModule Module
            {
                get
                {
                    return m_Module;
                }
            }

            /// <summary>
            /// Imposta il modulo
            /// </summary>
            /// <param name="value"></param>
            internal void SetModule(CModule value)
            {
                m_Module = value;
            }
        }

       
    }
}