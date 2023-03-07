
namespace minidom.Forms
{
    public class CChartLegend : CChartElement
    {
        public CChartLegend()
        {
            BackColor = System.Drawing.Color.White;
            Position = ChartElementPosition.Right;
            BorderSize = 1f;
            BorderColor = System.Drawing.Color.Gray;
        }

        public CChartLegend(CChart owner) : base(owner)
        {
            BackColor = System.Drawing.Color.White;
            Position = ChartElementPosition.Right;
            BorderSize = 1f;
            BorderColor = System.Drawing.Color.Gray;
        }
    }
}