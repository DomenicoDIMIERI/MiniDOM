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
        /// Cursore sulla tabella dei log di accesso
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CLoginHistoryCursor 
            : minidom.Databases.DBObjectCursorBase<CLoginHistory>
        {
            private DBCursorField<int> m_UserID;
            private DBCursorField<DateTime> m_LoginTime;
            private DBCursorField<DateTime> m_LogoutTime;
            private DBCursorField<LogOutMethods> m_LogoutMethod;
            private DBCursorStringField m_RemoteIP;
            private DBCursorField<int> m_RemotePort;
            private DBCursorStringField m_UserAgent;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CLoginHistoryCursor()
            {
                m_UserID = new DBCursorField<int>("User");
                m_LoginTime = new DBCursorField<DateTime>("LogInTime");
                m_LogoutTime = new DBCursorField<DateTime>("LogOutTime");
                m_LogoutMethod = new DBCursorField<LogOutMethods>("LogoutMethod");
                m_RemoteIP = new DBCursorStringField("RemoteIP");
                m_RemotePort = new DBCursorField<int>("RemotePort");
                m_UserAgent = new DBCursorStringField("UserAgent");
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
            /// LoginTime
            /// </summary>
            public DBCursorField<DateTime> LoginTime
            {
                get
                {
                    return m_LoginTime;
                }
            }

            /// <summary>
            /// LogoutTime
            /// </summary>
            public DBCursorField<DateTime> LogoutTime
            {
                get
                {
                    return m_LogoutTime;
                }
            }

            /// <summary>
            /// LogoutMethod
            /// </summary>
            public DBCursorField<LogOutMethods> LogoutMethod
            {
                get
                {
                    return m_LogoutMethod;
                }
            }

            /// <summary>
            /// RemoteIP
            /// </summary>
            public DBCursorStringField RemoteIP
            {
                get
                {
                    return m_RemoteIP;
                }
            }

            /// <summary>
            /// RemotePort
            /// </summary>
            public DBCursorField<int> RemotePort
            {
                get
                {
                    return m_RemotePort;
                }
            }

            /// <summary>
            /// UserAgent
            /// </summary>
            public DBCursorStringField UserAgent
            {
                get
                {
                    return m_UserAgent;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_LoginHistory";
            //}

            // Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
            // Dim col As CCursorFieldsCollection
            // col = MyBase.GetCursorFields
            // col.Add(m_UserID)
            // col.Add(m_LoginTime)
            // col.Add(m_LogoutTime)
            // col.Add(m_LogoutMethod)
            // col.Add(m_RemoteIP)
            // col.Add(m_RemotePort)
            // col.Add(m_UserAgent)
            // Return col
            // End Function

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CLoginHistory InstantiateNewT(DBReader dbRis)
            {
                return new CLoginHistory();
            }

            /// <summary>
            /// Respository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Users.LoginHistories;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return LOGConn;
            //}
        }
    }
}