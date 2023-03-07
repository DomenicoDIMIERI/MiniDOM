using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Databases
    {
        [Serializable]
        public class CDBEntityField : CDBObject
        {
            [NonSerialized]
            private CDBEntity m_Owner;
            private bool m_IsAutoIncrement;
            private Type m_DataType;
            private int m_Length;
            private int m_AutoIncrementSeed;
            private int m_AutoIncrementStep;
            private bool m_AllowDBNull;
            private string m_Caption;
            private int m_Ordinal;
            private string m_Namespace;
            private string m_Expression;
            private object m_DefaultValue;
            private string m_ColumnMapping;
            private string m_Prefix;
            private bool m_ReadOnly;
            private bool m_Unique;

            public CDBEntityField()
            {
                m_DefaultValue = DBNull.Value;
                m_Owner = null;
                m_IsAutoIncrement = false;
                m_DataType = null;
                m_Length = 0;
                m_AutoIncrementSeed = 0;
                m_AutoIncrementStep = 1;
                m_AllowDBNull = true;
                m_Caption = DMD.Strings.vbNullString;
                m_Ordinal = 0;
                m_Namespace = DMD.Strings.vbNullString;
                m_Expression = DMD.Strings.vbNullString;
                m_ColumnMapping = DMD.Strings.vbNullString;
                m_Prefix = DMD.Strings.vbNullString;
                m_ReadOnly = false;
                m_Unique = false;
            }

            public CDBEntityField(string name, Type dataType) : this()
            {
                SetName(name);
                m_DataType = dataType;
            }

            public CDBEntityField(string name, TypeCode dataType) : this(name, Sistema.Types.GetTypeFromCode(dataType))
            {
            }

            public CDBEntity Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            protected internal void SetOwner(CDBEntity value)
            {
                m_Owner = value;
                if (value is object)
                    SetConnection(value.Connection);
            }

            public int MaxLength
            {
                get
                {
                    return m_Length;
                }

                set
                {
                    m_Length = value;
                }
            }

            public Type DataType
            {
                get
                {
                    return m_DataType;
                }

                set
                {
                    m_DataType = value;
                }
            }

            public bool AutoIncrement
            {
                get
                {
                    return m_IsAutoIncrement;
                }

                set
                {
                    m_IsAutoIncrement = value;
                }
            }

            public int AutoIncrementSeed
            {
                get
                {
                    return m_AutoIncrementSeed;
                }

                set
                {
                    m_AutoIncrementSeed = value;
                }
            }

            public int AutoIncrementStep
            {
                get
                {
                    return m_AutoIncrementStep;
                }

                set
                {
                    m_AutoIncrementStep = value;
                }
            }

            public bool AllowDBNull
            {
                get
                {
                    return m_AllowDBNull;
                }

                set
                {
                    m_AllowDBNull = value;
                }
            }

            public int Ordinal
            {
                get
                {
                    return m_Ordinal;
                }
            }

            public void SetOrdinal(int value)
            {
                m_Ordinal = value;
            }

            public string Prefix
            {
                get
                {
                    return m_Prefix;
                }

                set
                {
                    m_Prefix = DMD.Strings.Trim(value);
                }
            }

            public bool ReadOnly
            {
                get
                {
                    return m_ReadOnly;
                }

                set
                {
                    m_ReadOnly = value;
                }
            }

            public bool Unique
            {
                get
                {
                    return m_Unique;
                }

                set
                {
                    m_Unique = value;
                }
            }

            public string Namespace
            {
                get
                {
                    return m_Namespace;
                }

                set
                {
                    m_Namespace = DMD.Strings.Trim(value);
                }
            }

            public string Expression
            {
                get
                {
                    return m_Expression;
                }

                set
                {
                    m_Expression = DMD.Strings.Trim(value);
                }
            }

            public object DefaultValue
            {
                get
                {
                    return m_DefaultValue;
                }

                set
                {
                    m_DefaultValue = value;
                }
            }

            public string ColumnMapping
            {
                get
                {
                    return m_ColumnMapping;
                }

                set
                {
                    m_ColumnMapping = DMD.Strings.Trim(value);
                }
            }

            public string Caption
            {
                get
                {
                    return m_Caption;
                }

                set
                {
                    m_Caption = value;
                }
            }

            protected override void CreateInternal1()
            {
                Connection.CreateField(this);
            }

            protected override void DropInternal1()
            {
                Connection.DropField(this);
            }

            protected override void UpdateInternal1()
            {
                Connection.UpdateField(this);
            }

            protected override void RenameItnernal(string newName)
            {
                throw new NotImplementedException();
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IsAutoIncrement", m_IsAutoIncrement);
                writer.WriteAttribute("DataType", m_DataType.FullName);
                writer.WriteAttribute("Length", m_Length);
                writer.WriteAttribute("AutoIncrementSeed", m_AutoIncrementSeed);
                writer.WriteAttribute("AutoIncrementStep", m_AutoIncrementStep);
                writer.WriteAttribute("AllowDBNull", m_AllowDBNull);
                writer.WriteAttribute("Caption", m_Caption);
                writer.WriteAttribute("Ordinal", m_Ordinal);
                writer.WriteAttribute("Namespace", m_Namespace);
                writer.WriteAttribute("Expression", m_Expression);
                writer.WriteAttribute("DefaultValue", Sistema.Formats.ToString(m_DefaultValue));
                writer.WriteAttribute("ColumnMapping", m_ColumnMapping);
                writer.WriteAttribute("Prefix", m_Prefix);
                writer.WriteAttribute("ReadOnly", m_ReadOnly);
                writer.WriteAttribute("Unique", m_Unique);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IsAutoIncrement":
                        {
                            m_IsAutoIncrement = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataType":
                        {
                            m_DataType = Sistema.Types.GetType(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "Length":
                        {
                            m_Length = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AutoIncrementSeed":
                        {
                            m_AutoIncrementSeed = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AutoIncrementStep":
                        {
                            m_AutoIncrementStep = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AllowDBNull":
                        {
                            m_AllowDBNull = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Caption":
                        {
                            m_Caption = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Ordinal":
                        {
                            m_Ordinal = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Namespace":
                        {
                            m_Namespace = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Expression":
                        {
                            m_Expression = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DefaultValue":
                        {
                            m_DefaultValue = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ColumnMapping":
                        {
                            m_ColumnMapping = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Prefix":
                        {
                            m_Prefix = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ReadOnly":
                        {
                            m_ReadOnly = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Unique":
                        {
                            m_Unique = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}