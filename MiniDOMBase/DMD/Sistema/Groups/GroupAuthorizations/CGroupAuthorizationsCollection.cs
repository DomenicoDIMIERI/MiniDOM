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
        /// Collezione delle autorizzazioni di gruppo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CGroupAuthorizationCollection
            : CCollection<CGroupAuthorization>
        {
            
            [NonSerialized] private CGroup m_Group;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupAuthorizationCollection()
            {
                m_Group = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="Group"></param>
            public CGroupAuthorizationCollection(CGroup Group) : this()
            {
                Load(Group);
            }

            /// <summary>
            /// Carica gli elementi
            /// </summary>
            /// <param name="Group"></param>
            internal void Load(CGroup Group)
            {
                if (Group is null)
                    throw new ArgumentNullException("Group");
                Clear();
                m_Group = Group;

                using (var cursor = new CGroupAuthorizationCursor())
                {
                    cursor.GroupID.Value = DBUtils.GetID(Group, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        var rel = cursor.Item;
                        rel.SetGroup(this.m_Group);
                        Add(rel);                        
                    }
                }
                 
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CGroupAuthorization value)
            {
                if (this.m_Group is object)
                    value.SetGroup(this.m_Group);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CGroupAuthorization oldValue, CGroupAuthorization newValue)
            {
                if (this.m_Group is object)
                    newValue.SetGroup(this.m_Group);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Restituisce l'elemento corrispondente all'azione
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public CGroupAuthorization GetItemByAction(CModuleAction action)
            {
                foreach (CGroupAuthorization Item in this)
                {
                    if (ReferenceEquals(Item.Action, action))
                        return Item;
                }

                return null;
            }

            /// <summary>
            /// Imposta i parametri dell'azione
            /// </summary>
            /// <param name="action"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
            /// <returns></returns>
            public CGroupAuthorization SetAllowNegate(CModuleAction action, bool allow, bool negate)
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
                    item = new CGroupAuthorization();
                    item.Action = action;
                    item.Group = m_Group;
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                    Add(item);
                }

                return item;
            }

            /// <summary>
            /// Restituisce true se l'azione è abilitata 
            /// </summary>
            /// <param name="action"></param>
            /// <param name="a"></param>
            /// <param name="n"></param>
            public void GetAllowNegate(CModuleAction action, ref bool a, ref bool n)
            {
                var item = GetItemByAction(action);
                if (item is object)
                {
                    a = a || item.Allow;
                    n = n || item.Negate;
                }
            }
        }
    }
}