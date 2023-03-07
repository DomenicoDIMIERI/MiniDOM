using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle province
        /// </summary>
        [Serializable]
        public class CProvinceCursor 
            : LuogoCursor<CProvincia>
        {
            private DBCursorStringField m_Sigla;
            private DBCursorStringField m_Regione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProvinceCursor()
            {
                m_Sigla = new DBCursorStringField("Sigla");
                m_Regione = new DBCursorStringField("Regione");
            }

            /// <summary>
            /// Sigla
            /// </summary>
            public DBCursorStringField Sigla
            {
                get
                {
                    return m_Sigla;
                }
            }

            /// <summary>
            /// Regione
            /// </summary>
            public DBCursorStringField Regione
            {
                get
                {
                    return m_Regione;
                }
            }

            //public override object InstantiateNewT(DBReader dbRis)
            //{
            //    return new CProvincia();
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_Luoghi_Province";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Province; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}
        }
    }
}