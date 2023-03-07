using System.Globalization;
using DMD;
using minidom.Internals;

namespace minidom
{
    namespace Internals
    {
        public sealed class CUploaderClass
        {
            public event UploadBeginEventHandler UploadBegin;

            public delegate void UploadBeginEventHandler(WebSite.UploadEventArgs e);

            public event UploadErrorEventHandler UploadError;

            public delegate void UploadErrorEventHandler(WebSite.UploadErrorEventArgs e);

            public event UploadCompletedEventHandler UploadCompleted;

            public delegate void UploadCompletedEventHandler(WebSite.UploadEventArgs e);

            public event UploadProgressEventHandler UploadProgress;

            public delegate void UploadProgressEventHandler(WebSite.UploadEventArgs e);

            public object @lock { get; private set; } = new object();

            private CCollection<WebSite.CFileUploader> m_Uploads = new CCollection<WebSite.CFileUploader>();

            internal CUploaderClass()
            {
                DMDObject.IncreaseCounter(this);
            }

            public CCollection<WebSite.CFileUploader> UploadedFiles
            {
                get
                {
                    lock (@lock)
                        return new CCollection<WebSite.CFileUploader>(m_Uploads);
                }
            }

            public bool IsUploading(string key)
            {
                var u = GetUpload(key);
                if (u is null)
                    return false;
                return !u.IsCompleted();
            }

            internal void FireBeginUpload(WebSite.UploadEventArgs e)
            {
                UploadBegin?.Invoke(e);
            }

            internal void FireEndUpload(WebSite.UploadEventArgs e)
            {
                UploadCompleted?.Invoke(e);
            }

            internal void FireUploadError(WebSite.UploadEventArgs e)
            {
                UploadError?.Invoke((WebSite.UploadErrorEventArgs)e);
            }

            internal void FireUploadProgress(WebSite.UploadEventArgs e)
            {
                UploadProgress?.Invoke(e);
            }

            public WebSite.CFileUploader CreateUploader<T>() where T : WebSite.CFileUploader
            {
                lock (@lock)
                {
                    int limit = WebSite.Instance.Configuration.NumberOfUploadsLimit;
                    if (limit > 0 && TotalNumberOfUploads > limit)
                    {
                        throw new WebSite.UploadCalcelledException("Superato il numero massimo di upload consentiti");
                    }

                    WebSite.CFileUploader u = (WebSite.CFileUploader)Sistema.Types.CreateInstance(typeof(T));
                    string key = Sistema.ASPSecurity.GetRandomKey(25);
                    while (GetUpload(key) is object)
                        key = Sistema.ASPSecurity.GetRandomKey(25);
                    u.SetKey(key);
                    m_Uploads.Add(u);
                    return u;
                }
            }

            public void RemoveUploader(WebSite.CFileUploader u)
            {
                if (u is object)
                    m_Uploads.Remove(u);
            }

            /// <summary>
        /// Restituisce il numero totale di uploads in corso nell'ambito dell'applicazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int TotalNumberOfUploads
            {
                get
                {
                    lock (@lock)
                    {
                        int timeOut = WebSite.Instance.Configuration.UploadTimeOut;
                        int i, cnt;
                        i = 0;
                        cnt = 0;
                        while (i < m_Uploads.Count)
                        {
                            var u = m_Uploads[i];
                            if (u.IsCompleted())
                            {
                            }
                            // Me.m_Uploads.RemoveByKey(u.Key)
                            else if (timeOut > 0 && DMD.DateUtils.DateDiff(DateTimeInterval.Second, u.StartTime, DMD.DateUtils.Now()) > timeOut + 10)
                            {
                                u.Abort();
                            }
                            else
                            {
                                cnt += 1;
                            }

                            i += 1;
                        }

                        return cnt;
                    }
                }
            }

            /// <summary>
        /// Restituisce l'oggetto Upload con l'ID specifico
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public WebSite.CFileUploader GetUpload(string key)
            {
                lock (@lock)
                {
                    foreach (WebSite.CFileUploader u in m_Uploads)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(u.Key ?? "", key ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            return u;
                    }

                    return null;
                }
            }

            ~CUploaderClass()
            {
                DMDObject.DecreaseCounter(this);
            }

            public void SetUpload(string uID, WebSite.CFileUploader f)
            {
                lock (@lock)
                {
                    f.SetKey(uID);
                    m_Uploads.Add(f);
                }
            }
        }
    }

    public partial class WebSite
    {
        private static CUploaderClass m_Uploader = null;

        public static CUploaderClass Uploader
        {
            get
            {
                if (m_Uploader is null)
                    m_Uploader = new CUploaderClass();
                return m_Uploader;
            }
        }
    }
}