/* TODO ERROR: Skipped DefineDirectiveTrivia */
using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class PratichePerFinestraLavorazione : CCollection<CPraticaCQSPD>
        {
            private FinestraLavorazione m_W;

            public PratichePerFinestraLavorazione()
            {
                m_W = null;
            }

            public PratichePerFinestraLavorazione(FinestraLavorazione w) : this()
            {
                Load(w);
            }

            public FinestraLavorazione Finestra
            {
                get
                {
                    return m_W;
                }
            }

            protected internal void SetFinestra(FinestraLavorazione value)
            {
                m_W = value;
                foreach (CPraticaCQSPD p in this)
                    p.SetFinestraLavorazione(value);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_W is object)
                    ((CPraticaCQSPD)value).SetFinestraLavorazione(m_W);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_W is object)
                    ((CPraticaCQSPD)newValue).SetFinestraLavorazione(m_W);
                base.OnSet(index, oldValue, newValue);
            }

            protected void Load(FinestraLavorazione w)
            {
                if (w is null)
                    throw new ArgumentNullException("w");
                SetFinestra(w);
                if (DBUtils.GetID(w) == 0)
                    return;
                var cursor = new CPraticheCQSPDCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
                cursor.IDFinestraLavorazione.Value = DBUtils.GetID(w);
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                cursor.Dispose();
            }
        }
    }
}