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
    public partial class Office
    {


        /// <summary>
        /// Cursore sulla tabella dei motivi delle commissioni
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class MotivoCommissioneCursor
            : minidom.Databases.DBObjectCursor<MotivoCommissione>
        {

            private DBCursorStringField m_Motivo = new DBCursorStringField("Motivo");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotivoCommissioneCursor()
            {

            }

            /// <summary>
            /// Motivo
            /// </summary>
            public DBCursorStringField Motivo
            {
                get
                {
                    return m_Motivo;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Commissioni.Motivi;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_OfficeCommissioniM";
            //}

            //protected override Databases.CDBConnection GetConnection()
            //{
            //    return Database;
            //}
        }
    }
}