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
using static minidom.Store;


namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="AttributoCategoria"/>
        /// </summary>
        [Serializable]
        public class AttributoCategoriaRepository
            : CModulesClass<AttributoCategoria>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributoCategoriaRepository()
                : base("modStoreAttributiCategoria", typeof(AttributoCategoriaCursor), 0)
            {

            }

        }

        public partial class CCategorieArticoliClass
        {

            private AttributoCategoriaRepository _Attributi = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="AttributoCategoria"/>
            /// </summary>
            public AttributoCategoriaRepository Attributi
            {
                get
                {
                    if (this._Attributi is null)
                        this._Attributi = new AttributoCategoriaRepository();
                    return this._Attributi;
                }
            }
        }
    }

     
}