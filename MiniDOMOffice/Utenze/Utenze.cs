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
        /// Repository di <see cref="Utenza"/>
        /// </summary>
        [Serializable]
        public class UtenzeClass 
            : CModulesClass<Utenza>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public UtenzeClass()
                : base("modOfficeUtenze", typeof(UtenzeCursor), 0)
            {
            }
        }
    }

    public partial class Office
    {
        private static UtenzeClass m_Utenze = null;

        /// <summary>
        /// Repository di <see cref="Utenza"/>
        /// </summary>
        public static UtenzeClass Utenze
        {
            get
            {
                if (m_Utenze is null)
                    m_Utenze = new UtenzeClass();
                return m_Utenze;
            }
        }
    }
}