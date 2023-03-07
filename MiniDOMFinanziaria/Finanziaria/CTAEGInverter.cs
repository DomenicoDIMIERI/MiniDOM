
namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
      /// Calcola il valore del netto ricavo in funzione del TAEG
      /// </summary>
      /// <remarks></remarks>
        public class CTAEGInverter
        {
            private decimal m_Rata;
            private int m_Durata;
            private double m_TAEG;

            public CTAEGInverter(decimal rata = 0m, int durata = 120, double taeg = 0d)
            {
                DMDObject.IncreaseCounter(this);
                m_Rata = rata;
                m_Durata = 120;
                m_TAEG = taeg;
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

            public double TAEG
            {
                get
                {
                    return m_TAEG;
                }

                set
                {
                    m_TAEG = value;
                }
            }

            public double Calc()
            {
                int k;
                double s = 0d;
                var loopTo = Durata;
                for (k = 1; k <= loopTo; k++)
                    s = s + Maths.Pow(1d + TAEG / 100d, -k / 12d);
                return (double)Rata * s;
            }
        }
    }
}