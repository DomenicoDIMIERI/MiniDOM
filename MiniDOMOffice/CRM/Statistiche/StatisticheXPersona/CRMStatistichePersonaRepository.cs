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
        /// Repository di oggetti di tipo <see cref="CRMStatistichePersona"/>
        /// </summary>
        [Serializable]
        public class CRMStatistichePersonaRepository
            : CModulesClass<CRMStatistichePersona>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatistichePersonaRepository()
                : base("modCRMStatsPersone", typeof(CRMStatistichePersonaCursor), 0)
            {

            }
        }


        public partial class CRMStatisticheRepository
        {

            private CRMStatistichePersonaRepository m_StatistichePersona = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CRMStatistichePersona"/>
            /// </summary>
            public CRMStatistichePersonaRepository StatistichePersona
            {
                get
                {
                    if (this.m_StatistichePersona is null)
                        this.m_StatistichePersona = new CRMStatistichePersonaRepository();
                    return this.m_StatistichePersona;
                }
            }


        }

    }
     
}