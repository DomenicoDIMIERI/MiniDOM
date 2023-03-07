using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    [Serializable]
    public class WebControlBorder 
        : IDMDXMLSerializable
    {
        [NonSerialized]
        private WebControlStyle m_Owner;
        public string Color;
        public string Size;
        public string Style;

        public WebControlBorder()
        {
            DMDObject.IncreaseCounter(this);
            Color = "";
            Size = "";
            Style = "";
        }

        public WebControlBorder(string text) : this()
        {
            Parse(text);
        }

        public WebControlBorder(WebControlStyle owner) : this()
        {
            m_Owner = owner;
        }

        private void Parse(string value)
        {
            var items = Strings.Split(Strings.Replace(Strings.Trim(value), "  ", " "), " ");
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                if (Strings.Left(items[i], 1) == "#")
                {
                    Color = items[i];
                }
                else if ( 
                    DMD.Strings.Compare(Strings.Left(items[i], 1), "0", false) >= 0 
                    && 
                    DMD.Strings.Compare(Strings.Left(items[i], 1), "9", false) <= 0
                    )
                {
                    Size = items[i];
                }
                else
                {
                    Style = items[i];
                }
            }
        }

        public override string ToString()
        {
            string ret = DMD.Strings.vbNullString;
            if (!string.IsNullOrEmpty(Style))
                ret = DMD.Strings.Combine(ret, Style, " ");
            if (!string.IsNullOrEmpty(Size))
                ret = DMD.Strings.Combine(ret, Size, " ");
            if (!string.IsNullOrEmpty(Color))
                ret = DMD.Strings.Combine(ret, Color, " ");
            return ret;
        }

        public static implicit operator WebControlBorder(string value)
        {
            return new WebControlBorder(value);
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Color":
                    {
                        Color = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Size":
                    {
                        Size = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Style":
                    {
                        Style = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Color", Color);
            writer.WriteAttribute("Size", Size);
            writer.WriteAttribute("Style", Style);
        }

        ~WebControlBorder()
        {
            DMDObject.DecreaseCounter(this);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

    }
}