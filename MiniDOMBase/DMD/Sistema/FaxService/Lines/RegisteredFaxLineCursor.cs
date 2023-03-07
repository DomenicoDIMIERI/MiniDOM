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
        /// Cursore sulla tabella degli oggetti <see cref="RegisteredFaxLine"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class RegisteredFaxLineCursor
            : minidom.Databases.DBObjectCursor<RegisteredFaxLine>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorStringField m_Tipologia = new DBCursorStringField("Tipologia");
            private DBCursorStringField m_DialPrefix = new DBCursorStringField("DialPrefix");
            private DBCursorStringField m_Numero = new DBCursorStringField("Numero");
            private DBCursorStringField m_EMailInvio = new DBCursorStringField("EMailInvio");
            private DBCursorStringField m_EMailRicezione = new DBCursorStringField("EMailRicezione");
            private DBCursorStringField m_ServerName = new DBCursorStringField("ServerName");
            private DBCursorField<int> m_ServerPort = new DBCursorField<int>("ServerPort");
            private DBCursorStringField m_Account = new DBCursorStringField("Account");
            private DBCursorStringField m_UserName = new DBCursorStringField("UserName");
            private DBCursorStringField m_Password = new DBCursorStringField("Password");


            /// <summary>
            /// Costruttore
            /// </summary>
            public RegisteredFaxLineCursor()
            {
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.FaxService.RegisteredLines;
            }

            /// <summary>
            /// Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return this.m_Name;
                }
            }

            /// <summary>
            /// Tipologia
            /// </summary>
            public DBCursorStringField Tipologia
            {
                get
                {
                    return this.m_Tipologia;
                }
            }

            /// <summary>
            /// DialPrefix
            /// </summary>
            public DBCursorStringField DialPrefix
            {
                get
                {
                    return this.m_DialPrefix;
                }
            }

            /// <summary>
            /// Numero
            /// </summary>
            public DBCursorStringField Numero
            {
                get
                {
                    return this.m_Numero;
                }
            }

            /// <summary>
            /// EMailInvio
            /// </summary>
            public DBCursorStringField EMailInvio
            {
                get
                {
                    return this.m_EMailInvio;
                }
            }

            /// <summary>
            /// EMailRicezione
            /// </summary>
            public DBCursorStringField EMailRicezione
            {
                get
                {
                    return this.m_EMailRicezione;
                }
            }

            /// <summary>
            /// ServerName
            /// </summary>
            public DBCursorStringField ServerName
            {
                get
                {
                    return this.m_ServerName;
                }
            }

            /// <summary>
            /// ServerPort
            /// </summary>
            public DBCursorField<int> ServerPort
            {
                get
                {
                    return this.m_ServerPort;
                }
            }

            /// <summary>
            /// Account
            /// </summary>
            public DBCursorStringField Account
            {
                get
                {
                    return this.m_Account;
                }
            }

            /// <summary>
            /// UserName
            /// </summary>
            public DBCursorStringField UserName
            {
                get
                {
                    return this.m_UserName;
                }
            }

            /// <summary>
            /// Password
            /// </summary>
            public DBCursorStringField Password
            {
                get
                {
                    return this.m_Password;
                }
            }


        }
    }
}