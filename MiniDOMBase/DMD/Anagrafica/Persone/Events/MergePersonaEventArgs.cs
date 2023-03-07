using System;
using static DMD.Anagrafica;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Evento generato quando due persone vengono unite
        /// </summary>
        [Serializable]
        public class MergePersonaEventArgs 
            : PersonaEventArgs
        {
            private CMergePersona m_Mi;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MergePersonaEventArgs()
            {

            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="mi"></param>
            public MergePersonaEventArgs(CMergePersona mi) : base(mi.Persona1)
            {
                m_Mi = mi;
            }

            /// <summary>
            /// Informazioni sull'unione
            /// </summary>
            public CMergePersona MI
            {
                get
                {
                    return m_Mi;
                }
            }
        }
    }
}