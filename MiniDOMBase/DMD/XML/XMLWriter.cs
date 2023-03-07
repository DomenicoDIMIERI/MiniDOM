using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace minidom.XML
{

    /// <summary>
    /// Oggetto
    /// </summary>
    public class XMLWriter
    {
        private string m_Encoding;
        private CCollection<string> m_OpenedTags = new CCollection<string>();
        private Dictionary<string, string> m_Attributes = new Dictionary<string, string>();
        private System.Text.StringBuilder m_Buffer = new System.Text.StringBuilder(2048);
        // Private m_Buffer1 As New System.Text.StringBuilder(2048)
        private Sistema.CSettings m_Settings = null;
        private XMLSerializeMethod m_Method = XMLSerializeMethod.Document;
        private string m_BaseElemType = DMD.Strings.vbNullString;
        private bool m_IsArray = false;
        private bool m_IsInTag = false;

        public XMLWriter()
        {
            DMDObject.IncreaseCounter(this);
            m_Encoding = Utils.Serializer.Encoding;
        }

        public string Encoding
        {
            get
            {
                return m_Encoding;
            }

            set
            {
                m_Encoding = value;
            }
        }

        public Sistema.CSettings Settings
        {
            get
            {
                if (m_Settings is null)
                    m_Settings = new Sistema.CSettings();
                return m_Settings;
            }
        }

        public int Length
        {
            get
            {
                return m_Buffer.Length;
            }
        }

        /// <summary>
        /// Aggiunge una stringa di testo senza formattazione
        /// </summary>
        /// <param name="text"></param>
        /// <remarks></remarks>
        public void WriteRowString(string text)
        {
            m_Buffer.Append(text);
        }

        /// <summary>
        /// Aggiunge una stringa di testo senza formattazione
        /// </summary>
        /// <param name="text"></param>
        /// <remarks></remarks>
        public void WriteRowData(string text)
        {
            m_Buffer.Append(text);
        }

        public void BeginTag(string tagName)
        {
            CheckInTag();
            m_OpenedTags.Add(Strings.Trim(tagName));
            m_Buffer.Append("<" + tagName);
            m_IsInTag = true;
            // Me.m_CurrTag = tagName
            // Me.m_Value = ""
        }

        public void WriteAttribute1(string key, object value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            if (value is byte)
            {
                WriteAttribute(key, (byte)value);
            }
            else if (value is byte?)
            {
                WriteAttribute(key, (byte?)value);
            }
            else if (value is short)
            {
                WriteAttribute(key, (short)value);
            }
            else if (value is short?)
            {
                WriteAttribute(key, (short?)value);
            }
            else if (value is int)
            {
                WriteAttribute(key, (int)value);
            }
            else if (value is int?)
            {
                WriteAttribute(key, (int?)value);
            }
            else if (value is float)
            {
                WriteAttribute(key, (float)value);
            }
            else if (value is float?)
            {
                WriteAttribute(key, (float?)value);
            }
            else if (value is double)
            {
                WriteAttribute(key, (double)value);
            }
            else if (value is double?)
            {
                WriteAttribute(key, (double?)value);
            }
            else if (value is long)
            {
                WriteAttribute(key, (long)value);
            }
            else if (value is long?)
            {
                WriteAttribute(key, (long?)value);
            }
            else if (value is decimal)
            {
                WriteAttribute(key, (decimal)value);
            }
            else if (value is decimal?)
            {
                WriteAttribute(key, (decimal?)value);
            }
            else if (value is string)
            {
                WriteAttribute(key, (string)value);
            }
            else if (value is bool)
            {
                WriteAttribute(key, (bool)value);
            }
            else if (value is bool?)
            {
                WriteAttribute(key, (bool?)value);
            }
            else if (value is DateTime)
            {
                WriteAttribute(key, (DateTime)value);
            }
            else if (value is DateTime?)
            {
                WriteAttribute(key, (DateTime?)value);
            }
            else
            {
                WriteAttribute(key, Utils.Serializer.SerializeObject(value));
            }
        }

        public void WriteAttribute(string key, string value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeString(value);
            text = Utils.Serializer.SerializeString(text);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }


        // Public Sub WriteAttribute(ByVal key As String, ByVal value As Integer)
        // If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
        // Dim text As String = XML.Utils.Serializer.SerializeInteger(value)
        // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
        // '  Me.m_Attributes.Add(key, text)
        // End Sub

        public void WriteAttribute(string key, int? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeInteger(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, double? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeDouble(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, bool? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeBoolean(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, decimal? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeDouble(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, DateTime? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeDate(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, long? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeInteger(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, byte? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeInteger(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public void WriteAttribute(string key, short? value)
        {
            if (!m_IsInTag)
                throw new InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo");
            string text = Utils.Serializer.SerializeInteger(value);
            // Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            m_Attributes.Add(key, text);
        }

        public virtual bool CanCloseTag(string tag)
        {
            switch (Strings.UCase(Strings.Trim(tag)) ?? "")
            {
                case "IMG":
                case "INPUT":
                case "METADATA":
                case "BR":
                case "HR":
                    {
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }

        public void EndTag()
        {
            CheckInTag();
            string currTag = m_OpenedTags[m_OpenedTags.Count - 1];
            m_OpenedTags.RemoveAt(m_OpenedTags.Count - 1);
            if (CanCloseTag(currTag))
            {
                CloseAttributes();
            }
            // Me.m_Buffer.Append("/>")
            else
            {
                m_Buffer.Append("</" + currTag + ">");
            }
        }

        public void WriteTag(string tagname, bool? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteArray(Array value)
        {
            CheckInTag();
            // Me.m_Buffer.Append(XML.Utils.Serializer.SerializeArray(value))
            if (value is object)
            {
                for (int i = 0, loopTo = DMD.Arrays.UBound(value); i <= loopTo; i++)
                {
                    var o = value.GetValue(i);
                    CheckInTag();
                    BeginTag(Utils.XMLTypeName(o));
                    if (o is bool || o is bool?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeBoolean(o));
                    }
                    else if (o is byte || o is byte?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is short || o is short?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is int || o is int?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is long || o is long?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is float || o is float?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is double || o is double?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is decimal || o is decimal?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is DateTime || o is DateTime?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDate(o));
                    }
                    // Me.CheckInTag()
                    else if (o is string)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(o)));
                    }
                    else if (Arrays.IsArray(o))
                    {
                        // Throw New Exception("Errore: Tipo non previsto in WriteArray")
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeArray(o));
                    }
                    else if (o is IDMDXMLSerializable)
                    {
                        ((IDMDXMLSerializable)o).XMLSerialize(this);
                    }
                    else if (o is null || o is DBNull)
                    {
                        // no
                        CheckInTag();
                    }
                    // Me.m_Buffer.Append("<Nothing></Nothing>")
                    else if (o is Enum)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(Integers.CInt(o)));
                    }
                    else
                    {
                        throw new ArgumentException("Tipo non serializzabile: " + Sistema.vbTypeName(o));
                    }

                    EndTag();
                }
            }
        }

        public void Write<T>(T[] value, string basicType)
        {
            CheckInTag();
            // Me.m_Buffer.Append(XML.Utils.Serializer.SerializeArray(value, basicType, Me))
            if (value is object)
            {
                for (int i = 0, loopTo = DMD.Arrays.UBound(value); i <= loopTo; i++)
                {
                    object o = value[i];
                    CheckInTag();
                    BeginTag(basicType);
                    if (o is bool || o is bool?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeBoolean(o));
                    }
                    else if (o is byte || o is byte?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is short || o is short?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is int || o is int?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is long || o is long?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is float || o is float?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is double || o is double?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is decimal || o is decimal?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is DateTime || o is DateTime?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDate(o));
                    }
                    else if (o is string)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(o)));
                    }
                    else if (Arrays.IsArray(o))
                    {
                        Debug.Print("x");
                    }
                    else if (o is IDMDXMLSerializable)
                    {
                        ((IDMDXMLSerializable)o).XMLSerialize(this);
                    }
                    else if (o is null || o is DBNull)
                    {
                    }
                    else
                    {
                        throw new ArgumentException("Tipo non serializzabile: " + Sistema.vbTypeName(o));
                    }

                    EndTag();
                }
            }
        }

        public void WriteTag<T>(string tagname, T[] value)
        {
            BeginTag(tagname);
            if (value is object)
            {
                for (int i = 0, loopTo = DMD.Arrays.UBound(value); i <= loopTo; i++)
                {
                    object o = value[i];
                    CheckInTag();
                    BeginTag(Utils.XMLTypeName(o));
                    if (o is bool || o is bool?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeBoolean(o));
                    }
                    else if (o is byte || o is byte?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is short || o is short?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is int || o is int?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is long || o is long?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is float || o is float?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is double || o is double?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is decimal || o is decimal?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is DateTime || o is DateTime?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDate(o));
                    }
                    else if (o is string)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(o)));
                    }
                    else if (Arrays.IsArray(o))
                    {
                        Debug.Print("x");
                    }
                    else if (o is IDMDXMLSerializable)
                    {
                        ((IDMDXMLSerializable)o).XMLSerialize(this);
                    }
                    else if (o is null || o is DBNull)
                    {
                        CheckInTag();
                    }
                    else
                    {
                        throw new ArgumentException("Tipo non serializzabile: " + Sistema.vbTypeName(o));
                    }

                    EndTag();
                }
            }

            EndTag();
        }

        public void WriteTag<T>(string tagname, T[] value, string basicType)
        {
            BeginTag(tagname);
            if (value is object)
            {
                for (int i = 0, loopTo = DMD.Arrays.UBound(value); i <= loopTo; i++)
                {
                    object o = value[i];
                    CheckInTag();
                    BeginTag(basicType);
                    if (o is bool || o is bool?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeBoolean(o));
                    }
                    else if (o is byte || o is byte?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is short || o is short?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is int || o is int?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is long || o is long?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is float || o is float?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is double || o is double?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is decimal || o is decimal?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is DateTime || o is DateTime?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDate(o));
                    }
                    else if (o is string)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(o)));
                    }
                    else if (Arrays.IsArray(o))
                    {
                        Debug.Print("x");
                    }
                    else if (o is IDMDXMLSerializable)
                    {
                        ((IDMDXMLSerializable)o).XMLSerialize(this);
                    }
                    else if (o is null || o is DBNull)
                    {
                        CheckInTag();
                    }
                    else
                    {
                        throw new ArgumentException("Tipo non serializzabile: " + Sistema.vbTypeName(o));
                    }

                    EndTag();
                }
            }

            EndTag();
        }

        public void WriteTag(string tagname, object value)
        {
            if (value is bool || value is bool?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeBoolean(value));
                EndTag();
            }
            else if (value is byte || value is byte?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
                EndTag();
            }
            else if (value is short || value is short?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
                EndTag();
            }
            else if (value is int || value is int?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
                EndTag();
            }
            else if (value is long || value is long?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
                EndTag();
            }
            else if (value is float || value is float?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
                EndTag();
            }
            else if (value is double || value is double?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
                EndTag();
            }
            else if (value is decimal || value is decimal?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
                EndTag();
            }
            else if (value is DateTime || value is DateTime?)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeDate(value));
                EndTag();
            }
            else if (value is string)
            {
                BeginTag(tagname);
                CheckInTag();
                m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(value)));
                EndTag();
            }
            else if (Arrays.IsArray(value))
            {
                CheckInTag();
                BeginTag(tagname);
                Array arr = (Array)value;
                for (int i = 0, loopTo = arr.Length - 1; i <= loopTo; i++)
                {
                    var o = arr.GetValue(i);
                    BeginTag(Utils.XMLTypeName(o));
                    if (o is bool || o is bool?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeBoolean(o));
                    }
                    else if (o is byte || o is byte?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is short || o is short?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is int || o is int?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is long || o is long?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeInteger(o));
                    }
                    else if (o is float || o is float?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is double || o is double?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is decimal || o is decimal?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDouble(o));
                    }
                    else if (o is DateTime || o is DateTime?)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeDate(o));
                    }
                    else if (o is string)
                    {
                        CheckInTag();
                        m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(o)));
                    }
                    else if (Arrays.IsArray(o))
                    {
                        Debug.Print("x");
                    }
                    else if (o is IDMDXMLSerializable)
                    {
                        ((IDMDXMLSerializable)o).XMLSerialize(this);
                    }
                    else if (o is null || o is DBNull)
                    {
                        CheckInTag();
                    }
                    else
                    {
                        throw new ArgumentException("Tipo non serializzabile: " + Sistema.vbTypeName(o));
                    }

                    EndTag();
                }

                EndTag();
            }
            else if (value is IDMDXMLSerializable)
            {
                CheckInTag();
                BeginTag(tagname);
                BeginTag(Utils.XMLTypeName(value));
                ((IDMDXMLSerializable)value).XMLSerialize(this);
                EndTag();
                EndTag();
            }
            else if (Sistema.Types.IsNull(value))
            {
                CheckInTag();
                BeginTag(tagname);
                EndTag();
            }
            else
            {
                throw new ArgumentException("Tipo non serializzabile: " + Sistema.vbTypeName(value));
            }
        }

        public void WriteTag(string tagname, byte? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, short? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, int? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, long? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, float? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, double? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, decimal? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, DateTime? value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        public void WriteTag(string tagname, string value)
        {
            BeginTag(tagname);
            Write(value);
            EndTag();
        }

        private static readonly string vbQuote = DMD.Strings.CStr('"');

        private void CloseAttributes()
        {
            foreach (string k in m_Attributes.Keys)
            {
                m_Buffer.Append(" ");
                m_Buffer.Append(k);
                m_Buffer.Append("=");
                m_Buffer.Append(vbQuote);
                m_Buffer.Append(m_Attributes[k]);
                m_Buffer.Append(vbQuote);
            }

            m_Attributes.Clear();
        }

        /// <summary>
        /// Controlla lo stato del writer. Se siamo all'interno di un tag (è stata aperta una parentesi angolata, ma non è ancora stata chiusa), chiude il tag
        /// </summary>
        /// <remarks></remarks>
        public void CheckInTag()
        {
            if (m_IsInTag)
            {
                if (CanCloseTag(m_OpenedTags[m_OpenedTags.Count - 1]))
                {
                    CloseAttributes();
                    m_Buffer.Append("/>");
                }
                else
                {
                    CloseAttributes();
                    m_Buffer.Append(">");
                }
            }

            m_IsInTag = false;
        }

        public void Write(object value)
        {
            CheckInTag();
            if (Arrays.IsArray(value))
            {
                if (DMD.Arrays.Len((Array)value) > 0)
                {
                    WriteArray((Array)value);
                }
            }
            else if (value is bool || value is bool?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeBoolean(value));
            }
            else if (value is byte || value is byte?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
            }
            else if (value is short || value is short?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
            }
            else if (value is int || value is int?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
            }
            else if (value is long || value is long?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
            }
            else if (value is float || value is float?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
            }
            else if (value is double || value is double?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
            }
            else if (value is decimal || value is decimal?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
            }
            else if (value is DateTime || value is DateTime?)
            {
                m_Buffer.Append(Utils.Serializer.SerializeDate(value));
            }
            else if (value is string)
            {
                m_Buffer.Append(Utils.Serializer.SerializeString(Strings.CStr(value)));
            }
            else if (value is IDMDXMLSerializable)
            {
                BeginTag(Utils.XMLTypeName(value));
                ((IDMDXMLSerializable)value).XMLSerialize(this);
                EndTag();
            }
        }

        public void Write(bool? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeBoolean(value));
        }

        public void Write(string value)
        {
            CheckInTag();
            string text = Utils.Serializer.SerializeString(value);
            text = Utils.Serializer.SerializeString(text);
            m_Buffer.Append(text);
        }

        public void Write(byte? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
        }

        public void Write(short? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
        }

        public void Write(int? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
        }

        public void Write(long? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeInteger(value));
        }

        public void Write(float? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
        }

        public void Write(double? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
        }

        public void Write(DateTime? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeDate(value));
        }

        public void Write(decimal? value)
        {
            CheckInTag();
            m_Buffer.Append(Utils.Serializer.SerializeDouble(value));
        }

        public override string ToString()
        {
            string ret = m_Buffer.ToString();
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            if (Booleans.CBool(Strings.InStr(ret, "<Nothing><Nothing></Nothing></Nothing>")))
            {
                Debug.Print("test");
            }
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            return ret;
        }

        public void BeginDocument(XMLSerializeMethod method, object obj)
        {
            m_Method = method;
            switch (method)
            {
                case XMLSerializeMethod.Document:
                    {
                        m_Buffer.Append("<?xml version=\"1.0\" encoding=\"" + m_Encoding + "\"?>" + DMD.Strings.vbCrLf);
                        if (Arrays.IsArray(obj))
                        {
                            Array arr = (Array)obj;
                            if (arr.Length > 0)
                            {
                                m_BaseElemType = Utils.XMLTypeName(arr.GetValue(0));
                            }
                            else
                            {
                                m_BaseElemType = "Object";
                            }

                            m_IsArray = true;
                            m_Buffer.Append("<ArrayOf" + m_BaseElemType + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + DMD.Strings.vbCrLf);
                        }
                        else
                        {
                            m_IsArray = false;
                            m_BaseElemType = Utils.XMLTypeName(obj);
                            m_Buffer.Append("<" + m_BaseElemType + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + DMD.Strings.vbCrLf);
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        public void EndDocument()
        {
            switch (m_Method)
            {
                case XMLSerializeMethod.Document:
                    {
                        if (m_IsArray)
                        {
                            m_Buffer.Append("</ArrayOf" + m_BaseElemType + ">");
                        }
                        else
                        {
                            m_Buffer.Append("</" + m_BaseElemType + ">");
                        }

                        break;
                    }
            }
        }

        public void Clear()
        {
            m_OpenedTags.Clear();
            m_Buffer.Clear();
            m_BaseElemType = DMD.Strings.vbNullString;
            m_IsArray = false;
            m_IsInTag = false;
        }

        ~XMLWriter()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}