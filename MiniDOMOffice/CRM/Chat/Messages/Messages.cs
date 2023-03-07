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
using static minidom.Messenger;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CMessage"/>
        /// </summary>
        public sealed class CMessagesClass 
            : CModulesClass<Messenger.CMessage>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMessagesClass() 
                : base("modMessenger", typeof(Messenger.CMessagesCursor), 0)
            {
            }



            /// <summary>
            /// Invia un messaggio al destinatario specificato
            /// </summary>
            /// <param name="target">[in] Utente destinatario</param>
            /// <param name="message">[in] Corpo del messaggio da inviare</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Messenger.CMessage SendMessageToUser(string message, Sistema.CUser target)
            {
                var msg = new Messenger.CMessage();
                msg.Source = Sistema.Users.CurrentUser;
                msg.SourceDescription = Sistema.Users.CurrentUser.Nominativo;
                msg.Time = DMD.DateUtils.Now();
                msg.Target = target;
                msg.Message = message;
                // msg.SourceSession = WebSite.int.Session.ID
                msg.Stato = ObjectStatus.OBJECT_VALID;
                msg.Save();
                return msg;
            }

            /// <summary>
            /// Invia un messaggio a tutti i membri attivi in una stanza
            /// </summary>
            /// <param name="room">[in] Stanza</param>
            /// <param name="message">[in] Corpo del messaggio da inviare</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Messenger.CMessage> SendMessageToRoom(string message, Messenger.CChatRoom room)
            {
                var ret = new CCollection<Messenger.CMessage>();
                var t = DMD.DateUtils.Now();
                foreach (Messenger.CChatRoomUser u in room.GetMembers())
                {
                    if (u.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        var msg = new Messenger.CMessage();
                        msg.Source = Sistema.Users.CurrentUser;
                        msg.SourceDescription = Sistema.Users.CurrentUser.Nominativo;
                        msg.Time = t;
                        msg.Target = u.User;
                        msg.Message = message;
                        // msg.SourceSession = WebSite.int.Session.ID
                        msg.Stanza = room;
                        msg.Stato = ObjectStatus.OBJECT_VALID;
                        msg.Save();
                        ret.Add(msg);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Invia un messaggio al destinatario specificato
            /// </summary>
            /// <param name="targetID">[in] Utente destinatario</param>
            /// <param name="message">[in] Corpo del messaggio da inviare</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Messenger.CMessage SendMessagToUsere(string message, int targetID)
            {
                return SendMessageToUser(message, Sistema.Users.GetItemById(targetID));
            }

            /// <summary>
            /// Conta il numero di utenti online
            /// </summary>
            /// <returns></returns>
            public int CountOnlineUsers()
            {
                // return Sistema.Formats.ToInteger(Databases.APPConn.ExecuteScalar("SELECT Count(*) FROM [tbl_Users] WHERE ([Visible]=True) And ([Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + ") And [IsLogged]=True"));
                //TODO CountOnlineUsers
                return -1;
            }


            /// <summary>
        /// Restituisce l'elenco dei messaggi inviati all'utente
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Messenger.CMessage> LoadPrevUserMessages(
                                        Sistema.CUser fromUserO, 
                                        Sistema.CUser toUserO, 
                                        int fromID, 
                                        int nMax
                                        )
            {
                if (fromUserO is null)
                    throw new ArgumentNullException("fromUser");
                if (toUserO is null)
                    throw new ArgumentNullException("toUser");
                var ret = new CCollection<Messenger.CMessage>();
                int fromUser = DBUtils.GetID(fromUserO, 0);
                int toUser = DBUtils.GetID(toUserO, 0);
                using (var cursor = new Messenger.CMessagesCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ID.Value = fromID;
                    cursor.ID.Operator = OP.OP_LT;
                    cursor.ID.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    cursor.IDStanza.Value = 0;
                    cursor.WhereClauses *= cursor.Field("SourceID").In( new int[] { fromUser, toUser } ) * cursor.Field("TargetID").In( new int[] { fromUser, toUser });
                    while (cursor.Read() && ret.Count < nMax)
                    {
                        ret.Add(cursor.Item);
                    }

                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'elenco dei messaggi inviati al gruppo
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Messenger.CMessage> LoadPrevRoomMessages(
                                            Sistema.CUser user, 
                                            Messenger.CChatRoom roomO, 
                                            int fromID, 
                                            int nMax
                                            )
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                if (roomO is null)
                    throw new ArgumentNullException("room");
                var ret = new CCollection<Messenger.CMessage>();
                int rID = DBUtils.GetID(roomO, 0);
                using (var cursor = new Messenger.CMessagesCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ID.Value = fromID;
                    cursor.ID.Operator = OP.OP_LT;
                    cursor.ID.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    cursor.IDStanza.Value = rID;
                    cursor.TargetID.Value = DBUtils.GetID(user, 0);
                    while (cursor.Read() && ret.Count < nMax)
                    {
                        ret.Add(cursor.Item);
                    }
                }
                return ret;
            }

            /// <summary>
            /// Restituisce l'elenco dei messaggi inviati all'utente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Messenger.CMessage> LoadUserMessages(Sistema.CUser fromUserO, Sistema.CUser toUserO, DateTime? fromDate, DateTime? toDate)
            {
                if (fromUserO is null)
                    throw new ArgumentNullException("fromUser");
                if (toUserO is null)
                    throw new ArgumentNullException("toUser");
                var ret = new CCollection<Messenger.CMessage>();
                Messenger.CMessage msg;
                int fromUser = DBUtils.GetID(fromUserO, 0);
                int toUser = DBUtils.GetID(toUserO, 0);

                using (var cursor = new Messenger.CMessagesCursor())
                {
                    cursor.WhereClauses *= cursor.Field("SourceID").In(new int[] { fromUser, toUser })
                                        *  cursor.Field("[TargetID").In(new int[] { fromUser, toUser });
                    // cursor.Time.SortOrder = SortEnum.SORT_ASC
                    // cursor.ID.SortOrder = SortEnum.SORT_ASC
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (fromDate.HasValue)
                    {
                        cursor.Time.Value = fromDate.Value;
                        cursor.Time.Operator = OP.OP_GE;
                        if (toDate.HasValue)
                        {
                            cursor.Time.Value1 = toDate.Value;
                            cursor.Time.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (toDate.HasValue)
                    {
                        cursor.Time.Value = toDate.Value;
                        cursor.Time.Operator = OP.OP_LE;
                    }

                    cursor.IDStanza.Value = 0;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        msg = cursor.Item;
                        ret.Add(msg);
                    }

                    cursor.Reset1();
                    if (ret.Count < 30)
                    {
                        cursor.Time.Clear();
                        // cursor.Time.SortOrder = SortEnum.SORT_DESC
                        cursor.ID.SortOrder = SortEnum.SORT_DESC;
                        while (ret.Count < 30 && cursor.Read())
                        {
                            msg = cursor.Item;
                            if (ret.GetItemById(DBUtils.GetID(msg)) is null)
                                ret.Add(msg);
                        }
                    }

                }
                return ret;
            }

            /// <summary>
            /// Restituisce l'elenco dei messaggi inviati al gruppo
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Messenger.CMessage> LoadRoomMessages(
                                                Sistema.CUser user,
                                                Messenger.CChatRoom roomO,
                                                DateTime? fromDate,
                                                DateTime? toDate
                                                )
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                if (roomO is null)
                    throw new ArgumentNullException("room");
                var ret = new CCollection<Messenger.CMessage>();
                Messenger.CMessage msg;
                int rid = DBUtils.GetID(roomO, 0);

                using (var cursor = new Messenger.CMessagesCursor())
                { 
                    cursor.IDStanza.Value = rid;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.TargetID.Value = DBUtils.GetID(user, 0);
                    if (fromDate.HasValue)
                    {
                        cursor.Time.Value = fromDate.Value;
                        cursor.Time.Operator = OP.OP_GE;
                        if (toDate.HasValue)
                        {
                            cursor.Time.Value1 = toDate.Value;
                            cursor.Time.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (toDate.HasValue)
                    {
                        cursor.Time.Value = toDate.Value;
                        cursor.Time.Operator = OP.OP_LE;
                    }

                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        msg = cursor.Item;
                        ret.Add(msg);
                    }

                    cursor.Reset1();
                    if (ret.Count < 30)
                    {
                        cursor.Time.Clear();
                        // cursor.Time.SortOrder = SortEnum.SORT_DESC
                        cursor.ID.SortOrder = SortEnum.SORT_DESC;
                        while (ret.Count < 30 && cursor.Read())
                        {
                            msg = cursor.Item;
                            if (ret.GetItemById(DBUtils.GetID(msg)) is null)
                                ret.Add(msg);
                        }
                    }

                }
                return ret;
            }

            /// <summary>
            /// Restituisce la collezione dei nuovi messaggi
            /// </summary>
            /// <param name="newerThan"></param>
            /// <param name="fromUserO"></param>
            /// <param name="toUserO"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public CCollection<Messenger.CMessage> GetNewerOrChangedUserMessages(
                                                        int newerThan, 
                                                        Sistema.CUser fromUserO, 
                                                        Sistema.CUser toUserO, 
                                                        DateTime? fromDate, 
                                                        DateTime? toDate
                                                        )
            {
                
                if (fromUserO is null)
                    throw new ArgumentNullException("fromUser");
                if (toUserO is null)
                    throw new ArgumentNullException("toUser");

                var ret = new CCollection<CMessage>();

                int fromUser = DBUtils.GetID(fromUserO, 0);
                int toUser = DBUtils.GetID(toUserO, 0);

                using (var cursor = new CMessagesCursor())
                {
                    cursor.WhereClauses *= cursor.Field("SourceID").In(new int[] { fromUser, toUser })
                                        * cursor.Field("[TargetID").In(new int[] { fromUser, toUser });
                    cursor.Time.SortOrder = SortEnum.SORT_ASC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStanza.Value = 0;
                    if (fromDate.HasValue)
                    {
                        cursor.Time.Value = fromDate.Value;
                        cursor.Time.Operator = OP.OP_GE;
                        if (toDate.HasValue)
                        {
                            cursor.Time.Value1 = toDate.Value;
                            cursor.Time.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (toDate.HasValue)
                    {
                        cursor.Time.Value = toDate.Value;
                        cursor.Time.Operator = OP.OP_LE;
                    }

                    cursor.IgnoreRights = true;
                    cursor.ID.Value = newerThan;
                    cursor.ID.Operator = OP.OP_GT;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                }

                return ret;
            }

            /// <summary>
            /// Restituisce la collezione dei nuovi messaggi
            /// </summary>
            /// <param name="newerThan"></param>
            /// <param name="user"></param>
            /// <param name="roomO"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public CCollection<Messenger.CMessage> GetNewerOrChangedRoomMessages(
                                        int newerThan, 
                                        Sistema.CUser user, 
                                        Messenger.CChatRoom roomO, 
                                        DateTime? fromDate, 
                                        DateTime? toDate
                                        )
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                if (roomO is null)
                    throw new ArgumentNullException("room");
                
                int rid = DBUtils.GetID(roomO, 0);

                var ret = new CCollection<Messenger.CMessage>();
                
                using(var cursor = new CMessagesCursor())
                {    
                    cursor.IDStanza.Value = rid;
                    cursor.Time.SortOrder = SortEnum.SORT_ASC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.TargetID.Value = DBUtils.GetID(user, 0);
                    if (fromDate.HasValue)
                    {
                        cursor.Time.Value = fromDate.Value;
                        cursor.Time.Operator = OP.OP_GE;
                        if (toDate.HasValue)
                        {
                            cursor.Time.Value1 = toDate.Value;
                            cursor.Time.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (toDate.HasValue)
                    {
                        cursor.Time.Value = toDate.Value;
                        cursor.Time.Operator = OP.OP_LE;
                    }

                    cursor.IgnoreRights = true;
                    cursor.ID.Value = newerThan;
                    cursor.ID.Operator = OP.OP_GT;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                }

                return ret;
            }


            /// <summary>
            /// Restituisce un oggetto CCollection di CChatUser contenente tutti gli utenti abilitati a ricevere messaggi
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Messenger.CChatUser> GetUsersList()
            {
                var ret = new Dictionary<int, CChatUser>();
                var users = Sistema.Users.LoadAll();
                CChatUser item;
                foreach (var u in users)
                {
                    if (u.UserStato == Sistema.UserStatus.USER_ENABLED)
                    {
                        item = new Messenger.CChatUser();
                        item.uID = DBUtils.GetID(u, 0);
                        item.UserName = u.UserName;
                        item.IconURL = u.IconURL;
                        item.DisplayName = u.Nominativo;
                        item.IsOnline = u.IsLogged();
                        if (u.CurrentLogin is object)
                            item.UltimoAccesso = u.CurrentLogin.LogInTime;
                        ret.Add(DBUtils.GetID(u, 0), item);
                    }
                }

                //TODO convertire in comandi al alto livello
                var dbSQL = "SELECT [SourceID], Count(*) As [NonLetti] FROM [tbl_Messenger] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " And [StatoMessaggio] In (" + ((int)Messenger.StatoMessaggio.NonConsegnato).ToString() + ", " + ((int)Messenger.StatoMessaggio.NonLetto).ToString() + ") And [Stanza]<>'' And Not [Stanza] Is Null GROUP BY [SourceID]";
                using (var dbRis = this.Database.ExecuteReader(dbSQL))
                {
                    while (dbRis.Read())
                    {
                        int sourceID = DBUtils.Read(dbRis, "SourceID", 0);
                        item = ret[sourceID];
                        if (item is object)
                            item.MessaggiNonLetti = DBUtils.Read(dbRis, "NonLetti", 0);
                    }
                }
                return new CCollection<Messenger.CChatUser>(ret);
            }

            /// <summary>
            /// Restituisce la collezione dei messaggi non letti
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="user"></param>
            /// <returns></returns>
            public object GetUnreadMessages(DateTime? fromDate, DateTime? toDate, Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                
                var ret = new CCollection<Messenger.CMessage>();

                using (var cursor = new Messenger.CMessagesCursor())
                {
                    cursor.TargetID.Value = DBUtils.GetID(user, 0);
                    cursor.StatoMessaggio.ValueIn(new[] { StatoMessaggio.NonConsegnato, StatoMessaggio.NonLetto });
                    cursor.Time.SortOrder = SortEnum.SORT_DESC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (fromDate.HasValue)
                    {
                        cursor.Time.Value = fromDate.Value;
                        cursor.Time.Operator = OP.OP_GE;
                        if (toDate.HasValue)
                        {
                            cursor.Time.Value1 = toDate.Value;
                            cursor.Time.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (toDate.HasValue)
                    {
                        cursor.Time.Value = toDate.Value;
                        cursor.Time.Operator = OP.OP_LE;
                    }

                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }


                }

                return ret;
            }
        }
    }

    public sealed partial class Messenger
    {
        private static CMessagesClass m_Messages = null;


        /// <summary>
        /// Repository di oggetti <see cref="CMessage"/>
        /// </summary>
        public static CMessagesClass Messages
        {
            get
            {
                if (m_Messages is null)
                    m_Messages = new CMessagesClass();
                return m_Messages;
            }
        }
         
    }
}