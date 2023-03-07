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
        /// Repository di <see cref="MarcaturaIngressoUscita"/>
        /// </summary>
        [Serializable]
        public sealed class CMarcatureClass 
            : CModulesClass<MarcaturaIngressoUscita>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMarcatureClass() 
                : base("modOfficeIngressiUscite", typeof(minidom.Office.MarcatureIngressoUscitaCursor))
            {
            }
        }
    }

    public partial class Office
    {
        private static CMarcatureClass m_Marcature = null;

        /// <summary>
        /// Repository di <see cref="MarcaturaIngressoUscita"/>
        /// </summary>
        public static CMarcatureClass Marcature
        {
            get
            {
                if (m_Marcature is null)
                    m_Marcature = new CMarcatureClass();
                return m_Marcature;
            }
        }
    }
}