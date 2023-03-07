using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="RegolaTaskLavorazione"/>
        /// </summary>
        [Serializable]
        public sealed class CRegoleTasksDiLavorazioneClass 
            : CModulesClass<Anagrafica.RegolaTaskLavorazione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegoleTasksDiLavorazioneClass() 
                : base("modAnaRegoleTaskLavorazione", typeof(Anagrafica.RegolaTaskLavorazioneCursor), -1)
            {
            }
        }
    }

    public partial class Anagrafica
    {
        private static CRegoleTasksDiLavorazioneClass m_RegoleTasksLavorazione = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="RegolaTaskLavorazione"/>
        /// </summary>
        public static CRegoleTasksDiLavorazioneClass RegoleTasksLavorazione
        {
            get
            {
                if (m_RegoleTasksLavorazione is null)
                    m_RegoleTasksLavorazione = new CRegoleTasksDiLavorazioneClass();
                return m_RegoleTasksLavorazione;
            }
        }
    }
}