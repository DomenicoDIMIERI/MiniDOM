
namespace minidom.Forms
{


    /// <summary>
    /// Renderer di un grafico a barre verticali
    /// </summary>
    /// <remarks></remarks>
    public class VerticalBarOverlappedChartRenderer : ChartRenderer
    {

        // Protected barWidth As Single
        protected float barDistance;
        // Private m_Subformat As BarChartFormats
        protected float m_BarWidth;

        public VerticalBarOverlappedChartRenderer(CChart chart) : base(chart)
        {
            // Me.barWidth = 10
            barDistance = 2f;
        }

        public override string Description
        {
            get
            {
                return "Vertical bars";
            }
        }

        public float BarWidth
        {
            get
            {
                return m_BarWidth;
            }
        }

        protected override void DrawSerie(CChartSerie serie)
        {
            float x;
            System.Drawing.Color color;
            System.Drawing.Brush brush;
            x = 0f;
            color = serie.GetRenderColor();
            brush = new System.Drawing.SolidBrush(color);
            foreach (CChartValue value in serie.Values)
            {
                FillRectangle(brush, x, 0f, BarWidth, (float)value.Value);
                DrawRectangle(System.Drawing.Pens.Black, x, 0f, BarWidth, (float)value.Value);
                x += BarWidth + barDistance;
            }

            brush.Dispose();
        }

        protected override void CalculateParameters()
        {
            bool init = false;
            int maxNumXSerie = 0;
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

            m_BarWidth = (float)(renderRect.Width / (double)(maxNumXSerie * Chart.Series.Count));
            minX = 0f;
            maxX = Chart.Labels.Count;
            foreach (CChartSerie serie in Chart.Series)
            {
                if (serie.Values.Count > maxX)
                    maxX = serie.Values.Count;
            }

            maxX = maxX * (BarWidth + barDistance);
            xScale = 1f;
            yScale = 1f;
            if (minY < maxY == true)
                yScale = (float)((renderRect.Height - 5) / (maxY - minY));
            // If (Me.minX < Me.maxX) Then Me.xScale = ((Me.renderRect.Width - 5) / (Me.maxX - Me.minX))
            xOrig = -minX;
            yOrig = (float)-minY;
        }

        public override ChartTypes Type
        {
            get
            {
                return ChartTypes.VerticalBarsOverlapped;
            }
        }
    }
}