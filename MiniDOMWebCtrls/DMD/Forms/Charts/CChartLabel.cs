using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    [Serializable]
    public class CChartLabel 
        : IComparable, IDMDXMLSerializable
    {
        [NonSerialized]
        private CChart m_Char;
        private string m_Label;
        private float m_X;
        private string m_Color;

        public CChartLabel()
        {
            DMDObject.IncreaseCounter(this);
            m_Label = "";
            m_X = 0f;
            m_Color = "";
            m_Char = null;
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
                return m_Char;
            }
        }

        protected internal void SetChart(CChart value)
        {
            m_Char = value;
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

        public string Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = Strings.Trim(value);
            }
        }

        private int CompareTo(CChartLabel obj)
        {
            if (m_X < obj.m_X)
                return -1;
            if (m_X > obj.m_X)
                return 1;
            return 0;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((CChartLabel)obj);
        }

        protected void InvalidateChart()
        {
            if (Chart is object)
                Chart.Invalidate();
        }

        protected internal virtual void SetFieldInternal(string fieldName, object fieldValue)
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

                case "Color":
                    {
                        m_Color = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        protected internal virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Label", m_Label);
            writer.WriteAttribute("X", m_X);
            writer.WriteAttribute("Color", m_Color);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

        ~CChartLabel()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}