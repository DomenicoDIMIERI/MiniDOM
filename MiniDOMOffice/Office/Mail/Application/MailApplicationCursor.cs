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
        /// Cursore di <see cref="MailApplication"/>
        /// </summary>
        [Serializable]
        public class MailApplicationCursor 
            : minidom.Databases.DBObjectCursorPO<MailApplication>
        {
            private DBCursorField<int> m_UserID = new DBCursorField<int>("UserID");
            private DBCursorStringField m_UserName = new DBCursorStringField("UserName");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_RootID = new DBCursorField<int>("RootID");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailApplicationCursor()
            {
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
            /// RootID
            /// </summary>
            public DBCursorField<int> RootID
            {
                get
                {
                    return m_RootID;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Applications;
            }
             
        }
    }
}