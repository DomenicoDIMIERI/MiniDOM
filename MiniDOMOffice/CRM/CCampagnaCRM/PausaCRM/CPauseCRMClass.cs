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
        /// Repository di oggetti di tipo <see cref="CSessioneCRM"/>
        /// </summary>
        [Serializable]
        public sealed class CPauseCRMClass 
            : CModulesClass<CustomerCalls.CSessioneCRM>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CPauseCRMClass() 
                : base("modCRMPause", typeof(CustomerCalls.CPausaCRMCursor), 0)
            {
            }

             
        }
    }

    public partial class CustomerCalls
    {
        private static CPauseCRMClass m_PauseCRM = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CSessioneCRM"/>
        /// </summary>
        public static CPauseCRMClass PauseCRM
        {
            get
            {
                if (m_PauseCRM is null)
                    m_PauseCRM = new CPauseCRMClass();
                return m_PauseCRM;
            }
        }
    }
}