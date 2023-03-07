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
        /// Repository di oggetti di tipo <see cref="CAssicurazione"/>
        /// </summary>
        [Serializable]
        public sealed class CAssicurazioniClass
           : CModulesClass<CAssicurazione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAssicurazioniClass() 
                : base("modAnaAssicurazioni", typeof(CAssicurazioniCursor), -1)
            {
            }
        }

    }

    public partial class Finanziaria
    {
       
        private static CAssicurazioniClass m_Assicurazioni = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CAssicurazione"/>
        /// </summary>
        public static CAssicurazioniClass Assicurazioni
        {
            get
            {
                if (m_Assicurazioni is null)
                    m_Assicurazioni = new CAssicurazioniClass();
                return m_Assicurazioni;
            }
        }
    }
}