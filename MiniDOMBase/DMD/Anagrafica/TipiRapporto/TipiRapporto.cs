using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CTipoRapporto"/>
        /// </summary>
        [Serializable]
        public sealed class CTipiRapportoClass 
            : CModulesClass<Anagrafica.CTipoRapporto>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTipiRapportoClass()
                : base("modTipiRapporto", typeof(Anagrafica.CTipoRapportoCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base all'id
            /// </summary>
            /// <param name="sigla"></param>
            /// <returns></returns>
            public Anagrafica.CTipoRapporto GetItemByIdTipoRapporto(string sigla)
            {
                sigla = DMD.Strings.Left(DMD.Strings.Trim(sigla), 1);
                if (string.IsNullOrEmpty(sigla))
                    return null;
                foreach (Anagrafica.CTipoRapporto item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.IdTipoRapporto, sigla, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce l'elemento in base alla descrizione
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Anagrafica.CTipoRapporto GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (Anagrafica.CTipoRapporto item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Descrizione, name, true) == 0)
                        return item;
                }

                return null;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CTipiRapportoClass m_TipiRapporto = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CTipoRapporto"/>
        /// </summary>
        public static CTipiRapportoClass TipiRapporto
        {
            get
            {
                if (m_TipiRapporto is null)
                    m_TipiRapporto = new CTipiRapportoClass();
                return m_TipiRapporto;
            }
        }
    }
}