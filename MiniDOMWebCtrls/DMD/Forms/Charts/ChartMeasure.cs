
namespace minidom.Forms
{

    /// <summary>
    /// Informazioni su una serie o su una collezione di serie
    /// </summary>
    /// <remarks></remarks>
    public struct ChartMeasure
    {
        public float MinX;
        public float MaxX;
        public float? MinY;
        public float? MaxY;
        public int MinCount;
        public int MaxCount;

        public ChartMeasure Combine(ChartMeasure value)
        {
            var ret = new ChartMeasure();
            ret.MaxCount = Maths.Max(MaxCount, value.MaxCount);
            ret.MinCount = Maths.Min(MinCount, value.MinCount);
            ret.MaxY = Maths.Max(MaxY, value.MaxY);
            ret.MinY = Maths.Min(MinY, value.MinY);
            ret.MaxX = Maths.Max(MaxX, value.MaxX);
            ret.MinX = Maths.Min(MinX, value.MinX);
            return ret;
        }
    }
}