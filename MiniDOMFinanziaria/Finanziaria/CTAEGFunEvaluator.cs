
namespace minidom
{
    public partial class Finanziaria
    {
        public class CTAEGFunEvaluator : FunEvaluator
        {
            public int Durata;
            public decimal Quota;
            public decimal NettoRicavo;

            public override double EvalFunction(double x)
            {
                int s;
                double ret = 0d;
                s = Durata;
                while (s > 0)
                {
                    ret = ret + Maths.Pow(1 + x, -s / 12d);
                    s -= 1;
                }

                ret = ret * (double)Quota - (double)NettoRicavo;
                return ret;
            }
        }
    }
}