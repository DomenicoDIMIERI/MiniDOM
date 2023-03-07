using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{


    /// <summary>
    /// Tipo base del renderer dei grafici
    /// </summary>
    /// <remarks></remarks>
    public abstract class ChartRenderer : IDisposable
    {
        private CChart m_Chart;
        private System.Drawing.Graphics m_Context;
        protected float xScale, yScale;
        protected float minX, maxX;
        protected float? minY, maxY;
        protected int minCount, maxCount;
        protected System.Drawing.Rectangle renderRect;
        protected System.Drawing.Rectangle legendRect;
        protected System.Drawing.Rectangle titleRect;
        protected System.Drawing.Rectangle yAxeRect;
        protected System.Drawing.Rectangle xAxeRect;
        protected float m_Padding;
        protected float xOrig, yOrig;
        private static System.Drawing.Color[] m_DefaultColors = DMD.Arrays.CreateInstance<System.Drawing.Color>(11);
        private static System.Drawing.Font m_DefaultFont = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
        // Private m_Font As System.Drawing.Font

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public class CSerieAttributes : IComparable
        {
            public CChartSerie Serie;
            public float xScale = 1f;
            public float yScale = 1f;

            public CSerieAttributes()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CSerieAttributes()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int CompareTo(CSerieAttributes value)
            {
                if (ReferenceEquals(Serie, value.Serie))
                    return 0;
                return 1;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CSerieAttributes)obj);
            }
        }

        private CCollection<CSerieAttributes> m_SerieAttributes;

        public virtual CSerieAttributes GetSerieAttributes(CChartSerie serie)
        {
            if (m_SerieAttributes is null)
                m_SerieAttributes = new CCollection<CSerieAttributes>();
            int i;
            var tmp = new CSerieAttributes();
            tmp.Serie = serie;
            i = m_SerieAttributes.IndexOf(tmp);
            if (i < 0)
            {
                if (m_SerieAttributes.Count >= DMD.Arrays.UBound(CChart.DefaultColors))
                {
                }
                // tmp.Color = Me.GetRandomColor
                else
                {
                    // tmp.Color = Colors.FromWeb(CChart.DefaultColors(m_SerieAttributes.Count))
                }

                m_SerieAttributes.Add(tmp);
            }
            else
            {
                tmp = m_SerieAttributes[i];
            }

            return tmp;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public ChartRenderer(CChart chart)
        {
            DMDObject.IncreaseCounter(this);
            m_Chart = chart;
            m_Padding = 5f;
            // Me.m_Font = m_DefaultFont
        }

        public System.Drawing.Graphics Context
        {
            get
            {
                return m_Context;
            }
        }

        protected internal void SetContext(System.Drawing.Graphics g)
        {
            m_Context = g;
        }

        // Public Property Font As System.Drawing.Font
        // Get
        // Return Me.m_Font
        // End Get
        // Set(value As System.Drawing.Font)
        // Me.m_Font = value
        // End Set
        // End Property

        /// <summary>
        /// Restituisce il grafico visualizzato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChart Chart
        {
            get
            {
                return m_Chart;
            }
        }

        public System.Drawing.Color GetRandomColor()
        {
            byte r, g, b;
            r = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
            g = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
            b = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
            return System.Drawing.Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// Crea il font utilizzato per il titolo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual System.Drawing.Font GetTitleFont()
        {
            var ret = m_DefaultFont;
            var f = Chart.Title.Font;
            if (f is null)
                f = Chart.Font;
            if (f is object)
                ret = f.GetSystemFont();
            var tmp = new System.Drawing.Font(ret.FontFamily, ret.Size * 1.5f, System.Drawing.FontStyle.Bold, ret.Unit);
            if (!ReferenceEquals(ret, m_DefaultFont))
                ret.Dispose();
            return tmp;
        }

        protected virtual System.Drawing.Font GetLegendFont()
        {
            var ret = m_DefaultFont;
            var f = Chart.Legend.Font;
            if (f is null)
                f = Chart.Font;
            if (f is object)
                ret = f.GetSystemFont();
            return (System.Drawing.Font)ret.Clone();
        }

        protected virtual System.Drawing.Font GetXAxeFont()
        {
            var ret = m_DefaultFont;
            var f = Chart.XAxe.Font;
            if (f is null)
                f = Chart.Font;
            if (f is object)
                ret = f.GetSystemFont();
            return (System.Drawing.Font)ret.Clone();
        }

        protected virtual System.Drawing.Font GetYAxeFont()
        {
            var ret = m_DefaultFont;
            var f = Chart.YAxe.Font;
            if (f is null)
                f = Chart.Font;
            if (f is object)
                ret = f.GetSystemFont();
            return (System.Drawing.Font)ret.Clone();
        }

        protected virtual System.Drawing.SizeF MeasureTitleElement()
        {
            using (var gfont = GetTitleFont())
            {
                var s = Context.MeasureString(Chart.Title.Text, gfont);
                s.Width += 10f;
                s.Height += 5f;
                return s;
            }
        }

        protected virtual System.Drawing.SizeF MeasureLegendElement()
        {
            using (var font = GetLegendFont())
            {
                System.Drawing.SizeF ret = default, s;
                ret.Width = 0f;
                ret.Height = 10f;
                for (int i = 0, loopTo = Chart.Series.Count - 1; i <= loopTo; i++)
                {
                    var serie = Chart.Series[i];
                    s = Context.MeasureString(serie.Name, font);
                    ret.Height += Maths.Min(15f, s.Height) + 2f;
                    if (s.Width > ret.Width)
                        ret.Width = s.Width;
                }

                ret.Width += 25f;
                return ret;
            }
        }

        protected virtual System.Drawing.SizeF MeasureXAxe()
        {
            var c = Chart;
            var ret = new System.Drawing.SizeF();
            if (c.XAxe.Size is object)
                ret = new System.Drawing.SizeF((float)c.XAxe.Size.Width, (float)c.XAxe.Size.Height);
            return ret;
        }

        protected virtual System.Drawing.SizeF MeasureYAxe()
        {
            var c = Chart;
            if (c.YAxe.Size is object)
                return new System.Drawing.SizeF((float)c.YAxe.Size.Width, (float)c.YAxe.Size.Height);
            System.Drawing.SizeF s = new System.Drawing.SizeF(), ret = new System.Drawing.SizeF();
            var ms = c.GetMeasures();
            using (var font = GetXAxeFont())
            {
                ret.Height = renderRect.Height;
                if (ms.MinY.HasValue && ms.MaxY.HasValue)
                {
                    for (int i = 0, loopTo = Chart.Series.Count - 1; i <= loopTo; i++)
                    {
                        var serie = Chart.Series[i];
                        ms = serie.GetMeasures();
                        if (ms.MinY.HasValue)
                        {
                            string str = Sistema.Formats.FormatNumber(DMD.Strings.CStr(ms.MinY), 2);
                            s = Context.MeasureString(str, font);
                            if (s.Width > ret.Width)
                                ret.Width = s.Width;
                            str = Sistema.Formats.FormatNumber(DMD.Strings.CStr(ms.MaxY), 2);
                            s = Context.MeasureString(str, font);
                            if (s.Width > ret.Width)
                                ret.Width = s.Width;
                        }
                    }
                }

                ret.Width += 15f;
                return ret;
            }
        }

        protected virtual void CalculateParameters()
        {
            var ms = Chart.GetMeasures();
            minX = ms.MinX;
            maxX = ms.MaxX;
            minY = Maths.Min(0, ms.MinY);
            maxY = Maths.Max(0, ms.MaxY);
            xOrig = -ms.MinX;
            yOrig = (float)-ms.MinY;
            minCount = ms.MinCount;
            maxCount = ms.MaxCount;
            if (ms.MaxX > ms.MinX)
            {
                xScale = (renderRect.Width - 20) / (ms.MaxX - ms.MinX);
            }
            else if (ms.MaxCount > 0)
            {
                xScale = (float)((renderRect.Width - 20) / (double)ms.MaxCount);
            }
            else
            {
                xScale = 1f;
            }

            if (ms.MaxY > ms.MinY == true)
            {
                yScale = (float)(renderRect.Height / (ms.MaxY - ms.MinY));
            }
            else
            {
                yScale = 1f;
            }
        }

        private void DrawXGrid()
        {
            System.Drawing.Pen bigpen = null;
            System.Drawing.Pen smallpen = null;
            System.Drawing.Font font = null;
            try
            {
                smallpen = new System.Drawing.Pen(System.Drawing.Brushes.Gray, 0.5f);
                bigpen = new System.Drawing.Pen(System.Drawing.Brushes.Gray, 1f);
                font = GetXAxeFont();
                if (minY.HasValue == false)
                    return;
                float diff = (float)(maxY - minY);
                if (diff <= 0f)
                    return;
                if (Chart.YAxe.ScalaLogaritmica)
                {
                    float s = diff / 10f;
                    int n = (int)System.Maths.Log10(s);
                    s = (float)System.Maths.Round(s / Maths.Pow(10d, n), 1);
                    if (s - Maths.Floor(s) > 0.5d)
                    {
                        s = (float)(Maths.Floor(s) + 1d);
                    }
                    else
                    {
                        s = (float)Maths.Floor(s);
                    }

                    s = (float)(s * Maths.Pow(10d, n));
                    float n1 = (float)(Maths.Floor(minY.Value / s) * s);
                    float n2 = (float)(Maths.Floor(maxY.Value / s) * s);
                    for (float i = n1, loopTo = n2; s >= 0 ? i <= loopTo : i >= loopTo; i += s)
                    {
                        if (Chart.YAxe.ShowValues)
                        {
                            string str = Sistema.Formats.FormatNumber(i.ToString(), 2);
                            var ts = Context.MeasureString(str, font);
                            DrawLine(bigpen, 0f, yAxeRect.Top + yAxeRect.Height - i * yScale, yAxeRect.Width, yAxeRect.Top + yAxeRect.Height - i * yScale);
                        }
                    }
                }
                else
                {
                    var m = Chart.GetMeasures();
                    // Dim l As Integer = 1
                    // If (Maths.Abs(m.MaxY) > 0) Then l = Maths.Log10(Maths.Abs(m.MaxY)) ' - m.MinY)
                    int n1, n2;
                    float s;
                    if (Chart.Grid.ShowSecondaryGrid)
                    {
                        s = Chart.YAxe.GetSmallStep((float)m.MinY, (float)m.MaxY); // Maths.Sign(m.MaxY.Value) * 10 ^ (l - 1)
                        n1 = (int)Maths.Max(0d, Maths.Floor(m.MaxY.Value / s));
                        n2 = (int)Maths.Min(0d, Maths.Floor(m.MinY.Value / s));
                        for (int i = n2, loopTo1 = n1; i <= loopTo1; i++)
                            // If Me.Chart.YAxe.ShowValues Then
                            // Dim str As String = Formats.FormatNumber(i * s, 2)
                            // Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                            // End If
                            DrawLine(smallpen, 0f, i * s, xAxeRect.Width, i * s);
                    }

                    s = Chart.YAxe.GetLargeStep((float)m.MinY, (float)m.MaxY); // Maths.Sign(m.MaxY.Value) * 10 ^ (l - 1)
                    n1 = (int)Maths.Max(0d, Maths.Floor(m.MaxY.Value / s));
                    n2 = (int)Maths.Min(0d, Maths.Floor(m.MinY.Value / s));
                    for (int i = n2, loopTo2 = n1; i <= loopTo2; i++)
                        // If Me.Chart.YAxe.ShowValues Then
                        // Dim str As String = Formats.FormatNumber(i * s, 2)
                        // Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                        // End If
                        DrawLine(bigpen, 0f, i * s, xAxeRect.Width, i * s);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (bigpen is object)
                {
                    bigpen.Dispose();
                    bigpen = null;
                }

                if (smallpen is object)
                {
                    smallpen.Dispose();
                    smallpen = null;
                }

                if (font is object)
                {
                    font.Dispose();
                    font = null;
                }
            }
        }

        private void DrawYGrid()
        {
            System.Drawing.Pen bigpen = null;
            System.Drawing.Pen smallpen = null;
            System.Drawing.Font font = null;
            try
            {
                smallpen = new System.Drawing.Pen(System.Drawing.Brushes.Gray, 1f);
                bigpen = new System.Drawing.Pen(System.Drawing.Brushes.Gray, 1.5f);
                font = GetXAxeFont();
                var m = Chart.GetMeasures();
                for (int i = 1, loopTo = m.MaxCount; i <= loopTo; i++)
                {
                    if (m.MaxY > 0 == true)
                        DrawLine(bigpen, i - 1, 0f, i - 1, (float)m.MaxY);
                    if (m.MinY > 0 == true)
                        DrawLine(bigpen, i - 1, 0f, i - 1, (float)m.MinY);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (bigpen is object)
                {
                    bigpen.Dispose();
                    bigpen = null;
                }

                if (smallpen is object)
                {
                    smallpen.Dispose();
                    smallpen = null;
                }

                if (font is object)
                {
                    font.Dispose();
                    font = null;
                }
            }
        }

        protected virtual void DrawGrid()
        {
            if (Chart.Grid.ShowXGrid)
                DrawXGrid();
            if (Chart.Grid.ShowYGrid)
                DrawYGrid();
        }

        // Protected Overridable Sub DrawXGrid()
        // Dim bigpen As System.Drawing.Pen = Nothing
        // Dim smallpen As System.Drawing.Pen = Nothing
        // Try
        // smallpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1)
        // bigpen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1.5)

        // If (Me.Chart.Grid.ShowXGrid) Then
        // Dim m As ChartMeasure = Me.Chart.GetMeasures
        // Dim s As Single = Me.Chart.XAxe.GetLargeStep(m.MinX, m.MaxX)
        // Dim n1 As Integer = Me.Chart.XAxe.GetLowestValue(m.MinX, m.MaxX)
        // Dim n2 As Integer = Me.Chart.XAxe.GetUpperValue(m.MinX, m.MaxX)
        // For i As Integer = n1 To n2
        // Me.DrawLine(bigpen, i * s, -Me.yOrig, i * s, Me.renderRect.Height)
        // Next
        // End If

        // If (Me.Chart.Grid.ShowYGrid) Then
        // Dim m As ChartMeasure = Me.Chart.GetMeasures
        // Dim s As Single = 1
        // Dim n1 As Double = 1
        // Dim n2 As Double = 1

        // If (m.MinY.HasValue AndAlso m.MaxY.HasValue) Then
        // If (Me.Chart.Grid.ShowSecondaryGrid) Then
        // s = Me.Chart.YAxe.GetSmallStep(m.MinY, m.MaxY)
        // n1 = Me.Chart.YAxe.GetLowestValue(m.MinY, m.MaxY)
        // n2 = Me.Chart.YAxe.GetUpperValue(m.MinY, m.MaxY)

        // For i As Integer = n1 To n2
        // Me.DrawLine(smallpen, -Me.xOrig, i * s, Me.renderRect.Width, i * s)
        // Next
        // End If

        // s = Me.Chart.YAxe.GetLargeStep(m.MinY, m.MaxY)
        // n1 = Me.Chart.YAxe.GetLowestValue(m.MinY, m.MaxY)
        // n2 = Me.Chart.YAxe.GetUpperValue(m.MinY, m.MaxY)

        // For i As Integer = n1 To n2
        // Me.DrawLine(bigpen, -Me.xOrig, i * s, Me.renderRect.Width, i * s)
        // Next
        // End If


        // End If

        // Catch ex As Exception
        // Throw
        // Finally
        // If (bigpen IsNot Nothing) Then bigpen.Dispose() : bigpen = Nothing
        // If (smallpen IsNot Nothing) Then smallpen.Dispose() : smallpen = Nothing
        // End Try
        // End Sub

        // Protected Overridable Sub DrawGrid()
        // Dim pen As System.Drawing.Pen = Nothing
        // Try
        // pen = New System.Drawing.Pen(Drawing.Brushes.Gray, 1)
        // If (Me.Chart.Grid.ShowXGrid) Then
        // Dim m As ChartMeasure = Me.Chart.GetMeasures
        // Dim s As Single = Me.Chart.XAxe.GetLargeStep(m.MinX, m.MaxX)
        // Dim n1 As Integer = Me.Chart.XAxe.GetLowestValue(m.MinX, m.MaxX)
        // Dim n2 As Integer = Me.Chart.XAxe.GetUpperValue(m.MinX, m.MaxX)
        // For i As Integer = n1 To n2
        // Me.DrawLine(pen, i * s, -Me.yOrig, i * s, Me.renderRect.Height)
        // Next
        // End If
        // If (Me.Chart.Grid.ShowYGrid) Then
        // Dim m As ChartMeasure = Me.Chart.GetMeasures
        // Dim s As Single = 1
        // Dim n1 As Double = 1
        // Dim n2 As Double = 1

        // If (m.MinY.HasValue AndAlso m.MaxY.HasValue) Then
        // s = Me.Chart.YAxe.GetLargeStep(m.MinY, m.MaxY)
        // n1 = Me.Chart.YAxe.GetLowestValue(m.MinY, m.MaxY)
        // n2 = Me.Chart.YAxe.GetUpperValue(m.MinY, m.MaxY)
        // End If

        // For i As Integer = n1 To n2
        // Me.DrawLine(pen, -Me.xOrig, i * s, Me.renderRect.Width, i * s)
        // Next
        // End If

        // Catch ex As Exception
        // Throw
        // Finally
        // If (pen IsNot Nothing) Then pen.Dispose() : pen = Nothing
        // End Try
        // End Sub

        protected virtual void DrawXAxe()
        {
            System.Drawing.Pen pen = null;
            try
            {
                pen = new System.Drawing.Pen(System.Drawing.Brushes.Black, 1.5f);
                DrawLine(pen, -renderRect.Width / xScale, 0f, renderRect.Width / xScale, 0f);
                pen.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (pen is object)
                {
                    pen.Dispose();
                    pen = null;
                }
            }
        }

        protected virtual void DrawYAxe()
        {
            using (var font = GetYAxeFont())
            {
                System.Drawing.Pen pen = null;
                try
                {
                    pen = new System.Drawing.Pen(System.Drawing.Brushes.Black, 1.5f);
                    DrawLine(pen, 0f, -renderRect.Height / yScale, 0f, renderRect.Height / yScale);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (pen is object)
                    {
                        pen.Dispose();
                        pen = null;
                    }
                }

                if (minY.HasValue == false)
                    return;
                float diff = (float)(maxY - minY);
                if (diff <= 0f)
                    return;
                if (Chart.YAxe.ScalaLogaritmica)
                {
                    float s = diff / 10f;
                    int n = (int)System.Maths.Log10(s);
                    s = (float)System.Maths.Round(s / Maths.Pow(10d, n), 1);
                    if (s - Maths.Floor(s) > 0.5d)
                    {
                        s = (float)(Maths.Floor(s) + 1d);
                    }
                    else
                    {
                        s = (float)Maths.Floor(s);
                    }

                    s = (float)(s * Maths.Pow(10d, n));
                    float n1 = (float)(Maths.Floor(minY.Value / s) * s);
                    float n2 = (float)(Maths.Floor(maxY.Value / s) * s);
                    for (float i = n1, loopTo = n2; s >= 0 ? i <= loopTo : i >= loopTo; i += s)
                    {
                        if (Chart.YAxe.ShowValues)
                        {
                            string str = Sistema.Formats.FormatNumber(i.ToString(), Chart.YAxe.Decimals);
                            var ts = Context.MeasureString(str, font);
                            Context.DrawString(str, font, System.Drawing.Brushes.Black, new System.Drawing.Rectangle((int)(yAxeRect.Right - ts.Width), (int)(yAxeRect.Top + yAxeRect.Height - i * yScale), 2 * yAxeRect.Width, 100));
                        }
                    }
                }
                else
                {
                    // Dim font As System.Drawing.Font = Nothing
                    try
                    {
                        // font = New System.Drawing.Font("Tahoma", 10, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
                        var m = Chart.GetMeasures();
                        int l = 1;
                        if (Maths.Abs((double)m.MaxY) > 0d)
                            l = (int)Maths.Log10(Maths.Abs((double)m.MaxY)); // - m.MinY)
                        float s = (float)(Maths.Sign(m.MaxY.Value) * Maths.Pow(10d, l - 1));
                        int n1 = (int)Maths.Max(0d, Maths.Floor(m.MaxY.Value / s));
                        int n2 = (int)Maths.Min(0d, Maths.Floor(m.MinY.Value / s));
                        for (int i = n2, loopTo1 = n1; i <= loopTo1; i++)
                        {
                            if (Chart.YAxe.ShowValues)
                            {
                                string str = Sistema.Formats.FormatNumber(i * s, Chart.YAxe.Decimals);
                                var ts = Context.MeasureString(str, font);
                                // Me.DrawString(str, font, Drawing.Brushes.Black, (Me.xOrig - ts.Width) / Me.xScale, (ts.Height *0.5) / Me.yScale + i * s)
                                DrawString(str, font, System.Drawing.Brushes.Black, -ts.Width / xScale, (float)(ts.Height * 0.5d / yScale + i * s));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        // If (font IsNot Nothing) Then font.Dispose() : font = Nothing
                    }
                }
            }
        }

        protected virtual void DrawSeries()
        {
            for (int i = 0, loopTo = Chart.Series.Count - 1; i <= loopTo; i++)
            {
                var serie = Chart.Series[i];
                DrawSerie(serie);
            }
        }

        protected abstract void DrawSerie(CChartSerie serie);


        /// <summary>
        /// Restituisce il tipo del grafico che questo renderer consente di disegnare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract ChartTypes Type { get; }

        /// <summary>
        /// Restituisce una descrizione di questo renderer
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract string Description { get; }

        protected virtual void SetupRegions(System.Drawing.Rectangle region)
        {
            var titleSize = MeasureTitleElement();
            var legendSize = MeasureLegendElement();
            var xAxeSize = MeasureXAxe();
            var yAxeSize = MeasureYAxe();
            renderRect = new System.Drawing.Rectangle((int)(region.Left + m_Padding), (int)(region.Top + m_Padding), (int)(region.Width - 2f * m_Padding), (int)(region.Height - 2f * m_Padding));
            switch (Chart.Title.Position)
            {
                case ChartElementPosition.Bottom:
                    {
                        renderRect.Height = (int)(renderRect.Height - titleSize.Height);
                        titleRect = new System.Drawing.Rectangle((int)((region.Width - titleSize.Width) / 2f), renderRect.Height, (int)titleSize.Width, (int)titleSize.Height);
                        break;
                    }

                case ChartElementPosition.Top:
                    {
                        renderRect.Y = (int)(renderRect.Y + titleSize.Height);
                        renderRect.Height = (int)(renderRect.Height - titleSize.Height);
                        titleRect = new System.Drawing.Rectangle((int)((region.Width - titleSize.Width) / 2f), 0, (int)titleSize.Width, (int)titleSize.Height);
                        break;
                    }

                case ChartElementPosition.Left:
                    {
                        renderRect.X = (int)(renderRect.X + titleSize.Width);
                        renderRect.Width = (int)(renderRect.Width - titleSize.Width);
                        titleRect = new System.Drawing.Rectangle(0, (int)((renderRect.Height - titleSize.Height) / 2f), (int)titleSize.Width, (int)titleSize.Height);
                        break;
                    }

                case ChartElementPosition.Right:
                    {
                        renderRect.Width = (int)(renderRect.Width - titleSize.Width);
                        titleRect = new System.Drawing.Rectangle(renderRect.Width, (int)((renderRect.Height - titleSize.Height) / 2f), (int)titleSize.Width, (int)titleSize.Height);
                        break;
                    }
            }

            switch (Chart.Legend.Position)
            {
                case ChartElementPosition.Bottom:
                    {
                        renderRect.Height = (int)(renderRect.Height - legendSize.Height);
                        legendRect = new System.Drawing.Rectangle((int)((region.Width - legendSize.Width) / 2f), renderRect.Height, (int)legendSize.Width, (int)legendSize.Height);
                        break;
                    }

                case ChartElementPosition.Top:
                    {
                        renderRect.Y = (int)(renderRect.Y + legendSize.Height);
                        renderRect.Height = (int)(renderRect.Height - legendSize.Height);
                        legendRect = new System.Drawing.Rectangle((int)((region.Width - legendSize.Width) / 2f), 0, (int)legendSize.Width, (int)legendSize.Height);
                        break;
                    }

                case ChartElementPosition.Left:
                    {
                        renderRect.X = (int)(renderRect.X + legendSize.Width);
                        renderRect.Width = (int)(renderRect.Width - legendSize.Width);
                        legendRect = new System.Drawing.Rectangle(0, (int)((renderRect.Height - legendSize.Height) / 2f), (int)legendSize.Width, (int)legendSize.Height);
                        break;
                    }

                case ChartElementPosition.Right:
                    {
                        renderRect.Width = (int)(renderRect.Width - legendSize.Width);
                        legendRect = new System.Drawing.Rectangle(renderRect.Width, (int)Maths.Max(0f, (renderRect.Height - legendSize.Height) / 2f), (int)legendSize.Width, (int)Maths.Min(legendSize.Height, renderRect.Height));
                        break;
                    }
            }

            renderRect.X = (int)(renderRect.X + yAxeSize.Width);
            renderRect.Width = (int)(renderRect.Width - yAxeSize.Width);
            yAxeRect = new System.Drawing.Rectangle(0, renderRect.Top, (int)yAxeSize.Width, renderRect.Height);
            renderRect.Height -= xAxeRect.Height;
            xAxeRect = new System.Drawing.Rectangle(renderRect.Left, renderRect.Top + renderRect.Height, renderRect.Width, (int)xAxeSize.Height);
        }

        /// <summary>
        /// Disegna il grafico
        /// </summary>
        /// <param name="g"></param>
        /// <param name="region"></param>
        /// <remarks></remarks>
        public void PaintTo(System.Drawing.Graphics g, System.Drawing.Rectangle region)
        {
            // Dim oldClip As System.Drawing.Region = g.Clip
            // Dim oldTransform As System.Drawing.Drawing2D.Matrix = g.Transform

            SetContext(g);
            g.Clear(System.Drawing.Color.White);

            // g.SetClip(region)
            SetupRegions(region);
            CalculateParameters();

            // g.TranslateTransform(region.X + Me.xOrig * Me.xScale, region.Y + Me.yOrig * Me.yScale)
            // g.ScaleTransform(Me.xScale, Me.yScale)
            DrawGrid();
            DrawSeries();
            DrawXAxe();
            DrawYAxe();
            DrawLegend();
            DrawTitle();


            // g.Transform = oldTransform
            // g.Clip = oldClip
        }

        public override string ToString()
        {
            return Description;
        }

        protected virtual void FillRectangle(System.Drawing.Brush brush, float x, float y, float width, float height)
        {
            {
                var withBlock = renderRect;
                int x1 = (int)(withBlock.Left + (xOrig + x) * xScale);
                int y1 = (int)(withBlock.Top + withBlock.Height - (y + yOrig + height) * yScale);
                int w1 = (int)(width * xScale);
                int h1 = (int)(height * yScale);
                if (w1 != 0 & h1 != 0)
                {
                    Context.FillRectangle(brush, x1, y1, w1, h1);
                }
            }
        }

        protected virtual void FillRectangle(System.Drawing.Brush brush, System.Drawing.Rectangle rect)
        {
            {
                var withBlock = renderRect;
                int x1 = (int)(withBlock.Left + (rect.X + xOrig) * xScale);
                int y1 = (int)(withBlock.Top + withBlock.Height - (rect.Y + yOrig + rect.Height) * yScale);
                int w1 = (int)(rect.Width * xScale);
                int h1 = (int)(rect.Height * yScale);
                if (w1 != 0 & h1 != 0)
                {
                    Context.FillRectangle(brush, x1, y1, w1, h1);
                }
            }
        }

        protected virtual void DrawRectangle(System.Drawing.Pen pen, float x, float y, float width, float height)
        {
            {
                var withBlock = renderRect;
                float x1 = withBlock.Left + (x + xOrig) * xScale;
                float y1 = withBlock.Top + withBlock.Height - (y + yOrig + height) * yScale;
                float w1 = width * xScale;
                float h1 = height * yScale;
                if (w1 != 0f & h1 != 0f)
                {
                    Context.DrawRectangle(pen, x1, y1, w1, h1);
                }
            }
        }

        protected virtual void DrawRectangle(System.Drawing.Pen pen, System.Drawing.Rectangle rect)
        {
            {
                var withBlock = renderRect;
                float x1 = withBlock.Left + (rect.X + xOrig) * xScale;
                float y1 = withBlock.Top + withBlock.Height - (rect.Y + yOrig + rect.Height) * yScale;
                float w1 = rect.Width * xScale;
                float h1 = rect.Height * yScale;
                if (w1 != 0f & h1 != 0f)
                {
                    Context.DrawRectangle(pen, x1, y1, w1, h1);
                }
            }
        }

        protected virtual void DrawEllipse(System.Drawing.Pen pen, float x, float y, float width, float height)
        {
            {
                var withBlock = renderRect;
                Context.DrawEllipse(pen, new System.Drawing.Rectangle((int)(withBlock.Left + (xOrig + x) * xScale), (int)(withBlock.Top + withBlock.Height - (y + yOrig) * yScale), (int)(width * xScale), (int)(height * yScale)));
            }
        }

        protected virtual void DrawLine(System.Drawing.Pen pen, float x1, float y1, float x2, float y2)
        {
            {
                var withBlock = renderRect;
                Context.DrawLine(pen, withBlock.Left + (xOrig + x1) * xScale, withBlock.Top + withBlock.Height - (y1 + yOrig) * yScale, withBlock.Left + (xOrig + x2) * xScale, withBlock.Top + withBlock.Height - (yOrig + y2) * yScale);
            }
        }

        protected virtual void DrawArc(System.Drawing.Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngel)
        {
            // With Me.renderRect
            // Dim x1 As Single = .Left + (Me.xOrig + x) * Me.xScale
            // Dim y1 As Single = .Top + .Height - (Me.yOrig + y) * Me.yScale
            // Dim w1 As Single = width * Me.xScale
            // Dim h1 As Single = height * Me.yScale
            // If (w1 <> 0) And (h1 <> 0) Then
            // Me.Context.DrawArc(pen, x1 - w1, y1 - h1, w1 * 2, h1 * 2, 90 - startAngle, sweepAngel)
            // End If
            // End With
            Context.DrawArc(pen, new System.Drawing.Rectangle((int)(renderRect.Left + x - width / 2f), (int)(renderRect.Top + y - height / 2f), (int)width, (int)height), -90 + startAngle, sweepAngel);
        }

        protected virtual void FillArc(System.Drawing.Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngel)
        {
            // With Me.renderRect
            // Dim x1 As Single = .Left + (Me.xOrig + x) * Me.xScale
            // Dim y1 As Single = .Top + .Height - (Me.yOrig + y) * Me.yScale
            // Dim w1 As Single = width * Me.xScale
            // Dim h1 As Single = height * Me.yScale
            Context.FillPie(brush, new System.Drawing.Rectangle((int)(renderRect.Left + x - width / 2f), (int)(renderRect.Top + y - height / 2f), (int)width, (int)height), -90 + startAngle, sweepAngel);
            // Me.Context.FillPie(brush, x1 - w1, y1 - h1, w1 * 2, h1 * 2, startAngle, sweepAngel)

            // End With
        } // Sub

        protected virtual void DrawString(string text, System.Drawing.Font font, System.Drawing.Brush brush, System.Drawing.Rectangle rect)
        {
            {
                var withBlock = renderRect;
                float x1 = withBlock.Left + (xOrig + rect.X) * xScale;
                float y1 = withBlock.Top + withBlock.Height - (rect.Y + yOrig + rect.Height) * yScale;
                float w1 = rect.Width * xScale;
                float h1 = rect.Height * yScale;
                if (w1 != 0f & h1 != 0f)
                {
                    Context.DrawString(text, font, brush, rect);
                }
            }
        }

        protected virtual void DrawString(string text, System.Drawing.Font font, System.Drawing.Brush brush, float x, float y)
        {
            {
                var withBlock = renderRect;
                float x1 = withBlock.Left + (xOrig + x) * xScale;
                float y1 = withBlock.Top + withBlock.Height - (y + yOrig) * yScale;
                Context.DrawString(text, font, brush, x1, y1);
            }
        }

        protected virtual void DrawTitle()
        {
            var fontg = GetTitleFont();
            var brush = new System.Drawing.SolidBrush(Chart.Title.ForeColor);
            Context.DrawString(Chart.Title.Text, fontg, brush, titleRect);
            fontg.Dispose();
            brush.Dispose();
        }

        protected virtual void DrawLegend()
        {
            using (var font = GetLegendFont())
            {
                var brush = new System.Drawing.SolidBrush(Chart.Legend.BackColor);
                System.Drawing.Pen pen;
                float y;
                System.Drawing.SizeF s;
                Context.FillRectangle(brush, legendRect);
                brush.Dispose();
                if (Chart.Legend.BorderSize > 0f)
                {
                    pen = new System.Drawing.Pen(Chart.Legend.BorderColor, Chart.Legend.BorderSize);
                    Context.DrawRectangle(pen, legendRect);
                    pen.Dispose();
                }

                y = legendRect.Top + 5;
                if (!string.IsNullOrEmpty(Chart.Legend.Text))
                {
                    s = Context.MeasureString(Chart.Legend.Text, font);
                    Context.DrawString(Chart.Legend.Text, font, System.Drawing.Brushes.Black, legendRect.Left + 5, y);
                    y += s.Height + 2f;
                }

                for (int i = 0, loopTo = Chart.Series.Count - 1; i <= loopTo; i++)
                {
                    var serie = Chart.Series[i];
                    s = Context.MeasureString(serie.Name, font);
                    brush = new System.Drawing.SolidBrush(serie.GetRenderColor());
                    Context.FillRectangle(brush, legendRect.Left + 5, y - 1f, 15f, 15f);
                    Context.DrawString(serie.Name, font, System.Drawing.Brushes.Black, legendRect.Left + 25, y);
                    y += s.Height + 2f;
                    brush.Dispose();
                }
            }
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public virtual void Dispose()
        {
            m_Chart = null;
            m_Context = null;
            // Me.renderRect = Nothing:             Me.legendRect = Nothing:             Me.titleRect = Nothing:             Me.yAxeRect = Nothing:             Me.xAxeRect = Nothing
            // Me.m_Font = Nothing
        }

        ~ChartRenderer()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}