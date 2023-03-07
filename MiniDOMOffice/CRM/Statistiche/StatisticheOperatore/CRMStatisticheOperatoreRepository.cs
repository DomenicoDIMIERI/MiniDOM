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
        /// Repository di oggetti di tipo <see cref="CRMStatisticheOperatore"/>
        /// </summary>
        [Serializable]
        public class CRMStatisticheOperatoreRepository
            : CModulesClass<CRMStatisticheOperatore>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatisticheOperatoreRepository()
                : base("modCRMStatsOperatore", typeof(CRMStatisticheOperatoreCursor), 0)
            {

            }
        }


        public partial class CRMStatisticheRepository
        {

            private CRMStatisticheOperatoreRepository m_StatisticheOperatore = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CRMStatisticheOperatore"/>
            /// </summary>
            public CRMStatisticheOperatoreRepository StatisticheOperatore
            {
                get
                {
                    if (this.m_StatisticheOperatore is null)
                        this.m_StatisticheOperatore = new CRMStatisticheOperatoreRepository();
                    return this.m_StatisticheOperatore;
                }
            }


        }

    }
     
}