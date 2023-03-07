using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    [Serializable]
    public class CChartValue 
        : IComparable, IDMDXMLSerializable
    {
        private string m_Label;
        private float m_X;
        private float? m_Value;
        private System.Drawing.Color m_Color;
        [NonSerialized]
        private CChartSerie m_Serie;
        private System.Drawing.Color m_RenderColor = System.Drawing.Color.Empty;

        public CChartValue()
        {
            DMDObject.IncreaseCounter(this);
            m_Label = "";
            m_X = 0f;
            m_Value = default;
            m_Color = System.Drawing.Color.Empty;
            m_Serie = null;
        }

        /// <summary>
        /// Restituisce il grafico a cui appartiene questo oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChart Chart
        {
            get
            {
                if (m_Serie is null)
                    return null;
                return m_Serie.Chart;
            }
        }

        /// <summary>
        /// Restituisce la serie a cui appartiene questo oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartSerie Serie
        {
            get
            {
                return m_Serie;
            }
        }

        protected internal void SetSerie(CChartSerie value)
        {
            m_Serie = value;
        }

        /// <summary>
        /// Restituisce il valore dell'etichetta visualizzata in corrispondenza di questo elemento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Label
        {
            get
            {
                return m_Label;
            }

            set
            {
                m_Label = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta il valore dell'elemento (ordinata)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public float? Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                if (value == m_Value == true)
                    return;
                m_Value = value;
                InvalidateChart();
            }
        }

        /// <summary>
        /// Restituisce o imposta la posizione dell'oggetto (ascissa)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public float X
        {
            get
            {
                return m_X;
            }

            set
            {
                if (m_X == value)
                    return;
                m_X = value;
                InvalidateChart();
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        private int CompareTo(CChartValue obj)
        {
            return DMD.Arrays.Compare(m_Value, obj.m_Value);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((CChartValue)obj);
        }

        protected void InvalidateChart()
        {
            if (Chart is object)
                Chart.Invalidate();
        }

        public System.Drawing.Color GetRenderColor()
        {
            if (Color.Equals(System.Drawing.Color.Empty))
            {
                if (m_RenderColor.Equals(System.Drawing.Color.Empty))
                {
                    int i = Serie.Values.IndexOf(this);
                    if (i >= 0 && i <= DMD.Arrays.UBound(CChart.DefaultColors))
                    {
                        m_RenderColor = Colors.FromWeb(CChart.DefaultColors[i]);
                    }
                    else
                    {
                        m_RenderColor = Utils.FormsUtils.GetRandomColor();
                    }
                }

                return m_RenderColor;
            }
            else
            {
                return Color;
            }
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Label":
                    {
                        m_Label = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "X":
                    {
                        m_X = (float)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Value":
                    {
                        m_Value = (float?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Color":
                    {
                        m_Color = Utils.FormsUtils.FromRGBSTR(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                        break;
                    }

                case "RenderColor":
                    this.m_RenderColor = Utils.FormsUtils.FromRGBSTR(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                    break;
                     
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Label", m_Label);
            writer.WriteAttribute("X", m_X);
            writer.WriteAttribute("Value", m_Value);
            writer.WriteAttribute("Color", Utils.FormsUtils.ToRGBSTR(m_Color));
            writer.WriteAttribute("RenderColor", Utils.FormsUtils.ToRGBSTR(m_RenderColor));
        }

        ~CChartValue()
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