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
        /// Cursore di <see cref="MailFolder"/>
        /// </summary>
        [Serializable]
        public class MailFolderCursor 
            : minidom.Databases.DBObjectCursorPO<MailFolder>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorField<int> m_ParentID = new DBCursorField<int>("ParentID");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDUtente = new DBCursorField<int>("IDUtente");
            private DBCursorField<int> m_ApplicationID = new DBCursorField<int>("ApplicationID");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailFolderCursor()
            {
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
            /// ParentID
            /// </summary>
            public DBCursorField<int> ParentID
            {
                get
                {
                    return m_ParentID;
                }
            }

            /// <summary>
            /// Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// IDutente
            /// </summary>
            public DBCursorField<int> IDutente
            {
                get
                {
                    return m_IDUtente;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Folders;
            }
             
        }
    }
}