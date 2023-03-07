using System;
using DMD;

namespace minidom.Forms
{
    [Serializable]
    public class CChartValues : CCollection<CChartValue>
    {
        [NonSerialized]
        private CChartSerie m_Serie;

        public CChartValues()
        {
        }

        public CChartValues(CChartSerie serie) : this()
        {
            SetSerie(serie);
        }

        protected internal void SetSerie(CChartSerie value)
        {
            m_Serie = value;
            foreach (CChartValue v in this)
                v.SetSerie(value);
        }

        protected override void OnInsert(int index, object value)
        {
            if (m_Serie is object)
                ((CChartValue)value).SetSerie(m_Serie);
            base.OnInsert(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (m_Serie is object)
                ((CChartValue)newValue).SetSerie(m_Serie);
            base.OnSet(index, oldValue, newValue);
        }

        public CChartValue Add(float? y, string label = DMD.Strings.vbNullString)
        {
            var item = new CChartValue();
            item.Label = label;
            item.Value = y;
            item.Color = Utils.FormsUtils.GetRandomColor();
            Add(item);
            return item;
        }

        public CChartValue Add(float? y, float x, string label)
        {
            var item = new CChartValue();
            item.Label = label;
            item.X = x;
            item.Value = y;
            item.Color = Utils.FormsUtils.GetRandomColor();
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
                    ret.MaxY = withBlock.Value;
                    ret.MinY = withBlock.Value;
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

                        ret.MinY = Maths.Min(withBlock1.Value, ret.MinY);
                        ret.MaxY = Maths.Max(withBlock1.Value, ret.MaxY);
                    }
                }
            }

            return ret;
        }
    }
}