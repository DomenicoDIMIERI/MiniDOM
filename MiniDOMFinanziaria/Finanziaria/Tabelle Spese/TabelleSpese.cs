using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
    /// Classe Generica per l'accesso allae tabelle spese
    /// </summary>
    /// <remarks></remarks>
        public sealed class CTabelleSpeseClass : CModulesClass<Finanziaria.CTabellaSpese>
        {
            internal CTabelleSpeseClass() : base("modCQSPDTabelleSpese", typeof(Finanziaria.CTabellaSpeseCursor), -1)
            {
            }

            public CCollection<Finanziaria.CTabellaSpese> GetTabelleByCessionario(int cid, bool ov = true)
            {
                var ret = new CCollection<Finanziaria.CTabellaSpese>();
                foreach (Finanziaria.CTabellaSpese item in LoadAll())
                {
                    if (item.CessionarioID == cid && (ov == false || item.IsValid()))
                        ret.Add(item);
                }

                return ret;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CTabelleSpeseClass m_TabelleSpese = null;

        public static CTabelleSpeseClass TabelleSpese
        {
            get
            {
                if (m_TabelleSpese is null)
                    m_TabelleSpese = new CTabelleSpeseClass();
                return m_TabelleSpese;
            }
        }
    }
}