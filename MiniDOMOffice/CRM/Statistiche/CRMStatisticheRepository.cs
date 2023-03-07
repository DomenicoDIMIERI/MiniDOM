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
using static minidom.CustomerCalls;


namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Classe di accesso alle statistiche
        /// </summary>
        [Serializable]
        public class CRMStatisticheRepository
        {

             
        }


    }
     

    public partial class CustomerCalls
    {

        private static CRMStatisticheRepository m_Statistiche = null;

        /// <summary>
        /// Statistiche del CRM
        /// </summary>
        public static CRMStatisticheRepository Statistiche
        {
            get
            {
                if (m_Statistiche is null)
                    m_Statistiche = new CRMStatisticheRepository();
                return m_Statistiche;
            }
        }


    }
}