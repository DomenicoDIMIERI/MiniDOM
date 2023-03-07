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
        /// Cursore sulla tabella dei token di sincronizzazione
        /// </summary>
        [Serializable]
        public sealed class CSecurityTokenCursor
            : minidom.Databases.DBObjectCursor<CSecurityToken>
        {
            private DBCursorStringField m_TokenID = new DBCursorStringField("Token");
            private DBCursorStringField m_TokenName = new DBCursorStringField("TokenName");
            private DBCursorStringField m_Valore = new DBCursorStringField("Valore");
            private DBCursorStringField m_Session = new DBCursorStringField("Session");
            private DBCursorField<int> m_UsatoDaID = new DBCursorField<int>("UsatoDa");
            private DBCursorField<DateTime> m_UsatoIl = new DBCursorField<DateTime>("UsatoIl");
            private DBCursorStringField m_Dettaglio = new DBCursorStringField("Dettaglio");
            private DBCursorField<DateTime> m_ExpireTime = new DBCursorField<DateTime>("ExpireTime");
            private DBCursorField<int> m_ExpireCount = new DBCursorField<int>("ExpireCount");
            private DBCursorField<int> m_UseCount = new DBCursorField<int>("UseCount");
            private DBCursorStringField m_TokenSourceName = new DBCursorStringField("TokenSourceName");
            private DBCursorField<int> m_TokenSourceID = new DBCursorField<int>("TokenSourceID");
            private DBCursorStringField m_TokenClass = new DBCursorStringField("TokenClass");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSecurityTokenCursor()
            {
            }

            /// <summary>
            /// TokenID
            /// </summary>
            public DBCursorStringField TokenID
            {
                get
                {
                    return m_TokenID;
                }
            }

            /// <summary>
            /// TokenClass
            /// </summary>
            public DBCursorStringField TokenClass
            {
                get
                {
                    return m_TokenClass;
                }
            }

            /// <summary>
            /// TokenSourceName
            /// </summary>
            public DBCursorStringField TokenSourceName
            {
                get
                {
                    return m_TokenSourceName;
                }
            }

            /// <summary>
            /// TokenSourceID
            /// </summary>
            public DBCursorField<int> TokenSourceID
            {
                get
                {
                    return m_TokenSourceID;
                }
            }

            /// <summary>
            /// TokenName
            /// </summary>
            public DBCursorStringField TokenName
            {
                get
                {
                    return m_TokenName;
                }
            }

            /// <summary>
            /// Valore
            /// </summary>
            public DBCursorStringField Valore
            {
                get
                {
                    return m_Valore;
                }
            }

            /// <summary>
            /// Session
            /// </summary>
            public DBCursorStringField Session
            {
                get
                {
                    return m_Session;
                }
            }
                

            /// <summary>
            /// UsatoDaID
            /// </summary>
            public DBCursorField<int> UsatoDaID
            {
                get
                {
                    return m_UsatoDaID;
                }
            }

            /// <summary>
            /// UsatoIl
            /// </summary>
            public DBCursorField<DateTime> UsatoIl
            {
                get
                {
                    return m_UsatoIl;
                }
            }

            /// <summary>
            /// Dettaglio
            /// </summary>
            public DBCursorStringField Dettaglio
            {
                get
                {
                    return m_Dettaglio;
                }
            }

            /// <summary>
            /// ExpireTime
            /// </summary>
            public DBCursorField<DateTime> ExpireTime
            {
                get
                {
                    return m_ExpireTime;
                }
            }

            /// <summary>
            /// ExpireCount
            /// </summary>
            public DBCursorField<int> ExpireCount
            {
                get
                {
                    return m_ExpireCount;
                }
            }

            /// <summary>
            /// UseCount
            /// </summary>
            public DBCursorField<int> UseCount
            {
                get
                {
                    return m_UseCount;
                }
            }

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
                return minidom.Sistema.ASPSecurity;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SecurityTokens";
            }
        }
    }
}