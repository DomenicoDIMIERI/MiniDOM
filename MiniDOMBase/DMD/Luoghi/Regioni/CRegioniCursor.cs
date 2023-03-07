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
        /// Cursore sulla tabella delle regioni
        /// </summary>
        [Serializable]
        public class CRegioniCursor 
            : minidom.Databases.DBObjectCursorPO<CRegione>
        {
            private DBCursorStringField m_Sigla = new DBCursorStringField("Sigla");
            private DBCursorStringField m_Nazione = new DBCursorStringField("Nazione");
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegioniCursor()
            {
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
            /// Nazione
            /// </summary>
            public DBCursorStringField Nazione
            {
                get
                {
                    return m_Nazione;
                }
            }
 
            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CRegione();
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_Luoghi_Regioni";
            //}


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Regioni; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}
        }
    }
}