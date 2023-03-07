
namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Gestione degli altri prestiti
    /// </summary>
    /// <remarks></remarks>
        public sealed class CAltriPrestitiClass : Sistema.CModulesClass<CAltroPrestito>
        {
            internal CAltriPrestitiClass() : base("modAltriPrestiti", typeof(CAltriPrestitiCursor), 0)
            {
            }

            public string FormatTipo(TipoEstinzione tipo)
            {
                switch (tipo)
                {
                    case TipoEstinzione.ESTINZIONE_NO:
                        {
                            return "";
                        }

                    case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO:
                        {
                            return "CQS";
                        }

                    case TipoEstinzione.ESTINZIONE_CQP:
                        {
                            return "CQP";
                        }

                    case TipoEstinzione.ESTINZIONE_PRESTITODELEGA:
                        {
                            return "PD";
                        }

                    case TipoEstinzione.ESTINZIONE_PRESTITOPERSONALE:
                        {
                            return "Prestito Personale";
                        }

                    case TipoEstinzione.ESTINZIONE_PIGNORAMENTO:
                        {
                            return "Pignoramento";
                        }

                    case TipoEstinzione.ESTINZIONE_MUTUO:
                        {
                            return "Mutuo";
                        }

                    case TipoEstinzione.ESTINZIONE_PROTESTI:
                        {
                            return "Protesti";
                        }

                    case TipoEstinzione.ESTINZIONE_ASSICURAZIONE:
                        {
                            return "Assicurazione";
                        }

                    case TipoEstinzione.ESTINZIONE_ALIMENTI:
                        {
                            return "Alimenti";
                        }

                    default:
                        {
                            return "invalid";
                        }
                }
            }
        }

        private static CAltriPrestitiClass m_AltriPrestiti = null;

        public static CAltriPrestitiClass AltriPrestiti
        {
            get
            {
                if (m_AltriPrestiti is null)
                    m_AltriPrestiti = new CAltriPrestitiClass();
                return m_AltriPrestiti;
            }
        }
    }
}