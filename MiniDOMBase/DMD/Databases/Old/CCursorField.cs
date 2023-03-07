using System;
using minidom;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom
{
    public partial class Databases
    {
        public enum OP : int
        {
            OP_NE = -3,        // Not equal
            OP_LT = -2,        // Less than
            OP_LE = -1,        // Less than or equal
            OP_EQ = 0,         // Equal
            OP_GE = 1,         // Greater than or equal
            OP_GT = 2,         // Greater than
            OP_LIKE = 3,       // Like
            OP_NOTLIKE = 5,      // Not Like
            OP_BETWEEN = 4,    // Compreso tra
            OP_IN = 6,
            OP_NOTIN = 7,

            /// <summary>
        /// Confronto binario ( FieldValue BAND Value ) != 0
        /// </summary>
        /// <remarks></remarks>
            OP_ANYBITAND = 8,

            /// <summary>
        /// Confronto binario ( FieldValue BAND Value ) == Value
        /// </summary>
        /// <remarks></remarks>
            OP_ALLBITAND = 9,

            /// <summary>
        /// Confronto binario ( FieldValue BOR Value ) != 0
        /// </summary>
        /// <remarks></remarks>
            OP_ANYBITOR = 10,

            /// <summary>
        /// Confronto binario ( FieldValue BOR Value ) == Value
        /// </summary>
        /// <remarks></remarks>
            OP_ALLBITOR = 11
        }

        public enum SortEnum : int
        {
            SORT_NOTSET = 0,   // La colonna non fa parte dei criteri di ordinamento
            SORT_ASC = 1,      // Colonna ordinata in senso crescente
            SORT_DESC = 2     // Colonna ordinata in senso decrescente
        }


        /// <summary>
        /// Filtro su un campo della tabella
        /// </summary>
        [Serializable]
        public class CCursorField 
            : ICopyObject, IDMDXMLSerializable
        {
            private bool m_IsSet;
            private object[] m_Values;
            private string m_FieldName;
            private adDataTypeEnum m_DataType;
            private OP m_Operator;
            private bool m_IncludeNulls;
            private SortEnum m_SortOrder;
            private int m_SortPriority;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCursorField()
            {
                this.m_Operator = OP.OP_EQ;
                this.m_DataType = 0;
                this.m_Values = new object[] { DBNull.Value, DBNull.Value };
                this.m_IsSet = false;
                this.m_IncludeNulls = false;
                this.m_SortPriority = -1;
                this.m_SortOrder = SortEnum.SORT_NOTSET;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="name"></param>
            /// <param name="tType"></param>
            /// <param name="op"></param>
            /// <param name="nullable"></param>
            public CCursorField(string name, adDataTypeEnum tType, OP op = OP.OP_EQ, bool nullable = false) : this()
            {
                m_FieldName = name;
                m_DataType = tType;
                m_Operator = op;
                m_IncludeNulls = nullable;
            }

            /// <summary>
            /// Restituisce o imposta il valore del filtro
            /// </summary>
            public object Value
            {
                get
                {
                    return this.m_Values[0];
                }

                set
                {
                    this.m_Values[0] = Convert(value);
                    this.m_IsSet = true;
                }
            }

            public object Value1
            {
                get
                {
                    return m_Values[1];
                }

                set
                {
                    m_Values[1] = Convert(value);
                    m_IsSet = true;
                }
            }

            /// <summary>
            /// Imposta la condizione "In" sul valore del campo
            /// </summary>
            /// <param name="values"></param>
            public void ValueIn(object[] values)
            {
                if (values is null) values = new object[] { };
                if (values.Length == 1)
                {
                    this.Operator = OP.OP_EQ;
                    this.Value = values[0];
                }
                else
                {
                    this.Operator = OP.OP_IN;
                    this.m_Values = values;
                }
                this.m_IsSet = true;
            }

            private object Convert(object value)
            {
                if (value is DBNull || value is null)
                {
                    return DBNull.Value;
                }

                switch (m_DataType)
                {
                    case adDataTypeEnum.adArray:
                        {
                            if (value is Array)
                            {
                                return value;
                            }
                            else
                            {
                                return new[] { value };
                            }

                        }

                    case adDataTypeEnum.adBoolean:
                        {
                            return DMD.Booleans.CBool(value);
                        }

                    case adDataTypeEnum.adChar:
                    case adDataTypeEnum.adWChar:
                    case adDataTypeEnum.adVarChar:
                    case adDataTypeEnum.adVarWChar:
                        {
                            return DMD.Strings.CStr(value);
                        }

                    case adDataTypeEnum.adBigInt:
                        {
                            return DMD.Longs.CLng(value);
                        }

                    case adDataTypeEnum.adCurrency:
                    case adDataTypeEnum.adDecimal:
                        {
                            return DMD.Decimals.CDec(value);
                        }

                    case adDataTypeEnum.adDate:
                    case adDataTypeEnum.adDBDate:
                    case adDataTypeEnum.adDBTime:
                    case adDataTypeEnum.adDBTimeStamp:
                        {
                            return DMD.DateUtils.CDate(value);
                        }

                    case adDataTypeEnum.adDouble:
                        {
                            return DMD.Doubles.CDbl(value);
                        }

                    case adDataTypeEnum.adInteger:
                        {
                            return DMD.Integers.CInt(value);
                        }

                    case adDataTypeEnum.adSingle:
                        {
                            return DMD.Floats.CSng(value);
                        }

                    case adDataTypeEnum.adSmallInt:
                        {
                            return DMD.Shorts.CShort(value);
                        }

                    case adDataTypeEnum.adTinyInt:
                        {
                            return DMD.SBytes.CSByte(value);
                        }

                    case adDataTypeEnum.adUnsignedBigInt:
                        {
                            return DMD.ULongs.CULng(value);
                        }

                    case adDataTypeEnum.adUnsignedInt:
                        {
                            return DMD.UIntegers.CUInt(value);
                        }

                    case adDataTypeEnum.adUnsignedSmallInt:
                        {
                            return DMD.UShorts.CUShort(value);
                        }

                    case adDataTypeEnum.adUnsignedTinyInt:
                        {
                            return DMD.Bytes.CByte(value);
                        }

                    default:
                        {
                            return value;
                        }
                }
            }

            public void Between(object a, object b)
            {
                if (Sistema.Types.IsNull(a))
                {
                    if (Sistema.Types.IsNull(b))
                    {
                        m_Values = new object[1];
                        m_Values[0] = null;
                        m_Operator = OP.OP_EQ;
                    }
                    else
                    {
                        m_Values = new object[1];
                        m_Values[0] = b;
                        m_Operator = OP.OP_LE;
                    }
                }
                else if (Sistema.Types.IsNull(b))
                {
                    m_Values = new object[1];
                    m_Values[0] = a;
                    m_Operator = OP.OP_GE;
                }
                else
                {
                    m_Operator = OP.OP_BETWEEN;
                    m_Values[0] = a;
                    m_Values[1] = b;
                }

                m_IsSet = true;
            }

            public bool IsSet()
            {
                return m_IsSet;
            }

            /// <summary>
        /// Restituisce vero se lo stato del campo è diverso da quello predefinito
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsChanged()
            {
                return IsSet() || m_SortOrder != SortEnum.SORT_NOTSET || m_SortPriority != -1 || m_Operator != OP.OP_EQ || m_IncludeNulls != false;
            }

            public bool IsExpression(string text)
            {
                const string chars = "([.])";
                for (int i = 1, loopTo = DMD.Strings.Len(chars); i <= loopTo; i++)
                {
                    if (DMD.Strings.InStr(text, DMD.Strings.Mid(chars, i, 1)) > 0)
                        return true;
                }

                return false;
            }

            public void Clear()
            {
                m_Values = new object[] { DBNull.Value, DBNull.Value };
                m_IsSet = false;
            }

            public OP Operator
            {
                get
                {
                    return m_Operator;
                }

                set
                {
                    m_Operator = value;
                }
            }

            public bool IncludeNulls
            {
                get
                {
                    return m_IncludeNulls;
                }

                set
                {
                    m_IncludeNulls = value;
                }
            }

            public adDataTypeEnum DataType
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

            public string FieldName
            {
                get
                {
                    return m_FieldName;
                }
            }

            private string GetOperatorString()
            {
                switch (Operator)
                {
                    case OP.OP_NE:
                        {
                            return "<>";
                        }

                    case OP.OP_LT:
                        {
                            return "<";
                        }

                    case OP.OP_LE:
                        {
                            return "<=";
                        }

                    case OP.OP_EQ:
                        {
                            return "=";
                        }

                    case OP.OP_GE:
                        {
                            return ">=";
                        }

                    case OP.OP_GT:
                        {
                            return ">";
                        }

                    case OP.OP_LIKE:
                        {
                            return "Like";
                        }

                    case OP.OP_NOTLIKE:
                        {
                            return "Not Like";
                        }

                    case OP.OP_BETWEEN:
                        {
                            return "Between";
                        }

                    case OP.OP_IN:
                        {
                            return "In";
                        }

                    case OP.OP_NOTIN:
                        {
                            return "Not In";
                        }

                    case OP.OP_ALLBITAND:
                        {
                            return " BAND ";
                        }

                    case OP.OP_ANYBITAND:
                        {
                            return " BAND ";
                        }

                    case OP.OP_ANYBITOR:
                        {
                            return " BOR ";
                        }

                    case OP.OP_ALLBITOR:
                        {
                            return " BOR ";
                        }

                    default:
                        {
                            return "";
                        }
                }
            }

            private string GetFieldNameInternal(string name)
            {
                if (IsExpression(name))
                {
                    return name;
                }
                else
                {
                    return "[" + name + "]";
                }
            }

            public SortEnum SortOrder
            {
                get
                {
                    return m_SortOrder;
                }

                set
                {
                    m_SortOrder = value;
                }
            }

            public int SortPriority
            {
                get
                {
                    return m_SortPriority;
                }

                set
                {
                    m_SortPriority = value;
                }
            }

            public virtual string GetSQL()
            {
                return GetSQL(FieldName);
            }

            public virtual string GetSQL(string nomeCampoOverride)
            {
                string opStr, ret;
                string nomeCampo = GetFieldNameInternal(nomeCampoOverride);
                ret = nomeCampo;
                if (DMD.Strings.InStr(ret, "%OP%") > 0 & DMD.Strings.InStr(ret, "%VALUE%") > 0)
                {
                    if (Sistema.Types.IsNull(m_Values[0]))
                    {
                        switch (m_Operator)
                        {
                            case OP.OP_EQ:
                                {
                                    ret = nomeCampo + " Is Null";
                                    break;
                                }

                            case OP.OP_NE:
                                {
                                    ret = "Not (" + nomeCampo + " Is Null)";
                                    break;
                                }

                            default:
                                {
                                    throw new InvalidOperationException("CCursorField: Operatore non valido su NULL");
                                     
                                }
                        }
                    }
                    else
                    {
                        opStr = GetOperatorString();
                        switch (DBUtils.GetDataTypeFamily(m_DataType))
                        {
                            case DBTypesEnum.DBNUMERIC_TYPE:
                                {
                                    if (opStr == "Like")
                                        opStr = "=";
                                    if (opStr == "Not Like")
                                        opStr = "<>";
                                    ret = "";
                                    ret = DMD.Strings.Replace(ret, "%OP%", opStr);
                                    ret = DMD.Strings.Replace(ret, "%VALUE%", DBUtils.DBNumber(m_Values[0]));
                                    break;
                                }

                            case DBTypesEnum.DBDATETIME_TYPE:
                                {
                                    if (opStr == "Between")
                                    {
                                        ret = DMD.Strings.Replace(ret, "%OP%", opStr);
                                        ret = DMD.Strings.Replace(ret, "%VALUE%", DBUtils.DBDate(m_Values[0]) + " AND " + DBUtils.DBDate(m_Values[1]));
                                    }
                                    else
                                    {
                                        if (opStr == "Like")
                                            opStr = "=";
                                        if (opStr == "Not Like")
                                            opStr = "<>";
                                        ret = DMD.Strings.Replace(ret, "%OP%", opStr);
                                        ret = DMD.Strings.Replace(ret, "%VALUE%", DBUtils.DBDate(m_Values[0]));
                                    }

                                    break;
                                }

                            case DBTypesEnum.DBBOOLEAN_TYPE:
                                {
                                    if (opStr == "Like")
                                        opStr = "=";
                                    if (opStr == "Not Like")
                                        opStr = "<>";
                                    ret = DMD.Strings.Replace(ret, "%OP%", opStr);
                                    ret = DMD.Strings.Replace(ret, "%VALUE%", DBUtils.DBBool(m_Values[0]));
                                    break;
                                }

                            case DBTypesEnum.DBTEXT_TYPE:
                                {
                                    if (opStr == "Like")
                                    {
                                        ret = DMD.Strings.Replace(ret, "%OP%", opStr);
                                        ret = DMD.Strings.Replace(ret, "%VALUE%", "'" + DMD.Strings.Replace(DMD.Strings.CStr(m_Values[0]), "'", "''") + "'");
                                    }
                                    else if (opStr == "Not Like")
                                    {
                                        ret = DMD.Strings.Replace(ret, "%OP%", "Like");
                                        ret = "Not (" + DMD.Strings.Replace(ret, "%VALUE%", "'" + DMD.Strings.Replace(DMD.Strings.CStr(m_Values[0]), "'", "''") + "'") + ")";
                                    }
                                    else
                                    {
                                        ret = DMD.Strings.Replace(ret, "%OP%", opStr);
                                        ret = DMD.Strings.Replace(ret, "%VALUE%", DBUtils.DBString(m_Values[0]));
                                    }

                                    break;
                                }

                            case DBTypesEnum.DBBINARY_TYPE:
                                {
                                    throw new NotImplementedException("CCursorField: Tipo di dato binario non implementato nel cursore");
                                     
                                }

                            default:
                                {
                                    throw new NotSupportedException("CCursorField: Tipo di dato non supportato nel cursore");
                                     
                                }
                        }

                        if (m_IncludeNulls)
                            ret += " Or (" + DMD.Strings.Replace(DMD.Strings.Replace(nomeCampo, "%OP%", " Is "), "%VALUE%", " Null") + ")";
                    }
                }
                else
                {
                    switch (Operator)
                    {
                        case OP.OP_BETWEEN:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    // Case DBTypesEnum.DBBOOLEAN_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBBool(Me.m_Values(0)) & " AND " & DBUtils.DBBool(Me.m_Values(1))
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " >= " + DBUtils.DBBool(m_Values[0]) + " AND " + nomeCampo + " <= " + DBUtils.DBBool(m_Values[1]);
                                            break;
                                        }
                                    // Case DBTypesEnum.DBDATETIME_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBDate(Me.m_Values(0)) & " AND " & DBUtils.DBDate(Me.m_Values(1))
                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " >= " + DBUtils.DBDate(m_Values[0]) + " AND " + nomeCampo + " <= " + DBUtils.DBDate(m_Values[1]);
                                            break;
                                        }
                                    // Case DBTypesEnum.DBNUMERIC_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBNumber(Me.m_Values(0)) & " AND " & DBUtils.DBNumber(Me.m_Values(1))
                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " >= " + DBUtils.DBNumber(m_Values[0]) + " AND " + nomeCampo + " <= " + DBUtils.DBNumber(m_Values[1]);
                                            break;
                                        }
                                    // Case DBTypesEnum.DBTEXT_TYPE : ret = nomeCampo & " BETWEEN " & DBUtils.DBString(Me.m_Values(0)) & " AND " & DBUtils.DBString(Me.m_Values(1))
                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " >= " + DBUtils.DBString(m_Values[0]) + " AND " + nomeCampo + " <= " + DBUtils.DBString(m_Values[1]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                             
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_EQ:
                            {
                                if (Sistema.Types.IsNull(m_Values[0]))
                                {
                                    ret = nomeCampo + " Is Null";
                                }
                                else
                                {
                                    switch (DBUtils.GetDataTypeFamily(m_DataType))
                                    {
                                        case DBTypesEnum.DBBOOLEAN_TYPE:
                                            {
                                                ret = nomeCampo + " =" + DBUtils.DBBool(m_Values[0]);
                                                break;
                                            }

                                        case DBTypesEnum.DBDATETIME_TYPE:
                                            {
                                                ret = nomeCampo + " =" + DBUtils.DBDate(m_Values[0]);
                                                break;
                                            }

                                        case DBTypesEnum.DBNUMERIC_TYPE:
                                            {
                                                ret = nomeCampo + " =" + DBUtils.DBNumber(m_Values[0]);
                                                break;
                                            }

                                        case DBTypesEnum.DBTEXT_TYPE:
                                            {
                                                ret = nomeCampo + " =" + DBUtils.DBString(m_Values[0]);
                                                break;
                                            }

                                        default:
                                            {
                                                throw new ArgumentException("Tipo di dato non supportato");
                                                 
                                            }
                                    }

                                    if (m_IncludeNulls)
                                        ret = ret + " Or " + nomeCampo + " Is Null";
                                }

                                break;
                            }

                        case OP.OP_GE:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " >=" + DBUtils.DBBool(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " >=" + DBUtils.DBDate(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " >=" + DBUtils.DBNumber(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " >=" + DBUtils.DBString(m_Values[0]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                             
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_GT:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " >" + DBUtils.DBBool(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " >" + DBUtils.DBDate(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " >" + DBUtils.DBNumber(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " >" + DBUtils.DBString(m_Values[0]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                             
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_IN:
                            {
                                var buffer = new System.Text.StringBuilder();
                                if (this.m_Values.Length > 0)
                                {
                                    buffer.Append(nomeCampo);
                                    buffer.Append(" In (");
                                    for (int i = 0; i < this.m_Values.Length; i++)
                                    {
                                        if (i > 0) buffer.Append(",");
                                        switch (DBUtils.GetDataTypeFamily(m_DataType))
                                        {
                                            case DBTypesEnum.DBBOOLEAN_TYPE:
                                                {
                                                    buffer.Append(DBUtils.DBBool(m_Values[i]));
                                                    break;
                                                }

                                            case DBTypesEnum.DBDATETIME_TYPE:
                                                {
                                                    buffer.Append(DBUtils.DBDate(m_Values[i]));
                                                    break;
                                                }

                                            case DBTypesEnum.DBNUMERIC_TYPE:
                                                {
                                                    buffer.Append(DBUtils.DBNumber(m_Values[i]));
                                                    break;
                                                }

                                            case DBTypesEnum.DBTEXT_TYPE:
                                                {
                                                    buffer.Append(DBUtils.DBString(m_Values[i]));
                                                    break;
                                                }

                                            default:
                                                {
                                                    throw new ArgumentException("Tipo di dato non supportato");

                                                }
                                        }
                                    }
                                    buffer.Append(")");
                                    if (this.m_IncludeNulls)
                                    {
                                        buffer.Append(" Or ");
                                        buffer.Append(nomeCampo);
                                        buffer.Append(" Is Null");
                                    }
                                } else {
                                    if (m_IncludeNulls)
                                    {
                                        buffer.Append(nomeCampo);
                                        buffer.Append(" Is Null");
                                    }
                                    else
                                    {
                                        buffer.Append(nomeCampo);
                                        buffer.Append("<>");
                                        buffer.Append(nomeCampo);
                                    }
                                }
                                
                                ret = buffer.ToString();
                                break;
                            }

                        case OP.OP_LE:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " <=" + DBUtils.DBBool(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " <=" + DBUtils.DBDate(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " <=" + DBUtils.DBNumber(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " <=" + DBUtils.DBString(m_Values[0]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                             
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_LIKE:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " =" + DBUtils.DBBool(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " =" + DBUtils.DBDate(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " =" + DBUtils.DBNumber(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " LIKE " + DBUtils.DBString(m_Values[0]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                             
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_LT:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " <" + DBUtils.DBBool(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " <" + DBUtils.DBDate(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " <" + DBUtils.DBNumber(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " <" + DBUtils.DBString(m_Values[0]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                             
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_NE:
                            {
                                if (Sistema.Types.IsNull(m_Values[0]))
                                {
                                    ret = nomeCampo + " Is Not Null";
                                }
                                else
                                {
                                    switch (DBUtils.GetDataTypeFamily(m_DataType))
                                    {
                                        case DBTypesEnum.DBBOOLEAN_TYPE:
                                            {
                                                ret = nomeCampo + " <>" + DBUtils.DBBool(m_Values[0]);
                                                break;
                                            }

                                        case DBTypesEnum.DBDATETIME_TYPE:
                                            {
                                                ret = nomeCampo + " <>" + DBUtils.DBDate(m_Values[0]);
                                                break;
                                            }

                                        case DBTypesEnum.DBNUMERIC_TYPE:
                                            {
                                                ret = nomeCampo + " <>" + DBUtils.DBNumber(m_Values[0]);
                                                break;
                                            }

                                        case DBTypesEnum.DBTEXT_TYPE:
                                            {
                                                ret = nomeCampo + " <>" + DBUtils.DBString(m_Values[0]);
                                                break;
                                            }

                                        default:
                                            {
                                                 
                                                throw new ArgumentException("Tipo di dato non supportato");
                                                
                                            }
                                    }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_NOTIN:
                            {
                                ret = nomeCampo + " Not In (";
                                for (int i = 0, loopTo1 = DMD.Arrays.UBound(m_Values); i <= loopTo1; i++)
                                {
                                    if (i > 0)
                                        ret += ",";
                                    switch (DBUtils.GetDataTypeFamily(m_DataType))
                                    {
                                        case DBTypesEnum.DBBOOLEAN_TYPE:
                                            {
                                                ret += DBUtils.DBBool(m_Values[i]);
                                                break;
                                            }

                                        case DBTypesEnum.DBDATETIME_TYPE:
                                            {
                                                ret += DBUtils.DBDate(m_Values[i]);
                                                break;
                                            }

                                        case DBTypesEnum.DBNUMERIC_TYPE:
                                            {
                                                ret += DBUtils.DBNumber(m_Values[i]);
                                                break;
                                            }

                                        case DBTypesEnum.DBTEXT_TYPE:
                                            {
                                                ret += DBUtils.DBString(m_Values[i]);
                                                break;
                                            }

                                        default:
                                            {
                                                throw new NotSupportedException("Tipo di dato non supportato");
                                                 
                                            }
                                    }
                                }

                                ret += ")";
                                if (m_IncludeNulls)
                                    ret += " Or " + nomeCampo + " Is Null";
                                break;
                            }
                            //break;
                        case OP.OP_NOTLIKE:
                            {
                                switch (DBUtils.GetDataTypeFamily(m_DataType))
                                {
                                    case DBTypesEnum.DBBOOLEAN_TYPE:
                                        {
                                            ret = nomeCampo + " <> " + DBUtils.DBBool(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBDATETIME_TYPE:
                                        {
                                            ret = nomeCampo + " <>" + DBUtils.DBDate(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBNUMERIC_TYPE:
                                        {
                                            ret = nomeCampo + " <>" + DBUtils.DBNumber(m_Values[0]);
                                            break;
                                        }

                                    case DBTypesEnum.DBTEXT_TYPE:
                                        {
                                            ret = nomeCampo + " Not LIKE " + DBUtils.DBString(m_Values[0]);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new ArgumentException("Tipo di dato non supportato");
                                            
                                        }
                                }

                                if (m_IncludeNulls)
                                    ret = ret + " Or " + nomeCampo + " Is Null";
                                break;
                            }

                        case OP.OP_ANYBITAND:
                            {
                                ret = "((" + nomeCampo + " BAND " + DBUtils.DBNumber(m_Values[0]) + ") <> 0)";
                                if (m_IncludeNulls)
                                    ret = ret + " Or (" + nomeCampo + " Is Null)";
                                break;
                            }

                        case OP.OP_ALLBITAND:
                            {
                                ret = "((" + nomeCampo + " BAND " + DBUtils.DBNumber(m_Values[0]) + ") = " + DBUtils.DBNumber(m_Values[0]) + ")";
                                if (m_IncludeNulls)
                                    ret = ret + " Or (" + nomeCampo + " Is Null)";
                                break;
                            }

                        case OP.OP_ANYBITOR:
                            {
                                ret = "((" + nomeCampo + " BOR " + DBUtils.DBNumber(m_Values[0]) + ") <> 0)";
                                if (m_IncludeNulls)
                                    ret = ret + " Or (" + nomeCampo + " Is Null)";
                                break;
                            }

                        case OP.OP_ALLBITOR:
                            {
                                ret = "((" + nomeCampo + " BOR " + DBUtils.DBNumber(m_Values[0]) + ") = " + DBUtils.DBNumber(m_Values[0]) + ")";
                                if (m_IncludeNulls)
                                    ret = ret + " Or (" + nomeCampo + " Is Null)";
                                break;
                            }

                        default:
                            {
                                throw new ArgumentOutOfRangeException("Operator");
                                 
                            }
                    }
                }

                return ret;
            }

            internal CCursorField CopyFrom(CCursorField field)
            {
                {
                    var withBlock = (CCursorField)field;
                    m_FieldName = withBlock.FieldName;
                    m_DataType = withBlock.DataType;
                    m_Operator = withBlock.Operator;
                    m_IncludeNulls = withBlock.IncludeNulls;
                    m_SortOrder = withBlock.SortOrder;
                    m_SortPriority = withBlock.SortPriority;
                    m_Values = (object[])withBlock.m_Values.Clone();
                    // Me.m_Value1 = .Value1
                    m_IsSet = withBlock.IsSet();
                }

                return this;
            }

            public object InitFrom(object field)
            {
                {
                    var withBlock = (CCursorField)field;
                    m_DataType = withBlock.DataType;
                    m_Operator = withBlock.Operator;
                    m_IncludeNulls = withBlock.IncludeNulls;
                    m_SortOrder = withBlock.SortOrder;
                    m_SortPriority = withBlock.SortPriority;
                    m_Values = (object[])withBlock.m_Values.Clone();
                    // Me.m_Value1 = .Value1
                    m_IsSet = withBlock.IsSet();
                }

                return this;
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IsSet":
                        {
                            m_IsSet = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "FieldName":
                        {
                            m_FieldName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataType":
                        {
                            m_DataType = (adDataTypeEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Operator":
                        {
                            m_Operator = (OP)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IncludeNulls":
                        {
                            m_IncludeNulls = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "SortOrder":
                        {
                            m_SortOrder = (SortEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SortPriority":
                        {
                            m_SortPriority = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Values":
                        {
                            m_Values = parseFieldValues(fieldValue);
                            break;
                        }
                }
            }

            protected virtual object[] parseFieldValues(object value)
            {
                if (!DMD.Arrays.IsArray(value))
                {
                    value = new[] { value };
                }

                object[] ret = (object[])value;
                switch (m_DataType)
                {
                    case adDataTypeEnum.adBoolean:
                        {
                            for (int i = 0, loopTo = DMD.Arrays.UBound(ret); i <= loopTo; i++)
                                ret[i] = Sistema.Formats.ParseBool(ret[i]);
                            break;
                        }

                    case adDataTypeEnum.adInteger:
                    case adDataTypeEnum.adBigInt:
                    case adDataTypeEnum.adSmallInt:
                    case adDataTypeEnum.adTinyInt:
                        {
                            for (int i = 0, loopTo1 = DMD.Arrays.UBound(ret); i <= loopTo1; i++)
                                ret[i] = Sistema.Formats.ParseInteger(ret[i]);
                            break;
                        }

                    case adDataTypeEnum.adDouble:
                    case adDataTypeEnum.adSingle:
                        {
                            for (int i = 0, loopTo2 = DMD.Arrays.UBound(ret); i <= loopTo2; i++)
                                ret[i] = Sistema.Formats.ParseDouble(ret[i]);
                            break;
                        }

                    case adDataTypeEnum.adDate:
                        {
                            for (int i = 0, loopTo3 = DMD.Arrays.UBound(ret); i <= loopTo3; i++)
                                ret[i] = Sistema.Formats.ParseDate(ret[i]);
                            break;
                        }

                    case adDataTypeEnum.adWChar:
                    case adDataTypeEnum.adChar:
                    case adDataTypeEnum.adVarChar:
                    case adDataTypeEnum.adVarWChar:
                        {
                            for (int i = 0, loopTo4 = DMD.Arrays.UBound(ret); i <= loopTo4; i++)
                                ret[i] = Sistema.Formats.ToString(ret[i]);
                            break;
                        }
                }

                return ret;
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IsSet", m_IsSet);
                writer.WriteAttribute("FieldName", m_FieldName);
                writer.WriteAttribute("DataType", (int?)m_DataType);
                writer.WriteAttribute("Operator", (int?)m_Operator);
                writer.WriteAttribute("IncludeNulls", m_IncludeNulls);
                writer.WriteAttribute("SortOrder", (int?)m_SortOrder);
                writer.WriteAttribute("SortPriority", m_SortPriority);
                writer.WriteTag("Values", m_Values);
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }

            object ICopyObject.CopyFrom(object source)
            {
                return this.CopyFrom((CCursorField) source);
            }

            ~CCursorField()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}