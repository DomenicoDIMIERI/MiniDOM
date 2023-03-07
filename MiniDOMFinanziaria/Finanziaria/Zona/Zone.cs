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
using static minidom.Finanziaria;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CZona"/>
        /// </summary>
        [Serializable]
        public sealed class CZoneClass 
            : CModulesClass<CZona>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CZoneClass() 
                : base("modZoneGeografiche", typeof(CZonaCursor), -1)
            {
            }
        }
    }

    public partial class Finanziaria
    {
        private static CZoneClass m_Zone = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CZona"/>
        /// </summary>
        public static CZoneClass Zone
        {
            get
            {
                if (m_Zone is null)
                    m_Zone = new CZoneClass();
                return m_Zone;
            }
        }
    }
}