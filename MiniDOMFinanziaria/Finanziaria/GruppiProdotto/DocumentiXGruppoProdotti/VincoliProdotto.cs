using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
    /// Rappresenta un documento caricabile per un prodotto
    /// </summary>
    /// <remarks></remarks>
        public class CVincoliProdottiClass : CModulesClass<Finanziaria.CDocumentoXGruppoProdotti>
        {
            public CVincoliProdottiClass() : base("modDocumentiXGruppoProdotti", typeof(Finanziaria.CDocumentiXGruppoProdottiCursor), -1)
            {
            }
        }
    }

    public partial class Finanziaria
    {
        private static CVincoliProdottiClass m_VincoliProdotto = null;

        public static CVincoliProdottiClass VincoliProdotto
        {
            get
            {
                if (m_VincoliProdotto is null)
                    m_VincoliProdotto = new CVincoliProdottiClass();
                return m_VincoliProdotto;
            }
        }
    }
}