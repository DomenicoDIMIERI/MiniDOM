using System;

namespace minidom.XML
{
    [Serializable]
    public class XMLCapsule : IDMDXMLSerializable
    {
        private object m_Value;
        private string m_ValueType;
        private string m_ValueXML;
        private bool m_buildVal;
        private bool m_buoldXml;

        public XMLCapsule()
        {
            DMDObject.IncreaseCounter(this);
        }

        public object Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
                m_ValueType = Sistema.vbTypeName(value);
            }
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Value":
                    {
                        switch (m_ValueType ?? "")
                        {
                        }

                        break;
                    }

                case "ValueType":
                    {
                        m_ValueType = Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            switch (m_ValueType ?? "")
            {
                case "Byte":
                case "SByte":
                    {
                        writer.WriteAttribute("Value", MakeValue<byte>(m_Value));
                        break;
                    }

                case "Short":
                case "UShort":
                case "Int16":
                case "UInt16":
                    {
                        writer.WriteAttribute("Value", MakeValue<short>(m_Value));
                        break;
                    }

                case "Integer":
                case "UInteger":
                case "Int32":
                case "UInt32":
                    {
                        writer.WriteAttribute("Value", MakeValue<short>(m_Value));
                        break;
                    }

                case "Long":
                case "ULong":
                case var @case when @case == "Int16":
                case "UInt64":
                    {
                        writer.WriteAttribute("Value", MakeValue<short>(m_Value));
                        break;
                    }

                case "Single":
                    {
                        writer.WriteAttribute("Value", MakeValue<float>(m_Value));
                        break;
                    }

                case "Double":
                    {
                        writer.WriteAttribute("Value", MakeValue<double>(m_Value));
                        break;
                    }

                case "Decimal":
                    {
                        writer.WriteAttribute("Value", MakeValue<decimal>(m_Value));
                        break;
                    }

                case "Date":
                case "DateTime":
                    {
                        writer.WriteAttribute("Value", MakeValue<DateTime>(m_Value));
                        break;
                    }

                case "Boolean":
                    {
                        writer.WriteAttribute("Value", MakeValue<bool>(m_Value));
                        break;
                    }
            }
        }

        private T? MakeValue<T>(object v) where T : struct
        {
            return (T?)v;
        }

        ~XMLCapsule()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}