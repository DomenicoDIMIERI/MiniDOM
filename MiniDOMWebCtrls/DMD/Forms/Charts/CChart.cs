using System;
using DMD.XML;

namespace minidom.Forms
{

    /// <summary>
    /// Tipi di grafico supportato
    /// </summary>
    public enum ChartTypes : int
    {
        /// <summary>
        /// Non impostato
        /// </summary>
        Unset = 0,

        /// <summary>
        /// Barre verticali sovrapposte
        /// </summary>
        VerticalBarsOverlapped = 10,

        /// <summary>
        /// Linee
        /// </summary>
        Lines = 20,

        /// <summary>
        /// Torta
        /// </summary>
        Pie = 30,

        /// <summary>
        /// Barre verticali
        /// </summary>
        VerticalBars = 35,

        /// <summary>
        /// Barre verticali raggruppate
        /// </summary>
        VerticalBarsMultibars = 40,

        /// <summary>
        /// Barre 3D verticali
        /// </summary>
        VerticalBars3D = 50
    }

    /// <summary>
    /// Grafico
    /// </summary>
    public class CChart
        : WebControl
    {
        private CChartTitle m_Title;
        private CChartLegend m_Legend;
        private CChartAxe m_XAxe;
        private CChartAxe m_YAxe;
        private ChartTypes m_Type;
        private ChartRenderer m_Renderer;
        private CChartSeries m_Series;
        private CChartLabels m_Labels;
        private CChartGrid m_Grid;
        public static string[] DefaultColors = new string[] { "40699c", "9e413e", "7f9a48", "695185", "3c8da3", "cc7b38", "4f81bd", "c0504d", "9bbb59", "8064a2" };

        public CChart()
        {
            m_Title = new CChartTitle(this);
            m_Legend = new CChartLegend(this);
            m_XAxe = new CChartAxe("X", this);
            m_YAxe = new CChartAxe("Y", this);
            m_Type = ChartTypes.Lines;
            m_Renderer = null;
            m_Series = null;
            m_Labels = null;
            m_Grid = new CChartGrid(this);
        }

        protected internal void Invalidate()
        {
        }


        /// <summary>
        /// Accede alla griglia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartGrid Grid
        {
            get
            {
                if (m_Grid is null)
                    m_Grid = new CChartGrid(this);
                return m_Grid;
            }
        }

        /// <summary>
        /// Accede alle etichette
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartLabels Labels
        {
            get
            {
                if (m_Labels is null)
                    m_Labels = new CChartLabels(this);
                return m_Labels;
            }
        }

        /// <summary>
        /// Restituisce o imposta il tipo del grafico
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartTypes Type
        {
            get
            {
                return m_Type;
            }

            set
            {
                if (m_Type == value)
                    return;
                m_Type = value;
                m_Renderer = null;
                Invalidate();
            }
        }

        /// <summary>
        /// Accede alle proprietà dell'asse X
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartAxe XAxe
        {
            get
            {
                return m_XAxe;
            }
        }

        /// <summary>
        /// Accede alle proprietà dell'asse Y
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartAxe YAxe
        {
            get
            {
                return m_YAxe;
            }
        }

        /// <summary>
        /// Restituisce la collezione delle serie di dati rappresentate dal grafico
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartSeries Series
        {
            get
            {
                if (m_Series is null)
                    m_Series = new CChartSeries(this);
                return m_Series;
            }
        }

        /// <summary>
        /// Restituisce il titolo del grafico
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CChartTitle Title
        {
            get
            {
                return m_Title;
            }
        }

        public CChartLegend Legend
        {
            get
            {
                return m_Legend;
            }
        }

        /// <summary>
        /// Restituisce il renderer predefinito del tipo di grafico
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartRenderer GetDefaultRenderer()
        {
            switch (Type)
            {
                case ChartTypes.VerticalBarsMultibars:
                    {
                        return new VerticalBarChartMultibarsRenderer(this);
                    }

                case ChartTypes.VerticalBarsOverlapped:
                    {
                        return new VerticalBarOverlappedChartRenderer(this);
                    }

                case ChartTypes.VerticalBars:
                    {
                        return new VerticalBarChartRenderer(this);
                    }

                case ChartTypes.Lines:
                    {
                        return new LineChartRenderer(this);
                    }

                case ChartTypes.Pie:
                    {
                        return new PieChartRenderer(this);
                    }

                default:
                    {
                        throw new NotSupportedException("Il tipo di grafico specificato non è supportato");
                        break;
                    }
            }
        }

        public override void GetInnerHTML(System.Web.UI.HtmlTextWriter writer)
        {
            var rect = new System.Drawing.Rectangle(0, 0, DMD.Integers.ValueOf(Width), DMD.Integers.ValueOf(Height));
            var bmp = new System.Drawing.Bitmap(rect.Width, rect.Height);
            var gph = System.Drawing.Graphics.FromImage(bmp);
            PaintTo(gph, rect);
            gph.Dispose();
            string fn = Sistema.FileSystem.GetTempFileName("bmp");
            bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Bmp);
            bmp.Dispose();
            writer.Write("<img src=\"");
            writer.Write(Sistema.ApplicationContext.UnMapPath(fn));
            writer.Write("\" border=\"0\" alt=\"Chart\" style=\"width:");
            writer.Write(Width);
            writer.Write("px;height:");
            writer.Write(Height);
            writer.Write("px;\" />");
        }


