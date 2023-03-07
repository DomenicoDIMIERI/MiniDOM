using DMD;
using DMD.Databases;
using static minidom.Messenger;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CChatRoom"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CChatRoomsClass
            : CModulesClass<Messenger.CChatRoom>
        {
            private Messenger.CChatRoom m_MainRoom;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatRoomsClass() 
                : base("modChatRooms", typeof(Messenger.CChatRoomCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce la stanza con nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Messenger.CChatRoom GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                foreach (Messenger.CChatRoom room in LoadAll())
                {
                    if (room.Stato == ObjectStatus.OBJECT_VALID && DMD.Strings.Compare(room.Name, name, true) == 0)
                        return room;
                }

                return null;
            }

            /// <summary>
            /// Restituisce un riferimento alla stanza principale
            /// </summary>
            public Messenger.CChatRoom MainRoom
            {
                get
                {
                    if (m_MainRoom is null)
                    {
                        m_MainRoom = GetItemByName("main");
                    }

                    if (m_MainRoom is null)
                    {
                        m_MainRoom = new Messenger.CChatRoom();
                        m_MainRoom.Name = "main";
                        m_MainRoom.Stato = ObjectStatus.OBJECT_VALID;
                        m_MainRoom.Save();
                        foreach (Sistema.CUser u in Sistema.Users.LoadAll())
                        {
                            if (u.Stato == ObjectStatus.OBJECT_VALID)
                            {
                                m_MainRoom.AddMember(u);
                            }
                        }
                    }

                    return m_MainRoom;
                }
            }
        }
    }

    public partial class Messenger
    {

        private static CChatRoomsClass m_Rooms = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CChatRoom"/>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CChatRoomsClass Rooms
        {
            get
            {
                if (m_Rooms is null)
                    m_Rooms = new CChatRoomsClass();
                return m_Rooms;
            }
        }
    }
}