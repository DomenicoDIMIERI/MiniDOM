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
        /// Repository di oggetti di tipo <see cref="AttributoArticolo"/>
        /// </summary>
        [Serializable]
        public class CAttributiArticoloRepository
            : CModulesClass<AttributoArticolo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAttributiArticoloRepository()
                : base("modStoreAttributiArticolo", typeof(AttributoArticoloCursor), 0)
            {

            }

        }

        partial class CArticoliClass
        {

            private CAttributiArticoloRepository _Attributi = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="AttributoArticolo"/>
            /// </summary>
            public CAttributiArticoloRepository Attributi
            {
                get
                {
                    if (this._Attributi is null)
                        this._Attributi = new CAttributiArticoloRepository();
                    return this._Attributi;
                }
            }
        }
    }

     
}