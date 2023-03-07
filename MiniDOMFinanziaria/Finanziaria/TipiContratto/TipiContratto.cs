using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public sealed class CTipiContrattoClass : CModulesClass<CTipoContratto>
        {
            internal CTipiContrattoClass() : base("modTipiContratto", typeof(CTipoContrattoCursor), -1)
            {
            }

            public CTipoContratto GetItemByIdTipoContratto(string sigla)
            {
                sigla = Strings.Left(Strings.Trim(sigla), 1);
                if (string.IsNullOrEmpty(sigla))
                    return null;
                foreach (CTipoContratto item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.IdTipoContratto, sigla, true) == 0)
                        return item;
                }

                return null;
            }

            public CTipoContratto GetItemByName(string name)
            {
                name = Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (CTipoContratto item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Descrizione, name, true) == 0)
                        return item;
                }

                return null;
            }
        }

        private static CTipiContrattoClass m_TipiContratto = null;

        public static CTipiContrattoClass TipiContratto
        {
            get
            {
                if (m_TipiContratto is null)
                    m_TipiContratto = new CTipiContrattoClass();
                return m_TipiContratto;
            }
        }
    }
}