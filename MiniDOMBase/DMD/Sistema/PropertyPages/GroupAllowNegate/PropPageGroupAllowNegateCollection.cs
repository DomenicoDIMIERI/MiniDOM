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
        /// Relazioni allow/negate tra i gruppi utente e le pagine di proprietà
        /// </summary>
        [Serializable]
        public class PropPageGroupAllowNegateCollection
            : CCollection<PropPageGroupAllowNegate>
        {
            [NonSerialized] private CRegisteredPropertyPage m_PropPage;

            /// <summary>
            /// Costruttore
            /// </summary>
            public PropPageGroupAllowNegateCollection()
            {
                m_PropPage = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="page"></param>
            public PropPageGroupAllowNegateCollection(CRegisteredPropertyPage page) : this()
            {
                if (page is null)
                    throw new ArgumentNullException("page");
                Load(page);
            }

            /// <summary>
            /// Pagina di proprietà
            /// </summary>
            public CRegisteredPropertyPage PropPage
            {
                get
                {
                    return m_PropPage;
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, PropPageGroupAllowNegate value)
            {
                if (m_PropPage is object)
                    value.SetItem(m_PropPage);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, PropPageGroupAllowNegate oldValue, PropPageGroupAllowNegate newValue)
            {
                if (m_PropPage is object)
                    newValue.SetItem(m_PropPage);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="page"></param>
            protected virtual void Load(CRegisteredPropertyPage page)
            {
                if (page is null)
                    throw new ArgumentNullException("page");
                Clear();
                m_PropPage = page;
                if (DBUtils.GetID(page, 0) == 0)
                    return;

                using (var cursor = new PropPageGroupAllowNegateCursor())
                {
                    cursor.ItemID.Value = DBUtils.GetID(page, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// Imposta la pagina di proprietà
            /// </summary>
            /// <param name="page"></param>
            internal void SetPropPage(CRegisteredPropertyPage page)
            {
                m_PropPage = page;
                if (page is null)
                    return;
                foreach (PropPageGroupAllowNegate l in this)
                    l.SetItem(page);
            }
        }
    }
}