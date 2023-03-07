
namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Calcola il valore del capitale finanziato in funzione del TAN
    /// </summary>
    /// <remarks></remarks>
        public class CTANInverter
        {
            private double m_Rata;
            private int m_Durata;
            private double m_TAN;
            private double m_CapitaleFinanziato;
            private bool m_Calculated;

            public CTANInverter(double rata, double durata, double tan)
            {
                m_Rata = rata;
                m_Durata = (int)durata;
                m_TAN = tan;
                m_CapitaleFinanziato = 0d;
                m_Calculated = false;
            }

            public CTANInverter() : this(0d, 120d, 0d)
            {
            }

            public void Invalidate()
            {
                m_Calculated = false;
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

            public double TAN
            {
                get
                {
                    return m_TAN;
                }

                set
                {
                    if (m_TAN == value)
                        return;
                    m_TAN = value;
                    Invalidate();
                }
            }

            public double CapitaleFinanziato
            {
                get
                {
                    if (m_Calculated == false)
                        Calc();
                    return m_CapitaleFinanziato;
                }
            }

            public double Calc()
            {
                if (m_Calculated)
                    return m_CapitaleFinanziato;
                var fEval = new CTANFunEvaluator();
                int errCode = 0;
                // Dim ret As Double
                fEval.Durata = Durata;
                fEval.TAN = TAN;
                fEval.Rata = Rata;
                double ml = Rata * Durata;
                double x = 5d;
                double tan1 = 0d;
                double cf;
                do
                {
                    cf = Maths.FindZero(fEval, ml * x / 10d, ml);
                    var cc = new CTANCalculator();
                    cc.Rata = (decimal)Rata;
                    cc.Durata = Durata;
                    cc.Importo = (decimal)cf;
                    tan1 = cc.Calc();
                    x += 1d;
                }
                while (x < 10d && Maths.Abs(tan1 - TAN) >= 0.001d);
                return Sistema.IIF(x >= 10d, 0d, cf);
            }
        }
    }
}