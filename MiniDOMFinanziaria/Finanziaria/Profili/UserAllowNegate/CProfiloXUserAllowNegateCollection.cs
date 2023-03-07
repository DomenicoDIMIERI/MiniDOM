using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CProfiloXUserAllowNegateCollection : CCollection<CProfiloXUserAllowNegate>
        {
            private CProfilo m_Profilo;

            public CProfiloXUserAllowNegateCollection()
            {
                m_Profilo = null;
            }

            public CProfiloXUserAllowNegateCollection(CProfilo Profilo) : this()
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
                    ((CProfiloXUserAllowNegate)value).SetItem(m_Profilo);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Profilo is object)
                    ((CProfiloXUserAllowNegate)newValue).SetItem(m_Profilo);
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
                var cursor = new CProfiloXUserAllowNegateCursor();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.ItemID.Value = DBUtils.GetID(Profilo);
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    Add((CProfiloXUserAllowNegate)cursor.Item);
                    cursor.MoveNext();
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Dispose();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }
        }
    }
}