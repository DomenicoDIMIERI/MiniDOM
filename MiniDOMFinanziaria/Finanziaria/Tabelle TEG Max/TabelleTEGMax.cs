
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Class Generica che consente di accedere al modulo Tabelle TEG Max dall'oggetto Finanziaria
    /// </summary>
    /// <remarks></remarks>
        public sealed class CTabelleTEGMaxClass : CModulesClass<CTabellaTEGMax>
        {
            internal CTabelleTEGMaxClass() : base("modCQSPDTblTEGMax", typeof(CTabelleTEGMaxCursor), -1)
            {
            }
        }

        private static CTabelleTEGMaxClass m_TabelleTEGMax = null;

        public static CTabelleTEGMaxClass TabelleTEGMax
        {
            get
            {
                if (m_TabelleTEGMax is null)
                    m_TabelleTEGMax = new CTabelleTEGMaxClass();
                return m_TabelleTEGMax;
            }
        }
    }
}