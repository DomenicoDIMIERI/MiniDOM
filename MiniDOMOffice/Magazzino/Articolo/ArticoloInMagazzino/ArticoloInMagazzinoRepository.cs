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
        /// Repository di oggetti di tipo <see cref="ArticoloInMagazzino"/>
        /// </summary>
        [Serializable]
        public class ArticoloInMagazzinoRepository
            : CModulesClass<ArticoloInMagazzino>
        {
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloInMagazzinoRepository()
                : base("modStoreGiacenzeArticoli", typeof(ArticoloInMagazzinoCursor), 0)
            {
            }
              
        }

        partial class CArticoliClass
        {

            private ArticoloInMagazzinoRepository m_QuantitaInMagazzino = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="ArticoloInMagazzino"/>
            /// </summary>
            public ArticoloInMagazzinoRepository QuantitaInMagazzino
            {
                get
                {
                    if (this.m_QuantitaInMagazzino is null)
                        this.m_QuantitaInMagazzino = new ArticoloInMagazzinoRepository();
                    return this.m_QuantitaInMagazzino;
                }
            }

        }
    }

    
}