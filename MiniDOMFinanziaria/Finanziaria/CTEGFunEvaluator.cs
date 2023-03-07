
namespace minidom
{
    public partial class Finanziaria
    {
        public class CTEGFunEvaluator : FunEvaluator
        {
            public int Durata;
            public decimal Quota;
            public decimal NettoRicavo;
            public decimal SpeseAssicurative;
            public decimal Imposte;

            public override double EvalFunction(double x)
            {
                int s;
                double ret = 0d;
                var loopTo = Durata;
                for (s = 1; s <= loopTo; s += 1)
                    ret = ret + Maths.Pow(1 + x, (double)-s / 12);
                ret = ret * (double)Quota - (double)(NettoRicavo + SpeseAssicurative + Imposte);
                return ret;
            }
        }
    }
}