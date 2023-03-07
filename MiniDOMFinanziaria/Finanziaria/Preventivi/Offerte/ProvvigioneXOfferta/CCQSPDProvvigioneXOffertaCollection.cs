using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CCQSPDProvvigioneXOffertaCollection : CCollection<CCQSPDProvvigioneXOfferta>
        {
            private COffertaCQS m_Offerta;

            public CCQSPDProvvigioneXOffertaCollection()
            {
                m_Offerta = null;
            }

            public CCQSPDProvvigioneXOffertaCollection(COffertaCQS owner) : this()
            {
                Load(owner);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Offerta is object)
                    ((CCQSPDProvvigioneXOfferta)value).SetOfferta(m_Offerta);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Offerta is object)
                    ((CCQSPDProvvigioneXOfferta)newValue).SetOfferta(m_Offerta);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal virtual void SetOfferta(COffertaCQS value)
            {
                m_Offerta = value;
                foreach (CCQSPDProvvigioneXOfferta o in this)
                    o.SetOfferta(value);
            }

            public CCQSPDProvvigioneXOfferta GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (CCQSPDProvvigioneXOfferta provv in this)
                {
                    if (DMD.Strings.Compare(provv.Nome, nome, true) == 0)
                        return provv;
                }

                return null;
            }

            public CCQSPDProvvigioneXOfferta GetItemByTipoProvvigione(CCQSPDTipoProvvigione tp)
            {
                if (tp is null)
                    throw new ArgumentNullException("tp");
                foreach (CCQSPDProvvigioneXOfferta provv in this)
                {
                    if (provv.IDTipoProvvigione == DBUtils.GetID(tp))
                        return provv;
                }

                return null;
            }

            public CCQSPDProvvigioneXOfferta GetItemByTrattativaCollaboratore(CTrattativaCollaboratore tc)
            {
                if (tc is null)
                    throw new ArgumentNullException("tc");
                foreach (CCQSPDProvvigioneXOfferta provv in this)
                {
                    if (provv.IDTrattativaCollaboratore == DBUtils.GetID(tc))
                        return provv;
                }

                return null;
            }

            protected internal void Load(COffertaCQS offerta)
            {
                if (offerta is null)
                    throw new ArgumentNullException("offerta");
                Clear();
                SetOfferta(offerta);
                if (DBUtils.GetID(offerta) == 0)
                    return;
                var cursor = new CCQSPDProvvigioneXOffertaCursor();
                try
                {
                    cursor.IDOfferta.Value = DBUtils.GetID(offerta);
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
        }
    }
}