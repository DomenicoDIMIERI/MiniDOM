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
        /// Repository di <see cref="Scansione"/>
        /// </summary>
        [Serializable]
        public sealed class CScansioniClass 
            : CModulesClass<Scansione>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CScansioniClass() 
                : base("modOfficeScansioni", typeof(minidom.Office.ScansioneCursor), 0)
            {
            }
        }
    }

    public partial class Office
    {
        private static CScansioniClass m_Scansioni = null;

        /// <summary>
        /// Repository di <see cref="Scansione"/>
        /// </summary>
        public static CScansioniClass Scansioni
        {
            get
            {
                if (m_Scansioni is null)
                    m_Scansioni = new CScansioniClass();
                return m_Scansioni;
            }
        }
    }
}