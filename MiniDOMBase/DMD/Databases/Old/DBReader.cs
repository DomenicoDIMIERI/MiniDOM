using System;
using System.Data;



namespace minidom
{
    public partial class Databases
    {
        public sealed class DBReader : IDisposable
        {
            private CDBEntity m_Schema;
            private IDataReader m_dbRis;
            private DataTable m_Dt;
            private int m_Index = -1;
            private string m_Prefix;

            public DBReader()
            {
            }

            public DBReader(IDataReader dbRis) : this(dbRis, null, "")
            {
            }

            public DBReader(IDataReader dbRis, CDBEntity schema) : this(dbRis, schema, "")
            {
            }

            public DBReader(IDataReader dbRis, CDBEntity schema, string prefix)
            {
                DMDObject.IncreaseCounter(this);
                if (dbRis is null)
                    throw new ArgumentNullException("dbRis");
                m_Schema = schema;
                m_dbRis = dbRis;
                m_Prefix = DMD.Strings.Trim(prefix);
            }

            public DBReader(CDBEntity schema, DataTable dt)
            {
                DMDObject.IncreaseCounter(this);
                if (schema is null)
                    throw new ArgumentNullException("schema");
                if (dt is null)
                    throw new ArgumentNullException("dt");
                m_Schema = schema;
                m_dbRis = null;
                m_Dt = dt;
                m_Index = -1;
                m_Prefix = DMD.Strings.Trim(Prefix);
            }

            public DBReader(CDBEntity schema) : this(schema, "SELECT * FROM " + schema.InternalName)
            {
            }

            public DBReader(CDBEntity schema, string sql) : this(schema, sql, "")
            {
            }

            public DBReader(CDBEntity schema, string sql, string prefix)
            {
                DMDObject.IncreaseCounter(this);
                if (schema is null)
                    throw new ArgumentNullException("schema");
                sql = DMD.Strings.Trim(sql);
                if (string.IsNullOrEmpty(sql))
                    throw new ArgumentNullException("sql");
                m_Schema = schema;
                m_dbRis = schema.Connection.ExecuteReader(sql);
                m_Prefix = DMD.Strings.Trim(prefix);
            }




            /// <summary>
        /// Restituisce lo schema sottostante
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CDBEntity Schema
            {
                get
                {
                    return m_Schema;
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che viene anteposta al nome dei campi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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

            /// <summary>
        /// Se il cursore non si trova oltre la fine del file carica il record corrente e restituisce vero altrimenti restituisce falso
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Read()
            {
                if (m_Dt is object)
                {
                    if (m_Index < m_Dt.Rows.Count - 1)
                    {
                        m_Index += 1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return m_dbRis.Read();
                }
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            // Public Sub Read(ByVal fieldName As String, ByRef value As Boolean)
            // value = Formats.ToBool(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Boolean))
            // value = Formats.ParseBool(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Byte)
            // value = Formats.ToInteger(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Byte))
            // value = Formats.ParseInteger(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As SByte)
            // value = Formats.ToInteger(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of SByte))
            // value = Formats.ParseInteger(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Short)
            // value = Formats.ToInteger(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Short))
            // value = Formats.ParseInteger(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Integer)
            // value = Formats.ToInteger(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Integer?)
            // value = Formats.ParseInteger(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Long)
            // value = Formats.ToInteger(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Long))
            // value = Formats.ParseInteger(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Single)
            // value = Formats.ToDouble(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Single))
            // value = Formats.ParseDouble(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Double)
            // value = Formats.ToDouble(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Double))
            // value = Formats.ParseDouble(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Decimal)
            // value = Formats.ToValuta(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Nullable(Of Decimal))
            // value = Formats.ToValuta(Me.GetValue(fieldName))
            // End Sub

            // Public Sub Read(ByVal fieldName As String, ByRef value As Date)
            // value = Formats.ToDate(Me.GetValue(fieldName))
            // End Sub
            // Public Sub Read(ByVal fieldName As String, ByRef value As Date?)
            // value = Formats.Par(Me.GetValue(fieldName))
            // End Sub

            /// <summary>
            /// Carica il campo del DB 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="fieldName"></param>
            /// <param name="defValue"></param>
            /// <returns></returns>
            public T Read<T>(string fieldName, T defValue = default)
            {
                object tmp = this.GetValue(fieldName);
                return DMD.IOUtils.TryParse<T>(tmp, defValue);
            }

            //public T? Read<T>(string fieldName, ref T? value) where T : struct
            //{
            //    var tmp = GetValue(fieldName);
            //    if (tmp is DBNull)
            //    {
            //        value = default;
            //    }
            //    else
            //    {
            //        value = (T)GetValue(fieldName);
            //    }

            //    return value;
            //}

            //public string Read(string fieldName, ref string value)
            //{
            //    var tmp = GetValue(fieldName);
            //    if (tmp is DBNull)
            //    {
            //        value = DMD.Strings.vbNullString;
            //    }
            //    else
            //    {
            //        value = DMD.Strings.CStr(tmp);
            //    }

            //    return value;
            //}

            public void Read(string fieldName, Array value)
            {
                value = (Array)GetValue(fieldName);
            }

            public int[] Read(string fieldName, int[] value)
            {
                value = null;
                var tmp = m_dbRis[GetFieldName(fieldName)];
                if (!(tmp is DBNull) && tmp is object)
                    value = DMD.Arrays.BytesToIntegers((byte[])tmp);
                return value;
            }

            // Public Function Read(Of T)(ByVal fieldName As String) As T
            // Return CType(Me.GetValue(fieldName), T)
            // End Function

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            /// <summary>
        /// Restituisce vero se il campo specificato contiene il valore DBNull
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsNull(string fieldName)
            {
                return GetValue(fieldName) is DBNull;
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            protected string GetFieldName(string value)
            {
                value = value.Replace('.', '#');
                if (string.IsNullOrEmpty(m_Prefix))
                    return value;
                var ret = new System.Text.StringBuilder();
                ret.Append(m_Prefix);
                ret.Append(value);
                return ret.ToString();
            }

            public object GetValue(string fieldName)
            {
                if (m_Dt is object)
                {
                    return m_Dt.Rows[m_Index][GetFieldName(fieldName)];
                }
                else
                {
                    return m_dbRis[GetFieldName(fieldName)];
                }
            }

            public T? GetValue<T>(string fieldName) where T : struct
            {
                var ret = GetValue(fieldName);
                if (ret is DBNull)
                    return default;
                return (T)ret;
            }

            public T GetValue<T>(string fieldName, object defValue = null)
            {
                var ret = GetValue(fieldName);
                if (ret is DBNull)
                    return (T)defValue;
                return (T)ret;
            }

            public string GetValue(string fieldName, string defValue)
            {
                var ret = GetValue(fieldName);
                if (ret is DBNull)
                    return defValue;
                return DMD.Strings.CStr(ret);
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */



            // This code added by Visual Basic to correctly implement the disposable pattern.
            public void Dispose()
            {
                if (m_dbRis is object)
                {
                    m_dbRis.Dispose();
                    m_dbRis = null;
                }

                if (m_Dt is object)
                {
                    m_Dt.Dispose();
                    m_Dt = null;
                }
                // If (Me.m_Schema IsNot Nothing) Then Me.m_Schema.Dispose() 
                m_Schema = null;
            }

            ~DBReader()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}