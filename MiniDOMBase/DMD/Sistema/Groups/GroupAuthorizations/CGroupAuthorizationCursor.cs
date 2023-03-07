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
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella delle autorizzazione di gruppo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CGroupAuthorizationCursor
            : minidom.Databases.DBObjectCursorBase<CGroupAuthorization>
        {
            private DBCursorField<int> m_ActionID = new DBCursorField<int>("Action");
            private DBCursorField<int> m_GroupID = new DBCursorField<int>("Gruppo");
            private DBCursorField<bool> m_Allow = new DBCursorField<bool>("Allow");
            private DBCursorField<bool> m_Negate = new DBCursorField<bool>("Negate");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupAuthorizationCursor()
            {
            }

            /// <summary>
            /// ActionID
            /// </summary>
            public DBCursorField<int> ActionID
            {
                get
                {
                    return m_ActionID;
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
            //    return "tbl_GroupAuthorizations";
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
                return minidom.Sistema.Groups.Authorizations;
            }
        }
    }
}