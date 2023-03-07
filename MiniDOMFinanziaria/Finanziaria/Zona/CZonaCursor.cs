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
        /// Costruttore sulla tabella degli oggetti <see cref="CZona"/>
        /// </summary>
        [Serializable]
        public class CZonaCursor 
            : minidom.Databases.DBObjectCursorPO<CZona>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CZonaCursor()
            {
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Zone;
            }
             
        }
    }
}