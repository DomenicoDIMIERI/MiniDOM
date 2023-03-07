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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Collezione dei moduli definiti per l'utente
        /// </summary>
        /// <remarks></remarks>
        public sealed class CModuleXUserCollection 
            : CCollection<CModuleXUser>
        {

            [NonSerialized] private CUser m_User;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXUserCollection()
            {
                m_User = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="User"></param>
            public CModuleXUserCollection(CUser User) : this()
            {
                Load(User);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="User"></param>
            internal void Load(CUser User)
            {
                if (User is null)
                    throw new ArgumentNullException("User");
                Clear();
                m_User = User;

                using (var cursor = new CModuleXUserCursor())
                { 
                    cursor.UserID.Value = DBUtils.GetID(User, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// Restituisce l'elemento in base al modulo
            /// </summary>
            /// <param name="module"></param>
            /// <returns></returns>
            public CModuleXUser GetItemByModule(CModule module)
            {
                foreach (CModuleXUser item in this)
                {
                    if (ReferenceEquals(item.Module, module))
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Imposta le autorizzazioni sul modulo
            /// </summary>
            /// <param name="module"></param>
            /// <param name="allow"></param>
            public void SetAllowNegate(CModule module, bool allow, bool negate)
            {
                var item = GetItemByModule(module);
                if (item is object)
                {
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                }
                else
                {
                    item = new CModuleXUser();
                    item.Module = module;
                    item.User = m_User;
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                    Add(item);
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CModuleXUser oldValue, CModuleXUser newValue)
            {
                if (this.m_User is object)
                    newValue.SetUser(this.m_User);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CModuleXUser value)
            {
                if (this.m_User is object)
                    value.SetUser(this.m_User);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// Restituisce le autorizzazioni
            /// </summary>
            /// <param name="module"></param>
            /// <param name="a"></param>
            /// <param name="n"></param>
            public void GetAllowNegate(CModule module, ref bool a, ref bool n)
            {
                var item = GetItemByModule(module);
                if (item is object)
                {
                    a = a | item.Allow;
                    n = n | item.Negate;
                }
            }
        }
    }
}