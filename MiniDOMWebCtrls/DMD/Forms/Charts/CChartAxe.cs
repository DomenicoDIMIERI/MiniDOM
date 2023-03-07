using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public class CChartAxe : CChartElement
    {
        private string m_Name;
        private bool m_DisplayValues;
        private bool m_ScalaLogaritmica;
        private int m_Decimals;
        private double m_LongStep;
        private double m_SmallStep;
        private double? m_MinValue;
        private double? m_MaxValue;
        private CSize m_Size = null;

        public CChartAxe()
        {
            m_Name = "";
            m_DisplayValues = true;
            m_ScalaLogaritmica = false;
            m_Decimals = 2;
            m_LongStep = 0d; // Auto
            m_SmallStep = 0d; // Auto
            m_MinValue = default;
            m_MaxValue = default;
            m_Size = null;
        }

        public CChartAxe(string name) : this()
        {
            m_Name = Strings.Trim(name);
        }

        public CChartAxe(CChart chart) : this()
        {
            SetChart(chart);
        }

        public CChartAxe(string name, CChart chart) : this()
        {
            m_Name = Strings.Trim(name);
            SetChart(chart);
        }

        public CSize Size
        {
            get
            {
                return m_Size;
            }

            set
            {
                m_Size = value;
                InvalidateChart();
            }
        }

        public double? MinValue
        {
            get
            {
                return m_MinValue;
            }

            set
            {
                if (m_MinValue == value == true)
                    return;
                m_MinValue = value;
                InvalidateChart();
            }
        }

        public double? MaxValue
        {
            get
            {
                return m_MaxValue;
            }

            set
            {
                if (m_MaxValue == value == true)
                    return;
                m_MaxValue = value;
                InvalidateChart();
            }
        }

        public double LongStep
        {
            get
            {
                return m_LongStep;
            }

            set
            {
                if (m_LongStep == value)
                    return;
                m_LongStep = value;
                InvalidateChart();
            }
        }

        public double SmallStep
        {
            get
            {
                return m_SmallStep;
            }

            set
            {
                if (m_SmallStep == value)
                    return;
                m_SmallStep = value;
                InvalidateChart();
            }
        }

        public int Decimals
        {
            get
            {
                return m_Decimals;
            }

            set
            {
                if (m_Decimals == value)
                    return;
                m_Decimals = value;
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

        public bool ScalaLogaritmica
        {
            get
            {
                return m_ScalaLogaritmica;
            }

            set
            {
                if (m_ScalaLogaritmica == value)
                    return;
                m_ScalaLogaritmica = value;
                InvalidateChart();
            }
        }

        public bool ShowValues
        {
            get
            {
                return m_DisplayValues;
            }

            set
            {
                if (m_DisplayValues == value)
                    return;
                m_DisplayValues = value;
                InvalidateChart();
            }
        }

        public float GetSmallStep(float min, float max)
        {
            if (SmallStep > 0d)
                return (float)SmallStep;
            return GetLargeStep(min, max) / 2f;
        }

        public float GetLargeStep(float min, float max)
        {
            if (LongStep > 0d)
                return (float)LongStep;
            int l = 1;
            if (Maths.Abs(max) > 0d)
                l = (int)Maths.Log10(Maths.Abs(max)); // - m.MinY)
            return (float)(Maths.Sign(max) * Maths.Pow(10d, l - 1));
        }

        public float GetLowestValue(float min, float max)
        {
            float s = GetLargeStep(min, max);
            if (max > 0f)
                return (float)Maths.Min(0d, Maths.Floor(max / s));
            return 0f;
        }

        public float GetUpperValue(float min, float max)
        {
            float s = GetLargeStep(min, max);
            if (max > 0f)
                return (float)Maths.Max(0d, Maths.Floor(max / s));
            return 0f;
        }

        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Name", m_Name);
            writer.WriteAttribute("DisplayValues", m_DisplayValues);
            writer.WriteAttribute("ScalaLogaritmica", m_ScalaLogaritmica);
            writer.WriteAttribute("Decimals", m_Decimals);
            writer.WriteAttribute("LongStep", m_LongStep);
            writer.WriteAttribute("SmallStep", m_SmallStep);
            writer.WriteAttribute("MinValue", m_MinValue);
            writer.WriteAttribute("MaxValue", m_MaxValue);
            base.XMLSerialize(writer);
            writer.WriteTag("Size", Size);
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

                case "DisplayValues":
                    {
                        m_DisplayValues = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "ScalaLogaritmica":
                    {
                        m_ScalaLogaritmica = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "Decimals":
                    {
                        m_Decimals = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "LongStep":
                    {
                        m_LongStep = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "SmallStep":
                    {
                        m_SmallStep = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "MinValue":
                    {
                        m_MinValue = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "MaxValue":
                    {
                        m_MaxValue = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Size":
                    {
                        m_Size = (CSize)fieldValue;
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