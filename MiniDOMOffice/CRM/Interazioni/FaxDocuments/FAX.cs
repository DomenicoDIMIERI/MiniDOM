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
        /// Repository di oggetti <see cref="FaxDocument"/>
        /// </summary>
        [Serializable]
        public sealed class CFAXClass
         : CContattiRepository<FaxDocument>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFAXClass()
                : base("modContattiFAX", typeof(FaxDocumentsCursor), 0)
            {
            }

    
        }


    }

    public partial class CustomerCalls
    {


     
        private static CFAXClass m_FAX = null;

        /// <summary>
        /// Repository di oggetti <see cref="FaxDocument"/>
        /// </summary>
        public static CFAXClass FAX
        {
            get
            {
                if (m_FAX is null)
                    m_FAX = new CFAXClass();
                return m_FAX;
            }
        }
    }
}