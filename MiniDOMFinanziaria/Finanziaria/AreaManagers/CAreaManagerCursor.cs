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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Cursore di oggetti Area Manager
        /// </summary>
        [Serializable]
        public class CAreaManagerCursor 
            : CTeamManagersCursor
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAreaManagerCursor()
            {
            }

         
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.AreaManagers;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_AreaManagers";
            //}

            public override CTeamManager InstantiateNew(DBReader dbRis)
            {
                return new CAreaManager();
            }
        }
    }
}