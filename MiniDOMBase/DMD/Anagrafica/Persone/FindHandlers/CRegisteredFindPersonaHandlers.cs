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
        /// Repository di oggetti di tipo <see cref="CRegisteredFindHandler"/>
        /// </summary>
        [Serializable]
        public class CRegisteredFindPersonaHandlersRepository
            : CModulesClass<CRegisteredFindHandler>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredFindPersonaHandlersRepository()
                : base("modRegisterdFindPersonaHandlers", typeof(CRegisteredFindHandlerCursor), -1)
            {

            }



        }
    }

    public partial class Anagrafica
    {

        private static CRegisteredFindPersonaHandlersRepository m_RegisteredFindPersonaHandlers = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CRegisteredFindHandler"/>
        /// </summary>
        public static CRegisteredFindPersonaHandlersRepository RegisteredFindPersonaHandlers
        {
            get
            {
                if (m_RegisteredFindPersonaHandlers is null) m_RegisteredFindPersonaHandlers = new CRegisteredFindPersonaHandlersRepository();
                return m_RegisteredFindPersonaHandlers;
            }
        }
    }
}