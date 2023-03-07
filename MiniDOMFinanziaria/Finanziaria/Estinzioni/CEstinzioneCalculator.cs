using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CEstinzioneCalculator
        {
            private decimal? m_Rata; // [Double]   
            private int m_Durata; // [INT]      Durata in numero di rate
            private double m_TAN; // [Double]    
            private int m_NumeroRateInsolute; // [int] Numero di rate insolute
            private double m_Penale; // [Double] Percentuale da corrispondere come penale per estinzione anticipata
            private int m_NumeroRatePagate;
            private bool m_Calculated;
            private decimal m_AbbuonoInteressi;
            private double m_PenaleEstinzione;
            private decimal m_SpeseAccessorie;

            public CEstinzioneCalculator()
            {
                DMDObject.IncreaseCounter(this);
                m_Rata = 0;
                m_Durata = 0;
                m_TAN = 0d;
                m_NumeroRatePagate = 0;
                m_Calculated = false;
                m_AbbuonoInteressi = 0m;
                m_PenaleEstinzione = 1d;
                m_SpeseAccessorie = 0m;
            }

            protected virtual void DoChanged(string propName, object newValue, object oldValue)
            {
                // Me.Invalidate()
            }

            public void Validate()
            {
                if (m_Calculated == false)
                    Calculate();
            }

            public void Invalidate()
            {
                m_Calculated = false;
            }

            /// <summary>
        /// Restituisce o imposta la percentuale sul capitale da restituire da pagare come penale per l'anticipata estinzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double PenaleEstinzione
            {
                get
                {
                    return m_PenaleEstinzione;
                }

                set
                {
                    if (value < 0d)
                        throw new ArgumentOutOfRangeException("La penale di estinzione non può assumere un valore negativo");
                    double oldValue = m_PenaleEstinzione;
                    if (oldValue == value)
                        return;
                    m_PenaleEstinzione = value;
                    Invalidate();
                    DoChanged("PenaleEstinzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore delle spese accessorie
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SpeseAccessorie
            {
                get
                {
                    return m_SpeseAccessorie;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("Le spese accessorie non possono assumere un valore negativo");
                    decimal oldValue = m_SpeseAccessorie;
                    if (oldValue == value)
                        return;
                    m_SpeseAccessorie = value;
                    Invalidate();
                    DoChanged("SpeseAccessorie", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di quote scadute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroQuoteScadute
            {
                get
                {
                    return m_NumeroRatePagate;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("Il numero di rate scadute non può essere negativo");
                    int oldValue = m_NumeroRatePagate;
                    if (oldValue == value)
                        return;
                    m_NumeroRatePagate = value;
                    Invalidate();
                    DoChanged("NumeroRatePagate", value, oldValue);
                }
            }

            public int NumeroRateResidue
            {
                get
                {
                    return Durata - NumeroQuoteScadute;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("Il numero di rate residue non può essere negativo");
                    int oldValue = NumeroRateResidue;
                    if (oldValue == value)
                        return;
                    m_NumeroRatePagate = Durata - value;
                    Invalidate();
                    DoChanged("NumeroRateResidue", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di rate già maturate ma non  ancora pagate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroRateInsolute
            {
                get
                {
                    return m_NumeroRateInsolute;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("Il numero di rate insolute non può essere negativo");
                    int oldValue = m_NumeroRateInsolute;
                    if (oldValue == value)
                        return;
                    m_NumeroRateInsolute = value;
                    DoChanged("NumeroRateInsolute", value, oldValue);
                }
            }

            public decimal? Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    var oldValue = m_Rata;
                    if (oldValue == value == true)
                        return;
                    m_Rata = value;
                    Invalidate();
                    DoChanged("Rata", value, oldValue);
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
                    int oldValue = m_Durata;
                    if (oldValue == value)
                        return;
                    m_Durata = value;
                    Invalidate();
                    DoChanged("Durata", value, oldValue);
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
                    double oldValue = m_TAN;
                    if (oldValue == value)
                        return;
                    m_TAN = value;
                    Invalidate();
                    DoChanged("TAN", value, oldValue);
                }
            }

            public decimal? DebitoIniziale
            {
                get
                {
                    if (m_Rata.HasValue)
                        return m_Rata.Value * m_Durata;
                    return default;
                }

                set
                {
                    var oldValue = DebitoIniziale;
                    if (oldValue == value == true)
                        return;
                    if (m_Durata <= 0)
                        m_Durata = 1;
                    m_Rata = value / m_Durata;
                    DoChanged("DebitoIniziale", value, oldValue);
                }
            }

            public decimal? DebitoResiduo
            {
                get
                {
                    if (DebitoIniziale.HasValue && m_Rata.HasValue)
                    {
                        return DebitoIniziale.Value - m_Rata.Value * m_NumeroRatePagate;
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    var oldValue = DebitoResiduo;
                    if (oldValue == value == true)
                        return;
                    m_NumeroRatePagate = (int)((DebitoIniziale - value) / m_Rata.Value);
                    DoChanged("DebitoResiduo", value, oldValue);
                }
            }

            public decimal AbbuonoInteressi
            {
                get
                {
                    Validate();
                    return m_AbbuonoInteressi;
                }
            }

            /// <summary>
        /// Restituisce il capitale da rimborsare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal CapitaleDaRimborsare
            {
                get
                {
                    if (DebitoResiduo.HasValue)
                    {
                        return DebitoResiduo.Value - AbbuonoInteressi;
                    }
                    else
                    {
                        return 0m;
                    }
                }
            }

            /// <summary>
        /// Restituisce il valore della penale di estinzione calcolata sul capitale da rimborsare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ValorePenale
            {
                get
                {
                    return (decimal)((double)CapitaleDaRimborsare * m_PenaleEstinzione / 100d);
                }
            }

            /// <summary>
        /// Restituisce il totale da rimborsare:
        /// CapitaleDaRimborsare + ValorePenale + SpeseAccessorie + ValoreQuoteInsolute
        /// </summary>
        /// <returns></returns>
            public decimal TotaleDaRimborsare
            {
                get
                {
                    return CapitaleDaRimborsare + ValorePenale + SpeseAccessorie + ValoreQuoteInsolute;
                }
            }

            public decimal ValoreQuoteInsolute
            {
                get
                {
                    Validate();
                    if (m_Rata.HasValue)
                    {
                        return m_NumeroRateInsolute * m_Rata.Value;
                    }
                    else
                    {
                        return 0m;
                    }
                }
            }

            public bool Calculate()
            {
                // Dim Dk As Decimal   'Debito residuo per k=1..n-1
                decimal Ik; // Interesse in ciascun periodo k=1...n
                int k;
                double i;

                // Me.m_DebitoIniziale = Me.m_Rata.Value * Me.m_Durata
                // Me.m_DebitoResiduo = Me.m_DebitoIniziale - Me.m_Quota * Me.m_NumeroRatePagate

                i = m_TAN / 100d / 12d;
                m_AbbuonoInteressi = 0m;
                var loopTo = m_Durata;
                for (k = m_NumeroRatePagate + 1; k <= loopTo; k++)
                {
                    Ik = (decimal)((double)m_Rata.Value * (1d - 1d / Maths.Pow(1d + i, m_Durata - k + 1)));
                    m_AbbuonoInteressi = m_AbbuonoInteressi + Ik;
                }

                m_Calculated = true;
                return true;
            }

            ~CEstinzioneCalculator()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}