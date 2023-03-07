using DMD;
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
    /// Gruppi prodotto
    /// </summary>
    /// <remarks></remarks>
        public sealed class CGruppiProdottoClass : CModulesClass<Finanziaria.CGruppoProdotti>
        {
            internal CGruppiProdottoClass() : base("modProdGrp", typeof(Finanziaria.CGruppoProdottiCursor), -1)
            {
            }



            /// <summary>
        /// Restituisce il gruppo prodotto in base al suo nome (la ricerca è limitata ai soli gruppi validi)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CGruppoProdotti GetItemByName(string value)
            {
                value = Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CGruppoProdotti ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.Descrizione, value, true) == 0)
                        return ret;
                }

                return null;
            }

            internal void InvalidateTipiProvvigione()
            {
                foreach (Finanziaria.CGruppoProdotti g in LoadAll())
                    g.InvalidateTipiProvvigione();
            }
        }
    }

    public partial class Finanziaria
    {
        private static CGruppiProdottoClass m_GruppiProdotto = null;

        public static CGruppiProdottoClass GruppiProdotto
        {
            get
            {
                if (m_GruppiProdotto is null)
                    m_GruppiProdotto = new CGruppiProdottoClass();
                return m_GruppiProdotto;
            }
        }
    }
}