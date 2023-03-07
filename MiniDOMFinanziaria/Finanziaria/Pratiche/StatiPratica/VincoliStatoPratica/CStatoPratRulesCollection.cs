using System;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Insieme di oggetti CStatoPratRule definiti su uno stato
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CStatoPratRulesCollection : CCollection<CStatoPratRule>
        {
            [NonSerialized]
            private CStatoPratica m_Parent; // [CStatoPratica] Oggetto a cui appartiene la collezione

            public CStatoPratRulesCollection()
            {
                m_Parent = null;
            }

            public CStatoPratRulesCollection(CStatoPratica statoIniziale) : this()
            {
                Load(statoIniziale);
            }

            public CStatoPratica Parent
            {
                get
                {
                    return m_Parent;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Parent is object)
                    ((CStatoPratRule)value).SetSource(m_Parent);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Parent is object)
                    ((CStatoPratRule)newValue).SetSource(m_Parent);
                base.OnSet(index, oldValue, newValue);
            }

            internal void Load(CStatoPratica parent)
            {
                lock (this)
                {
                    if (parent is null)
                        throw new ArgumentNullException("parent");
                    Clear();
                    m_Parent = parent;
                    if (DBUtils.GetID(parent) == 0)
                        return;

                    // Dim cursor As New CStatoPratRuleCursor
                    // cursor.IDSource.Value = DBUtils.GetID(parent, 0)
                    // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    // cursor.IgnoreRights = True
                    // While Not cursor.EOF
                    // MyBase.Add(cursor.Item)
                    // cursor.MoveNext()
                    // End While
                    // cursor.Reset()
                    // cursor = Nothing
                    lock (StatiPratRules)
                    {
                        foreach (CStatoPratRule rule in StatiPratRules.LoadAll())
                        {
                            if (rule.Stato == ObjectStatus.OBJECT_VALID && rule.IDSource == DBUtils.GetID(parent))
                            {
                                Add(rule);
                            }
                        }
                    }

                    Sort();
                }
            }

            protected internal virtual void SetOwner(CStatoPratica owner)
            {
                m_Parent = owner;
                if (owner is null)
                    return;
                foreach (CStatoPratRule rule in this)
                    rule.SetSource(owner);
            }
        }
    }
}