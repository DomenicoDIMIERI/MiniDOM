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
using static minidom.Messenger;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CChatRoomUser"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed partial class CChatRoomUserRepository
            : CModulesClass<Messenger.CChatRoomUser>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatRoomUserRepository()
                : base("modChatUsersXRoom", typeof(Messenger.CChatRoomUserCursor), 0)
            {
            }
               
        }

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CChatRoom"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed partial class CChatRoomsClass
            
        {
            private CChatRoomUserRepository m_UsersXRoom;


            

            /// <summary>
            /// Restituisce un riferimento alla stanza principale
            /// </summary>
            public CChatRoomUserRepository UsersXRoom
            {
                get
                {
                    if (this.m_UsersXRoom is null) 
                        this.m_UsersXRoom = new CChatRoomUserRepository();
                    return this.m_UsersXRoom;
                }
            }
        }
    }

   
}