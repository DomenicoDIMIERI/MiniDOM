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
    public partial class Messenger
    {

        /// <summary>
        /// Cursore sulla tabella delle stanze
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CChatRoomCursor 
            : minidom.Databases.DBObjectCursor<CChatRoom>
        {
            
            private DBCursorStringField m_Name = new DBCursorStringField("Name");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatRoomCursor()
            {
            }

            /// <summary>
            /// Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Messenger.Rooms;
            }
             
        }
    }
}