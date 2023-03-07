using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CManutenzione"/>
        /// </summary>
        [Serializable]
        public class CManutenzioniClass 
            : CModulesClass<Anagrafica.CManutenzione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CManutenzioniClass() 
                : base("modManutenzini", typeof(Anagrafica.CManutenzioniCursor), 0)
            {
            }

            private CVociManutenzioneClass m_Voci = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="VoceManutenzione"/>
            /// </summary>
            public CVociManutenzioneClass Voci
            {
                get
                {
                    if (m_Voci is null)
                        m_Voci = new CVociManutenzioneClass();
                    return m_Voci;
                }
            }
        }
    }

    public partial class Anagrafica
    {
        private static CManutenzioniClass m_Manutenzioni = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CManutenzione"/>
        /// </summary>
        public static CManutenzioniClass Manutenzioni
        {
            get
            {
                if (m_Manutenzioni is null)
                    m_Manutenzioni = new CManutenzioniClass();
                return m_Manutenzioni;
            }
        }
    }
}