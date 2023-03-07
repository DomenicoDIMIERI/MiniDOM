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
        /// Repository di <see cref="MotivoRichiesta"/>  
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CMotiviRichiesteClass
            : CModulesClass<MotivoRichiesta>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMotiviRichiesteClass() 
                : base("modOfficeMotiviRichieste", typeof(MotivoRichiestaCursor), -1)
            {
            }

           
        }

    }

    public partial class Office
    {
        public partial class CRichiesteCERQClass
        {

           

            private CMotiviRichiesteClass m_MotiviRichieste = null;

            /// <summary>
            /// Repository di <see cref="MotivoRichiesta"/>  
            /// </summary>
            /// <remarks></remarks>
            public CMotiviRichiesteClass MotiviRichieste
            {
                get
                {
                    lock (this)
                    {
                        if (m_MotiviRichieste is null)
                            m_MotiviRichieste = new CMotiviRichiesteClass();
                        return m_MotiviRichieste;
                    }
                }
            }

            
        }
    }
}