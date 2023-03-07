using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class UploadEventArgs : EventArgs
        {
            [NonSerialized]
            private CFileUploader m_Uploader;

            public UploadEventArgs()
            {
                DMDObject.IncreaseCounter(this);
            }

            public UploadEventArgs(CFileUploader u) : this()
            {
                m_Uploader = u;
            }

            public CFileUploader Upload
            {
                get
                {
                    return m_Uploader;
                }
            }

            ~UploadEventArgs()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}