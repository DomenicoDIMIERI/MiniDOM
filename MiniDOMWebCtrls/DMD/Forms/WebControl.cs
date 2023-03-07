using System;
using System.Diagnostics;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    [Serializable]
    public class WebControl 
        : IDMDXMLSerializable
    {
        private static int WebControls_m_Curr = 0;
        [NonSerialized]
        private WebControl m_Parent;
        private bool m_AutoScroll;
        private bool m_AutoGrow;
        private DockType m_Dock;
        private string m_Text;
        private CWebControlAttributes m_Attributes;
        private CWebControlsCollection m_Controls;
        private bool m_Enabled;
        private bool m_Visible;
        private bool m_LayoutSuspended;
        private bool m_ShouldLayout;
        private bool m_CanRaiseEvents;
        private CRectangle m_Bounds;
        private bool domInit;
        private WebControlStyle m_Style;
        private CFont m_Font;
        private TextAlignEnum m_TextAlign;
        private bool __FSEINIT;
        private string _tooltip;
        private bool m_IsUpdating;
        private object m_Tag;
        private bool m_IsRefresh;
        private string m_Name;

        public WebControl()
        {
            DMDObject.IncreaseCounter(this);
            m_Parent = null;
            m_AutoScroll = false;
            m_AutoGrow = false;
            m_Dock = DockType.DOCK_NONE;
            m_Text = "";
            m_Attributes = null;
            m_Controls = null;
            m_Enabled = true;
            m_Visible = true;
            m_LayoutSuspended = false;
            m_ShouldLayout = true;
            m_CanRaiseEvents = true;
            m_Bounds = null;
            domInit = false;
            m_Style = new WebControlStyle(this);
            m_Style.Border = new WebControlBorder("none"); // 1px solid #e0e0e0");
            m_Font = null;
            m_TextAlign = TextAlignEnum.TEXTALIGN_DEFAULT;
            __FSEINIT = false;
            _tooltip = "";
            m_Name = "";
            m_IsRefresh = false;
            // Me.m_Style.Border = "solid 1px #e0e0e0"
            Enabled = true;
        }

        public WebControl(string elemName) : this()
        {
            m_Name = Strings.Trim(elemName);
        }

        public WebControlStyle Style
        {
            get
            {
                return m_Style;
            }

            set
            {
                m_Style = value;
            }
        }

        public CRectangle Bounds
        {
            get
            {
                if (m_Bounds is null)
                {
                    var b = GetPreferredSize();
                    return new CRectangle(0d, 0d, b.Width, b.Height);
                }
                else
                {
                    return new CRectangle(m_Bounds);
                }
            }

            set
            {
                // var b = (arguments.length == 1) ? arguments[0] : new CRectangle(Formats.ToInteger(arguments[0]), Formats.ToInteger(arguments[1]), Formats.ToInteger(arguments[2]), Formats.ToInteger(arguments[3]));
                if (value.Equals(Bounds))
                    return;
                SetBounds(value);
                var e = new EventArgs();
                // var e = new Object();
                // e.Size = this.getSize();
                // e.Location = this.getLocation();
                OnLocationChanged(e);
                OnResize(e);
            }
        }

        protected virtual void OnLocationChanged(EventArgs e)
        {
        }

        protected virtual void OnResize(EventArgs e)
        {
        }

        protected internal virtual void SetBounds(CRectangle b)
        {
            m_Bounds = b;
            // Me._updateInterface()
        }

        // Public Function GetParameter1(ByVal paramName As String) As String
        // Return Sistema.ApplicationContext.GetParameter(paramName)
        // End Function

        public T? GetParameter<T>(string paramName, object defValue = null) where T : struct
        {
            var tp = typeof(T);
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            if (string.IsNullOrEmpty(param))
                return (T?)defValue;
            object ret = null;
            if (ReferenceEquals(tp, typeof(bool)))
                ret = Sistema.Formats.ParseBool(param);
            if (ReferenceEquals(tp, typeof(byte)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(sbyte)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(short)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(ushort)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(int)) || tp.IsEnum)
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(uint)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(long)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(ulong)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(float)))
                ret = Sistema.Formats.ParseInteger(param);
            if (ReferenceEquals(tp, typeof(double)))
                ret = Sistema.Formats.ParseDouble(param);
            if (ReferenceEquals(tp, typeof(decimal)))
                ret = Sistema.Formats.ParseValuta(param);
            if (ReferenceEquals(tp, typeof(DateTime)))
                ret = Sistema.Formats.ParseDate(param);
            if (ReferenceEquals(tp, typeof(char)))
                ret = Sistema.Formats.ToString(param);
            if (ReferenceEquals(tp, typeof(string)))
                ret = Sistema.Formats.ToString(param);
            Debug.Assert(ret is object);
            return (T)ret;
        }

        public bool IsParameterSet(string paramName)
        {
            return !string.IsNullOrEmpty(GetParameter(paramName, DMD.Strings.vbNullString));
        }

        public int GetParameter(string paramName, int defValue)
        {
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            var ret = Sistema.Formats.ParseInteger(param);
            if (ret.HasValue)
                return (int)ret;
            return defValue;
        }

        public bool GetParameter(string paramName, bool defValue)
        {
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            var ret = Sistema.Formats.ParseBool(param);
            if (ret.HasValue)
                return ret == true;
            return defValue;
        }

        public double GetParameter(string paramName, double defValue)
        {
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            var ret = Sistema.Formats.ParseDouble(param);
            if (ret.HasValue)
                return (double)ret;
            return defValue;
        }

        public decimal GetParameter(string paramName, decimal defValue)
        {
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            var ret = Sistema.Formats.ParseValuta(param);
            if (ret.HasValue)
                return (decimal)ret;
            return defValue;
        }

        public DateTime GetParameter(string paramName, DateTime defValue)
        {
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            var ret = Sistema.Formats.ParseDate(param);
            if (ret.HasValue)
                return (DateTime)ret;
            return defValue;
        }

        public string GetParameter(string paramName, string defValue)
        {
            string param = Sistema.ApplicationContext.GetParameter(paramName);
            string ret = Sistema.Formats.ToString(param);
            if (string.IsNullOrEmpty(ret))
                return defValue;
            return ret;
        }

        /// <summary>
        /// Organizza i controlli in base al loro layout
        /// </summary>
        /// <remarks></remarks>
        public virtual void DoLayout()
        {
            m_ShouldLayout = false;
            // Me.ParsePostbackData()

            foreach (WebControl ctrl in Controls)
                // ctrl.ParsePostbackData()
                ctrl.DoLayout();
        }

        protected static string WebControl_NewName()
        {
            WebControls_m_Curr += 1;
            string tmp = "DMDCTRL";
            int p = Strings.InStr(tmp, "(");
            if (p > 0)
            {
                tmp = DMD.Strings.Left(tmp, p - 1);
            }

            return tmp + WebControls_m_Curr;
        }

        public bool Visible
        {
            get
            {
                return m_Visible;
            }

            set
            {
                m_Visible = value;
            }
        }

        public CWebControlsCollection Controls
        {
            get
            {
                if (m_Controls is null)
                {
                    m_Controls = new CWebControlsCollection();
                    m_Controls.SetOwner(this);
                }

                return m_Controls;
            }
        }

        public virtual string Text
        {
            get
            {
                return m_Text;
            }

            set
            {
                m_Text = DMD.WebUtils.HtmlEncode(value);
            }
        }

        public virtual string InnerHTML
        {
            get
            {
                return m_Text;
            }

            set
            {
                m_Text = value;
            }
        }

        public CSize GetPreferredSize()
        {
            return new CSize(100d, 100d);
        }

        public bool AutoScroll
        {
            get
            {
                return m_AutoScroll;
            }

            set
            {
                m_AutoScroll = value;
            }
        }

        public bool AutoGrow
        {
            get
            {
                return m_AutoGrow;
            }

            set
            {
                m_AutoGrow = value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = Utils.FormsUtils.GetValidControlName(value);
                OnNameChanged(new EventArgs());
            }
        }

        protected virtual void OnNameChanged(EventArgs e)
        {
            SetAttribute("name", Name);
            SetAttribute("id", Name);
        }

        public string Left
        {
            get
            {
                return Bounds.Left.ToString();
            }

            set
            {
                var b = Bounds;
                b.Left = DMD.Doubles.CDbl(value);
                Bounds = b;
            }
        }

        public string Top
        {
            get
            {
                return Bounds.Top.ToString();
            }

            set
            {
                var b = Bounds;
                b.Top = DMD.Doubles.CDbl(value);
                Bounds = b;
            }
        }

        public virtual string Width
        {
            get
            {
                return Bounds.Width.ToString();
            }

            set
            {
                var b = Bounds;
                b.Width = DMD.Doubles.CDbl(value);
                Bounds = b;
            }
        }

        public virtual string Height
        {
            get
            {
                return Bounds.Height.ToString();
            }

            set
            {
                var b = Bounds;
                b.Height = DMD.Doubles.CDbl(value);
                Bounds = b;
            }
        }

        public DockType Dock
        {
            get
            {
                return m_Dock;
            }

            set
            {
                m_Dock = value;
            }
        }

        public string BackColor
        {
            get
            {
                return Style.Background.Color;
            }

            set
            {
                Style.Background.Color = Strings.Trim(value);
            }
        }

        public string TextColor
        {
            get
            {
                return Style.Color;
            }

            set
            {
                Style.Color = Strings.Trim(value);
            }
        }

        public WebControl Parent
        {
            get
            {
                return m_Parent;
            }
        }

        protected internal virtual void SetParent(WebControl value)
        {
            m_Parent = value;
        }

        public CWebControlAttributes Attributes
        {
            get
            {
                if (m_Attributes is null)
                    m_Attributes = new CWebControlAttributes();
                return m_Attributes;
            }
        }

        public bool Enabled
        {
            get
            {
                return m_Enabled;
            }

            set
            {
                m_Enabled = value;
            }
        }

        public string OnClick
        {
            get
            {
                return Attributes["onclick"];
            }

            set
            {
                Attributes["onclick"] = Strings.Trim(value);
            }
        }

        public string OnChange
        {
            get
            {
                return Attributes["onchange"];
            }

            set
            {
                Attributes["onchange"] = Strings.Trim(value);
            }
        }

        public string OnKeyPress
        {
            get
            {
                return GetAttribute("onkeypress");
            }

            set
            {
                SetAttribute("onkeypress", Strings.Trim(value));
            }
        }

        public void SetAttribute(string name, string value)
        {
            Attributes.SetItemByKey(name, value);
        }

        public string GetAttribute(string name)
        {
            return Attributes.GetItemByKey(name);
        }

        public void RemoveAttribute(string name)
        {
            if (Attributes.ContainsKey(name))
                Attributes.RemoveByKey(name);
        }

        public virtual void GetAttributesHTML(System.Web.UI.HtmlTextWriter writer)
        {
            if (Dock != DockType.DOCK_NONE)
                SetAttribute("frm_dock", ((int)Dock).ToString());
            if (!string.IsNullOrEmpty(Name))
            {
                SetAttribute("name", Name);
                SetAttribute("id", Name);
            }
            // Stile
            string style = GetStyleHTML();
            if (!string.IsNullOrEmpty(style))
            {
                SetAttribute("style", style);
            }
            else
            {
                RemoveAttribute("style");
            }

            foreach (var key in Attributes.Keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    writer.Write(" ");
                    writer.Write(key);
                    string value = Attributes[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        writer.Write("=");
                        writer.Write('"');
                        writer.Write(value);
                        writer.Write('"');
                    }
                }
            }
        }

        public virtual string GetStyleHTML()
        {
            string html = m_Style.ToString();
            if (AutoScroll == true)
            {
                Style.Overflow = "auto";
            }
            else
            {
                Style.Overflow = "hidden";
            }

            Style.WhiteSpace = "nowrap";
            switch (Dock)
            {
                case DockType.DOCK_LEFT:
                    {
                        Style.Left = 0.ToString();
                        Style.Height = "100%";
                        Style.Width = Width;
                        break;
                    }

                case DockType.DOCK_TOP:
                    {
                        Style.Top = 0.ToString();
                        Style.Width = "100%";
                        Style.Height = Height;
                        break;
                    }

                case DockType.DOCK_RIGHT:
                    {
                        Style.Right = 0.ToString();
                        Style.Height = "100%";
                        Style.Width = Width;
                        break;
                    }

                case DockType.DOCK_BOTTOM:
                    {
                        Style.Bottom = 0.ToString();
                        Style.Width = "100%";
                        Style.Height = Height;
                        break;
                    }

                case DockType.DOCK_FILL:
                    {
                        Style.Left = 0.ToString();
                        Style.Top = 0.ToString();
                        Style.Width = "100%";
                        Style.Height = "100%";
                        break;
                    }

                default:
                    {
                        // Case DOCK_NONE
                        if (m_AutoGrow)
                        {
                            Style.MinWidth = Width + "px";
                            Style.MinHeight = Height + "px";
                        }
                        else
                        {
                            Style.Width = Width;
                            Style.Height = Height;
                        }

                        break;
                    }
            }
            // If m_BackColor <> "" Then html = Strings.Combine(html, "background-color:" & m_BackColor, ";")
            if (string.IsNullOrEmpty(Style.Display) && Visible == false)
                Style.Display = "none";
            return Style.ToString();
        }

        public virtual void GetInnerHTML(System.Web.UI.HtmlTextWriter writer)
        {
            int i;
            var loopTo = Controls.Count - 1;
            for (i = 0; i <= loopTo; i++)
                Controls[i].CreateHTML(writer);
            writer.Write(Text);
        }

        public virtual string GetElementName()
        {
            return "div";
        }

        // Public Overridable Function CreateHTML() As String
        // Dim w As New System.Text.StringBuilder
        // Dim writer As New System.Web.UI.HtmlTextWriter(w)
        // Me.CreateHTML(writer)
        // Dim ret As String = writer.ToString
        // 'writer.Dispose()
        // Return ret
        // End Function

        public virtual void CreateHTML(System.Web.UI.HtmlTextWriter writer)
        {
            if (m_ShouldLayout)
                DoLayout();
            string elemName = GetElementName();
            writer.Write("<");
            writer.Write(elemName);
            GetAttributesHTML(writer);
            writer.Write(">");
            // writer.CheckInTag()
            // If (Not writer.CanCloseTag(Me.GetElementName)) Then
            GetInnerHTML(writer);
            // End If
            // writer.EndTag()
            writer.Write("</");
            writer.Write(elemName);
            writer.Write(">");

            // Attributi


            if (m_Parent is null)
            {
                writer.Write("<script type=\"text/javascript\" language=\"javascript\">" + DMD.Strings.vbNewLine);
                GetScriptHTML(writer);
                writer.Write("</script>");
            }
        }

        public virtual void GetScriptHTML(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("var _" + Name + " = new " + DMD.RunTime.vbTypeName(this) + "(\"" + Name + "\");");
            foreach (WebControl c in Controls)
                c.GetScriptHTML(writer);
        }

        public bool IsRefresh()
        {
            return m_IsRefresh;
        }

        // Public Sub Refresh()
        // Me.m_IsRefresh = True
        // Me.WriteHTML()
        // Me.m_IsRefresh = False
        // End Sub

        /// <summary>
        /// Chiamata in fase di inizializzazione per ricostruire lo stato dell'oggetto
        /// </summary>
        /// <remarks></remarks>
        public virtual void ParsePostbackData()
        {
        }

        public CFont Font
        {
            get
            {
                return m_Font;
            }

            set
            {
                m_Font = value;
            }
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Name":
                    {
                        m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "AutoScroll":
                    {
                        m_AutoScroll = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "AutoGrow":
                    {
                        m_AutoGrow = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "Dock":
                    {
                        m_Dock = (DockType)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Enabled":
                    {
                        m_Enabled = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "Visible":
                    {
                        m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "LayoutSuspended":
                    {
                        m_LayoutSuspended = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "ShouldLayout":
                    {
                        m_ShouldLayout = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "CanRaiseEvents":
                    {
                        m_CanRaiseEvents = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "Bounds":
                    {
                        m_Bounds = (CRectangle)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }

                case "Text":
                    {
                        m_Text = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Attributes":
                    {
                        m_Attributes = (CWebControlAttributes)fieldValue;
                        m_Attributes.SetOwner(this);
                        break;
                    }

                case "Style":
                    {
                        m_Style = (WebControlStyle)fieldValue;
                        m_Style.SetOwner(this);
                        break;
                    }

                case "Controls":
                    {
                        m_Controls = (CWebControlsCollection)fieldValue;
                        m_Controls.SetOwner(this);
                        break;
                    }

                case "Font":
                    {
                        m_Font = (CFont)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Name", m_Name);
            writer.WriteAttribute("AutoScroll", m_AutoScroll);
            writer.WriteAttribute("AutoGrow", m_AutoGrow);
            writer.WriteAttribute("Dock", (int?)m_Dock);
            writer.WriteAttribute("Enabled", m_Enabled);
            writer.WriteAttribute("Visible", m_Visible);
            writer.WriteAttribute("LayoutSuspended", m_LayoutSuspended);
            writer.WriteAttribute("ShouldLayout", m_ShouldLayout);
            writer.WriteAttribute("CanRaiseEvents", m_CanRaiseEvents);
            writer.WriteAttribute("TextAlign", (int?)m_TextAlign);
            writer.WriteTag("Bounds", m_Bounds);
            writer.WriteTag("Text", m_Text);
            writer.WriteTag("Attributes", Attributes);
            writer.WriteTag("Style", Style);
            writer.WriteTag("Controls", Controls);
            writer.WriteTag("Font", Font);
        }

        ~WebControl()
        {
            DMDObject.DecreaseCounter(this);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

    }
}