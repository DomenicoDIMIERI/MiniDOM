using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    [Serializable]
    public class WebControlStyle : IDMDXMLSerializable
    {
        [NonSerialized]
        private WebControl m_Owner;
        public string Position;
        private string m_Left;
        private string m_Top;
        private string m_Bottom;
        private string m_Right;
        private string m_Width;
        private string m_Height;
        public WebControlBackGround Background;
        public WebControlBorder Border;
        public string Color;
        public string TextAlign;
        public string zIndex;
        public string Visibility;
        public string Overflow;
        public string WhiteSpace;
        public string MinWidth;
        public string MaxWidth;
        public string MinHeight;
        public string MaxHeight;
        public string Display;
        public string VerticalAlign = "";

        public WebControlStyle()
        {
            DMDObject.IncreaseCounter(this);
            Background = new WebControlBackGround(this);
            Border = new WebControlBorder(this);
        }

        public WebControlStyle(WebControl owner) : this()
        {
            m_Owner = owner;
        }

        public string Left
        {
            get
            {
                return m_Left;
            }

            set
            {
                m_Left = NormalizeSize(value);
            }
        }

        public string Top
        {
            get
            {
                return m_Top;
            }

            set
            {
                m_Top = NormalizeSize(value);
            }
        }

        public string Right
        {
            get
            {
                return m_Right;
            }

            set
            {
                m_Right = NormalizeSize(value);
            }
        }

        public string Bottom
        {
            get
            {
                return m_Bottom;
            }

            set
            {
                m_Bottom = NormalizeSize(value);
            }
        }

        public string Width
        {
            get
            {
                return m_Width;
            }

            set
            {
                m_Width = NormalizeSize(value);
            }
        }

        public string Height
        {
            get
            {
                return m_Height;
            }

            set
            {
                m_Height = NormalizeSize(value);
            }
        }

        private string NormalizeSize(string value)
        {
            value = Strings.Trim(value);
            if (string.IsNullOrEmpty(value))
                return "";
            string ch = DMD.Strings.Right(value, 1);
            if (DMD.Strings.Compare(ch, "0", false) >= 0 && DMD.Strings.Compare(ch, "9", false) <= 0)
                value = value + "px";
            return value;
        }

        public override string ToString()
        {
            string ret = DMD.Strings.vbNullString;
            string tmp;
            if (!string.IsNullOrEmpty(TextAlign))
                ret = DMD.Strings.Combine(ret, "text-align:" + TextAlign, ";");
            if (!string.IsNullOrEmpty(Position))
                ret = DMD.Strings.Combine(ret, "position:" + Position, ";");
            if (!string.IsNullOrEmpty(Left))
                ret = DMD.Strings.Combine(ret, "left:" + Left, ";");
            if (!string.IsNullOrEmpty(Top))
                ret = DMD.Strings.Combine(ret, "top:" + Top, ";");
            if (!string.IsNullOrEmpty(Right))
                ret = DMD.Strings.Combine(ret, "right:" + Right, ";");
            if (!string.IsNullOrEmpty(Bottom))
                ret = DMD.Strings.Combine(ret, "bottom:" + Bottom, ";");
            if (!string.IsNullOrEmpty(Width))
                ret = DMD.Strings.Combine(ret, "width:" + Width, ";");
            if (!string.IsNullOrEmpty(Height))
                ret = DMD.Strings.Combine(ret, "height:" + Height, ";");
            if (!string.IsNullOrEmpty(Overflow))
                ret = DMD.Strings.Combine(ret, "overflow:" + Overflow, ";");
            if (!string.IsNullOrEmpty(WhiteSpace))
                ret = DMD.Strings.Combine(ret, "white-space:" + WhiteSpace, ";");
            if (!string.IsNullOrEmpty(MinWidth))
                ret = DMD.Strings.Combine(ret, "min-width:" + MinWidth, ";");
            if (!string.IsNullOrEmpty(MaxWidth))
                ret = DMD.Strings.Combine(ret, "max-width:" + MaxWidth, ";");
            if (!string.IsNullOrEmpty(MinHeight))
                ret = DMD.Strings.Combine(ret, "min-height:" + MinHeight, ";");
            if (!string.IsNullOrEmpty(MaxHeight))
                ret = DMD.Strings.Combine(ret, "max-height:" + MaxHeight, ";");
            if (!string.IsNullOrEmpty(Display))
                ret = DMD.Strings.Combine(ret, "display:" + Display, ";");
            if (!string.IsNullOrEmpty(VerticalAlign))
                ret = DMD.Strings.Combine(ret, "vertical-align:" + VerticalAlign, ";");
            tmp = Border.ToString();
            if (!string.IsNullOrEmpty(tmp))
                ret = DMD.Strings.Combine(ret, "border:" + tmp, ";");
            tmp = Background.ToString();
            if (!string.IsNullOrEmpty(tmp))
                ret = DMD.Strings.Combine(ret, "background:" + tmp, ";");
            if (!string.IsNullOrEmpty(Color))
                ret = DMD.Strings.Combine(ret, "color:" + Color, ";");
            if (!string.IsNullOrEmpty(zIndex))
                ret = DMD.Strings.Combine(ret, "z-index:" + zIndex, ";");
            if (!string.IsNullOrEmpty(Visibility))
                ret = DMD.Strings.Combine(ret, "visibility:" + Visibility, ";");
            return ret;
        }

        public void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Position":
                    {
                        Position = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Left":
                    {
                        Left = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Top":
                    {
                        Top = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Bottom":
                    {
                        Bottom = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Right":
                    {
                        Right = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Width":
                    {
                        Width = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Height":
                    {
                        Height = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Color":
                    {
                        Color = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "TextAlign":
                    {
                        TextAlign = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "zIndex":
                    {
                        zIndex = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Visibility":
                    {
                        Visibility = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Overflow":
                    {
                        Overflow = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "WhiteSpace":
                    {
                        WhiteSpace = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "MinWidth":
                    {
                        MinWidth = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "MaxWidth":
                    {
                        MaxWidth = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "MinHeight":
                    {
                        MinHeight = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "MaxHeight":
                    {
                        MaxHeight = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Display":
                    {
                        Display = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "VerticalAlign":
                    {
                        VerticalAlign = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Background":
                    {
                        Background = (WebControlBackGround)fieldValue;
                        break;
                    }

                case "Border":
                    {
                        Border = (WebControlBorder)fieldValue;
                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Position", Position);
            writer.WriteAttribute("Left", Left);
            writer.WriteAttribute("Top", Top);
            writer.WriteAttribute("Bottom", Bottom);
            writer.WriteAttribute("Right", Right);
            writer.WriteAttribute("Width", Width);
            writer.WriteAttribute("Height", Height);
            writer.WriteAttribute("Color", Color);
            writer.WriteAttribute("TextAlign", TextAlign);
            writer.WriteAttribute("zIndex", zIndex);
            writer.WriteAttribute("Visibility", Visibility);
            writer.WriteAttribute("Overflow", Overflow);
            writer.WriteAttribute("WhiteSpace", WhiteSpace);
            writer.WriteAttribute("MinWidth", MinWidth);
            writer.WriteAttribute("MaxWidth", MaxWidth);
            writer.WriteAttribute("MinHeight", MinHeight);
            writer.WriteAttribute("MaxHeight", MaxHeight);
            writer.WriteAttribute("Display", Display);
            writer.WriteAttribute("VerticalAlign", VerticalAlign);
            writer.WriteTag("Background", Background);
            writer.WriteTag("Border", Border);
        }

        protected internal virtual void SetOwner(WebControl owner)
        {
            m_Owner = owner;
        }

        ~WebControlStyle()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}