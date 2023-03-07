using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class CConvenzioniClass : CModulesClass<Finanziaria.CQSPDConvenzione>
        {
            internal CConvenzioniClass() : base("modCQSPDConvenzioni", typeof(Finanziaria.CQSPDConvenzioniCursor), -1)
            {
            }

            public CCollection<Finanziaria.CQSPDConvenzione> GetConvenzioniPerProdotto(Finanziaria.CCQSPDProdotto item, bool onlyValid = false)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                var ret = new CCollection<Finanziaria.CQSPDConvenzione>();
                if (DBUtils.GetID(item) == 0)
                    return ret;
                foreach (Finanziaria.CQSPDConvenzione c in LoadAll())
                {
                    if (c.IDProdotto == DBUtils.GetID(item) && (!onlyValid || c.IsValid()))
                        ret.Add(c);
                }

                return ret;
            }

            public CCollection<Finanziaria.CQSPDConvenzione> GetConvenzioniPerAmministrazione(Anagrafica.CAzienda amm, bool onlyValid = false)
            {
                if (amm is null)
                    throw new ArgumentNullException("amministrazione");
                var ret = new CCollection<Finanziaria.CQSPDConvenzione>();
                if (DBUtils.GetID(amm) == 0)
                    return ret;
                foreach (Finanziaria.CQSPDConvenzione c in LoadAll())
                {
                    if ((c.IDAmministrazione == DBUtils.GetID(amm) || (amm.TipoRapporto ?? "") == (c.TipoRapporto ?? "")) && (!onlyValid || c.IsValid()))
                        ret.Add(c);
                }

                return ret;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CConvenzioniClass m_Convenzioni = null;

        public static CConvenzioniClass Convenzioni
        {
            get
            {
                if (m_Convenzioni is null)
                    m_Convenzioni = new CConvenzioniClass();
                return m_Convenzioni;
            }
        }
    }
}