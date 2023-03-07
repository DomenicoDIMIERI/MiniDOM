
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Gestione degli stati di lavorazione
    /// </summary>
    /// <remarks></remarks>
        public sealed class CStatiLavorazionePraticaCLass : CModulesClass<CStatoLavorazionePratica>
        {
            internal CStatiLavorazionePraticaCLass() : base("modCQSPDStatiLav", typeof(CStatiLavorazionePraticaCursor))
            {
            }
        }

        private static CStatiLavorazionePraticaCLass m_StatiDiLavorazionePratica = null;

        public static CStatiLavorazionePraticaCLass StatiLavorazionePratica
        {
            get
            {
                if (m_StatiDiLavorazionePratica is null)
                    m_StatiDiLavorazionePratica = new CStatiLavorazionePraticaCLass();
                return m_StatiDiLavorazionePratica;
            }
        }
    }
}