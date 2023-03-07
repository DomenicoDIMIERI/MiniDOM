
namespace minidom.Forms
{
    public class VerticalBarChartRenderer : VerticalBarOverlappedChartRenderer
    {
        private int maxNumXSerie = 0;

        public VerticalBarChartRenderer(CChart chart) : base(chart)
        {
        }

        protected override void CalculateParameters()
        {
            bool init = false;
            var c = Chart;
            int N = Maths.Max(1, c.Labels.Count);
            float minVal = 0f;
            float maxVal = 0f;
            var ms = c.GetMeasures();
            minVal = (float)ms.MinY;
            maxVal = (float)ms.MaxY;
            foreach (CChartSerie serie in c.Series)
            {
                ms = serie.GetMeasures();
                N = Maths.Max(serie.Values.Count, N);
                // If (Not init) Then
                // If (ms.MinY.HasValue) Then minVal = ms.MinY.Value
                // If (ms.MaxY.HasValue) Then maxVal = ms.MaxY.Value
                // init = True
                // Else
                // If (ms.MinY.HasValue) Then minVal = Maths.Min(ms.MinY.Value, minVal)
                // If (ms.MaxY.HasValue) Then maxVal = Maths.Max(ms.MaxY.Value, minVal)
                // End If
            }

            maxNumXSerie = N;
            float W = renderRect.Width;
            m_BarWidth = (float)Maths.Ceiling(W / N - barDistance);
            minX = 0f;
            minY = minVal;
            maxX = m_BarWidth * N;
            maxY = maxVal;
            xScale = 1f;
            yScale = 1f;
            if (minY * maxY > 0 == true)
            {
                yScale = (float)((renderRect.Height - 50) / maxY);
                yOrig = 0f;
            }
            else if (maxY > 0 == true)
            {
                yScale = (float)((renderRect.Height - 50) / (maxY - minY));
                yOrig = (float)-minY;
            }

            if (minX * maxX > 0f)
            {
                xScale = (renderRect.Width - 50) / maxX;
                xOrig = 0f;
            }
            else if (maxY > 0 == true)
            {
                xScale = (renderRect.Width - 50) / (maxX - minX);
                xOrig = -minX;
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
            // For tmp As Single = 0 To renderRect.Width Step (GroupWidth + GroupDistance)
            // Me.Context.DrawLine(Drawing.Pens.Red, renderRect.Left + tmp, renderRect.Top, renderRect.Left + tmp, renderRect.Bottom)
            // Next

            var c = Chart;
            CChartLabel label;
            CChartSerie serie;
            System.Drawing.Color color;
            System.Drawing.Brush brush;
            var font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            for (int j = 0, loopTo = maxNumXSerie; j <= loopTo; j++)
            {
                label = null;
                if (j < c.Labels.Count)
                    label = c.Labels[j];
                serie = null;
                if (j < c.Series.Count)
                    serie = c.Series[j];
                if (serie is object)
                {
                    float x;
                    float y = 0f;
                    x = j * (barDistance + BarWidth);
                    foreach (var value in serie.Values)
                    {
                        if (value.Value.HasValue)
                        {
                            color = serie.GetRenderColor();
                            brush = new System.Drawing.SolidBrush(color);
                            FillRectangle(brush, x, y, BarWidth, (float)value.Value);
                            DrawRectangle(System.Drawing.Pens.Black, x, y, BarWidth, (float)value.Value);
                            string str = Sistema.Formats.FormatValuta(value.Value);
                            var ts = Context.MeasureString(str, font);
                            if (serie.ShowLabels)
                                DrawString(str, font, System.Drawing.Brushes.Black, x, (float)(y * yScale + value.Value + (ts.Height + 1f) / yScale));
                            y += value.Value.Value * yScale;
                            brush.Dispose();
                        }
                    }
                }
            }

            font.Dispose();
        }

        protected override void DrawXAxe()
        {
            var c = Chart;
            if (!c.XAxe.ShowValues)
                return;
            var m = c.GetMeasures();
            float dx = BarWidth + barDistance;
            var font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            for (int j = 0, loopTo = maxNumXSerie; j <= loopTo; j++)
            {
                CChartLabel label = null;
                if (j < c.Labels.Count)
                    label = c.Labels[j];
                string str = "";
                if (label is object)
                    str = label.Label;
                var ts = Context.MeasureString(str, font);
                DrawString(str, font, System.Drawing.Brushes.Black, 1f / xScale + j * dx, yOrig - 1f / yScale);
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