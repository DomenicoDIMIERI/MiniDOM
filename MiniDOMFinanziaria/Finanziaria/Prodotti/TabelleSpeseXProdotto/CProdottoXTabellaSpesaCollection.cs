using System;
using System.Data;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Collezione di tabelle spese associate ad un prodotto
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CProdottoXTabellaSpesaCollection : CCollection<CProdottoXTabellaSpesa>
        {
            [NonSerialized]
            private CCQSPDProdotto m_Prodotto;

            public CProdottoXTabellaSpesaCollection()
            {
                m_Prodotto = null;
            }

            public CProdottoXTabellaSpesaCollection(CCQSPDProdotto prodotto) : this()
            {
                Load(prodotto);
            }

            public CProdottoXTabellaSpesa Create(string nome, CTabellaSpese tabella)
            {
                var item = GetItemByName(nome);
                if (item is object)
                    throw new DuplicateNameException("nome");
                item = new CProdottoXTabellaSpesa();
                item.Nome = nome;
                item.TabellaSpese = tabella;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                item.Save();
                return item;
            }

            public CProdottoXTabellaSpesa GetItemByName(string nome)
            {
                foreach (CProdottoXTabellaSpesa rel in this)
                {
                    if (DMD.Strings.Compare(rel.Nome, nome, true) == 0)
                        return rel;
                }

                return null;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Prodotto is object)
                    ((CProdottoXTabellaSpesa)value).SetProdotto(m_Prodotto);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Prodotto is object)
                    ((CProdottoXTabellaSpesa)newValue).SetProdotto(m_Prodotto);
                base.OnSet(index, oldValue, newValue);
            }

            protected void Load(CCQSPDProdotto prodotto)
            {
                if (prodotto is null)
                    throw new ArgumentNullException("prodotto");
                Clear();
                SetProdotto(prodotto);
                if (DBUtils.GetID(prodotto) == 0)
                    return;
                CProdottoXTabellaSpesaCursor cursor = null;
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor = new CProdottoXTabellaSpesaCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDProdotto.Value = DBUtils.GetID(prodotto);
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                Sort();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }

            protected internal virtual void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                if (value is object)
                {
                    foreach (CProdottoXTabellaSpesa item in this)
                        item.SetProdotto(value);
                }
            }
        }
    }
}