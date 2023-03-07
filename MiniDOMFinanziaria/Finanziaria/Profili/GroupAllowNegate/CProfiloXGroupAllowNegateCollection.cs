using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CProfiloXGroupAllowNegateCollection : CCollection<CProfiloXGroupAllowNegate>
        {
            private CProfilo m_Profilo;

            public CProfiloXGroupAllowNegateCollection()
            {
                m_Profilo = null;
            }

            public CProfiloXGroupAllowNegateCollection(CProfilo Profilo) : this()
            {
                Load(Profilo);
            }

            public CProfilo Profilo
            {
                get
                {
                    return m_Profilo;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Profilo is object)
                    ((CProfiloXGroupAllowNegate)value).SetItem(m_Profilo);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Profilo is object)
                    ((CProfiloXGroupAllowNegate)newValue).SetItem(m_Profilo);
                base.OnSet(index, oldValue, newValue);
            }

            protected virtual void Load(CProfilo Profilo)
            {
                if (Profilo is null)
                    throw new ArgumentNullException("Profilo");
                Clear();
                m_Profilo = Profilo;
                if (DBUtils.GetID(Profilo) == 0)
                    return;
                var cursor = new CProfiloXGroupAllowNegateCursor();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.ItemID.Value = DBUtils.GetID(Profilo);
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    Add((CProfiloXGroupAllowNegate)cursor.Item);
                    cursor.MoveNext();
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Dispose();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }
        }
    }
}