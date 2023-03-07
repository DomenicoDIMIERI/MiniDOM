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
        /// Repository di oggetti <see cref="PercorsoDefinito"/>
        /// </summary>
        [Serializable]
        public sealed partial class CPercorsiDefinitiClass
            : CModulesClass<PercorsoDefinito>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPercorsiDefinitiClass() 
                : base("modOfficePercorsiDefiniti", typeof(PercorsiDefinitiCursor), -1)
            {
            }
        }

    }

    public partial class Office
    {

    
        private static CPercorsiDefinitiClass m_PercorsiDefiniti = null;

        /// <summary>
        /// Repository di oggetti <see cref="PercorsoDefinito"/>
        /// </summary>
        public static CPercorsiDefinitiClass PercorsiDefiniti
        {
            get
            {
                if (m_PercorsiDefiniti is null)
                    m_PercorsiDefiniti = new CPercorsiDefinitiClass();
                return m_PercorsiDefiniti;
            }
        }
    }
}