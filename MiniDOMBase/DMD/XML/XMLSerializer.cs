using DMD;
using System;
using System.Diagnostics;

namespace minidom.XML
{
    public class XMLSerializer
    {
        private string m_Encoding;

        public XMLSerializer()
        {
            DMDObject.IncreaseCounter(this);
            m_Encoding = "utf-8"; // "windows-1252""ISO-8859-1" '
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

        private System.Xml.XmlNode getNextElementSibling(System.Xml.XmlNode elem)
        {
            System.Xml.XmlNode n;
            n = elem.NextSibling;
            while (n is object)
            {
                if ((int)n.NodeType == 1)
                {
                    return n;
                }

                n = n.NextSibling;
            }

            return n;
        }

        private System.Xml.XmlNode getFirstElementChild(System.Xml.XmlNode elem)
        {
            System.Xml.XmlNode n;
            n = elem.FirstChild;
            while (n is object)
            {
                if ((int)n.NodeType == 1)
                {
                    return n;
                }

                n = n.NextSibling;
            }

            return n;
        }

        private object getNodeText(System.Xml.XmlNode node)
        {
            if (string.IsNullOrEmpty(node.InnerText))
            {
                return null;
            }
            else
            {
                return node.InnerText;
            }
        }

        public string GetInnerTAG(string text, string tagName)
        {
            int i, j, k;
            string ret;
            ret = "";
            i = DMD.Strings.InStr(text, "<" + tagName);
            if (i > 0)
            {
                j = DMD.Strings.InStr(i + DMD.Strings.Len(tagName), text, ">");
                if (j > i)
                {
                    k = DMD.Strings.InStr(j + 1, text, "</" + tagName + ">");
                    if (k <= j)
                    {
                        ret = DMD.Strings.Mid(text, j + 1);
                    }
                    else
                    {
                        ret = DMD.Strings.Mid(text, j + 1, k - j - 1);
                    }
                }
            }

            return ret;
        }



        // Public Overridable Function Serialize1(ByVal obj As Object, ByVal method As XMLSerializeMethod) As String
        // Dim writer As New minidom.XML.XMLWriter
        // writer.BeginDocument(method, obj)
        // writer.Write(obj)
        // writer.EndDocument()
        // Return writer.ToString
        // End Function

        public virtual string Serialize(object obj)
        {
            return Serialize(obj, XMLSerializeMethod.Document);
        }

        public virtual string Serialize(object obj, XMLSerializeMethod method)
        {
            var writer = new XMLWriter();
            string ret = Serialize(obj, method, writer);
            // writer.Dispose()
            return ret;
        }

        /// <summary>
        /// Serializzazione
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual string Serialize(object obj, XMLSerializeMethod method, XMLWriter writer)
        {
            if (method == XMLSerializeMethod.Document)
            {
                writer.WriteRowData("<?xml version=\"1.0\" encoding=\"" + m_Encoding + "\"?>" + DMD.Strings.vbNewLine);
                if (Arrays.IsArray(obj) && DMD.Arrays.Len((Array)obj) > 0)
                {
                    Array arr = (Array)obj;
                    writer.BeginTag("ArrayOf" + Utils.XMLTypeName(arr.GetValue(0)));
                    writer.WriteAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                }
                else
                {
                    writer.BeginTag(Utils.XMLTypeName(obj));
                    writer.WriteAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                }

                if (obj is IDMDXMLSerializable)
                {
                    ((IDMDXMLSerializable)obj).XMLSerialize(writer);
                }
                else
                {
                    writer.Write(obj);
                }

                writer.EndTag(); // += "</ArrayOf" + vbTypeName(obj[0]) + ">\n";
            }
            else
            {
                writer.Write(obj);
            }

            return writer.ToString();
        }

        public virtual object Deserialize(string text)
        {
            object ret;
            var xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node;
            System.Xml.XmlNode n;
            xmlDoc.LoadXml(text);
            node = xmlDoc.DocumentElement;
            if (Strings.Left(node.Name, 7) == "ArrayOf")
            {
                var tmp = new CCollection();
                n = getFirstElementChild(xmlDoc.DocumentElement);
                while (n is object)
                {
                    object rItem;
                    switch (n.Name ?? "")
                    {
                        case "String":
                            {
                                rItem = DMD.Strings.CStr(getNodeText(n));
                                break;
                            }

                        case "Int32":
                        case "Integer":
                            {
                                rItem = Utils.Serializer.DeserializeInteger(getNodeText(n));
                                break;
                            }

                        case "Byte":
                        case "SByte":
                            {
                                rItem = Utils.Serializer.DeserializeInteger(getNodeText(n));
                                break;
                            }

                        case "Int64":
                        case "Long":
                            {
                                rItem = Utils.Serializer.DeserializeLong(Strings.CStr(getNodeText(n)));
                                break;
                            }

                        case "Int16":
                        case "Short":
                            {
                                rItem = Utils.Serializer.DeserializeInteger(getNodeText(n));
                                break;
                            }

                        case "Single":
                        case "Double":
                            {
                                rItem = Utils.Serializer.DeserializeDouble(Strings.CStr(getNodeText(n)));
                                break;
                            }

                        case "Decimal":
                            {
                                rItem = Utils.Serializer.DeserializeDouble(Strings.CStr(getNodeText(n)));
                                break;
                            }

                        case "Boolean":
                            {
                                rItem = Utils.Serializer.DeserializeBoolean(getNodeText(n));
                                break;
                            }

                        default:
                            {
                                rItem = Sistema.Types.CreateInstance(n.Name);
                                IDMDXMLSerializable argtargetObject = (IDMDXMLSerializable)rItem;
                                Utils.Serializer.DeserializeInline(ref n, ref argtargetObject);
                                break;
                            }
                    }

                    tmp.Add(rItem);
                    n = getNextElementSibling(n);
                }

                ret = tmp.ToArray();
            }
            else
            {
                string tpName = xmlDoc.DocumentElement.Name;
                ret = Sistema.Types.CreateInstance(tpName);
                if (ret is object)
                {
                    System.Xml.XmlNode argcurrNode = xmlDoc.DocumentElement;
                    IDMDXMLSerializable argtargetObject1 = (IDMDXMLSerializable)ret;
                    Utils.Serializer.DeserializeInline(ref argcurrNode, ref argtargetObject1);
                    //xmlDoc.DocumentElement = (System.Xml.XmlElement)argcurrNode;
                }
            }

            xmlDoc = null;
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            if (ret is Databases.DBObjectCursorBase)
            {
                Debug.Print("Cursore");
            }
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            return ret;
        }

        public virtual object Deserialize(string text, Type type)
        {
            var x = new System.Xml.Serialization.XmlSerializer(type);
            var sr = new System.IO.StringReader(text);
            return x.Deserialize(sr);
        }

        public virtual object Deserialize(string text, string tpName)
        {
            // Return Deserialize(text, Sistema.GetType(tpName))
            object ret;
            var xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node;
            System.Xml.XmlNode n;
            // xmlDoc.async= "false"
            xmlDoc.LoadXml(text);
            node = xmlDoc.DocumentElement;
            if ((node.Name ?? "") == ("ArrayOf" + tpName ?? ""))
            {
                var tmp = new CCollection();
                n = getFirstElementChild(xmlDoc.DocumentElement);
                while (n is object)
                {
                    object rItem;
                    rItem = Sistema.Types.CreateInstance(n.Name);
                    IDMDXMLSerializable argtargetObject = (IDMDXMLSerializable)rItem;
                    Utils.Serializer.DeserializeInline(ref n, ref argtargetObject);
                    tmp.Add(rItem);
                    n = getNextElementSibling(n);
                }

                ret = tmp.ToArray();
            }
            else
            {
                ret = Sistema.Types.CreateInstance(tpName);
                if (ret is object)
                {
                    System.Xml.XmlNode argcurrNode = xmlDoc.DocumentElement;
                    IDMDXMLSerializable argtargetObject1 = (IDMDXMLSerializable)ret;
                    Utils.Serializer.DeserializeInline(ref argcurrNode, ref argtargetObject1);
                   // xmlDoc.DocumentElement = (System.Xml.XmlElement)argcurrNode;
                }
            }

            xmlDoc = null;
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            if (ret is Databases.DBObjectCursorBase)
            {
                Debug.Print("Cursore");
            }
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            return ret;
        }

        private object DeserializeNode(System.Xml.XmlNode cn)
        {
            object item;
            switch (cn.Name ?? "")
            {
                case "Nothing":
                case "Null":
                case "NULL":
                    {
                        if (cn.FirstChild is object && (cn.FirstChild.Name ?? "") == (cn.Name ?? ""))
                        {
                            return new object[] { null };
                        }
                        else
                        {
                            item = null;
                        }

                        break;
                    }

                case "String":
                case "string":
                    {
                        item = DMD.Strings.CStr(getNodeText(cn));
                        break;
                    }

                case "Number":
                case "number":
                    {
                        item = Utils.Serializer.DeserializeDouble(Strings.CStr(getNodeText(cn)));
                        break;
                    }

                case "Boolean":
                case "boolean":
                case "bool":
                case "Bool":
                    {
                        item = Utils.Serializer.DeserializeBoolean(getNodeText(cn));
                        break;
                    }

                case "Byte":
                case "SByte":
                case "int8":
                case "uint8":
                    {
                        item = Utils.Serializer.DeserializeInteger(getNodeText(cn));
                        break;
                    }

                case "Short":
                case "Int16":
                case "short":
                    {
                        item = Utils.Serializer.DeserializeInteger(getNodeText(cn));
                        break;
                    }

                case "UShort":
                case "UInt16":
                case "unsigned short":
                    {
                        item = Utils.Serializer.DeserializeInteger(getNodeText(cn));
                        break;
                    }

                case "Integer":
                case "int":
                case "Int32":
                    {
                        item = Utils.Serializer.DeserializeInteger(getNodeText(cn));
                        break;
                    }

                case "UInteger":
                case "unsigned int":
                case "UInt32":
                    {
                        item = Utils.Serializer.DeserializeInteger(getNodeText(cn));
                        break;
                    }

                case "Long":
                case "long":
                case "Ing64":
                    {
                        item = Utils.Serializer.DeserializeLong(Strings.CStr(getNodeText(cn)));
                        break;
                    }

                case "ULong":
                case "unsigned long":
                case "UIng64":
                    {
                        item = Utils.Serializer.DeserializeInteger(getNodeText(cn));
                        break;
                    }

                case "Double":
                case "Single":
                case "Decimal":
                    {
                        item = Utils.Serializer.DeserializeDouble(Strings.CStr(getNodeText(cn)));
                        break;
                    }

                case "Date":
                case "DateTime":
                    {
                        item = Utils.Serializer.DeserializeDate(getNodeText(cn));
                        break;
                    }

                default:
                    {
                        item = Sistema.Types.CreateInstance(cn.Name);
                        IDMDXMLSerializable argtargetObject = (IDMDXMLSerializable)item;
                        Utils.Serializer.DeserializeInline(ref cn, ref argtargetObject);
                        break;
                    }
            }
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            if (item is Databases.DBObjectCursorBase)
            {
                Debug.Print("Cursore");
            }
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            return item;
        }

        public virtual void DeserializeInline(ref System.Xml.XmlNode currNode, ref IDMDXMLSerializable targetObject)
        {
            System.Xml.XmlNode node;
            System.Xml.XmlNode cn;
            CCollection items;
            object item;
            foreach (System.Xml.XmlAttribute attr in currNode.Attributes)
                targetObject.SetFieldInternal(attr.Name, attr.Value);
            node = getFirstElementChild(currNode);
            while (node is object)
            {
                cn = getFirstElementChild(node);
                if (cn is object)
                {
                    if (getNextElementSibling(cn) is null)
                    {
                        item = DeserializeNode(cn);
                        targetObject.SetFieldInternal(node.Name, item);
                    }
                    else
                    {
                        items = new CCollection();
                        while (cn is object)
                        {
                            item = DeserializeNode(cn);
                            items.Add(item);
                            cn = getNextElementSibling(cn);
                        }

                        targetObject.SetFieldInternal(node.Name, items.ToArray());
                    }
                }
                else
                {
                    targetObject.SetFieldInternal(node.Name, getNodeText(node));
                }

                node = getNextElementSibling(node);
            }
        }

        // Public  Function FieldSerialize(ByVal value As Object) As String
        // If IsArray(value) Then
        // ElseIf IsObject(value) Then
        // ElseIf LCase(TypeName(value)) = "string" Then
        // FieldSerialize = Server.HTMLEncode(value)
        // ElseIf IsDate(value) Then
        // FieldSerialize = Server.HTMLEncode(Year(value) & "/" & Month(value) & "/" & Day(value) & " " & Hour(value) & "." & Minute(value) & "." & Second(value))
        // Else
        // FieldSerialize = Server.HTMLEncode("" & value)
        // End If
        // End Function

        public DateTime? DeserializeDate(object value)
        {
            string text = DMD.Strings.Trim(DMD.Strings.CStr(value));
            if (string.IsNullOrEmpty(text))
                return default;
            string[] n;
            string[] d;
            string[] t;
            DateTime ret;
            n = DMD.Strings.Split(text, " ");
            d = DMD.Strings.Split(n[0], "/");
            t = DMD.Strings.Split(n[1], ".");
            ret = DMD.DateUtils.MakeDate(Integers.CInt(d[0]), Integers.CInt(d[1]), Integers.CInt(d[2]), Integers.CInt(t[0]), Integers.CInt(t[1]), Integers.CInt(t[2]));
            return ret;
        }

        public bool? DeserializeBoolean(object value)
        {
            string text = DMD.Strings.Trim(DMD.Strings.CStr(value));
            if (string.IsNullOrEmpty(text))
                return default;
            text = DMD.Strings.UCase(Strings.Left(Strings.Trim(text), 1));
            return text == "T" | text == "1";
        }

        public int? DeserializeInteger(object value)
        {
            string text = DMD.Strings.Trim(DMD.Strings.CStr(value));
            if (string.IsNullOrEmpty(text))
                return default;
            return Sistema.Formats.ToInteger(text);
        }

        public long? DeserializeLong(string text)
        {
            text = DMD.Strings.Trim(text);
            if (string.IsNullOrEmpty(text))
                return default;
            return Sistema.Formats.ToLong(text);
        }

        public double? DeserializeDouble(object text)
        {
            return DeserializeFloat(text);
        }

        public double? DeserializeNumber(string text)
        {
            return DeserializeFloat(text);
        }

        public double? DeserializeFloat(object text)
        {
            return Sistema.Formats.ParseDouble(Strings.Replace(Strings.CStr(text), ".", ","));
        }

        private string ISOtoken(string token)
        {
            string ret = "";
            switch (token ?? "")
            {
                case "&quot;":
                case var @case when @case == "&quot;":
                    {
                        ret = DMD.Strings.CStr('"');
                        break;
                    }

                case "&lt;":
                    {
                        ret = "<";
                        break;
                    }

                case "&gt;":
                    {
                        ret = ">";
                        break;
                    }

                case "&amp;":
                    {
                        ret = "&";
                        break;
                    }

                case "&apos;":
                    {
                        ret = "'";
                        break;
                    }

                case "&#192;":
                    {
                        ret = "À";
                        break;
                    }

                case "&#193;":
                    {
                        ret = "Á";
                        break;
                    }

                case "&#194;":
                    {
                        ret = "Â";
                        break;
                    }

                case "&#195;":
                    {
                        ret = "Ã";
                        break;
                    }

                case "&#196;":
                    {
                        ret = "Ä";
                        break;
                    }

                case "&#197;":
                    {
                        ret = "Å";
                        break;
                    }

                case "&#198;":
                    {
                        ret = "Æ";
                        break;
                    }

                case "&#199;":
                    {
                        ret = "Ç";
                        break;
                    }

                case "&#200;":
                    {
                        ret = "È";
                        break;
                    }

                case "&#201;":
                    {
                        ret = "É";
                        break;
                    }

                case "&#202;":
                    {
                        ret = "Ê";
                        break;
                    }

                case "&#203;":
                    {
                        ret = "Ë";
                        break;
                    }

                case "&#204;":
                    {
                        ret = "Ì";
                        break;
                    }

                case "&#205;":
                    {
                        ret = "Í";
                        break;
                    }

                case "&#206;":
                    {
                        ret = "Î";
                        break;
                    }

                case "&#207;":
                    {
                        ret = "Ï";
                        break;
                    }

                case "&#208;":
                    {
                        ret = "Ð";
                        break;
                    }

                case "&#209;":
                    {
                        ret = "Ñ";
                        break;
                    }

                case "&#210;":
                    {
                        ret = "Ò";
                        break;
                    }

                case "&#211;":
                    {
                        ret = "Ó";
                        break;
                    }

                case "&#212;":
                    {
                        ret = "Ô";
                        break;
                    }

                case "&#213;":
                    {
                        ret = "Õ";
                        break;
                    }

                case "&#214;":
                    {
                        ret = "Ö";
                        break;
                    }

                case "&#215;":
                    {
                        ret = "×";
                        break;
                    }

                case "&#216;":
                    {
                        ret = "Ø";
                        break;
                    }

                case "&#217;":
                    {
                        ret = "Ù";
                        break;
                    }

                case "&#218;":
                    {
                        ret = "Ú";
                        break;
                    }

                case "&#219;":
                    {
                        ret = "Û";
                        break;
                    }

                case "&#220;":
                    {
                        ret = "Ü";
                        break;
                    }

                case "&#221;":
                    {
                        ret = "Ý";
                        break;
                    }

                case "&#222;":
                    {
                        ret = "Þ";
                        break;
                    }

                case "&#223;":
                    {
                        ret = "ß";
                        break;
                    }

                case "&#224;":
                    {
                        ret = "à";
                        break;
                    }

                case "&#225;":
                    {
                        ret = "á";
                        break;
                    }

                case "&#226;":
                    {
                        ret = "â";
                        break;
                    }

                case "&#227;":
                    {
                        ret = "ã";
                        break;
                    }

                case "&#228;":
                    {
                        ret = "ä";
                        break;
                    }

                case "&#229;":
                    {
                        ret = "å";
                        break;
                    }

                case "&#230;":
                    {
                        ret = "æ";
                        break;
                    }

                case "&#231;":
                    {
                        ret = "ç";
                        break;
                    }

                case "&#232;":
                    {
                        ret = "è";
                        break;
                    }

                case "&#233;":
                    {
                        ret = "é";
                        break;
                    }

                case "&#234;":
                    {
                        ret = "ê";
                        break;
                    }

                case "&#235;":
                    {
                        ret = "ë";
                        break;
                    }

                case "&#236;":
                    {
                        ret = "ì";
                        break;
                    }

                case "&#237;":
                    {
                        ret = "í";
                        break;
                    }

                case "&#238;":
                    {
                        ret = "î";
                        break;
                    }

                case "&#239;":
                    {
                        ret = "ï";
                        break;
                    }

                case "&#240;":
                    {
                        ret = "ð";
                        break;
                    }

                case "&#241;":
                    {
                        ret = "ñ";
                        break;
                    }

                case "&#242;":
                    {
                        ret = "ò";
                        break;
                    }

                case "&#243;":
                    {
                        ret = "ó";
                        break;
                    }

                case "&#244;":
                    {
                        ret = "ô";
                        break;
                    }

                case "&#245;":
                    {
                        ret = "õ";
                        break;
                    }

                case "&#246;":
                    {
                        ret = "ö";
                        break;
                    }

                case "&#247;":
                    {
                        ret = "÷";
                        break;
                    }

                case "&#248;":
                    {
                        ret = "ø";
                        break;
                    }

                case "&#249;":
                    {
                        ret = "ù";
                        break;
                    }

                case "&#250;":
                    {
                        ret = "ú";
                        break;
                    }

                case "&#251;":
                    {
                        ret = "û";
                        break;
                    }

                case "&#252;":
                    {
                        ret = "ü";
                        break;
                    }

                case "&#253;":
                    {
                        ret = "ý";
                        break;
                    }

                case "&#254;":
                    {
                        ret = "þ";
                        break;
                    }

                case "&#255;":
                    {
                        ret = "ÿ";
                        break;
                    }

                case "&#161;":
                    {
                        ret = "¡";
                        break;
                    }

                case "&#162;":
                    {
                        ret = "¢";
                        break;
                    }

                case "&#163;":
                    {
                        ret = "£";
                        break;
                    }

                case "&#164;":
                    {
                        ret = "¤";
                        break;
                    }

                case "&#165;":
                    {
                        ret = "¥";
                        break;
                    }

                case "&#128;":
                case "&euro;":
                    {
                        ret = "€";
                        break;
                    }

                case "&#166;":
                    {
                        ret = "¦";
                        break;
                    }

                case "&#167;":
                    {
                        ret = "§";
                        break;
                    }

                case "&#168;":
                    {
                        ret = "¨";
                        break;
                    }

                case "&#169;":
                    {
                        ret = "©";
                        break;
                    }

                case "&#170;":
                    {
                        ret = "ª";
                        break;
                    }

                case "&#171;":
                    {
                        ret = "«";
                        break;
                    }

                case "&#172;":
                    {
                        ret = "¬";
                        break;
                    }
                // case "¡"  "&#173;" 
                case "&#174;":
                    {
                        ret = "®";
                        break;
                    }

                case "&#175;":
                    {
                        ret = "¯";
                        break;
                    }

                case "&#176;":
                    {
                        ret = "°";
                        break;
                    }

                case "&#177;":
                    {
                        ret = "±";
                        break;
                    }

                case "&#178;":
                    {
                        ret = "²";
                        break;
                    }

                case "&#179;":
                    {
                        ret = "³";
                        break;
                    }

                case "&#180;":
                    {
                        ret = "´";
                        break;
                    }

                case "&#181;":
                    {
                        ret = "µ";
                        break;
                    }

                case "&#182;":
                    {
                        ret = "¶";
                        break;
                    }

                case "&#183;":
                    {
                        ret = "·";
                        break;
                    }

                case "&#184;":
                    {
                        ret = "¸";
                        break;
                    }

                case "&#185;":
                    {
                        ret = "¹";
                        break;
                    }

                case "&#186;":
                    {
                        ret = "º";
                        break;
                    }

                case "&#187;":
                    {
                        ret = "»";
                        break;
                    }

                case "&#188;":
                    {
                        ret = "¼";
                        break;
                    }

                case "&#189;":
                    {
                        ret = "½";
                        break;
                    }

                case "&#190;":
                    {
                        ret = "¾";
                        break;
                    }

                case "&#191;":
                    {
                        ret = "¿";
                        break;
                    }

                case "&#130;":
                    {
                        ret = "‚";
                        break;
                    }

                case "&#131;":
                    {
                        ret = "ƒ";
                        break;
                    }

                case "&#132;":
                    {
                        ret = "„";
                        break;
                    }

                case "&#133;":
                    {
                        ret = "…";
                        break;
                    }

                case "&#134;":
                    {
                        ret = "†";
                        break;
                    }

                case "&#135;":
                    {
                        ret = "‡";
                        break;
                    }

                case "&#136;":
                    {
                        ret = "ˆ";
                        break;
                    }

                case "&#137;":
                    {
                        ret = "‰";
                        break;
                    }

                case "&#138;":
                    {
                        ret = "Š";
                        break;
                    }

                case "&#139;":
                    {
                        ret = "‹";
                        break;
                    }

                case "&#140;":
                    {
                        ret = "Œ";
                        break;
                    }

                case "&#142;":
                    {
                        ret = "Ž";
                        break;
                    }

                case "&#145;":
                    {
                        ret = "‘";
                        break;
                    }

                case "&#146;":
                    {
                        ret = "’";
                        break;
                    }

                case "&#147;":
                    {
                        ret = DMD.Strings.CStr('"'); // "“"
                        break;
                    }

                case " &#148;":
                    {
                        ret = DMD.Strings.CStr('"'); // "”"
                        break;
                    }

                case "&#149;":
                    {
                        ret = "•";
                        break;
                    }

                case "&#150;":
                    {
                        ret = "–";
                        break;
                    }

                case "&#151;":
                    {
                        ret = "—";
                        break;
                    }

                case "&#152;":
                    {
                        ret = "˜";
                        break;
                    }

                case "&#153;":
                    {
                        ret = "™";
                        break;
                    }

                case "&#154;":
                    {
                        ret = "š";
                        break;
                    }

                case "&#155;":
                    {
                        ret = "›";
                        break;
                    }

                case "&#156;":
                    {
                        ret = "œ";
                        break;
                    }

                case "&#158;":
                    {
                        ret = "ž";
                        break;
                    }

                case "&#159;":
                    {
                        ret = "Ÿ";
                        break;
                    }

                default:
                    {
                        if (string.IsNullOrEmpty(token))
                        {
                            ret = "";
                        }
                        else if (token.StartsWith("&#") && token.EndsWith(";"))
                        {
                            token = DMD.Strings.Mid(token, 3, DMD.Strings.Len(token) - 3); // token.substring(2, token.length - 1);
                            ret = DMD.Strings.CStr((char)Integers.CInt(token)); // String.fromCharCode(parseInt(token));
                        }
                        else
                        {
                            ret = token;
                        }

                        break;
                    }
            }

            return ret;
        }

        public string DeserializeString(object text)
        {
            return DMD.Strings.HtmlDecode(Strings.CStr(text));
            // Dim value As String = DMD.Strings.CStr(text)
            // Dim ret As New System.Text.StringBuilder(value.Length)
            // Dim token As String = ""
            // Dim stato As Integer = 0
            // For i As Integer = 0 To value.Length - 1
            // Dim ch As Char = value.Chars(i)
            // Select Case (stato)
            // Case 0
            // If (ch = "&") Then
            // stato = 1
            // If (token <> "") Then ret.Append(token)
            // token = ""
            // End If
            // token &= ch
            // Case 1
            // If (ch = ";") Then
            // token &= ch
            // token = ISOtoken(token)
            // ret.Append(token)
            // token = ""
            // stato = 0
            // ElseIf (ch = "&") Then
            // ret.Append(token)
            // token = ""
            // stato = 1
            // token &= ch
            // Else
            // token &= ch
            // End If
            // End Select
            // Next

            // ret.Append(token)
            // Return ret.ToString

            // Return DMD.Strings.HTMLDecode(Strings.CStr(text))

            // 'Dim ret As String = DMD.Strings.HTMLDecode(text)
            // 'If (ret = vbNullString) Then ret = ""
            // 'Return ret ' text
            // Return DMD.Strings.URLDecode(text)
        }

        public string SerializeString(string text)
        {
            return DMD.Strings.HtmlEncode(text);
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        public string SerializeBoolean(object value)
        {
            if (value is bool? && ((bool?)value).HasValue == false)
                return "";
            if (value is null)
                return "";
            return DMD.Strings.HtmlEncode(Sistema.IIF(Booleans.CBool(value) == true, "True", "False"));
        }

        public string SerializeNumber(object value)
        {
            if (value is byte? && ((byte?)value).HasValue == false)
                return "";
            if (value is sbyte? && ((sbyte?)value).HasValue == false)
                return "";
            if (value is short? && ((short?)value).HasValue == false)
                return "";
            if (value is ushort? && ((ushort?)value).HasValue == false)
                return "";
            if (value is int? && ((int?)value).HasValue == false)
                return "";
            if (value is uint? && ((uint?)value).HasValue == false)
                return "";
            if (value is long? && ((long?)value).HasValue == false)
                return "";
            if (value is ulong? && ((ulong?)value).HasValue == false)
                return "";
            if (value is float? && ((float?)value).HasValue == false)
                return "";
            if (value is double? && ((double?)value).HasValue == false)
                return "";
            if (value is decimal? && ((decimal?)value).HasValue == false)
                return "";
            if (value is null)
                return "";
            return DMD.Strings.HtmlEncode(Sistema.Formats.USA.FormatDouble(value));
        }

        public string SerializeDouble(object value)
        {
            return SerializeNumber(value);
        }

        public string SerializeInteger(object value)
        {
            if (value is byte? && ((byte?)value).HasValue == false)
                return "";
            if (value is sbyte? && ((sbyte?)value).HasValue == false)
                return "";
            if (value is short? && ((short?)value).HasValue == false)
                return "";
            if (value is ushort? && ((ushort?)value).HasValue == false)
                return "";
            if (value is int? && ((int?)value).HasValue == false)
                return "";
            if (value is uint? && ((uint?)value).HasValue == false)
                return "";
            if (value is long? && ((long?)value).HasValue == false)
                return "";
            if (value is ulong? && ((ulong?)value).HasValue == false)
                return "";
            if (value is float? && ((float?)value).HasValue == false)
                return "";
            if (value is double? && ((double?)value).HasValue == false)
                return "";
            if (value is decimal? && ((decimal?)value).HasValue == false)
                return "";
            if (value is null)
                return "";
            return DMD.Strings.HtmlEncode(Longs.CLng(value).ToString());
        }

        public string SerializeDate(object value)
        {
            if (value is DateTime? && ((DateTime?)value).HasValue == false)
                return "";
            if (value is null)
                return "";
            DateTime d = DMD.DateUtils.CDate(value);
            return DMD.Strings.HtmlEncode(DMD.DateUtils.Year(d) + "/" + DMD.DateUtils.Month(d) + "/" + DMD.DateUtils.Day(d) + " " + DMD.DateUtils.Hour(d) + "." + DMD.DateUtils.Minute(d) + "." + DMD.DateUtils.Second(d));
        }

        public string SerializeArray<T>(T[] items)
        {
            return SerializeArray(items, "", new XMLWriter());
        }

        public string SerializeArray<T>(T[] items, string baseType)
        {
            return SerializeArray(items, baseType, new XMLWriter());
        }

        public virtual string SerializeArray<T>(T[] items, string baseType, XMLWriter writer)
        {
            int i;
            string ret = DMD.Strings.vbNullString;
            if (Arrays.UBound(items) >= 0)
            {
                if (string.IsNullOrEmpty(baseType))
                {
                    var loopTo = DMD.Arrays.Len(items) - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        baseType = Utils.XMLTypeName(items[i]);
                        ret += "<" + baseType + ">" + Utils.Serializer.Serialize(items[i], XMLSerializeMethod.None, writer) + "</" + baseType + ">";
                    }
                }
                else
                {
                    var loopTo1 = DMD.Arrays.Len(items) - 1;
                    for (i = 0; i <= loopTo1; i++)
                        ret += "<" + baseType + ">" + Utils.Serializer.Serialize(items[i], XMLSerializeMethod.None, writer) + "</" + baseType + ">";
                }
            }

            return ret;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public bool Read(ref bool target, object source)
        {
            return Sistema.Formats.ToBool(source);
        }

        public void Read(ref bool? target, object source)
        {
            target = Sistema.Formats.ParseBool(source);
        }

        public void Read(ref byte target, object source)
        {
            target = (byte)Sistema.Formats.ToInteger(source);
        }

        public void Read(ref byte? target, object source)
        {
            target = (byte?)Sistema.Formats.ParseInteger(source);
        }

        public void Read(ref sbyte target, object source)
        {
            target = (sbyte)Sistema.Formats.ToInteger(source);
        }

        public void Read(ref sbyte? target, object source)
        {
            target = (sbyte?)Sistema.Formats.ParseInteger(source);
        }

        public void Read(ref short target, object source)
        {
            target = (short)Sistema.Formats.ToInteger(source);
        }

        public void Read(ref short? target, object source)
        {
            target = (short?)Sistema.Formats.ParseInteger(source);
        }

        public void Read(ref int target, object source)
        {
            target = Sistema.Formats.ToInteger(source);
        }

        public void Read(ref int? target, object source)
        {
            target = Sistema.Formats.ParseInteger(source);
        }

        public void Read(ref long target, object source)
        {
            target = Sistema.Formats.ToInteger(source);
        }

        public void Read(ref long? target, object source)
        {
            target = Sistema.Formats.ParseInteger(source);
        }

        public void Read(ref float target, object source)
        {
            target = (float)Sistema.Formats.ToDouble(source);
        }

        public void Read(ref float? target, object source)
        {
            target = (float?)Sistema.Formats.ParseDouble(source);
        }

        public void Read(ref double target, object source)
        {
            target = Sistema.Formats.ToDouble(source);
        }

        public void Read(ref double? target, object source)
        {
            target = Sistema.Formats.ParseDouble(source);
        }

        public void Read(ref decimal target, object source)
        {
            target = (decimal)Sistema.Formats.ToDouble(source);
        }

        public void Read(ref decimal? target, object source)
        {
            target = (decimal?)Sistema.Formats.ParseDouble(source);
        }

        public void Read(ref DateTime target, object source)
        {
            target = Sistema.Formats.ToDate(source);
        }

        public void Read(ref DateTime? target, object source)
        {
            target = Sistema.Formats.ParseDate(source);
        }

        public void Read(ref string target, object source)
        {
            target = Sistema.Formats.ToString(source);
        }

        // Public  Sub Read(Of T)(ByRef target As T, ByVal source As Object)
        // target = Formats.ToInteger(source)
        // End Sub

        // Public  Sub Read(ByRef target As Nullable(Of [Enum]), ByVal source As Object)
        // target = Formats.ToInteger(source)
        // End Sub


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public virtual string SerializeObject(object obj)
        {
            if (obj is null)
                return DMD.Strings.vbNullString;
            return "<" + Utils.XMLTypeName(obj) + ">" + Serialize(obj, XMLSerializeMethod.None) + "</" + Utils.XMLTypeName(obj) + ">";
        }

        public virtual string SerializeObject(object obj, XMLWriter writer)
        {
            if (obj is null)
                return DMD.Strings.vbNullString;
            return "<" + Utils.XMLTypeName(obj) + ">" + Serialize(obj, XMLSerializeMethod.None, writer) + "</" + Utils.XMLTypeName(obj) + ">";
        }

        public virtual void DeserializeField(ref double field, object fieldValue)
        {
            field = (double)DeserializeDouble(Strings.CStr(fieldValue));
        }

        public virtual void DeserializeField(ref int field, object fieldValue)
        {
            field = (int)DeserializeInteger(fieldValue);
        }

        public virtual void DeserializeField(ref short field, object fieldValue)
        {
            field = (short)DeserializeInteger(fieldValue);
        }

        public virtual void DeserializeField(ref byte field, object fieldValue)
        {
            field = (byte)Utils.Serializer.DeserializeInteger(fieldValue);
        }

        public virtual void DeserializeField(ref float field, object fieldValue)
        {
            field = (float)Utils.Serializer.DeserializeDouble(Strings.CStr(fieldValue));
        }

        public virtual void DeserializeField(ref DateTime field, object fieldValue)
        {
            field = (DateTime)Utils.Serializer.DeserializeDate(fieldValue);
        }

        public virtual void DeserializeField(ref bool field, object fieldValue)
        {
            field = Utils.Serializer.DeserializeBoolean(fieldValue) == true;
        }

        public virtual void DeserializeField(ref string field, object fieldValue)
        {
            field = Utils.Serializer.DeserializeString(fieldValue);
        }

        public virtual void DeserializeField(ref double? field, object fieldValue)
        {
            field = Utils.Serializer.DeserializeDouble(Strings.CStr(fieldValue));
        }

        public virtual void DeserializeField(ref int? field, object fieldValue)
        {
            field = Utils.Serializer.DeserializeInteger(fieldValue);
        }

        public virtual void DeserializeField(ref short? field, object fieldValue)
        {
            field = (short?)Utils.Serializer.DeserializeInteger(fieldValue);
        }

        public virtual void DeserializeField(ref byte? field, object fieldValue)
        {
            field = (byte?)Utils.Serializer.DeserializeInteger(fieldValue);
        }

        public virtual void DeserializeField(ref float? field, object fieldValue)
        {
            field = (float?)Utils.Serializer.DeserializeDouble(Strings.CStr(fieldValue));
        }

        public virtual void DeserializeField(ref DateTime? field, object fieldValue)
        {
            field = Utils.Serializer.DeserializeDate(fieldValue);
        }

        public virtual void DeserializeField(ref Array field, object fieldValue)
        {
            field = (Array)fieldValue;
        }

        public object ToObject(object value)
        {
            if (value is string)
                return null;
            return value;
        }

        public Array ToArray<T>(object fieldValue)
        {
            return Sistema.Arrays.Convert<T>(fieldValue);
        }

        ~XMLSerializer()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}