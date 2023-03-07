
namespace minidom
{
    public partial class Finanziaria
    {
        public class CTANFunEvaluator : FunEvaluator
        {
            private int m_Durata;
            private double m_Rata;
            private double m_TAN;

            public CTANFunEvaluator()
            {
                m_Durata = 120;
                m_Rata = 0.0d;
                m_TAN = 0.0d;
            }

            public int Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    m_Durata = value;
                }
            }

            public double Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    m_Rata = value;
                }
            }

            public double TAN
            {
                get
                {
                    return m_TAN;
                }

                set
                {
                    m_TAN = value;
                }
            }

            public override double EvalFunction(double x)
            {
                var c = new CTANCalculator();
                c.Rata = (decimal)m_Rata;
                c.Durata = m_Durata;
                c.Importo = (decimal)x;
                return c.Calc() - m_TAN;
            }
        }
    }
}