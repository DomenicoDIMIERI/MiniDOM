using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CQSPDSoluzioniXStudioDiFattibilita : CCollection<CQSPDConsulenza>
        {
            private CQSPDStudioDiFattibilita m_StudioDiFattibilita;

            public CQSPDSoluzioniXStudioDiFattibilita()
            {
                m_StudioDiFattibilita = null;
            }

            public CQSPDSoluzioniXStudioDiFattibilita(CQSPDStudioDiFattibilita gruppo) : this()
            {
                Load(gruppo);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_StudioDiFattibilita is object)
                    ((CQSPDConsulenza)value).SetStudioDiFattibilita(m_StudioDiFattibilita);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_StudioDiFattibilita is object)
                    ((CQSPDConsulenza)newValue).SetStudioDiFattibilita(m_StudioDiFattibilita);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal void SetStudioDiFattibilita(CQSPDStudioDiFattibilita value)
            {
                if (value is null)
                    throw new ArgumentNullException("value");
                m_StudioDiFattibilita = value;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var c = this[i];
                    c.SetStudioDiFattibilita(value);
                }
            }

            private void Load(CQSPDStudioDiFattibilita studio)
            {
                if (studio is null)
                    throw new ArgumentNullException("gruppo");
                Clear();
                m_StudioDiFattibilita = studio;
                if (DBUtils.GetID(studio) == 0)
                    return;
                var cursor = new CQSPDConsulenzaCursor();
                try
                {
                    cursor.IDStudioDiFattibilita.Value = DBUtils.GetID(studio);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    // cursor.ID.SortOrder = SortEnum.SORT_DESC
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    cursor.Dispose();
                }

                Sort();
            }
        }
    }
}