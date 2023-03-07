
namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
    /// Rappresenta le informazioni registrate nel DB relativamente ad un upload
    /// </summary>
    /// <remarks></remarks>
        public sealed class CUploadedFilesClass 
            : Sistema.CModulesClass<CUploadedFile>
        {
            internal CUploadedFilesClass() : base("modUploadedFiles", typeof(CUploadedFileCursor), 0)
            {
            }

            public CUploadedFile GetItemByKey(string k)
            {
                k = DMD.Strings.Trim(k);
                if (string.IsNullOrEmpty(k))
                    return null;
                var cursor = new CUploadedFileCursor();
                CUploadedFile ret;
                cursor.IgnoreRights = true;
                cursor.PageSize = 1;
                cursor.Key.Value = k;
                ret = cursor.Item;
                cursor.Dispose();
                return ret;
            }
        }

        private static CUploadedFilesClass m_UploadedFiles = null;

        public static CUploadedFilesClass Uploads
        {
            get
            {
                if (m_UploadedFiles is null)
                    m_UploadedFiles = new CUploadedFilesClass();
                return m_UploadedFiles;
            }
        }
    }
}