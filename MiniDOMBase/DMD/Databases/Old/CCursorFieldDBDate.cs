using System;
using System.Xml.Serialization;


namespace minidom
{
    public partial class Databases
    {
        [Serializable]
        public class CCursorFieldDBDate : CCursorField
        {
            public CCursorFieldDBDate()
            {
            }

            public CCursorFieldDBDate(string fieldName, OP op = OP.OP_EQ, bool nullable = false) 
                : base(fieldName, adDataTypeEnum.adWChar, op, nullable)
            {
            }

            [XmlIgnore]
            public new DateTime? Value
            {
                get
                {
                    if (base.Value is DBNull)
                    {
                        return default;
                    }
                    else
                    {
                        return DBUtils.FromDBDateStr(DMD.Strings.CStr(base.Value));
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
                        base.Value = DBUtils.ToDBDateStr(value);
                    }
                }
            }

            [XmlIgnore]
            public new DateTime? Value1
            {
                get
                {
                    if (base.Value1 is DBNull)
                    {
                        return default;
                    }
                    else
                    {
                        return DBUtils.FromDBDateStr(DMD.Strings.CStr(base.Value1));
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
                        base.Value1 = DBUtils.ToDBDateStr(value);
                    }
                }
            }

            public new void Between(DateTime? value1, DateTime? value2)
            {
                Between(DBUtils.ToDBDateStr(value1), DBUtils.ToDBDateStr(value2));
            }

            public override string GetSQL()
            {
                return base.GetSQL();
            }

            public override string GetSQL(string nomeCampoOverride)
            {
                return base.GetSQL(nomeCampoOverride);
            }
        }
    }
}