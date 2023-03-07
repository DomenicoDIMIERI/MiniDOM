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
        /// Repository di <see cref="RilevatorePresenze"/>
        /// </summary>
        [Serializable]
        public sealed class CRilevatoriPresenzeClass 
            : CModulesClass<RilevatorePresenze>
        {
            
            private DriversRilevatoriPresenze m_Drivers;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRilevatoriPresenzeClass() 
                : base("modOfficeRilevatoriPresenze", typeof(minidom.Office.RilevatoriPresenzeCursor), -1)
            {
            }

            public DriversRilevatoriPresenze Drivers
            {
                get
                {
                    if (m_Drivers is null)
                        m_Drivers = new DriversRilevatoriPresenze();
                    return m_Drivers;
                }
            }
        }
    }

    public partial class Office
    {
        private static CRilevatoriPresenzeClass m_RilevatoriPresenze = null;


        /// <summary>
        /// Repository di <see cref="RilevatorePresenze"/>
        /// </summary>
        [Serializable]
        public static CRilevatoriPresenzeClass RilevatoriPresenze
        {
            get
            {
                if (m_RilevatoriPresenze is null)
                    m_RilevatoriPresenze = new CRilevatoriPresenzeClass();
                return m_RilevatoriPresenze;
            }
        }
    }
}