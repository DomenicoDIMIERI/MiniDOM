
namespace minidom
{
    public partial class Finanziaria
    {
        public sealed class CRichiesteAssegniClass : CModulesClass<CRichiestaAssegni>
        {
            internal CRichiesteAssegniClass() : base("RichiesteAssegni", typeof(CRichiestaAssegniCursor))
            {
            }
        }

        private static CRichiesteAssegniClass m_RichiestaAssegni = null;

        public static CRichiesteAssegniClass RichiesteAssegni
        {
            get
            {
                if (m_RichiestaAssegni is null)
                    m_RichiestaAssegni = new CRichiesteAssegniClass();
                return m_RichiestaAssegni;
            }
        }
    }
}