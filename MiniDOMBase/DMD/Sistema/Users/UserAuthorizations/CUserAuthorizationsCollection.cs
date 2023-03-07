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
        /// Collezione delle autorizzazioni utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CUserAuthorizationCollection
            : CCollection<CUserAuthorization>
        {
            [NonSerialized] private CUser m_User;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserAuthorizationCollection()
            {
                m_User = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="user"></param>
            public CUserAuthorizationCollection(CUser user) : this()
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                this.Load(user);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="user"></param>
            internal void Load(CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                Clear();
                m_User = user;
                using (var cursor = new CUserAuthorizationCursor())
                {
                    cursor.UserID.Value = DBUtils.GetID(user, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }

                }
            }

            /// <summary>
            /// Restituisce l'oggetto associato all'azione
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public CUserAuthorization GetItemByAction(CModuleAction action)
            {
                foreach (CUserAuthorization Item in this)
                {
                    if (ReferenceEquals(Item.Action, action))
                        return Item;
                }

                return null;
            }

            /// <summary>
            /// Imposta le autorizzazioni per l'azione
            /// </summary>
            /// <param name="action"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public CUserAuthorization SetAllowNegate(CModuleAction action, bool allow, bool negate)
            {
                var item = GetItemByAction(action);
                if (item is object)
                {
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                }
                else
                {
                    item = new CUserAuthorization();
                    item.Action = action;
                    item.User = m_User;
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                    Add(item);
                }

                return item;
            }

            /// <summary>
            /// Recupera il valore dell'azione
            /// </summary>
            /// <param name="action"></param>
            /// <param name="a"></param>
            /// <param name="n"></param>
            public void GetAllowNegate(CModuleAction action, ref bool a, ref bool n)
            {
                var item = GetItemByAction(action);
                if (item is object)
                {
                    a = a | item.Allow;
                    a = a | item.Negate;
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CUserAuthorization oldValue, CUserAuthorization newValue)
            {
                if (m_User is object)
                    newValue.SetUser(m_User);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CUserAuthorization value)
            {
                if (m_User is object)
                    value.SetUser(m_User);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="user"></param>
            internal void SetUser(CUser user)
            {
                m_User = user;
                if (user is object)
                {
                    foreach (CUserAuthorization item in this)
                        item.SetUser(user);
                }
            }
        }
    }
}