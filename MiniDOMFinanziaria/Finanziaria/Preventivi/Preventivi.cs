
namespace minidom
{
    public partial class Finanziaria
    {
        public sealed class CPreventiviClass : CModulesClass<CPreventivo>
        {
            internal CPreventiviClass() : base("modPreventivatori", typeof(CPreventivoCursor))
            {
            }
        }

        private static CPreventiviClass m_Preventivi = null;

        public static CPreventiviClass Preventivi
        {
            get
            {
                if (m_Preventivi is null)
                    m_Preventivi = new CPreventiviClass();
                return m_Preventivi;
            }
        }
    }
}