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

namespace minidom.internals
{

    /// <summary>
    /// Collezione di allegati figlio
    /// </summary>
    [Serializable]
    public class CAttachmentChilds 
        : CCollection<Sistema.CAttachment>
    {
        [NonSerialized] private Sistema.CAttachment m_Parent;

        /// <summary>
        /// Costruttore
        /// </summary>
        public CAttachmentChilds()
        {
            m_Parent = null;
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="parent"></param>
        public CAttachmentChilds(Sistema.CAttachment parent) : this()
        {
            Load(parent);
        }

        /// <summary>
        /// OnInsert
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnInsert(int index, CAttachment value)
        {
            if (m_Parent is object)
                value.SetParent(m_Parent);

            base.OnInsert(index, value);
        }

        /// <summary>
        /// OnSet
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnSet(int index, CAttachment oldValue, CAttachment newValue)
        {
            if (m_Parent is object)
                newValue.SetParent(m_Parent);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Imposta il genitore
        /// </summary>
        /// <param name="value"></param>
        protected internal void SetParent(Sistema.CAttachment value)
        {
            m_Parent = value;
            if (value is object)
            {
                foreach (Sistema.CAttachment a in this)
                    a.SetParent(value);
            }
        }

        /// <summary>
        /// Carica gli oggetti
        /// </summary>
        /// <param name="parent"></param>
        protected void Load(Sistema.CAttachment parent)
        {
            if (parent is null)
                throw new ArgumentNullException("parent");
            Clear();
            this.m_Parent = parent;
            if (DBUtils.GetID(parent, 0) == 0)
                return;

            using (var cursor = new Sistema.CAttachmentsCursor())
            { 
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.ParentID.Value = DBUtils.GetID(parent, 0);
                cursor.IgnoreRights = true;
                while (cursor.Read())
                {
                    Add(cursor.Item);
                }
            }              
        }
    }
}