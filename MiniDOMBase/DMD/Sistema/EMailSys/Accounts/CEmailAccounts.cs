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
        /// Repository di oggetti di tipo <see cref="CEmailAccount"/>
        /// </summary>
        [Serializable]
        public class CEmailAccounts 
            : CModulesClass<CEmailAccount>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CEmailAccounts()
                : base("modEMailAccounts", typeof(CEmailAccountCursor), -1)
            {
            }
             
        }
    }
}