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
        /// Cursore sulla tabella delle autorizzazione utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CUserAuthorizationCursor 
            : Databases.DBObjectCursorBase<CUserAuthorization>
        {
            
            private DBCursorField<int> m_ActionID = new DBCursorField<int>("Action");
            private DBCursorField<int> m_UserID = new DBCursorField<int>("User");
            private DBCursorField<bool> m_Allow = new DBCursorField<bool>("Allow");
            private DBCursorField<bool> m_Negate = new DBCursorField<bool>("Negate");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserAuthorizationCursor()
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
            //    return "tbl_UserAuthorizations";
            //}

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Users.UserAuthorizations;
            }
        }
    }
}