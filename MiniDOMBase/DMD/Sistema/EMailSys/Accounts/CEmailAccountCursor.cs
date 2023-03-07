using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella di <see cref="CEmailAccount"/>
        /// </summary>
        [Serializable]
        public class CEmailAccountCursor
            : minidom.Databases.DBObjectCursor<CEmailAccount>
        {
            private DBCursorField<bool> m_Attivo;
            private DBCursorStringField m_AccountType;
            private DBCursorStringField m_AccountName;
            private DBCursorStringField m_POPServer;
            private DBCursorField<int> m_POPPort;
            private DBCursorStringField m_POPUserName;
            private DBCursorStringField m_POPPassword;
            private DBCursorField<bool> m_POPUseSSL;
            private DBCursorField<DateTime> m_LastSync;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEmailAccountCursor()
            {
                m_Attivo = new DBCursorField<bool>("Attivo");
                m_AccountName = new DBCursorStringField("AccountName");
                m_AccountType = new DBCursorStringField("AccountType");
                m_POPServer = new DBCursorStringField("POPServer");
                m_POPPort = new DBCursorField<int>("POPPort");
                m_POPUserName = new DBCursorStringField("POPUserName");
                m_POPPassword = new DBCursorStringField("POPPassword");
                m_POPUseSSL = new DBCursorField<bool>("POPUseSSL");
                m_LastSync = new DBCursorField<DateTime>("LastSync");
            }



            /// <summary>
            /// LastSync
            /// </summary>
            public DBCursorField<DateTime> LastSync
            {
                get
                {
                    return m_LastSync;
                }
            }


            /// <summary>
            /// Attivo
            /// </summary>
            public DBCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
                }
            }

            /// <summary>
            /// AccountType
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorStringField AccountType
            {
                get
                {
                    return m_AccountType;
                }

            }

            /// <summary>
            /// POPServer
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorStringField POPServer
            {
                get
                {
                    return m_POPServer;
                }
            }

            /// <summary>
            /// POPPort
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorField<int> POPPort
            {
                get
                {
                    return m_POPPort;
                }

            }

            /// <summary>
            /// POPUseSSL
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorField<bool> POPUseSSL
            {
                get
                {
                    return m_POPUseSSL;
                }
            }


            /// <summary>
            /// POPUserName
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorStringField POPUserName
            {
                get
                {
                    return m_POPUserName;
                }
            }

            /// <summary>
            /// POPPassword
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorStringField POPPassword
            {
                get
                {
                    return m_POPPassword;
                }
            }

            /// <summary>
            /// AccountName
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBCursorStringField AccountName
            {
                get
                {
                    return m_AccountName;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.EMailer.MailAccounts;
            }

        }
    }
}