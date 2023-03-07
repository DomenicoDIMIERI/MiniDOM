using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class LinkGroupAllowNegateCollection : CCollection<LinkGroupAllowNegate>
        {
            private CCollegamento m_Link;

            public LinkGroupAllowNegateCollection()
            {
                m_Link = null;
            }

            public LinkGroupAllowNegateCollection(CCollegamento link) : this()
            {
                Load(link);
            }

            public CCollegamento Link
            {
                get
                {
                    return m_Link;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Link is object)
                    ((LinkGroupAllowNegate)value).Item = m_Link;
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Link is object)
                    ((LinkGroupAllowNegate)newValue).Item = m_Link;
                base.OnSet(index, oldValue, newValue);
            }

            protected virtual void Load(CCollegamento link)
            {
                if (link is null)
                    throw new ArgumentNullException("link");
                Clear();
                m_Link = link;
                if (Databases.GetID(link) == 0)
                    return;
                var cursor = new LinkGroupAllowNegateCursor();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.ItemID.Value = Databases.GetID(link);
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    Add((LinkGroupAllowNegate)cursor.Item);
                    cursor.MoveNext();
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Dispose();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }
        }
    }
}