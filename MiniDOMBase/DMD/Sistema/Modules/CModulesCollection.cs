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
        /// Collezione di moduli
        /// </summary>
        [Serializable]
        public class CModulesCollection 
            : CKeyCollection<CModule>
        {
            [NonSerialized] private CModule m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModulesCollection()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CModulesCollection(CModule owner) : this()
            {
                Load(owner);
            }

            /// <summary>
            /// Restituisce l'elemento con l'id specificato
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public int IndexOfId(int ID)
            {
                int i;
                var loopTo = base.Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (DBUtils.GetID(base[i], 0) == ID)
                        return i;
                }

                return -1;
            }



            // Public Function GetItemByKey(ByVal key As String) As CModule
            // Dim i As Integer = Me.IndexOfKey(key)
            // If (i >= 0) Then Return MyBase.Item(i)
            // Return Nothing
            // End Function

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CModule value)
            {
                if (m_Owner is object)
                    value.SetParent(m_Owner);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CModule oldValue, CModule newValue)
            {
                if (m_Owner is object)
                    newValue.SetParent(m_Owner);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="owner"></param>
            /// <returns></returns>
            protected internal bool Load(CModule owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_Owner = owner;

                int parentID = DBUtils.GetID(owner, 0);
                if (parentID != 0)
                {
                    foreach (CModule m in Modules.LoadAll())
                    {
                        if (m.ParentID == parentID)
                        {
                            Add(m.ModuleName, m);
                        }
                    }
                }

                return true;
            }
        }
    }
}