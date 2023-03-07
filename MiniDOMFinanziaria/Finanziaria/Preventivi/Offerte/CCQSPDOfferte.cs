using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Flags]
        public enum TEGCalcFlag : int
        {
            SPREAD = 1,
            PROVVIGIONE = 2,
            PREMIOVITA = 4,
            PREMIOIMPIEGO = 8,
            PREMIOCREDITO = 16,
            COMMISSIONI = 32,
            INTERESSI = 64,
            IMPOSTASOSTITUTIVA = 128,
            ONERIERARIALI = 256,
            ALTREIMPOSTE = 512,
            SPESECONVENZIONI = 1024,
            ALTRESPESE = 2048,
            RIVALSA = 4096,
            CALCOLOTEG_SPESEALL = SPESECONVENZIONI | ALTRESPESE
        }

        [Serializable]
        public class CCQSPDOfferte : CCollection<COffertaCQS>
        {
            [NonSerialized]
            private CPreventivo m_Preventivo;

            public CCQSPDOfferte()
            {
                m_Preventivo = null;
            }

            public CCQSPDOfferte(CPreventivo owner)
            {
                Load(owner);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Preventivo is object)
                    ((COffertaCQS)value).SetPreventivo(m_Preventivo);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Preventivo is object)
                    ((COffertaCQS)newValue).SetPreventivo(m_Preventivo);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal virtual void SetOwner(CPreventivo value)
            {
                m_Preventivo = value;
                foreach (COffertaCQS o in this)
                    o.SetPreventivo(value);
            }

            protected internal void Load(CPreventivo preventivo)
            {
                if (preventivo is null)
                    throw new ArgumentNullException("preventivo");
                Clear();
                m_Preventivo = preventivo;
                if (DBUtils.GetID(preventivo) == 0)
                    return;
                var cursor = new CCQSPDOfferteCursor();
                try
                {
                    cursor.PreventivoID.Value = DBUtils.GetID(preventivo);
                    cursor.ID.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }

                Comparer = m_Preventivo;
                Sort();
            }
        }
    }
}