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
        /// Cursore sulla tabella degli eventi
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CEventsCursor 
            : minidom.Databases.DBObjectCursorBase<CEventLog>
        {

            private DBCursorField<DateTime> m_Data;      // Data e ora in cui si è verificato l'evento
            private DBCursorStringField m_Source;     // Nome del modulo che ha generato l'evento
            private DBCursorField<int> m_UserID; // ID dell'utente nel cui contesto si è verificato l'evento
            private DBCursorStringField m_UserName; // Utente nel cui contesto si è verificato l'evento	
            private DBCursorStringField m_EventName; // Nome dell'evento
            private DBCursorStringField m_Description; // Descrizione dell'evento
            private DBCursorStringField m_Parameters; // Parametri dell'evento

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEventsCursor()
            {
                m_Data = new DBCursorField<DateTime>("Data");
                m_Source = new DBCursorStringField("Source");
                m_UserID = new DBCursorField<int>("User");
                m_UserName = new DBCursorStringField("UserName");
                m_EventName = new DBCursorStringField("EventName");
                m_Description = new DBCursorStringField("Description");
                m_Parameters = new DBCursorStringField("Parameters");
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CEventLog InstantiateNewT(DBReader dbRis)
            {
                return new CEventLog();
            }
 
         

          
            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            /// <summary>
            /// Source
            /// </summary>
            public DBCursorStringField Source
            {
                get
                {
                    return m_Source;
                }
            }

            /// <summary>
            /// UserID
            /// </summary>
            public DBCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
                }
            }

            /// <summary>
            /// UserName
            /// </summary>
            public DBCursorStringField UserName
            {
                get
                {
                    return m_UserName;
                }
            }

            /// <summary>
            /// EventName
            /// </summary>
            public DBCursorStringField EventName
            {
                get
                {
                    return m_EventName;
                }
            }

            /// <summary>
            /// Description
            /// </summary>
            public DBCursorStringField Description
            {
                get
                {
                    return m_Description;
                }
            }

            /// <summary>
            /// Parameters
            /// </summary>
            public DBCursorStringField Parameters
            {
                get
                {
                    return m_Parameters;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Events;
            }
        }
    }
}