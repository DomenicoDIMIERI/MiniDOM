using DMD;
using System;

namespace minidom.Exceptions
{

    /// <summary>
    /// Formato fonte non valido
    /// </summary>
    [Serializable]
    public class FontFormatException 
        : DMDException
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public FontFormatException()
        {
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="message"></param>
        public FontFormatException(string message) : base(message)
        {
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FontFormatException(string message, Exception innerException) : base(message, innerException)
        {
            //DMDObject.IncreaseCounter(this);
        }

        //~FontFormatException()
        //{
        //    //DMDObject.DecreaseCounter(this);
        //}
    }
}