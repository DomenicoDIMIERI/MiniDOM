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
        /// Repository di oggetti <see cref="LuogoDaVisitare"/>
        /// </summary>
        [Serializable]
        public sealed class CLuoghiDaVisitareClass
            : CModulesClass<LuogoDaVisitare>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CLuoghiDaVisitareClass() 
                : base("modOfficePercorsiLuoghiDaVisitare", typeof(LuoghiDaVisitareCursor))
            {
            }
        }
     
        public partial class CPercorsiDefinitiClass
        {

            private CLuoghiDaVisitareClass m_LuoghiDaVisitare = null;

            /// <summary>
            /// Repository di oggetti <see cref="LuogoDaVisitare"/>
            /// </summary>
            public CLuoghiDaVisitareClass LuoghiDaVisitare
            {
                get
                {
                    if (m_LuoghiDaVisitare is null)
                        m_LuoghiDaVisitare = new CLuoghiDaVisitareClass();
                    return m_LuoghiDaVisitare;
                }
            }
        }
    }
}