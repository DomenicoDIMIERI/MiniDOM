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
        /// Repository di oggetti di tipo <see cref="CModuleXUser"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXUsersRepository
            : CModulesClass<CModuleXUser>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXUsersRepository()
                : base("modCModuleXUsersRepository", typeof(CModuleXUserCursor), -1)
            {
            }
             
        }

        public partial class CModulesModeClass
        {

            private CModuleXUsersRepository _ModulesXUserRepository = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CModuleXUser"/>
            /// </summary>
            public CModuleXUsersRepository ModulesXUserRepository
            {
                get
                {
                    if (this._ModulesXUserRepository is null) this._ModulesXUserRepository = new CModuleXUsersRepository();
                    return this._ModulesXUserRepository;
                }
            }


        }
    
    }
     

}