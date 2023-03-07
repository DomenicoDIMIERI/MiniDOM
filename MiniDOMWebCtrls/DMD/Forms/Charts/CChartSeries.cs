using System;

namespace minidom.Forms
{
    [Serializable]
    public class CChartSeries : CKeyCollection<CChartSerie>
    {
        [NonSerialized]
        private CChart m_Chart;

        public CChartSeries()
        {
        }

        public CChartSeries(CChart chart) : this()
        {
            m_Chart = chart;
        }

        public CChart Chart
        {
            get
            {
                return m_Chart;
            }
        }

        protected override void OnInsert(int index, object value)
        {
            if (m_Chart is object)
                ((CChartSerie)value).SetChart(m_Chart);
            base.OnInsert(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (m_Chart is object)
                ((CChartSerie)newValue).SetChart(m_Chart);
            base.OnSet(index, oldValue, newValue);
        }

        public CChartSerie Add(string serieName)
        {
            var item = new CChartSerie();
            item.Name = serieName;
            Add(serieName, item);
            return item;
        }

        public ChartMeasure GetMeasures()
        {
            ChartMeasure ret = default;
            if (Count > 0)
            {
                ret = this[0].GetMeasures();
                for (int i = 1, loopTo = Count - 1; i <= loopTo; i++)
                    ret = ret.Combine(this[i].GetMeasures());
            }

            return ret;
        }

        protected internal virtual void SetChart(CChart value)
        {
            m_Chart = value;
            foreach (CChartSerie s in this)
                s.SetChart(value);
        }
    }
}