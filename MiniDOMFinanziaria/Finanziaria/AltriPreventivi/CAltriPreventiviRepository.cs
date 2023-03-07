using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Finanziaria;


namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CAltroPreventivo"/>
        /// </summary>
        [Serializable]
        public class CAltriPreventiviRepository
            : CModulesClass<CAltroPreventivo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAltriPreventiviRepository()
                : base("modAltriPreventivi", typeof(CAltriPreventiviCursor), 0)
            {

            }
        }

    }
    public partial class Finanziaria
    {

        private static CAltriPreventiviRepository m_AltriPreventivi = null;

        /// <summary>
        /// Repository di oggetti <see cref="CAltroPreventivo"/>
        /// </summary>
        public static CAltriPreventiviRepository AltriPreventivi
        {
            get
            {
                if (m_AltriPreventivi is null) m_AltriPreventivi = new CAltriPreventiviRepository();
                return m_AltriPreventivi;
            }
        }

    }
}