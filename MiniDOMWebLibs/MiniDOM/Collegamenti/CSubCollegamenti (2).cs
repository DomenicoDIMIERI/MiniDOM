using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class CSubCollegamenti : CCollection<CCollegamento>
        {
            [NonSerialized]
            private CCollegamento m_Parent;

            public CSubCollegamenti()
            {
                m_Parent = null;
            }

            public CSubCollegamenti(CCollegamento parent) : this()
            {
                Load(parent);
            }

            public CCollegamento Parent
            {
                get
                {
                    return m_Parent;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Parent is object)
                    ((CCollegamento)value).SetParent(m_Parent);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Parent is object)
                    ((CCollegamento)newValue).SetParent(m_Parent);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal void Load(CCollegamento parent)
            {
                if (parent is null)
                    throw new ArgumentNullException("parent");
                Clear();
                m_Parent = parent;
                int pid = Databases.GetID(parent);
                if (pid == 0)
                    return;
                foreach (CCollegamento c in Collegamenti.LoadAll())
                {
                    if (c.IDParent == pid)
                    {
                        Add(c);
                    }
                }

                // Dim cursor As New CCollegamentiCursor
                // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                // 'cursor.Data.SortOrder = SortEnum.SORT_ASC
                // cursor.IDParent.Value = GetID(parent)
                // While cursor.EOF
                // MyBase.Add(cursor.Item)
                // cursor.MoveNext()
                // End While
                // cursor.Reset()
                // End If

                // Return True
            }
        }
    }
}