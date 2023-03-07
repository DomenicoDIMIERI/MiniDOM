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
        /// Cursore di <see cref="MailAttachment"/>
        /// </summary>
        [Serializable]
        public class MailAttachmentCursor 
            : minidom.Databases.DBObjectCursor<MailAttachment>
        {
            private DBCursorField<int> m_MessageID = new DBCursorField<int>("MessageID");
            private DBCursorStringField m_FileName = new DBCursorStringField("FileName");
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorStringField m_ContentID = new DBCursorStringField("ContentID");
            private DBCursorStringField m_ContentType = new DBCursorStringField("ContentType");
            private DBCursorStringField m_ContentDisposition = new DBCursorStringField("ContentDisposition");
            private DBCursorField<int> m_FileSize = new DBCursorField<int>("FileSize");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAttachmentCursor()
            {
            }

            /// <summary>
            /// MessageID
            /// </summary>
            public DBCursorField<int> MessageID
            {
                get
                {
                    return m_MessageID;
                }
            }

            /// <summary>
            /// FileName
            /// </summary>
            public DBCursorStringField FileName
            {
                get
                {
                    return m_FileName;
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
            /// ContentID
            /// </summary>
            public DBCursorStringField ContentID
            {
                get
                {
                    return m_ContentID;
                }
            }

            /// <summary>
            /// ContentType
            /// </summary>
            public DBCursorStringField ContentType
            {
                get
                {
                    return m_ContentType;
                }
            }

            /// <summary>
            /// ContentDisposition
            /// </summary>
            public DBCursorStringField ContentDisposition
            {
                get
                {
                    return m_ContentDisposition;
                }
            }

            /// <summary>
            /// FileSize
            /// </summary>
            public DBCursorField<int> FileSize
            {
                get
                {
                    return m_FileSize;
                }
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Attachments;
            }
             
        }
    }
}