using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
    /// Modulo delle categorie prodotto
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CategorieProdottoClass : CModulesClass<Finanziaria.CCategoriaProdotto>
        {
            public CategorieProdottoClass() : base("modCQSPDCatProd", typeof(Finanziaria.CCategorieProdottoCursor), -1)
            {
            }

            public Finanziaria.CCategoriaProdotto GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (Finanziaria.CCategoriaProdotto c in LoadAll())
                {
                    if (
                        c.Stato == ObjectStatus.OBJECT_VALID 
                        && 
                        DMD.Strings.Compare(c.Nome, nome, true) == 0
                        )
                        return c;
                }

                return null;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CategorieProdottoClass m_CategorieProdotto = null;

        public static CategorieProdottoClass CategorieProdotto
        {
            get
            {
                if (m_CategorieProdotto is null)
                    m_CategorieProdotto = new CategorieProdottoClass();
                return m_CategorieProdotto;
            }
        }
    }
}