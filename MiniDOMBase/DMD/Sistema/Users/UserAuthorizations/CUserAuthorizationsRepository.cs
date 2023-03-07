using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CUserAuthorization"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CUserAuthorizationsRepository
            : CModulesClass<CUserAuthorization>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserAuthorizationsRepository()
                : base("modUserAuthorizations", typeof(CUserAuthorizationCursor), -1)
            {

            }
        }
    }
}