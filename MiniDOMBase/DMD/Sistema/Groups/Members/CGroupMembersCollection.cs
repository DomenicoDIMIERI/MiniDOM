using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Collezione di utenti appartenenti ad un gruppo
        /// </summary>
        [Serializable]
        public class CGroupMembersCollection 
            : CCollection<CUser>
        {

            [NonSerialized] private CGroup m_Group;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupMembersCollection()
            {
                m_Group = null;
                Comparer = new CUserComparer();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="group"></param>
            public CGroupMembersCollection(CGroup group)
                : this()
            {
                Initialize(group);
            }


            /// <summary>
            /// Restituisce il gruppo
            /// </summary>
            public CGroup Group
            {
                get
                {
                    return m_Group;
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="group"></param>
            /// <returns></returns>
            internal bool Initialize(CGroup group)
            {
                if (group is null)
                    throw new ArgumentNullException("group");

                Clear();

                m_Group = group;
                if (DBUtils.GetID(group, 0) == 0) 
                    return true;

                using (var cursor = new CUserXGroupCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.GroupID.Value = DBUtils.GetID(group, 0);
                    while (cursor.Read())
                    {
                        var u = cursor.Item.User;
                        if (u is object)
                            base.Add(cursor.Item.User);
                        else
                        {
                            minidom.Sistema.ApplicationContext.Log(
                                        "Invalid CUserXGroup: " + DBUtils.GetID(cursor.Item, 0), 
                                        WarningSeverity.WARNING
                                        );
                        }
                    }
                }
                this.Sort();
                return true;
            }

            /// <summary>
            /// Restituisce l'indice dell'utente con l'ID specificato
            /// </summary>
            /// <param name="userID"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IndexOf(int userID)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if (DBUtils.GetID(this[i], 0) == userID)
                        return i;
                }

                return -1;
            }

            /// <summary>
            /// Restituisce l'indice dell'utente con l'ID specificato
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public new int IndexOf(CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                return this.IndexOf(DBUtils.GetID(user, 0));
            }

            /// <summary>
            /// Restituisce un valore booleano che indica se l'utente appartiene a questo gruppo
            /// </summary>
            /// <param name="userID"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Contains(int userID)
            {
                return this.IndexOf(userID) >= 0;
            }

            /// <summary>
            /// Restituisce true l'utente appartiene al gruppo
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public new bool Contains(CUser user)
            {
                return this.IndexOf(user) >= 0;
            }

            /// <summary>
            /// Rimuove l'utente specificato da questo gruppo
            /// </summary>
            /// <param name="userID"></param>
            /// <remarks></remarks>
            public void Remove(int userID)
            {
                this.Remove(minidom.Sistema.Users.GetItemById(userID));
            }

            /// <summary>
            /// Rimuove l'utente
            /// </summary>
            /// <param name="user"></param>
            public new void Remove(CUser user)
            {
                lock (user)
                {
                    int i = IndexOf(user);
                    base.RemoveAt(i);
                    using (var cursor = new CUserXGroupCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.GroupID.Value = DBUtils.GetID(this.m_Group, 0);
                        while (cursor.Read())
                        {
                            var item = cursor.Item;
                            if (item.UserID == DBUtils.GetID(user, 0))
                            {
                                item.Delete();
                                if (user.Groups.Contains(this.m_Group))
                                    user.Groups.Remove(this.m_Group);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Aggiunte l'utente specificato da questo gruppo
            /// </summary>
            /// <param name="value"></param>
            /// <remarks></remarks>
            public new void Add(CUser value)
            {
                lock (value)
                {
                    if (value is null)
                        throw new ArgumentNullException("user");
                    if (this.m_Group is null)
                        throw new ArgumentNullException("group");

                    var item = new CUserXGroup();
                    item.Group = this.m_Group;
                    item.User = value;
                    item.Save();
                    base.Add(value);
                    if (!value.Groups.Contains(this.m_Group))
                        value.Groups.Add(this.m_Group);
                }
            }

            /// <summary>
            /// Aggiunge l'utente
            /// </summary>
            /// <param name="UserID"></param>
            public void Add(int UserID)
            {
                this.Add(minidom.Sistema.Users.GetItemById(UserID));
            }
        }
    }
}