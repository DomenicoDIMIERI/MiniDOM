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
using static minidom.Store;

namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Collezione di sottocategorie
        /// </summary>
        [Serializable]
        public class SottocategorieCollection 
            : CKeyCollection<CategoriaArticolo>
        {
            [NonSerialized]
            private CategoriaArticolo m_Parent = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public SottocategorieCollection()
            {
                m_Parent = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="parent"></param>
            public SottocategorieCollection(CategoriaArticolo parent) : this()
            {
                Load(parent);
            }

            /// <summary>
            /// Imposta la categoria genitore
            /// </summary>
            /// <param name="parent"></param>
            protected internal virtual void SetParent(CategoriaArticolo parent)
            {
                m_Parent = parent;
                if (parent is object )
                {
                    foreach(var s in this)
                    {
                        s.SetParent(parent);
                    }
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CategoriaArticolo oldValue, CategoriaArticolo newValue)
            {
                if (m_Parent is object)
                    newValue.SetParent(m_Parent);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CategoriaArticolo value)
            {
                if (m_Parent is object)
                    value.SetParent(m_Parent);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// Carica le sottocategorie
            /// </summary>
            /// <param name="parent"></param>
            protected internal virtual void Load(CategoriaArticolo parent)
            {
                if (parent is null)
                    throw new ArgumentNullException("parent");
                
                Clear();

                if (DBUtils.GetID(parent, 0) == 0)
                    return;

                using (var cursor = new CategoriaArticoloCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDParent.Value = DBUtils.GetID(parent, 0);
                    //cursor.Nome.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }

                this.Sort();
            }
        }
    }
}