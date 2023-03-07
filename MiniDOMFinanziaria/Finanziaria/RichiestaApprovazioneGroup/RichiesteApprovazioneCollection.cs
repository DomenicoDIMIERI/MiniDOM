using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class RichiesteApprovazioneCollection : CCollection<CRichiestaApprovazione>
        {
            private RichiestaApprovazioneGroup m_Group;

            public RichiesteApprovazioneCollection()
            {
                m_Group = null;
            }

            public RichiesteApprovazioneCollection(RichiestaApprovazioneGroup group) : this()
            {
                Load(group);
            }

            public RichiestaApprovazioneGroup Group
            {
                get
                {
                    return m_Group;
                }
            }

            protected internal void SetGroup(RichiestaApprovazioneGroup value)
            {
                m_Group = value;
                if (value is null)
                    return;
                foreach (CRichiestaApprovazione r in this)
                    r.SetGruppo(value);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Group is object)
                    ((CRichiestaApprovazione)value).SetGruppo(m_Group);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Group is object)
                    ((CRichiestaApprovazione)newValue).SetGruppo(m_Group);
                base.OnSet(index, oldValue, newValue);
            }

            protected void Load(RichiestaApprovazioneGroup group)
            {
                if (group is null)
                    throw new ArgumentNullException("group");
                Clear();
                m_Group = group;
                if (DBUtils.GetID(group) == 0)
                    return;
                var cursor = new CRichiestaApprovazioneCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
                cursor.IDGruppo.Value = DBUtils.GetID(group);
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                cursor.Dispose();
                cursor = null;
            }
        }
    }
}