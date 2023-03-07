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
        /// Cursore di <see cref="DocumentTemplate"/>
        /// </summary>
        [Serializable]
        public class DocumentTemplateCursor 
            : minidom.Databases.DBObjectCursorPO<DocumentTemplate>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorStringField m_SourceFile = new DBCursorStringField("SourceFile");
            private DBCursorStringField m_ContextType = new DBCursorStringField("ContextType");
            private DBCursorStringField m_Description = new DBCursorStringField("Description");
            private DBCursorStringField m_PageFormatName = new DBCursorStringField("PageFormatName");
            private DBCursorField<float> m_PageFormatWidth = new DBCursorField<float>("PageFormatWidth");
            private DBCursorField<float> m_PageFormatHeight = new DBCursorField<float>("PageFormatHeight");

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentTemplateCursor()
            {
            }

            /// <summary>
            /// ContextType
            /// </summary>
            public DBCursorStringField ContextType
            {
                get
                {
                    return m_ContextType;
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
            /// SourceFile
            /// </summary>
            public DBCursorStringField SourceFile
            {
                get
                {
                    return m_SourceFile;
                }
            }

            /// <summary>
            /// Description
            /// </summary>
            public DBCursorStringField Description
            {
                get
                {
                    return m_Description;
                }
            }

            /// <summary>
            /// PageFormatName
            /// </summary>
            public DBCursorStringField PageFormatName
            {
                get
                {
                    return m_PageFormatName;
                }
            }

            /// <summary>
            /// PageFormatWidth
            /// </summary>
            public DBCursorField<float> PageFormatWidth
            {
                get
                {
                    return m_PageFormatWidth;
                }
            }

            /// <summary>
            /// PageFormatHeight
            /// </summary>
            public DBCursorField<float> PageFormatHeight
            {
                get
                {
                    return m_PageFormatHeight;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Templates;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_DocumentiTemplates";
            //}
        }
    }
}