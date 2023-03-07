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
        /// Repository di oggetti di tipo <see cref="CUserXGroup"/>
        /// </summary>
        [Serializable]
        public class CUsersXGroupsRepository
            : CModulesClass<CUserXGroup>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUsersXGroupsRepository()
                : base("modUsersXGroupsRepository", typeof(CUserXGroupCursor), -1)
            {

            }

        }

        public partial class CUsersClass
        {

            private CUsersXGroupsRepository m_UsersXGroupsRepository = null;


            /// <summary>
            /// Repository di oggetti di tipo <see cref="CUserXGroup"/>
            /// </summary>
            public CUsersXGroupsRepository UsersXGroupsRepository
            {
                get 
                {
                    if (this.m_UsersXGroupsRepository is null) this.m_UsersXGroupsRepository = new CUsersXGroupsRepository();
                    return this.m_UsersXGroupsRepository;
                }
            }

        }

    }
    
}