using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CTrattativeCollaboratore : CCollection<CTrattativaCollaboratore>
        {
            [NonSerialized]
            private CCollaboratore m_Owner;

            public CTrattativeCollaboratore()
            {
                m_Owner = null;
            }

            public CTrattativeCollaboratore(CCollaboratore collaboratore) : this()
            {
                Initialize(collaboratore);
            }

            public CCollaboratore Collaboratore
            {
                get
                {
                    return m_Owner;
                }
            }

            protected internal void SetCollaboratore(CCollaboratore value)
            {
                m_Owner = value;
                if (value is null)
                    return;
                foreach (CTrattativaCollaboratore t in this)
                    t.SetCollaboratore(value);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CTrattativaCollaboratore)value).SetCollaboratore(m_Owner);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Owner is object)
                    ((CTrattativaCollaboratore)newValue).SetCollaboratore(m_Owner);
                base.OnSet(index, oldValue, newValue);
            }

            public CTrattativaCollaboratore Add()
            {
                var item = new CTrattativaCollaboratore();
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                return item;
            }

            public bool HasProdotto(CCQSPDProdotto p)
            {
                return IndexOfProdotto(p) >= 0;
            }

            public int IndexOfProdotto(CCQSPDProdotto p)
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                int i = 0;
                foreach (CTrattativaCollaboratore t in this)
                {
                    if (ReferenceEquals(t.Prodotto, p) || t.IDProdotto == DBUtils.GetID(p))
                    {
                        return i;
                    }

                    i += 1;
                }

                return -1;
            }

            public CTrattativaCollaboratore GetItemByNameAndProdotto(string name, CCQSPDProdotto prodotto, bool diretta)
            {
                name = DMD.Strings.Trim(name);
                foreach (CTrattativaCollaboratore t in this)
                {
                    if (DMD.Strings.Compare(name, t.Nome, true) == 0 && (DBUtils.GetID(prodotto) == 0 || DBUtils.GetID(prodotto) == t.IDProdotto))
                    {
                        if (diretta == Sistema.TestFlag(t.Flags, TrattativaCollaboratoreFlags.SoloDirettaCollaboratore))
                        {
                            return t;
                        }
                    }
                }

                return null;
            }

            /// <summary>
        /// Inserisce nella collezione tutti i prodotti mancanti
        /// </summary>
        /// <remarks></remarks>
            public void Update()
            {
                CTrattativaCollaboratore item;
                CCollection<CCQSPDProdotto> prodotti;
                if (Collaboratore.ListinoPredefinito is null)
                {
                    prodotti = Prodotti.LoadAll();
                }
                else
                {
                    prodotti = Collaboratore.ListinoPredefinito.GetProdottiValidi();
                }

                foreach (CCQSPDProdotto p in prodotti)
                {
                    if (p.IsValid() && !HasProdotto(p))
                    {
                        item = Add();
                        item.Richiesto = false;
                        item.Collaboratore = Collaboratore;
                        item.Cessionario = p.Cessionario;
                        item.Prodotto = p;
                        item.StatoTrattativa = StatoTrattativa.STATO_ACCETTATO;
                        item.SpreadProposto = default;
                        item.SpreadRichiesto = default;
                        item.SpreadApprovato = default;
                        item.Stato = ObjectStatus.OBJECT_VALID;
                        item.Save();
                    }
                }
            }

            protected internal void Initialize(CCollaboratore owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_Owner = owner;
                if (DBUtils.GetID(owner) == 0)
                    return;
                var cursor = new CTrattativeCollaboratoreCursor();
                cursor.IDCollaboratore.Value = DBUtils.GetID(owner);
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
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
            }

            public bool IsNonProposto()
            {
                bool ret = true;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var Item = this[i];
                    if (Item.StatoTrattativa != StatoTrattativa.STATO_NONPROPOSTO)
                    {
                        ret = false;
                        break;
                    }
                }

                return ret;
            }

            public bool IsNuovaProposta()
            {
                bool ret = false;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var Item = this[i];
                    if (Item.StatoTrattativa == StatoTrattativa.STATO_PROPOSTA)
                    {
                        ret = true;
                        break;
                    }
                }

                return ret;
            }

            public bool IsAccettato()
            {
                bool ret = true;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var Item = this[i];
                    if (Item.StatoTrattativa != StatoTrattativa.STATO_ACCETTATO)
                    {
                        ret = false;
                        break;
                    }
                }

                return ret;
            }
        }
    }
}