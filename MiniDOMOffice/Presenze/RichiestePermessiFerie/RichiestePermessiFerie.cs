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
        /// Repository di <see cref="RichiestaPermessoFerie"/>
        /// </summary>
        [Serializable]
        public sealed class CRichiestePermessiFerie 
            : CModulesClass<RichiestaPermessoFerie>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRichiestePermessiFerie() 
                : base("modOfficeRichiestePermF", typeof(RichiestaPermessoFerieCursor), 0)
            {
            }
        }
    }

    public partial class Office
    {
        private static CRichiestePermessiFerie m_RichiestePermessiFerie = null;

        /// <summary>
        /// Repository di <see cref="RichiestaPermessoFerie"/>
        /// </summary>
        public static CRichiestePermessiFerie RichiestePermessiFerie
        {
            get
            {
                if (m_RichiestePermessiFerie is null)
                    m_RichiestePermessiFerie = new CRichiestePermessiFerie();
                return m_RichiestePermessiFerie;
            }
        }
    }
}