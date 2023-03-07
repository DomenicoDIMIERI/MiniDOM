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
        /// Cursore sulla tabella dei moduli definiti per gruppo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CModuleXGroupCursor 
            : minidom.Databases.DBObjectCursorBase<CModuleXGroup>
        {

            private DBCursorField<int> m_ModuleID = new DBCursorField<int>("Module");
            private DBCursorField<int> m_GroupID = new DBCursorField<int>("Group");
            private DBCursorField<bool> m_Allow = new DBCursorField<bool>("Allow");
            private DBCursorField<bool> m_Negate = new DBCursorField<bool>("Negate");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleXGroupCursor()
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
            /// GroupID
            /// </summary>
            public DBCursorField<int> GroupID
            {
                get
                {
                    return m_GroupID;
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
            //    return "tbl_ModulesXGroup";
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
                return minidom.Sistema.Modules.ModulesXGroupRepository;
            }
        }
    }
}