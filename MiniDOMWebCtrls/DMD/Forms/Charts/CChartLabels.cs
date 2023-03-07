using System;

namespace minidom.Forms
{
    [Serializable]
    public class CChartLabels : CCollection<CChartLabel>
    {
        [NonSerialized]
        private CChart m_Chart;

        public CChartLabels()
        {
        }

        public CChartLabels(CChart chart) : this()
        {
            SetChart(chart);
        }

        protected internal void SetChart(CChart value)
        {
            m_Chart = value;
            foreach (CChartLabel l in this)
                l.SetChart(value);
        }

        protected override void OnInsert(int index, object value)
        {
            if (m_Chart is object)
                ((CChartLabel)value).SetChart(m_Chart);
            base.OnInsert(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (m_Chart is object)
                ((CChartLabel)newValue).SetChart(m_Chart);
            base.OnSet(index, oldValue, newValue);
        }

        public CChartLabel Add(string label)
        {
            var item = new CChartLabel();
            item.Label = label;
            item.X = Count;
            item.Color = Utils.FormsUtils.GenerateRandomColor();
            Add(item);
            return item;
        }

        public CChartLabel Add(string label, float x)
        {
            var item = new CChartLabel();
            item.Label = label;
            item.X = x;
            item.Color = Utils.FormsUtils.GenerateRandomColor();
            Add(item);
            return item;
        }

        // Public Sub GetMinMax(ByRef min As Single, ByRef max As Single)
        // min = 0 : max = 0
        // If Me.Count = 0 Then Exit Sub
        // min = Me(0).Value : max = min
        // For Each Item As CChartValue In Me
        // If Item.Value > max Then
        // max = Item.Value
        // ElseIf Item.Value < min Then
        // min = Item.Value
        // End If
        // Next
        // End Sub

        public ChartMeasure GetMeasures()
        {
            var ret = new ChartMeasure();
            ret.MaxCount = Count;
            ret.MinCount = Count;
            if (Count > 0)
            {
                {
                    var withBlock = this[0];
                    ret.MaxX = withBlock.X;
                    ret.MinX = withBlock.X;
                }

                for (int i = 1, loopTo = Count - 1; i <= loopTo; i++)
                {
                    {
                        var withBlock1 = this[i];
                        if (withBlock1.X < ret.MinX)
                        {
                            ret.MinX = withBlock1.X;
                        }
                        else if (withBlock1.X > ret.MaxX)
                        {
                            ret.MaxX = withBlock1.X;
                        }
                    }
                }
            }

            return ret;
        }
    }
}