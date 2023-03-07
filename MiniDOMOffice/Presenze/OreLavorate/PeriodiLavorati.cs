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
using static minidom.Office;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="PeriodoLavorato"/>
        /// </summary>
        [Serializable]
        public sealed class CPeriodiLavorati 
            : CModulesClass<PeriodoLavorato>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPeriodiLavorati() 
                : base("modOfficePeriodiLavorati", typeof(PeriodoLavoratoCursor), 0)
            {
            }
        }
    }

    public partial class Office
    {
        private static CPeriodiLavorati m_PeriodiLavorati = null;

        /// <summary>
        /// Repository di <see cref="PeriodoLavorato"/>
        /// </summary>
        public static CPeriodiLavorati PeriodiLavorati
        {
            get
            {
                if (m_PeriodiLavorati is null)
                    m_PeriodiLavorati = new CPeriodiLavorati();
                return m_PeriodiLavorati;
            }
        }
    }
}