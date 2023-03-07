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
        /// Collezione dei moduli definiti per il gruppo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXGroupCollection
            : CCollection<CModuleXGroup>
        {

            [NonSerialized] private CGroup m_Group;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXGroupCollection()
            {
                m_Group = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="Group"></param>
            public CModuleXGroupCollection(CGroup Group) : this()
            {
                Load(Group);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="Group"></param>
            internal void Load(CGroup Group)
            {
                if (Group is null)
                    throw new ArgumentNullException("Group");
                Clear();
                m_Group = Group;
                using (var cursor = new CModuleXGroupCursor())
                {
                    cursor.GroupID.Value = DBUtils.GetID(Group, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CModuleXGroup value)
            {
                if (m_Group is object)
                    value.SetGroup(this.m_Group);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CModuleXGroup oldValue, CModuleXGroup newValue)
            {
                if (this.m_Group is object)
                    newValue.SetGroup(this.m_Group);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Restituisce l'elemento in base al modulo
            /// </summary>
            /// <param name="module"></param>
            /// <returns></returns>
            public CModuleXGroup GetItemByModule(CModule module)
            {
                foreach (CModuleXGroup item in this)
                {
                    if (ReferenceEquals(item.Module, module))
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Imposta le autorizzazioni per il modulo
            /// </summary>
            /// <param name="module"></param>
            /// <param name="allow"></param>
            /// <param name="negate"></param>
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
                    item = new CModuleXGroup();
                    item.Module = module;
                    item.Group = m_Group;
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                    Add(item);
                }
            }

            /// <summary>
            /// Restituisce le impostazioni allow negate per il modulo
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