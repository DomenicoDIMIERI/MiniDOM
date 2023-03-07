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

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti di tipo <see cref="AzioneEseguita"/>
    /// </summary>
    [Serializable]
    public class AzioniEseguiteRepository
        : CModulesClass<AzioneEseguita>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public AzioniEseguiteRepository()
            : base("modNotificheAzioniEseguite", typeof(AzioneEseguitaCursor ), -1)
        {

        }

         
    }


}
 