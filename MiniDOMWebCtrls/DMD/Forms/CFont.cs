using System.Drawing;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CFont 
        : IDMDXMLSerializable
    {
        private string m_Name;
        private string m_Size;
        private int m_Flags;

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }


        public CFont() : this("", "12pt", 0)
        {
        }

        public CFont(string name) : this(name, "12pt", 0)
        {
        }

        public CFont(string name, string size) : this(name, size, 0)
        {
        }

        public CFont(string name, string size, int flags)
        {
            DMDObject.IncreaseCounter(this);
            m_Name = DMD.Strings.Trim(name);
            m_Size = DMD.Strings.Trim(size);
            m_Flags = flags;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public string Size
        {
            get
            {
                return m_Size;
            }
        }

        public bool isBold()
        {
            return (m_Flags & 1) == 1;
        }

        public bool isItalic()
        {
            return (m_Flags & 2) == 2;
        }

        public bool isUnderline()
        {
            return (m_Flags & 4) == 4;
        }

        public CFont resize(string newSize)
        {
            return new CFont(m_Name, newSize, m_Flags);
        }

        public override string ToString()
        {
            return DMD.Strings.JoinW(m_Name , " " , m_Size , m_Flags);
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Name":
                    {
                        m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Size":
                    {
                        m_Size = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Flags":
                    {
                        m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Name", m_Name);
            writer.WriteAttribute("Size", m_Size);
            writer.WriteAttribute("Flags", m_Flags);
        }

        ~CFont()
        {
            DMDObject.DecreaseCounter(this);
        }

        public Font GetSystemFont()
        {
            var style = default(FontStyle);
            if (isBold())
                style = style | FontStyle.Bold;
            if (isItalic())
                style = style | FontStyle.Italic;
            if (isUnderline())
                style = style | FontStyle.Underline;
            var unit = GraphicsUnit.Point;
            string sizestr = Strings.LCase(Size);
            var size = default(float);
            if (sizestr.EndsWith(" pt"))
            {
                size = (float)Sistema.Formats.ToDouble(sizestr.Substring(0, sizestr.Length - 2));
                unit = GraphicsUnit.Point;
            }
            else if (sizestr.EndsWith(" mm"))
            {
                size = (float)Sistema.Formats.ToDouble(sizestr.Substring(0, sizestr.Length - 2));
                unit = GraphicsUnit.Millimeter;
            }
            else if (sizestr.EndsWith(" px"))
            {
                size = (float)Sistema.Formats.ToDouble(sizestr.Substring(0, sizestr.Length - 2));
                unit = GraphicsUnit.Pixel;
            }

            return new Font(Name, size, style, unit);
        }
    }
}