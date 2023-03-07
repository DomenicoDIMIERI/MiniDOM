
namespace minidom.Forms
{
    public class CChartTitle : CChartElement
    {
        public CChartTitle()
        {
            BackColor = System.Drawing.Color.White;
            Position = ChartElementPosition.Top;
        }

        public CChartTitle(CChart owner) : base(owner)
        {
            BackColor = System.Drawing.Color.White;
            Position = ChartElementPosition.Top;
        }
    }
}