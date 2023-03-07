
namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Calcola il TAEG
    /// </summary>
    /// <remarks></remarks>
        public class CTAEGCalculator
        {
            private int m_Durata;
            private double m_Rata;
            private double m_NettoRicavo;
            private double m_TAEG;
            private bool m_Calculated;

            public CTAEGCalculator() : this(120, 0d, 0d)
            {
            }

            public CTAEGCalculator(int durata) : this(durata, 0d, 0d)
            {
            }

            public CTAEGCalculator(int durata, double rata) : this(durata, rata, 0d)
            {
            }

            public CTAEGCalculator(int durata, double rata, double nettoRicavo)
            {
                m_Durata = durata;
                m_Rata = rata;
                m_NettoRicavo = nettoRicavo;
                m_TAEG = 0d;
                m_Calculated = false;
            }

            public void Invalidate()
            {
                m_Calculated = false;
            }

            public int Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    if (m_Durata == value)
                        return;
                    m_Durata = value;
                    Invalidate();
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
                    if (m_Rata == value)
                        return;
                    m_Rata = value;
                    Invalidate();
                }
            }

            public double NettoRicavo
            {
                get
                {
                    return m_NettoRicavo;
                }

                set
                {
                    if (m_NettoRicavo == value)
                        return;
                    m_NettoRicavo = value;
                    Invalidate();
                }
            }

            public double TAEG
            {
                get
                {
                    Calc();
                    return m_TAEG;
                }
            }

            public double Calc()
            {
                if (m_Calculated)
                    return m_TAEG;
                var fEval = new CTAEGFunEvaluator();
                int errCode = 0;
                fEval.Durata = Durata;
                fEval.NettoRicavo = (decimal)NettoRicavo;
                fEval.Quota = (decimal)Rata;
                m_TAEG = Maths.FindZero(fEval, Maths.TOLMIN, 1d, 0.0000000001d) * 100d;
                m_Calculated = true;
                return m_TAEG;
            }
        }
    }
}