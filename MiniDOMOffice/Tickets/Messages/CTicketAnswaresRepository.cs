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
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="CTicketAnsware"/>
        /// </summary>
        [Serializable]
        public class CTicketAnswaresRepository
            : CModulesClass<CTicketAnsware>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketAnswaresRepository()
                : base("modOfficeTicketAnswers", typeof(CTicketAnswareCursor), 0)
            {

            }
        }

        /// <summary>
        /// Collezione delle risposte per un ticket
        /// </summary>
        /// <remarks></remarks>
        public partial class CTicketsClass
        {

            [NonSerialized] private CTicketAnswaresRepository m_Messages = null;

            /// <summary>
            /// Repository di <see cref="CTicketAnsware"/>
            /// </summary>
            public CTicketAnswaresRepository Messages
            {
                get
                {
                    if (m_Messages is null)
                        m_Messages = new CTicketAnswaresRepository();
                    return m_Messages;
                }
            }


        }
    }
}