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
        /// Cursore di <see cref="MailAccount"/>
        /// </summary>
        [Serializable]
        public class MailAccountCursor 
            : minidom.Databases.DBObjectCursorPO<MailAccount>
        {
            private DBCursorStringField m_AccountName = new DBCursorStringField("AccountName");
            private DBCursorField<int> m_DefaultFolderID = new DBCursorField<int>("DefaultFolderID");
            private DBCursorStringField m_UserName = new DBCursorStringField("UserName");
            private DBCursorStringField m_Password = new DBCursorStringField("Password");
            private DBCursorStringField m_ServerName = new DBCursorStringField("ServerName");
            private DBCursorField<int> m_ServerPort = new DBCursorField<int>("ServerPort");
            private DBCursorStringField m_eMailAddress = new DBCursorStringField("eMailAddress");
            private DBCursorStringField m_Protocol = new DBCursorStringField("Protocol");
            private DBCursorField<bool> m_UseSSL = new DBCursorField<bool>("UseSSL");
            private DBCursorStringField m_SMTPServerName = new DBCursorStringField("SMTPServerName");
            private DBCursorField<int> m_SMTPPort = new DBCursorField<int>("SMTPPort");
            private DBCursorStringField m_ReplayTo = new DBCursorStringField("ReplayTo");
            private DBCursorStringField m_DisplayName = new DBCursorStringField("DisplayName");
            private DBCursorStringField m_SMTPUserName = new DBCursorStringField("SMTPUserName");
            private DBCursorStringField m_SMTPPassword = new DBCursorStringField("SMTPPassword");
            private DBCursorField<bool> m_PopBeforeSMPT = new DBCursorField<bool>("PopBeforeSMTP");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_DelServerAfterDays = new DBCursorField<int>("DelAfterDays");
            private DBCursorField<int> m_TimeOut = new DBCursorField<int>("TimeOut");
            private DBCursorField<SMTPTipoCrittografica> m_SMTPCrittografia = new DBCursorField<SMTPTipoCrittografica>("SMTPCrittografia");
            private DBCursorField<DateTime> m_LastSync = new DBCursorField<DateTime>("LastSync");
            private DBCursorStringField m_FirmaPerNuoviMessaggi = new DBCursorStringField("FirmaPerNuoviMessaggi");
            private DBCursorStringField m_FirmaPerRisposte = new DBCursorStringField("FirmaPerRisposte");
            private DBCursorField<int> m_ApplicationID = new DBCursorField<int>("ApplicationID");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAccountCursor()
            {
            }

         

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Accounts;
            }

            /// <summary>
            /// ApplicationID
            /// </summary>
            public DBCursorField<int> ApplicationID
            {
                get
                {
                    return m_ApplicationID;
                }
            }

            /// <summary>
            /// FirmaPerNuoviMessaggi
            /// </summary>
            public DBCursorStringField FirmaPerNuoviMessaggi
            {
                get
                {
                    return m_FirmaPerNuoviMessaggi;
                }
            }

            /// <summary>
            /// FirmaPerRisposte
            /// </summary>
            public DBCursorStringField FirmaPerRisposte
            {
                get
                {
                    return m_FirmaPerRisposte;
                }
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
            /// SMTPCrittografia
            /// </summary>
            public DBCursorField<SMTPTipoCrittografica> SMTPCrittografia
            {
                get
                {
                    return m_SMTPCrittografia;
                }
            }

            /// <summary>
            /// TimeOut
            /// </summary>
            public DBCursorField<int> TimeOut
            {
                get
                {
                    return m_TimeOut;
                }
            }

            /// <summary>
            /// AccountName
            /// </summary>
            public DBCursorStringField AccountName
            {
                get
                {
                    return m_AccountName;
                }
            }

            /// <summary>
            /// DefaultFolderID
            /// </summary>
            public DBCursorField<int> DefaultFolderID
            {
                get
                {
                    return m_DefaultFolderID;
                }
            }

            /// <summary>
            /// UserName
            /// </summary>
            public DBCursorStringField UserName
            {
                get
                {
                    return m_UserName;
                }
            }

            /// <summary>
            /// Password
            /// </summary>
            public DBCursorStringField Password
            {
                get
                {
                    return m_Password;
                }
            }

            /// <summary>
            /// ServerName
            /// </summary>
            public DBCursorStringField ServerName
            {
                get
                {
                    return m_ServerName;
                }
            }

            /// <summary>
            /// ServerPort
            /// </summary>
            public DBCursorField<int> ServerPort
            {
                get
                {
                    return m_ServerPort;
                }
            }

            /// <summary>
            /// eMailAddress
            /// </summary>
            public DBCursorStringField eMailAddress
            {
                get
                {
                    return m_eMailAddress;
                }
            }

            /// <summary>
            /// Protocol
            /// </summary>
            public DBCursorStringField Protocol
            {
                get
                {
                    return m_Protocol;
                }
            }

            /// <summary>
            /// UseSSL
            /// </summary>
            public DBCursorField<bool> UseSSL
            {
                get
                {
                    return m_UseSSL;
                }
            }

            /// <summary>
            /// SMTPServerName
            /// </summary>
            public DBCursorStringField SMTPServerName
            {
                get
                {
                    return m_SMTPServerName;
                }
            }

            /// <summary>
            /// SMTPPort
            /// </summary>
            public DBCursorField<int> SMTPPort
            {
                get
                {
                    return m_SMTPPort;
                }
            }

            /// <summary>
            /// ReplayTo
            /// </summary>
            public DBCursorStringField ReplayTo
            {
                get
                {
                    return m_ReplayTo;
                }
            }

            /// <summary>
            /// DisplayName
            /// </summary>
            public DBCursorStringField DisplayName
            {
                get
                {
                    return m_DisplayName;
                }
            }

            /// <summary>
            /// SMTPUserName
            /// </summary>
            public DBCursorStringField SMTPUserName
            {
                get
                {
                    return m_SMTPUserName;
                }
            }

            /// <summary>
            /// SMTPPassword
            /// </summary>
            public DBCursorStringField SMTPPassword
            {
                get
                {
                    return m_SMTPPassword;
                }
            }

            /// <summary>
            /// PopBeforeSMPT
            /// </summary>
            public DBCursorField<bool> PopBeforeSMPT
            {
                get
                {
                    return m_PopBeforeSMPT;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// DelServerAfterDays
            /// </summary>
            public DBCursorField<int> DelServerAfterDays
            {
                get
                {
                    return m_DelServerAfterDays;
                }
            }
        }
    }
}