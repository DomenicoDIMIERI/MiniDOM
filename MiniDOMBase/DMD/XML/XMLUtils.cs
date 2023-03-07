

namespace minidom.XML
{
    public enum XMLSerializeMethod : int
    {
        Document = 0,
        None = 1
    }

    public sealed class Utils
    {
        private Utils()
        {
            DMDObject.IncreaseCounter(this);
        }

        public static readonly XMLSerializer Serializer = new XMLSerializer();

        public static string XMLTypeName(object obj)
        {
            string elemType = Sistema.Types.vbTypeName(obj);
            int i = DMD.Strings.InStr(elemType, "(Of ");
            if (i > 0)
                elemType = DMD.Strings.Left(elemType, i - 1);
            return elemType;
        }

        ~Utils()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}