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
using static minidom.CustomerCalls;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CAppunto"/>
        /// </summary>
        [Serializable]
        public sealed class CAppuntiClass 
            : CContattiRepository<CAppunto>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CAppuntiClass() 
                : base("modContattiAppunti", typeof(CAppuntiCursor), 0)
            {
            }
        }

    }

    public partial class CustomerCalls
    {
     
        private static CAppuntiClass m_Appunti = null;

        /// <summary>
        ///  Repository di oggetti di tipo <see cref="CAppunto"/>
        /// </summary>
        public static CAppuntiClass Appunti
        {
            get
            {
                if (m_Appunti is null)
                    m_Appunti = new CAppuntiClass();
                return m_Appunti;
            }
        }
    }
}