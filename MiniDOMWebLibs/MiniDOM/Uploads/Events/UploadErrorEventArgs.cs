using System;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public class UploadErrorEventArgs : UploadEventArgs
        {
            private Exception m_Cause;

            public UploadErrorEventArgs()
            {
            }

            public UploadErrorEventArgs(CFileUploader u, Exception e) : base(u)
            {
                m_Cause = e;
            }

            public Exception Cause
            {
                get
                {
                    return m_Cause;
                }
            }
        }
    }
}