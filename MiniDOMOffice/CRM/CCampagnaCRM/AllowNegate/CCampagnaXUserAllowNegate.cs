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
    public partial class CustomerCalls
    {

        /// <summary>
        /// Autorizzazione o negazione esplicita per un utente
        /// </summary>
        [Serializable]
        public class CCampagnaXUserAllowNegate 
            : UserAllowNegate<CCampagnaCRM>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagnaXUserAllowNegate()
            {
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return null;
            }


            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return null;
            }
 
            /// <summary>
            /// ID
            /// </summary>
            /// <returns></returns>
            protected override string GetItemFieldName()
            {
                return "";
            }
        }
    }
}