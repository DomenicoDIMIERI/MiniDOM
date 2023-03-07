using System;
using DMD;
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
        /// Repository di oggetti di tipo <see cref="CCategoriaAzienda"/>
        /// </summary>
        [Serializable]
        public sealed class CCategorieAziendaClass 
            : CModulesClass<Anagrafica.CCategoriaAzienda>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCategorieAziendaClass() 
                : base("modCategorieAzienda", typeof(Anagrafica.CCategorieAziendaCursor), -1)
            {
            }
        }
    }

    public partial class Anagrafica
    {
        private static CCategorieAziendaClass m_CategorieAzienda = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CCategoriaAzienda"/>
        /// </summary>
        public static CCategorieAziendaClass CategorieAzienda
        {
            get
            {
                if (m_CategorieAzienda is null)
                    m_CategorieAzienda = new CCategorieAziendaClass();
                return m_CategorieAzienda;
            }
        }
    }
}