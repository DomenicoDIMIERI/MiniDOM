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
        /// Repository di oggetti di tipo <see cref="ArticoloXListino"/>
        /// </summary>
        [Serializable]
        public class ArticoloXListinoRepository
            : CModulesClass<ArticoloXListino>
        {
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloXListinoRepository()
                : base("modStoreListiniArticolo", typeof(ArticoloXListinoCursor), 0)
            {
            }
              
        }

        partial class CArticoliClass
        {

            private ArticoloXListinoRepository m_Listini = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="ArticoloXListino"/>
            /// </summary>
            public ArticoloXListinoRepository Listini
            {
                get
                {
                    if (this.m_Listini is null)
                        this.m_Listini = new ArticoloXListinoRepository();
                    return this.m_Listini;
                }
            }

        }
    }

    
}