using System;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Rappresenta una sorgente di importazione/esportazione
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CImportExportSourceCursor : Databases.DBObjectCursor<CImportExportSource>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorStringField m_DisplayName = new DBCursorStringField("DisplayName");
            private DBCursorField<ImportExportSourceFlags> m_Flags = new DBCursorField<ImportExportSourceFlags>("Flags");
            private DBCursorStringField m_Password = new DBCursorStringField("Password");
            private DBCursorStringField m_RemoteURL = new DBCursorStringField("RemoteURL");

            public CImportExportSourceCursor()
            {
            }

            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            public DBCursorStringField DisplayName
            {
                get
                {
                    return m_DisplayName;
                }
            }

            public DBCursorField<ImportExportSourceFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorStringField Password
            {
                get
                {
                    return m_Password;
                }
            }

            public DBCursorStringField RemoteURL
            {
                get
                {
                    return m_RemoteURL;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return ImportExportSources.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDImportExportS";
            }
        }
    }
}