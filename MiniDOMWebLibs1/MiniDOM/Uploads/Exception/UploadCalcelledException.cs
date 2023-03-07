using System;

namespace minidom
{
    public partial class WebSite
    {
        public class UploadCalcelledException : Exception
        {
            public UploadCalcelledException()
            {
                DMDObject.IncreaseCounter(this);
            }

            public UploadCalcelledException(string message) : base(message)
            {
                DMDObject.IncreaseCounter(this);
            }

            ~UploadCalcelledException()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}