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
        /// Collezione delle risposte per un ticket
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTicketAnswaresCollection 
            : CCollection<CTicketAnsware>
        {
            [NonSerialized] private CTicket m_Ticket;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketAnswaresCollection()
            {
                m_Ticket = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="ticket"></param>
            public CTicketAnswaresCollection(CTicket ticket)
                : this()
            {
                Load(ticket);
            }

            /// <summary>
            /// Owner
            /// </summary>
            public CTicket Ticket
            {
                get
                {
                    return m_Ticket;
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CTicketAnsware oldValue, CTicketAnsware newValue)
            {
                if (this.m_Ticket is object)
                    newValue.SetTicket(this.m_Ticket);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CTicketAnsware value)
            {
                if (m_Ticket is object)
                    value.SetTicket(m_Ticket);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="value"></param>
            protected internal void Load(CTicket value)
            {
                if (value is null)
                    throw new ArgumentNullException("ticket");
                Clear();
                m_Ticket = value;
                if (DBUtils.GetID(value, 0) == 0)
                    return;
                using (var cursor = new CTicketAnswareCursor())
                {
                    cursor.IDTicket.Value = DBUtils.GetID(value, 0);
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }

                }

                Sort();
            }
        }
    }
}