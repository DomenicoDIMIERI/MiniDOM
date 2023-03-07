using System;
using System.Drawing;
using System.Drawing.Imaging;
using DMD.XML;

namespace minidom.PBX
{
    [Serializable]
    public class ScreenShot : IDisposable
    {
        public const long jpgQuality = 50L;
        public static ImageCodecInfo jpgEncoder;
        public static EncoderParameters myEncoderParameters;

        static ScreenShot()
        {
            jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            var myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, jpgQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            // bmp1.Save(@"c:\TestPhotoQualityFifty.jpg", jpgEncoder, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public DateTime Time;
        public string Name;
        public bool IsFullScreen;
        public Rectangle Bounds;
        public Image Content;
        [NonSerialized]
        private System.IO.MemoryStream ms;

        public ScreenShot()
        {
            DMDObject.IncreaseCounter(this);
            Time = default;
            Name = "";
            IsFullScreen = false;
            Bounds = default;
            Content = null;
        }

        public ScreenShot(string name, bool isFullScreen, int x, int y, int width, int height, Bitmap content) : this()
        {
            Time = DMD.DateUtils.Now();
            Name = name;
            IsFullScreen = isFullScreen;
            Bounds = new Rectangle(x, y, width, height);
            int alphaTransparency = 1;
            int alphaFader = 1;
            string targetPath = System.IO.Path.GetTempFileName();
            Content = new Bitmap(targetPath); // Me.jpegCompress(content)
        }

        private Bitmap jpegCompress(Bitmap img)
        {
            ms = new System.IO.MemoryStream();
            // Dim fName As String = System.IO.Path.GetTempFileName
            img.Save(ms, jpgEncoder, myEncoderParameters);
            img.Dispose();
            ms.Position = 0L;
            img = new Bitmap(ms);
            return img;
        }


        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            if (Content is object)
            {
                Content.Dispose();
                Content = null;
            }

            if (ms is object)
            {
                ms.Dispose();
                ms = null;
            }

            Name = DMD.Strings.vbNullString;
        }

        ~ScreenShot()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}