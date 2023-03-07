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
        /// Repository di oggetti di tipo <see cref="CModuleXGroup"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXGroupsRepository
            : CModulesClass<CModuleXGroup>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXGroupsRepository()
                : base("modCModuleXGroupsRepository", typeof(CModuleXGroupCursor), -1)
            {
            }
             
        }

        public partial class CModulesModeClass
        {

            private CModuleXGroupsRepository _ModulesXGroupRepository = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CModuleXGroup"/>
            /// </summary>
            public CModuleXGroupsRepository ModulesXGroupRepository
            {
                get
                {
                    if (this._ModulesXGroupRepository is null) this._ModulesXGroupRepository = new CModuleXGroupsRepository();
                    return this._ModulesXGroupRepository;
                }
            }


        }

    }
}