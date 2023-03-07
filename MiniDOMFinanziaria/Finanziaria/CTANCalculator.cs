using minidom;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CTANCalculator
        {
            public int max_iterations = 100;
            private decimal m_Rata;
            private decimal m_Importo; // ML - Interessi
            private int m_Durata;

            public CTANCalculator()
            {
                DMDObject.IncreaseCounter(this);
                m_Rata = 0m;
                m_Durata = 120;
                m_Importo = 0m;
            }

            public CTANCalculator(decimal rata, int durata, decimal importo)
            {
                DMDObject.IncreaseCounter(this);
                m_Rata = rata;
                m_Durata = durata;
                m_Importo = importo;
            }

            public decimal Rata
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

            public decimal Importo
            {
                get
                {
                    return m_Importo;
                }

                set
                {
                    m_Importo = value;
                }
            }

            public double Calc()
            {
                double c1C3, c1C4, c1C5;
                c1C3 = (double)Rata;
                c1C4 = (double)Importo;
                c1C5 = Durata;
                if (c1C3 * c1C4 * c1C5 == 0d)
                {
                    return 0d;
                }
                else
                {
                    return Rate(c1C5, c1C3 * -1, c1C4, 0d, DMD.Booleans.ValueOf(0), 0.1d) * 12d * 100d;
                }
            }

            private double Rate(double nper, double pmt, double pv, double fv, bool tipo, double guess)
            {
                // If (Not isFinite(guess) Or Not isFinite(tipo) Or Not isFinite(fv) Or Not isFinite(pv) Or Not isFinite(pmt) Or Not isFinite(nper)) Then
                // Rate = NaN 
                // End If
                int type2;
                double wanted_precision;
                double current_diff;
                double x, next_x, y, z;
                int iterations_done;
                if (tipo)
                {
                    type2 = 1;
                }
                else
                {
                    type2 = 0;
                }

                wanted_precision = 0.00000001d;
                current_diff = 999999999999999999d; // Number.MAX_VALUE 
                if (guess == 0d)
                {
                    x = 0.1d;
                }
                else
                {
                    x = guess;
                }

                iterations_done = 0;
                while (current_diff > wanted_precision & iterations_done < max_iterations)
                {
                    if (x == 0d)
                    {
                        next_x = x - (pv + pmt * nper + fv) / (pv * nper + pmt * (nper * (nper - 1d) + 2 * type2 * nper) / 2d);
                    }
                    else
                    {
                        y = Maths.Pow(1d + x, nper - 1d);
                        z = y * (1d + x);
                        next_x = x * (1d - (x * pv * z + pmt * (1d + x * type2) * (z - 1d) + x * fv) / (x * x * nper * pv * y - pmt * (z - 1d) + x * pmt * (1d + x * type2) * nper * y));
                    }

                    iterations_done = iterations_done + 1;
                    current_diff = Maths.Abs(next_x - x);
                    x = next_x;
                }

                if (guess == 0d & Maths.Abs(x) < wanted_precision)
                    x = 0d;
                if (current_diff >= wanted_precision)
                {
                    return 0d; // Number.NaN;
                }
                else
                {
                    return x;
                }
            }

            ~CTANCalculator()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}