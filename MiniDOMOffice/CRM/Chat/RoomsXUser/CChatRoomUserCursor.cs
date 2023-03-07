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
        /// Cursore di oggetti <see cref="CChatRoomUser"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CChatRoomUserCursor 
            : minidom.Databases.DBObjectCursor<CChatRoomUser>
        {
            private DBCursorField<ChatUserFlags> m_Flags = new DBCursorField<ChatUserFlags>("Flags");
            private DBCursorField<int> m_IDStanza = new DBCursorField<int>("IDStanza");
            private DBCursorField<int> m_UserID = new DBCursorField<int>("UserID");
            private DBCursorField<DateTime> m_LastVisit = new DBCursorField<DateTime>("LastVisit");
            private DBCursorField<DateTime> m_FirstVisit = new DBCursorField<DateTime>("FirstVisit");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatRoomUserCursor()
            {
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<ChatUserFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// IDStanza
            /// </summary>
            public DBCursorField<int> IDStanza
            {
                get
                {
                    return m_IDStanza;
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
            /// FirstVisit
            /// </summary>
            public DBCursorField<DateTime> FirstVisit
            {
                get
                {
                    return m_FirstVisit;
                }
            }

            /// <summary>
            /// LastVisit
            /// </summary>
            public DBCursorField<DateTime> LastVisit
            {
                get
                {
                    return m_LastVisit;
                }
            }
             

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Messenger.Rooms.UsersXRoom;
            }

             
        }
    }
}