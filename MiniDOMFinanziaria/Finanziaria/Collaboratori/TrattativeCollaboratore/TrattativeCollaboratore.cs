using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public sealed class CTrattativeCollaboratoreClass : CModulesClass<Finanziaria.CTrattativaCollaboratore>
        {
            private static readonly Finanziaria.StatoTrattativa[] values = new[] { Finanziaria.StatoTrattativa.STATO_NONPROPOSTO, Finanziaria.StatoTrattativa.STATO_PROPOSTA, Finanziaria.StatoTrattativa.STATO_ATTESAAPPROVAZIONE, Finanziaria.StatoTrattativa.STATO_APPROVATO, Finanziaria.StatoTrattativa.STATO_NONAPPROVATO, Finanziaria.StatoTrattativa.STATO_ACCETTATO, Finanziaria.StatoTrattativa.STATO_NONACCETTATO };
            private static readonly string[] names = new[] { "Non proposto", "proposto", "attesa approvazione", "approvato", "non approvato", "accettato", "non accettato" };

            internal CTrattativeCollaboratoreClass() : base("modTrattativeCollaboratore", typeof(Finanziaria.CTrattativeCollaboratoreCursor))
            {
            }

            public string FormatStato(Finanziaria.StatoTrattativa value)
            {
                int i = DMD.Arrays.IndexOf(CTrattativeCollaboratoreClass.values, 0, DMD.Arrays.Len(values), value);
                return names[i];
            }

            public Finanziaria.StatoTrattativa ParseStato(string value)
            {
                value = DMD.Strings.Trim(value);
                int i = DMD.Arrays.IndexOf(CTrattativeCollaboratoreClass.names, 0, DMD.Arrays.Len(names), value);
                return values[i];
            }
        }
    }

    public partial class Finanziaria
    {
        private static CTrattativeCollaboratoreClass m_TrattativeCollaboratore = null;

        public static CTrattativeCollaboratoreClass TrattativeCollaboratore
        {
            get
            {
                if (m_TrattativeCollaboratore is null)
                    m_TrattativeCollaboratore = new CTrattativeCollaboratoreClass();
                return m_TrattativeCollaboratore;
            }
        }
    }
}