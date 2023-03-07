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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella dei moduli definiti per utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXUserCursor
            : minidom.Databases.DBObjectCursorBase<CModuleXUser>
        {
            private DBCursorField<int> m_ModuleID = new DBCursorField<int>("Module");
            private DBCursorField<int> m_UserID = new DBCursorField<int>("User");
            private DBCursorField<bool> m_Allow = new DBCursorField<bool>("Allow");
            private DBCursorField<bool> m_Negate = new DBCursorField<bool>("Negate");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXUserCursor()
            {
            }

            /// <summary>
            /// ModuleID
            /// </summary>
            public DBCursorField<int> ModuleID
            {
                get
                {
                    return m_ModuleID;
                }
            }

            /// <summary>
            /// UserID
            /// </summary>
            public DBCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
                }
            }

            /// <summary>
            /// Allow
            /// </summary>
            public DBCursorField<bool> Allow
            {
                get
                {
                    return m_Allow;
                }
            }

            /// <summary>
            /// Negate
            /// </summary>
            public DBCursorField<bool> Negate
            {
                get
                {
                    return m_Negate;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_ModulesXUser";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules.ModulesXUserRepository;
            }
        }
    }
}