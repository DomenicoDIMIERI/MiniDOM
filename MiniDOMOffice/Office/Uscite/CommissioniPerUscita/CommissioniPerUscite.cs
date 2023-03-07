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

        public partial class CUsciteClass
        {

            /// <summary>
            /// Repository di <see cref="CommissionePerUscita"/>
            /// </summary>
            [Serializable]
            public sealed class CommissioniPerUscite
                : CModulesClass<CommissionePerUscita>
            {

                /// <summary>
                /// Costruttore
                /// </summary>
                public CommissioniPerUscite()
                    : base("modOfficeCommissXUscita", typeof(CommissioniPerUscitaCursor), 0)
                {

                }

               
            }
        }
    }

  
}