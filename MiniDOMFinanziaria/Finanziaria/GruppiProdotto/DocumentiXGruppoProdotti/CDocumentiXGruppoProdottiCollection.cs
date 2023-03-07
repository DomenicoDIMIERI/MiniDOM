using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CDocumentiXGruppoProdottiCollection : CCollection<CDocumentoXGruppoProdotti>
        {
            [NonSerialized]
            private CGruppoProdotti m_GruppoProdotti;

            public CDocumentiXGruppoProdottiCollection()
            {
                m_GruppoProdotti = null;
            }

            public CDocumentiXGruppoProdottiCollection(CGruppoProdotti gruppo) : this()
            {
                Load(gruppo);
            }

            public CGruppoProdotti GruppoProdotti
            {
                get
                {
                    return m_GruppoProdotti;
                }
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_GruppoProdotti is object)
                {
                    {
                        var withBlock = (CDocumentoXGruppoProdotti)newValue;
                        withBlock.SetGruppoProdotti(m_GruppoProdotti);
                        withBlock.SetProgressivo(index);
                    }
                }

                base.OnSet(index, oldValue, newValue);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_GruppoProdotti is object)
                {
                    {
                        var withBlock = (CDocumentoXGruppoProdotti)value;
                        withBlock.SetGruppoProdotti(m_GruppoProdotti);
                        withBlock.SetProgressivo(index);
                    }

                    for (int i = index, loopTo = Count - 1; i <= loopTo; i++)
                        this[i].Progressivo = i + 1;
                }

                base.OnInsert(index, value);
            }

            protected internal void Load(CGruppoProdotti gruppo)
            {
                if (gruppo is null)
                    throw new ArgumentNullException("gruppo");
                Clear();
                m_GruppoProdotti = gruppo;
                if (DBUtils.GetID(gruppo) == 0)
                    return;
                var cursor = new CDocumentiXGruppoProdottiCursor();
                try
                {
                    cursor.IDGruppoProdotti.Value = DBUtils.GetID(gruppo);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.Progressivo.SortOrder = SortEnum.SORT_ASC;
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
            }

            public void ReLoad()
            {
                Load(m_GruppoProdotti);
            }

            protected internal void SetOwner(CGruppoProdotti value)
            {
                m_GruppoProdotti = value;
                if (value is null)
                    return;
                foreach (CDocumentoXGruppoProdotti d in this)
                    d.SetGruppoProdotti(value);
            }
        }
    }
}