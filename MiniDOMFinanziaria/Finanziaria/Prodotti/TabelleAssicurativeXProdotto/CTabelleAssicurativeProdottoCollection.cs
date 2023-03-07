using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Classe che racchiude l'insieme degli oggetti relazione tra un prodotto ed una tripla (vita, impiego, credito) di tabelle assicurativ
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CTabelleAssicurativeProdottoCollection : CCollection<CProdottoXTabellaAss>
        {
            [NonSerialized]
            private CCQSPDProdotto m_Prodotto;

            public CTabelleAssicurativeProdottoCollection()
            {
                m_Prodotto = null;
            }

            public CTabelleAssicurativeProdottoCollection(CCQSPDProdotto owner) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Initialize(owner);
            }


            /// <summary>
        /// Crea un oggetto relazione prodotto - tabella Finanziaria, lo aggiunge alla collezione e lo restituisce in uscita.
        /// Se la collezione è associata ad un prodotto l'argomento deve essere la tabella a cui correlare il prodotto.
        /// Se, invece, la collezione è associata ad una tabella Finanziaria l'argomento deve essere il prodotto a cui correlarlo.
        /// </summary>
        /// <param name="descrizione"></param>
        /// <param name="tabellaVita"></param>
        /// <param name="tabellaImpiego"></param>
        /// <param name="tabellaCredito"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottoXTabellaAss Create(string descrizione, int tabellaVita, int tabellaImpiego, int tabellaCredito)
            {
                var item = new CProdottoXTabellaAss();
                item.Descrizione = descrizione;
                item.IDRischioVita = tabellaVita;
                item.IDRischioImpiego = tabellaImpiego;
                item.IDRischioCredito = tabellaCredito;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                item.Save();
                return item;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Prodotto is object)
                    ((CProdottoXTabellaAss)value).SetProdotto(m_Prodotto);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Prodotto is object)
                    ((CProdottoXTabellaAss)newValue).SetProdotto(m_Prodotto);
                base.OnSet(index, oldValue, newValue);
            }

            protected void Initialize(CCQSPDProdotto owner)
            {
                lock (TabelleAssicurative.cacheLock)
                {
                    Clear();
                    m_Prodotto = owner;
                    if (DBUtils.GetID(owner) == 0)
                        return;
                    foreach (CProdottoXTabellaAss rel in TabelleAssicurative.ProdottiRelations)
                    {
                        if (rel.IDProdotto == DBUtils.GetID(owner))
                        {
                            Add(rel);
                        }
                    }

                    // Dim cursor As CProdottoXTabellaAssCursor = Nothing
                    // Try

                    // MyBase.Clear()

                    // Me.m_Prodotto = owner

                    // If (GetID(owner) = 0) Then Exit Sub

                    // 'cursor = New CProdottoXTabellaAssCursor
                    // 'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    // 'cursor.ProdottoID.Value = DBUtils.GetID(owner, 0)
                    // ''cursor.Descrizione.SortOrder = SortEnum.SORT_ASC
                    // 'cursor.IgnoreRights = True
                    // 'While Not cursor.EOF
                    // '    Call Me.Add(cursor.Item)
                    // '    cursor.MoveNext()
                    // 'End While


                    // Me.Sort()
                    // Catch ex As Exception
                    // Sistema.Events.NotifyUnhandledException(ex)
                    // Throw
                    // Finally
                    // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                    // End Try
                }
            }

            protected internal virtual void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                if (value is object)
                {
                    foreach (CProdottoXTabellaAss item in this)
                        item.SetProdotto(value);
                }
            }
        }
    }
}