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
        /// Repository di <see cref="AttributoOggetto"/>
        /// </summary>
        [Serializable]
        public class AttributiOggettoRepository
            : CModulesClass<AttributoOggetto>
        {
              
            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributiOggettoRepository()
                : base("modStoreObjInvAttributes", typeof(AttributoOggettoCursor), 0)
            {

            }

        }

        public partial class COggettiInventariatiClass
        {

            private AttributiOggettoRepository m_Attributi = null;

            /// <summary>
            /// Repository di <see cref="AttributoOggetto"/>
            /// </summary>
            public AttributiOggettoRepository Attributi
            {
                get
                {
                    if (this.m_Attributi is null)
                        this.m_Attributi = new AttributiOggettoRepository();
                    return this.m_Attributi;
                }
            }

        }
    }
}