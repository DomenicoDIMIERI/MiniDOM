using System;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Insieme di vincoli definiti per una tabella Finanziaria
    /// </summary>
    /// <remarks></remarks>
        public class CVincoliProdottoTabellaFin : CCollection<CProdTabFinConstraint>
        {
            private CProdottoXTabellaFin m_Owner;

            public CVincoliProdottoTabellaFin()
            {
            }

            public CVincoliProdottoTabellaFin(CProdottoXTabellaFin owner) : this()
            {
                Initialize(owner);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CProdTabFinConstraint)value).SetOwner(m_Owner);
                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CProdottoXTabellaFin owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_Owner = owner;
                if (DBUtils.GetID(owner) != 0)
                {
                    var cursor = new CProdTabFinConstraintCursor();
                    cursor.OwnerID.Value = DBUtils.GetID(owner);
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                }

                return true;
            }

            public bool Check(COffertaCQS offerta)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var c = this[i];
                    if (c.Check(offerta) == false)
                        return false;
                }

                return true;
            }
        }
    }
}