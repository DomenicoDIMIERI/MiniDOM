using System;
using DMD;
using DMD.XML;

namespace minidom.PBX
{
    // Implements Xml.Serialization.IXmlSerializable
    public class LineaEsterna 
        : IDMDXMLSerializable, IComparable
    {
        public string Nome;
        public string Prefisso;
        public int Ordine;
        public string Server;

        public LineaEsterna()
        {
            DMDObject.IncreaseCounter(this);
            Nome = "";
            Prefisso = "";
            Ordine = 0;
            Server = "";
        }

        public LineaEsterna(string nome, string prefisso) : this()
        {
            Nome = DMD.Strings.Trim(nome);
            Prefisso = DMD.Strings.Trim(prefisso);
        }

        public override string ToString()
        {
            return Nome + " (" + Prefisso + ")";
        }


        // Private Function GetSchema() As Xml.Schema.XmlSchema Implements Xml.Serialization.IXmlSerializable.GetSchema
        // Return Nothing
        // End Function

        // Private Sub ReadXml(reader As Xml.XmlReader) Implements Xml.Serialization.IXmlSerializable.ReadXml
        // Me.Nome = reader.GetAttribute("nome")
        // Me.Prefisso = reader.GetAttribute("prefisso")
        // reader.Read()
        // End Sub

        // Public Sub WriteXml(writer As Xml.XmlWriter) Implements Xml.Serialization.IXmlSerializable.WriteXml
        // writer.WriteAttributeString("nome", Me.Nome)
        // writer.WriteAttributeString("prefisso", Me.Prefisso)
        // End Sub

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Nome":
                    {
                        Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Prefisso":
                    {
                        Prefisso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Ordine":
                    {
                        Ordine = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Server":
                    {
                        Server = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Nome", Nome);
            writer.WriteAttribute("Prefisso", Prefisso);
            writer.WriteAttribute("Ordine", Ordine);
            writer.WriteAttribute("Server", Server);
        }

        public int CompareTo(LineaEsterna linea)
        {
            int ret = Ordine.CompareTo(linea.Ordine);
            if (ret == 0)
                ret = DMD.Strings.Compare(Nome, linea.Nome, true);
            if (ret == 0)
                ret = DMD.Strings.Compare(Prefisso, linea.Prefisso, true);
            return ret;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((LineaEsterna)obj);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

        ~LineaEsterna()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}