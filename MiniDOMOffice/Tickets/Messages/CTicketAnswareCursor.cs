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
        /// Cursore di <see cref="CTicketAnsware"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTicketAnswareCursor
            : minidom.Databases.DBObjectCursor<CTicketAnsware>
        {
            private DBCursorField<int> m_IDTicket = new DBCursorField<int>("IDTicket");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<TicketStatus> m_StatoTicket = new DBCursorField<TicketStatus>("StatoTicket");
            private DBCursorStringField m_Messaggio = new DBCursorStringField("Messaggio");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketAnswareCursor()
            {
            }

            /// <summary>
            /// IDTicket
            /// </summary>
            public DBCursorField<int> IDTicket
            {
                get
                {
                    return m_IDTicket;
                }
            }

            /// <summary>
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
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
            /// StatoTicket
            /// </summary>
            public DBCursorField<TicketStatus> StatoTicket
            {
                get
                {
                    return m_StatoTicket;
                }
            }

            /// <summary>
            /// Messaggio
            /// </summary>
            public DBCursorStringField Messaggio
            {
                get
                {
                    return m_Messaggio;
                }
            }

          
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Tickets.Messages;
            }
 
        }
    }
}