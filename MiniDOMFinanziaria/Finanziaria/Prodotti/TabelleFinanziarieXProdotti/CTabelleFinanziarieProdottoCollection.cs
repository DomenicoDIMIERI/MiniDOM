using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Classe che racchiude l'insieme degli oggetti relazione tra un prodotto ed una tabella Finanziaria
    /// </summary>
    /// <remarks></remarks>
        public class CTabelleFinanziarieProdottoCollection : CCollection<CProdottoXTabellaFin>
        {
            private CCQSPDProdotto m_Prodotto;
            private CTabellaFinanziaria m_TabellaFinanziaria;

            public CTabelleFinanziarieProdottoCollection()
            {
                m_Prodotto = null;
                m_TabellaFinanziaria = null;
            }

            public CTabelleFinanziarieProdottoCollection(CCQSPDProdotto prodotto)
            {
                if (prodotto is null)
                    throw new ArgumentNullException("prodotto");
                Initialize(prodotto);
            }

            public CTabelleFinanziarieProdottoCollection(CTabellaFinanziaria tabella)
            {
                if (tabella is null)
                    throw new ArgumentNullException("tabella");
                Initialize(tabella);
            }

            protected override void OnInsert(int index, object value)
            {
                {
                    var withBlock = (CProdottoXTabellaFin)value;
                    if (m_Prodotto is object)
                        withBlock.SetProdotto(m_Prodotto);
                    if (m_TabellaFinanziaria is object)
                        withBlock.SetTabella(m_TabellaFinanziaria);
                }

                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                {
                    var withBlock = (CProdottoXTabellaFin)newValue;
                    if (m_Prodotto is object)
                        withBlock.SetProdotto(m_Prodotto);
                    if (m_TabellaFinanziaria is object)
                        withBlock.SetTabella(m_TabellaFinanziaria);
                }

                base.OnSet(index, oldValue, newValue);
            }

            // Crea un oggetto relazione prodotto - tabella Finanziaria, lo aggiunge alla collezione e lo restituisce in uscita.
            // Se la collezione è associata ad un prodotto l'argomento deve essere la tabella a cui correlare il prodotto.
            // Se, invece, la collezione è associata ad una tabella Finanziaria l'argomento deve essere il prodotto a cui correlarlo.
            public CProdottoXTabellaFin Create(CCQSPDProdotto prodotto)
            {
                var item = new CProdottoXTabellaFin();
                item.Prodotto = prodotto;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                item.Save();
                return item;
            }

            /// <summary>
        /// Crea un oggetto relazione prodotto - tabella Finanziaria, lo aggiunge alla collezione e lo restituisce in uscita.
        /// Se la collezione è associata ad un prodotto l'argomento deve essere la tabella a cui correlare il prodotto.
        /// Se, invece, la collezione è associata ad una tabella Finanziaria l'argomento deve essere il prodotto a cui correlarlo.
        /// </summary>
        /// <param name="tabella"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoXTabellaFin Create(CTabellaFinanziaria tabella)
            {
                var item = new CProdottoXTabellaFin();
                item.Tabella = tabella;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                item.Save();
                return item;
            }

            public CProdottoXTabellaFin RemoveTabella(CTabellaFinanziaria table)
            {
                int i = 0;
                while (i < Count)
                {
                    var ret = this[i];
                    if (ret.IDTabella == DBUtils.GetID(table))
                    {
                        ret.Delete();
                        RemoveAt(i);
                        return ret;
                    }
                    else
                    {
                        i += 1;
                    }
                }

                return null;
            }

            public CProdottoXTabellaFin GetItemByTabellaFinanziaria(int tblID)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if (this[i].IDTabella == tblID)
                    {
                        return this[i];
                    }
                }

                return null;
            }

            protected bool Initialize(CCQSPDProdotto owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_TabellaFinanziaria = null;
                m_Prodotto = owner;
                if (DBUtils.GetID(owner) != 0)
                {
                    var cursor = new CProdottoXTabellaFinCursor();
                    try
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.ProdottoID.Value = DBUtils.GetID(owner);
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
                }

                return true;
            }

            protected bool Initialize(CTabellaFinanziaria owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_TabellaFinanziaria = owner;
                m_Prodotto = null;
                if (DBUtils.GetID(owner) != 0)
                {
                    var cursor = new CProdottoXTabellaFinCursor();
                    try
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.TabellaFinanziariaID.Value = DBUtils.GetID(owner);
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
                }

                return true;
            }

            protected internal virtual void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                if (value is object)
                {
                    foreach (CProdottoXTabellaFin item in this)
                        item.SetProdotto(value);
                }
            }
        }
    }
}