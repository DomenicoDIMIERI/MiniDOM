using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CCQSPDTipoProvvigioneCollection : CCollection<CCQSPDTipoProvvigione>
        {
            [NonSerialized]
            private CGruppoProdotti m_GruppoProdotti;

            public CCQSPDTipoProvvigioneCollection()
            {
                m_GruppoProdotti = null;
            }

            public CCQSPDTipoProvvigioneCollection(CGruppoProdotti owner) : this()
            {
                Load(owner);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_GruppoProdotti is object)
                    ((CCQSPDTipoProvvigione)value).SetGruppoProdotti(m_GruppoProdotti);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_GruppoProdotti is object)
                    ((CCQSPDTipoProvvigione)newValue).SetGruppoProdotti(m_GruppoProdotti);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal virtual void SetGruppoProdotti(CGruppoProdotti value)
            {
                m_GruppoProdotti = value;
                if (value is null)
                    return;
                foreach (CCQSPDTipoProvvigione o in this)
                    o.SetGruppoProdotti(value);
            }

            protected internal void Load(CGruppoProdotti gruppo)
            {
                if (gruppo is null)
                    throw new ArgumentNullException("gruppo");
                Clear();
                SetGruppoProdotti(gruppo);
                if (DBUtils.GetID(gruppo) == 0)
                    return;
                var cursor = new CCQSPDTipoProvvigioneCursor();
                try
                {
                    cursor.IDGruppoProdotti.Value = DBUtils.GetID(gruppo);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
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

                Sort();
            }

            public CCQSPDTipoProvvigione GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (CCQSPDTipoProvvigione t in this)
                {
                    if (
                        DMD.Strings.Compare(t.Nome, nome, true) == 0 
                        && t.Stato == ObjectStatus.OBJECT_VALID
                        )
                        return t;
                }

                return null;
            }
        }
    }
}