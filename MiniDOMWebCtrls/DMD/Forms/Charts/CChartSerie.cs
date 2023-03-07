using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public sealed class Colors
    {
        public static System.Drawing.Color FromWeb(string value)
        {
            byte r, g, b;
            value = Strings.Right("000000" + Strings.Replace(value, " ", ""), 6);
            r = (byte)DMD.Integers.ValueOf("&H" + Strings.Left(value, 2));
            g = (byte)DMD.Integers.ValueOf("&H" + Strings.Mid(value, 3, 2));
            b = (byte)DMD.Integers.ValueOf("&H" + Strings.Right(value, 2));
            return System.Drawing.Color.FromArgb(255, r, g, b);
        }
    }

    [Serializable]
    public class CChartSerie : CChartElement
    {
        private string m_Name;
        private CChartValues m_Values;
        private object m_Tag;
        private bool m_ShowLabels;
        private System.Drawing.Color m_RenderColor;
        private ChartTypes m_Tipo;

        public CChartSerie()
        {
            m_Tipo = ChartTypes.Unset;
            m_Name = "";
            m_Values = null;
            m_ShowLabels = false;
            m_RenderColor = System.Drawing.Color.Empty;
            BackColor = System.Drawing.Color.Empty;
        }

        public ChartTypes Tipo
        {
            get
            {
                return m_Tipo;
            }

            set
            {
                var oldValue = m_Tipo;
                if (oldValue == value)
                    return;
                m_Tipo = value;
                // Me.DoChanged("Tipo", value, oldValue)
                InvalidateChart();
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
                m_Name = Strings.Trim(value);
            }
        }

        public CChartValues Values
        {
            get
            {
                if (m_Values is null)
                    m_Values = new CChartValues(this);
                return m_Values;
            }
        }

        public object Tag
        {
            get
            {
                return m_Tag;
            }

            set
            {
                m_Tag = value;
            }
        }

        /// <summary>
        /// Restituisce le misure dei valori della serie
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartMeasure GetMeasures()
        {
            return Values.GetMeasures();
        }

        public bool ShowLabels
        {
            get
            {
                return m_ShowLabels;
            }

            set
            {
                if (m_ShowLabels == value)
                    return;
                m_ShowLabels = value;
                InvalidateChart();
            }
        }

        private System.Drawing.Color GetRandomColor()
        {
            byte r, g, b;
            r = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
            g = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
            b = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
            return System.Drawing.Color.FromArgb(255, r, g, b);
        }

        public System.Drawing.Color GetRenderColor()
        {
            if (BackColor.Equals(System.Drawing.Color.Empty))
            {
                if (m_RenderColor.Equals(System.Drawing.Color.Empty))
                {
                    int i = Chart.Series.IndexOf(this);
                    if (i >= 0 && i <= DMD.Arrays.UBound(CChart.DefaultColors))
                    {
                        m_RenderColor = Colors.FromWeb(CChart.DefaultColors[i]);
                    }
                    else
                    {
                        m_RenderColor = GetRandomColor();
                    }
                }

                return m_RenderColor;
            }
            else
            {
                return BackColor;
            }
        }

        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Name", m_Name);
            writer.WriteAttribute("ShowLabels", m_ShowLabels);
            writer.WriteAttribute("RenderColor", Utils.FormsUtils.ToRGBSTR(m_RenderColor));
            writer.WriteAttribute("Tipo", (int?)m_Tipo);
            base.XMLSerialize(writer);
            writer.WriteTag("Values", Values);
            // m_Tag As Object
        }

        protected override void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Name":
                    {
                        m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "ShowLabels":
                    {
                        m_ShowLabels = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "RenderColor":
                    {
                        m_RenderColor = Utils.FormsUtils.FromRGBSTR(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                        break;
                    }

                case "Values":
                    {
                        m_Values = (CChartValues)fieldValue;
                        m_Values.SetSerie(this);
                        break;
                    }

                case "Tipo":
                    {
                        m_Tipo = (ChartTypes)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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