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
        /// Repository di oggetti di tipo <see cref="CodiceArticolo"/>
        /// </summary>
        [Serializable]
        public class CCodiciArticoloRepository
            : CModulesClass<CodiceArticolo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCodiciArticoloRepository()
                : base("modStoreCodiciArticolo", typeof(CodiceArticoloCursor), 0)
            {

            }

        }

        partial class CArticoliClass
        {

            private CCodiciArticoloRepository _Codici = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CodiceArticolo"/>
            /// </summary>
            public CCodiciArticoloRepository Codici
            {
                get
                {
                    if (this._Codici is null)
                        this._Codici = new CCodiciArticoloRepository();
                    return this._Codici;
                }
            }
        }
    }

     
}