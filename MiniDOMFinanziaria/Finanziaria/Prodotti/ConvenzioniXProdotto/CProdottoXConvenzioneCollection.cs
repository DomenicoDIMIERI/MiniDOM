using System;
using System.Data;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Collezione di convenzioni associate ad un prodotto
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CProdottoXConvenzioneCollection : CCollection<CProdottoXConvenzione>
        {
            [NonSerialized]
            private CCQSPDProdotto m_Prodotto;

            public CProdottoXConvenzioneCollection()
            {
                m_Prodotto = null;
            }

            public CProdottoXConvenzioneCollection(CCQSPDProdotto prodotto) : this()
            {
                Load(prodotto);
            }

            public CProdottoXConvenzione Create(string nome, CQSPDConvenzione convenzione)
            {
                var item = GetItemByName(nome);
                if (item is object)
                    throw new DuplicateNameException("nome");
                item = new CProdottoXConvenzione();
                item.Nome = nome;
                item.Convenzione = convenzione;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                item.Save();
                return item;
            }

            public CProdottoXConvenzione GetItemByName(string nome)
            {
                foreach (CProdottoXConvenzione rel in this)
                {
                    if (DMD.Strings.Compare(rel.Nome, nome, true) == 0)
                        return rel;
                }

                return null;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Prodotto is object)
                    ((CProdottoXConvenzione)value).SetProdotto(m_Prodotto);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Prodotto is object)
                    ((CProdottoXConvenzione)newValue).SetProdotto(m_Prodotto);
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
                CProdottoXConvenzioneCursor cursor = null;
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor = new CProdottoXConvenzioneCursor();
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
                    foreach (CProdottoXConvenzione item in this)
                        item.SetProdotto(value);
                }
            }
        }
    }
}