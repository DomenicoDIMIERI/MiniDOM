using System;
using System.Xml.Serialization;

namespace minidom
{
    public partial class Databases
    {
        [Serializable]
        public class CCursorFieldObj<T> : CCursorField
        {
            public CCursorFieldObj()
            {
            }

            public CCursorFieldObj(string fieldName, OP op = OP.OP_EQ, bool nullable = false) : base(fieldName, DBUtils.GetADOType(typeof(T)), op, nullable)
            {
            }

            [XmlIgnore]
            public new T Value
            {
                get
                {
                    if (base.Value is DBNull)
                    {
                        return default;
                    }
                    else
                    {
                        return (T)base.Value;
                    }
                }

                set
                {
                    if (value is null)
                    {
                        base.Value = DBNull.Value;
                    }
                    else
                    {
                        base.Value = value;
                    }
                }
            }

            [XmlIgnore]
            public new T Value1
            {
                get
                {
                    if (base.Value1 is DBNull)
                    {
                        return default;
                    }
                    else
                    {
                        return (T)base.Value1;
                    }
                }

                set
                {
                    if (value is null)
                    {
                        base.Value1 = DBNull.Value;
                    }
                    else
                    {
                        base.Value1 = value;
                    }
                }
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                base.SetFieldInternal(fieldName, fieldValue);
            }

            protected override object[] parseFieldValues(object value)
            {
                return base.parseFieldValues(value);
            }
        }
    }
}