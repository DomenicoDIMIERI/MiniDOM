
namespace minidom.XML
{
    public interface IDMDXMLSerializable
    {
        void XMLSerialize(XMLWriter writer); // As String
        void SetFieldInternal(string fieldName, object fieldValue);
    }
}