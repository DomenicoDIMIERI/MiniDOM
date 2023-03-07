using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class VerificheAmministrativeClass : CModulesClass<Finanziaria.VerificaAmministrativa>
        {
            internal VerificheAmministrativeClass() : base("modCQSPDVerificheAmministrative", typeof(Finanziaria.VerificheAmministrativeCursor))
            {
            }

            protected override Sistema.CModule CreateModuleInfo()
            {
                var m = base.CreateModuleInfo();
                m.DisplayName = "Verifiche Amministrative";
                m.ClassHandler = "VerificheAmministrativeHandler";
                m.Save();
                return m;
            }
        }
    }

    public partial class Finanziaria
    {
        private static VerificheAmministrativeClass m_VerificheAmministrative = null;

        public static VerificheAmministrativeClass VerificheAmministrative
        {
            get
            {
                if (m_VerificheAmministrative is null)
                    m_VerificheAmministrative = new VerificheAmministrativeClass();
                return m_VerificheAmministrative;
            }
        }
    }
}