        /// <summary>
        /// Disegna il grafico nel rettandolo specificato nell'area grafica
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <remarks></remarks>
        public void PaintTo(System.Drawing.Graphics g, System.Drawing.Rectangle rect)
        {
            GetDefaultRenderer().PaintTo(g, rect);
        }

        public ChartMeasure GetMeasures()
        {
            var ret = Series.GetMeasures();
            if (XAxe.MinValue.HasValue)
                ret.MinX = (float)XAxe.MinValue.Value;
            if (XAxe.MaxValue.HasValue)
                ret.MaxX = (float)XAxe.MaxValue.Value;
            if (YAxe.MinValue.HasValue)
                ret.MinY = (float?)YAxe.MinValue.Value;
            if (YAxe.MaxValue.HasValue)
                ret.MaxY = (float?)YAxe.MaxValue.Value;
            return ret;
        }

        protected override void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Type":
                    {
                        m_Type = (ChartTypes)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Title":
                    {
                        m_Title = (CChartTitle)fieldValue;
                        m_Title.SetChart(this);
                        break;
                    }

                case "Legend":
                    {
                        m_Legend = (CChartLegend)fieldValue;
                        m_Legend.SetChart(this);
                        break;
                    }

                case "XAxe":
                    {
                        m_XAxe = (CChartAxe)fieldValue;
                        m_XAxe.SetChart(this);
                        break;
                    }

                case "YAxe":
                    {
                        m_YAxe = (CChartAxe)fieldValue;
                        m_YAxe.SetChart(this);
                        break;
                    }

                case "Grid":
                    {
                        m_Grid = (CChartGrid)fieldValue;
                        m_Grid.SetChart(this);
                        break;
                    }

                case "Series":
                    {
                        m_Series = (CChartSeries)fieldValue;
                        m_Series.SetChart(this);
                        break;
                    }

                case "Labels":
                    {
                        m_Labels = (CChartLabels)fieldValue;
                        m_Labels.SetChart(this);
                        break;
                    }

                default:
                    {
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                    }
            }
        }

        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Type", (int?)m_Type);
            base.XMLSerialize(writer);
            writer.WriteTag("Title", Title);
            writer.WriteTag("Legend", Legend);
            writer.WriteTag("XAxe", XAxe);
            writer.WriteTag("YAxe", YAxe);
            writer.WriteTag("Grid", Grid);
            writer.WriteTag("Series", Series);
            writer.WriteTag("Labels", Labels);
        }
    }
}