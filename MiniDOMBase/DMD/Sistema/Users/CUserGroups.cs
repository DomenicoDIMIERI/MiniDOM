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
using System.ComponentModel;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Collezione di gruppi a cui appartiene un utente
        /// </summary>
        [Serializable]
        public class CUserGroups 
            : CCollection<CGroup>
        {
            
            [NonSerialized] private CUser m_User;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserGroups()
            {
                m_User = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="user"></param>
            public CUserGroups(CUser user) : this()
            {
                Load(user);
            }


            /// <summary>
            /// Restituisce un riferimento all'utente
            /// </summary>
            [Browsable(false)]
            public CUser User
            {
                get
                {
                    return m_User;
                }
            }

          
            /// <summary>
            /// Carica 
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            protected internal bool Load(CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                Clear();
                m_User = user;

                using (var cursor = new CUserXGroupCursor())
                { 
                    cursor.UserID.Value = DBUtils.GetID(user, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        if (cursor.Item.Group is object)
                            Add(cursor.Item.Group);
                    }
                }

                return true;
            }

            /// <summary>
            /// Restituisce l'indice base 0dell'elemento corrispondente al gruppo
            /// </summary>
            /// <param name="groupName"></param>
            /// <returns></returns>
            public int IndexOf(string groupName)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if ((this[i].GroupName ?? "") == (groupName ?? ""))
                        return i;
                }

                return -1;
            }

            /// <summary>
            /// Restituisce true se la collezione contiene un gruppo con il nome specifico
            /// </summary>
            /// <param name="groupName"></param>
            /// <returns></returns>
            public bool Contains(string groupName)
            {
                return this.IndexOf(groupName) >= 0;
            }
        }
    }
}