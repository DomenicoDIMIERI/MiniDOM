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
        /// Repository di oggetti di tipo <see cref="CGroupAuthorization"/>
        /// </summary>
        [Serializable]
        public class CGroupAuthorizationsRepository
            : CModulesClass<CGroupAuthorization>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupAuthorizationsRepository()
                : base("modCGroupAuthorization", typeof(CGroupAuthorizationCursor), -1)
            {

            }
        }


        public sealed partial class CGroupsClass 
        {

            
             
            private CGroupAuthorizationsRepository m_Authorizations = null;

            /// <summary>
            /// Gruppi predefiniti
            /// </summary>
            public CGroupAuthorizationsRepository Authorizations
            {
                get
                {
                    if (m_Authorizations is null)
                        m_Authorizations = new CGroupAuthorizationsRepository();
                    return m_Authorizations;
                }
            }
        }
    }
     
}