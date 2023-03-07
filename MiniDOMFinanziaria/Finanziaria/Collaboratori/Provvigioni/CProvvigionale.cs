using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum TipoCalcoloProvvigionale : int
        {
            /// <summary>
        /// Il provvigionale è calcolato solo come un riconoscimento per la pratica
        /// </summary>
        /// <remarks></remarks>
            SOLOBASE = 0,

            /// <summary>
        /// Il provvigionale è calcolato come base più una percentuale sul ML
        /// </summary>
        /// <remarks></remarks>
            BASEPIUPERCML = 1
        }

        [Serializable]
        public class CProvvigionale : Databases.DBObjectBase
        {
            private TipoCalcoloProvvigionale m_Tipo;
            private decimal? m_ValoreBase;
            private decimal? m_ValorePercentuale;

            public CProvvigionale()
            {
                m_Tipo = TipoCalcoloProvvigionale.BASEPIUPERCML;
                m_ValoreBase = default;
                m_ValorePercentuale = default;
            }

            public TipoCalcoloProvvigionale Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    var oldValue = m_Tipo;
                    if (oldValue == value)
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            public decimal? ValoreBase
            {
                get
                {
                    return m_ValoreBase;
                }

                set
                {
                    var oldValue = m_ValoreBase;
                    if (oldValue == value == true)
                        return;
                    m_ValoreBase = value;
                    DoChanged("ValoreBase", value, oldValue);
                }
            }

            public decimal? ValorePercentuale
            {
                get
                {
                    return m_ValorePercentuale;
                }

                set
                {
                    var oldValue = m_ValorePercentuale;
                    if (oldValue == value == true)
                        return;
                    m_ValorePercentuale = value;
                    DoChanged("ValorePercentuale", value, oldValue);
                }
            }

            public decimal? ValoreTotale()
            {
                switch (Tipo)
                {
                    case TipoCalcoloProvvigionale.SOLOBASE:
                        {
                            return m_ValoreBase; // TipoCalcoloProvvigionale.BASEPIUPERCML
                        }

                    default:
                        {
                            if (m_ValoreBase.HasValue && m_ValorePercentuale.HasValue)
                            {
                                return m_ValoreBase.Value + m_ValorePercentuale.Value;
                            }
                            else
                            {
                                return default;
                            }

                            break;
                        }
                }
            }

            public void CalcolaSu(CPraticaCQSPD pratica)
            {
                throw new NotImplementedException();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return null;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "";
            }

            public float? get_PercentualeSu(decimal? ml)
            {
                switch (Tipo)
                {
                    case TipoCalcoloProvvigionale.SOLOBASE:
                        {
                            return 0;
                        }

                    default:
                        {
                            if (this.ValoreTotale().HasValue == false || ml.HasValue == false)
                                return default;
                            return (float?)(this.ValoreTotale().Value * 100m / ml.Value);
                        }
                }
            }

            public void set_PercentualeSu(decimal? ml, float? value)
            {
                switch (Tipo)
                {
                    case TipoCalcoloProvvigionale.SOLOBASE:
                        {
                            m_ValorePercentuale = default;
                            break;
                        }

                    default:
                        {
                            if (value.HasValue)
                            {
                                if (ml.HasValue)
                                {
                                    m_ValorePercentuale = (decimal?)(value.Value * (float)ml.Value / 100f);
                                }
                                else
                                {
                                    throw new ArgumentNullException("ml");
                                }
                            }
                            else
                            {
                                m_ValorePercentuale = default;
                            }

                            break;
                        }
                }
            }
        }
    }
}