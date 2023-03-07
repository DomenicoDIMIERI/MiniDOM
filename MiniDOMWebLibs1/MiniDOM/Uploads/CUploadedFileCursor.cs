using System;

namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
    /// Rappresenta le informazioni registrate nel DB relativamente ad un upload
    /// </summary>
    /// <remarks></remarks>
        public class CUploadedFileCursor : Databases.DBObjectCursorBase<CUploadedFile>
        {
            private Databases.CCursorFieldObj<string> m_Key = new Databases.CCursorFieldObj<string>("Key");
            private Databases.CCursorField<int> m_UserID = new Databases.CCursorField<int>("UserID");
            private Databases.CCursorFieldObj<string> m_SourceFile = new Databases.CCursorFieldObj<string>("SourceFile");
            private Databases.CCursorFieldObj<string> m_TargetURL = new Databases.CCursorFieldObj<string>("TargetURL");
            private Databases.CCursorField<DateTime> m_UploadTime = new Databases.CCursorField<DateTime>("UploadTime");
            private Databases.CCursorField<int> m_FileSize = new Databases.CCursorField<int>("FileSize");

            public CUploadedFileCursor()
            {
            }

            protected override Sistema.CModule GetModule()
            {
                return Uploads.Module;
            }

            public Databases.CCursorFieldObj<string> Key
            {
                get
                {
                    return m_Key;
                }
            }

            public Databases.CCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
                }
            }

            public Databases.CCursorFieldObj<string> SourceFile
            {
                get
                {
                    return m_SourceFile;
                }
            }

            public Databases.CCursorFieldObj<string> TargetURL
            {
                get
                {
                    return m_TargetURL;
                }
            }

            public Databases.CCursorField<DateTime> UploadTime
            {
                get
                {
                    return m_UploadTime;
                }
            }

            public Databases.CCursorField<int> FileSize
            {
                get
                {
                    return m_FileSize;
                }
            }

            public override string GetTableName()
            {
                return "tbl_Uploads";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            public override object InstantiateNew(Databases.DBReader dbRis)
            {
                return new CUploadedFile();
            }
        }
    }
}