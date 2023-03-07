
namespace minidom.Forms
{
    public class VerticalBarChartMultibarsRenderer : VerticalBarOverlappedChartRenderer
    {
        public float GroupDistance = 5f;
        private float GroupWidth;
        private int maxNumXSerie = 0;

        public VerticalBarChartMultibarsRenderer(CChart chart) : base(chart)
        {
        }

        protected override void CalculateParameters()
        {
            bool init = false;
            foreach (CChartSerie serie in Chart.Series)
            {
                var ms = serie.GetMeasures();
                if (init == false)
                {
                    minY = ms.MinY;
                    maxY = ms.MaxY;
                    init = true;
                }
                else
                {
                    if (ms.MinY < minY == true)
                        minY = ms.MinY;
                    if (ms.MaxY > maxY == true)
                        maxY = ms.MaxY;
                }

                maxNumXSerie = Maths.Max(serie.Values.Count, maxNumXSerie);
            }

            maxNumXSerie = Maths.Max(1, maxNumXSerie);
            float W = renderRect.Width;
            int N = maxNumXSerie;
            float GS = GroupDistance;
            GroupWidth = (W + GS) / N - GS;
            m_BarWidth = (GroupWidth + barDistance) / Maths.Max(1, Chart.Series.Count) - barDistance;

            // Me.Chart.Labels.GetMinMax(Me.minX, Me.maxX)
            minX = 0f;
            maxX = GroupWidth * Chart.Labels.Count + (GroupDistance * 2f * Chart.Labels.Count - 1f);
            xScale = 1f;
            yScale = 1f;
            if (minY * maxY > 0 == true)
            {
                yScale = (float)((renderRect.Height - 5) / maxY);
            }
            else if (maxY > 0 == true)
            {
                yScale = (float)((renderRect.Height - 5) / (maxY - minY));
            }
            // If (Me.minX * Me.maxX > 0) Then
            // Me.xScale = (Me.renderRect.Width - 5) / Me.maxX
            // ElseIf Me.maxX > 0 Then
            // Me.xScale = ((Me.renderRect.Width - 5) / (Me.maxX - Me.minX))
            // End If
            var xs = MeasureXAxe();
            if (xs.Width > 0f)
            {
                xOrig = xs.Width / xScale;
            }
            else
            {
                xOrig = -minX;
            }

            var ys = MeasureYAxe();
            if (ys.Height > 0f)
            {
                yOrig = ys.Height / yScale;
            }
            else
            {
                yOrig = (float)-minY;
            }
        }

        public override ChartTypes Type
        {
            get
            {
                return ChartTypes.VerticalBarsMultibars;
            }
        }

        protected override void DrawSeries()
        {
            CalculateParameters();
            Context.DrawRectangle(System.Drawing.Pens.Red, renderRect);
            for (float tmp = 0f, loopTo = renderRect.Width; GroupWidth + GroupDistance >= 0 ? tmp <= loopTo : tmp >= loopTo; tmp += GroupWidth + GroupDistance)
                Context.DrawLine(System.Drawing.Pens.Red, renderRect.Left + tmp, renderRect.Top, renderRect.Left + tmp, renderRect.Bottom);
            float x;
            int i;
            CChartSerie serie;
            System.Drawing.Color color;
            System.Drawing.Brush brush;
            CChartValue value;
            var font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            for (int j = 0, loopTo1 = Chart.Series.Count - 1; j <= loopTo1; j++)
            {
                serie = Chart.Series[j];
                color = serie.GetRenderColor();
                brush = new System.Drawing.SolidBrush(color);
                var loopTo2 = serie.Values.Count - 1;
                for (i = 0; i <= loopTo2; i++)
                {
                    x = i * (GroupWidth + GroupDistance) + j * (barDistance + BarWidth);
                    value = serie.Values[i];
                    FillRectangle(brush, x, 0f, BarWidth, (float)value.Value);
                    DrawRectangle(System.Drawing.Pens.Black, x, 0f, BarWidth, (float)value.Value);

                    // Me.TranslateTransform(x, value.Value + 1 * yScale)
                    // Me.RotateTransform(90)
                    string str = Sistema.Formats.FormatValuta(value.Value);
                    var ts = Context.MeasureString(str, font);
                    if (serie.ShowLabels)
                        DrawString(str, font, System.Drawing.Brushes.Black, x, (float)(value.Value + (ts.Height + 1f) / yScale));
                    // Me.RotateTransform(-90)
                    // Me.TranslateTransform(-x, -value.Value - 1 * yScale)
                }

                brush.Dispose();
            }

            font.Dispose();
        }

        protected override void DrawXAxe()
        {
            int i = 0;
            float dx = GroupWidth + GroupDistance;
            var font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            var m = Chart.GetMeasures();
            foreach (CChartLabel label in Chart.Labels)
            {
                string str = label.Label;
                var ts = Context.MeasureString(str, font);
                if (Chart.XAxe.ShowValues)
                    DrawString(str, font, System.Drawing.Brushes.Black, 1f / xScale + i * dx, yOrig - 1f / yScale);

                // Me.Context.DrawString(label.Label, font, Drawing.Brushes.Black, New System.Drawing.Rectangle(Me.xAxeRect.Left + x * Me.xScale, Me.xAxeRect.Top, dx, Me.xAxeRect.Height))
                i += 1;
            }

            font.Dispose();

            // MyBase.DrawXAxe()
        }

        protected override void DrawYAxe()
        {
            base.DrawYAxe();
        }
    }
}