using System;

namespace minidom.Forms
{
    public class LineChartRenderer : ChartRenderer
    {
        private int maxNumXSerie;

        public LineChartRenderer(CChart chart) : base(chart)
        {
        }

        public override string Description
        {
            get
            {
                return "Linee";
            }
        }

        private float MeasureYAxeText()
        {
            using (var font = GetXAxeFont())
            {
                try
                {
                    int l = 1;
                    float s = 1f;
                    int n1 = 1;
                    int n2 = 1;
                    float ret = 0f;
                    var m = Chart.GetMeasures();
                    if (m.MaxY.HasValue && Maths.Abs(m.MaxY.Value) > 0d)
                    {
                        l = (int)Maths.Log10(Maths.Abs(m.MaxY.Value)); // - m.MinY)
                        s = (float)(Maths.Sign(m.MaxY.Value) * Maths.Pow(10d, l - 1));
                        if (s > 0f)
                        {
                            n1 = (int)Maths.Max(0d, Maths.Floor(m.MaxY.Value / s));
                            n2 = (int)Maths.Min(0d, Maths.Floor(m.MinY.Value / s));
                        }
                    }

                    for (int i = n2, loopTo = n1; i <= loopTo; i++)
                    {
                        if (Chart.YAxe.ShowValues)
                        {
                            string str = Sistema.Formats.FormatNumber(i * s, 2);
                            var ts = Context.MeasureString(str, font);
                            ret = Maths.Max(ts.Width / xScale, ret);
                        }
                    }

                    return ret;
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                }
            }
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

            minY = Maths.Min(0, minY);
            maxY = Maths.Max(0, maxY);
            maxNumXSerie = Maths.Max(1, maxNumXSerie);
            float W = renderRect.Width;
            int N = maxNumXSerie;

            // Me.Chart.Labels.GetMinMax(Me.minX, Me.maxX)
            minX = 0f;
            maxX = maxNumXSerie;
            xScale = 1f;
            yScale = 1f;
            if (minY.HasValue)
            {
                if (minY * maxY > 0 == true)
                {
                    yScale = (float)((renderRect.Height - 5) / maxY);
                }
                else if (maxY > 0 == true)
                {
                    yScale = (float)((renderRect.Height - 5) / (maxY - minY));
                }

                yOrig = (float)-minY;
            }
            else
            {
                yOrig = 0f;
            }

            if (minX * maxX > 0f)
            {
                xScale = (renderRect.Width - 5) / maxX;
            }
            else if (maxX > 0f)
            {
                xScale = (renderRect.Width - 5) / (maxX - minX);
            }
            // Me.xOrig = Maths.Max(Me.MeasureYAxeText, -Me.minX)

            var xs = MeasureYAxe();
            if (xs.Width > 0f)
            {
                xOrig = xs.Width / xScale;
            }
            else
            {
                xOrig = -minX;
            }

            // Dim ys As System.Drawing.SizeF = Me.MeasureXAxe
            // If (ys.Height > 0) Then
            // Me.yOrig = ys.Height / Me.yScale
            // Else
            // Me.yOrig = -Me.minY
            // End If


        }

        private void DrawSerieAsLines(CChartSerie serie)
        {
            using (var font = GetXAxeFont())
            {
                System.Drawing.Color color;
                System.Drawing.Pen pen;
                CChartValue value = null;
                CChartValue oldValue = null;
                if (serie.Values.Count < 1)
                    return;
                color = serie.GetRenderColor();
                pen = new System.Drawing.Pen(color, 2f);
                oldValue = serie.Values[0];
                if (oldValue.Value.HasValue)
                {
                    DrawEllipse(pen, 0f, (float)oldValue.Value, 4f / xScale, 4f / yScale);
                    if (!string.IsNullOrEmpty(oldValue.Label))
                        DrawString(oldValue.Label, font, System.Drawing.Brushes.Black, xOrig + 5f / xScale, (float)(oldValue.Value + 5f / yScale));
                }

                for (int i = 1, loopTo = serie.Values.Count - 1; i <= loopTo; i++)
                {
                    value = serie.Values[i];
                    if (oldValue.Value.HasValue)
                    {
                        if (value.Value.HasValue)
                        {
                            DrawLine(pen, i - 1, (float)oldValue.Value, i, (float)value.Value);
                            DrawEllipse(pen, i, (float)value.Value, 4f / xScale, 4f / yScale);
                        }

                        if (!string.IsNullOrEmpty(oldValue.Label))
                            DrawString(oldValue.Label, font, System.Drawing.Brushes.Black, xOrig + i + 5f / xScale, (float)(oldValue.Value + 5f / yScale));
                    }

                    oldValue = value;
                }

                pen.Dispose();
            }
        }

        private void DrawSerieAsBars(CChartSerie serie)
        {
            using (var font = GetXAxeFont())
            {
                System.Drawing.Color color;
                System.Drawing.Brush brush;
                CChartValue value = null;
                if (serie.Values.Count < 1)
                    return;
                color = serie.GetRenderColor();
                brush = new System.Drawing.SolidBrush(color);
                for (int i = 0, loopTo = serie.Values.Count - 1; i <= loopTo; i++)
                {
                    value = serie.Values[i];
                    if (value.Value.HasValue)
                    {
                        FillRectangle(brush, (float)(i + 0.33d), 0f, 0.34f, (float)value.Value);
                        if (!string.IsNullOrEmpty(value.Label))
                            DrawString(value.Label, font, System.Drawing.Brushes.Black, (float)(i + 0.33d + 3f / xScale), (float)(value.Value + 3f / yScale));
                    }
                }

                brush.Dispose();
            }
        }

        protected override void DrawSerie(CChartSerie serie)
        {
            switch (serie.Tipo)
            {
                case ChartTypes.Lines:
                    {
                        DrawSerieAsLines(serie);
                        break;
                    }

                case ChartTypes.VerticalBarsOverlapped:
                    {
                        DrawSerieAsBars(serie);
                        break;
                    }

                default:
                    {
                        DrawSerieAsLines(serie);
                        break;
                    }
            }
        }

        protected override void DrawXAxe()
        {
            using (var font = GetXAxeFont())
            {
                int i = 0;
                float dx = 1f;
                var m = Chart.GetMeasures();
                foreach (CChartLabel label in Chart.Labels)
                {
                    string str = label.Label;
                    var ts = Context.MeasureString(str, font);
                    if (Chart.XAxe.ShowValues)
                        DrawString(str, font, System.Drawing.Brushes.Black, (xOrig + 2f) / xScale + i * dx, (yOrig - 1f) / yScale);

                    // Me.Context.DrawString(label.Label, font, Drawing.Brushes.Black, New System.Drawing.Rectangle(Me.xAxeRect.Left + x * Me.xScale, Me.xAxeRect.Top, dx, Me.xAxeRect.Height))
                    i += 1;
                }
                // MyBase.DrawXAxe()
            }
        }

        protected override void DrawYAxe()
        {
            base.DrawYAxe();
        }

        public override ChartTypes Type
        {
            get
            {
                return ChartTypes.Lines;
            }
        }
    }
}