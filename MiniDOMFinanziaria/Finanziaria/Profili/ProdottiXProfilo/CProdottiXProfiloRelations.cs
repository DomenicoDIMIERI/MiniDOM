using System;
using System.Collections;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CProdottiXProfiloRelations : CCollection<CProdottoProfilo>
        {
            [NonSerialized]
            private CProfilo m_Profilo;

            public CProdottiXProfiloRelations()
            {
                m_Profilo = null;
            }

            public CProdottiXProfiloRelations(CProfilo profilo) : this()
            {
                Initialize(profilo);
            }
            /// <summary>
        /// Crea il record di associazione tra il profilo corrente ed il prodotto specificato
        /// </summary>
        /// <param name="prodotto">[in] Prodotto da associare</param>
        /// <param name="azione">[in] Tipo di relazione</param>
        /// <param name="spread">[in] Spread da aggiungere rispetto al genitore</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoProfilo SetRelationShip(CCQSPDProdotto prodotto, IncludeModes azione, double spread)
            {
                CProdottoProfilo item;
                bool isNew = false;
                item = GetRelationship(prodotto);
                if (item is null)
                {
                    item = new CProdottoProfilo();
                    item.Prodotto = prodotto;
                    isNew = true;
                }

                item.Azione = azione;
                item.Spread = spread;
                item.Stato = ObjectStatus.OBJECT_VALID;
                if (isNew)
                    Add(item);
                item.Save(true);
                return item;
            }

            public CProdottoProfilo SetRelationship(int idProdotto, IncludeModes azione, double spread)
            {
                return SetRelationShip(Prodotti.GetItemById(idProdotto), azione, spread);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Profilo is object)
                    ((CProdottoProfilo)value).SetProfilo(m_Profilo);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Profilo is object)
                    ((CProdottoProfilo)newValue).SetProfilo(m_Profilo);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal bool Initialize(CProfilo profilo)
            {
                if (profilo is null)
                    throw new ArgumentNullException("profilo");
                Clear();
                m_Profilo = profilo;
                if (DBUtils.GetID(profilo) == 0)
                    return true;
                var cursor = new CProdottoProfiloCursor();
                cursor.IDProfilo.Value = DBUtils.GetID(profilo);
                cursor.IgnoreRights = true;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return true;
            }

            /// <summary>
        /// Restituisce i prodotti definiti in questo preventivatore ed eventualmente i prodotti di tutta la gerarchia di genitori
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CCQSPDProdotto> GetProdotti()
            {
                var parents = GetParentsList();
                var prods = new CCollection<CProdottoProfilo>();
                var ret = new CCollection<CCQSPDProdotto>();
                prods.Sorted = true;
                prods.Comparer = new CProdProfComparer();
                for (int i1 = 0, loopTo = parents.Count - 1; i1 <= loopTo; i1++)
                {
                    var profilo = parents[i1];
                    for (int i2 = 0, loopTo1 = profilo.ProdottiXProfiloRelations.Count - 1; i2 <= loopTo1; i2++)
                    {
                        var rel = profilo.ProdottiXProfiloRelations[i2];
                        int i;
                        switch (rel.Azione)
                        {
                            case IncludeModes.Eredita:
                                {
                                    break;
                                }

                            case IncludeModes.Escludi:
                                {
                                    i = prods.IndexOf(rel);
                                    if (i >= 0)
                                        prods.RemoveAt(i);
                                    break;
                                }

                            case IncludeModes.Include:
                                {
                                    i = prods.IndexOf(rel);
                                    if (i < 0)
                                        prods.Add(rel);
                                    break;
                                }
                        }
                    }
                }

                for (int i3 = 0, loopTo2 = prods.Count - 1; i3 <= loopTo2; i3++)
                {
                    var rel = prods[i3];
                    if (rel.Prodotto is object && rel.Prodotto.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        ret.Add(rel.Prodotto);
                    }
                }

                ret.Sort();
                return ret;
            }

            private class CProdProfComparer : IComparer
            {
                public int Compare(CProdottoProfilo x, CProdottoProfilo y)
                {
                    return x.IDProdotto - y.IDProdotto;
                }

                int IComparer.Compare(object x, object y)
                {
                    return Compare((CProdottoProfilo)x, (CProdottoProfilo)y);
                }
            }

            private CCollection<CProfilo> GetParentsList()
            {
                var parents = new CCollection<CProfilo>();
                CProfilo curr;
                curr = m_Profilo;
                while (curr is object)
                {
                    parents.Insert(0, curr);
                    if (curr.EreditaProdotti)
                    {
                        curr = curr.Parent;
                    }
                    else
                    {
                        curr = null;
                    }
                }

                return parents;
            }

            /// <summary>
        /// Restituisce lo spread definito per il prodotto specificato. Lo spread viene calcolato sommando eventuali spread definiti nell'oggetto parent in maniera ricorsiva fino a Parent NULL
        /// </summary>
        /// <param name="idProdotto">[in] ID del prodotto da verificare</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public double GetSpread(int idProdotto)
            {
                CProfilo currPrev;
                double ret;
                CProdottoProfilo rel;
                currPrev = m_Profilo;
                ret = 0d;
                while (currPrev is object)
                {
                    rel = currPrev.ProdottiXProfiloRelations.GetRelationship(idProdotto);
                    if (rel is object && rel.Azione != IncludeModes.Escludi)
                        ret += rel.Spread;
                    if (currPrev.EreditaProdotti)
                    {
                        currPrev = currPrev.Parent;
                    }
                    else
                    {
                        currPrev = null;
                    }
                }

                return ret;
            }

            /// <summary>
        /// Restituisce lo spread definito per il prodotto specificato. Lo spread viene calcolato sommando eventuali spread definiti nell'oggetto parent in maniera ricorsiva fino a Parent NULL
        /// </summary>
        /// <param name="prodotto"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public double GetSpread(CCQSPDProdotto prodotto)
            {
                return GetSpread(DBUtils.GetID(prodotto));
            }

            /// <summary>
        /// Restituisce il record di associazione tra il profilo corrente ed il prodotto specificato
        /// </summary>
        /// <param name="idProdotto">[in] ID del prodotto da verificare</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoProfilo GetRelationship(int idProdotto)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var rel = this[i];
                    if (rel.IDProdotto == idProdotto)
                        return rel;
                }

                return null;
            }

            /// <summary>
        /// Restituisce il record di associazione tra il profilo corrente ed il prodotto specificato
        /// </summary>
        /// <param name="prodotto">[in] Prodotto da verificare</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoProfilo GetRelationship(CCQSPDProdotto prodotto)
            {
                return GetRelationship(DBUtils.GetID(prodotto));
            }

            protected internal void Update(CProdottoProfilo item)
            {
                var oldItem = GetItemById(DBUtils.GetID(item));
                if (item.Stato == ObjectStatus.OBJECT_VALID)
                {
                    int i = IndexOf(oldItem);
                    if (i >= 0)
                    {
                        RemoveAt(i);
                        Insert(i, item);
                    }
                    else
                    {
                        Add(item);
                    }
                }
                else
                {
                    Remove(oldItem);
                }
            }
        }
    }
}