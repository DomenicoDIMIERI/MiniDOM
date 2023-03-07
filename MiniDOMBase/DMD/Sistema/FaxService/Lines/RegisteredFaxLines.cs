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
using DMD.FAX;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="RegisteredFaxLine"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CRegisteredFaxLinesRepository
            : CModulesClass<RegisteredFaxLine>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredFaxLinesRepository()
                : base("modCRegisteredFaxLines", typeof(RegisteredFaxLineCursor), -1)
            {
            }
             
        }


        partial class CFaxServiceClass
        {

            private CRegisteredFaxLinesRepository m_RegisteredLines = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="RegisteredFaxLine"/>
            /// </summary>
            public CRegisteredFaxLinesRepository RegisteredLines
            {
                get
                {
                    if (m_RegisteredLines is null)
                        m_RegisteredLines = new CRegisteredFaxLinesRepository();
                    return m_RegisteredLines;
                }
            }

            
        }
    }

     
}