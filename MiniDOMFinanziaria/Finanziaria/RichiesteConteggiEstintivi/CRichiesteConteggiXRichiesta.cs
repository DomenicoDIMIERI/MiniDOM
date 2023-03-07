using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CRichiesteConteggiXRichiesta : CCollection<CRichiestaConteggio>
        {
            [NonSerialized]
            private CRichiestaFinanziamento m_RichiestaDiFinanziamento;

            public CRichiesteConteggiXRichiesta()
            {
                m_RichiestaDiFinanziamento = null;
            }

            public CRichiesteConteggiXRichiesta(CRichiestaFinanziamento richiesta) : this()
            {
                Load(richiesta);
            }

            public CRichiestaFinanziamento RichiestaDiFinanziamento
            {
                get
                {
                    return m_RichiestaDiFinanziamento;
                }
            }

            protected internal virtual void SetRichiesta(CRichiestaFinanziamento value)
            {
                m_RichiestaDiFinanziamento = value;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    this[i].SetRichiestaDiFinanziamento(value);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_RichiestaDiFinanziamento is object)
                    ((CRichiestaConteggio)value).SetRichiestaDiFinanziamento(m_RichiestaDiFinanziamento);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_RichiestaDiFinanziamento is object)
                    ((CRichiestaConteggio)newValue).SetRichiestaDiFinanziamento(m_RichiestaDiFinanziamento);
                base.OnSet(index, oldValue, newValue);
            }

            protected void Load(CRichiestaFinanziamento richiesta)
            {
                if (richiesta is null)
                    throw new ArgumentNullException("richiesta");
                Clear();
                m_RichiestaDiFinanziamento = richiesta;
                if (DBUtils.GetID(richiesta) != 0)
                {
                    var cursor = new CRichiestaConteggioCursor();
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDRichiestaDiFinanziamento.Value = DBUtils.GetID(richiesta);
                    cursor.DataRichiesta.SortOrder = SortEnum.SORT_ASC;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    cursor.Dispose();
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                }
            }
        }
    }
}