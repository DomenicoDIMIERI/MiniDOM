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
        public class CUserAllowNegateCollection<T, Titem, Tcursor>
            : CCollection<Titem>
            where T : class
            where Titem : UserAllowNegate<T>, new()
            where Tcursor : UserAllowNegateCursor<T>, new()
        {

            [NonSerialized] private T m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserAllowNegateCollection()
            {
                m_Owner = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CUserAllowNegateCollection(T owner)
                : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                this.Load(owner);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void Load(T owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_Owner = owner;
                using (var cursor = new Tcursor())
                {
                    cursor.UserID.Value = DBUtils.GetID(owner, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }

                }
            }

            /// <summary>
            /// Restituisce l'oggetto associato all'elemento
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public virtual Titem GetItemByUser(CUser user)
            {
                var userID = DBUtils.GetID(user, 0);
                foreach (var o in this)
                {
                    if (o.UserID == userID)
                        return o;
                }

                return null;
            }

            /// <summary>
            /// Imposta le autorizzazioni per l'azione
            /// </summary>
            /// <param name="user"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public virtual Titem SetAllowNegate(CUser user, bool allow, bool negate)
            {
                var item = this.GetItemByUser(user);
                if (item is object)
                {
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                }
                else
                {
                    item = new Titem();
                    item.Item = this.m_Owner;
                    item.User = user;
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
            /// <param name="user"></param>
            /// <param name="a"></param>
            /// <param name="n"></param>
            public virtual void GetAllowNegate(CUser user, ref bool a, ref bool n)
            {
                var item = GetItemByUser(user);
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
            protected override void OnSet(
                                int index, 
                                Titem oldValue,
                                Titem newValue
                                )
            {
                if (m_Owner is object)
                    newValue.SetItem(m_Owner);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(
                                        int index,
                                        Titem value
                                        )
            {
                if (m_Owner is object)
                    value.SetItem(m_Owner);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="owner"></param>
            protected internal void SetOwner(T owner)
            {
                m_Owner = owner;
                if (owner is object)
                {
                    foreach (var item in this)
                        item.SetItem(owner);
                }
            }
        }
    }
}