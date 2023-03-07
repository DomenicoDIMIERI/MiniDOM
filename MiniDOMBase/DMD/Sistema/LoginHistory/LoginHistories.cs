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

namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CLoginHistory"/>
        /// </summary>
        [Serializable]
        public class CLoginHistoryRepository
            : CModulesClass<CLoginHistory>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CLoginHistoryRepository()
                : base("modUserLoginHistory", typeof(CLoginHistoryCursor), 0)
            {

            }

        }

    }

    
}