using System;
using DMD;

namespace minidom
{
    namespace CQSPDInternals
    {
        [Serializable]
        public class CMotiviScontoPraticaClass 
            : CModulesClass<Finanziaria.CMotivoScontoPratica>
        {
            public CMotiviScontoPraticaClass() : base("modCQSPDMotiviScontoPratica", typeof(Finanziaria.CMotivoScontoPraticaCursor), -1)
            {
            }

            /// <summary>
        /// Restituisce un oggetto in base al suo nome (la ricerca è fatta solo sui motivi attivi)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CMotivoScontoPratica GetItemByName(string value)
            {
                var items = LoadAll();
                value = Strings.LCase(Strings.Trim(value));
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CMotivoScontoPratica m in items)
                {
                    if (m.Attivo && (Strings.LCase(m.Nome) ?? "") == (value ?? ""))
                        return m;
                }

                return null;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CQSPDInternals.CMotiviScontoPraticaClass m_MotiviSconto;

        public static CQSPDInternals.CMotiviScontoPraticaClass MotiviSconto
        {
            get
            {
                if (m_MotiviSconto is null)
                    m_MotiviSconto = new CQSPDInternals.CMotiviScontoPraticaClass();
                return m_MotiviSconto;
            }
        }
    }
}