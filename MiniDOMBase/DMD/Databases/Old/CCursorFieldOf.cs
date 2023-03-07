using System;
using System.Xml.Serialization;


namespace minidom
{
    public partial class Databases
    {
        [Serializable]
        public class CCursorField<T> : CCursorField where T : struct
        {
            public CCursorField()
            {
            }

            public CCursorField(string fieldName, OP op = OP.OP_EQ, bool nullable = false) : base(fieldName, DBUtils.GetADOType(typeof(T)), op, nullable)
            {
            }

            [XmlIgnore]
            public new T? Value
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
                    if (value.HasValue)
                    {
                        base.Value = value.Value;
                    }
                    else
                    {
                        base.Value = DBNull.Value;
                    }
                }
            }

            [XmlIgnore]
            public new T? Value1
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
                    if (value.HasValue)
                    {
                        base.Value1 = value.Value;
                    }
                    else
                    {
                        base.Value1 = DBNull.Value;
                    }
                }
            }

            public void ValueIn(T[] values)
            {
                var tmp = new object[(values is null)? 0 : values.Length];
                for (int i = 0; i < tmp.Length; i++)
                    tmp[i] = values[i];
                this.ValueIn(tmp);
            }

            public void ValueIn(T?[] values)
            {
                var tmp = new object[ (values is null)? 0 : values.Length];
                for (int i = 0; i < values.Length; i++)
                    tmp[i] = (values[i] is null)? DBNull.Value : values[i];
                base.ValueIn(tmp);
            }
        }
    }
}