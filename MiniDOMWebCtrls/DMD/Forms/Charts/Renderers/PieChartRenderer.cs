using System.Collections;
using DMD;

namespace minidom.Forms
{
    public class PieChartRenderer : ChartRenderer, IComparer
    {
        public PieChartRenderer(CChart chart) : base(chart)
        {
        }

        protected override void DrawXAxe()
        {
            // MyBase.DrawXAxe()
        }

        protected override void DrawYAxe()
        {
            // MyBase.DrawYAxe()
        }

        protected override void DrawGrid()
        {
            // MyBase.DrawGrid()
        }

        public override string Description
        {
            get
            {
                return "Torta";
            }
        }

        protected override void CalculateParameters()
        {
            minX = renderRect.Left;
            maxX = renderRect.Right;
            minY = renderRect.Top;
            maxY = renderRect.Bottom;
            xOrig = (float)(renderRect.Width / 2d);
            yOrig = (float)(renderRect.Height / 2d);
            xScale = 1f;
            yScale = 1f;
        }

        protected override void DrawSerie(CChartSerie serie)
        {
            if (serie.Values.Count == 0)
                return;
            CChartValue[] values;
            values = serie.Values.ToArray();
            DMD.Arrays.Sort(values, 0, DMD.Arrays.Len(values), this);
            double sum = 0d;
            for (int i = 0, loopTo = DMD.Arrays.UBound(values); i <= loopTo; i++)
                sum = (double)(sum + values[i].Value);
            float x, y, w, h;
            x = xOrig;
            y = yOrig;
            w = (float)Maths.Min(renderRect.Width / 2d, renderRect.Height / 2d);
            h = w;
            double currSum = 0d;
            double startAngle = 0d;
            double sweepAngle;
            for (int i = 0, loopTo1 = DMD.Arrays.UBound(values); i <= loopTo1; i++) // To 0 Step -1
            {
                startAngle = currSum / sum * 360d;
                sweepAngle = (double)(values[i].Value / sum * 360);
                var color = values[i].GetRenderColor();
                var brush = new System.Drawing.SolidBrush(color);
                FillArc(brush, xOrig, yOrig, w, h, (float)startAngle, (float)sweepAngle);
                brush.Dispose();
                DrawArc(System.Drawing.Pens.Black, xOrig, yOrig, w, h, (float)startAngle, (float)sweepAngle);
                currSum = (double)(currSum + values[i].Value);
            }
        }

        public override ChartTypes Type
        {
            get
            {
                return ChartTypes.Pie;
            }
        }

        int IComparer.Compare(object x, object y)
        {
            CChartValue a = (CChartValue)x;
            CChartValue b = (CChartValue)y;
            return (int)(b.Value - a.Value);
        }
    }
}