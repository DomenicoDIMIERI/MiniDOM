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
        /// Repository di oggetti <see cref="OggettoCollegato"/>
        /// </summary>
        [Serializable]
        public class OggettiCollegatiRepository
            : CModulesClass<OggettoCollegato>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettiCollegatiRepository() 
                : base("modOggettiCollegati", typeof(OggettoCollegatoCursor), 0)
            {
            
             
            }
        }

        public partial class COggettiInventariatiClass
        {

            private OggettiCollegatiRepository m_OggettiCollegati = null;

            /// <summary>
            /// Repository di oggetti <see cref="OggettoCollegato"/>
            /// </summary>
            public OggettiCollegatiRepository OggettiCollegati
            {
                get
                {
                    if (m_OggettiCollegati is null)
                        m_OggettiCollegati = new OggettiCollegatiRepository();
                    return m_OggettiCollegati;
                }
            }
        }
    }

  
}