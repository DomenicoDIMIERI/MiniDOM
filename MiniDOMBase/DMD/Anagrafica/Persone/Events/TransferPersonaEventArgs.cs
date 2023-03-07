using System;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Evento relativo al trasferimento di una persona ad un ufficio diverso
        /// </summary>
        [Serializable]
        public class TransferPersonaEventArgs 
            : PersonaEventArgs
        {
            [NonSerialized] private CUfficio m_Ufficio;
            private string m_Messaggio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public TransferPersonaEventArgs()
            {

            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="p"></param>
            /// <param name="ufficio"></param>
            /// <param name="messaggio"></param>
            public TransferPersonaEventArgs(CPersona p, CUfficio ufficio, string messaggio) : base(p)
            {
                m_Ufficio = ufficio;
                m_Messaggio = messaggio;
            }

            /// <summary>
            /// Ufficio di destinazione
            /// </summary>
            public CUfficio Ufficio
            {
                get
                {
                    return m_Ufficio;
                }
            }

            /// <summary>
            /// Messaggio inviato all'ufficio di destinazione
            /// </summary>
            public string Messaggio
            {
                get
                {
                    return m_Messaggio;
                }
            }
        }
    }
}