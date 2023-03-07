using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CChartGrid 
        : CChartElement
    {
        private bool m_ShowSecondaryGrid = false;
        private bool m_ShowXGrid = true;
        private bool m_ShowYGrid = true;

        public CChartGrid()
        {
        }

        public CChartGrid(CChart chart) : this()
        {
            SetChart(chart);
        }

        public bool ShowXGrid
        {
            get
            {
                return m_ShowXGrid;
            }

            set
            {
                if (m_ShowXGrid == value)
                    return;
                m_ShowXGrid = value;
                InvalidateChart();
            }
        }

        public bool ShowYGrid
        {
            get
            {
                return m_ShowYGrid;
            }

            set
            {
                if (m_ShowYGrid == value)
                    return;
                m_ShowYGrid = value;
                InvalidateChart();
            }
        }

        public bool ShowSecondaryGrid
        {
            get
            {
                return m_ShowSecondaryGrid;
            }

            set
            {
                if (m_ShowSecondaryGrid == value)
                    return;
                m_ShowSecondaryGrid = value;
                InvalidateChart();
            }
        }

        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("ShowXGrid", m_ShowXGrid);
            writer.WriteAttribute("ShowYGrid", m_ShowYGrid);
            writer.WriteAttribute("ShowSecondaryGrid", m_ShowSecondaryGrid);
            base.XMLSerialize(writer);
        }

        protected override void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "ShowXGrid":
                    {
                        m_ShowXGrid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "ShowYGrid":
                    {
                        m_ShowYGrid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }

                case "ShowSecondaryGrid":
                    {
                        m_ShowSecondaryGrid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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