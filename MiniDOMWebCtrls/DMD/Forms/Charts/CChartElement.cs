using System;
using DMD.XML;

namespace minidom.Forms
{
    public enum ChartElementPosition
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    [Serializable]
    public abstract class CChartElement 
        : IDMDXMLSerializable
    {
        [NonSerialized]
        private CChart m_Chart;
        private string m_Text;
        private System.Drawing.Color m_ForeColor;
        private System.Drawing.Color m_BackColor;
        private ChartElementPosition m_Position;
        private bool m_Visible;
        private float m_BorderSize;
        private System.Drawing.Color m_BorderColor;
        private CFont m_Font;

        public CChartElement()
        {
            DMDObject.IncreaseCounter(this);
            m_Chart = null;
            m_Text = "";
            m_ForeColor = System.Drawing.Color.Black;
            m_BackColor = System.Drawing.Color.White;
            m_Position = ChartElementPosition.Right;
            m_Visible = true;
            m_BorderSize = 1f;
            m_BorderColor = System.Drawing.Color.Gray;
            m_Font = null;
        }

        public CChartElement(CChart chart) : this()
        {
            SetChart(chart);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }


        public CChart Chart
        {
            get
            {
                return m_Chart;
            }
        }

        protected internal virtual void SetChart(CChart value)
        {
            m_Chart = value;
        }

        public string Text
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

        public CFont Font
        {
            get
            {
                return m_Font;
            }

            set
            {
                if (ReferenceEquals(m_Font, value))
                    return;
                m_Font = value;
                InvalidateChart();
            }
        }

        public System.Drawing.Color ForeColor
        {
            get
            {
                return m_ForeColor;
            }

            set
            {
                m_ForeColor = value;
            }
        }

        public System.Drawing.Color BackColor
        {
            get
            {
                return m_BackColor;
            }

            set
            {
                m_BackColor = value;
            }
        }

        public System.Drawing.Color BorderColor
        {
            get
            {
                return m_BorderColor;
            }

            set
            {
                m_BorderColor = value;
            }
        }

        public float BorderSize
        {
            get
            {
                return m_BorderSize;
            }

            set
            {
                m_BorderSize = value;
            }
        }

        public ChartElementPosition Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
            }
        }

        protected void InvalidateChart()
        {
            Chart.Invalidate();
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Text":
                    {
                        m_Text = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "ForeColor":
                    {
                        m_ForeColor = Utils.FormsUtils.FromRGBSTR(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                        break;
                    }

                case "BackColor":
                    {
                        m_BackColor = Utils.FormsUtils.FromRGBSTR(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                        break;
                    }

                case "Position":
                    {
                        m_Position = (ChartElementPosition)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Visible":
                    {
                        m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "BorderSize":
                    {
                        m_BorderSize = (float)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "BorderColor":
                    {
                        m_BorderColor = Utils.FormsUtils.FromRGBSTR(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                        break;
                    }

                case "Font":
                    {
                        m_Font = (CFont)fieldValue;
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Text", m_Text);
            writer.WriteAttribute("ForeColor", Utils.FormsUtils.ToRGBSTR(m_ForeColor));
            writer.WriteAttribute("BackColor", Utils.FormsUtils.ToRGBSTR(m_BackColor));
            writer.WriteAttribute("Position", (int?)Position);
            writer.WriteAttribute("Visible", m_Visible);
            writer.WriteAttribute("BorderSize", m_BorderSize);
            writer.WriteAttribute("BorderColor", Utils.FormsUtils.ToRGBSTR(m_BorderColor));
            writer.WriteTag("Font", m_Font);
        }

        ~CChartElement()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}