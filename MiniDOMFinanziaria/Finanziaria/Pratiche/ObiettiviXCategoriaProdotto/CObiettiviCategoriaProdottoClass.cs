using System;

namespace minidom
{
    namespace CQSPDInternals
    {
        public class CObiettiviCategoriaProdottoClass : CModulesClass<Finanziaria.CObiettivoCategoriaProdotto>
        {
            public CObiettiviCategoriaProdottoClass() : base("modCObiettiviCategoriaProdotto", typeof(Finanziaria.CObiettivoCategoriaProdottoCursor), -1)
            {
            }

            /// <summary>
        /// Restituisce tutti gli obiettivi attivi e validi alla data indicata
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CObiettivoCategoriaProdotto> GetObiettiviAl(DateTime d)
            {
                var ret = new CCollection<Finanziaria.CObiettivoCategoriaProdotto>();
                lock (this)
                {
                    foreach (Finanziaria.CObiettivoCategoriaProdotto o in LoadAll())
                    {
                        if (o.Stato == ObjectStatus.OBJECT_VALID && o.IsValid(d))
                        {
                            ret.Add(o);
                        }
                    }
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
        /// Restituisce gli obiettivi validi alla data per l'ufficio specificato
        /// </summary>
        /// <param name="po"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CObiettivoCategoriaProdotto> GetObiettiviAl(Anagrafica.CUfficio po, DateTime d)
            {
                var ret = new CCollection<Finanziaria.CObiettivoCategoriaProdotto>();
                if (po is null)
                    throw new ArgumentNullException("po");
                lock (this)
                {
                    foreach (Finanziaria.CObiettivoCategoriaProdotto o in LoadAll())
                    {
                        if (o.Stato == ObjectStatus.OBJECT_VALID && (o.IDPuntoOperativo == 0 || o.IDPuntoOperativo == DBUtils.GetID(po)) && o.IsValid(d))
                        {
                            ret.Add(o);
                        }
                    }
                }

                ret.Sort();
                return ret;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CQSPDInternals.CObiettiviCategoriaProdottoClass m_ObiettiviXCategoria;

        public static CQSPDInternals.CObiettiviCategoriaProdottoClass ObiettiviXCategoria
        {
            get
            {
                if (m_ObiettiviXCategoria is null)
                    m_ObiettiviXCategoria = new CQSPDInternals.CObiettiviCategoriaProdottoClass();
                return m_ObiettiviXCategoria;
            }
        }
    }
}