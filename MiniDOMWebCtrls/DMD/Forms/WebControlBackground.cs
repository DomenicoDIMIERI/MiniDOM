using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    [Serializable]
    public class WebControlBackGround
        : IDMDXMLSerializable
    {
        [NonSerialized]
        private WebControlStyle m_Owner;
        private string m_Color;
        public string Image;
        public string Repeat;
        public string Attachment;
        public string Position;

        public WebControlBackGround()
        {
            DMDObject.IncreaseCounter(this);
        }

        public WebControlBackGround(WebControlStyle owner) : this()
        {
            m_Owner = owner;
        }

        public override string ToString()
        {
            string ret = DMD.Strings.vbNullString;
            if (!string.IsNullOrEmpty(m_Color))
                ret = DMD.Strings.Combine(ret, m_Color, " ");
            if (!string.IsNullOrEmpty(Image))
                ret = DMD.Strings.Combine(ret, Image, " ");
            if (!string.IsNullOrEmpty(Repeat))
                ret = DMD.Strings.Combine(ret, Repeat, " ");
            if (!string.IsNullOrEmpty(Attachment))
                ret = DMD.Strings.Combine(ret, Attachment, " ");
            if (!string.IsNullOrEmpty(Position))
                ret = DMD.Strings.Combine(ret, Position, " ");
            // #00ff00 url('smiley.gif') no-repeat fixed center; 
            return ret;
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }


        public string Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        // Public Shared Widening Operator CType(ByVal value As String) As WebControlBackground
        // Dim items() As String = Split(Replace(Trim(value), "  ", " "), " ")
        // Dim ret As New WebControlBackground
        // For i = 0 To UBound(items)
        // If Left(items(i), 1) = "#" Then
        // ret.Color = items(i)
        // ElseIf Left(items(i), 1) >= "0" And Left(items(i), 1) <= "9" Then
        // ret.Size = items(i)
        // Else
        // ret.Style = items(i)
        // End If
        // Next
        // Return ret
        // End Operator


        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Color":
                    {
                        Color = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Image":
                    {
                        Image = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Repeat":
                    {
                        Repeat = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Attachment":
                    {
                        Attachment = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Position":
                    {
                        Position = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Color", Color);
            writer.WriteAttribute("Image", Image);
            writer.WriteAttribute("Repeat", Repeat);
            writer.WriteAttribute("Attachment", Attachment);
            writer.WriteAttribute("Position", Position);
        }

        ~WebControlBackGround()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}